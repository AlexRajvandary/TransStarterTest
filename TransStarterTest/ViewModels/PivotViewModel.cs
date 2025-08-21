using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TransStarterTest.Models;
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

        public ObservableCollection<PivotRowViewModel> Rows { get; set; } = new();

        public ObservableCollection<string> ColumnHeaders { get; set; } = new();

        public void Load(ReportSettings reportSettings)
        {
            var sales = _context.Sales
                            .Where(sale => sale.Date.Year == reportSettings.YearFilter)
                            .SelectMany(sale => sale.Items)
                            .Include(saleItem => saleItem.Car).ThenInclude(car => car.Model)
                            .Include(sellItem => sellItem.Car).ThenInclude(car => car.Brand)
                            .Include(saleItem => saleItem.Sale)
                            .AsEnumerable()
                            .Select(si => new
                                {
                                    RowKey = reportSettings.GroupBy switch
                                    {
                                        GroupingOptions.Model => si.Car.Model.Name,
                                        GroupingOptions.Brand => si.Car.Brand.Name,
                                        GroupingOptions.Customer => si.Sale.Customer.GetFullName(),
                                        _ => string.Empty
                                    },

                                    ColumnKey = reportSettings.ColumnBy == ColumnOptions.Months
                                                    ? si.Sale.Date.Month.ToString()
                                                    : si.Sale.Date.Year.ToString(),

                                    Value = si.Price
                                }).ToList();

            var grouped = sales
                .GroupBy(x => new { x.RowKey, x.ColumnKey })
                .Select(g => new
                {
                    g.Key.RowKey,
                    g.Key.ColumnKey,
                    Value = reportSettings.AggregateBy switch
                    {
                        AggregateOptions.QuantityOfSales => g.Count(),
                        AggregateOptions.SumOfSales => g.Sum(x => x.Value),
                        _ => 0
                    }
                })
                .ToList();

            var pivot = grouped
                .GroupBy(c => c.RowKey)
                .Select(g =>
                {
                    var row = new PivotRowViewModel { RowKey = g.Key };
                    foreach (var cell in g)
                        row.Cells[cell.ColumnKey] = cell.Value;
                    return row;
                })
                .ToList();

            ColumnHeaders = new ObservableCollection<string>(
            grouped.Select(x => x.ColumnKey)
           .Distinct()
           .OrderBy(x => x));

            Rows = new ObservableCollection<PivotRowViewModel>(pivot);
            OnPropertyChanged(nameof(Rows));
            OnPropertyChanged(nameof(ColumnHeaders));
        }
    }
}