namespace Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
