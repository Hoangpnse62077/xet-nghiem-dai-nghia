using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentGetByPhoneDto
    {
        public string PatientName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string AppointmentCode { get; set; }

        public string SamplesName { get; set; }

        public DateTime StartTime { get; set; }
    }
}
