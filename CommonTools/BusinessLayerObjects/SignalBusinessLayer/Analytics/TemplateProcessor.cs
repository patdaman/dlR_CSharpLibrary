using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;
using SignalEFDataModel.SGNL_ANALYTICS;
using System.IO;
using AppDataLib.Exceptions;
using ViewModel.Analytics;
using AutoMapper;
using System.Diagnostics;

namespace BusinessLayer
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A template processor. Interacts with databse / datastore to provide templates  </summary>
    ///
    /// <remarks>   Dtorres, 20160603. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class TemplateProcessor : BehaviorBase, ITemplateProcessor
    {

        private IMapper Mapper { get; set; }

        public TemplateProcessor() : base()
        {
            ConfigureMapper(); 
        }

        private void ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SignalEFDataModel.SGNL_ANALYTICS.Signature, ReportSignature>();                
            });
            Mapper = config.CreateMapper();
        }

        public TemplateProcessor(string analyticsConnectionString)
        {
            this.AnalyticsDbContext = new SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities(analyticsConnectionString);
            ConfigureMapper();
        }

        public TemplateProcessor(SignalEFDataModel.SGNL_ANALYTICS.SGNL_ANALYTICSEntities context)
        {
            this.AnalyticsDbContext = context;
            ConfigureMapper();
        }


        public ClinicalReportTemplate GetReportTemplate( string reportTypeName, string reportTypeVersion, string reportTemplateName )
        {
            if (reportTypeName == null || reportTypeVersion == null)
                throw new ArgumentNullException("At least one argument was null");

            ReportTemplate reportTemplate = 
                AnalyticsDbContext.ReportTypes.Where(r => r.Name == reportTypeName && r.Version == reportTypeVersion)
                .SelectMany(r => r.ReportTemplates)
                .Where(rt => rt.Name == reportTemplateName).FirstOrDefault(); 

            if( reportTemplate == null )
                throw new SgnlReportTypeNotFoundException($"Unknown reportTypeName ({reportTypeName}), reportVersion ({reportTypeVersion}) or reportTemplateName ({reportTemplateName})");


            List<ReportField> fields = GenerateReportFields(reportTemplate);

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
        }


        public ReportSignature GetReportSignature( string displayName )
        {
            if( string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("Invalid argument. Was null or blank");
            }

            SignalEFDataModel.SGNL_ANALYTICS.Signature signatureEF = AnalyticsDbContext.Signatures
                .Where(s => s.DisplayName == displayName)
                .First();

            ReportSignature reportSignature = Mapper.Map<ReportSignature>(signatureEF);
            Debug.Assert(reportSignature != null);
            return reportSignature;
        }

        

        private List<ReportField> GenerateReportFields(ReportTemplate reportTemplate)
        {
            List<ReportField> list = new List<ReportField>();
            foreach (SignalEFDataModel.SGNL_ANALYTICS.FieldMapping mapping in reportTemplate.FieldMappings)
            {
                list.Add(new ReportField()
                {
                    FieldPath = mapping.Path,
                    FieldType = mapping.Type,
                    FormatString = mapping.Format,
                    TemplateFieldTag = mapping.Field
                });
            }
            return list;
        }
    }
}
