using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.BaiFile
{
    public class AccountSummary
    {
        public string RecordCode { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string CurrencyCode { get; set; }

        public List<TypeCodes> TypeCodeList { get; set; }

        public List<Transaction> TransactionList { get; set; }
    }
}
