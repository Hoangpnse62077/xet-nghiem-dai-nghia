using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class SlotDto
    {
        public int SlotId { get; set; }
        public int SampleGroupId { get; set; }
        public int StartTime { get; set; }
        public int FinishTime { get; set; }
        public string FmStartTime { get; set; }
        public string FmFinishTime { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
        public int RemainQuantity { get; set; }
    }
}
