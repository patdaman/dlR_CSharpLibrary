
using DataLayer;
using SignalEFDataModel.SGNL_FINANCE;
using ViewModel;
using ViewModel.BaiFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using EF = SignalEFDataModel;

namespace BusinessLayer
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    /// The purpose of the .*Processor objects is to act as the glue code between EF objects and the
    /// View Models. It should handle getting, inserting, updating and removing EF/View Model types.
    /// This is how we abstract EF objects from all the layers above the SignalBusinessLayer.
    /// </summary>
    ///
    /// <remarks>   Dtorres, 20150923. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class TransactionProcessor : BehaviorBase
    {
        /// <summary>   The case classification repo. </summary>
        IRepository<BaiTransaction> Transaction_Repo;
        IRepository<BaiAccountSummary> Account_Repo;
        IRepository<usp_GetTransaction_Result> TransactionData_Repo;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   var context = new InternalDbContext() </summary>
        ///
        /// <remarks>   pdelosreyes, 20160104. </remarks>
        ///-------------------------------------------------------------------------------------------------

        public TransactionProcessor()
        {
            Transaction_Repo = new ConcreteRepository<BaiTransaction>(this.FinanceDbContext);
            Account_Repo = new ConcreteRepository<BaiAccountSummary>(this.FinanceDbContext);
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets Transaction file list. </summary>
        ///
        /// <remarks>   Pdelosreyes, 1/5/2016. </remarks>
        ///
        /// <exception cref="CustomException">  Thrown when a Signal error condition occurs. </exception>
        ///
        /// <returns>   The Transaction list. </returns>
        ///-------------------------------------------------------------------------------------------------

        public List<TransactionInfo> GetTransactionList()
        {
            string Payor = "";
            string Bank = "";
            string Transaction = "";

            try
            {
                List<usp_GetTransactionLines_Result> transactions = ((SGNL_FINANCEEntities)DBNameToEntities[EF.DBNames.SGNL_Finance]).usp_GetTransactionLines(
                    baiFileId: 0,
                    startDate: DateTime.Parse("1900-01-01"),
                    endDate: DateTime.Now.AddDays(1),
                    payor: Payor,
                    bank: Bank,
                    transaction: Transaction
                    ).ToList();
                if (transactions == null)
                    throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction list from database.");
                return BusinessLayerConverter.CreateTransactionList(transactions);
            }
            catch (Exception e)
            {
                throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction list from database.", e);
            }
        }

        public List<TransactionInfo> GetTransactions(DateTime? StartDate, DateTime? EndDate)
        {
            string Payor = "";
            string Bank = "";
            string Transaction = "";

            try
            {
                List<usp_GetTransactionLines_Result> transactions = ((SGNL_FINANCEEntities)DBNameToEntities[EF.DBNames.SGNL_Finance]).usp_GetTransactionLines(
                    baiFileId: 0,
                    startDate: StartDate,
                    endDate: EndDate,
                    payor: Payor,
                    bank: Bank,
                    transaction: Transaction
                    ).ToList();
                if (transactions == null)
                    throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction list from database.");
                return BusinessLayerConverter.CreateTransactionList(transactions);
            }
            catch (Exception e)
            {
                throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction list from database.", e);
            }
        }

        public TransactionInfo GetTransaction(int TransactionId)
        {
            try
            {
                usp_GetTransactionId_Result transaction = ((SGNL_FINANCEEntities)DBNameToEntities[EF.DBNames.SGNL_Finance]).usp_GetTransactionId(
                   TransactionId
                    ).Where(c => c.TransactionId == TransactionId).FirstOrDefault();
                if (transaction == null)
                    throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction " + TransactionId + " from database.");
                return BusinessLayerConverter.CreateTransactionId(transaction);
            }
            catch (Exception e)
            {
                throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction " + TransactionId + " from database. (*Please reload data and select transaction row in grid)", e);
            }
        }

        public bool IsTransactionDuplicate(TransactionInfo tx)
        {
            try
            {
                bool IsDuplicate = false;
                int cnt = ((SGNL_FINANCEEntities)DBNameToEntities[EF.DBNames.SGNL_Finance])
                    .usp_GetTransactionLines(null,null,null,null,null,null)
                   .Where(c => c.AsOfDate == tx.AsOfDate && c.Description == tx.Description && c.TransactionAmount == tx.TransactionAmount).Count();
                if (cnt > 1) IsDuplicate = true;
         
                return IsDuplicate;
            }
            catch (Exception e)
            {
                throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction " + tx.TransactionId + " from database.", e);
            }
        }

        public TransactionData GetTransactionData(int TransactionId)
        {
            try
            {
                usp_GetTransaction_Result transaction = ((SGNL_FINANCEEntities)DBNameToEntities[EF.DBNames.SGNL_Finance]).usp_GetTransaction(
                    TransactionId).Where(c => c.TransactionId == TransactionId).FirstOrDefault();
                if (transaction == null)
                    throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction " + TransactionId + " from database.");
                return BusinessLayerConverter.CreateTransactionData(transaction);
            }
            catch (Exception e)
            {
                throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction list from database.", e);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Updates the transaction described by theTransaction. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160212. </remarks>
        ///
        /// <exception cref="CustomException">  Thrown when a Signal error condition occurs. </exception>
        ///
        /// <param name="theTransaction">   the transaction. </param>
        ///
        /// <returns>   A TransactionInfo. </returns>
        ///-------------------------------------------------------------------------------------------------

        public TransactionInfo UpdateTransaction(TransactionInfo theTransaction)
        {
            if (theTransaction == null)
            {
                throw new CustomException(CustomExceptionTypes.UpdateFailure_Error,
               "Null transaction sent for update");
            }
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    BaiTransaction tran = Transaction_Repo.Query<BaiTransaction>(c => c.id == theTransaction.TransactionId).SingleOrDefault();
                    if (tran == null) //insert record
                    {
                        BaiTransaction newTransaction = new BaiTransaction();
                        newTransaction.Amount = theTransaction.TransactionAmount.ToString() ?? "0";
                        newTransaction.DepositID = theTransaction.DepositId ?? String.Empty;
                        newTransaction.Interest = theTransaction.Interest ?? 0;
                        newTransaction.LatePayment = theTransaction.LatePayment ?? 0;
                        newTransaction.OtherDeposit = theTransaction.OtherDeposit ?? 0;
                        newTransaction.LastModifiedDate = DateTime.Now;
                        newTransaction.LastModifiedUser = HttpContext.Current.User.Identity.Name;

                        Transaction_Repo.Insert(newTransaction);
                    }
                    if (tran != null) //update record
                    {
                        tran.DepositID = theTransaction.DepositId ?? String.Empty;
                        tran.Interest = theTransaction.Interest ?? 0;
                        tran.LatePayment = theTransaction.LatePayment ?? 0;
                        tran.OtherDeposit = theTransaction.OtherDeposit ?? 0;
                        tran.LastModifiedDate = DateTime.Now;
                        tran.LastModifiedUser = HttpContext.Current.User.Identity.Name;

                        Transaction_Repo.Update(tran);
                    }
                    ts.Complete();
                }
                return GetTransaction(theTransaction.TransactionId);
            }
            catch (TransactionAbortedException ex)
            {
                throw new CustomException(CustomExceptionTypes.TransactionFailure_Error,
                    "Transaction error in updating Transaction #: " + theTransaction.TransactionId.ToString(), ex);
            }
            catch (CustomException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomException(CustomExceptionTypes.UpdateFailure_Error,
                    "Error in updating Transaction #: " + theTransaction.TransactionId.ToString(), ex);
            }
        }

        public void DeleteTransaction(int TransactionId)
        {
            try
            {
                BaiTransaction t = Transaction_Repo.Query<BaiTransaction>(b => b.id == TransactionId).FirstOrDefault();
                if (t== null)
                    throw new CustomException(CustomExceptionTypes.RetrieveFailure_NotExists, "Did not retrieve transaction " + TransactionId + " from database.");

                t.Active = false;
                t.LastModifiedDate = DateTime.Now;
                t.LastModifiedUser = HttpContext.Current.User.Identity.Name;

                Transaction_Repo.Update(t);
               // Transaction_Repo.Delete(t);
              
            }
            catch (Exception e)
            {
                throw new CustomException(CustomExceptionTypes.GeneralFailure_Error, "Unable to delete transaction from database.", e.InnerException);
            }
        }
    }
}
