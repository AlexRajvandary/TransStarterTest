using TransStarterTest.Domain.DTOs;
using TransStarterTest.Domain.Enums;
using TransStarterTest.Models.DTOs;
using TransStarterTest.ViewModels;

public class PivotCalculator
{
    public Task<List<PivotRowViewDto>> CalculateAsync(IEnumerable<SaleItemDto> sales, ReportSettings reportSettings)
    {
        return Task.Run(() => Calculate(sales, reportSettings));
    }

    public List<PivotRowViewDto> Calculate(IEnumerable<SaleItemDto> sales, ReportSettings reportSettings)
    {
        
        var filtered = ApplyFilters(sales, reportSettings);
        var grouped = GroupSales(filtered, reportSettings);

        return BuildPivotRows(grouped);
    }

    private IEnumerable<SaleItemDto> ApplyFilters(IEnumerable<SaleItemDto> sales, ReportSettings settings)
    {
        return sales
            .Where(s => s.Date.Year == settings.YearFilter)
            .Where(s => string.IsNullOrEmpty(settings.ModelFilter)
                     || !settings.IsModelFilterSelected
                     || s.ModelName == settings.ModelFilter);
    }

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

    private List<PivotRowViewDto> BuildPivotRows(IEnumerable<(string RowKey, int Month, double Value)> grouped)
    {
        return grouped
            .GroupBy(x => x.RowKey)
            .Select(group =>
            {
                var row = new PivotRowViewDto { RowKey = group.Key };
                foreach (var cell in group)
                {
                    switch (cell.Month)
                    {
                        case 1: row.January = cell.Value; break;
                        case 2: row.Febuary = cell.Value; break;
                        case 3: row.March = cell.Value; break;
                        case 4: row.April = cell.Value; break;
                        case 5: row.May = cell.Value; break;
                        case 6: row.June = cell.Value; break;
                        case 7: row.July = cell.Value; break;
                        case 8: row.August = cell.Value; break;
                        case 9: row.September = cell.Value; break;
                        case 10: row.October = cell.Value; break;
                        case 11: row.November = cell.Value; break;
                        case 12: row.December = cell.Value; break;
                    }
                }
                return row;
            })
            .ToList();
    }
}