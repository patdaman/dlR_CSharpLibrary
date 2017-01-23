using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Database
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A connection utilities for dealing with SQL connection strings. </summary>
    ///
    /// <remarks>   Dtorres, 20160401. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ConnectionUtils
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets scrubbed entity connection string from an Entity Framework style 
        ///             connection string. (E.g. has meta data tags, etc.) </summary>
        ///
        /// <remarks>   Dtorres, 20160401. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        ///
        /// <returns>   The scrubbed entity connection string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string GetScrubbedEntityConnectionString( string connectionString)
        {
            var connectionBuilder =
                new SqlConnectionStringBuilder(
                    new EntityConnectionStringBuilder(connectionString)
                        .ProviderConnectionString);
            
            connectionBuilder.Remove("Password");
            return connectionBuilder.ToString();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets scrubbed SQL connection string from a 'plain' SQL connection string.
        ///             If connection string has additional metadata (like EF connection strings)
        ///             use the alterntive method in this module. </summary>
        ///
        /// <remarks>   Dtorres, 20160401. </remarks>
        ///
        /// <param name="connectionString"> The connection string. </param>
        ///
        /// <returns>   The scrubbed SQL connection string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string GetScrubbedSqlConnectionString(string connectionString)
        {
            var connectionBuilder =
                new SqlConnectionStringBuilder(connectionString);
            connectionBuilder.Remove("Password");
            return connectionBuilder.ToString();
        }
    }
}
