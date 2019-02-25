using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    //sd cho modal thông tin cac cuoc hen cua benh nhan
    //sd trong RecepService
    public class SampleGettingForReceptionistDto
    {
        public int? OrderNumber { get; set; }

        public int SampleGettingId { get; set; }

        public string SampleName { get; set; }

        public string LabTestName { get; set; }

        public string Status { get; set; }

        public string EnterDate { get; set; }

        public string EnterTime { get; set; }

        public string StartTime { get; set; }

    }
}