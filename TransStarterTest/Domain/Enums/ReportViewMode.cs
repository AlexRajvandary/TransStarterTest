using System.ComponentModel;

namespace TransStarterTest.Domain.Enums
{
    /// <summary>
    /// Режим отчёта <list type="bullet">
    /// <item><see cref="ReportViewMode.Details"/> — Все продажи</item>
    /// <item><see cref="ReportViewMode.Pivot"/> — Сводка по месяцам</item>
    /// </list>
    /// </summary>
    public enum ReportViewMode
    {
        [Description("Все продажи")]
        Details,
        
        [Description("Сводка по месяцам")]
        Pivot
    }
}