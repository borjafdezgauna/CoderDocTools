using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Documenter
{
    public class CppSourceParser: SourceParser
    {
        //PRIVATE stuff///////////////////////////////////////////////////
        string m_currentFile = "";

        public static void GetEnclosedBody(string content, int startIndex, string openChar, string closeChar
            , out string definition, out string prefix)
        {
            int contentLength = content.Length;
            int numOpenChars = 0;
            int firstOpenPos = content.IndexOf(openChar, startIndex);
            prefix = content.Substring(startIndex, firstOpenPos - startIndex);
            int pos = firstOpenPos + 1;
            string c = content.Substring(pos, 1);
            while (pos < contentLength && (c != closeChar || numOpenChars != 0))
            {
                if (c == openChar)
                    numOpenChars++;
                if (c == closeChar)
                    numOpenChars--;
                pos++;
                c = content.Substring(pos, 1);
            }
            definition = content.Substring(firstOpenPos + 1, pos - firstOpenPos - 1);
        }


        List<ObjectClass> ParsedObjectClasses = new List<ObjectClass>();

        void ParseAllMethodsAndComments(string filename, string content)
        {
            //We only process comments starting with ///
            string methodRegEx = @"(///[^\r\n]+[\r\n]+)+(\w+)\s+(\w+)::(\w+)\(([^\)]*)\)";
            string constructorRegEx = @"(///[^\r\n]+[\r\n]+)+(\w+)::\2\s*\(([^\)]*)\)";
            string className, methodName, returnType, arguments;
            CaptureCollection comments;
            //regular methods
            foreach (Match match in Regex.Matches(content, methodRegEx))
            {
                comments = match.Groups[1].Captures;
                returnType = TextUtils.RemoveLinebreaksAndTrim(match.Groups[2].Value);
                className = TextUtils.RemoveLinebreaksAndTrim(match.Groups[3].Value);
                methodName = TextUtils.RemoveLinebreaksAndTrim(match.Groups[4].Value);
                arguments = TextUtils.RemoveLinebreaksAndTrim(match.Groups[5].Value);

                ObjectClass objClass = ParsedObjectClasses.Find(c => c.Name == className);
                if (objClass == null)
                {
                    objClass = new ObjectClass(filename, className);
                    ParsedObjectClasses.Add(objClass);
                }
                objClass.AddMethod(new ClassMethod(methodName, comments, arguments, returnType, ClassMethod.MethodType.Regular));
            }
            //constructors
            foreach (Match match in Regex.Matches(content, constructorRegEx))
            {
                comments = match.Groups[1].Captures;
                className = TextUtils.RemoveLinebreaksAndTrim(match.Groups[2].Value);
                methodName = TextUtils.RemoveLinebreaksAndTrim(match.Groups[2].Value);
                arguments = TextUtils.RemoveLinebreaksAndTrim(match.Groups[3].Value);

                ObjectClass objClass = ParsedObjectClasses.Find(c => c.Name == className);
                if (objClass == null)
                {
                    objClass = new ObjectClass(filename, className);
                    ParsedObjectClasses.Add(objClass);
                }
                objClass.AddMethod(new ClassMethod(methodName, comments, arguments, null, ClassMethod.MethodType.Constructor));
            }
        }
        //PUBLIC methods//////////////////////////////////////////////////
        public int numCharsProcessed = 0;
        public CppSourceParser() { }
        public void ParseSrcFile(string filename)
        {
            //Console.WriteLine("Parsing source file " + filename);
            m_currentFile = filename;
            string originalFileContents = File.ReadAllText(filename, Encoding.UTF8);
            string processedFileContents = originalFileContents.Replace('\r', ' ');
            processedFileContents = processedFileContents.Replace('\n', ' ');

            numCharsProcessed += originalFileContents.Length;

            ParseAllMethodsAndComments(Path.GetFileName(filename), originalFileContents);
        }

        public override void ParseSourceFilesInDir(string inputDir)
        {
            //Parse .cpp files for constructor and factory definition
            List<string> sourceFiles = new List<string>(Directory.EnumerateFiles(inputDir, "*.cpp", SearchOption.AllDirectories));
            foreach (var file in sourceFiles)
            {
                Console.WriteLine("Parsing source file: " + file);
                ParseSrcFile(file);
            }
        }

        public override List<ObjectClass> GetObjectClasses()
        {
            return ParsedObjectClasses;
        }
        public override int GetNumBytesProcessed()
        {
            return numCharsProcessed;
        }
    }
}
