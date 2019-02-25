using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; } // FeedbackID (Primary key)
        public int PatientId { get; set; }
        public int EmployeeId { get; set; }
        public string PatientName { get; set; } // PatientID
        public string EmployeeName { get; set; } // EmployeeID
        public int StarRating { get; set; }
        public string Content { get; set; } // Content (length: 500)
        public string ReceivedDateTime { get; set; } // ReceivedDateTime
        public bool? IsDeleted { get; set; } // IsDeleted
        public string Status { get; set; } // Status (length: 100)
    }
}

