using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MarkdownToPDF
{
    class GitHubWikiDownloader
    {
        public GitHubWikiDownloader()
        { }

        public void CloneWikiGitRepo(string repositoryName, string outputFolder)
        {
            if (!repositoryName.StartsWith("/"))
                repositoryName = "/" + repositoryName;
            if (repositoryName.EndsWith("/"))
                repositoryName = repositoryName.Substring(0, repositoryName.Length -1);

            string wikiHomeUrl = "https://github.com" + repositoryName + ".wiki.git";

            Console.WriteLine("Downloading the last version of the wiki from " + wikiHomeUrl);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "git";

            if (!Directory.Exists(outputFolder + "/.git"))
                startInfo.Arguments = "clone " + wikiHomeUrl + " " + outputFolder;
            else
            {
                startInfo.WorkingDirectory = outputFolder;
                startInfo.Arguments = "pull";
            }
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            Console.WriteLine(startInfo.FileName + " " + startInfo.Arguments + " " + wikiHomeUrl);

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            Console.WriteLine(process.StandardOutput.ReadToEnd());

            process.WaitForExit();
        }
    }
}
