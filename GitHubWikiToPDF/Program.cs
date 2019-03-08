using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubWikiToPDF
{
    class Program
    {
        const string projectNameArg = "-project=";
        static string projectName = null;

        static bool ParseArguments(string [] args)
        {
            foreach(string arg in args)
            {
                if (arg.StartsWith(projectNameArg)) projectName = arg.Substring(projectNameArg.Length);
            }
            if (projectName != null) return true;
            return false; //error parsing arguments
        }
        static void Main(string[] args)
        {
            if (!ParseArguments(args))
            {
                Console.WriteLine("ERROR. Incorrect arguments.\nUsage: GitHubWikiToPDF -project=<name-of-the-github-project>\nFor example: GitHubWikiToPDF -project=simionsoft/SimionZoo");
                return;
            }

            GitHubWikiDownloader downloader = new GitHubWikiDownloader();
            downloader.CloneWikiGitRepo(projectName, "temp");

            GitHubWikiToHtmlConverter converter = new GitHubWikiToHtmlConverter();
            converter.Convert("temp", "Home.md");
        }
    }
}
