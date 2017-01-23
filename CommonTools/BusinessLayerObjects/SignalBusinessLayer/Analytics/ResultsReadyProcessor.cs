///-------------------------------------------------------------------------------------------------
// <copyright file="ResultsReadyProcessor.cs" company="Signal Genetics Inc.">
// Copyright (c) 2016 Signal Genetics Inc.. All rights reserved.
// </copyright>
// <author>Dtorres</author>
// <date>20160720</date>
// <summary>Implements the results ready processor class</summary>
///-------------------------------------------------------------------------------------------------

using AppDataLib.Exceptions;
using ViewModel.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF = SignalEFDataModel;

namespace BusinessLayer.Analytics
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   The results ready processor. </summary>
    ///
    /// <remarks>   Dtorres, 20160720. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class ResultsReadyProcessor : BehaviorBase
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalBusinessLayer.Analytics.ResultsReadyProcessor
        /// class.
        /// </summary>
        ///
        /// <remarks>   Dtorres, 20160720. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public ResultsReadyProcessor() : base()
        {

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalBusinessLayer.Analytics.ResultsReadyProcessor
        /// class.
        /// </summary>
        ///
        /// <remarks>   Dtorres, 20160720. </remarks>
        ///
        /// <param name="analyticsConnectionString">    The analytics connection string. </param>
        ///-------------------------------------------------------------------------------------------------

        public ResultsReadyProcessor(string analyticsConnectionString)
        {
            this.AnalyticsDbContext = new SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities(analyticsConnectionString);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the SignalBusinessLayer.Analytics.ResultsReadyProcessor
        /// class.
        /// </summary>
        ///
        /// <remarks>   Dtorres, 20160720. </remarks>
        ///
        /// <param name="context">  The context. </param>
        ///-------------------------------------------------------------------------------------------------

        public ResultsReadyProcessor(SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities context)
        {
            this.AnalyticsDbContext = context;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Query if 'casenumber' is qc pass. </summary>
        ///
        /// <remarks>   Dtorres, 20160720. </remarks>
        ///
        /// <exception cref="SgnlCaseNotFoundException">    Thrown when a signal Case Not Found error
        ///                                                 condition occurs. </exception>
        ///
        /// <param name="casenumber">   The casenumber. </param>
        ///
        /// <returns>   true if qc pass, false if not. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool IsQCPass(string casenumber)
        {

            EF.SGNL_ANALYTICS.vi_ResultsReadyCombined  rrc = AnalyticsDbContext.vi_ResultsReadyCombined.Where(r => r.CaseNumber == casenumber).SingleOrDefault();
            if (rrc == null)
                throw new SgnlCaseNotFoundException(casenumber, $"Results ready entry not available for {casenumber}.");

            if (!rrc.ResultsMatch.HasValue || !rrc.SGNLQCPass.HasValue || !rrc.QCEvalMatch.HasValue)
                return false;
            return (rrc.ResultsMatch.Value && rrc.SGNLQCPass.Value && rrc.QCEvalMatch.Value);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets qc results. </summary>
        ///
        /// <remarks>   Dtorres, 20160720. </remarks>
        ///
        /// <exception cref="SgnlCaseNotFoundException">    Thrown when a signal Case Not Found error
        ///                                                 condition occurs. </exception>
        ///
        /// <param name="casenumber">   The casenumber. </param>
        ///
        /// <returns>   The qc results. </returns>
        /// 
        ///-------------------------------------------------------------------------------------------------

        public ResultsReadyCombined GetQCResults(string casenumber)
        {
            EF.SGNL_ANALYTICS.vi_ResultsReadyCombined rrcEF = AnalyticsDbContext.vi_ResultsReadyCombined.Where(r => r.CaseNumber == casenumber).SingleOrDefault();
            if (rrcEF == null)

                throw new SgnlCaseNotFoundException(casenumber, "Results Ready record not available for case " + casenumber);

            ResultsReadyCombined rrc = new ResultsReadyCombined();
            rrc.CaseNumber = rrcEF.CaseNumber;
            rrc.SGNLResultsAvailable = rrcEF.SGNLResultsAvailable;
            rrc.ResultsPxAvailable = rrcEF.ResultsPxQCAvailable;
            rrc.ResultsMatch = rrcEF.ResultsMatch;
            rrc.ResultDiscrepancies = rrcEF.ResultDiscrepancies;
            rrc.SGNLQCAvailable = rrcEF.SGNLQCAvailable;
            rrc.SGNLQCPass = rrcEF.SGNLQCPass;
            rrc.ResultsPxQCAvailable = rrcEF.ResultsPxQCAvailable;
            rrc.ResultsPxQCPass = rrcEF.ResultsPxQCPass;
            rrc.QCMatch = rrcEF.QCMatch;
            rrc.QCEvalMatch = rrcEF.QCEvalMatch;
            rrc.QCDiscrepancies = rrcEF.QCDiscrepancies;

            return rrc;
        }


        public bool TryGetQCResults(string casenumber, out ResultsReadyCombined rrc)
        {            
            try
            {
                rrc = GetQCResults(casenumber);
                return true; 
            }
            catch(SgnlCaseNotFoundException)
            {
                rrc = null;
                return false;
            }
            
        }

    }

}

