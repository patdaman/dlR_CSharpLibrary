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
    
    public partial class XIFIN_ICD9
    {
        public int TestOrderID { get; set; }
        public string CaseNo { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
    
        public virtual XIFIN_TestOrder XIFIN_TestOrder { get; set; }
    }
}