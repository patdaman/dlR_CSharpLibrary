using DataLayer;
using SignalEFDataModel;
using ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BusinessLayer
{
    public class LisCasesProcessor : BehaviorBase
    {
        IRepository<SignalEFDataModel.SGNL_LIS.LisCase> repository;
        IRepository<SignalEFDataModel.SGNL_LIS.PatientInsurance> repoPxIns;

        public LisCasesProcessor()
        {
            repository = new ConcreteRepository<SignalEFDataModel.SGNL_LIS.LisCase>(LISDbContext);
            repoPxIns = new ConcreteRepository<SignalEFDataModel.SGNL_LIS.PatientInsurance>(LISDbContext);
        }

        public List<Case> GetCases()
        {
            DateTime searchDate = Convert.ToDateTime("2015-07-31");
            List<SignalEFDataModel.SGNL_LIS.LisCase> caseList = repository.Query<SignalEFDataModel.SGNL_LIS.LisCase>(e => e.Accession.OrderDate > searchDate).ToList<SignalEFDataModel.SGNL_LIS.LisCase>();

            List<Case> outList = new List<Case>();

            foreach (var c in caseList)
            {
                Case cs = new Case();

                cs.CaseId = c.id;
                cs.CaseNumber = c.CaseNumber;

                // cs.Payor1Id = c.PayorId1 ?? 0;
                // cs.Payor2Id = c.PayorId2 ?? 0;
                cs.DateOfService = c.DateofService.Value;
                cs.Client = c.Accession.Client.Name;

                outList.Add(cs);
            }
            return outList;

        }

        public Case UpdateCase(Case theCase)
        {
            if (theCase == null)
            {
                return null;
            }
            SignalEFDataModel.SGNL_LIS.LisCase LisCase = repository.Query<SignalEFDataModel.SGNL_LIS.LisCase>(e => e.CaseNumber == theCase.CaseNumber).FirstOrDefault();

            if (LisCase == null)
            {
                return null;
            }

           LisCase.PayorId1 = theCase.Payor1Id;
           LisCase.PayorId2 = theCase.Payor2Id;
           LisCase.LastModifiedDate = DateTime.Now;
           LisCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;

            repository.Update(LisCase);
            return theCase;
        }

        internal void UpdateGroupNumber(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.GroupNumber = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredRelationship(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.InsuredRelationship = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdatePolicyNumber(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.PolicyNumber = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateHomePlan(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.HomePlan = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);

        }
        internal void UpdatePlanType(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.PlanType = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }

        internal void UpdateNetwork1(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.PayorNetwork1 = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }

        internal void UpdateNetwork2(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.PayorNetwork2 = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }

        internal void UpdateBillType(int caseId, object newvalue)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            SignalEFDataModel.SGNL_LIS.LisCase theCase = repository.Query<SignalEFDataModel.SGNL_LIS.LisCase>(e => e.id == caseId).Single();

            theCase.Accession.BillTypeName = (string)newvalue;

            theCase.Accession.LastModifiedDate = DateTime.Now;
            theCase.Accession.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repository.Update(theCase);
            //  dtorres UNCOMMENT BELOW BEFORE COMMITTING..
            this.InternalDbContext.usp_Update_CaseClassification(theCase.CaseNumber);
        }
        internal void UpdatePayor(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            SignalEFDataModel.SGNL_LIS.LisCase theCase = repository.Query<SignalEFDataModel.SGNL_LIS.LisCase>(e => e.id == caseId).Single();
            switch (payorRank)
            {
                case 1:
                    theCase.PayorId1 = (int)newvalue;
                    break;
                case 2:
                    theCase.PayorId2 = (int)newvalue;
                    break;
                default:
                    throw new CustomException(CustomExceptionTypes.InvalidArguments_Error, "Cannot save a payor with rank " + payorRank);
            }
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repository.Update(theCase);
        }

        internal void UpdateInsuredFirstName(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberFirstName = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }

        internal void UpdateInsuredMiddleName(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberMiddleName = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredLastName(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberLastName = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredDOB(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberDateOfBirth = (DateTime?)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredGender(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberGender = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredAddress1(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberAddress1 = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredAddress2(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberAddress2 = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredCity(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberCity = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredState(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberStateProvince = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredPostalCode(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberPostalCode = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredCountryCode(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberCountry = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }
        internal void UpdateInsuredPhone(int caseId, object newvalue, int payorRank)
        {
            if (caseId == 0) throw new ArgumentException("caseId argument cannot be null");
            int insInfoCount = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Count();
            if (insInfoCount == 0)
            {
                CreatePxInfo(caseId, payorRank);
            }
            SignalEFDataModel.SGNL_LIS.PatientInsurance theCase = repoPxIns.Query<SignalEFDataModel.SGNL_LIS.PatientInsurance>(e => e.CaseId == caseId & e.Rank == payorRank).Single();
            theCase.SubscriberPhoneNumber = (string)newvalue;
            theCase.LastModifiedDate = DateTime.Now;
            theCase.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Update(theCase);
        }

        internal Case GetCase(int caseId)
        {
            throw new NotImplementedException();
        }
        internal void CreatePxInfo(int caseId, int payorRank)
        {
            SignalEFDataModel.SGNL_LIS.LisCase LisCase = repository.Query<SignalEFDataModel.SGNL_LIS.LisCase>(e => e.id == caseId).Single();
            SignalEFDataModel.SGNL_LIS.PatientInsurance pxIns = new SignalEFDataModel.SGNL_LIS.PatientInsurance();
            pxIns.CaseId = caseId;
            pxIns.CaseNumber = LisCase.CaseNumber;
            pxIns.Rank = payorRank;
            pxIns.LastModifiedDate = DateTime.Now;
            pxIns.LastModifiedUser = HttpContext.Current.User.Identity.Name;
            repoPxIns.Insert(pxIns);
        }
    }
}
