using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLTMS.Models.Models.dto
{
    public class PriceListDto //nó là cái này, cái này như cái giỏ hàng v đó chứa tổng tiền và list các món m chọn
    {
        public int? TotalPrice { get; set; }

        public List<PriceListItemDto> PriceListItemDto { get ; set; }

    }
}
