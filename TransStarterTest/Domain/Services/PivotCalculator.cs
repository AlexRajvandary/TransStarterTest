using TransStarterTest.Domain.DTOs;
using TransStarterTest.Domain.Enums;
using TransStarterTest.Models.DTOs;
using TransStarterTest.ViewModels;

namespace TransStarterTest.Domain.Services
{
    public class PivotCalculator
    {
        public List<PivotRowViewDto> Calculate(IEnumerable<SaleItemDto> sales, ReportSettings reportSettings)
        {
            var grouped = sales
                .Where(s => string.IsNullOrEmpty(reportSettings.ModelFilter)
                         || !reportSettings.IsModelFilterSelected
                         || s.ModelName == reportSettings.ModelFilter)
                .GroupBy(s => new
                {
                    RowKey = reportSettings.GroupBy switch
                    {
                        GroupingOptions.Model => $"{s.BrandName} {s.ModelName}",
                        GroupingOptions.Brand => s.BrandName,
                        GroupingOptions.Customer => s.CustomerFullName,
                        _ => string.Empty
                    },
                    s.Date.Month
                })
                .Select(g => new
                {
                    g.Key.RowKey,
                    g.Key.Month,
                    Value = reportSettings.AggregateBy switch
                    {
                        AggregateOptions.QuantityOfSales => g.Count(),
                        AggregateOptions.SumOfSales => g.Sum(x => x.Price),
                        _ => 0
                    }
                })
                .GroupBy(x => x.RowKey)
                .Select(group =>
                {
                    var row = new PivotRowViewDto { RowKey = group.Key };
                    foreach (var cell in group)
                    {
                        switch (cell.Month)
                        {
                            case 1: row.January = (double)cell.Value; break;
                            case 2: row.Febuary = (double)cell.Value; break;
                            case 3: row.March = (double)cell.Value; break;
                            case 4: row.April = (double)cell.Value; break;
                            case 5: row.May = (double)cell.Value; break;
                            case 6: row.June = (double)cell.Value; break;
                            case 7: row.July = (double)cell.Value; break;
                            case 8: row.August = (double)cell.Value; break;
                            case 9: row.September = (double)cell.Value; break;
                            case 10: row.October = (double)cell.Value; break;
                            case 11: row.November = (double)cell.Value; break;
                            case 12: row.December = (double)cell.Value; break;
                        }
                    }
                    return row;
                })
                .ToList();

            return grouped;
        }
    }
}