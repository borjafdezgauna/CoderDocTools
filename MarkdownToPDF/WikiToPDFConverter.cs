using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading.Tasks;


namespace MarkdownToPDF
{
    public class MardownToPDFConverter
    {
        public List<string> ConvertedPages = new List<string>();
        public List<string> LinkedPages = new List<string>();
        List<string> DownloadedImages = new List<string>();

        WebClient webClient= new WebClient(); //used to download images

        void DownloadImage(string url, string localFile)
        {
            string outputFolder = Path.GetDirectoryName(localFile);
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
            webClient.DownloadFile(url, localFile);
        }
        string FromUrlToLocalFile(string url, string localFolder)
        {
            string localFile = localFolder;
            if (!localFile.EndsWith("\\") && !localFile.EndsWith("/"))
                localFile += "/";
            return localFile + "img/" + Path.GetFileName(url);
        }
        string FromUrlToLocalFileRelativeToHtml(string url)
        {
            return "img/" + Path.GetFileName(url);
        }

        string WikifyLink(string url)
        {
            return url.ToLower().Replace(' ', '-');
        }
        string LinkToAnchorName(string url)
        {
            int lastSlash = Math.Max(url.LastIndexOf('/'), url.LastIndexOf('\\'));
            if (lastSlash >= 0)
                return url.Substring(lastSlash + 1);
            return url;
        }

        string DownloadImage(string imageUrl)
        {
            string localFile = FromUrlToLocalFileRelativeToHtml(imageUrl);
            if (!DownloadedImages.Contains(imageUrl))
            {
                DownloadImage(imageUrl, localFile);
                DownloadedImages.Add(imageUrl);
            }
            return localFile;
        }
        bool ParseImage(string line, bool inline = true)
        {
            Match match = Regex.Match(line, CapturePatternImage); //![]()
            if (match.Success)
            {
                string imageUrl = match.Groups[2].Value;
                string localFile = DownloadImage(imageUrl);

                if (inline) m_wikiPDFDocument.AddInlineImageToLastParagraph(localFile);
                else m_wikiPDFDocument.AddImage(localFile);
                return true;
            }
            return false;
        }
        void ParseLinks(string line)
        {
            Match match;
            foreach (string linkPattern in LinkPatterns)
            {
                match = Regex.Match(line, linkPattern);
                if (match.Success)
                {
                    string text = match.Groups[1].Value;
                    string wikiLink;

                    if (match.Groups.Count < 4)
                    {
                        //regular link: text and link
                        if (match.Groups.Count > 2)
                            wikiLink = match.Groups[2].Value;
                        else wikiLink = text;

                        if (!wikiLink.StartsWith("http"))
                            m_wikiPDFDocument?.AddLinkToLastParagraph(text, WikifyLink(wikiLink));
                        else
                            m_wikiPDFDocument?.AddLinkToLastParagraph(text, wikiLink);
                    }
                    else
                    {
                        //linked image: [![alt-text](image-link)](link)
                        wikiLink = match.Groups[3].Value;

                        string localFile = DownloadImage(match.Groups[2].Value);
                        m_wikiPDFDocument?.AddInlineImageToLastParagraph(localFile);
                    }

                    //Add linked documents to pending list
                    if (!wikiLink.StartsWith("http"))
                    {
                        wikiLink = WikifyLink(wikiLink);
                        //Add to the list of linked pages
                        LinkedPages.Add(wikiLink + ".md");
                    }
                }
            }
        }
 
        void SetParagraphTypeByLineStart(ref string line, int numIndents)
        {
            if (line.StartsWith("# "))
            {
                line = line.Substring(2);
                m_wikiPDFDocument.StartHeader(2, WikifyLink(line)); // <- the title will be used as the name of the reference to be used in links to the page that originated this heading
            }
            else if (line.StartsWith("## "))
            {
                line = line.Substring(3);
                m_wikiPDFDocument.StartHeader(3, WikifyLink(line)); // <- the title will be used as the name of the reference to be used in links to the page that originated this heading
            }
            else if (line.StartsWith("### "))
            {
                line = line.Substring(4);
                m_wikiPDFDocument.StartHeader(4);
            }
            else if (line.StartsWith("#### "))
            {
                line = line.Substring(5);
                m_wikiPDFDocument.StartHeader(5);
            }
            else if (line.StartsWith("```"))
            {
                m_wikiPDFDocument.ToggleCodeBlock(2);
                line = null;
            }
            else if (line.StartsWith("> "))
            {
                line = line.Substring(2);
                m_wikiPDFDocument.StartNote(numIndents);
            }
            else if (line.StartsWith("* "))
            {
                line = line.Substring(2);
                m_wikiPDFDocument.AddListItem(numIndents);
            }
            else if (line.StartsWith("- "))
            {
                line = line.Substring(2);
                m_wikiPDFDocument.AddListItem(numIndents);
            }
            else if (line.StartsWith("!["))
            {
                ParseImage(line, false);
                line = null;
            }
            else m_wikiPDFDocument?.StartParagraph(numIndents);           
        }

