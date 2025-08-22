using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TransStarterTest.Models.DTOs;
using TransStarterTest.Models.Enums;

namespace TransStarterTest.ViewModels
{
    public class ReportTabViewModel : BaseViewModel
    {
        private const int _defaultHighlightThreshold = 25_000_000;

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
            SalesHighlightSettings = new SalesHighlightSettings(isEnabled: true, _defaultHighlightThreshold);
            ViewMode = ReportViewMode.Details;
        }

        public string Title { get; }

        public ReportViewMode ViewMode 
        {
            get => _viewMode;
            set
            {
                if (SetProperty(ref _viewMode, value))
                {
                    _ = RefreshAsync();
                }
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

        public async Task InitializeAsync()
        {
            await LoadData();
            var years = GetAvailableSalesYears();
            var models = GetAvailableModels();
            ReportSettings.Initialize(models, years);
        }

        public async Task RefreshAsync()
        {
            if (ViewMode == ReportViewMode.Details)
                await LoadData();
            else
                await Pivot.Load(ReportSettings);
        }

        private async Task LoadData()
        {
            var items = await _context.Sales
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
                .ToListAsync();

            ReportData.Clear();
            foreach (var item in items)
                ReportData.Add(item);
            OnPropertyChanged(nameof(ReportData));
        }

        private List<int> GetAvailableSalesYears()
        {
            return ReportData.Select(saleItem => saleItem.Date.Year).Distinct().OrderDescending().ToList();
        }

        private List<string> GetAvailableModels()
        {
            return new[] { "(Не выбрано)" }
                .Concat(ReportData.Select(x => x.ModelName).Distinct().Order())
                .ToList();
        }

        private async void ReportSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            await RefreshAsync();
        }
    }
}