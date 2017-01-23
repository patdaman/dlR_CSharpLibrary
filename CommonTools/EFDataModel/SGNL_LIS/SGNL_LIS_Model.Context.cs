﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SignalEFDataModel.SGNL_LIS
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SGNL_LISEntities : DbContext
    {
        public SGNL_LISEntities()
            : base("name=SGNL_LISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Accession> Accessions { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<Lab> Labs { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<vi_DoctorClientList> vi_DoctorClientList { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Payor> Payors { get; set; }
        public virtual DbSet<CaseErrorReport> CaseErrorReports { get; set; }
        public virtual DbSet<PatientInsurance> PatientInsurances { get; set; }
        public virtual DbSet<LisCase> LisCases { get; set; }
        public virtual DbSet<enum_PlaceOfService> enum_PlaceOfService { get; set; }
        public virtual DbSet<CELHeaderInfo> CELHeaderInfoes { get; set; }
        public virtual DbSet<vi_ClientDoctor> vi_ClientDoctor { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<vi_apiGetBillingCases> vi_apiGetBillingCases { get; set; }
        public virtual DbSet<vi_GetAnalyticsCasePatientInfo> vi_GetAnalyticsCasePatientInfo { get; set; }
    
        public virtual int usp_UpdateLisWarehouse(string caseNumber)
        {
            var caseNumberParameter = caseNumber != null ?
                new ObjectParameter("CaseNumber", caseNumber) :
                new ObjectParameter("CaseNumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_UpdateLisWarehouse", caseNumberParameter);
        }
    
        public virtual int usp_UpdateMessageImportLog(string caseNumber, string executableName, string executableVersion)
        {
            var caseNumberParameter = caseNumber != null ?
                new ObjectParameter("CaseNumber", caseNumber) :
                new ObjectParameter("CaseNumber", typeof(string));
    
            var executableNameParameter = executableName != null ?
                new ObjectParameter("ExecutableName", executableName) :
                new ObjectParameter("ExecutableName", typeof(string));
    
            var executableVersionParameter = executableVersion != null ?
                new ObjectParameter("ExecutableVersion", executableVersion) :
                new ObjectParameter("ExecutableVersion", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_UpdateMessageImportLog", caseNumberParameter, executableNameParameter, executableVersionParameter);
        }
    
        public virtual ObjectResult<usp_GetAnalyticsCasePatientInfo_Result> usp_GetAnalyticsCasePatientInfo(string s_CaseNumbers, Nullable<int> n_OrderingPhysicianID)
        {
            var s_CaseNumbersParameter = s_CaseNumbers != null ?
                new ObjectParameter("s_CaseNumbers", s_CaseNumbers) :
                new ObjectParameter("s_CaseNumbers", typeof(string));
    
            var n_OrderingPhysicianIDParameter = n_OrderingPhysicianID.HasValue ?
                new ObjectParameter("n_OrderingPhysicianID", n_OrderingPhysicianID) :
                new ObjectParameter("n_OrderingPhysicianID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_GetAnalyticsCasePatientInfo_Result>("usp_GetAnalyticsCasePatientInfo", s_CaseNumbersParameter, n_OrderingPhysicianIDParameter);
        }
    }
}
