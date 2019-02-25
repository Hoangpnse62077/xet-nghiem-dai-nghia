using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class SlotOptionDto
    {
        public int SlotId { get; set; }

        public int StartTime { get; set; }

        public int FinishTime { get; set; }

        public bool IsAvailable { get; set; }
    }
}