using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.PdfDrawLib
{
    //base
    public class DrawActionsBase
    {
    }

    public class DrawTextAction : DrawActionsBase
    {
        public string Text { get; set; }
        public string FontName { get; set; }
        public string FontStyle { get; set; }
        public string FontColor { get; set; }
        public double? FontSize { get; set; }
        public double XLocationPts { get; set; }
        public double YLocationPts { get; set; }
        public double? WordWrapBoxWidth { get; set; }
        public double ? WordWrapBoxHeight { get; set; }

        public XStringFormat Alignment { get; set; }

        public DrawTextAction(double xLocationPts,
        double yLocationPts,
        string text=null,
        string fontName=null,
        string fontStyle=null,
        string fontColor=null,
        double? fontSize=null,
        double? wordWrapBoxWidth=null,
        double? wordWrapBoxHeight=null,
        XStringFormat alignment=null
            )
        {
            XLocationPts = xLocationPts;
            YLocationPts = yLocationPts;
            Text = text;
            FontName = fontName;
            FontStyle = fontStyle;
            FontColor = fontColor;
            FontSize = fontSize;
            WordWrapBoxWidth = wordWrapBoxWidth;
            WordWrapBoxHeight = wordWrapBoxHeight;
            Alignment = alignment;
        }
    }

    


    public class DrawImageAction : DrawActionsBase
    {
        public double XLocationPts { get; set; }
        public double YLocationPts { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Stream Image { get; set; }

        public DrawImageAction(double xLocationPts, double yLocationPts, double width, double height,
            Stream image)
        {
            XLocationPts = xLocationPts;
            YLocationPts = yLocationPts;
            Width = width;
            Height = height;
            Image = image;
        }

    }
}
