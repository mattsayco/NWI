using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Dtos.Product
{
    public class GetProductResponseDto
    {
        public int Id { get; set; } = 0;
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public decimal? UnitPrice { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}