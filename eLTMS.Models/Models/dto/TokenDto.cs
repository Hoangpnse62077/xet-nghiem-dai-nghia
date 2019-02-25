using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class TokenDto
    {
        public int TokenId { get; set; }
        public string TokenString { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
