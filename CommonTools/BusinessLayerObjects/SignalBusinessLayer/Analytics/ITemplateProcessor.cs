using ViewModel;
using ViewModel.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public interface ITemplateProcessor
    {
        ClinicalReportTemplate GetReportTemplate(string reportTypeName, string reportVersion, string reportTemplateName);
    }
}
