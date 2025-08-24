using TransStarterTest.Domain.DTOs;
using TransStarterTest.Domain.Enums;
using TransStarterTest.ViewModels;

namespace TransStarterTest.Domain.Services;

public class PivotCalculator
{
    /// <summary>
    /// Фильтрация, группировка и вычисление аггрегатов
    /// </summary>
    /// <param name="sales">Продажи</param>
    /// <param name="reportSettings"></param>
    /// <returns></returns>
    public Task<List<PivotRowDto>> CalculateAsync(IEnumerable<SaleItemDto> sales, ReportSettings reportSettings)
    {
        return Task.Run(() => Calculate(sales, reportSettings));
    }

    public List<PivotRowDto> Calculate(IEnumerable<SaleItemDto> sales, ReportSettings reportSettings)
    {
        
        var filtered = ApplyFilters(sales, reportSettings);
        var grouped = GroupSales(filtered, reportSettings);

        return BuildPivotRows(grouped);
    }

    /// <summary>
    /// Фильтрует продажи по выбраному году и модели авто
    /// </summary>
    /// <param name="sales"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    private IEnumerable<SaleItemDto> ApplyFilters(IEnumerable<SaleItemDto> sales, ReportSettings settings)
    {
        return sales
            .Where(s => s.Date.Year == settings.YearFilter)
            .Where(s => string.IsNullOrEmpty(settings.ModelFilter)
                     || !settings.IsModelFilterSelected
                     || s.ModelName == settings.ModelFilter);
    }

    /// <summary>
    /// Группирует продажи по выбранному полю (модель авто, бренд, покупатель) и вычисляет выбранный аггрегат (кол-во продаж, сумма продаж)
    /// </summary>
    /// <param name="sales"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    private IEnumerable<(string RowKey, int Month, double Value)> GroupSales(IEnumerable<SaleItemDto> sales, ReportSettings settings)
    {
        return sales
            .GroupBy(s => new
            {
                RowKey = settings.GroupBy switch
                {
                    GroupingOptions.Model => $"{s.BrandName} {s.ModelName}",
                    GroupingOptions.Brand => s.BrandName,
                    GroupingOptions.Customer => s.CustomerFullName,
                    _ => string.Empty
                },
                s.Date.Month
            })
            .Select(g => (
                g.Key.RowKey,
                g.Key.Month,
                Value: settings.AggregateBy switch
                {
                    AggregateOptions.QuantityOfSales => g.Count(),
                    AggregateOptions.SumOfSales => g.Sum(x => x.Price),
                    _ => 0
                }
            ));
    }

    private List<PivotRowDto> BuildPivotRows(IEnumerable<(string RowKey, int Month, double Value)> grouped)
    {
        return grouped
            .GroupBy(x => x.RowKey)
            .Select(group =>
            {
                var row = new PivotRowDto { RowKey = group.Key };
                foreach (var (RowKey, Month, Value) in group)
                {
                    switch (Month)
                    {
                        case 1: row.January = Value; break;
                        case 2: row.Febuary = Value; break;
                        case 3: row.March = Value; break;
                        case 4: row.April = Value; break;
                        case 5: row.May = Value; break;
                        case 6: row.June = Value; break;
                        case 7: row.July = Value; break;
                        case 8: row.August = Value; break;
                        case 9: row.September = Value; break;
                        case 10: row.October = Value; break;
                        case 11: row.November = Value; break;
                        case 12: row.December = Value; break;
                    }
                }
                return row;
            })
            .ToList();
    }
}