using System.ComponentModel;

namespace TransStarterTest.Domain.DTOs
{
    public class SaleItemDto
    {
        [Description("Марка")]
        public string BrandName { get; set; } = null!;

        [Description("Модель")]
        public string ModelName { get; set; } = null!;

        [Description("ФИО покупателя")]
        public string CustomerFullName { get; set; } = null!;

        [Description("Дата покупки")]
        public DateTime Date { get; set; }

        [Description("Цена")]
        public double Price { get; set; }
    }
}