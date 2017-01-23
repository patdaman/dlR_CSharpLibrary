///-------------------------------------------------------------------------------------------------
// <copyright file="DatabaseConnection.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160223</date>
// <summary>Utility class for database connections</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Database
{
    public class DatabaseConnection
    {
        public string Database { get; set; }
        public string ServerName { get; set; }
        public string UserID { get; set; }
        public String Password { get; set; }
        public string MetaData { get; set; }
        public SecureString SecurePassword { get; set; }
        public String ProviderName { get; set; }


        public DatabaseConnection() { }

        public string ToConnectionString()
        {
            // Initialize the connection string builder for the 
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = ServerName;
            sqlBuilder.InitialCatalog = Database;
            sqlBuilder.UserID = UserID;
            sqlBuilder.Password = Password;
            // sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.MultipleActiveResultSets = true;

            // Build the SqlConnection connection string. 
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = ProviderName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = MetaData;
            return entityBuilder.ToString();
        }
    }
}
