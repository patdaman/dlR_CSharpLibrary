using CommonUtils.AppConfiguration;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF = SignalEFDataModel;

namespace BusinessLayer.PxPortal
{
    public class NYResultsProcessor
    {
        private static EF.SGNL_ANALYTICS.SGNL_ANALYTICSEntities AnalyticsDbContext = null;
        private static EF.SGNL_LIS.SGNL_LISEntities LISDbContext = null;
        private static EF.SGNL_INTERNAL.SGNL_INTERNALEntities INTDbContext = null;

        private PrsResultsMapper Mapper = new PrsResultsMapper();

        public NYResultsProcessor()
        {
            Mapper.Configure();
            var config = new AppConfiguration();
            config.AddProvider(new ConfigFileConfigProvider());
            config.AddProvider(new EnvironmentVariableConfigProvider(EnvironmentVariableTarget.User));
            AnalyticsDbContext = new EF.SGNL_ANALYTICS.SGNL_ANALYTICSEntities(config.GetValue<string>("SGNL_ANALYTICSEntities"));
            LISDbContext = new EF.SGNL_LIS.SGNL_LISEntities(config.GetValue<string>("SGNL_LISEntities"));
            INTDbContext = new EF.SGNL_INTERNAL.SGNL_INTERNALEntities(config.GetValue<string>("SGNL_INTERNALEntities"));
        }

        public NYResultsProcessor(EF.SGNL_ANALYTICS.SGNL_ANALYTICSEntities anaDbContext, EF.SGNL_LIS.SGNL_LISEntities lisDbContext
                , EF.SGNL_INTERNAL.SGNL_INTERNALEntities intDbContext) : base()
        {
            Mapper.Configure();
            AnalyticsDbContext = anaDbContext;
            LISDbContext = lisDbContext;
            INTDbContext = intDbContext;
        }

        public List<NYCase> GetNYCases(DateTime? fromCompletedDate = null, DateTime? toCompletedDate = null)
        {
            //get lis results 

            var queryResult = LISDbContext.vi_GetAnalyticsCasePatientInfo
                .Where(p => (p.State == "NY" || p.PatientState == "NY"));

            if (fromCompletedDate != null)
            {
                DateTimeOffset fdate = new DateTimeOffset(fromCompletedDate.Value);
                queryResult = queryResult.Where(p => fdate.CompareTo(p.CompletedDate.Value) <= 0);
            }

            if (toCompletedDate != null)
            {
                DateTimeOffset tdate = new DateTimeOffset(toCompletedDate.Value);
                queryResult = queryResult.Where(p => tdate.CompareTo(p.CompletedDate.Value) > 0);
            }



            List<EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo> resultsLis = queryResult
                .OrderByDescending(p => p.CaseNumber)
                .Select(p => p).ToList();

            Debug.Assert(resultsLis != null);

            List<NYCase> nycList = resultsLis.Select(p => new NYCase(p)).ToList();

            return nycList;
        }



