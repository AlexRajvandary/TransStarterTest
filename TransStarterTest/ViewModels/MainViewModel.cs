using Domain.Interfaces;
using Infrastructure.Data;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using TransStarterTest.Commands;
using TransStarterTest.Models.Contracts;

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

            AddTabCommand = new RelayCommand(_ => AddTab());
            ExportCommand = new AsyncRelayCommand(ExportAsync);

            AddTab();
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

        public string SelectedExportFilePath { get; set; }

        public ICommand AddTabCommand { get; }

        public ICommand ExportCommand { get; }

        private async void AddTab()
        {
            var tab = new ReportTabViewModel($"Отчет {_tabCounter++}", _context);
            await tab.InitializeAsync();
            Tabs.Add(tab);
        }

        private async Task ExportAsync()
        {
            var folderPath = await _folderPickerService.PickFolderAsync();

            if(folderPath == null) { return; }

            if (SelectedTab.ViewMode == Models.Enums.ReportViewMode.Details)
            {
                var fileName = $"sales_report_{DateTime.Now:dd_MM_YYYY}.xlsx";
                var fullPath = Path.Combine(folderPath, fileName);

                await _exportService.ExportReportAsync(SelectedTab.Title, SelectedTab.ReportData, fullPath);
            }
            else
            {
                var fileName = $"pivot_report_{DateTime.Now:dd_MM_YYYY}.xlsx";
                var fullPath = Path.Combine(folderPath, fileName);

                await _exportService.ExportReportAsync(SelectedTab.Title, SelectedTab.Pivot.Rows, fullPath);
            }

            _notificationDialogService.ShowNotification("Файл успешно сохранён");
        }
    }
}