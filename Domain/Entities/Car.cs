namespace Domain.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int ModelId { get; set; }
        public Model Model { get; set; } = null!;
        public int ConfigurationId { get; set; }
        public Configuration Configuration { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
