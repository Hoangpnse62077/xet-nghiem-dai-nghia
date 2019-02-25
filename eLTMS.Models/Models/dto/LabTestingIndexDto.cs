using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class LabTestingIndexDto
    {
        public int LabtTestingIndexId { get; set; } // LabtTestingIndexID (Primary key)
        public int? LabTestingId { get; set; } // LabTestingID
        public string IndexName { get; set; } // IndexName (length: 20)
        public string IndexValue { get; set; } // IndexValue (length: 50)
        public string LowNormalHigh { get; set; } // LowNormalHigh (length: 10)
        public string NormalRange { get; set; } // NormalRange (length: 50)
        public string Unit { get; set; } // Unit (length: 20)
        public bool? IsDeleted { get; set; } // IsDeleted

    }
}
