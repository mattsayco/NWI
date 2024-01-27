using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NWI.Models
{
    public class Product
    {
        public int Id { get; set; } = 0;
        public required string ProductName { get; set; } = "";
        public required string ProductCode { get; set; } = "";
        [Column(TypeName = "decimal(18,2)")]
        public required decimal UnitPrice { get; set; } = 0;
        public required string CreatedBy { get; set; } = "Admin";
        public required string UpdatedBy { get; set; } = "Admin";
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}