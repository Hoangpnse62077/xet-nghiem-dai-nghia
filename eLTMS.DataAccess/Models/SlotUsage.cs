using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.DataAccess.Models
{
    public class SlotUsage
    {
        public int SlotId { get; set; }
        public int StartTime { get; set; }
        public int FinishTime { get; set; }
        public int? NBooked { get; set; }
    }
}
