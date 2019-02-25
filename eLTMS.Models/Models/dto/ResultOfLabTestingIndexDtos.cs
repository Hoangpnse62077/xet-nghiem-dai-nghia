using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class ResultOfLabTestingIndexDto
    {
        public int LabTestingIndexId { get; set; }

        public string IndexName { get; set; }

        public string IndexValue { get; set; }

        public string LowNormalHigh { get; set; }

        public string NormalRange { get; set; }

        public string Unit { get; set; }
    }
}