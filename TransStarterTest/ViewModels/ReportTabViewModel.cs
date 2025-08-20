using Infrastructure.Data;
using System.Collections.ObjectModel;

namespace TransStarterTest.ViewModels
{
    public class ReportTabViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;

        public ReportTabViewModel(string title, AppDbContext context)
        {
            Title = title;
            _context = context;
            Pivot = new PivotViewModel(context);
            ReportSettings = new ReportSettings();
            ViewMode = ReportViewMode.Details;
        }

        public string Title { get; }

        public ReportViewMode ViewMode { get; set; }

        public ObservableCollection<SaleItemViewModel> ReportData { get; set; } = new();

        public PivotViewModel Pivot { get; }

        public ReportSettings ReportSettings { get; set; }

        public object CurrentRows => ViewMode == ReportViewMode.Details ? (object)ReportData : Pivot.Rows;

        public void Refresh()
        {
            if (ViewMode == ReportViewMode.Details)
                LoadDetails();
            else if (ViewMode == ReportViewMode.Pivot)
                Pivot.Load(ReportSettings.GroupBy, ReportSettings.ColumnBy, ReportSettings.AggregateBy, ReportSettings.YearFilter);

            OnPropertyChanged(nameof(CurrentRows));
        }

        private void LoadDetails()
        {
            var items = _context.Sales
                .Where(sale => sale.Date.Year == ReportSettings.YearFilter)
                .SelectMany(sale => sale.Items)
                .Select(saleItem => new SaleItemViewModel
                {
                    Date = saleItem.Sale.Date,
                    CustomerFullName = saleItem.Sale.Customer.GetFullName(),
                    BrandName = saleItem.Car.Brand.Name,
                    ModelName = saleItem.Car.Model.Name,
                    Price = saleItem.Price
                })
                .ToList();

            ReportData = new ObservableCollection<SaleItemViewModel>(items);
            OnPropertyChanged(nameof(ReportData));
            OnPropertyChanged(nameof(CurrentRows));
        }
    }
}
