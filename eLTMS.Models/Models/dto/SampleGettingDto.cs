using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class SampleGettingDto
    {
        public int SampleId { get; set; }

        public string SampleName { get; set; }

        public string GettingDate { get; set; }

        public string StartTime { get; set; } // may be not used
        
        public string FinishTime { get; set; } // may be not used

        public string FmStartTime { get; set; }

        public string FmFinishTime { get; set; }

        public int SlotId { get; set; }

        public int TableId { get; set; }

        public List<int> LabTestIds { get; set; }

        public List<LabTestDto> LabTests { get; set; }

    }
}