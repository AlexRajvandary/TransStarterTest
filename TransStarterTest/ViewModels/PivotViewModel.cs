using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

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

        public void Load(string groupBy, string columnBy, string aggregateBy, int year)
        {
            var sales = _context.Sales
                            .Where(sale => sale.Date.Year == year)
                            .SelectMany(sale => sale.Items)
                            .Include(saleItem => saleItem.Car).ThenInclude(car => car.Model)
                            .Include(sellItem => sellItem.Car).ThenInclude(car => car.Brand)
                            .Include(saleItem => saleItem.Sale)
                            .AsEnumerable()
                            .Select(si => new
                                {
                                    RowKey = groupBy == "Модель" 
                                              ? si.Car.Model.Name
                                              : groupBy == "Бренд"
                                                    ? si.Car.Brand.Name
                                                    : si.Sale.Customer.GetFullName(),

                                    ColumnKey = columnBy == "Месяц" ? si.Sale.Date.Month.ToString() :
                                    columnBy == "Год" ? si.Sale.Date.Year.ToString() :
                                    "?",
                                    Value = si.Price
                                })
                            .ToList();

            var grouped = sales
                .GroupBy(x => new { x.RowKey, x.ColumnKey })
                .Select(g => new
                {
                    g.Key.RowKey,
                    g.Key.ColumnKey,
                    Value = aggregateBy switch
                    {
                        "Количество" => g.Count(),
                        "Сумма" => g.Sum(x => x.Value),
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