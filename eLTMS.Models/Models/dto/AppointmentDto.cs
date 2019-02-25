using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public string AppointmentCode { get; set; }
        public string DoctorName { get; set; }

        public int? PatientId { get; set; }

        public string PatientName { get; set; }
        public bool? IsOnline { get; set; }

        public string TestPurpose { get; set; }
        public string DoctorComment { get; set; } // DoctorComment
        public string DateResult { get; set; }
        public List<SampleGettingDto> SampleGettingDtos { get; set; }
        public PatientDto PatientDto { get; set; }
        public string Conclusion { get; set; } // Conclusion (length: 500)
        public string Status { get; set; } // Status (length: 20)

    }
}