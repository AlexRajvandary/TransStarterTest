using Infrastructure.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TransStarterTest.ViewModels
{
    public class ReportTabViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;
        private ReportViewMode _viewMode;
        private ReportSettings _reportSettings;

        public ReportTabViewModel(string title, AppDbContext context)
        {
            Title = title;
            _context = context;
            Pivot = new PivotViewModel(context);
            ReportSettings = new ReportSettings();
            ViewMode = ReportViewMode.Details;
            LoadDetails();
        }

        public string Title { get; }

        public ReportViewMode ViewMode 
        {
            get => _viewMode;
            set
            {
                SetProperty(ref _viewMode, value);
                Refresh();
            }
        }

        public ObservableCollection<SaleItemViewModel> ReportData { get; set; } = new();

        public PivotViewModel Pivot { get; }

        public ReportSettings ReportSettings
        {
            get => _reportSettings;
            set
            {
                if (_reportSettings == value) return;
                if (_reportSettings != null)
                    _reportSettings.PropertyChanged -= ReportSettings_PropertyChanged;

                _reportSettings = value;
                OnPropertyChanged(nameof(ReportSettings));

                if (_reportSettings != null)
                    _reportSettings.PropertyChanged += ReportSettings_PropertyChanged;
            }
        }

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

        private void ReportSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }
    }
}
