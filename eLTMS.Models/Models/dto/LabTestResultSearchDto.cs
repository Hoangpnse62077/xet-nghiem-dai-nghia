using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class LabTestResultSearchDto
    {
        public string PatientId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
    public class LabTestResultDto
    {
        public int LabTestResultId { get; set; }
        public string PatientCode { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Comment { get; set; }
        public string Age { get; set; }
        public string CreatedDate => CreateDateTmp.Value.ToString("dd-MM-yyyy HH:mm");
        public DateTime? CreateDateTmp { get; set; }
        public int TotalCount { get; set; }
    }
}