        const string CapturePatternImage= @"\!\[([^\]]+)\]\(([^\)]+)\)"; //![]()
        const string CapturePatternInlineLinkedImage = @"\[\!\[([^\]]+)\]\(([^\)]+)\)\]\(([^\)]+)\)"; //[![]()]()
        const string CapturePatternInlineLink1 = @"\[\[([^\]]+)\|([^\]]+)\]\]"; //[[text|url]]
        const string CapturePatternInlineLink2 = @"\[([^\]]+)\]\(([^\)]+)\)"; //[text](url)
        const string CapturePatternInlineLink3 = @"\[\[([^\]\|]+)\]\]"; //[[url]]

        //Patterns without a capture group
        const string PatterInlineLinkedImage = @"\[\!\[[^\]]+\]\([^\)]+\)\]\([^\)]+\)"; //[![]()]()
        const string PatternInlineImage = @"\!\[[^\]]+\]\([^\)]+\)"; //![]()
        const string PatternInlineLink1 = @"\[\[[^\]]+\|[^\]]+\]\]"; //[[text|url]]
        const string PatternInlineLink2 = @"\[[^\]]+\]\([^\)]+\)"; //[text](url)
        const string PatternInlineLink3 = @"\[\[[^\]\|]+\]\]"; //[[url]]
        const string PatternInlineBold1 = @"\*\*[^\*]+\*\*"; //**bold text** <- not sure this is standard or just my thing
        const string PatternInlineBold2 = @"\*[^\*]+\*"; //*bold text*
        const string PatternInlineItalic = @"(?<!\w)_.+?_(?!\w)"; // _italic text_
        const string PatternInlineCode = @"`[^`]+`"; // ` text `
        const string PatternAllInlines = "(" + PatterInlineLinkedImage + "|" + PatternInlineImage + "|" + PatternInlineBold1 + "|" + PatternInlineBold2 + "|" + PatternInlineCode 
            + "|" + PatternInlineItalic + "|" + PatternInlineLink1 + "|" + PatternInlineLink2 + "|" + PatternInlineLink3 + ")";
        string [] LinkPatterns = { CapturePatternInlineLinkedImage, CapturePatternInlineLink1, CapturePatternInlineLink2, CapturePatternInlineLink3 };

        public List<string> SplitByInlinePatterns(string text)
        {
            int lastStop = 0;
            List<string> result = new List<string>();

            MatchCollection matches = Regex.Matches(text, PatternAllInlines);
            if (matches.Count == 0)
            {
                //no matches -> all the text is a single element
                result.Add(text);
            }
            else
            {
                foreach (Match match in matches)
                {
                    if (match.Index > lastStop) result.Add(text.Substring(lastStop, match.Index - lastStop));
                    result.Add(match.Value);
                    lastStop = match.Index + match.Length;

                }
                //add remaining text after the last match if there's any
                Match lastMatch = matches[matches.Count - 1];
                int lastMatchEnd = lastMatch.Index + lastMatch.Length;
                if (lastMatchEnd < text.Length) result.Add(text.Substring(lastMatchEnd));
            }

            return result;
        }

