using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class HospitalSuggestionDto
    {
         public int HospitalSuggestionId { get; set; } // HospitalSuggestionID (Primary key)
         public string DiseaseName { get; set; } // DiseaseName
         public string HospitalList { get; set; } // HospitalList
         public bool? IsDeleted { get; set; } // IsDeleted
         public string HospitalAdd { get; set; } // HospitalAdd
          public string HospitalPhone { get; set; } // HospitalPhone
    }
}