///-------------------------------------------------------------------------------------------------
// <copyright file="DataTableExportExtensions.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160318</date>
// <summary>Implements the data table export extensions class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.DataTableUtils
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A data table export extensions. </summary>
    ///
    /// <remarks>   Ssur, 20160318. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class DataTableExportExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts a data table to a string in tab separated format. </summary>
        ///
        /// <remarks>   Dtorres, 20151022. </remarks>
        ///
        /// <param name="dt">       The datatable to convert. </param>
        /// <param name="header">   The optional header. If null the columns names in the data table will
        ///                         be used. </param>
        ///
        /// <returns>   The tsv string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string GetTsvString(this DataTable dt, string header = null)
        {
            var builder = new StringBuilder();
            AppendHeader(dt, header, builder);

            foreach (DataRow row in dt.Rows)
                builder.AppendLine(String.Join("\t", row.ItemArray));

            return builder.ToString();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Appends a header. </summary>
        ///
        /// <remarks>   Dtorres, 20151022. </remarks>
        ///
        /// <param name="dt">       The datatable to convert. </param>
        /// <param name="header">   The optional header. If null the columns names in the data table will
        ///                         be used. </param>
        /// <param name="builder">  The builder. </param>
        ///-------------------------------------------------------------------------------------------------

        private static void AppendHeader(DataTable dt, string header, StringBuilder builder)
        {
            if (header != null)
            {
                builder.AppendLine(String.Join("\t", header));
            }
            else
            {
                string[] colnames = dt.Columns.Cast<DataColumn>()
                                      .Select(x => x.ColumnName)
                                      .ToArray();
                builder.AppendLine(String.Join("\t", colnames));
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Export data to tsv. </summary>
        ///
        /// <remarks>   Ssur, 20160318. </remarks>
        ///
        /// <param name="dt">       The datatable to convert. </param>
        /// <param name="filename"> Filename of the file. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void ExportDataToTSV(this DataTable dt, string filename)
        {
            string content = GetTsvString(dt);
            File.WriteAllText(filename, content);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Export data to tsv. </summary>
        ///
        /// <remarks>   Ssur, 20160318. </remarks>
        ///
        /// <param name="dt">                   The datatable to convert. </param>
        /// <param name="filename">             Filename of the file. </param>
        /// <param name="columnsToExport">      The columns to export. </param>
        /// <param name="columnHeaderNamesIn">  The column header names in. </param>
        ///-------------------------------------------------------------------------------------------------

        public static void ExportDataToTSV(this DataTable dt, string filename, List<String> columnsToExport, List<String> columnHeaderNamesIn=null)
        {
            var builder = new StringBuilder();            
            string[] dataColumnNames = columnsToExport.ToArray();
            string[] columnHeaderNames = (columnHeaderNamesIn == null) ? dataColumnNames : columnHeaderNamesIn.ToArray();


            builder.AppendLine(String.Join("\t", columnHeaderNames.ToArray()));
                
            foreach (DataRow row in dt.Rows)
            {
                foreach (String colName in columnsToExport)
                {
                    object item = FormatItem(row[colName]);
                    builder.Append(item.ToString() + "\t");                        
                }
                builder.Append("\n");
            }
            
            File.WriteAllText(filename, builder.ToString());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Format item. </summary>
        ///
        /// <remarks>   Ssur, 20160318. </remarks>
        ///
        /// <param name="item"> The item. </param>
        ///
        /// <returns>   The formatted item. </returns>
        ///-------------------------------------------------------------------------------------------------

        private static object FormatItem(object item)
        {
            if( ! (item is DateTime))
                return item; 
            else 
            {
                DateTime dateTime = (DateTime) item;
                return dateTime.ToString("MM/dd/yyyy");
            }            
        }

    }
}
