///-------------------------------------------------------------------------------------------------
// <copyright file="DBUtils.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160308</date>
// <summary>Implements the database utilities class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Database
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A database utilities. </summary>
    ///
    /// <remarks>   Ssur, 20160308. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public static class DBUtils
    {
        public static string GetServerCatalogFromEntity(DbContext dbc)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = dbc.Database.Connection.ConnectionString;
            
            return builder.DataSource + ":" + builder.InitialCatalog;
        }
    }
}
