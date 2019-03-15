using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubWikiToPDF
{
    class WikiPDFDocument
    {
        Document m_document;

        public WikiPDFDocument(string title, string author, string subject)
        {
            // Create a new MigraDoc document
            m_document = new Document();

            m_document.Info.Title = title;
            m_document.Info.Subject = subject;
            m_document.Info.Author = author;

            //Add style definitions to the document
            DefineStyles();

            //Add main content section
            Section section = m_document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            section.PageSetup.StartingNumber = 1;

            HeaderFooter header = section.Headers.Primary;
            header.AddParagraph("\t" + title);

            header = section.Headers.EvenPage;
            header.AddParagraph("\t" + title);
            header.AddTextFrame();

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

        const string StyleHeading1 = StyleNames.Heading1;
        const string StyleHeading2 = StyleNames.Heading2;
        const string StyleHeading3 = StyleNames.Heading3;
        const string StyleHeading4 = StyleNames.Heading4;
        const string StyleHeading5 = StyleNames.Heading5;
        const string StyleNormal = StyleNames.Normal;
        const string StyleNormal1 = StyleNames.Normal + "1";
        const string StyleNormal2 = StyleNames.Normal + "2";
        const string StyleLink = StyleNames.Hyperlink;
        const string StyleList1 = StyleNames.List;
        const string StyleList2 = StyleNames.List + "1";
        const string StyleList3 = StyleNames.List + "2";
        const string StyleHeader = StyleNames.Header;
        const string StyleFooter = StyleNames.Footer;
        const string StyleNote = "Note";
        const string StyleCode = "Code";
        const string StyleImage = "Image";
        const string StyleHyperlink = StyleNames.Hyperlink;

        void DefineStyles()
        {
            Style style = m_document.Styles["Normal"];
            style.Font.Name = "Helvetica";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            //style.ParagraphFormat.SpaceAfter = "0.75cm";
            style = m_document.Styles[StyleHeading1];
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.Font.Size = 20;
            style.Font.Bold = true;
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;
            style = m_document.Styles[StyleHeading2];
            style.Font.Size = 16;
            style.ParagraphFormat.SpaceBefore = "3cm";
            style.ParagraphFormat.SpaceAfter = "1.5cm";
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;
            style = m_document.Styles[StyleHeading3];
            //style.Font.Color = Colors.BlueViolet;
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style = m_document.Styles[StyleHeading4];
            //style.Font.Color = Colors.Purple;
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style = m_document.Styles[StyleHeading5];
            //style.Font.Color = Colors.MidnightBlue;
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;

            style = m_document.Styles[StyleHyperlink];
            style.Font.Color = Colors.Blue;

            //Normal - level 1
            style = m_document.AddStyle(StyleNormal1, StyleNormal);
            style.ParagraphFormat.LeftIndent = "1cm";
            style = m_document.AddStyle(StyleNormal2, StyleNormal);
            style.ParagraphFormat.LeftIndent = "1.5cm";

            //List - level 1
            style = m_document.Styles[StyleList1];
            style.ParagraphFormat.LeftIndent = "0.5cm";
            style.ParagraphFormat.FirstLineIndent = "0cm";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.SpaceAfter = "0.2cm";
            style.ParagraphFormat.SpaceBefore = "0.2cm";
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;

            style = m_document.AddStyle(StyleList2, StyleList1);
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level2;
            style.ParagraphFormat.LeftIndent = "1cm";
            style = m_document.AddStyle(StyleList3, StyleList1);
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level3;
            style.ParagraphFormat.LeftIndent = "1.5cm";

            style = m_document.Styles[StyleHeader];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style = m_document.Styles[StyleFooter];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            //Add our own styles needed for markdown files
            Color codeBackground = new Color(248, 248, 248);
            Color codeBorder = new Color(234, 234, 234);

            style = m_document.Styles.AddStyle(StyleCode, StyleNormal);
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.SpaceBefore = "0.5cm";
            style.ParagraphFormat.SpaceAfter = "0.5cm";
            style.ParagraphFormat.Borders.Width = 0.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Borders.Color = codeBorder;
            style.Font.Name = "Courier";
            style.ParagraphFormat.Shading.Color = codeBackground;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.LeftIndent = "1.5cm";
            style.ParagraphFormat.RightIndent = "1.5cm";
            style = m_document.Styles.AddStyle(StyleNote, StyleCode);
            style.Font.Name = "Helvetica";

            style = m_document.Styles.AddStyle(StyleImage, StyleNormal);
            style.ParagraphFormat.SpaceBefore = "1cm";
            style.ParagraphFormat.SpaceAfter = "1cm";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        }

        public void StartHeader(int level, string refName = null)
        {
            //Close all open lists
            m_numOpenLists = 0;
            m_openListLevel = 0;
            Paragraph headingParagraph = m_document?.LastSection.AddParagraph("", "Heading" + level);
            if (level < 4 && refName != null)
                headingParagraph.AddBookmark(refName);
        }

        public void StartParagraph()
        {
            if (!m_codeBlockOpen)
            {
                string style;
                switch (m_numOpenLists)
                {
                    case 1: style = StyleNormal1; break;
                    case 2: style = StyleNormal2; break;
                    default: style = StyleNormal; break;
                }
                m_document?.LastSection.AddParagraph("", style);
            }
        }

        int m_numOpenLists = 0;
        int m_openListLevel = 0;

        public void AddListItem(int level)
        {
            ListInfo listInfo = new ListInfo();

            listInfo.ContinuePreviousList = m_numOpenLists>0 && level == m_openListLevel;

            if (level > m_openListLevel || m_numOpenLists == 0)
            {
                m_numOpenLists++;
                m_openListLevel = level;
            }

            listInfo.ListType = ListType.BulletList1;

            string style;
            switch(m_numOpenLists)
            {
                case 1: style = StyleList1; break;
                case 2: style = StyleList2; break;
                default: style = StyleList3; break;
            }
            Paragraph paragraph= m_document?.LastSection.AddParagraph("", style);
            paragraph.Format.ListInfo = listInfo;
        }

        public void StartNote(int level)
        {
            m_document?.LastSection.AddParagraph("", StyleNote);
        }

        bool m_codeBlockOpen = false;
        public void ToggleCodeBlock(int level)
        {
            if (!m_codeBlockOpen)
                m_document?.LastSection.AddParagraph("", StyleCode);
            m_codeBlockOpen = !m_codeBlockOpen;
        }


        public void AddTextToLastParagraph(string text)
        {
            if (!m_codeBlockOpen )
                m_document?.LastSection.LastParagraph.AddText(text);
            else m_document?.LastSection.LastParagraph.AddText(text + "\n");
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
            m_document?.LastSection.LastParagraph.AddFormattedText(text, TextFormat.Italic);
        }

        int GetImageWidth(string filename)
        {
            int width;
            using (XImage image2 = XImage.FromFile(filename))
            {
                width = image2.PixelWidth;
            }
            return width;
        }

        public void AddInlineImageToLastParagraph(string filename)
        {
            MigraDoc.DocumentObjectModel.Shapes.Image image;
            if (!filename.EndsWith(".svg"))
            {
                image = m_document.LastSection.LastParagraph.AddImage(filename);
                int width = GetImageWidth(filename);
                image.LockAspectRatio = true;
                image.Width = width;
            }
        }

        public void AddImage(string filename)
        {
            MigraDoc.DocumentObjectModel.Shapes.Image image;
            if (!filename.EndsWith(".svg"))
            {
                image = m_document.LastSection.AddParagraph("", StyleImage).AddImage(filename);
                Unit maxWidth = "14cm";
                Unit width = GetImageWidth(filename);
                image.LockAspectRatio = true;
                image.Width = Math.Min(width.Centimeter, maxWidth.Centimeter) + "cm";
            }
        }

        int hyperLinkCount = 0;
        public void AddLinkToLastParagraph(string text, string link)
        {
            hyperLinkCount++;
            string hyperLinkName = "hyperlink-" + hyperLinkCount;
            Hyperlink hyperlink;
            if (!link.StartsWith(".") && !link.StartsWith("http:"))
            {
                //reference to a document converted to section
                hyperlink = m_document.LastSection.LastParagraph.AddHyperlink(link);
                hyperlink.AddText(text);
                //hyperlink.AddPageRefField(link);
            }
            else if (link.StartsWith("http:"))
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
                Console.WriteLine("ERROR: " + e.ToString());
            }
        }
    }
}
