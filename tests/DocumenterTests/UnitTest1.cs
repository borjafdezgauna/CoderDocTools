using System;
using Xunit;
using Documenter;
using System.Collections.Generic;
using System.IO;

namespace DocumenterTests
{
    public class UnitTest1
    {
        void CreateFileStructure(string folder, Dictionary<string, string> files)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            if (!folder.EndsWith('/') && !folder.EndsWith('\\'))
                folder += '\\';

            foreach(string file in files.Keys)
            {
                using (StreamWriter writer = File.CreateText(folder + file))
                {
                    writer.Write(files[file]);
                }
            }
        }

        void DeleteFileStructure(string folder)
        {
            Directory.Delete(folder, true);
        }

 
        [Fact]
        public void CSharpFilesParsingTest()
        {
            string folder = "tmp";
            Dictionary<string, string> files = new Dictionary<string, string>
            {
                ["Class1File.cs"] =
                    "using System.IO;\nnamespace MyProject\n{\npublic class Class1\n{\r\n"
                    + "/// <summary>\n/// Constructor of Class1</summary>\n"
                    + "/// <param name=\"name\">The object's name</param>\n"
                    + "public Class1(string \n"
                    + "name){m_name= name;}\n"
                    + "/// <summary>\n/// Description of the method Foo()</summary>\n"
                    + "/// <param name=\"input\">Input argument</param>\n"
                    + "/// <returns>Return code</returns>\n"
                    + "public static void Foo(int input){return 1;}}\n}",
                ["Class2File.cs"] =
                    "using System.IO;\nnamespace MyProject\n{\npublic class Class2\n{\npublic Class2(){}\n\r"
                    + "/// <summary>\n/// Method Class2.Foo(int)\n/// </summary>\n"
                    + "/// <param name=\"input\">Just a number</param>\n"
                    + "/// <returns>Return code</returns>\n"
                    + "public int Foo(int input){return 2;}\n"
                    + "/// <summary>\n/// Method Class2.Foo2(int)\n/// </summary>\n"
                    + "/// <param name=\"input2\">Just a number</param>\n"
                    + "public async static void Foo2(int input2){ // Do nothing, return nothing}\n"
                    + "}\n}"
            };

            CSharpSourceParser parser = new CSharpSourceParser();

            CreateFileStructure(folder, files);

            parser.ParseSourceFilesInDir(folder);

            List<ObjectClass> parsedClasses = parser.GetObjectClasses();

            Assert.Equal(2, parsedClasses.Count);

            ObjectClass objClass = parsedClasses[0];
            Assert.Single(objClass.Methods);
            ClassMethod method = objClass.Methods[0];
            Assert.Equal("Class1", objClass.Name);
            Assert.Equal("Foo", method.Name);
            Assert.Equal("Description of the method Foo()", method.MethodSummary);
            Assert.Equal("void", method.ReturnType);
            Assert.Equal("int input", method.Arguments);
            Assert.Equal("Input argument", method.ArgumentDescriptions["input"]);
            Assert.Equal("Return code", method.ReturnValueDescription);

            Assert.Single(objClass.Constructors);
            method = objClass.Constructors[0];
            Assert.Equal("Class1", objClass.Name);
            Assert.Equal(objClass.Name, method.Name);
            Assert.Equal("Constructor of Class1", method.MethodSummary);
            Assert.Null(method.ReturnType);
            Assert.Equal("string name", method.Arguments);
            Assert.Equal("The object's name", method.ArgumentDescriptions["name"]);
            Assert.Null(method.ReturnValueDescription);

            objClass = parsedClasses[1];
            method = objClass.Methods[0];
            Assert.Equal("Class2", objClass.Name);
            Assert.Equal(2, objClass.Methods.Count);

            Assert.Equal("Foo", method.Name);
            Assert.Equal("Method Class2.Foo(int)", method.MethodSummary);
            Assert.Equal("int", method.ReturnType);
            Assert.Equal("int input", method.Arguments);
            Assert.Equal("Just a number", method.ArgumentDescriptions["input"]);
            Assert.Equal("Return code", method.ReturnValueDescription);

            method = objClass.Methods[1];
            Assert.Equal("Foo2", method.Name);
            Assert.Equal("Method Class2.Foo2(int)", method.MethodSummary);
            Assert.Equal("void", method.ReturnType);
            Assert.Equal("int input2", method.Arguments);
            Assert.Equal("Just a number", method.ArgumentDescriptions["input2"]);
            Assert.Null(method.ReturnValueDescription);

            DeleteFileStructure(folder);
        }

