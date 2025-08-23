using Domain.Interfaces;
using Infrastructure.Data;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using TransStarterTest.Commands;
using TransStarterTest.Domain.Contracts;
using TransStarterTest.Domain.Enums;

namespace TransStarterTest.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int _tabCounter = 1;
        private ReportTabViewModel _selectedTab;
        private readonly AppDbContext _context;
        private readonly IExportService _exportService;
        private readonly IFolderPickerService _folderPickerService;
        private readonly INotificationDialogService _notificationDialogService;

        public MainViewModel(AppDbContext context,
                             IExportService exportService,
                             IFolderPickerService folderPickerService,
                             INotificationDialogService notificationDialogService)
        {
            _context = context;
            _exportService = exportService;
            _folderPickerService = folderPickerService;
            _notificationDialogService = notificationDialogService;

            Tabs = new ObservableCollection<ReportTabViewModel>();

            AddTabCommand = new AsyncRelayCommand(AddTabAsync);
            ExportCommand = new AsyncRelayCommand(ExportAsync);

            //Для демонстрационных целей, чтобы экран не был пустым
            AddTabAsync(ReportViewMode.Details);
            AddTabAsync(ReportViewMode.Pivot);
            SelectedTab = Tabs.First();
        }

        public ObservableCollection<ReportTabViewModel> Tabs { get; set; }

        public ReportTabViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                SetProperty(ref _selectedTab, value);
            }
        }

        public ICommand AddTabCommand { get; }

        public ICommand ExportCommand { get; }

        private async Task AddTabAsync()
        {
            await AddTabAsync(ReportViewMode.Details);
        }

        private async Task AddTabAsync(ReportViewMode viewMode = ReportViewMode.Details)
        {
            try
            {
                var tab = new ReportTabViewModel($"Отчет {_tabCounter++}", _context, _notificationDialogService, viewMode);
                await tab.InitializeAsync();
                Tabs.Add(tab);
            }
            catch (Exception ex)
            {
                _notificationDialogService.ShowError($"При создании нового отчёта произошла ошибка: {ex.Message}");
            }
        }

        private async Task ExportAsync()
        {
            try
            {
                var folderPath = await _folderPickerService.PickFolderAsync();
               
                if (folderPath == null) { return; }

                if (SelectedTab.ViewMode == ReportViewMode.Details)
                {
                    var fileName = $"sales_report_{DateTime.Now:dd_MM_yyyy}.xlsx";
                    var fullPath = Path.Combine(folderPath, fileName);

                    await _exportService.ExportReportAsync(SelectedTab.Title, SelectedTab.ReportData, fullPath);
                }
                else
                {
                    var fileName = $"pivot_{SelectedTab.ReportSettings.ToString()}_report_{DateTime.Now:dd_MM_yyyy}.xlsx";
                    var fullPath = Path.Combine(folderPath, fileName);

                    await _exportService.ExportReportAsync(SelectedTab.Title, SelectedTab.Pivot.Rows, fullPath);
                }

                _notificationDialogService.ShowNotification("Файл успешно сохранён");
            }
            catch(Exception ex)
            {
                _notificationDialogService.ShowError($"Во время сохранения файла произошла ошибка: {ex.Message}");
            }
        }
    }
}