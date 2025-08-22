using Infrastructure.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TransStarterTest.Commands;

namespace TransStarterTest.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private int _tabCounter = 1;
        private ReportTabViewModel _selectedTab;
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public MainViewModel(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;

            Tabs = new ObservableCollection<ReportTabViewModel>();
           
            AddTabCommand = new RelayCommand(_ => AddTab());
            ExportCommand = new RelayCommand(_ => Export());

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

        public ICommand AddTabCommand { get; }

        public ICommand ExportCommand { get; }

        private async void AddTab()
        {
            var tab = new ReportTabViewModel($"Отчет {_tabCounter++}", _context);
            await tab.InitializeAsync();
            Tabs.Add(tab);
        }

        private void Export()
        {
            // Тут логика экспорта в Excel
        }
    }
}