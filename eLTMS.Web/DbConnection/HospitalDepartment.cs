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
    
    public partial class HospitalDepartment
    {
        public int HospitalDepartmentID { get; set; }
        public Nullable<int> HospitalID { get; set; }
        public Nullable<int> FacultyID { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual Hospital Hospital { get; set; }
    }
}
