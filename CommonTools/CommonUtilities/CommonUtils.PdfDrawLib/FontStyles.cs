using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.PdfDrawLib
{
    public class FontStyles
    {

        public static XFontStyle GetXFontStyle(string fontStyle)
        {
            switch (fontStyle)
            {
                case "Regular":
                    return XFontStyle.Regular;
                    break;
                case "Bold":
                    return XFontStyle.Bold;
                    break;
                case "Italic":
                    return XFontStyle.Italic;
                    break;
                case "BoldItalic":
                    return XFontStyle.BoldItalic;
                    break;
                case "Underline":
                    return XFontStyle.Underline;
                    break;
                case "Strikeout":
                    return XFontStyle.Strikeout;
                    break;
                default:
                    throw new Exception("Unknown font style.");
            }

        }
    }
}
