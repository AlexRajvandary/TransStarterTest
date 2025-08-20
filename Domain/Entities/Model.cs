namespace Domain.Entities
{
    public class Model
    {
        public int Id { get; set; }
        public int BrandId { get;set; }
        public string Name { get; set; } = null!;

        public Brand Brand { get; set; } = null!;
    }
}
