using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Dtos.Product
{
    public class UpdateProductRequestDto
    {
        public int Id { get; set; } = 0;
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }
    }
}