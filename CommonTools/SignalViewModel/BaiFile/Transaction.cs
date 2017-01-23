using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.BaiFile
{
    public class Transaction
    {
        public int id { get; set; }
        public int BaiAccountId { get; set; }
        public string SenderIdentification { get; set; }
        public string RecordCode { get; set; }
        public string TypeCode { get; set; }
        public string Amount { get; set; }
        public string FundsType { get; set; }
        public string BankReferenceNumber { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string Text { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> LatePayment { get; set; }
        public Nullable<decimal> Adjustment { get; set; }
        public Nullable<decimal> Unreconciled { get; set; }
        public Nullable<decimal> OtherDeposit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> RollOver { get; set; }
        public string DepositId { get; set; }
        public Boolean IsSelected { get; set; }
    }
}
