using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPdf;

namespace GitHubWikiToPDF
{
    class Program
    {
        const string userNameArg = "-user=";
        static string userName = null;
        const string projectNameArg = "-project=";
        static string projectName = null;
        const string cssArg = "-css=";
        static string cssFile = null;

        static bool ParseArguments(string [] args)
        {
            foreach(string arg in args)
            {
                if (arg.StartsWith(projectNameArg)) projectName = arg.Substring(projectNameArg.Length);
                if (arg.StartsWith(userNameArg)) userName = arg.Substring(userNameArg.Length);
                if (arg.StartsWith(cssArg)) cssFile = arg.Substring(cssArg.Length);
            }
            if (projectName != null && userName != null) return true;
            return false; //error parsing arguments
        }
        static void Main(string[] args)
        {
            if (!ParseArguments(args))
            {
                Console.WriteLine("ERROR. Incorrect arguments.\nUsage: GitHubWikiToPDF -user=<github-user> -project=<github-project>\nFor example: GitHubWikiToPDF -user=simionsoft -project=SimionZoo");
                return;
            }

            Console.WriteLine("\n#### 1. Downloading the last version of the wiki");

            GitHubWikiDownloader downloader = new GitHubWikiDownloader();
            downloader.CloneWikiGitRepo(userName + "/" + projectName, "temp");

            Console.WriteLine("\n#### 2. Converting the wiki to a single Html file");
            GitHubWikiToHtmlConverter markDownWikiToHtmlConverter = new GitHubWikiToHtmlConverter();
            string htmlMergedDocFilename = "temp/" + projectName + ".html";
            using (StreamWriter htmlWriter = File.CreateText(htmlMergedDocFilename))
            {
                markDownWikiToHtmlConverter.Convert(htmlWriter, "temp", "Home.md", "style.css");
            }

            Console.WriteLine("\n#### 3. Generating the PDF file from the merged Html file");

            var exporter= new HtmlToPdf();
            var pdf= exporter.RenderHTMLFileAsPdf(htmlMergedDocFilename);
            pdf.SaveAs(projectName + ".pdf");
        }
    }
}
