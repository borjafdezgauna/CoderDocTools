using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Fonts;

namespace MarkdownToPDF
{
    class FontManager: IFontResolver
    {
        public const string RegularFont = "Regular";
        public const string CodeFont = "Code";

        Dictionary<string, string> FontFiles = new Dictionary<string, string>()
        { {RegularFont, "../fonts/regular.ttf" } , {CodeFont, "../fonts/code.ttf" } };

        public FontManager()
        { }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (!FontFiles.Keys.Contains(familyName))
                throw new Exception("Unknown font alias: " + familyName);

            FontResolverInfo info = new FontResolverInfo(familyName);
            return info;
        }

        public byte[] GetFont(string faceName)
        {
            if (!FontFiles.Keys.Contains(faceName))
                throw new Exception("Unknown font alias: " + faceName);

            if (!System.IO.File.Exists(FontFiles[faceName]))
                throw new Exception("Could not find font file: " + FontFiles[faceName]);

            return System.IO.File.ReadAllBytes(FontFiles[faceName]);
        }
    }
}
