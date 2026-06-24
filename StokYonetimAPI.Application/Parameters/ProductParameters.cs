namespace StokYonetimAPI.Application.Parameters
{
    public class ProductParameters
    {
        public int? id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }   
        public decimal? minPrice { get; set; }  
        public decimal? maxPrice { get; set; }   
        public int? StockQuantity { get; set; }
        public int? minStockQuantity { get; set; }
        public int? maxStockQuantity { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}