using System;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Diagnostics;

namespace Portable_Badger
{
    class Program
    {
        public const string ProjectName = "MarkdownToPDF";
        public static string inBaseRelPath = @"../../../" + ProjectName + "/";
        public static string outBaseFolder;
        public static void Main()
        {
            List<string> files = new List<string>();
            string version;

            version = GetVersion(inBaseRelPath + "bin/Release/" + ProjectName + ".exe");
            outBaseFolder = ProjectName + "-" + version + @"/";

            files.Add(inBaseRelPath + "bin/Release/" + ProjectName + ".exe");

            List<string> dependencyList = new List<string>();
            GetDependencies(inBaseRelPath + "bin/Release/", ProjectName + ".exe", ref dependencyList);
            files.AddRange(dependencyList);

            string outputFile = inBaseRelPath + ProjectName + "-" + version + ".zip";

            Console.WriteLine("Compressing files");
            Compress(outputFile, files);
            Console.WriteLine("Finished");
        }

        public static string GetVersion(string file)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(file);

            return fvi.FileVersion;
        }

        public static void GetDependencies(string inFolder, string module, ref List<string> dependencyList, bool bRecursive = true)
        {
            string depName, modName;

            Assembly assembly = Assembly.LoadFrom(inFolder + module);

            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                modName = assemblyName.Name + ".dll";
                depName = inFolder + modName;
                if (System.IO.File.Exists(depName) && !dependencyList.Exists(name => name == depName))
                {
                    dependencyList.Add(depName);
                    if (bRecursive)
                        GetDependencies(inFolder, modName, ref dependencyList, false);
                }
            }
        }

        public static List<string> GetFilesInFolder(string folder, bool bRecursive, string filter = "*.*")
        {
            List<string> files = new List<string>();

            if (bRecursive)
                files.AddRange(Directory.EnumerateFiles(folder, filter, SearchOption.AllDirectories));
            else
                files.AddRange(Directory.EnumerateFiles(folder));
            return files;
        }

        public static void Compress(string outputFilename, List<string> files)
        {
            uint numFilesAdded = 0;
            double totalNumFiles = (double)files.Count;
            using (FileStream zipToOpen = new FileStream(outputFilename, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (string file in files)
                    {
                        if (System.IO.File.Exists(file))
                        {
                            archive.CreateEntryFromFile(file, outBaseFolder + file.Substring(inBaseRelPath.Length));
                            numFilesAdded++;
                        }
                        else Console.WriteLine("Couldn't find file: {0}", file);

                        Console.Write("\rProgress: {0:F2}%", 100.0 * ((double)numFilesAdded) / totalNumFiles);
                    }
                    Console.WriteLine("\nSaving {0} files in  {1}", numFilesAdded, Path.GetFullPath(outputFilename));
                }
            }
        }
    }
}
