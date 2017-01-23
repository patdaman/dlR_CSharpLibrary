///-------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentVariableConfigProvider.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <date>20160525</date>
// <summary>Implements the environment variable configuration provider class</summary>
///-------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.AppConfiguration
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   An environment variable configuration provider. </summary>
    ///
    ///-------------------------------------------------------------------------------------------------

    public class EnvironmentVariableConfigProvider : IAppConfigurationProvider
    {

        EnvironmentVariableTarget environmentTarget; 
                        

        public EnvironmentVariableConfigProvider(EnvironmentVariableTarget envTarget)
        {
            environmentTarget = envTarget;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets a value. </summary>
        ///
        ///
        /// <param name="key">  The key. </param>
        ///
        /// <returns>   The value. </returns>
        ///-------------------------------------------------------------------------------------------------

        public object GetValue(string key)
        {
            object value = Environment.GetEnvironmentVariable(key, environmentTarget);
            return value;
        }
    }
}
