///-------------------------------------------------------------------------------------------------
// <copyright file="ConfigConnectionString.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160223</date>
// <summary>Implements the configuration connection string class. A utility class to hold connection
// strings specified in config files</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
//using System.Data.Entity.Core.EntityClient;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.Database
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A configuration connection string. </summary>
    ///
    /// <remarks>   Ssur, 20160223. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ConnectionConfig
    {
        public String Name { get; set; }
        public String ConnectionString { get; set; }
        public String ProviderName { get; set; }

        public string ToConnectionString()
        {
            EntityConnectionStringBuilder ecsb = new EntityConnectionStringBuilder(ConnectionString);
            return ecsb.ToString();
        }
    }
}
