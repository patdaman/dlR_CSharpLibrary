//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommonTools.EFDataModel.SGNL_LIS
{
    using System;
    using System.Collections.Generic;
    
    public partial class vi_GetAnalyticsCasePatientInfo
    {
        public string CaseNumber { get; set; }
        public Nullable<System.DateTime> DateofService { get; set; }
        public string ExternalPatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MRN { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string FacilityName { get; set; }
        public string OrderingPhysicianLastName { get; set; }
        public string OrderingPhysicianFirstName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string SpecimenTypeName { get; set; }
        public Nullable<System.DateTime> CollectedDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public Nullable<int> AccessionId { get; set; }
        public string PreSortFlowCD38__ { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> CD138PurityPass { get; set; }
        public Nullable<bool> RNAPurityIntegrityPass { get; set; }
        public string QNSComments { get; set; }
        public Nullable<System.DateTimeOffset> OrderDate { get; set; }
        public Nullable<System.DateTimeOffset> CompletedDate { get; set; }
        public string SocialSecurityNo { get; set; }
        public string PatientAddress1 { get; set; }
        public string PatientAddress2 { get; set; }
        public string PatientCity { get; set; }
        public string PatientState { get; set; }
        public string PatientPostalCode { get; set; }
        public string QNS { get; set; }
        public string ComputedICD10Codes { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
    }
}