        public List<PrsResult> GetNYPrsResults(DateTime? fromCompletedDate = null, DateTime? toCompletedDate = null)
        {
            //get NY lis results 
            var queryResult = LISDbContext.vi_GetAnalyticsCasePatientInfo
                .Where(p => (p.State == "NY" || p.PatientState == "NY"));

            if (fromCompletedDate != null)
            {
                DateTimeOffset fdate = new DateTimeOffset(fromCompletedDate.Value);
                queryResult = queryResult.Where(p => fdate.CompareTo(p.CompletedDate.Value) <= 0);
            }

            if (toCompletedDate != null)
            {
                DateTimeOffset tdate = new DateTimeOffset(toCompletedDate.Value);
                queryResult = queryResult.Where(p => tdate.CompareTo(p.CompletedDate.Value) > 0);
            }

            List<EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo> resultsLis = queryResult
                .OrderByDescending(p => p.CaseNumber)
                .Select(p => p).ToList();

            List<String> casenumbers = resultsLis.Select(p => p.CaseNumber).ToList();


            // \TODO need to get current report versions from table 
            string GEP70_AlgorithmVersion = "0.08beta";
            string Subtype_AlgorithmVersion = "0.07beta";
            string VK_AlgorithmVersion = "0.08beta";
            string TP53_AlgorithmVersion = "0.07beta";

            //get analytics results 
            List<EF.SGNL_ANALYTICS.vi_GetAnalyticsCaseResults> resultsAnalytics =
                AnalyticsDbContext.vi_GetAnalyticsCaseResults
                .Where( r => casenumbers.Contains(r.CaseNumber) 
                    && r.GEP70_AlgorithmVersion == GEP70_AlgorithmVersion
                    && r.Subtype_AlgorithmVersion == Subtype_AlgorithmVersion
                    && r.VK_AlgorithmVersion == VK_AlgorithmVersion
                    && (r.TP53_AlgorithmVersion == TP53_AlgorithmVersion || r.TP53_AlgorithmVersion == null)
                )
                .OrderByDescending(r => r.CaseNumber)
                .ToList();
            Debug.Assert(resultsAnalytics != null);

            List<string> resultCaseNumbers = resultsAnalytics.Select(r => r.CaseNumber).ToList();


            //get intersecting cases in lis results 
            resultsLis = resultsLis.Where(p => resultCaseNumbers.Contains(p.CaseNumber))
                .OrderByDescending(p => p.CaseNumber)
                .ToList();
            Debug.Assert(resultsLis != null);

            //return PrsResult list 
            List<PrsResult> prsResults = Mapper.Map(resultsAnalytics, resultsLis);
            return prsResults;
        }

        public void MarkNYCaseUploaded(string caseno)
        {
            int nyCasenoExists = INTDbContext.NYCasesUploadeds.Where(n => n.CaseNumber == caseno && n.Uploaded == true).Count();
            if (nyCasenoExists != 0)
                throw new Exception("Case " + caseno + " already marked as uploaded");

            EF.SGNL_INTERNAL.NYCasesUploaded ny = new EF.SGNL_INTERNAL.NYCasesUploaded();
            ny.CaseNumber = caseno;
            ny.Uploaded = true;
            ny.LastModifiedDate = DateTime.Now;
            ny.LastModifiedUser = "NYResultsProcessor";
            INTDbContext.NYCasesUploadeds.Add(ny);
            INTDbContext.SaveChanges();
        }

    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A ny case. </summary>
    ///
    /// <remarks>   Ssur, 20160616. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class NYCase
    {
        public string CaseNumber { get; set; }
        public DateTimeOffset CompletedDate { get; set; }
        public String Facility { get; set; }
        public String Physician { get; set; }
        public string Patient { get; set; }
        public bool IsReportedToRegistry { get; set; }


        public EF.SGNL_INTERNAL.SGNL_INTERNALEntities INTDbContext = new EF.SGNL_INTERNAL.SGNL_INTERNALEntities();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalBusinessLayer.PxPortal.NYCase class.
        /// </summary>
        ///
        /// <remarks>   RPhilavanh, 20160616. </remarks>
        ///
        /// <param name="p">    The vi_GetAnalyticsCasePatientInfo to process. </param>
        ///-------------------------------------------------------------------------------------------------

        public NYCase(EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo p)
        {
            CaseNumber = p.CaseNumber;
            CompletedDate = p.CompletedDate.Value;
            Facility = p.FacilityName + "(" + p.City + "," + p.State + ")" ;
            Physician = p.OrderingPhysicianLastName + ", " + p.OrderingPhysicianFirstName;
            Patient = p.LastName + ", " + p.FirstName + "(" + p.PatientCity + ","+ p.PatientState + ")";
            IsReportedToRegistry = INTDbContext.NYCasesUploadeds.Where(n => n.CaseNumber == p.CaseNumber).Select(n => n.Uploaded).SingleOrDefault();


        }
    }
}
