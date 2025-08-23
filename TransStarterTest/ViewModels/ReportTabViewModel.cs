using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private INotificationDialogService _notificationDialogService;

        public ReportTabViewModel(string title,
                                  AppDbContext context,
                                  INotificationDialogService notificationDialogService,
                                  ReportViewMode viewMode = ReportViewMode.Details)
        {
            Title = title;
            _context = context;
            _notificationDialogService = notificationDialogService;
            Pivot = new PivotViewModel(context);
            ReportSettings = new ReportSettings();
            SalesHighlightSettings = new SalesHighlightSettings(isEnabled: true, _defaultHighlightThreshold);
            ViewMode = viewMode;
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
            try
            {
                //Проблемное место, сейчас данные загружаются всякий раз при изменении фильтров, при выборе таба
                if (ViewMode == ReportViewMode.Details)
                    await LoadData();
                else
                    await Pivot.LoadAsync(ReportSettings);
            }
            catch(Exception ex)
            {
                _notificationDialogService.ShowError($"В процессе обновления отчёта произошла ошибка: {ex.Message}");
            }
        }

        private IQueryable<SaleItemDto> ApplyFilters(IQueryable<SaleItemDto> saleItems)
        {
            if (!string.IsNullOrEmpty(ReportSettings.ModelFilter) && ReportSettings.IsModelFilterSelected)
            {
                saleItems = saleItems.Where(s => s.ModelName == ReportSettings.ModelFilter);
            }

            return saleItems;
        }

        private async Task LoadData()
        {
            try
            {
                //Проблемное место, сейчас данные загружаются всякий раз при изменении фильтров, при выборе таба
                var query = _context.Sales
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
                    });

                query = ApplyFilters(query);
                var items = await query.OrderByDescending(saleItem => saleItem.Date).ToListAsync();

                ReportData.Clear();
                foreach (var item in items)
                    ReportData.Add(item);

                OnPropertyChanged(nameof(ReportData));
            }
            catch (Exception ex) 
            {
                _notificationDialogService.ShowError($"При загрузке отчёта всех продаж произошла ошибка: {ex.Message}.");
            }
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
            await RefreshAsync();
        }
    }
}