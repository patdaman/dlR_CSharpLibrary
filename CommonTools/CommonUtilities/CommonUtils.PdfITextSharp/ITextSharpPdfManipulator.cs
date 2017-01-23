using CommonUtils.PdfCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.PdfITextSharp
{
    public class ITextSharpPdfManipulator : IPdfManipulator
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
            
            //create final PDF shell                     
            MemoryStream finalOutputStream = new MemoryStream();
            using (var finalDocument = new iTextSharp.text.Document())
            {
                var pdfCopier = new iTextSharp.text.pdf.PdfCopy(finalDocument, finalOutputStream);
                finalDocument.Open();

                //Loop over source PDFs                     
                foreach (byte[] sourceDocumentBytes in documentlist)
                {
                    //open source PDF                 
                    using (var pdfReader = new iTextSharp.text.pdf.PdfReader(sourceDocumentBytes))
                    {
                        //loop over pages 
                        for (int iPage = 1; iPage <= pdfReader.NumberOfPages; iPage++)
                        {
                            pdfCopier.AddPage(pdfCopier.GetImportedPage(pdfReader, iPage));
                        }
                        pdfCopier.FreeReader(pdfReader);
                        pdfReader.Close();
                    }                
                }
                finalDocument.Close();
            }
            //return finalOutputStream.GetBuffer();
            return finalOutputStream;
        }

        public int GetPageCount(byte[] bytes)
        {
            var pdfReader = new iTextSharp.text.pdf.PdfReader(bytes);
            return pdfReader.NumberOfPages;

        }
    }
}
