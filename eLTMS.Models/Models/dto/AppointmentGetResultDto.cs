using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentGetResultDto
    {
        public int AppCode { get; set; }

        public string PatientName { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public string DoctorName { get; set; }

        public string TestPurpose { get; set; }

        public string SampleName { get; set; }

        public List<LabTestingDto> LabTestingDtos { get; set; }

        public DateTime EnterTime { get; set; }

        public DateTime ReturnTime { get; set; }

        public string Conclusion { get; set; }

        public bool ResultApproved { get; set; }
        public DateTime Date { get; set; } // CreateDate
    }
}