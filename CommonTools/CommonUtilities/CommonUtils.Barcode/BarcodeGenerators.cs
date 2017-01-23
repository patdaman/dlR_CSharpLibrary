using System;
using System.Text;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Web;
using Rapid_e4473.Models;

namespace Rapid_e4473.HelperClasses
{
    public class barcodecs
    {
        public string generateBarcode()
        {
            try
            {
                string[] charPool = "1-2-3-4-5-6-7-8-9-0".Split('-');
                StringBuilder rs = new StringBuilder();
                int length = 10;
                Random rnd = new Random();
                while (length-- > 0)
                {
                    int index = (int)(rnd.NextDouble() * charPool.Length);
                    if (charPool[index] != "-")
                    {
                        rs.Append(charPool[index]);
                        charPool[index] = "-";
                    }
                    else
                        length++;
                }
                return rs.ToString();
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
            }
            return "";
        }
    }

    public class getBarcodeObjects
    {
        public string getBarcode()
        {
            // Check for BARCODE already loaded with user
            // Create new BARCODE if user barcode is empty
            // Checking for barcode already existing equal to the randomly generated number
            barcodecs objbar = new barcodecs();
            var bc = objbar.generateBarcode();
            var loop = 0;
            using (RAPID_e4473Entities db = new RAPID_e4473Entities())
            {
                while (loop == 0)
                {
                    var dbbc = (from c in db.CUSTOMERS
                                where c.BARCODE == bc
                                select c).SingleOrDefault();
                    if (dbbc == null)
                    {
                        loop = 1;
                    }
                    else
                    {
                        bc = objbar.generateBarcode();
                    }
                }
            }
            return bc.ToString();
        }

        public byte[] getBarcodeImage(string custBarcode)
        {
            // BarcodeLib.dll (Barcode) Solution
            BarcodeLib.Barcode newbarcode = new BarcodeLib.Barcode()
            {
                IncludeLabel = false,
                Alignment = BarcodeLib.AlignmentPositions.LEFT,
                Width = 450,
                Height = 100,
                RotateFlipType = System.Drawing.RotateFlipType.RotateNoneFlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            };
            Image image = newbarcode.Encode(BarcodeLib.TYPE.CODE39, custBarcode);
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}