using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.BaiFile
{
    public class AccountGroup
    {
        public string RecordCode { get; set; }
        public string UltimateReceiverIdentification { get; set; }
        public string OriginatorIdentification { get; set; }
        public string GroupStatus { get; set; }
        public string AsOfDate { get; set; }
        public string AsOfTime { get; set; }
        public string CurrencyCode { get; set; }
        public string AsOfDateModifier { get; set; }

        public List<AccountSummary> AccountSummaryList { get; set; }
    }
}
