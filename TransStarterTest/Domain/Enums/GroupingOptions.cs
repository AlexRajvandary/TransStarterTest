using System.ComponentModel;

namespace TransStarterTest.Domain.Enums
{
    public enum GroupingOptions
    {
        [Description("Модель")]
        Model,

        [Description("Бренд")]
        Brand,

        [Description("Клиент")]
        Customer
    }
}