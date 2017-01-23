using System;
using System.Collections.Generic;

namespace ViewModel
{
    public partial class Report
    {
        public string ReportTemplateId { get; set; }
        public string ReportTemplateName { get; set; }
        public string ReportTypeName { get; set; }
        public string SignedDate { get; set; }
        public string Version { get; set; }
        public string VersionTypeName { get; set; }
        public string SignedUserId { get; set; }
    }
}
