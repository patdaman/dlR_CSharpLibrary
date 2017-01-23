using CommonUtils.Logging;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.PdfDrawLib
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A utility to consilidate all PDF drawing functionality.  </summary>
    ///
    /// <remarks>   Dtorres, 20160830. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public partial class PdfDrawer
    {

        public static LoggingPatternUser Logger = new LoggingPatternUser() { Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType) };

        #region  Default font settings
        public string DefaultFontName { get; set; }
        public string DefaultFontStyle { get; set; }
        public string DefaultFontColor { get; set; }
        public double? DefaultFontSize { get; set; }
        #endregion

        public PdfDrawer()
        {

        }


        public void Draw(PdfDocument pdf, int pageNumber, DrawActionsBase drawAction)
        {
            Draw(pdf.Pages[pageNumber], drawAction);
        }

        public void Draw(PdfDocument pdf, int pageNumber, List<DrawActionsBase> drawActions)
        {
            Draw(pdf.Pages[pageNumber], drawActions);
        }

        public void DrawOnAllPages(PdfDocument pdf, List<DrawActionsBase> drawActions)
        {
            for (int iPage = 0; iPage < pdf.Pages.Count; iPage++)
            {
                PdfPage page = pdf.Pages[iPage];
                Draw(page, drawActions);
            }
        }

        public void Draw(PdfPage pdfPage, List<DrawActionsBase> actionList)
        {
            foreach( var action in actionList)
            {
                Draw(pdfPage, action);
            }
        }
        public void Draw(PdfPage pdfPage, DrawActionsBase action)
        {
            if (pdfPage == null || action == null)
                throw new ArgumentNullException();

            if (action is DrawTextAction)
            {
                DrawText(pdfPage, (DrawTextAction)action);
            }
            else if (action is DrawImageAction)
            {
                DrawImage(pdfPage, (DrawImageAction)action);
            }
            else
            {
                Logger.Error($"Unknown action encountered. Not rendering anything for this action.");
            }
        }

        private void DrawImage(PdfPage pdfPage, DrawImageAction action)
        {
            using (XGraphics xg = XGraphics.FromPdfPage(pdfPage))
            using (var image = XImage.FromStream(action.Image))
            {
                xg.DrawImage(image,
                    action.XLocationPts,
                    action.YLocationPts,
                    action.Width,
                    action.Height);
            }
        }

        public XSize MeasureText(PdfPage pdfPage, DrawTextAction action)
        {
            //Check for errors 
            if (action.FontName == null && DefaultFontName == null)
                throw new Exception("Error value or default or otherwise for font name.");
            if (action.FontSize == null && DefaultFontSize == null)
                throw new Exception("Error value or default or otherwise for font size.");
            if (action.FontStyle == null && DefaultFontStyle == null)
                throw new Exception("Error value or default or otherwise for font style.");
            if (action.FontColor == null && DefaultFontColor == null)
                throw new Exception("Error value or default or otherwise for font color.");
            if (action.WordWrapBoxWidth != null || action.WordWrapBoxHeight != null)
                throw new Exception("Error, cannot specify word wrap when measuring text.");

            XSize size;
            using (XGraphics xg = XGraphics.FromPdfPage(pdfPage))
            {

                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);

                XFont xFont = new XFont(
                    action.FontName ?? DefaultFontName,
                    action.FontSize ?? DefaultFontSize.Value,
                    FontStyles.GetXFontStyle(action.FontStyle ?? DefaultFontStyle),
                    options);

                XSolidBrush brushColor = BrushNames.GetBrush(action.FontColor ?? DefaultFontColor);

                size = xg.MeasureString(action.Text, xFont);
            }

            return size;
        }

        private void DrawText(PdfPage pdfPage, DrawTextAction action)
        {
            //Check for errors 
            if (action.FontName == null && DefaultFontName == null)
                throw new Exception("Error value or default or otherwise for font name.");
            if (action.FontSize == null && DefaultFontSize == null)
                throw new Exception("Error value or default or otherwise for font size.");
            if (action.FontStyle == null && DefaultFontStyle == null)
                throw new Exception("Error value or default or otherwise for font style.");
            if (action.FontColor == null && DefaultFontColor == null)
                throw new Exception("Error value or default or otherwise for font color.");
            if (action.WordWrapBoxWidth != null ^ action.WordWrapBoxHeight != null)
                throw new Exception("Did not specify both word wrap box width and height parameters.");

            if (string.IsNullOrEmpty(action.Text) == false)
            {
                //No word wrap being used 
                if (action.WordWrapBoxWidth == null)
                {
                    using (XGraphics xg = XGraphics.FromPdfPage(pdfPage))
                    {

                        XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);

                        XFont xFont = new XFont(
                            action.FontName ?? DefaultFontName,
                            action.FontSize ?? DefaultFontSize.Value,
                            FontStyles.GetXFontStyle(action.FontStyle ?? DefaultFontStyle),
                            options);

                        XSolidBrush brushColor = BrushNames.GetBrush(action.FontColor ?? DefaultFontColor);

                        xg.DrawString(action.Text,
                            xFont,
                            brushColor,
                            action.XLocationPts,
                            action.YLocationPts);
                    }
                }
                //word wrap is being used 
                else
                {
                    using (XGraphics xg = XGraphics.FromPdfPage(pdfPage))
                    {

                        XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);

                        XFont xFont = new XFont(
                            action.FontName ?? DefaultFontName,
                            action.FontSize ?? DefaultFontSize.Value,
                            FontStyles.GetXFontStyle(action.FontStyle ?? DefaultFontStyle),
                            options);

                        XSolidBrush brushColor = BrushNames.GetBrush(action.FontColor ?? DefaultFontColor);

                        XRect rect = new XRect(action.XLocationPts, action.YLocationPts,
                            action.WordWrapBoxWidth.Value, action.WordWrapBoxHeight.Value);

                        XTextFormatter tf = new XTextFormatter(xg);
                        tf.DrawString(action.Text,
                            xFont,
                            brushColor,
                            rect,
                            XStringFormats.TopLeft
                            );
                    }
                }
            }//end if 
        }//end function

    }
}
