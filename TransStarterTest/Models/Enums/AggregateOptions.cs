using System.ComponentModel;

namespace TransStarterTest.Models.Enums
{
    public enum AggregateOptions 
    {
        [Description("Сумма продаж")]
        SumOfSales,
      
        [Description("Кол-во продаж")]
        QuantityOfSales
    }
}