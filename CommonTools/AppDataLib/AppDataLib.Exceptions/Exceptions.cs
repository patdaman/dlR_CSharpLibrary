using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDataLib.Exceptions
{

    [Serializable]
    public class SgnlCaseBaseException : Exception
    {
        public string CaseNumber { get; set; }

        public SgnlCaseBaseException() { }
        public SgnlCaseBaseException(string casenumber) : base(message: $"Exception with case {casenumber}")
        {
            CaseNumber = casenumber;
        }

        public SgnlCaseBaseException(string casenumber, string message) : base(message)
        {
            CaseNumber = casenumber;
        }

        public SgnlCaseBaseException(string casenumber, string message, Exception inner) : base(message, inner)
        {
            CaseNumber = casenumber;
        }

        protected SgnlCaseBaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                this.CaseNumber = info.GetString("CaseNumber");
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            if (info != null)
            {
                info.AddValue("CaseNumber", this.CaseNumber);
            }
        }
    }



    [Serializable]
    public class SgnlEventExceptions : Exception
    {
        public SgnlEventExceptions() { }
        public SgnlEventExceptions(string message) : base(message) { }
        public SgnlEventExceptions(string message, Exception inner) : base(message, inner) { }
        protected SgnlEventExceptions(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SgnlReportTypeNotFoundException : SgnlCaseBaseException
    {
        public SgnlReportTypeNotFoundException(string casenumber) : base(casenumber, $"Unknown report type for case {casenumber}") { }
        public SgnlReportTypeNotFoundException(string casenumber, string message) : base(casenumber, message) { }
        public SgnlReportTypeNotFoundException(string casenumber, string message, Exception inner) : base(casenumber, message, inner) { }
        protected SgnlReportTypeNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SgnlInternalCaseException: SgnlCaseBaseException
    {
        public SgnlInternalCaseException(string casenumber) : base(casenumber, $"An internal error occured with case {casenumber}") { }
        public SgnlInternalCaseException(string casenumber, string message) : base(casenumber, message) { }
        public SgnlInternalCaseException(string casenumber, string message, Exception inner) : base(casenumber, message, inner) { }
        protected SgnlInternalCaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }



    [Serializable]
    public class SgnlResultsNotReadyException : SgnlCaseBaseException
    {
        public SgnlResultsNotReadyException(string casenumber) : base(casenumber, $"Results are not ready for case number {casenumber}") { }        
        public SgnlResultsNotReadyException(string casenumber, string message, Exception inner) : base(casenumber, message, inner) { }
        protected SgnlResultsNotReadyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
       

    [Serializable]
    public class SgnlResultsPx_ResultsDiscrepancyException : SgnlCaseBaseException
    {
        public SgnlResultsPx_ResultsDiscrepancyException(string casenumber) : 
            base(casenumber, $"Encountered a discrepancy with ResultsPX pipeline (Results mismatch.). Please inform IS Dept. Case: {casenumber}") { }
        public SgnlResultsPx_ResultsDiscrepancyException(string casenumber, string message) : base(casenumber,message) { }
        public SgnlResultsPx_ResultsDiscrepancyException(string casenumber, string message, Exception inner) : base(casenumber, message, inner) { }
        protected SgnlResultsPx_ResultsDiscrepancyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class SgnlResultsPx_QcDiscrepencyException : SgnlCaseBaseException
    {
        public SgnlResultsPx_QcDiscrepencyException(string casenumber) : 
            base(casenumber, $"Encountered a discrepancy with ResultsPX pipeline (QC mismatch). Please inform IS Dept. Case: {casenumber}") { } 
        public SgnlResultsPx_QcDiscrepencyException(string casenumber, string message) : base(casenumber, message) { }
        public SgnlResultsPx_QcDiscrepencyException(string casenumber, string message, Exception inner) : base(casenumber, message, inner) { }
        protected SgnlResultsPx_QcDiscrepencyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SgnlCaseNotFoundException : SgnlCaseBaseException
    {
        public SgnlCaseNotFoundException(string casenumber) : base(casenumber, $"Casenumber {casenumber} not found") { }
        public SgnlCaseNotFoundException(string casenumber, string message) : base(casenumber, message) { }
        public SgnlCaseNotFoundException(string casenumber, string message, Exception inner) : base(casenumber, message, inner) { }
        protected SgnlCaseNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class SgnlSignedTypeUnknownException : SgnlCaseBaseException
    {
        public SgnlSignedTypeUnknownException(string casenumber): 
            base(casenumber, $"Unknown signature type for casenumber {casenumber}") { }
        public SgnlSignedTypeUnknownException(string casenumber, string signatureType, string message) : base(casenumber, message) { }
        public SgnlSignedTypeUnknownException(string casenumber, string message, Exception inner) : base(casenumber, message, inner) { }
        protected SgnlSignedTypeUnknownException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }



    [Serializable]
    public class SgnlUnauthorizedAccessException : Exception
    {
        public SgnlUnauthorizedAccessException() { }
        public SgnlUnauthorizedAccessException(string message) : base(message) { }
        public SgnlUnauthorizedAccessException(string message, Exception inner) : base(message, inner) { }
        protected SgnlUnauthorizedAccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    
}
