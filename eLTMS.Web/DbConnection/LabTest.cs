//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eLTMS.Web.DbConnection
{
    using System;
    using System.Collections.Generic;
    
    public partial class LabTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LabTest()
        {
            this.LabTestings = new HashSet<LabTesting>();
        }
    
        public int LabTestID { get; set; }
        public string LabTestCode { get; set; }
        public string LabTestName { get; set; }
        public string Description { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> SampleID { get; set; }
    
        public virtual Sample Sample { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LabTesting> LabTestings { get; set; }
    }
}
