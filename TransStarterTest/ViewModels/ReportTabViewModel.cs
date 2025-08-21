using Infrastructure.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TransStarterTest.Models;
using TransStarterTest.Models.Enums;

namespace TransStarterTest.ViewModels
{
    public class ReportTabViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;
        private ReportViewMode _viewMode;
        private ReportSettings _reportSettings;
        private List<int> _years;

        public ReportTabViewModel(string title, AppDbContext context)
        {
            Title = title;
            _context = context;
            Pivot = new PivotViewModel(context);
            ReportSettings = new ReportSettings();
            ViewMode = ReportViewMode.Details;

            LoadData();
            GetSalesYears();
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

        public ObservableCollection<SaleItemDto> ReportData { get; set; } = new();

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

        public List<int> AvailableYears
        {
            get => _years;
            set
            {
                SetProperty(ref _years, value);
            }
        }

        public void Refresh()
        {
            if (ViewMode == ReportViewMode.Details)
                LoadData();
            else if (ViewMode == ReportViewMode.Pivot)
                Pivot.Load(ReportSettings);

            OnPropertyChanged(nameof(CurrentRows));
        }

        private void LoadData()
        {
            var items = _context.Sales
                .SelectMany(sale => sale.Items)
                .Select(saleItem => new SaleItemDto
                {
                    Date = saleItem.Sale.Date,
                    CustomerFullName = saleItem.Sale.Customer.GetFullName(),
                    BrandName = saleItem.Car.Brand.Name,
                    ModelName = saleItem.Car.Model.Name,
                    Price = saleItem.Price
                })
                .ToList();

            ReportData = new ObservableCollection<SaleItemDto>(items);
            OnPropertyChanged(nameof(ReportData));
            OnPropertyChanged(nameof(CurrentRows));
        }

        private void GetSalesYears()
        {
            AvailableYears = ReportData.Select(saleItem => saleItem.Date.Year).Distinct().ToList();
        }

        private void ReportSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }
    }
}
