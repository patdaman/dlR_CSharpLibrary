///-------------------------------------------------------------------------------------------------
// <copyright file="ConsoleAppCore.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20161006</date>
// <summary>Implements the console application core class</summary>
///-------------------------------------------------------------------------------------------------

using CommonUtils.Build;
using CommonUtils.Logging;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace AppDataLib.ConsoleApp
{
    public class ConsoleAppCore
    {
        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        static Dictionary<CtrlType, String> ExitTypeMap = new Dictionary<CtrlType, string>()
        {
            { CtrlType.CTRL_C_EVENT, "CTRL-C" },
            {CtrlType.CTRL_BREAK_EVENT, "BREAK" },
            {CtrlType.CTRL_CLOSE_EVENT, "CONSOLE CLOSE" },
            {CtrlType.CTRL_LOGOFF_EVENT, "LOGOFF" },
            {CtrlType.CTRL_SHUTDOWN_EVENT, "SHUTDOWN" }

        };

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Handler, called when the exiting program so some logging may be performed. </summary>
        ///
        /// <remarks>   Ssur, 20161006. </remarks>
        ///
        /// <param name="sig">  The signal. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        private static bool ExitHandler(CtrlType sig)
        {
            Console.WriteLine("Exiting system due to " + ExitTypeMap[sig]);
            Logger.Warn("Console Application Forcibly Terminated via <" + ExitTypeMap[sig] + ">");
            Environment.Exit(-1);

            return true;
        }
        #endregion

        public const string DefaultLogFileName = "logfile.txt";
        public const string DefaultLogConfigFileName = "Log.config";

        protected static ILog iLog;
        public static LoggingPatternUser Logger;
        protected readonly string lp = @"V6x0OEFj6c2H6f4k";
        protected readonly string un = @"IsEmail@signalgenetics.com";
        public SmtpAppender smtpAppender;

        public string EmailAddress;
        public string LogConfigFile = DefaultLogConfigFileName;
        public string EmailSubject { get; set; }
        protected bool ShowHelp = false;
        public OptionSet Options { get; set; }
        public string AppName { get; set; }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the assembly version string. </summary>
        ///
        /// <value> The assembly version string. </value>
        ///-------------------------------------------------------------------------------------------------

        public static string AssemblyVersionString
        {
            get
            {
                string message;
                try
                {
                    Version ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    message = ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("AssemblyVersionString error: " + ex.Message);
                    try
                    {
                        message = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                    }
                    catch (Exception exx)
                    {
                        message = "Unknown Version";
                    }
                }
                return message;
            }
        }



        protected String logFilePath = null;
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the full pathname of the log file. </summary>
        ///
        /// <value> The full pathname of the log file. </value>
        ///-------------------------------------------------------------------------------------------------

        public String LogFilePath
        {
            get { return logFilePath; }
            set
            {
                if (value != logFilePath)
                {
                    try
                    {
                        if (value != null)
                        {
                            Path.GetFullPath(value); // check if path is valid, throws exception otherwise 
                            logFilePath = value;
                            log4net.GlobalContext.Properties["LogFileName"] = Path.GetFullPath(logFilePath);
                            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(LogConfigFile));
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        public ConsoleAppCore()
        {
            //handle unexpected exits with logging
            _handler += new EventHandler(ExitHandler);
            SetConsoleCtrlHandler(_handler, true);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes this object. </summary>
        ///
        /// <remarks>   Dtorres, 20151116. </remarks>
        ///
        /// <param name="applicationType">      Type of the application. </param>
        /// <param name="additionalOptions">    Options for controlling the additional. </param>
        /// <param name="defaultEmailAddress">  The default email address. </param>
        /// <param name="logFileName">          Filename of the log file. </param>
        /// <param name="logConfigFileName">    Filename of the log configuration file. </param>
        /// <param name="emailSubject">         The email subject. </param>
        ///-------------------------------------------------------------------------------------------------

        public void Initialize(Type applicationType,
            string appName,
            OptionSet additionalOptions = null,
            string defaultEmailAddress = null,
            string logConfigFileName = null,
            string emailSubject = null,
            string configFacility = null
        )
        {
            AppName = appName;
            Console.WriteLine(AppName + " v " + AssemblyVersionString);
            Console.WriteLine("(c) Signal Genetics Inc.");
            InitializeLogging(applicationType, defaultEmailAddress, logConfigFileName, emailSubject, configFacility);
            InitializeOptions(additionalOptions);
        }

        private void InitializeLogging(Type applicationType, string defaultEmailAddress, string logConfigFileName, string emailSubject, string configFacility)
        {
            iLog = LogManager.GetLogger(applicationType);
            Logger = new LoggingPatternUser { Logger = iLog };
            EmailAddress = defaultEmailAddress;
            EmailSubject = emailSubject;
            LogConfigFile = logConfigFileName ?? LogConfigFile;
            LogFilePath = DefaultLogFileName;

            string className = applicationType.ToString();


            // set the syslog appender properties if the syslog appender exists in the log config file.
            Hierarchy hier = log4net.LogManager.GetRepository() as Hierarchy;
            if (hier != null)
            {
                var syslogAppender = hier.GetAppenders()
                    .Where(app => app.Name.Equals("RemoteSyslogAppender", StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault() as RemoteSyslogAppender;

                if (syslogAppender != null)
                {
                    string useFacility = configFacility ?? "Daemons";
                    if (!Log4NetHelper.SyslogFacilityBiDictionary.ContainsKey(useFacility))
                        useFacility = "Daemons";

                    List<AppenderPropertyValue> apvlist = new List<AppenderPropertyValue>()
                    {
                        new AppenderPropertyValue() {AppenderName="RemoteSyslogAppender", PropertyName="Facility", ValueString=useFacility },
                        new AppenderPropertyValue() {AppenderName="RemoteSyslogAppender", PropertyName="Identity", ValueString= "SGNL_" + className + "_" + AssemblyUtils.MachineName}
                    };
                    Log4NetHelper.SetAppenderProperties(apvlist);
                }
            }



        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Initializes the options. </summary>
        ///
        /// <remarks>   Dtorres, 20151116. </remarks>
        ///
        /// <param name="additionalOptions">    Options for controlling the additional. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void InitializeOptions(OptionSet additionalOptions)
        {
            //Default options provided to all derived applications 
            Options = new OptionSet()
            {
                {"l|log=", "log file path",   v => LogFilePath = v },
                {"h|?|help", "show usage", v => ShowHelp = true },
                {"e|email=", "email address for errors (optional), set to null to disable", v=> EmailAddress = v }
            };
            //Combined all options 
            if (additionalOptions != null)
                foreach (Option opt in additionalOptions.ToArray())
                {
                    Options.Add(opt);
                }
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse arguments. </summary>
        ///
        /// <remarks>   Dtorres, 20151116. </remarks>
        ///
        /// <param name="args"> The arguments. </param>
        ///-------------------------------------------------------------------------------------------------

        public void ParseArgs(string[] args)
        {
            Options.Parse(args);

            if (ShowHelp)
            {
                ShowUsage(Options);
                Environment.Exit(0);
            }

            SetLoggingOptions(LogFilePath, EmailAddress);


        }


        protected void SetLoggingOptions(string logfile, string emailaddress)
        {

            LogFilePath = logfile ?? DefaultLogFileName;
            Hierarchy hier = log4net.LogManager.GetRepository() as Hierarchy;

            if (hier != null)
            {
                smtpAppender = hier.GetAppenders().
                    Where(
                    app => app.Name.Equals("SmtpAppender", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault() as SmtpAppender;
                if (smtpAppender != null)
                {
                    smtpAppender.Password = lp;
                    smtpAppender.Username = un;
                    SetLogEmailSubject(EmailSubject);
                    if (EmailAddress != null)
                        smtpAppender.To = EmailAddress;

                    if (emailaddress != null)
                    {
                        if (emailaddress == "null") // do not email
                            smtpAppender.Close(); // this should disable the emailing
                        else
                        {
                            var addr = new System.Net.Mail.MailAddress(emailaddress); //this will throw an exception for badly formed email addresses
                            smtpAppender.To = emailaddress;
                        }
                    }
                }
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets log email subject. </summary>
        ///
        /// <remarks>   Ssur, 20160226. </remarks>
        ///
        /// <param name="subject">  The subject. </param>
        /// <param name="priority"> The priority. </param>
        ///-------------------------------------------------------------------------------------------------

        public void SetLogEmailSubject(string subject, System.Net.Mail.MailPriority priority = System.Net.Mail.MailPriority.Normal)
        {
            if (smtpAppender != null)
            {

                smtpAppender.Subject = subject ?? String.Format("Log from {0}", AppName);
                //Add machine name to subject line
                smtpAppender.Subject = smtpAppender.Subject + " [" + AssemblyUtils.MachineName + "]";
                smtpAppender.Priority = priority;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets log email priority. </summary>
        ///
        /// <remarks>   Ssur, 20160226. </remarks>
        ///
        /// <param name="priority"> The priority. </param>
        ///-------------------------------------------------------------------------------------------------

        public void SetLogEmailPriority(System.Net.Mail.MailPriority priority = System.Net.Mail.MailPriority.Normal)
        {
            smtpAppender.Priority = priority;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Shows the usage. </summary>
        ///
        /// <remarks>   Ssur, 20160226. </remarks>
        ///
        /// <param name="opts">         Options for controlling the operation. </param>
        /// <param name="AddTopLine">   The add top line. </param>
        /// <param name="badcmd">       true to badcmd. </param>
        ///-------------------------------------------------------------------------------------------------

        public void ShowUsage(OptionSet opts, String AddTopLine = null, bool badcmd = false)
        {
            if (AddTopLine != null)
                Console.WriteLine(AddTopLine + "\n");

            Console.WriteLine("Usage: " + AppName + " <options>");
            if (badcmd)
                Console.WriteLine("\n\nIncorrect commandline ... task complete\n\nPress any Key to Exit.");
            opts.WriteOptionDescriptions(Console.Out);
#if DEBUG
            Console.Read();
#endif
        }
    }
}
