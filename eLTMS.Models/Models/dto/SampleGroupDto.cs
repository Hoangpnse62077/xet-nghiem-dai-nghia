using eLTMS.Models.Models.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class SampleGroupDto
    {
        public int SampleGroupId { get; set; } // SampleGroupID (Primary key)
        public string GroupName { get; set; } // GroupName (length: 100)
        public int? GettingDuration { get; set; } // GettingDuration
        public float? OpenTime { get; set; } // OpenTime
        public float? CloseTime { get; set; } // CloseTime
        public bool? IsDeleted { get; set; } // IsDeleted
    }
}