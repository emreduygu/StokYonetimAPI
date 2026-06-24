using System;
using System.Collections.Generic;
using System.Text;

namespace StokYonetimAPI.Application.DTOs
{
    public class UpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
