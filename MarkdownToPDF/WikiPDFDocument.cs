﻿using MigraDoc.DocumentObjectModel;
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
            DefineStyles();

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

        const string StyleHeading1 = StyleNames.Heading1;
        const string StyleHeading2 = StyleNames.Heading2;
        const string StyleHeading3 = StyleNames.Heading3;
        const string StyleHeading4 = StyleNames.Heading4;
        const string StyleHeading5 = StyleNames.Heading5;
        const string StyleNormal = StyleNames.Normal;
        const string StyleLink = StyleNames.Hyperlink;
        const string StyleList1 = StyleNames.List + "1";
        const string StyleList2 = StyleNames.List + "2";
        const string StyleList3 = StyleNames.List + "3";
        const string StyleParagraphInList1 = "ParagraphInList1";
        const string StyleParagraphInList2 = "ParagraphInList2";
        const string StyleParagraphInList3 = "ParagraphInList3";
        const string StyleHeader = StyleNames.Header;
        const string StyleFooter = StyleNames.Footer;
        const string StyleNote = "Note";
        const string StyleCode = "Code";
        const string StyleInlineCode = "InlineCode";
        const string StyleImage = "Image";
        const string StyleHyperlink = StyleNames.Hyperlink;
        const string StyleCoverTitle = "CoverTitle";
        const string StyleCoverSubTitle = "CoverSubTitle";

        void DefineStyles()
        {
            //Normal
            Style style = m_document.Styles["Normal"];
            style.Font.Name = "CMU Serif";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.25);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.25);

            //Heading 1
            style = m_document.Styles[StyleHeading1];
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.5);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0);
            style.Font.Size = 20;
            style.Font.Bold = true;
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;

            //Heading 2
            style = m_document.Styles[StyleHeading2];
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.4);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.5);
            style.Font.Size = 16;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;

            //Heading 3
            style = m_document.Styles[StyleHeading3];
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.4);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.5);
            style.Font.Size = 14;
            style.Font.Bold = true;

            //Heading 4
            style = m_document.Styles[StyleHeading4];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;

            //Heading 5
            style = m_document.Styles[StyleHeading5];
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(1);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.5);
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;

            //Hyperlinks
            Color HyperlinkColor = Color.FromRgb(8, 46, 233);
            style = m_document.Styles[StyleHyperlink];
            style.Font.Color = HyperlinkColor;

            //List - level 1
            style = m_document.AddStyle(StyleList1, StyleNormal);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.25);
            style.ParagraphFormat.FirstLineIndent = Unit.FromCentimeter(-0.2);
            
            //List - level 2
            style = m_document.AddStyle(StyleList2, StyleList1);
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level2;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.5);

            //List - level 3
            style = m_document.AddStyle(StyleList3, StyleList1);
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.75);

            //Paragraph in list item - level 1
            style = m_document.AddStyle(StyleParagraphInList1, StyleList1);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.28);
            style.ParagraphFormat.FirstLineIndent = Unit.FromCentimeter(0);
            //Paragraph in list item - level 2
            style = m_document.AddStyle(StyleParagraphInList2, StyleList2);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.53);
            style.ParagraphFormat.FirstLineIndent = Unit.FromCentimeter(0);
            //Paragraph in list item - level 3
            style = m_document.AddStyle(StyleParagraphInList3, StyleList3);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.78);
            style.ParagraphFormat.FirstLineIndent = Unit.FromCentimeter(0);

            //Page Header
            style = m_document.Styles[StyleHeader];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style.ParagraphFormat.Borders.Bottom.Color = Colors.DarkGray;
            style.ParagraphFormat.Borders.Top.Visible = false;
            style.ParagraphFormat.Borders.Left.Visible = false;
            style.ParagraphFormat.Borders.Right.Visible = false;
            style.ParagraphFormat.Borders.Width = 0.5;
            //Page Footer
            style = m_document.Styles[StyleFooter];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
            style.ParagraphFormat.Borders.Top.Color = Colors.DarkGray;
            style.ParagraphFormat.Borders.Bottom.Visible = false;
            style.ParagraphFormat.Borders.Left.Visible = false;
            style.ParagraphFormat.Borders.Right.Visible = false;

            //Add our own styles needed for markdown files
            Color codeBackground = new Color(226, 226, 226);
            Color codeBorder = new Color(160, 160, 160);

            //Code
            style = m_document.Styles.AddStyle(StyleCode, StyleNormal);
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.SpaceBefore = "0.5cm";
            style.ParagraphFormat.SpaceAfter = "0.5cm";
            style.ParagraphFormat.Borders.Width = 0.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Borders.Color = codeBorder;
            style.Font.Name = "Courier New";
            style.ParagraphFormat.Shading.Color = codeBackground;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.5);
            style.ParagraphFormat.RightIndent = Unit.FromCentimeter(0.5);

            //Inline code
            Color InlineCodeColor = Color.FromRgb(50, 0, 50);
            style = m_document.Styles.AddStyle(StyleInlineCode, StyleCode);
            style.Font.Color = InlineCodeColor;

            //Note
            style = m_document.Styles.AddStyle(StyleNote, StyleCode);
            style.Font.Name = "Courier New";
            style.Font.Color = InlineCodeColor;

            //Image
            style = m_document.Styles.AddStyle(StyleImage, StyleNormal);
            style.ParagraphFormat.SpaceBefore = "1cm";
            style.ParagraphFormat.SpaceAfter = "1cm";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            //Cover title
            style = m_document.Styles.AddStyle(StyleCoverTitle,StyleNormal);
            style.Font.Size = 42;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.Borders.Bottom.Color = Colors.DarkGray;
            style.ParagraphFormat.Borders.Top.Visible = false;
            style.ParagraphFormat.Borders.Left.Visible = false;
            style.ParagraphFormat.Borders.Right.Visible = false;
            style.ParagraphFormat.Borders.Width = 0.5;
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(4);

            //Cover subtitle
            style = m_document.Styles.AddStyle(StyleCoverSubTitle, StyleNormal);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.4);
        }

        void AddCoverPage(string title, string subtitle, string author)
        {
            m_document.AddSection();
            m_document.LastSection.AddParagraph(title, StyleCoverTitle);
            string date = DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day;
            if (subtitle != null) m_document.LastSection.AddParagraph(subtitle, StyleCoverSubTitle);
            if (author != null) m_document.LastSection.AddParagraph(author + " (" + date + ")", StyleCoverSubTitle);
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
                if (numIndents == 0 || m_numOpenLists == 0) { style = StyleNormal; CurrentParagraphType = ParagraphType.Normal; }
                else
                {
                    switch (m_numOpenLists)
                    {
                        case 1: style = StyleParagraphInList1; break;
                        case 2: style = StyleParagraphInList2; break;
                        case 3: style = StyleParagraphInList3; break;
                        default: style = StyleNormal; CurrentParagraphType = ParagraphType.Normal; break;
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
                case 1: style = StyleList1; bullet = "- "; break;
                case 2: style = StyleList2; bullet = "- "; break;
                default: style = StyleList3; bullet = "* "; break;
            }
            Paragraph paragraph= m_document?.LastSection.AddParagraph(bullet, style);
        }

        public void StartNote(int level)
        {
            CurrentParagraphType = ParagraphType.Note;
            m_document?.LastSection.AddParagraph("", StyleNote);
        }

        public void ToggleCodeBlock(int level)
        {
            if (CurrentParagraphType != ParagraphType.Code)
            {
                CurrentParagraphType = ParagraphType.Code;
                m_document?.LastSection.AddParagraph("", StyleCode);
            }
            else CurrentParagraphType = ParagraphType.Normal;
        }
        public bool IsCodeBlockOpen()
        {
            return CurrentParagraphType == ParagraphType.Code;
        }

        int m_lastSectionIndex = 0;
        int m_lastSubSectionIndex = 0;

        public void AddTextToLastParagraph(string text, bool textStart, int numIndents= 0)
        {
            if (textStart && CurrentParagraphType == ParagraphType.Heading1)
            {
                m_lastSectionIndex++;
                m_lastSubSectionIndex = 0;
                SetHeaderText(m_docTitle + ": " + m_lastSectionIndex + " " + text);
                m_document?.LastSection.LastParagraph.AddText(m_lastSectionIndex + " " + text);
            }
            else if (textStart && CurrentParagraphType == ParagraphType.Heading2)
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
            m_document?.LastSection.LastParagraph.AddFormattedText(text, StyleInlineCode);
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
                image = m_document.LastSection.AddParagraph("", StyleImage).AddImage(filename);
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