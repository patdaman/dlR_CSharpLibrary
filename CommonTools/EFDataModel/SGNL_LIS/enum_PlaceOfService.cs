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
    
    public partial class enum_PlaceOfService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public enum_PlaceOfService()
        {
            this.Payors = new HashSet<Payor>();
        }
    
        public string id { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payor> Payors { get; set; }
    }
}