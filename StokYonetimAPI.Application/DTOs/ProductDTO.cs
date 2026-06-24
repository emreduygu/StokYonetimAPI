namespace StokYonetimAPI.Application.DTOs
{
   
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int UserID { get; set; }
    }
}