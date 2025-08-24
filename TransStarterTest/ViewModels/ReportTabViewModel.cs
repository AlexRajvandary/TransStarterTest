using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TransStarterTest.Commands;
using TransStarterTest.Domain.Contracts;
using TransStarterTest.Domain.DTOs;
using TransStarterTest.Domain.Enums;

namespace TransStarterTest.ViewModels
{
    public class ReportTabViewModel : BaseViewModel
    {
        private const int _defaultHighlightThreshold = 25_000_000;

        private readonly AppDbContext _context;
        private ReportViewMode _viewMode;
        private ReportSettings _reportSettings;
        private IMessageBoxService _notificationDialogService;
        private List<SaleItemDto> fullData;

        public ReportTabViewModel(string title,
                                  AppDbContext context,
                                  IMessageBoxService notificationDialogService,
                                  ReportViewMode viewMode = ReportViewMode.Details)
        {
            Title = title;
            _context = context;
            _notificationDialogService = notificationDialogService;
            Pivot = new PivotViewModel();
            ReportSettings = new ReportSettings();
            SalesHighlightSettings = new SalesHighlightSettings(isEnabled: true, _defaultHighlightThreshold);
            ViewMode = viewMode;
            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
        }

        public string Title { get; }

        public ReportViewMode ViewMode
        {
            get => _viewMode;
            set
            {
                SetProperty(ref _viewMode, value);
            }
        }

        public List<SaleItemDto> FullData
        {
            get => fullData;
            set
            {
                if (fullData != value) 
                {
                    fullData = value;
                    UpdateReportData();
                }
            }
        }

        public ICommand RefreshCommand { get; }

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

        public async Task RefreshAsync()
        {
            try
            {
                await LoadSaleItemDtosAsync();
                var years = GetAvailableSalesYears();
                var models = GetAvailableModels();
                ReportSettings.Refresh(models, years);
            }
            catch (Exception ex)
            {
                _notificationDialogService.ShowError($"В процессе обновления отчёта произошла ошибка: {ex.Message}");
            }
        }

        private IEnumerable<SaleItemDto> ApplyFilters(IEnumerable<SaleItemDto> saleItems)
        {
            if (!string.IsNullOrEmpty(ReportSettings.ModelFilter) && ReportSettings.IsModelFilterSelected)
            {
                saleItems = saleItems.Where(s => s.ModelName == ReportSettings.ModelFilter);
            }

            return saleItems;
        }

        private async Task LoadSaleItemDtosAsync()
        {
            try
            {
                var salesItemDtos = await _context.Sales
                    .SelectMany(sale => sale.Items)
                    .Select(saleItem => new SaleItemDto
                    {
                        Date = saleItem.Sale.Date,
                        CustomerFullName = saleItem.Sale.Customer.GetFullName(),
                        BrandName = saleItem.Car.Brand.Name,
                        ModelName = saleItem.Car.Model.Name,
                        Price = (double)saleItem.Price,
                        Color = saleItem.Car.Color,
                        Configuration = saleItem.Car.Configuration.Name
                    }).ToListAsync();

                FullData = salesItemDtos;
            }
            catch (Exception ex)
            {
                _notificationDialogService.ShowError($"При загрузке отчёта всех продаж произошла ошибка: {ex.Message}.");
            }
        }

        private void UpdateReportData()
        {
            var filteredData = ApplyFilters(FullData);
            var items = filteredData.OrderByDescending(saleItem => saleItem.Date);

            ReportData.Clear();
            foreach (var item in items)
                ReportData.Add(item);
        }

        private List<int> GetAvailableSalesYears()
        {
            return ReportData.Select(saleItem => saleItem.Date.Year).Distinct().OrderDescending().ToList();
        }

        private List<string> GetAvailableModels()
        {
            return ReportData.Select(x => x.ModelName).Distinct().Order().ToList();
        }

        private async void ReportSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ReportSettings.ModelFilter))
            {
                UpdateReportData();
                await Pivot.Refresh(ReportSettings, fullData);
            }
            else
            {
                await Pivot.Refresh(ReportSettings, fullData);
            }
        }
    }
}