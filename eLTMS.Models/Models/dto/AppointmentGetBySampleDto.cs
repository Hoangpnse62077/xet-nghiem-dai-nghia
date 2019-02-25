using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class AppointmentGetBySampleDto
    {
        public int? OrderNumber { get; set; }

        public string AppointmentCode { get; set; }

        public string SampleName { get; set; }

        public string PatientName { get; set; }

        public string DateOfBirth { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Table { get; set; }

        public string Date { get; set; }

        public string StartTime { get; set; }

        public int SampleGettingId { get; set; }

        public int? SampleId { get; set; }

        public bool? IsPaid { get; set; }

        //public List<string> LabTestName { get; set; }

        //public List<string> Price { get; set; }

    }
}
