//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommonTools.EFDataModel.XIFIN_LIS
{
    using System;
    using System.Collections.Generic;
    
    public partial class XIFIN_Specimen
    {
        public int SpecimenID { get; set; }
        public string CaseNo { get; set; }
        public string ExternalSpecimenID { get; set; }
        public string BodySiteName { get; set; }
        public string SpecimenName { get; set; }
        public string Volume { get; set; }
        public Nullable<System.DateTime> CollectionDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public string SpecimenTransportName { get; set; }
        public string SpecimenTypeName { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    }
}
