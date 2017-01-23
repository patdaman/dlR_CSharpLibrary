using CommonUtils.PdfCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace CommonUtils.PdfPdfSharp
{
    public class PdfSharpPdfManipulator : IPdfManipulator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Flatten multiple PDF documents into a single document. </summary>
        ///
        /// <remarks>   Dtorres, 20160520. </remarks>
        ///
        /// <param name="documentlist"> The documentlist. </param>
        ///
        /// <returns>   A byte[]. </returns>
        ///-------------------------------------------------------------------------------------------------
        public MemoryStream FlattenDocuments(IEnumerable<byte[]> documentlist)
        {
            Debug.Assert(documentlist != null);
            if (documentlist == null)
                throw new ArgumentNullException();

            MemoryStream outpuStream = new MemoryStream();                        
            using (PdfDocument finalPdf = new PdfDocument())
            {
                foreach (byte[] documentBytes in documentlist)
                {
                    MemoryStream stream = new MemoryStream(documentBytes);
                    using (PdfDocument document = PdfReader.Open(stream, PdfDocumentOpenMode.Import))
                    {
                        for (int i = 0; i < document.PageCount; i++)
                            finalPdf.AddPage(document.Pages[i]);
                    }
                }                
                finalPdf.Save(outpuStream);
            }
            return outpuStream;
        }

        public int GetPageCount(byte[] documentBytes)
        {
            MemoryStream stream = new MemoryStream(documentBytes);
            PdfDocument document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
            int count = document.PageCount;
            document.Dispose();
            return count; 
        }
    }
}
