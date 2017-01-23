using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.PdfDrawLib
{
    public class LineAlignments
    {
        public const string Near = "Near";
        public const string Center = "Center";
        public const string Far = "Far";
        public const string BaseLine = "BaseLine";

        public static XLineAlignment GetLineAlignment(string alignment)
        {
            switch (alignment)
            {
                case "Near":
                    return XLineAlignment.Near;
                case "Center":
                    return XLineAlignment.Center;
                case "Far":
                    return XLineAlignment.Far;
                case "BaseLine":
                    return XLineAlignment.BaseLine;
                default:
                    throw new Exception("Unknown alignment");
            }

        }




    }
}
