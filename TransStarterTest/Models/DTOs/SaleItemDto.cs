namespace TransStarterTest.Models.DTOs
{
    public class SaleItemDto
    {
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public string CustomerFullName { get; set; } = null!;
        public DateTime Date { get; set; }
        public double Price { get; set; }
    }
}
