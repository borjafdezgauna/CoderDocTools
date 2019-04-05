using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documenter
{
    class TextUtils
    {
        public static string RemoveLinebreaksAndTrim(string code)
        {
            return code.Trim(' ').Replace('\n', ' ').Replace('\r', ' ').Replace("  ", " ");
        }
    }
}
