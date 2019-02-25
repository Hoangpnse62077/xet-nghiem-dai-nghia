using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class LabTestingDto
    {
        public string LabTestName { get; set; }

        public List<LabTestingIndexDto> LabTestingIndexDtos { get; set; }
        public string PatientName { get; set; } // Status (length: 20)
        public string PDOB { get; set; } // Status (length: 20)
        public string PatientPhone { get; set; } // Status (length: 20)
        public string Price { get; set; } // Status (length: 20)
        public int LabTestingId { get; set; } // LabTestingID (Primary key)
        public int? LabTestId { get; set; } // LabTestID
        public string AppointmentCode { get; set; } // AppointmentCode (length: 20)
        public int SampleId { get; set; } // AppointmentCode (length: 20)
        public int SampleGettingId { get; set; } // AppointmentCode (length: 20)
        public string SampleName { get; set; } // Status (length: 20)
        public string Status { get; set; } // Status (length: 20)
        public bool? IsDeleted { get; set; } // IsDeleted
        public int? MachineSlot { get; set; } // MachineSlot
        public string GetApp { get; set; } // Status (length: 20)
        public string ReturnRe { get; set; } // Status (length: 20)
    }
}
