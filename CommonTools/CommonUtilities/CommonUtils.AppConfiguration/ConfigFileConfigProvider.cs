using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.AppConfiguration
{
    public class ConfigFileConfigProvider : IAppConfigurationProvider
    {
        public object GetValue(string key)
        {
            if (key == null)
                throw new ArgumentNullException();

            //try and get form app settings first. 
            var value = ConfigurationManager.AppSettings[key];
            //if it doesn't exist try and get from database connection strings 
            if (value == null)
                value = ConfigurationManager.ConnectionStrings[key]?.ConnectionString;

            return value;
        }

    }

}
