using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Dtos.Product
{
    public class AddProductRequestDto
    {
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }
    }

}