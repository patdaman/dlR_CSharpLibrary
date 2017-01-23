//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using SignalDataLayer;
//using EF = SignalEFDataModel;
//using SignalViewModel;
//using System.Transactions;
//using System.Web;

//namespace SignalBusinessLayer
//{
//    public class ReportsProcessor : BehaviorBase
//    {
//        IRepository<EF.SGNL_ANALYTICS.ReportType> ReportTyperepo;
//        IRepository<EF.SGNL_ANALYTICS.FieldMapping> ReportFieldsRepo;

//        public ReportsProcessor()
//        {
//            ReportTypeRepo = new ConcreteRepository<EF.SGNL_ANALYTICS.ReportType>(AnalyticsDbContext);
//            ReportFieldsRepo = new ConcreteRepository<EF.SGNL_ANALYTICS.FieldMapping>(AnalyticsDbContext);
//        }

//        public List<Physician> GetPhysicians()
//        {
//            List<Physician> outList = null;

//            try
//            {
//                List<EF.SGNL_LIS.Doctor> physicianList = LISrepo.Query<EF.SGNL_LIS.Doctor>().ToList<EF.SGNL_LIS.Doctor>();
//                List<EF.SGNL_LIS.vi_DoctorClientList> physicianClientList = LISrepo.Query<EF.SGNL_LIS.vi_DoctorClientList>().ToList();// .FirstOrDefault()

//                outList = physicianList.Select(x => new Physician()
//                {
//                    DoctorId = x.DoctorId,
//                    Title = x.Title,
//                    FirstName = x.FirstName,
//                    LastName = x.LastName,
//                    MiddleName = x.MiddleName,
//                    Suffix = x.Suffix,
//                    Gender = x.Gender,
//                    EmailAddress = x.EmailAddress,
//                    NPI = x.NPI,
//                    DoctorTypeName = x.DoctorTypeName,
//                    CreatedDate = x.CreatedDate,

//                    ClientList = physicianClientList.Where(d => d.DoctorId == x.DoctorId).Select(d => d.Client).FirstOrDefault()

//                }).ToList();
//                return outList;
//            }
//            catch (Exception ex)
//            {
//                throw new SignalException(SignalExceptionTypes.RetrieveFailure_Error,
//                    "Could not retrieve physicians list", ex);
//            }
//            finally
//            {

//            }
//        }

//        public Physician GetPhysicians(int pid)
//        {
//            EF.SGNL_LIS.Doctor exp = LISrepo.Query<EF.SGNL_LIS.Doctor>(e => e.DoctorId == pid).FirstOrDefault();
//            if (exp != null)
//                return EFPhysician(exp);
//            throw new SignalException(SignalExceptionTypes.RetrieveFailure_NotExists,
//                "Doctor with id:" + pid.ToString() + " does not exist");

//        }
//        private Physician EFPhysician(EF.SGNL_LIS.Doctor x)
//        {
//            return new Physician()
//            {
//                DoctorId = x.DoctorId,
//                Title = x.Title,
//                FirstName = x.FirstName,
//                LastName = x.LastName,
//                MiddleName = x.MiddleName,
//                Suffix = x.Suffix,
//                Gender = x.Gender,
//                EmailAddress = x.EmailAddress,
//                NPI = x.NPI,
//                DoctorTypeName = x.DoctorTypeName,
//                CreatedDate = x.CreatedDate
//            };
//        }
//        public Physician UpdatePhysician(Physician x)
//        {

//            if (x == null)
//            {
//                throw new SignalException(SignalExceptionTypes.UpdateFailure_Error,
//                "Null physician sent for update");
//            }
//            try
//            {
//                using (TransactionScope ts = new TransactionScope())
//                {
//                    EF.SGNL_LIS.Doctor ed = LISrepo.Query<EF.SGNL_LIS.Doctor>(e => e.DoctorId == x.DoctorId).FirstOrDefault();
//                    if (ed == null)
//                    {
//                        throw new SignalException(SignalExceptionTypes.UpdateFailure_NotExists,
//                                    "Did not find Doctor with id:" + x.DoctorId.ToString() + " for update");
//                    }

//                    // only the NPI will be updated
//                    ed.NPI = x.NPI;
//                    ed.LastModifiedDate = DateTime.Now;
//                    ed.LastModifiedUser = HttpContext.Current.User.Identity.Name;

//                    LISrepo.Update(ed);
//                    ts.Complete();
//                }

//                EF.SGNL_LIS.Doctor updatedDoc = LISrepo.Query<EF.SGNL_LIS.Doctor>(e => e.DoctorId == x.DoctorId).FirstOrDefault();
//                Physician retPhys = GetPhysicians(updatedDoc.DoctorId);

//                return retPhys;
//            }
//            catch (TransactionAbortedException ex)
//            {
//                throw new SignalException(SignalExceptionTypes.TransactionFailure_Error,
//                    "Transaction error in updating Physician with id: " + x.DoctorId, ex);
//            }
//            catch (SignalException ex)
//            {
//                throw;
//            }
//            catch (Exception ex)
//            {
//                throw new SignalException(SignalExceptionTypes.UpdateFailure_Error,
//                    "Error in updating Physician with id: " + x.DoctorId, ex);
//            }
//        }

//    }
//}
