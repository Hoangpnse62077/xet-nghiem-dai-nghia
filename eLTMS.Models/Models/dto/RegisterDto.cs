

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models
{
    public class RegisterDto
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AvatarURL { get; set; }
        public string IdentityCardNumber { get; set; }
    }
}