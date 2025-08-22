using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TransStarterTest.Models.DTOs;
using TransStarterTest.Models.Enums;

namespace TransStarterTest.ViewModels
{
    public class PivotViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;

        public PivotViewModel(AppDbContext context)
        {
            _context = context;
        }

        public List<PivotRowViewDto> Rows { get; set; } = new();

        public async Task Load(ReportSettings reportSettings)
        {
            var groupedQuery = await _context.Sales
                .Where(sale => sale.Date.Year == reportSettings.YearFilter)
                .SelectMany(sale => sale.Items)
                .Include(si => si.Car).ThenInclude(c => c.Model)
                .Include(si => si.Car).ThenInclude(c => c.Brand)
                .Include(si => si.Sale)
                .Where(saleItem => string.IsNullOrEmpty(reportSettings.ModelFilter)
                                 || reportSettings.ModelFilter == "(Не выбрано)"
                                 || saleItem.Car.Model.Name == reportSettings.ModelFilter)
                .GroupBy(si => new
                {
                    RowKey = reportSettings.GroupBy == GroupingOptions.Model ? si.Car.Brand.Name + " " + si.Car.Model.Name :
                             reportSettings.GroupBy == GroupingOptions.Brand ? si.Car.Brand.Name :
                             reportSettings.GroupBy == GroupingOptions.Customer ? si.Sale.Customer.FirstName + " " + si.Sale.Customer.LastName :
                             string.Empty,
                    Month = si.Sale.Date.Month
                })
                .Select(g => new
                {
                    g.Key.RowKey,
                    g.Key.Month,
                    Value = reportSettings.AggregateBy == AggregateOptions.QuantityOfSales ? g.Count() :
                            reportSettings.AggregateBy == AggregateOptions.SumOfSales ? g.Sum(x => x.Price) :
                            0
                })
                .ToListAsync();

            var pivot = groupedQuery
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

            Rows = pivot;

            OnPropertyChanged(nameof(Rows));
        }
    }
}