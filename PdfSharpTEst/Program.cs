using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PdfSharpTest
{
    class Program
    {
        //public class LayoutHelper
        //{
        //    private readonly PdfDocument _document;
        //    private readonly XUnit _topPosition;
        //    private readonly XUnit _bottomMargin;
        //    private XUnit _currentPosition;

        //    public LayoutHelper(PdfDocument document, XUnit topPosition, XUnit bottomMargin)
        //    {
        //        _document = document;
        //        _topPosition = topPosition;
        //        _bottomMargin = bottomMargin;
        //        // Set a value outside the page - a new page will be created on the first request.
        //        _currentPosition = bottomMargin + 10000;
        //    }

        //    public XUnit GetLinePosition(XUnit requestedHeight)
        //    {
        //        return GetLinePosition(requestedHeight, -1f);
        //    }

        //    public XUnit GetLinePosition(XUnit requestedHeight, XUnit requiredHeight)
        //    {
        //        XUnit required = requiredHeight == -1f ? requestedHeight : requiredHeight;
        //        if (_currentPosition + required > _bottomMargin)
        //            CreatePage();
        //        XUnit result = _currentPosition;
        //        _currentPosition += requestedHeight;
        //        return result;
        //    }

        //    public XGraphics Gfx { get; private set; }
        //    public PdfPage Page { get; private set; }

        //    void CreatePage()
        //    {
        //        Page = _document.AddPage();
        //        Page.Size = PageSize.A4;
        //        Gfx = XGraphics.FromPdfPage(Page);
        //        _currentPosition = _topPosition;
        //    }
        //}


        static void Main(string[] args)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            PageSize size = page.Size;

            // Create a font
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont header1Font = new XFont("Helvetica", 28, XFontStyle.Bold);
            XFont header2Font = new XFont("Times New Roman", 24, XFontStyle.Bold);
            XFont header3Font = new XFont("Times New Roman", 18, XFontStyle.Bold);
            XFont header4Font = new XFont("Times New Roman", 16, XFontStyle.Bold);
            XFont regularFont = new XFont("Times New Roman", 12, XFontStyle.Regular);

            XTextFormatter tf = new XTextFormatter(gfx);

            XRect rect = new XRect(0, 0, page.Width, page.Height);

            string subsectionText = "";
            for (int i = 0; i < 4; i++)
            {
                subsectionText += "My text is so interesting you will not believe it";
            }
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString("1. First section", header1Font, XBrushes.DarkBlue, rect, XStringFormats.TopLeft);
            tf.DrawString("1.1 First subsection of section 1", header2Font, XBrushes.BlueViolet, rect, XStringFormats.TopLeft);
            tf.DrawString(subsectionText, regularFont, XBrushes.Black,rect, XStringFormats.TopLeft);

            document.Save("output.pdf");


            //string text = "Facin exeraessisit la consenim iureet dignibh eu facilluptat vercil dunt autpat. " +
            //"Ecte magna faccum dolor sequisc iliquat, quat, quipiss equipit accummy niate magna " +
            //"facil iure eraesequis am velit, quat atis dolore dolent luptat nulla adio odipissectet " +
            //"lan venis do essequatio conulla facillandrem zzriusci bla ad minim inis nim velit eugait " +
            //"aut aut lor at ilit ut nulla ate te eugait alit augiamet ad magnim iurem il eu feuissi.\n" +
            //"Guer sequis duis eu feugait luptat lum adiamet, si tate dolore mod eu facidunt adignisl in " +
            //"henim dolorem nulla faccum vel inis dolutpatum iusto od min ex euis adio exer sed del " +
            //"dolor ing enit veniamcon vullutat praestrud molenis ciduisim doloborem ipit nulla consequisi.\n" +
            //"Nos adit pratetu eriurem delestie del ut lumsandreet nis exerilisit wis nos alit venit praestrud " +
            //"dolor sum volore facidui blaor erillaortis ad ea augue corem dunt nis  iustinciduis euisi.\n" +
            //"Ut ulputate volore min ut nulpute dolobor sequism olorperilit autatie modit wisl illuptat dolore " +
            //"min ut in ute doloboreet ip ex et am dunt at.";

            //PdfDocument document = new PdfDocument();
            //PdfPage page = document.AddPage();
            //PageSize size = page.Size;

            //// Create a font
            //XGraphics gfx = XGraphics.FromPdfPage(page);

            //XFont font = new XFont("Times New Roman", 10, XFontStyle.Bold);

            //XTextFormatter tf = new XTextFormatter(gfx);

            //XRect rect = new XRect(0, 0, page.Width, page.Height);

            ////gfx.DrawRectangle(XBrushes.SeaShell, rect);

            //tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            //tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);


            //document.Save("output.pdf");
            //  PdfDocument document = new PdfDocument();

            //  Sample uses DIN A4, page height is 29.7 cm.We use margins of 2.5 cm.
            //LayoutHelper helper = new LayoutHelper(document, XUnit.FromCentimeter(2.5), XUnit.FromCentimeter(29.7 - 2.5));
            //  XUnit left = XUnit.FromCentimeter(2.5);

            //  Random generator with seed value, so created document will always be the same.
            //  Random rand = new Random(42);

            //  const int headerFontSize = 20;
            //  const int normalFontSize = 10;

            //  XFont fontHeader = new XFont("Verdana", headerFontSize, XFontStyle.BoldItalic);
            //  XFont fontNormal = new XFont("Verdana", normalFontSize, XFontStyle.Regular);

            //  const int totalLines = 666;
            //  bool washeader = false;
            //  for (int line = 0; line < totalLines; ++line)
            //  {
            //      bool isHeader = line == 0 || !washeader && line < totalLines - 1 && rand.Next(15) == 0;
            //      washeader = isHeader;
            //      We do not want a single header at the bottom of the page, so if we have a header we require space for header and a normal text line.

            //     XUnit top = helper.GetLinePosition(isHeader ? headerFontSize + 5 : normalFontSize + 2, isHeader ? headerFontSize + 5 + normalFontSize : normalFontSize);

            //      helper.Gfx.DrawString(isHeader ? "Sed massa libero, semper a nisi nec" : "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            //          isHeader ? fontHeader : fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
            //  }

            //  Save the document...
            //  const string filename = "MultiplePages.pdf";
            //  document.Save(filename);
            //   ...and start a viewer.
            //  Process.Start(filename);
        }
    }
}
