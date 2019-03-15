using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
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
            header.AddParagraph(title);

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
        const string StyleLink = StyleNames.Hyperlink;
        const string StyleList = StyleNames.List;
        const string StyleHeader = StyleNames.Header;
        const string StyleFooter = StyleNames.Footer;
        const string StyleNote = "Note";
        const string StyleCode = "Code";

        void DefineStyles()
        {
            Style style = m_document.Styles["Normal"];
            style.Font.Name = "Verdana";
            style = m_document.Styles[StyleHeading1];
            style.Font.Size = 18;
            style.Font.Bold = true;
            style.Font.Color = Colors.AliceBlue;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;
            style = m_document.Styles[StyleHeading2];
            style.Font.Size = 14;
            style.Font.Color = Colors.BlueViolet;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;
            style = m_document.Styles[StyleHeading3];
            style.Font.Color = Colors.DarkMagenta;
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style = m_document.Styles[StyleHeading4];
            style.Font.Color = Colors.Purple;
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style = m_document.Styles[StyleHeading5];
            style.Font.Color = Colors.MidnightBlue;
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;

            style = m_document.Styles[StyleHeader];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style = m_document.Styles[StyleFooter];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            //Add our own styles needed for markdown files
            style = m_document.Styles.AddStyle(StyleCode, StyleNormal);
            style.Font.Color = Colors.Red;
            style = m_document.Styles.AddStyle(StyleNote, StyleNormal);
            style.Font.Color = Colors.Purple;
            style = m_document.Styles.AddStyle(StyleList, StyleNormal);
            style.ParagraphFormat.LeftIndent = "0.5cm";
        }

        public void StartHeader(int level)
        {
            //Close all open lists
            m_numOpenLists = 0;
            m_openListLevel = 0;
            m_document?.LastSection.AddParagraph("", "Heading" + level);
        }

        public void StartParagraph()
        {
            m_document?.LastSection.AddParagraph("", StyleNormal);
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

            listInfo.ListType = ListType.BulletList3;

            Paragraph paragraph= m_document?.LastSection.AddParagraph("", StyleList);
            paragraph.Format.ListInfo = listInfo;
        }

        public void StartNote(int level)
        {
            m_document?.LastSection.AddParagraph("", StyleNote); //TODO: FIX this
        }

        bool m_codeBlockOpen = false;
        public void ToggleCodeBlock(int level)
        {
            if (m_codeBlockOpen)
                m_document?.LastSection.AddParagraph("", StyleCode);
            m_codeBlockOpen = !m_codeBlockOpen;
        }


        public void AddTextToLastParagraph(string text)
        {
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
            m_document?.LastSection.LastParagraph.AddFormattedText(text, TextFormat.Italic);
        }

        public void AddInlineImageToLastParagraph(string filename)
        {
            MigraDoc.DocumentObjectModel.Shapes.Image image;
            if (!filename.EndsWith(".svg"))
            {
                image = m_document.LastSection.LastParagraph.AddImage(filename);
            }
        }

        public void AddImage(string filename)
        {
            MigraDoc.DocumentObjectModel.Shapes.Image image;
            if (!filename.EndsWith(".svg"))
            {
                image = m_document.LastSection.AddParagraph().AddImage(filename);
                image.Width = "14cm";
            }
        }

        public void AddLinkToLastParagraph(string text, string link)
        {

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
