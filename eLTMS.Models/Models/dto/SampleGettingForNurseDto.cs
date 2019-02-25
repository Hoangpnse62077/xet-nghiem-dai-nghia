using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class SampleGettingForNurseDto
    {
        public int SampleGettingId { get; set; }

        public string SampleGettingCode { get; set; }

        public string SampleName { get; set; }

        public string PatientName { get; set; }
    }
}