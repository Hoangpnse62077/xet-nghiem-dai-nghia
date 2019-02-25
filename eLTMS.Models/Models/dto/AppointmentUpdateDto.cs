using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentUpdateDto
    {
        public string AppCode { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public bool IsEmergency { get; set; }

        public string TestPurpose { get; set; }

        public DateTime EnterTime { get; set; }

        public DateTime ReturnTime { get; set; }

        public string Conlusion { get; set; }

        public bool ResultApproved { get; set; }

        public string Status { get; set; }

    }
}