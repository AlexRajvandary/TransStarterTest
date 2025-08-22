using System.ComponentModel;

namespace TransStarterTest.Models.Enums
{
    public enum ReportViewMode
    {
        [Description("Все продажи")]
        Details,
        
        [Description("Сводка по месяцам")]
        Pivot,

        DynamicPivot
    }
}