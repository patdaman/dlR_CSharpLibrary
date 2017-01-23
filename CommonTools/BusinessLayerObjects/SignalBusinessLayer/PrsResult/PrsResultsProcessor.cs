///-------------------------------------------------------------------------------------------------
// <copyright file="PrsResultsProcessor.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Dtorres</author>
// <date>20160607</date>
// <summary>Implements the prs results processor class</summary>
///-------------------------------------------------------------------------------------------------
using AppDataLib.Enums;
using AutoMapper;
using CommonUtils.Logging;
using CommonUtils.Json;
using BusinessLayer.Analytics;
using ViewModel;
using ViewModel.Analytics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EF = SignalEFDataModel;

namespace BusinessLayer
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Utility class that maps datbase/EF objects into the CaseResult object. </summary>
    ///
    /// <remarks>   Dtorres, 20160513. </remarks>
    ///-------------------------------------------------------------------------------------------------

    internal class PrsResultsMapper
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the mapper. </summary>
        ///
        /// <value> The mapper. </value>
        ///-------------------------------------------------------------------------------------------------

        private IMapper Mapper { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalBusinessLayer.PrsResultsMapper class.
        /// </summary>
        ///
        /// <remarks>   Dtorres, 20160607. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public PrsResultsMapper()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Configures this object. </summary>
        ///
        /// <remarks>   Dtorres, 20160607. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EF.SGNL_ANALYTICS.vi_GetAnalyticsCaseResults, PrsResult>();
                cfg.CreateMap<EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo, PrsResult>();
            });
            this.Mapper = config.CreateMapper();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Maps. </summary>
        ///
        /// <remarks>   Dtorres, 20160607. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="analysisResult">   The analysis result. </param>
        /// <param name="patientResult">    The patient result. </param>
        ///
        /// <returns>   A List&lt;PrsResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        public PrsResult Map(
            EF.SGNL_ANALYTICS.vi_GetAnalyticsCaseResults analysisResult,
            EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo patientResult)
        {
            if (analysisResult != null && analysisResult.CaseNumber != patientResult.CaseNumber)
            {
                throw new Exception(String.Format(
                        "Exception in PrsResults mapping. Case numbers from an Analysis result and LIS result did not match. {0} - {1}", analysisResult.CaseNumber, patientResult.CaseNumber)
                        );
            }
            var caseResult = new PrsResult();
            if (analysisResult != null)
                Mapper.Map<EF.SGNL_ANALYTICS.vi_GetAnalyticsCaseResults, PrsResult>(analysisResult, caseResult);
            Mapper.Map<EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo, PrsResult>(patientResult, caseResult);
            return caseResult;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Maps. </summary>
        ///
        /// <remarks>   Dtorres, 20160607. </remarks>
        ///
        /// <param name="analysisResults">  The analysis results. </param>
        /// <param name="patientResult">    The patient result. </param>
        ///
        /// <returns>   A List&lt;PrsResult&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        [Obsolete]
        //!! WARNING THIS IS NOT CORRECT DOING TO TALK WITH RITA ABOUT HER USE IN NYRESULTS PROCESSOR 
        public List<PrsResult> Map(
            List<EF.SGNL_ANALYTICS.vi_GetAnalyticsCaseResults> analysisResults,
            List<EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo> patientResult)
        {
            Debug.Assert(patientResult.Count > 0, "patientResult.Count was 0");
            Debug.Assert(analysisResults.Count > 0
                && analysisResults.Count == patientResult.Count, "Lists did not contain the same number of items.");

            List<PrsResult> list = new List<PrsResult>();
            for (int ind = 0; ind < patientResult.Count; ind++)
            {
                list.Add(Map(analysisResults[ind], patientResult[ind]));
            }
            return list;
        }
    }

    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A case results processor. Returns results related to a case. </summary>
    ///
    /// <remarks>   Dtorres, 20160512. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class PrsResultsProcessor : BehaviorBase, IPrsResultsProcessor
    {
        /// <summary>   The logger. </summary>
        public static LoggingPatternUser logger = new LoggingPatternUser() { Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType) };

        /// <summary>   The test type my prs. </summary>
        private const string TestType_MyPrs = "MyPRS";
        /// <summary>   The algorithm name gep 70 risk score. </summary>
        private const string AlgorithmName_GEP70RiskScore = "MyPRS_GEP70RiskScore";
        /// <summary>   Type of the algorithm name sub. </summary>
        private const string AlgorithmName_SubType = "MyPRS_Subtype";
        /// <summary>   The algorithm name vk. </summary>
        private const string AlgorithmName_VK = "MyPRS_VK";

        

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   The mapper. </summary>
        ///
        /// <value> The mapper. </value>
        ///-------------------------------------------------------------------------------------------------

        private PrsResultsMapper Mapper { get; set; } = new PrsResultsMapper();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the algorithm version processor. </summary>
        ///
        /// <value> The algorithm version processor. </value>
        ///-------------------------------------------------------------------------------------------------

        private AlgorithmVersionProcessor AlgorithmVersionProcessor { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the results ready processor. </summary>
        ///
        /// <value> The results ready processor. </value>
        ///-------------------------------------------------------------------------------------------------

        private ResultsReadyProcessor ResultsReadyProcessor { get; set; }

        public bool SkipResultsPxCorrelation { get; set; } = false;


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalBusinessLayer.PrsResultsProcessor class.
        /// </summary>
        ///
        /// <remarks>   Dtorres, 20160605. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public PrsResultsProcessor() : base()
        {
            AlgorithmVersionProcessor = new AlgorithmVersionProcessor();
            ResultsReadyProcessor = new ResultsReadyProcessor();
            Mapper.Configure();
        }



        public PrsResultsProcessor(string analyticsConnString, string sgnlLisConnString, string sgnlWarehouseConnString) : base()
        {
            Mapper.Configure();
            AnalyticsDbContext = new SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities(analyticsConnString);
            LISDbContext = new SignalEFDataModel.SGNL_LIS.SGNL_LISEntities(sgnlLisConnString);
            WarehouseDbContext = new SignalEFDataModel.SGNL_WAREHOUSE.SGNL_WAREHOUSEEntities(sgnlWarehouseConnString);
            AlgorithmVersionProcessor = new AlgorithmVersionProcessor(AnalyticsDbContext);
            ResultsReadyProcessor = new ResultsReadyProcessor(AnalyticsDbContext);
        }

        public PrsResultsProcessor(SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities anaDbContext,
                                    SignalEFDataModel.SGNL_LIS.SGNL_LISEntities lisDbContext,
                                    SignalEFDataModel.SGNL_WAREHOUSE.SGNL_WAREHOUSEEntities warehouseDbContext
                                    ) : base()
        {
            Mapper.Configure();
            AnalyticsDbContext = anaDbContext;
            LISDbContext = lisDbContext;
            WarehouseDbContext = warehouseDbContext;
            AlgorithmVersionProcessor = new AlgorithmVersionProcessor(AnalyticsDbContext);
            ResultsReadyProcessor = new ResultsReadyProcessor(AnalyticsDbContext);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets prs result status. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160923. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        /// <exception cref="Exception">                Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="caseNumber">   The case number. </param>
        /// <param name="qns">          The qns. </param>
        /// <param name="isCancelled">  true if this object is cancelled. </param>
        ///
        /// <returns>   The prs result status. </returns>
        ///-------------------------------------------------------------------------------------------------

        public PrsResultStatus GetPrsResultStatus(string caseNumber, string qns, bool isCancelled)
        {
            Debug.Assert(caseNumber != null);
            if (caseNumber == null)
                throw new ArgumentNullException("Casenumber argument was null");

            ResultsReadyCombined rrc;
            bool isResultsReady = ResultsReadyProcessor.TryGetQCResults(caseNumber, out rrc);


            PrsResultStatus status = PrsResultStatus.Unknown;

            if (isCancelled)
                status = PrsResultStatus.CaseCancelled;

            else if (qns.ToLower() == "true")
                status = PrsResultStatus.QnsFailure;
                        
            else if (!isResultsReady || rrc == null || rrc.SGNLResultsAvailable.GetValueOrDefault() == false || rrc.SGNLQCAvailable.GetValueOrDefault() == false )
                status = PrsResultStatus.ResultsNotReady;

            else if (SkipResultsPxCorrelation == false && (rrc.ResultsPxAvailable.GetValueOrDefault() == false || rrc.ResultsPxQCAvailable.GetValueOrDefault() == false))
                status = PrsResultStatus.ResultsNotReady;

            else if (SkipResultsPxCorrelation == false && rrc.ResultsMatch.HasValue && rrc.ResultsMatch.Value == false)
                status = PrsResultStatus.ResultsPx_ResultDiscrepency;

            else if (SkipResultsPxCorrelation == false && rrc.QCEvalMatch.HasValue && rrc.QCEvalMatch.Value == false)
                status = PrsResultStatus.ResultsPx_QCDiscrepency;

            else if (rrc.SGNLQCPass.HasValue && rrc.SGNLQCPass.Value == false)
                status = PrsResultStatus.QCFailure;

            else
                status = PrsResultStatus.OK;


            Debug.Assert(status != PrsResultStatus.Unknown);
            if (status == PrsResultStatus.Unknown)
                throw new Exception("PrsResultStatus was Unknown. This should not happen.");

            return status;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets prs result status. </summary>
        ///
        /// <remarks>   Dtorres, 20160720. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        /// <exception cref="Exception">                Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="prsResult">    The casenumber. </param>
        ///
        /// <returns>   The prs result status. </returns>
        ///-------------------------------------------------------------------------------------------------

        public PrsResultStatus GetPrsResultStatus(PrsResult prsResult)
        {
            Debug.Assert(prsResult != null);
            if (prsResult == null)
                throw new ArgumentNullException("Casenumber argument was null");
            return GetPrsResultStatus(prsResult.CaseNumber, prsResult.QNS, prsResult.IsCancelled);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets prs result. </summary>
        ///
        /// <remarks>   Dtorres, 20160607. </remarks>
        ///
        /// <param name="caseNumber">   The case number. </param>
        ///
        /// <returns>   The prs result. </returns>
        ///-------------------------------------------------------------------------------------------------

        public PrsResult GetPrsResult(string caseNumber,
                string gep70AlgorithmVersion = null,
               string subtypeAlgorithmVersion = null,
               string vkAlgorithmVersion = null)
        {
            return GetPrsResults(
                new string[] { caseNumber },
                gep70AlgorithmVersion,
                subtypeAlgorithmVersion,
                vkAlgorithmVersion).FirstOrDefault();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets prs results. </summary>
        ///
        /// <remarks>   Dtorres, 20160720. </remarks>
        ///
        /// <exception cref="ArgumentNullException">    Thrown when one or more required arguments are
        ///                                             null. </exception>
        ///
        /// <param name="casenumbers">              The casenumbers. </param>
        /// <param name="gep70AlgorithmVersion">    The gep 70 algorithm version. </param>
        /// <param name="subtypeAlgorithmVersion">  The subtype algorithm version. </param>
        /// <param name="vkAlgorithmVersion">       The vk algorithm version. </param>
        ///
        /// <returns>   The prs results. </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<PrsResult> GetPrsResults(IEnumerable<string> casenumbers,
            string gep70AlgorithmVersion = null,
            string subtypeAlgorithmVersion = null,
            string vkAlgorithmVersion = null)
        {
            Debug.Assert(casenumbers != null);
            if (casenumbers == null)
                throw new ArgumentNullException("casenumbers list was was null");


            string GEP70_AlgorithmVersion = gep70AlgorithmVersion ?? AlgorithmVersionProcessor.GetCurrentAlgorithmVersion(TestType_MyPrs, AlgorithmName_GEP70RiskScore).Version;
            string Subtype_AlgorithmVersion = subtypeAlgorithmVersion ?? AlgorithmVersionProcessor.GetCurrentAlgorithmVersion(TestType_MyPrs, AlgorithmName_SubType).Version;
            string VK_AlgorithmVersion = vkAlgorithmVersion ?? AlgorithmVersionProcessor.GetCurrentAlgorithmVersion(TestType_MyPrs, AlgorithmName_VK).Version;


            //! OPTIMIZATION - Note because we may not always have results from analytics we can't simply process both results lists in parallel 
            // (we can't guarantee they will be in one-to-one correspondence.) To avoid hitting the database for each patient result in a for-loop, 
            // we pull both result sets in memory and perform the querying in memory 


            //get analytics results - these could have elements missing
            List<EF.SGNL_ANALYTICS.vi_GetAnalyticsCaseResults> resultsAnalytics =
                AnalyticsDbContext.vi_GetAnalyticsCaseResults
                .Where(
                    r => casenumbers.Contains(r.CaseNumber)
                    && r.GEP70_AlgorithmVersion == GEP70_AlgorithmVersion
                    && r.Subtype_AlgorithmVersion == Subtype_AlgorithmVersion
                    && r.VK_AlgorithmVersion == VK_AlgorithmVersion
                )
                .OrderByDescending(r => r.CaseNumber)
                .ToList();
            Debug.Assert(resultsAnalytics != null);
            logger.Debug($"Analytics Results: (AnalyticsDbContext.vi_GetAnalyticsCaseResults) {resultsAnalytics.Count}");

            //get lis results - we should always have these results for a casenumber.
            List<EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo> resultsLis =
                LISDbContext.vi_GetAnalyticsCasePatientInfo
                .Where(p => casenumbers.Contains(p.CaseNumber))
                .OrderByDescending(p => p.CaseNumber)
                .ToList();
            Debug.Assert(resultsLis != null);
            logger.Debug($"Patient Results: (LISDbContext.vi_GetAnalyticsCasePatientInfo) {resultsLis.Count}");



            //do the merging and mapping
            List<PrsResult> prsResults = new List<PrsResult>();
            foreach (EF.SGNL_LIS.vi_GetAnalyticsCasePatientInfo patientResult in resultsLis)
            {
                EF.SGNL_ANALYTICS.vi_GetAnalyticsCaseResults analyticsResult = resultsAnalytics.Where(r => r.CaseNumber == patientResult.CaseNumber).FirstOrDefault();
                prsResults.Add(Mapper.Map(analyticsResult, patientResult));
            }
            return prsResults;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets prs results summary for case. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160915. </remarks>
        ///
        /// <param name="caseNumber">   The case number. </param>
        ///
        /// <returns>   The prs results summary for case. </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<PrsResultsSummary> GetPrsResultsSummaryForCase(List<string> caseNumbers)
        {
            var anaInfo = (from results in AnalyticsDbContext.vi_GetAnalyticsCaseResultsSummaries
                           where caseNumbers.Contains(results.CaseNumber)
                           select results).ToList();

            var warehouseInfo = (from summary in WarehouseDbContext.vi_PxPortalUserCases
                                 where caseNumbers.Contains(summary.CaseNumber)
                                 select summary).ToList();

            var caseInfo = (from results in (
                                from summary in warehouseInfo
                                join prs in anaInfo on summary.CaseNumber equals prs.CaseNumber into prsResultGroup
                                from prsResult in anaInfo.DefaultIfEmpty()
                                select new { Summary = summary, Results = prsResult ?? new EF.SGNL_ANALYTICS.vi_GetAnalyticsCaseResultsSummaries() }
                                ).ToList()
                            select new PrsResultsSummary()
                            {
                                CaseNumber = results.Summary.CaseNumber,
                                CompletedDate = results.Summary.CompletedDate,
                                OrderDate = results.Summary.OrderDate,
                                ClientName = results.Summary.ClientName,
                                OrderingPhysicianFullName = results.Summary.OrderingPhysicianFullName ?? String.Empty,
                                PatientFullName = results.Summary.PatientFullName ?? String.Empty,
                                IsQNS = results.Summary.QNS ?? false,
                                HasCelFile = results.Results.HasCELFile ?? false,
                                HasChipImage = results.Results.HasChipImage ?? false,
                                StatusTag = GetPxPortalWorkflowStatus(results.Summary.WorkflowStep).ToString(),
                                ResultsReady = GetSummaryResultsReady(results.Summary.IsCancelled ?? false, results.Summary.QNS ?? false, results.Results.ResultsReady ?? false),
                                ReportReady = GetPxPortalWorkflowStatus(results.Summary.WorkflowStep ?? String.Empty) == PxPortalCaseStatus.ResultsReady,
                                WorkflowStatus = results.Summary.WorkflowStep ?? String.Empty,
                                ReadyToReport = (GetPxPortalWorkflowStatus(results.Summary.WorkflowStep ?? String.Empty) == PxPortalCaseStatus.ResultsReady)
                                                && GetSummaryResultsReady(results.Summary.IsCancelled ?? false, results.Summary.QNS ?? false, results.Results.ResultsReady ?? false),
                            }).ToList().OrderBy(y => y.OrderDate).GroupBy(x => x.CaseNumber).Select(grp => grp.FirstOrDefault()).ToList();
            return caseInfo;
        }

        private List<Report> GetReportObjects(string reportObjects)
        {
            var reports = new List<Report>();
            if (!String.IsNullOrEmpty(reportObjects))
            {
                foreach (var report in reportObjects.Split('|').ToList())
                {
                    reports.Add(JsonDeserializer.DeserializeJson<Report>(report));
                }
            }
            return reports;
        }

        private PxPortalCaseStatus GetPxPortalWorkflowStatus(string workflowStep)
        {
            PrsResultsWorkflowStep step;
            PxPortalCaseStatus status = PxPortalCaseStatus.Unknown;
            if (workflowStep != null && workflowStep != String.Empty)
            {
                workflowStep = workflowStep.Replace(" ", "_").Replace("-", "_").Replace(@"/", "_");
            }
            else
            {
                return status;
            }
            if (Enum.TryParse(workflowStep, out step))
            {
                switch (step)
                {
                    case PrsResultsWorkflowStep.Received:
                        status = PxPortalCaseStatus.SpecimenReceived;
                        break;
                    case PrsResultsWorkflowStep.Additional_Purification:
                    case PrsResultsWorkflowStep.cDNA_Synthesis:
                    case PrsResultsWorkflowStep.cRNA_Cleanup:
                    case PrsResultsWorkflowStep.cRNA_Fragmentation:
                    case PrsResultsWorkflowStep.Distribution:
                    case PrsResultsWorkflowStep.Hybridization:
                    case PrsResultsWorkflowStep.IVT:
                    case PrsResultsWorkflowStep.NA_Isolation:
                    case PrsResultsWorkflowStep.Post_Sort_Flow_Analysis:
                    case PrsResultsWorkflowStep.Pre_Sort_Flow_Analysis:
                    case PrsResultsWorkflowStep.Professional:
                    case PrsResultsWorkflowStep.RNA_Isolation:
                    case PrsResultsWorkflowStep.Wash_Stain_Scan:
                        status = PxPortalCaseStatus.Processing;
                        break;
                    case PrsResultsWorkflowStep.Cancelled:
                        status = PxPortalCaseStatus.Cancelled;
                        break;
                    //case PrsResultsWorkflowStep.Post_Sort_Flow_Analysis:
                    //    status = PxPortalCaseStatus.InReview;
                    //    break;
                    case PrsResultsWorkflowStep.Complete:
                        status = PxPortalCaseStatus.ResultsReady;
                        break;
                };
            }
            return status;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets results ready. Indicates that a results is ready to show to user.</summary>
        ///
        /// <remarks>   Dtorres, 20160923. </remarks>
        ///
        /// <param name="prsResult">    The casenumber. </param>
        ///
        /// <returns>   true if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        private bool GetResultsReady(PrsResult prsResult)
        {
            return GetResultsReady(prsResult.CaseNumber, prsResult.QNS, prsResult.IsCancelled);
        }

        private bool GetSummaryResultsReady(bool isCancelled, bool isQns, bool qcResultsReady)
        {
            if (isCancelled == true)
                return true;
            else if (isQns == true)
                return true;
            else return qcResultsReady;
        }

        private bool GetResultsReady(string caseNumber, string qns, bool isCancelled)
        {
            bool value = false;
            switch (GetPrsResultStatus(caseNumber, qns, isCancelled))
            {
                case PrsResultStatus.OK:
                case PrsResultStatus.QCFailure:
                case PrsResultStatus.QnsFailure:
                case PrsResultStatus.CaseCancelled:
                    value = true;
                    break;
            };
            return value;
        }
    }


}
