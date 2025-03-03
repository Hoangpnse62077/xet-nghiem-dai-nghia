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
    
    public partial class Supply
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Supply()
        {
            this.ExportPaperDetails = new HashSet<ExportPaperDetail>();
            this.ImportPaperDetails = new HashSet<ImportPaperDetail>();
        }
    
        public int SuppliesId { get; set; }
        public string SuppliesCode { get; set; }
        public string SuppliesName { get; set; }
        public Nullable<int> SuppliesTypeId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExportPaperDetail> ExportPaperDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ImportPaperDetail> ImportPaperDetails { get; set; }
        public virtual SupplyType SupplyType { get; set; }
    }
}
