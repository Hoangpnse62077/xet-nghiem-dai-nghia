using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class PatientDto
    {
        public int PatientId { get; set; } // PatientID (Primary key)
        public string PatientCode { get; set; } // PatientCode (length: 20)
        public int? AccountId { get; set; } // AccountID
        public string FullName { get; set; } // FullName (length: 100)
        public string Gender { get; set; } // Gender (length: 10)
        public string DateOfBirth { get; set; } // DateOfBirth
        public string PhoneNumber { get; set; } // PhoneNumber (length: 20)
        public string HomeAddress { get; set; } // HomeAddress (length: 200)
        public string CompanyAddress { get; set; } // CompanyAddress (length: 200)
        public bool? IsDeleted { get; set; } // IsDeleted
        public string Avatar { get; set; } // Avatar URL
        public string AppCode { get; set; } // PatientCode (length: 20)

        public string IdentityCardNumber { get; set; } // IdentityCardNumber (length: 20)
        public string Age { get; set; }
    }
}