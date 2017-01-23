using ViewModel.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF = SignalEFDataModel;

namespace BusinessLayer.Analytics
{
    public class AlgorithmVersionProcessor : BehaviorBase
    {
        public AlgorithmVersionProcessor() : base()
        {

        }

        public AlgorithmVersionProcessor(string analyticsConnectionString)
        {
            this.AnalyticsDbContext = new SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities(analyticsConnectionString);
        }

        public AlgorithmVersionProcessor(SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities context)
        {
            this.AnalyticsDbContext = context;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets current algorithm version. </summary>
        ///
        /// <remarks>   Dtorres, 20160712. </remarks>
        ///
        /// <exception cref="Exception">    Thrown when an exception error condition occurs. </exception>
        ///
        /// <param name="testType"> Type of the test. (See CAEnums.cs TestTypeValues) </param>
        /// <param name="algName">  Name of the algorithm. (See CAEnums.cs AlgorithmNames) </param>
        ///
        /// <returns>   The current algorithm version. </returns>
        ///-------------------------------------------------------------------------------------------------

        public AlgorithmVersion GetCurrentAlgorithmVersion(string testType, string algName)
        {
            EF.SGNL_ANALYTICS.AlgorithmVersion avEF = AnalyticsDbContext.AlgorithmVersions.Where(a => a.TestType == testType && a.Name == algName).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
            if (avEF == null)
                throw new Exception("Algorithm Version record not available for " + algName);

            AlgorithmVersion av = new AlgorithmVersion();
            av.id = avEF.id;
            av.Name = avEF.Name;
            av.TestType = avEF.TestType;
            av.Version = avEF.Version;
            av.ProbesetPanelList = avEF.ProbesetPanelList;
            av.AnalyticsObjectList = avEF.AnalyticsObjectList;
            av.CreatedDate = avEF.CreatedDate;
            av.LastModifiedDate = avEF.LastModifiedDate;
            av.LastModifiedUser = avEF.LastModifiedUser;
            av.HashNumber = avEF.HashNumber;

            return av;
        }
    }
}
