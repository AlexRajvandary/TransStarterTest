using Infrastructure.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TransStarterTest.Models.DTOs;
using TransStarterTest.Models.Enums;

namespace TransStarterTest.ViewModels
{
    public class ReportTabViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;
        private ReportViewMode _viewMode;
        private ReportSettings _reportSettings;
        private List<int> _years;
        private List<string> _models;

        public ReportTabViewModel(string title, AppDbContext context)
        {
            Title = title;
            _context = context;
            Pivot = new PivotViewModel(context);
            ReportSettings = new ReportSettings();
            SalesHighlightSettings = new SalesHighlightSettings(isEnabled: true, threshold: 25000000);
            ViewMode = ReportViewMode.Details;

            LoadData();
            GetAvailableSalesYears();
            GetAvailableModels();
            ReportSettings.YearFilter = AvailableYears.First();
            ReportSettings.ModelFilter = AvailableModels.First();
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

        public SalesHighlightSettings SalesHighlightSettings { get; set; }

        public List<int> AvailableYears
        {
            get => _years;
            set
            {
                SetProperty(ref _years, value);
            }
        }

        public List<string> AvailableModels
        {
            get => _models;
            set
            {
                SetProperty(ref _models, value);
            }
        }

        public void Refresh()
        {
            if (ViewMode == ReportViewMode.Details)
                LoadData();
            else
                Pivot.Load(ReportSettings);
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
                    Price = (double)saleItem.Price
                }).Where(saleItem=> string.IsNullOrEmpty(ReportSettings.ModelFilter)
                                 || ReportSettings.ModelFilter == "(Не выбрано)"
                                 || saleItem.ModelName == ReportSettings.ModelFilter).OrderByDescending(saleItem => saleItem.Date)
                .ToList();

            ReportData = new ObservableCollection<SaleItemDto>(items);
            OnPropertyChanged(nameof(ReportData));
        }

        private void GetAvailableSalesYears()
        {
            AvailableYears = ReportData.Select(saleItem => saleItem.Date.Year).Distinct().OrderDescending().ToList();
        }

        private void GetAvailableModels()
        {
            AvailableModels = new List<string>();
            AvailableModels.Add("(Не выбрано)");
            AvailableModels.AddRange(ReportData.Select(saleItem => saleItem.ModelName).Distinct().Order());
        }

        private void ReportSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }
    }
}