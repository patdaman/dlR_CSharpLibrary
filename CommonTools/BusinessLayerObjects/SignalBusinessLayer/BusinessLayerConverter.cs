using CommonUtils.Reflection;
using ViewModel;
using ViewModel.BaiFile;
using System;
using System.Collections.Generic;
using EF = EFDataModel;

namespace BusinessLayer
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   Helper class to convert back and forth between EF and view model objects.</summary>
    ///
    /// <remarks>   Dtorres, 20150925. </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class BusinessLayerConverter
    {
        public static IEnumerable<EF.SGNL_ANALYTICS.SignalEvent> CreateSignalEventList(IEnumerable<ViewModel.Events.Event> results)
        {
            var list = new List<EF.SGNL_ANALYTICS.SignalEvent>();
            foreach (ViewModel.Events.Event item in results)
            {
                list.Add(CreateSignalEvent(item));
            }
            return list;
        }

        public static EF.SGNL_ANALYTICS.SignalEvent CreateSignalEvent(ViewModel.Events.Event frameworkItem)
        {
            var newItem = new EF.SGNL_ANALYTICS.SignalEvent();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static List<BillingStatusCase> CreateBillingStatusCaseList(List<EF.SGNL_WAREHOUSE.usp_GetBillingSuiteCase_Result> results)
        {
            var list = new List<BillingStatusCase>();
            foreach (EF.SGNL_WAREHOUSE.usp_GetBillingSuiteCase_Result item in results)
            {
                list.Add(CreateBillingStatusCase(item));
            }
            return list;
        }

        public static BillingStatusCase CreateBillingStatusCase(EF.SGNL_WAREHOUSE.usp_GetBillingSuiteCase_Result frameworkItem)
        {
            var newItem = new BillingStatusCase();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static List<BillingStatusCase> CreateApiBillingStatusCaseList(List<EF.SGNL_WAREHOUSE.vi_apiGetBillingStatusCase> results)
        {
            var list = new List<BillingStatusCase>();
            foreach (EF.SGNL_WAREHOUSE.vi_apiGetBillingStatusCase item in results)
            {
                list.Add(CreateApiBillingStatusCase(item));
            }
            return list;
        }

        public static BillingStatusCase CreateApiBillingStatusCase(EF.SGNL_WAREHOUSE.vi_apiGetBillingStatusCase frameworkItem)
        {
            var newItem = new BillingStatusCase();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static BaiFile CreateBai(EF.SGNL_FINANCE.BaiFile frameworkItem)
        {
            var newItem = new BaiFile();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static BaiFileInfo CreateBaiFileInfo(EF.SGNL_FINANCE.usp_BaiFileList_Result frameworkItem)
        {
            var newItem = new BaiFileInfo();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static List<BaiFileInfo> CreateBaiFileInfoList(List<EF.SGNL_FINANCE.usp_BaiFileList_Result> frameworkItem)
        {
            var list = new List<BaiFileInfo>();
            foreach (EF.SGNL_FINANCE.usp_BaiFileList_Result item in frameworkItem)
            {
                list.Add(CreateBaiFileInfo(item));
            }
            return list;
        }

        public static TransactionInfo CreateTransaction(EF.SGNL_FINANCE.usp_GetTransactionLines_Result frameworkItem)
        {
            var newItem = new TransactionInfo();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static TransactionInfo CreateTransactionId(EF.SGNL_FINANCE.usp_GetTransactionId_Result frameworkItem)
        {
            var newItem = new TransactionInfo();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static List<TransactionInfo> CreateTransactionList(List<EF.SGNL_FINANCE.usp_GetTransactionLines_Result> frameworkItem)
        {
            var list = new List<TransactionInfo>();
            foreach (EF.SGNL_FINANCE.usp_GetTransactionLines_Result item in frameworkItem)
            {
                list.Add(CreateTransaction(item));
            }
            return list;
        }

        public static List<Note> CreateNoteList(List<EF.SGNL_LIS.Note> results)
        {
            var list = new List<Note>();
            foreach (EF.SGNL_LIS.Note item in results)
            {
                list.Add(CreateNote(item));
            }
            return list;
        }

        public static Note CreateNote(EF.SGNL_LIS.Note frameworkItem)
        {
            var newItem = new Note();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static CaseTransactionInfo CreateCaseTransaction(EF.SGNL_FINANCE.usp_GetCaseTransaction_Result frameworkItem)
        {
            var newItem = new CaseTransactionInfo();
            frameworkItem.CopyPropertiesTo(newItem);
            return newItem;
        }

        public static List<CaseTransactionInfo> CreateCaseTransactionList(List<EF.SGNL_FINANCE.usp_GetCaseTransaction_Result> frameworkItem)
        {
            var list = new List<CaseTransactionInfo>();
            foreach (EF.SGNL_FINANCE.usp_GetCaseTransaction_Result item in frameworkItem)
            {
                list.Add(CreateCaseTransaction(item));
            }
            return list;
        }

        internal static TransactionInfo CreateBaiTransaction(EF.SGNL_FINANCE.BaiTransaction transaction)
        {
            throw new NotImplementedException();
        }

        internal static TransactionData CreateTransactionData(EF.SGNL_FINANCE.usp_GetTransaction_Result transaction)
        {
            var newItem = new TransactionData();
            transaction.CopyPropertiesTo(newItem);
            return newItem;
        }
    }
}
