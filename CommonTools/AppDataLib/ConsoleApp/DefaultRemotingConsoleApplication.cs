///-------------------------------------------------------------------------------------------------
// <copyright file="SGNLDefaultRemotingConsoleApplication.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <date>20160226</date>
// <summary>Implements the signal default remoting console application class</summary>
///-------------------------------------------------------------------------------------------------

using CommonUtils.Logging;
using NDesk.Options;
using System;

namespace AppDataLib.ConsoleApp
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// A version of the default console app that extends MarshalByRefObj to accomodate remoting.
    /// This is used also for allowing changes in application domain e.g. to load custom
    /// configuration files.
    /// </summary>
    ///
    /// <remarks>   Ssur, 20160226. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class DefaultRemotingConsoleApplication : MarshalByRefObject
    {
        public ConsoleAppCore AppCore { get; set; }
        public OptionSet Options
        {
            get { return AppCore.Options; }
            set { AppCore.Options = value; }
        }

        public LoggingPatternUser Logger
        {
            get { return ConsoleAppCore.Logger; }
        }
        public static string AssemblyVersionString
        {
            get { return ConsoleAppCore.AssemblyVersionString; }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the full pathname of the log file. </summary>
        ///
        /// <value> The full pathname of the log file. </value>
        ///-------------------------------------------------------------------------------------------------

        public String LogFilePath
        {
            get { return AppCore.LogFilePath; }
            set { AppCore.LogFilePath = value; }
        }

        public DefaultRemotingConsoleApplication()
        {
            AppCore = new ConsoleAppCore();
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
            OptionSet additionalOptions,
            string defaultEmailAddress = null,
            string logConfigFileName = null,
            string emailSubject = null,
            string configFacility = null
        )
        {
            AppCore.Initialize(applicationType, appName, additionalOptions,
                defaultEmailAddress, logConfigFileName, emailSubject);
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
            AppCore.ParseArgs(args);
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

        protected void ShowUsage(OptionSet opts, String AddTopLine = null, bool badcmd = false)
        {
            AppCore.ShowUsage(opts, AddTopLine, badcmd);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sets log email subject. </summary>
        ///
        /// <remarks>   Ssur, 20160226. </remarks>
        ///
        /// <param name="subject">  The subject. </param>
        /// <param name="priority"> The priority. </param>
        ///-------------------------------------------------------------------------------------------------

        protected void SetLogEmailSubject(string subject, System.Net.Mail.MailPriority priority = System.Net.Mail.MailPriority.Normal)
        {
            AppCore.SetLogEmailSubject(subject, priority);
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
            AppCore.SetLogEmailPriority(priority);
        }
    }
}
