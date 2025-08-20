namespace Domain.Entities
{
    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; } = null!;
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;
        public decimal Price { get; set; }
    }
}