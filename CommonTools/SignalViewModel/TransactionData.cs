using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class TransactionData
    {
        public int TransactionId { get; set; }
        public int BaiAccountSummaryId { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> TotCollections { get; set; }
        public Nullable<decimal> InterestPayment { get; set; }
        public Nullable<decimal> TransactionLatePayment { get; set; }
        public Nullable<decimal> TotAmount { get; set; }
        public Nullable<decimal> BankFee { get; set; }
        public Nullable<int> CaseCount { get; set; }
        public Nullable<decimal> CaseAmountApplied { get; set; }
        public Nullable<decimal> CaseLatePayment { get; set; }
        public Nullable<decimal> CaseInterestPayment { get; set; }
        public Nullable<decimal> CaseTotApplied { get; set; }
        public Nullable<decimal> RemainingBalance { get; set; }
        public string DepositId { get; set; }
        public string LastModifiedUser { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
