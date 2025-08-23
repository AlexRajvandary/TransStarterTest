using System.ComponentModel;

namespace TransStarterTest.Domain.Enums
{
    public enum ReportViewMode
    {
        [Description("Все продажи")]
        Details,
        
        [Description("Сводка по месяцам")]
        Pivot
    }
}