        [Fact]
        public void CppFilesParsingTest()
        {
            string folder = "tmp";
            Dictionary<string, string> files = new Dictionary<string, string>
            {
                ["class1.cpp"] =
                    "#include \"class1.h\";\n"
                    + "/// <summary>\n/// Constructor of Class1</summary>"
                    + "/// <param name=\"name\">The object's name</param>\n"
                    + "Class1::Class1(string name){m_name= name;}\n"
                    + "/// <summary>\n/// Description of the method Foo()</summary>"
                    + "/// <param name=\"input\">Input argument</param>"
                    + "/// <returns>Return code</returns>\n"
                    + "void Class1::Foo(int input){return 1;}}\n}",
                ["class2.cpp"] =
                    "#include <stdlib>;\n#include <stdio.h>\n"
                    + "/// <summary>\n/// Method Class2.Foo(int)\n/// </summary>\n"
                    + "/// <param name=\"input\">Just a number</param>"
                    + "/// <returns>Return code</returns>\n"
                    + "int Class2::Foo(int input){return 2;}\n"
                    + "/// <summary>\n/// Method Class2.Foo2(int)\n/// </summary>\n"
                    + "/// <param name=\"input2\">Just a number</param>\n"
                    + "void Class2::Foo2(int input2){ // Do nothing, return nothing}\n"
                    + "}\n}"
            };

            CppSourceParser parser = new CppSourceParser();

            CreateFileStructure(folder, files);

            parser.ParseSourceFilesInDir(folder);

            List<ObjectClass> parsedClasses = parser.GetObjectClasses();

            Assert.Equal(2, parsedClasses.Count);

            ObjectClass objClass = parsedClasses[0];
            Assert.Single(objClass.Methods);
            ClassMethod method = objClass.Methods[0];
            Assert.Equal("Class1", objClass.Name);
            Assert.Equal("Foo", method.Name);
            Assert.Equal("Description of the method Foo()", method.MethodSummary);
            Assert.Equal("void", method.ReturnType);
            Assert.Equal("int input", method.Arguments);
            Assert.Equal("Input argument", method.ArgumentDescriptions["input"]);
            Assert.Equal("Return code", method.ReturnValueDescription);

            Assert.Single(objClass.Constructors);
            method = objClass.Constructors[0];
            Assert.Equal("Class1", objClass.Name);
            Assert.Equal(objClass.Name, method.Name);
            Assert.Equal("Constructor of Class1", method.MethodSummary);
            Assert.Null(method.ReturnType);
            Assert.Equal("string name", method.Arguments);
            Assert.Equal("The object's name", method.ArgumentDescriptions["name"]);
            Assert.Null(method.ReturnValueDescription);

            objClass = parsedClasses[1];
            method = objClass.Methods[0];
            Assert.Equal("Class2", objClass.Name);
            Assert.Equal(2, objClass.Methods.Count);

            Assert.Equal("Foo", method.Name);
            Assert.Equal("Method Class2.Foo(int)", method.MethodSummary);
            Assert.Equal("int", method.ReturnType);
            Assert.Equal("int input", method.Arguments);
            Assert.Equal("Just a number", method.ArgumentDescriptions["input"]);
            Assert.Equal("Return code", method.ReturnValueDescription);

            method = objClass.Methods[1];
            Assert.Equal("Foo2", method.Name);
            Assert.Equal("Method Class2.Foo2(int)", method.MethodSummary);
            Assert.Equal("void", method.ReturnType);
            Assert.Equal("int input2", method.Arguments);
            Assert.Equal("Just a number", method.ArgumentDescriptions["input2"]);
            Assert.Null(method.ReturnValueDescription);

            DeleteFileStructure(folder);
        }
    }
}
