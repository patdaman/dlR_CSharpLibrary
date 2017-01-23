using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.BaiFile
{
    public class TransactionInfo
    {
        public int TransactionId { get; set; }
        public Nullable<int> BaiFileId { get; set; }
        public Nullable<int> BaiAccountGroupId { get; set; }
        public Nullable<int> CaseCount { get; set; }
        public string OriginatorIdentification { get; set; }
        public string UltimateReceiverIdentification { get; set; }
        public Nullable<int> GroupStatus { get; set; }
        public Nullable<System.DateTime> AsOfDate { get; set; }
        public string AsOfTime { get; set; }
        public Nullable<int> AsOfDateModifier { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string CurrencyCode { get; set; }
        public string TypeCodeAmount { get; set; }
        public Nullable<int> ItemCount { get; set; }
        public string TypeCodeFundsType { get; set; }
        public string TypeCode { get; set; }
        public string TypeCodeDescription { get; set; }
        public string TransactionTypeCode { get; set; }
        public string TransactionFundsType { get; set; }
        public string BankReferenceNumber { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public Nullable<decimal> TransactionAmount { get; set; }
        public string DebitCredit { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> LatePayment { get; set; }
        public Nullable<decimal> OtherDeposit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string DepositId { get; set; }
        public string LastModifiedUser { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
}
