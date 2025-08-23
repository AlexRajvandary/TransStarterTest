using System.ComponentModel;

namespace TransStarterTest.Domain.Enums
{
    public enum AggregateOptions 
    {
        [Description("Сумма продаж")]
        SumOfSales,
      
        [Description("Кол-во продаж")]
        QuantityOfSales
    }
}