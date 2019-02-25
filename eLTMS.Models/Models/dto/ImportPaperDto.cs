using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLTMS.Models.Models.dto
{
    public class ImportPaperDto
    {

        public int ImportPaperId { get; set; } // ImportPaperId (Primary key)
        public string ImportPaperCode { get; set; } // ImportPaperCode (length: 20)
        public String CreateDate { get; set; } // CreateDate
        public int? AccountId { get; set; } // AccountId
        public string Note { get; set; } // Note
        public bool? IsDeleted { get; set; } // IsDeleted
        public List<ImportPaperDetailDto> ImportPaperDetailDtos { get; set; }

    }
}