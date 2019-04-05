using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToPDF
{
    class Utils
    {
        public static string WikifyLink(string url)
        {
            return url.Replace(' ', '-');
        }
    }
}
