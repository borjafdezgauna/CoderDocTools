using MigraDoc.DocumentObjectModel;


namespace MarkdownToPDF
{
    class Styler
    {
        public const string StyleHeading1 = StyleNames.Heading1;
        public const string StyleHeading2 = StyleNames.Heading2;
        public const string StyleHeading3 = StyleNames.Heading3;
        public const string StyleHeading4 = StyleNames.Heading4;
        public const string StyleHeading5 = StyleNames.Heading5;
        public const string StyleNormal = StyleNames.Normal;
        public const string StyleLink = StyleNames.Hyperlink;
        public const string StyleList1 = StyleNames.List + "1";
        public const string StyleList2 = StyleNames.List + "2";
        public const string StyleList3 = StyleNames.List + "3";
        public const string StyleParagraphInList1 = "ParagraphInList1";
        public const string StyleParagraphInList2 = "ParagraphInList2";
        public const string StyleParagraphInList3 = "ParagraphInList3";
        public const string StyleHeader = StyleNames.Header;
        public const string StyleFooter = StyleNames.Footer;
        public const string StyleNote = "Note";
        public const string StyleCode = "Code";
        public const string StyleInlineCode = "InlineCode";
        public const string StyleImage = "Image";
        public const string StyleHyperlink = StyleNames.Hyperlink;
        public const string StyleCoverTitle = "CoverTitle";
        public const string StyleCoverSubTitle = "CoverSubTitle";

        public static void DefineStyles(Document document)
        {
            //Normal
            Style style = document.Styles["Normal"];
            style.Font.Name = FontManager.RegularFont;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.25);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.25);

            //Heading 1
            style = document.Styles[StyleHeading1];
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.5);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0);
            style.Font.Size = 20;
            style.Font.Bold = true;
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.PageBreakBefore = true;

            //Heading 2
            style = document.Styles[StyleHeading2];
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.4);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.5);
            style.Font.Size = 16;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;

            //Heading 3
            style = document.Styles[StyleHeading3];
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.4);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.5);
            style.Font.Size = 14;
            style.Font.Bold = true;

            //Heading 4
            style = document.Styles[StyleHeading4];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = true;

            //Heading 5
            style = document.Styles[StyleHeading5];
            style.ParagraphFormat.SpaceAfter = Unit.FromCentimeter(0.4);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.5);
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;

            //Hyperlinks
            Color HyperlinkColor = Color.FromRgb(8, 46, 233);
            style = document.Styles[StyleHyperlink];
            style.Font.Color = HyperlinkColor;

            //List - level 1
            style = document.AddStyle(StyleList1, StyleNormal);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.25);
            style.ParagraphFormat.FirstLineIndent = Unit.FromCentimeter(-0.2);

            //List - level 2
            style = document.AddStyle(StyleList2, StyleList1);
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level2;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.5);

            //List - level 3
            style = document.AddStyle(StyleList3, StyleList1);
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.75);

            //Paragraph in list item - level 1
            style = document.AddStyle(StyleParagraphInList1, StyleList1);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.28);
            style.ParagraphFormat.FirstLineIndent = Unit.FromCentimeter(0);
            //Paragraph in list item - level 2
            style = document.AddStyle(StyleParagraphInList2, StyleList2);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.53);
            style.ParagraphFormat.FirstLineIndent = Unit.FromCentimeter(0);
            //Paragraph in list item - level 3
            style = document.AddStyle(StyleParagraphInList3, StyleList3);
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.78);
            style.ParagraphFormat.FirstLineIndent = Unit.FromCentimeter(0);

            //Page Header
            style = document.Styles[StyleHeader];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);
            style.ParagraphFormat.Borders.Bottom.Color = Colors.DarkGray;
            style.ParagraphFormat.Borders.Top.Visible = false;
            style.ParagraphFormat.Borders.Left.Visible = false;
            style.ParagraphFormat.Borders.Right.Visible = false;
            style.ParagraphFormat.Borders.Width = 0.5;
            //Page Footer
            style = document.Styles[StyleFooter];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);
            style.ParagraphFormat.Borders.Top.Color = Colors.DarkGray;
            style.ParagraphFormat.Borders.Bottom.Visible = false;
            style.ParagraphFormat.Borders.Left.Visible = false;
            style.ParagraphFormat.Borders.Right.Visible = false;

            //Add our own styles needed for markdown files
            Color codeBackground = new Color(226, 226, 226);
            Color codeBorder = new Color(160, 160, 160);

            //Code
            style = document.Styles.AddStyle(StyleCode, StyleNormal);
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.SpaceBefore = "0.5cm";
            style.ParagraphFormat.SpaceAfter = "0.5cm";
            style.ParagraphFormat.Borders.Width = 0.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Borders.Color = codeBorder;
            style.Font.Name = FontManager.CodeFont;
            style.ParagraphFormat.Shading.Color = codeBackground;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.5);
            style.ParagraphFormat.RightIndent = Unit.FromCentimeter(0.5);

            //Inline code
            Color InlineCodeColor = Color.FromRgb(50, 0, 50);
            style = document.Styles.AddStyle(StyleInlineCode, StyleCode);
            style.Font.Color = InlineCodeColor;
            style.Font.Name = FontManager.RegularFont;

            //Note
            Color noteColor = new Color(80, 80, 80);
            style = document.Styles.AddStyle(StyleNote, StyleNormal);
            style.Font.Name = FontManager.RegularFont;
            style.Font.Italic = true;
            style.Font.Color = noteColor;
            style.ParagraphFormat.LeftIndent = Unit.FromCentimeter(0.5);
            style.ParagraphFormat.RightIndent = Unit.FromCentimeter(0.5);

            //Image
            style = document.Styles.AddStyle(StyleImage, StyleNormal);
            style.ParagraphFormat.SpaceBefore = "1cm";
            style.ParagraphFormat.SpaceAfter = "1cm";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            //Cover title
            style = document.Styles.AddStyle(StyleCoverTitle, StyleNormal);
            style.Font.Size = 42;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.Borders.Bottom.Color = Colors.DarkGray;
            style.ParagraphFormat.Borders.Top.Visible = false;
            style.ParagraphFormat.Borders.Left.Visible = false;
            style.ParagraphFormat.Borders.Right.Visible = false;
            style.ParagraphFormat.Borders.Width = 0.5;
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(4);

            //Cover subtitle
            style = document.Styles.AddStyle(StyleCoverSubTitle, StyleNormal);
            style.ParagraphFormat.SpaceBefore = Unit.FromCentimeter(0.4);
        }
    }
}
