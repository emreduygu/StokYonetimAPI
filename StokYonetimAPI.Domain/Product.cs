using System.ComponentModel.DataAnnotations.Schema;

namespace StokYonetimAPI.Domain
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int UserID {  get; set; }
        public User User { get; set; } = null!;
    }

   
}
