using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class ResultOfLabTestingDto
    {
        public int LabTestingId { get; set; }

        public string LabTestName { get; set; }

        public List<ResultOfLabTestingIndexDto> LabTestingIndexes { get; set; }
    }
}