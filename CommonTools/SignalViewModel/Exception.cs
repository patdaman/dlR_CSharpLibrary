///-------------------------------------------------------------------------------------------------
// <copyright file="SignalException.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Ssur</author>
// <date>20160222</date>
// <summary>Implements the signal exception class</summary>
///-------------------------------------------------------------------------------------------------

using CommonUtils.Exception;
using CommonUtils.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ViewModel
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Values that represent signal exception types. </summary>
    ///
    /// <remarks>   Ssur, 20160222. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public enum CustomExceptionTypes
    {


        /// <summary>   An enum constant representing the general failure error option. </summary>
        GeneralFailure_Error = 1,

        /// <summary>   An enum constant representing the unknown failure error option. </summary>
        UnknownFailure_Error = 2,

        /// <summary>   An enum constant representing the non signal exception error option. </summary>
        NonSignalException_Error = 3,

        /// <summary>   An enum constant representing the invalid arguments error option. </summary>
        InvalidArguments_Error = 4,


        /// <summary>   An enum constant representing the create failure exists option. </summary>
        CreateFailure_Exists = 100,

        /// <summary>   An enum constant representing the create failure inconsistent option. </summary>
        CreateFailure_Inconsistent = 101,

        /// <summary>   An enum constant representing the create failure error option. </summary>
        CreateFailure_Error = 102,


        /// <summary>   An enum constant representing the update failure inconsistent option. </summary>
        UpdateFailure_Inconsistent = 200,

        /// <summary>   An enum constant representing the update failure not exists option. </summary>
        UpdateFailure_NotExists = 201,

        /// <summary>   An enum constant representing the update failure error option. </summary>
        UpdateFailure_Error = 202,


        /// <summary>   An enum constant representing the retrieve failure error option. </summary>
        RetrieveFailure_Error = 300,

        /// <summary>   An enum constant representing the retrieve failure not exists option. </summary>
        RetrieveFailure_NotExists = 301,

        /// <summary>   An enum constant representing the retrieve failure invalid reqest option. </summary>
        RetrieveFailure_InvalidReqest = 302,


        /// <summary>   An enum constant representing the transaction failure error option. </summary>
        TransactionFailure_Error = 400,
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   (Serializable)exception for signalling signal errors. </summary>
    ///
    /// <remarks>   Ssur, 20160222. </remarks>
    ///-------------------------------------------------------------------------------------------------

    [Serializable]
    public class CustomException : Exception
    {
        /// <summary>   Zero-based index of the log. </summary>
        private static readonly log4net.ILog iLog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>   The logger. </summary>
        public LoggingPatternUser logger = new CommonUtils.Logging.LoggingPatternUser() { Logger = iLog };

        /// <summary>   The exception to description map. </summary>
        public Dictionary<CustomExceptionTypes, string> ExceptionToDescriptionMap = new Dictionary<CustomExceptionTypes, string>()
        {

            {CustomExceptionTypes.GeneralFailure_Error, "Error occurred" },
            {CustomExceptionTypes.UnknownFailure_Error, "Unknown error occurred" },
            {CustomExceptionTypes.NonSignalException_Error, "Wrapper for other exceptions" },
            {CustomExceptionTypes.InvalidArguments_Error, "Invalid arguments" },
            {CustomExceptionTypes.CreateFailure_Exists, "Item not creatable: duplicated" },
            {CustomExceptionTypes.CreateFailure_Error, "Item not creatable"},
            {CustomExceptionTypes.CreateFailure_Inconsistent, "Item not creatable: badly specified" },
            {CustomExceptionTypes.RetrieveFailure_Error, "Items not retrievable" },
            {CustomExceptionTypes.RetrieveFailure_InvalidReqest, "Invalid request or key." },
            {CustomExceptionTypes.RetrieveFailure_NotExists, "Items not existant" },
            {CustomExceptionTypes.UpdateFailure_Error, "Item not updateable" },
            {CustomExceptionTypes.UpdateFailure_NotExists, "Item not updateable: does not exist" },
            {CustomExceptionTypes.UpdateFailure_Inconsistent, "Item not updateable: badly specified" },
            {CustomExceptionTypes.TransactionFailure_Error, "Error during transaction" }
        };

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the payload. </summary>
        ///
        /// <value> The payload. </value>
        ///-------------------------------------------------------------------------------------------------

        public Object Payload { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the user. </summary>
        ///
        /// <value> The user. </value>
        ///-------------------------------------------------------------------------------------------------

        public string User { set; get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the type of the exception. </summary>
        ///
        /// <value> The type of the exception. </value>
        ///-------------------------------------------------------------------------------------------------

        public CustomExceptionTypes ExceptionType { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalViewModel.SignalException class.
        /// </summary>
        ///
        /// <remarks>   Ssur, 20160222. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public CustomException() { }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalViewModel.SignalException class.
        /// </summary>
        ///
        /// <remarks>   Ssur, 20160222. </remarks>
        ///
        /// <param name="set">              The set. </param>
        /// <param name="message">          The message. </param>
        /// <param name="innerException">   The inner exception. </param>
        ///-------------------------------------------------------------------------------------------------

        public CustomException(CustomExceptionTypes set, string message, Exception innerException = null) : base(message, innerException)
        {
            LogException(innerException);
            ExceptionType = set;
            Payload = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalViewModel.SignalException class.
        /// </summary>
        ///
        /// <remarks>   Ssur, 20160222. </remarks>
        ///
        /// <param name="set">              The set. </param>
        /// <param name="pl">               The pl. </param>
        /// <param name="message">          The message. </param>
        /// <param name="innerException">   The inner exception. </param>
        ///-------------------------------------------------------------------------------------------------

        public CustomException(CustomExceptionTypes set, Object pl, string message, Exception innerException = null) : base(message, innerException)
        {
            LogException(innerException);
            ExceptionType = set;
            Payload = pl;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   for deserialization. </summary>
        ///
        /// <remarks>   Ssur, 20160222. </remarks>
        ///
        /// <param name="info">     The <see cref="T:System.Runtime.Serialization.SerializationInfo" />
        ///                         that holds the serialized object data about the exception being
        ///                         thrown. </param>
        /// <param name="context">  The <see cref="T:System.Runtime.Serialization.StreamingContext" />
        ///                         that contains contextual information about the source or destination. </param>
        ///-------------------------------------------------------------------------------------------------

        protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                this.User = info.GetString("User");
                try {
                    this.Payload = info.GetValue("Payload", typeof(Object));
                }
                catch (Exception e)
                {
                    this.Payload = null;
                }
                this.ExceptionType = (CustomExceptionTypes)info.GetValue("ExceptionType", typeof(CustomExceptionTypes));
            }

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// When overridden in a derived class, sets the
        /// <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the
        /// exception.
        /// </summary>
        ///
        /// <remarks>   Ssur, 20160222. </remarks>
        ///
        /// <param name="info">     The <see cref="T:System.Runtime.Serialization.SerializationInfo" />
        ///                         that holds the serialized object data about the exception being
        ///                         thrown. </param>
        /// <param name="context">  The <see cref="T:System.Runtime.Serialization.StreamingContext" />
        ///                         that contains contextual information about the source or destination. </param>
        ///-------------------------------------------------------------------------------------------------

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (info != null)
            {
                info.AddValue("User", this.User);
                if (this.Payload != null)
                    info.AddValue("Payload", this.Payload);
                info.AddValue("ExceptionType", this.ExceptionType);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Logs an exception. </summary>
        ///
        /// <remarks>   Ssur, 20160222. </remarks>
        ///
        /// <param name="innerException">   The inner exception. </param>
        ///-------------------------------------------------------------------------------------------------

        private void LogException(Exception innerException)
        {
            if( innerException != null)
            {
                logger.Error("SignalException created. The inner exception details follow.");
                logger.Error(String.Format("{0}\n\nStack Trace:\n{1}",
                    ExamineException.GetInnerExceptionMessages(innerException),
                    ExamineException.GetInnerExceptionStackTraces(innerException)));
            }
        }

    }
}
