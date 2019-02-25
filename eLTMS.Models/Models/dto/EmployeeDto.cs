using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class EmployeeDto
    {
        public int EmployeeID { get; set; } // PatientID (Primary key)
        public int? AccountId { get; set; } // AccountID
        public string Status { get; set; }//Status of Employee
        public string FullName { get; set; } // FullName (length: 100)
        public string Gender { get; set; } // Gender (length: 10)
        public string DateOfBirth { get; set; } // DateOfBirth
        public string PhoneNumber { get; set; } // PhoneNumber (length: 20)
        public string HomeAddress { get; set; } // HomeAddress (length: 200)
        public string Role { get; set; }// Role (length: 200)
        public string RoleDisplay { get; set; }
        public string DateOfStart { get; set; } // Date Start Of Work
        public bool? IsDeleted { get; set; } // IsDeleted
        public string Avatar { get; set; } // Avatar URL
        public string Email { get; set; }// Email
    }
}