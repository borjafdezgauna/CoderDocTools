using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
//using ExpertPdf.HtmlToPdf.PdfDocument;
//using OpenHtmlToPdf;
//using PdfSharp.Pdf;
//using IronPdf;
//using TheArtOfDev.HtmlRenderer.PdfSharp;
//using iTextSharp;
//using iTextSharp.text;
//using iTextSharp.text;
using PdfSharp.Pdf;

namespace GitHubWikiToPDF
{
    class Program
    {
        const string userNameArg = "-user=";
        static string userName = null;
        const string projectNameArg = "-project=";
        static string projectName = null;
        const string inputFileArg = "-input-file=";
        static string inputFile= null;
        const string outputFileArg = "-output-file=";
        static string outputFile = null;
        const string cssArg = "-css=";
        static string cssFile = null;
        static string tempFolder;
        static string mergedHtmlFilename;
        static string markDownInputFolder;

        enum ExecutionMode { Undefined, GitHubWikiToPDF, LocalMarkdownFileToPDF};
        static ExecutionMode executionMode= ExecutionMode.Undefined;

        static bool ParseArguments(string [] args)
        {
            foreach(string arg in args)
            {
                if (arg.StartsWith(projectNameArg)) projectName = arg.Substring(projectNameArg.Length);
                if (arg.StartsWith(userNameArg)) userName = arg.Substring(userNameArg.Length);
                if (arg.StartsWith(cssArg)) cssFile = arg.Substring(cssArg.Length);
                if (arg.StartsWith(inputFileArg)) inputFile = arg.Substring(inputFileArg.Length);
                if (arg.StartsWith(outputFileArg)) outputFile = arg.Substring(outputFileArg.Length);
            }

            if (projectName != null && userName != null && outputFile != null)
            {
                tempFolder = projectName;
                markDownInputFolder = tempFolder;
                mergedHtmlFilename = tempFolder + "/" + projectName + ".html";
                inputFile = "Home.md";
                executionMode = ExecutionMode.GitHubWikiToPDF;
                return true; //GitHub wiki -> PDF mode
            }
            if (inputFile != null && outputFile != null)
            {
                tempFolder = "tmp";
                string inputDocName = Path.GetFileNameWithoutExtension(inputFile);
                markDownInputFolder = Path.GetDirectoryName(inputFile);
                inputFile = Path.GetFileName(inputFile) ;
                mergedHtmlFilename = tempFolder + "/" + inputDocName + ".html";
                executionMode = ExecutionMode.LocalMarkdownFileToPDF;
                return true; //Local Markdown file -> PDF mode
            }
            return false; //error parsing arguments
        }
        static void Main(string[] args)
        {
            if (!ParseArguments(args))
            {
                Console.WriteLine("ERROR. Incorrect arguments.");
                Console.WriteLine("Usage: GitHubWikiToPDF [-user=<github-user> -project=<github-project> | -input-file=<input-file (.md)]> -css=<css-file> -output-file=<output-file (.pdf)>");
                Console.WriteLine("Use examples:");
                Console.WriteLine("\ta) Download and convert a GitHub wiki: GitHubWikiToPDF -user=simionsoft -project=SimionZoo -output-file=SimionZoo.pdf -css=style.css");
                Console.WriteLine("\tb) Convert a local markdown file: GitHubWikiToPDF -input-file=../myLocalFile.md -output-file=myLocalFile.pdf -css=style.css");
                return;
            }

            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            //Download the last version of the wiki
            Console.WriteLine("\n#### 1. Downloading the last version of the wiki");
            if (executionMode == ExecutionMode.GitHubWikiToPDF)
            {
                GitHubWikiDownloader downloader = new GitHubWikiDownloader();
                downloader.CloneWikiGitRepo(userName + "/" + projectName, tempFolder);
            }
            else
            {
                Console.WriteLine("Skipped");
            }

            //Convert it to html
            Console.WriteLine("\n#### 2. Converting markdown files to a single .html file");
            GitHubWikiToHtmlConverter markDownWikiToHtmlConverter = new GitHubWikiToHtmlConverter();

            using (StreamWriter htmlWriter = File.CreateText(mergedHtmlFilename))
            {
                markDownWikiToHtmlConverter.Convert(htmlWriter, markDownInputFolder, inputFile, tempFolder, cssFile);
            }

            //Convert the html file to pdf
            Console.WriteLine("\n#### 3. Generating the PDF file from the merged Html file");
            string htmlFileAsString = File.ReadAllText(mergedHtmlFilename);
            if (!outputFile.EndsWith(".pdf")) outputFile += ".pdf";

            PdfSharp.Pdf.PdfDocument document = new PdfDocument();
            PdfSharp.Pdf.PdfPage page = document.AddPage();
            PageSize size= page.Size;

            // Create a font
            XFont font = new XFont("Times", 12, XFontStyle.Bold);

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XRect rect = new XRect(0, 0, page.Width, page.Height);
            gfx.DrawString("trying PdfSharp", font, XBrushes.DarkRed, rect, XStringFormats.TopLeft);
            gfx.DrawString("trying PdfSharp to see if i can generate the pdf automatically", font, XBrushes.DarkBlue, rect, XStringFormats.BottomLeft);
            document.Save(outputFile);
            //using (FileStream writer = File.Create(outputFile))
            //{
            //    iTextSharp.text.Document document = new iTextSharp.text.Document();
            //    iTextSharp.text.pdf.PdfWriter.GetInstance(document, writer);
            //    document.Open();

            //    List<IElement> htmlarraylist = html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlFileAsString), null);
            //    for (int k = 0; k < htmlarraylist.Count; k++)
            //    {
            //        document.Add((IElement)htmlarraylist[k]);
            //    }

            //    document.Close();


            //    //writer.Write(pdf, 0, pdf.Length);
            //}

            ////PdfDocument pdf = PdfGenerator.GeneratePdf(htmlFileAsString, PdfSharp.PageSize.A4);
            ////if (!outputFile.EndsWith(".pdf")) outputFile += ".pdf";
            ////pdf.Save(outputFile);

            ////HtmlToPdf exporter= new HtmlToPdf();
            ////PdfDocument pdf= exporter.RenderHTMLFileAsPdf(mergedHtmlFilename);
            ////if (!outputFile.EndsWith(".pdf")) outputFile += ".pdf";
            ////pdf.SaveAs(outputFile);
        }
    }
}
