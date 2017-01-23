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
    
    public partial class Doctor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctor()
        {
            this.Accessions = new HashSet<Accession>();
            this.Accessions1 = new HashSet<Accession>();
        }
    
        public int DoctorId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string NPI { get; set; }
        public string DoctorTypeName { get; set; }
        public System.DateTimeOffset CreatedDate { get; set; }
        public string LastModifiedUser { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public byte FlagForReview { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accession> Accessions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accession> Accessions1 { get; set; }
        public virtual vi_DoctorClientList vi_DoctorClientList { get; set; }
    }
}