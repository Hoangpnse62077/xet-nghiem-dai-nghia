using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentAddDto
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public int PatientId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        //public string IdentityCardNumber { get; set; }
        public string Address { get; set; }
        public bool Mau { get; set; }
        public bool NuocTieu { get; set; }
        public bool TeBaoHoc { get; set; }
        public bool Phan { get; set; }
        public bool Dich { get; set; }
        public string Gender { get; set; }
    }
}
