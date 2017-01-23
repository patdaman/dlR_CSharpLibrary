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
    
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.Accessions = new HashSet<Accession>();
        }
    
        public int id { get; set; }
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public string AccountType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public Nullable<System.DateTime> CurrentEffectiveDate { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public System.DateTimeOffset CreatedDate { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public string LastModifiedUser { get; set; }
        public byte FlagForReview { get; set; }
        public Nullable<int> ClientId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accession> Accessions { get; set; }
    }
}
