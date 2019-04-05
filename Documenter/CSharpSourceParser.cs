using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Documenter
{
    public class CSharpSourceParser: SourceParser
    {

        void ParseClass(string filename, string namespaceName, string className, string classDefinition)
        {
            //We only process comments starting with ///
            //and public methods
            string methodRegEx = @"(\s*///[^\r\n]+[\r\n]+)+\s*(?:public\s+|protected\s+|async\s+|static\s+)+?(\w+)\s+(\w+)\(([^\)]*)\)";
            string constructorRegEx = @"(\s*///[^\r\n]+[\r\n]+)+\s*(?:public|protected)\s+" + className + @"\s*\(([^\)]*)\)";
            string methodName, returnType, arguments;
            CaptureCollection comments = null;
            //parse regular methods
            foreach (Match match in Regex.Matches(classDefinition, methodRegEx))
            {
                comments = match.Groups[1].Captures;
                returnType = match.Groups[2].Value;
                methodName = TextUtils.RemoveLinebreaksAndTrim(match.Groups[3].Value);
                arguments = TextUtils.RemoveLinebreaksAndTrim(match.Groups[4].Value);

                ObjectClass objClass = ParsedObjectClasses.Find(c => c.Name == className);
                if (objClass == null)
                {
                    objClass = new ObjectClass(filename, className, namespaceName);
                    ParsedObjectClasses.Add(objClass);
                }
                objClass.AddMethod(new ClassMethod(methodName, comments, arguments, returnType, ClassMethod.MethodType.Regular));
            }
            methodName = null;
            comments = null;
            arguments = null;
            //parse constructors
            foreach (Match match in Regex.Matches(classDefinition, constructorRegEx))
            {
                comments = match.Groups[1].Captures;
                methodName = className;
                arguments = TextUtils.RemoveLinebreaksAndTrim(match.Groups[2].Value);

                ObjectClass objClass = ParsedObjectClasses.Find(c => c.Name == className);
                if (objClass == null)
                {
                    objClass = new ObjectClass(filename, className, namespaceName);
                    ParsedObjectClasses.Add(objClass);
                }
                objClass.AddMethod(new ClassMethod(methodName, comments, arguments, null, ClassMethod.MethodType.Constructor));
            }
        }
        void ParseNamespace(string filename, string namespaceName, string namespaceContent)
        {
            //Extract every namespace
            string extractNamespaceExpReg = @"(?:public)?\s+(?:static\s+)?class\s+(\w+)\s*(?:\:\s*[\w\.]+(?:\s*,\s*\w+)?)?[\r\n\s]*\{((?:[^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!)))\}";
            foreach (Match match in Regex.Matches(namespaceContent, extractNamespaceExpReg))
            {
                string className = match.Groups[1].Value;
                //if the class inherits remove that part
                int pos = className.IndexOf(':');
                if (pos > 0)
                    className = className.Substring(0, pos);
                string classContent = match.Groups[2].Value;

                ParseClass(filename, namespaceName, className, classContent);
            }
        }
        void ParseSrcCode(string filename, string content)
        {
            //Extract every namespace
            string extractNamespaceExpReg = @"namespace\s+([\w\.]+)\s*\r*\n*\{((?:[^{}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!)))\}";
            foreach (Match match in Regex.Matches(content, extractNamespaceExpReg))
            {
                string namespaceName = match.Groups[1].Value;
                string namespaceContent = match.Groups[2].Value;

                ParseNamespace(filename, namespaceName, namespaceContent);
            }
        }
        void ParseSrcFile(string filename)
        {
            string originalFileContents = File.ReadAllText(filename, Encoding.UTF8);

            numCharsProcessed += originalFileContents.Length;

            ParseSrcCode(Path.GetFileName(filename), originalFileContents);
        }
        public override void ParseSourceFilesInDir(string inputDir)
        {
            //Parse .cpp files for constructor and factory definition
            List<string> sourceFiles = new List<string>(Directory.EnumerateFiles(inputDir, "*.cs"
                , SearchOption.AllDirectories));
            List<string> ignorePatterns = new List<string> { ".xaml.cs", "\\obj\\" };
            foreach (string ignorePattern in ignorePatterns)
                sourceFiles.RemoveAll(file => file.Contains(ignorePattern));
            foreach (var file in sourceFiles)
            {
                Console.WriteLine("Parsing source file: " + file);
                ParseSrcFile(file);
            }
        }

        List<ObjectClass> ParsedObjectClasses= new List<ObjectClass>();
        public override List<ObjectClass> GetObjectClasses()
        {
            return ParsedObjectClasses;
        }
        int numCharsProcessed = 0;
        public override int GetNumBytesProcessed()
        {
            return numCharsProcessed;
        }
    }
}
