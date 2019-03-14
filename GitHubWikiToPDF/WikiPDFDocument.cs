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
        //public static void DefineParagraphs(Document document)
        //{
        //    Paragraph paragraph = document.LastSection.AddParagraph("Paragraph Layout Overview", "Heading1");
        //    paragraph.AddBookmark("Paragraphs");

        //    DemonstrateAlignment(document);
        //    DemonstrateIndent(document);
        //    DemonstrateFormattedText(document);
        //    DemonstrateBordersAndShading(document);
        //}

        //static void DemonstrateAlignment(Document document)
        //{
        //    document.LastSection.AddParagraph("Alignment", "Heading2");

        //    document.LastSection.AddParagraph("Left Aligned", "Heading3");

        //    Paragraph paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Alignment = ParagraphAlignment.Left;
        //    paragraph.AddText(Text);

        //    document.LastSection.AddParagraph("Right Aligned", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Alignment = ParagraphAlignment.Right;
        //    paragraph.AddText(Text);

        //    document.LastSection.AddParagraph("Centered", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Alignment = ParagraphAlignment.Center;
        //    paragraph.AddText(Text);

        //    document.LastSection.AddParagraph("Justified", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Alignment = ParagraphAlignment.Justify;
        //    paragraph.AddText(Text);
        //}

        //static void DemonstrateIndent(Document document)
        //{
        //    document.LastSection.AddParagraph("Indent", "Heading2");

        //    document.LastSection.AddParagraph("Left Indent", "Heading3");

        //    Paragraph paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.LeftIndent = "2cm";
        //    paragraph.AddText(Text);

        //    document.LastSection.AddParagraph("Right Indent", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.RightIndent = "1in";
        //    paragraph.AddText(Text);

        //    document.LastSection.AddParagraph("First Line Indent", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.FirstLineIndent = "12mm";
        //    paragraph.AddText(Text);

        //    document.LastSection.AddParagraph("First Line Negative Indent", "Heading3");

        //    //MigraDoc.DocumentObjectModel.Shapes.Image image = document.LastSection.AddImage("SVG-image.svg");
        //    //image.Width = "10cm";

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.LeftIndent = "1.5cm";
        //    paragraph.Format.FirstLineIndent = "-1.5cm";
        //    paragraph.AddText(Text);
        //}

        //static void DemonstrateFormattedText(Document document)
        //{
        //    document.LastSection.AddParagraph("Formatted Text", "Heading2");

        //    Paragraph paragraph = document.LastSection.AddParagraph();
        //    paragraph.AddText("Text can be formatted ");
        //    paragraph.AddFormattedText("bold", TextFormat.Bold);
        //    paragraph.AddText(", ");
        //    paragraph.AddFormattedText("italic", TextFormat.Italic);
        //    paragraph.AddText(", or ");
        //    paragraph.AddFormattedText("bold & italic", TextFormat.Bold | TextFormat.Italic);
        //    paragraph.AddText(".");
        //    paragraph.AddLineBreak();
        //    paragraph.AddText("You can set the ");
        //    FormattedText formattedText = paragraph.AddFormattedText("size ");
        //    formattedText.Size = 15;
        //    paragraph.AddText("the ");
        //    formattedText = paragraph.AddFormattedText("color ");
        //    formattedText.Color = Colors.Firebrick;
        //    paragraph.AddText("the ");
        //    formattedText = paragraph.AddFormattedText("font", new MigraDoc.DocumentObjectModel.Font("Verdana"));
        //    paragraph.AddText(".");
        //    paragraph.AddLineBreak();
        //    paragraph.AddText("You can set the ");
        //    formattedText = paragraph.AddFormattedText("subscript");
        //    formattedText.Subscript = true;
        //    paragraph.AddText(" or ");
        //    formattedText = paragraph.AddFormattedText("superscript");
        //    formattedText.Superscript = true;
        //    paragraph.AddText(".");
        //}

        //static void DemonstrateBordersAndShading(Document document)
        //{
        //    document.LastSection.AddPageBreak();
        //    document.LastSection.AddParagraph("Borders and Shading", "Heading2");

        //    document.LastSection.AddParagraph("Border around Paragraph", "Heading3");

        //    Paragraph paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Borders.Width = 2.5;
        //    paragraph.Format.Borders.Color = Colors.Navy;
        //    paragraph.Format.Borders.Distance = 3;
        //    paragraph.AddText(Text);

        //    document.LastSection.AddParagraph("Shading", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Format.Shading.Color = Colors.LightCoral;
        //    paragraph.AddText(Text);

        //    document.LastSection.AddParagraph("Borders & Shading", "Heading3");

        //    paragraph = document.LastSection.AddParagraph();
        //    paragraph.Style = "TextBox";
        //    paragraph.AddText(Text);
        //}

        //public static void DefineCover(Document document)
        //{
        //    Section section = document.AddSection();

        //    Paragraph paragraph = section.AddParagraph();
        //    paragraph.Format.SpaceAfter = "3cm";

        //    MigraDoc.DocumentObjectModel.Shapes.Image image = section.AddImage("PNG-image.png");
        //    image.WrapFormat.Style = WrapStyle.TopBottom;
        //    image.LockAspectRatio = true;

        //    paragraph = section.AddParagraph("A sample document that demonstrates the\ncapabilities of MigraDoc");
        //    paragraph.Format.Font.Size = 16;
        //    paragraph.Format.Font.Color = Colors.DarkRed;
        //    paragraph.Format.SpaceBefore = "8cm";
        //    paragraph.Format.SpaceAfter = "3cm";

        //    paragraph = section.AddParagraph("Rendering date: ");
        //    paragraph.AddDateField();
        //}
        //public static void DefineTableOfContents(Document document)
        //{
        //    Section section = document.LastSection;

        //    section.AddPageBreak();
        //    Paragraph paragraph = section.AddParagraph("Table of Contents");
        //    paragraph.Format.Font.Size = 14;
        //    paragraph.Format.Font.Bold = true;
        //    paragraph.Format.SpaceAfter = 24;
        //    paragraph.Format.OutlineLevel = OutlineLevel.Level1;

        //    paragraph = section.AddParagraph();
        //    paragraph.Style = "TOC";
        //    Hyperlink hyperlink = paragraph.AddHyperlink("Paragraphs");
        //    hyperlink.AddText("Paragraphs\t");
        //    hyperlink.AddPageRefField("Paragraphs");

        //    paragraph = section.AddParagraph();
        //    paragraph.Style = "TOC";
        //    hyperlink = paragraph.AddHyperlink("Tables");
        //    hyperlink.AddText("Tables\t");
        //    hyperlink.AddPageRefField("Tables");

        //    paragraph = section.AddParagraph();
        //    paragraph.Style = "TOC";
        //    hyperlink = paragraph.AddHyperlink("Charts");
        //    hyperlink.AddText("Charts\t");
        //    hyperlink.AddPageRefField("Charts");
        //}
        
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
            header.AddParagraph("\ttitle");

            header = section.Headers.EvenPage;
            header.AddParagraph("title");

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

        void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = m_document.Styles["Normal"];
            style.Font.Name = "Verdana";
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";
            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.
            style = m_document.Styles[StyleNames.Heading1];
            style.Font.Name = "Verdana";
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Color = Colors.LightSkyBlue;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;
            style = m_document.Styles[StyleNames.Heading2];
            style.Font.Size = 13;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;
            style = m_document.Styles[StyleNames.Heading3];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style = m_document.Styles[StyleNames.Heading4];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style = m_document.Styles[StyleNames.Heading5];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;
            style = m_document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style = m_document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
            // Create a new style called TextBox based on style Normal
            style = m_document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = Colors.Black;
            // Create a new style called TOC based on style Normal
            style = m_document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Black;
        }

        public void AddHeader(string text, int level)
        {
            string style = "Heading" + level;
            m_document.LastSection.AddParagraph(text, style);
        }

        public void AddParagraph(string text)
        {
            string style = StyleNames.Normal;
            m_document.LastSection.AddParagraph(text, style);
        }

        public void AddListItem(string text, int level)
        {
            //m_document.LastSection.Ad
        }

        public void AddFormattedTextToLastParagraph(string text, TextFormat format)
        {
            m_document.LastSection.LastParagraph.AddFormattedText(text, format);
        }

        public void Save(string filename)
        {
            MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(m_document, "MigraDoc.mdddl");
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = m_document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
        }
    }
}
