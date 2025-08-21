using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using TransStarterTest.Models.Enums;

namespace TransStarterTest.ViewModels
{
    public class DynamicPivotViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;

        public DynamicPivotViewModel(AppDbContext context)
        {
            _context = context;
        }

        public ObservableCollection<DataGridColumn> ColumnDefinitions { get; set; } = new();

        public ObservableCollection<DynamicPivotRow> PivotRows { get; set; }

        public ObservableCollection<Dictionary<string, object>> Test { get; set; }

        public List<string> ColumnKeys { get; set; } = new();

        public void Load(ReportSettings settings)
        {
            var grouped = _context.Sales
                .Where(s => s.Date.Year == settings.YearFilter)
                .SelectMany(s => s.Items)
                .Include(si => si.Car).ThenInclude(c => c.Model)
                .Include(si => si.Car).ThenInclude(c => c.Brand)
                .Include(si => si.Sale)
                .AsEnumerable()
                .Select(si => new
                {
                    RowKey = settings.GroupBy switch
                    {
                        GroupingOptions.Model => si.Car.Model.Name,
                        GroupingOptions.Brand => si.Car.Brand.Name,
                        GroupingOptions.Customer => si.Sale.Customer.GetFullName(),
                        _ => string.Empty
                    },
                    ColumnKey = settings.ColumnBy == ColumnOptions.Months
                        ? si.Sale.Date.Month.ToString()
                        : si.Sale.Date.Year.ToString(),
                    Value = settings.AggregateBy == AggregateOptions.QuantityOfSales ? 1 :
                            settings.AggregateBy == AggregateOptions.SumOfSales ? si.Price : 0
                })
                .GroupBy(x => new { x.RowKey, x.ColumnKey })
                .Select(g => new
                {
                    g.Key.RowKey,
                    g.Key.ColumnKey,
                    Value = g.Sum(x => x.Value)
                })
                .ToList();

            // Pivot строки
            var rows = grouped
                .GroupBy(c => c.RowKey)
                .Select(g =>
                {
                    var row = new DynamicPivotRow { RowKey = g.Key };
                    foreach (var cell in g)
                        row.Values[cell.ColumnKey] = (double)cell.Value;
                    return row;
                })
                .ToList();

            var months = new[]
 {
    "Январь", "Февраль", "Март", "Апрель",
    "Май", "Июнь", "Июль", "Август",
    "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
};

            var testrows = grouped
                .GroupBy(c => c.RowKey)
                .Select(g =>
                {
                    var row = new Dictionary<string, object>();
                    row["Группа"] = g.Key;

                    for (var month=1; month <= months.Length; month++)
                    {
                        var cell = g.FirstOrDefault(c => c.ColumnKey == month.ToString());
                        row[months[month-1]] = cell != null ? (double)cell.Value : 0.0;
                    }

                    return row;
                })
                .ToList();

            PivotRows = new ObservableCollection<DynamicPivotRow>(rows);
            OnPropertyChanged(nameof(PivotRows));

            Test = new ObservableCollection<Dictionary<string, object>>(testrows);
           

            BuildColumns();
            OnPropertyChanged(nameof(Test));
        }

        private void BuildColumns()
        {
            ColumnDefinitions.Clear();

            // Первая колонка — "Группа"
            ColumnDefinitions.Add(new DataGridTextColumn
            {
                Header = "Группа",
                Binding = new Binding("Группа")
            });

            // Дальше месяцы
            var months = new[]
            {
        "Январь", "Февраль", "Март", "Апрель",
        "Май", "Июнь", "Июль", "Август",
        "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
    };

            foreach (var month in months)
            {
                ColumnDefinitions.Add(new DataGridTextColumn
                {
                    Header = month,
                    Binding = new Binding($"{month}") // привязка к словарю
                });
            }
        }
    }
}