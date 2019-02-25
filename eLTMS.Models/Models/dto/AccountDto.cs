using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class AccountDto
    {
        public int AccountId { get; set; }

        public int PatientId { get; set; } // PatientID

        public string FullName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string AvatarURL { get; set; }

        public string Email { get; set; }
        
        public string IdentityCardNumber { get; set; }

    }
}