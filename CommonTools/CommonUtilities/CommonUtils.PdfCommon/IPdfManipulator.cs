using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.PdfCommon
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Interface for PDF manipulator. </summary>
    ///
    /// <remarks>   Dtorres, 20160520. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public interface IPdfManipulator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Flatten multiple PDF documents into a single document. </summary>
        ///
        /// <param name="documentlist"> The documentlist. </param>
        ///
        /// <returns>   A byte[]. </returns>
        ///-------------------------------------------------------------------------------------------------

        MemoryStream FlattenDocuments(IEnumerable<byte[]> documentlist);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets page count of PDF . </summary>
        ///
        /// <param name="parameter1">   The first parameter. </param>
        ///
        /// <returns>   The page count. </returns>
        ///-------------------------------------------------------------------------------------------------

        int GetPageCount(byte[] bytes);
    }
}
