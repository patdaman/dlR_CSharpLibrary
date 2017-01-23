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
    
    public partial class Payor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Payor()
        {
            this.LisCases = new HashSet<LisCase>();
            this.LisCases1 = new HashSet<LisCase>();
        }
    
        public int id { get; set; }
        public string PayorCode { get; set; }
        public string PayorName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddr1 { get; set; }
        public string ContactAddr2 { get; set; }
        public string ContactCity { get; set; }
        public string ContactState { get; set; }
        public string ContactZipCode { get; set; }
        public Nullable<System.DateTimeOffset> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public string LastModifiedUser { get; set; }
        public Nullable<byte> FlagForReview { get; set; }
        public string PlaceOfService { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LisCase> LisCases { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LisCase> LisCases1 { get; set; }
        public virtual enum_PlaceOfService enum_PlaceOfService { get; set; }
    }
}
