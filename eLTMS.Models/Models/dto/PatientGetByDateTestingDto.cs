using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class PatientGetByDateTestingDto
    {
        public int? OrderNumber { get; set; }

        public string Date { get; set; }

        public string PatientName { get; set; }

        public int PatientID { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string DateOfBirth { get; set; }

        public bool? IsPaid { get; set; }

        //public string IdentityCardNumber { get; set; }

    }
}