        public void ParseInlineElements(string line, int numIndents)
        {
            if ( line == null || line.Length == 0 ) return;

            bool isFirstPart = true;

            List<string> splitParts = SplitByInlinePatterns(line);
            foreach (string part in splitParts)
            {
                //Inline code
                if (part.Length >= 3)
                {
                    if (part[0] == '`' && part[part.Length - 1] == '`')
                        m_wikiPDFDocument?.AddInlineCodeToLastParagraph(part.Substring(1, part.Length - 2));
                    //Inline bold text
                    else if (part.Length > 4 && part[0] == '*' && part[1] == '*' && part[part.Length - 2] == '*' && part[part.Length - 1] == '*')
                        m_wikiPDFDocument?.AddBoldTextToLastParagraph(part.Substring(2, part.Length - 4));
                    else if (part[0] == '*' && part[part.Length - 1] == '*')
                        m_wikiPDFDocument?.AddBoldTextToLastParagraph(part.Substring(1, part.Length - 2));
                    //Inline italic text
                    else if (part[0] == '_' && part[part.Length - 1] == '_')
                        m_wikiPDFDocument?.AddItalicTextToLastParagraph(part.Substring(1, part.Length - 2));
                    //Links
                    else if (part[0] == '[' && (part[part.Length - 1] == ']' || part[part.Length - 1] == ')')) // <- this condition is not 100% fail safe
                    {
                        ParseLinks(part);
                    }
                    else if (part[0] == '_' && part[part.Length - 1] == '_')
                        m_wikiPDFDocument?.AddItalicTextToLastParagraph(part.Substring(1, part.Length - 2));
                    else
                    {
                        //image?
                        bool isImage = false;
                        if (part.StartsWith("![")) isImage= ParseImage(part);
                        if (!isImage) m_wikiPDFDocument?.AddTextToLastParagraph(part, isFirstPart);
                    }
                }
                else m_wikiPDFDocument?.AddTextToLastParagraph(part, isFirstPart);

                isFirstPart = false;
            }
            
        }
        public static string DocNameFromFilename(string htmlDocFilename)
        {
            string docName = Path.GetFileNameWithoutExtension(htmlDocFilename);
            docName = docName.Replace('-', ' ');
            var result = Regex.Replace(docName, @"\b(\w)", m => m.Value.ToUpper());
            docName = Regex.Replace(result, @"(\s(of|in|by|and|the)|\'[st])\b", m => m.Value.ToLower(), RegexOptions.IgnoreCase);

            return docName;
        }

        int CountSpacesAtBeginning(string line)
        {
            int i = 0;
            while (i<line.Length && line[i] == ' ')
                i++;
            return i;
        }

        public void Convert(string inputMarkdownFolder, string markdownDocFilename, string outputHtmlFolder)
        {
            //we ignore external references
            List<string> ignoredPrefixes = new List<string>(){ "http" };
            foreach (string ignoredPrefix in ignoredPrefixes)
            {
                if (markdownDocFilename.StartsWith(ignoredPrefix))
                    return;
            }
            //we ignore references to anchors
            if (markdownDocFilename.Contains("#"))
                return;

            string localFilename = inputMarkdownFolder + "\\" + markdownDocFilename;

            if (!File.Exists(localFilename))
            {
                Console.WriteLine("Warning: Invalid reference to " + markdownDocFilename + " found");
                return;
            }

            string[] lines = File.ReadAllLines(localFilename);

            if (lines == null)
            {
                Console.WriteLine("ERROR. Couldn't find referenced page: " + markdownDocFilename);
                return;
            }

            m_wikiPDFDocument.StartHeader(1, Path.GetFileNameWithoutExtension(markdownDocFilename));
            m_wikiPDFDocument.AddTextToLastParagraph(DocNameFromFilename(localFilename), true);

            foreach (string line in lines)
            {
                int numIndents = 0;
                numIndents = CountSpacesAtBeginning(line);
                string trimmedLine= line.Trim(' ');

                if (trimmedLine.Length > 0)
                {
                    SetParagraphTypeByLineStart(ref trimmedLine, numIndents);

                    if (trimmedLine != null)
                    {
                        if (!m_wikiPDFDocument.IsCodeBlockOpen())
                            ParseInlineElements(trimmedLine, numIndents);
                        else
                            //we add the line unparsed if there's a code block open
                            m_wikiPDFDocument.AddTextToLastParagraph(line, false, numIndents);
                    }
                }
            }

            //parsedLines.Add(CloseAllOpenLists()); //In case there is some un-closed list

            ConvertedPages.Add(markdownDocFilename);
            while (LinkedPages.Count > 0)
            {
                string linkedPage = LinkedPages[0];
                LinkedPages.RemoveAt(0);

                if (!ConvertedPages.Contains(linkedPage))
                    Convert(inputMarkdownFolder, linkedPage, outputHtmlFolder);
            }
        }

        /// <summary>
        /// MigraDoc document
        /// </summary>
        WikiPDFDocument m_wikiPDFDocument;
        
        public void CreatePDFDocument(string title, string subtitle = "", string author = "", string subject= "")
        {
            m_wikiPDFDocument = new WikiPDFDocument(title, subtitle, author, subject);
        }

        public void SavePDFDocument(string filename)
        {
            m_wikiPDFDocument.Save(filename);
        }
    }
}
