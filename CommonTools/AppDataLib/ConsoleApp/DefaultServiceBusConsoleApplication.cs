using AppDataLib.ServiceBus;
using CommonUtils.AppConfiguration;
using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDataLib.ConsoleApp
{
    public class DefaultServiceBusConsoleApplication : DefaultConsoleApplication
    {
        public AppConfiguration AppConfig { get; private set; }

        protected string ServiceBusConnectionString { get; private set; }
        protected SGNLServiceBusManager ServiceBusManager { get; private set; }

        new public void Initialize(Type applicationType,
            string appName,
            OptionSet additionalOptions = null,
            string defaultEmailAddress = null,
            string logConfigFileName = null,
            string emailSubject = null
        )
        {
            base.Initialize(applicationType, appName, additionalOptions, defaultEmailAddress,
                logConfigFileName, emailSubject);

            AppConfig = new AppConfiguration();
            AppConfig.AddProvider(new ConfigFileConfigProvider());
            AppConfig.AddProvider(new EnvironmentVariableConfigProvider(EnvironmentVariableTarget.User));

            ServiceBusConnectionString = AppConfig.GetValue<string>("ServiceBusConnectionString");
            ServiceBusManager = new SGNLServiceBusManager(ServiceBusConnectionString);          

        }


    }
}
