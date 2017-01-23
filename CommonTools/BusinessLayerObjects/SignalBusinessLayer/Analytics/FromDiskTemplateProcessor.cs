using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using System.Diagnostics;
using SignalEFDataModel.SGNL_ANALYTICS;
using AppDataLib.Exceptions;
using System.IO;
using ViewModel.Analytics;

namespace BusinessLayer.Analytics
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   template processor that returns a template from disk. This is basically used 
    ///             in place of the database-based template processor when doing development, design and 
    ///             testing.</summary>
    ///
    /// <remarks>   Dtorres, 20160705. </remarks>
    ///-------------------------------------------------------------------------------------------------

    class FromDiskTemplateProcessor : BehaviorBase, ITemplateProcessor
    {

        public string PathToPdf { get; set; }
        

        public FromDiskTemplateProcessor(string pathToPdf, string analyticsConnectionString)
        {
            Debug.Assert(pathToPdf != null);
            PathToPdf = pathToPdf;

            this.AnalyticsDbContext = new SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities(analyticsConnectionString);
        }

        public ClinicalReportTemplate GetReportTemplate(string reportTypeName, string reportTypeVersion, string reportTemplateName)
        {
            if (reportTypeName == null || reportTypeVersion == null)
                throw new ArgumentNullException("At least one argument was null");

            ReportTemplate reportTemplate =
                AnalyticsDbContext.ReportTypes.Where(r => r.Name == reportTypeName && r.Version == reportTypeVersion)
                .SelectMany(r => r.ReportTemplates)
                .Where(rt => rt.Name == reportTemplateName).FirstOrDefault();

            if (reportTemplate == null)
                throw new SgnlReportTypeNotFoundException($"Unknown reportTypeName ({reportTypeName}), reportVersion ({reportTypeVersion}) or reportTemplateName ({reportTemplateName})");


            List<ReportField> fields = GenerateReportFields(reportTemplate);
            if (fields == null || fields.Count == 0)
                throw new Exception($"No report fields found for given parameters. reportTypeName ({reportTypeName}), reportVersion ({reportTypeVersion}) or reportTemplateName ({reportTemplateName})");


            //Get report from disk ...
            throw new NotImplementedException("Need to get report from disk.");

            var template = new ClinicalReportTemplate(
                reportTypeName: reportTypeName,
                reportTypeVersion: reportTypeVersion,
                reportTemplateName: reportTemplate.Name,
                reportTemplateVersion: reportTemplate.Version,
                templatePdf: new MemoryStream(reportTemplate.Report),
                fields: fields,
                signatures: null,
                signatureText: null
                );
            return template;

            //XXX HERE!!

        }

        private List<ReportField> GenerateReportFields(ReportTemplate reportTemplate)
        {
            throw new NotImplementedException();
        }

        public ReportSignature GetReportSignature(string displayName)
        {
            throw new NotImplementedException();
        }
    }
}
