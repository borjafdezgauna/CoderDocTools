using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownToPDF
{
    class WikiPDFDocument
    {
        Document m_document;
        string m_docTitle;

        public WikiPDFDocument(string title, string subtitle, string author, string subject)
        {
            // Create a new MigraDoc document
            m_document = new Document();

            m_document.UseCmykColor = false;
            m_document.Info.Title = title;
            m_document.Info.Subject = subject;
            m_document.Info.Author = author;
            m_docTitle = title;

            //Add style definitions to the document
            Styler.DefineStyles(m_document);

            AddCoverPage(title, subtitle, author);
        }

        public void Save(string filename)
        {
            try
            {
                MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(m_document, "MigraDoc.mdddl");
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = m_document;
                renderer.RenderDocument();
                renderer.PdfDocument.Save(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving the PDF file:\n" + e.ToString());
            }
        }





        void AddCoverPage(string title, string subtitle, string author)
        {
            m_document.AddSection();
            m_document.LastSection.AddParagraph(title, Styler.StyleCoverTitle);
            string date = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day;
            if (subtitle != null) m_document.LastSection.AddParagraph(subtitle, Styler.StyleCoverSubTitle);
            if (author != null) m_document.LastSection.AddParagraph(author + " (" + date + ")", Styler.StyleCoverSubTitle);
        }

        void SetHeaderText(string header)
        {
            Paragraph paragraph= m_document?.LastSection.Headers.Primary.AddParagraph("");
            paragraph.AddTab();
            paragraph.AddText(header);
            paragraph = m_document?.LastSection.Headers.EvenPage.AddParagraph("");
            paragraph.AddTab();
            paragraph.AddText(header);
        }

        bool bFirstSection = true;
        void StartSection()
        {
            //Add new section
            Section section = m_document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            if (bFirstSection)
            {
                section.PageSetup.StartingNumber = 1;
                bFirstSection = false;
            }

            // Create a paragraph with centered page number. See definition of style "Footer".
            Paragraph paragraph = new Paragraph();
            paragraph.AddTab();
            paragraph.AddPageField();

            // Add paragraph to footer for odd pages.
            section.Footers.Primary.Add(paragraph);
            // Add clone of paragraph to footer for odd pages. Cloning is necessary because an object must
            // not belong to more than one other object. If you forget cloning an exception is thrown.
            section.Footers.EvenPage.Add(paragraph.Clone());
        }

        public void StartHeader(int level, string refName = null)
        {
            switch (level)
            {
                case 1:
                    CurrentParagraphType = ParagraphType.Heading1; break;
                case 2:
                    CurrentParagraphType = ParagraphType.Heading2; break;
                case 3:
                    CurrentParagraphType = ParagraphType.Heading3; break;
                default:
                    CurrentParagraphType = ParagraphType.Heading4; break;
            }

            //Close all open lists
            m_numOpenLists = 0;

            if (level == 1) StartSection();

            Paragraph headingParagraph = m_document?.LastSection.AddParagraph("", "Heading" + level);
            if (level < 4 && refName != null)
                headingParagraph.AddBookmark(refName);
        }

        public void StartParagraph(int numIndents)
        {
            //We do nothing if we are in a code block
            if (CurrentParagraphType != ParagraphType.Code)
            {
                string style;
                if (numIndents == 0 || m_numOpenLists == 0) { style = Styler.StyleNormal; CurrentParagraphType = ParagraphType.Normal; }
                else
                {
                    switch (m_numOpenLists)
                    {
                        case 1: style = Styler.StyleParagraphInList1; break;
                        case 2: style = Styler.StyleParagraphInList2; break;
                        case 3: style = Styler.StyleParagraphInList3; break;
                        default: style = Styler.StyleNormal; CurrentParagraphType = ParagraphType.Normal; break;
                    }
                }
                m_document?.LastSection.AddParagraph("", style);
            }
        }

        enum ParagraphType {Heading1, Heading2, Heading3, Heading4, Code, Image, Normal, List, Note};
        ParagraphType CurrentParagraphType;

        int m_numOpenLists = 0;
        const int maxNumLists = 3;
        int [] m_openListLevel= new int[maxNumLists];

        public void AddListItem(int level)
        {
            CurrentParagraphType = ParagraphType.List;

            if (m_numOpenLists == 0 || level > m_openListLevel[m_numOpenLists-1])
            {
                m_openListLevel[m_numOpenLists] = level;
                m_numOpenLists++;
            }
            else 
            {
                while (m_numOpenLists > 0 && level < m_openListLevel[m_numOpenLists - 1])
                    m_numOpenLists--;
            }

            string style;
            string bullet;
            switch (m_numOpenLists)
            {
                case 1: style = Styler.StyleList1; bullet = "- "; break;
                case 2: style = Styler.StyleList2; bullet = "- "; break;
                default: style = Styler.StyleList3; bullet = "* "; break;
            }
            Paragraph paragraph= m_document?.LastSection.AddParagraph(bullet, style);
        }

        public void StartNote(int level)
        {
            //Do nothing if within a code block
            if (IsCodeBlockOpen()) return;

            CurrentParagraphType = ParagraphType.Note;
            m_document?.LastSection.AddParagraph("", Styler.StyleNote);
        }

        public void ToggleCodeBlock(int level)
        {
            if (CurrentParagraphType != ParagraphType.Code)
            {
                CurrentParagraphType = ParagraphType.Code;
                m_document?.LastSection.AddParagraph("", Styler.StyleCode);
            }
            else CurrentParagraphType = ParagraphType.Normal;
        }
        public bool IsCodeBlockOpen()
        {
            return CurrentParagraphType == ParagraphType.Code;
        }

        int m_lastSectionIndex = 0;
        int m_lastSubSectionIndex = 0;

        public void AddTextToLastParagraph(string text, bool isFirstPart= true, int numIndents= 0)
        {
            if (isFirstPart && CurrentParagraphType == ParagraphType.Heading1)
            {
                m_lastSectionIndex++;
                m_lastSubSectionIndex = 0;
                SetHeaderText(m_docTitle + ": " + m_lastSectionIndex + " " + text);
                m_document?.LastSection.LastParagraph.AddText(m_lastSectionIndex + " " + text);
            }
            else if (isFirstPart && CurrentParagraphType == ParagraphType.Heading2)
            {
                m_lastSubSectionIndex++;
                m_document?.LastSection.LastParagraph.AddText(m_lastSectionIndex + "." + m_lastSubSectionIndex + " " + text);
            }
            else if (CurrentParagraphType == ParagraphType.Code)
            {
                m_document?.LastSection.LastParagraph.AddSpace(numIndents);
                m_document?.LastSection.LastParagraph.AddText(text + "\n");
            }
            else
                m_document?.LastSection.LastParagraph.AddText(text);
        }

        public void AddBoldTextToLastParagraph(string text)
        {
            m_document?.LastSection.LastParagraph.AddFormattedText(text, TextFormat.Bold);
        }

        public void AddItalicTextToLastParagraph(string text)
        {
            m_document?.LastSection.LastParagraph.AddFormattedText(text, TextFormat.Italic);
        }

        public void AddInlineCodeToLastParagraph(string text)
        {
            m_document?.LastSection.LastParagraph.AddFormattedText(text, Styler.StyleInlineCode);
        }

        double GetImageWidthInPoints(string filename)
        {
            Unit width;
            using (XImage image2 = XImage.FromFile(filename))
            {
                width = Unit.FromPoint(image2.PixelWidth/ 1.54);
            }
            return width;
        }

        public void AddInlineImageToLastParagraph(string filename)
        {
            MigraDoc.DocumentObjectModel.Shapes.Image image;
            if (!filename.EndsWith(".svg"))
            {
                image = m_document.LastSection.LastParagraph.AddImage(filename);
                Unit width = Unit.FromPoint(GetImageWidthInPoints(filename));
                image.LockAspectRatio = true;
                image.Width = width;
            }
        }

        public void AddImage(string filename)
        {
            CurrentParagraphType = ParagraphType.Image;
            MigraDoc.DocumentObjectModel.Shapes.Image image;
            if (!filename.EndsWith(".svg"))
            {
                image = m_document.LastSection.AddParagraph("", Styler.StyleImage).AddImage(filename);
                Unit maxWidth = Unit.FromCentimeter(14);
                Unit width = Unit.FromPoint(GetImageWidthInPoints(filename));
                image.LockAspectRatio = true;
                image.Width = Unit.FromCentimeter( Math.Min(width.Centimeter, maxWidth.Centimeter) );
            }
        }

        public void AddLinkToLastParagraph(string text, string link)
        {
            Hyperlink hyperlink;
            if (!link.StartsWith(".") && !link.StartsWith("http"))
            {
                //reference to a document converted to section
                hyperlink = m_document.LastSection.LastParagraph.AddHyperlink(link);
                hyperlink.AddText(text);
                //hyperlink.AddPageRefField(link);
            }
            else if (link.StartsWith("http"))
            {
                hyperlink = m_document.LastSection.LastParagraph.AddHyperlink(link, HyperlinkType.Url);
                hyperlink.AddText(text);
                //hyperlink.AddPageRefField(link);
            }
            else
            {
                Console.WriteLine("Warning: Relative path to a document outside the wiki (" + link + "). Convert it to an absolute Url");
                m_document.LastSection.LastParagraph.AddText(text);
            }
        }
    }
}
