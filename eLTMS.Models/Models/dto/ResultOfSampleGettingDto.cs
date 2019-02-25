using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class ResultOfSampleGettingDto
    {
        public int SampleGettingId { get; set; }

        public string SampleName { get; set; }

        public List<ResultOfLabTestingDto> LabTestings { get; set; }
    }
}