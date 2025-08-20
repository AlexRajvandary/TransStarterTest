namespace TransStarterTest.ViewModels
{
    public class SaleItemViewModel
    {
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public string CustomerFullName { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
