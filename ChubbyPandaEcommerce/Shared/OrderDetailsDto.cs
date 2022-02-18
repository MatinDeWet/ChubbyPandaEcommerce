using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChubbyPandaEcommerce.Shared
{
    public class OrderDetailsDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotaslPrice { get; set; }
        public List<OrderDetailsProductDto> Products { get; set; }
    }
}
