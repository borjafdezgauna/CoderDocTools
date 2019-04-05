using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documenter
{
    public class DocumentationExporter
    {
        static void ExportMethod(StreamWriter writer, ClassMethod method, FileFormatter exporter)
        {
            exporter.Title3(writer, method.ReturnType + " " + method.Name + "(" + method.Arguments + ")");

            exporter.OpenList(writer);
            if (method.MethodSummary != null)
            {
                exporter.ListItem(writer, exporter.InlineBold("Summary"));
                exporter.PlainText(writer, method.MethodSummary);
            }
            //export inputs and their descriptions
            if (method.ArgumentDescriptions.Keys.Count > 0)
            {
                exporter.ListItem(writer, exporter.InlineBold("Parameters"));
                exporter.OpenList(writer);
                foreach (string argument in method.ArgumentDescriptions.Keys)
                {
                    exporter.Tab(writer, exporter.InlineListItem(
                        exporter.InlineItalic(argument) + ": " + method.ArgumentDescriptions[argument]));
                }
                exporter.CloseList(writer);
            }
            //export output
            if (method.ReturnValueDescription != null)
            {
                exporter.ListItem(writer, exporter.InlineBold("Return Value"));
                exporter.PlainText(writer, method.ReturnValueDescription);
            }
            exporter.CloseList(writer);
        }
        public static void ExportDocumentation(string outputDir, FileFormatter exporter, List<ObjectClass> classes)
        {
            if (!outputDir.EndsWith("\\") && !outputDir.EndsWith("/"))
                outputDir += "/";

            string extension = exporter.FormatExtension();

            string outputIndexFile = outputDir + "Home" + extension;

            classes = classes.OrderBy(x => x.Name).ToList();

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            using (StreamWriter indexWriter = File.CreateText(outputIndexFile))
            {
                exporter.OpeningSection(indexWriter);
                exporter.Title2(indexWriter, "API Reference");
                exporter.OpenList(indexWriter);
                foreach (ObjectClass objClass in classes)
                {
                    exporter.ListItem(indexWriter
                        , exporter.InlineLink(objClass.Name, objClass.Name));
                    string outputMdFile = outputDir + objClass.Name + extension;
                    using (StreamWriter classWriter = File.CreateText(outputMdFile))
                    {
                        exporter.OpeningSection(classWriter);
                        //exporter.Title1(classWriter, "Class " + objClass.Name);
                        exporter.Comment(classWriter, "Source: " + objClass.SrcFileName);

                        if (objClass.Constructors.Count > 0)
                        {
                            exporter.Title1(classWriter, "Constructors");
                            foreach (ClassMethod method in objClass.Constructors)
                                ExportMethod(classWriter, method, exporter);
                        }
                        if (objClass.Destructors.Count > 0)
                        {
                            exporter.Title1(classWriter, "Destructors");
                            foreach (ClassMethod method in objClass.Destructors)
                                ExportMethod(classWriter, method, exporter);
                        }
                        if (objClass.Methods.Count > 0)
                        {
                            exporter.Title1(classWriter, "Methods");
                            foreach (ClassMethod method in objClass.Methods)
                                ExportMethod(classWriter, method, exporter);
                        }
                        exporter.ClosingSection(classWriter);
                    }
                }
                exporter.CloseList(indexWriter);
                exporter.ClosingSection(indexWriter);
            }
        }
    }
}
