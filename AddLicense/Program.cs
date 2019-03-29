using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PrependSources
{
    class Program
    {
        static void ShowError(string error)
        {
            Console.WriteLine("ERROR." + error);
            Console.WriteLine(@"Usage example: AddLicense license=license.txt input-dir=..\..\myproject input-dir=..\..\myproject2 ext=cpp ext=cs list-only");
        }
        /// <summary>
        /// Usage:
        /// PrependSources license=license-file input-dir=dir1 [input-dir=dir2...]
        ///                                     ext=extension1 [ext=extension2...]
        ///                                     [list-only]
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            const string listOnlyArg = "list-only";
            const string licenseArgPrefix = "license=";
            const string dirArgPrefix = "dir=";
            const string extensionArgPrefix = "ext=";
            //default values for arguments
            string licenseFile = "..\\..\\license.txt";
            List<string> directories = new List<string>();
            List<string> extensions = new List<string>();
            bool listOnly = false;
            //parse arguments
            foreach(string argument in args)
            {
                if (argument.StartsWith(licenseArgPrefix)) licenseFile = argument.Substring(licenseArgPrefix.Length);
                else if (argument.StartsWith(dirArgPrefix)) directories.Add(argument.Substring(dirArgPrefix.Length));
                else if (argument.StartsWith(extensionArgPrefix)) extensions.Add(argument.Substring(extensionArgPrefix.Length));
                else if (argument == listOnlyArg) listOnly = true;
            }

            //check arguments
            if (!File.Exists(licenseFile))
            {
                ShowError("Cannot find license file");
                return;
            }
            if (directories.Count == 0)
            {
                ShowError("No input directory given");
                return;
            }
            foreach(string dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    ShowError("Could not found directory:" + dir);
                    return;
                }
            }
            if (extensions.Count == 0)
            {
                ShowError("No source code extensions were defined");
                return;
            }


            //read file with the license to be prepended to source files
            string license = File.ReadAllText(licenseFile);

            List<string> sourceFiles = new List<string>();

            //get all source files
            foreach (string dir in directories)
            {
                foreach (string extension in extensions)
                {
                    sourceFiles.AddRange(Directory.EnumerateFiles(dir, "*." + extension, SearchOption.AllDirectories));
                }
            }

            //prepend and output results
            if (!listOnly)
                Console.WriteLine("License has been added to source files:");
            else
                Console.WriteLine("With current parameters license would be added to source files:");

            string sourceFileText;
            foreach (string sourceFile in sourceFiles)
            {
                sourceFileText= File.ReadAllText(sourceFile);
                if (!sourceFileText.StartsWith(license))
                {
                    if (!listOnly)
                    {
                        File.WriteAllText(sourceFile, license + sourceFileText);
                        Console.WriteLine("License added to source file: " + sourceFile);
                    }
                    else
                    {
                        Console.WriteLine(sourceFile);
                    }
                }
            }
        }
    }
}
