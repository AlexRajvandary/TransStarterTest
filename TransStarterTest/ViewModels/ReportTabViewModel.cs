using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

using Infrastructure.Data;
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

        private List<SaleItemDto> _fullData = null!;
        private IMessageBoxService _messageBoxService;
        private ReportSettings _reportSettings = null!;
        private SalesHighlightSettings _salesHighlightSettings = null!;
        private ReportViewMode _viewMode;

        private readonly HashSet<string> _pivotReloadTriggers =
        [
            nameof(ReportSettings.GroupBy),
            nameof(ReportSettings.AggregateBy),
            nameof(ReportSettings.YearFilter)
        ];

        public ReportTabViewModel(AppDbContext context,
                                  IMessageBoxService messageBoxService,
                                  string title,
                                  ReportViewMode viewMode = ReportViewMode.Details)
        {
            _context = context;
            _messageBoxService = messageBoxService;
            Title = title;
            ViewMode = viewMode;

            ReportSettings = new ReportSettings();
            SalesHighlightSettings = new SalesHighlightSettings(isEnabled: true, _defaultHighlightThreshold);
            RefreshCommand = new AsyncRelayCommand(RefreshAsync);
            Pivot = new PivotViewModel();
        }

        /// <summary>
        /// Полный список продаж. Загружается из бд при инициализации один раз и потом по запросу с помощью кнопки "Обновить"
        /// </summary>
        public List<SaleItemDto> FullData
        {
            get => _fullData;
            set
            {
                if (_fullData != value)
                {
                    _fullData = value;
                    UpdateReportData();
                }
            }
        }

        /// <summary>
        /// Сводка продаж по месяцам для выбранного года
        /// </summary>
        public PivotViewModel Pivot { get; }

        public ICommand RefreshCommand { get; }

        /// <summary>
        /// Данные для отчёта (DataGrid привязан к этому свойству)
        /// </summary>
        public ObservableCollection<SaleItemDto> ReportData { get; set; } = new();

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

        public SalesHighlightSettings SalesHighlightSettings
        {
            get => _salesHighlightSettings;
            set => SetProperty(ref _salesHighlightSettings, value);
        }

        /// <summary>
        /// Название отчёта. Добавляется в заголовок таба и в название листа при экспорте
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Режим отчёта <list type="bullet">
        /// <item><see cref="ReportViewMode.Details"/> — Все продажи</item>
        /// <item><see cref="ReportViewMode.Pivot"/> — Сводка по месяцам</item>
        /// </list>
        /// </summary>
        public ReportViewMode ViewMode
        {
            get => _viewMode;
            set
            {
                SetProperty(ref _viewMode, value);
            }
        }

        /// <summary>
        /// Загружает данные продаж из бд
        /// </summary>
        /// <returns></returns>
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
                _messageBoxService.ShowError($"В процессе обновления отчёта произошла ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Фильтрует продажи по модели авто
        /// </summary>
        /// <param name="saleItems"></param>
        /// <returns></returns>
        private IEnumerable<SaleItemDto> ApplyFilters(IEnumerable<SaleItemDto> saleItems)
        {
            if (!string.IsNullOrEmpty(ReportSettings.ModelFilter) && ReportSettings.IsModelFilterSelected)
            {
                saleItems = saleItems.Where(s => s.ModelName == ReportSettings.ModelFilter);
            }

            return saleItems;
        }

        /// <summary>
        /// Полностью загружает данные в память
        /// </summary>
        /// <returns></returns>
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
                _messageBoxService.ShowError($"При загрузке отчёта всех продаж произошла ошибка: {ex.Message}.");
            }
        }

        /// <summary>
        /// Фильтрует и сортирует продажи. Метод вызывается после полного обновления списка продаж
        /// или изменений настроек фильтрации (при выборе другой модели авто).
        /// </summary>
        private void UpdateReportData()
        {
            var filteredData = ApplyFilters(FullData);
            var items = filteredData.OrderByDescending(saleItem => saleItem.Date);

            ReportData.Clear();
            foreach (var item in items)
                ReportData.Add(item);
        }

        /// <summary>
        /// Получаем список годов продаж для выпадающего списка фильтра.
        /// </summary>
        /// <returns></returns>
        private List<int> GetAvailableSalesYears()
        {
            return FullData.Select(saleItem => saleItem.Date.Year).Distinct().OrderDescending().ToList();
        }

        /// <summary>
        /// Получаем список моделей для выпадающего списка фильтра.
        /// </summary>
        /// <returns></returns>
        private List<string> GetAvailableModels()
        {
            return FullData.Select(x => x.ModelName).Distinct().Order().ToList();
        }

        /// <summary>
        /// Обновляет таблицы после изменений настроек фильтров и группировок.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ReportSettings_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReportSettings.ModelFilter))
            {
                UpdateReportData();
                await Pivot.Refresh(ReportSettings, _fullData);
            }
            else if (_pivotReloadTriggers.Contains(e.PropertyName!))
            {
                await Pivot.Refresh(ReportSettings, _fullData);
            }
        }
    }
}