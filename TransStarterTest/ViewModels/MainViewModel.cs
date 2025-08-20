using Infrastructure.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TransStarterTest.Commands;

namespace TransStarterTest.ViewModels
{
    public class MainViewModel
    {
        private int _tabCounter = 1;
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public MainViewModel(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;

            Tabs = new ObservableCollection<ReportTabViewModel>();
            GroupingOptions = new ObservableCollection<string> { "Модель", "Бренд", "Покупатель" };
            AggregateOptions = new ObservableCollection<string> { "Количество продаж", "Сумма продаж" };
            AvailableYears = new ObservableCollection<int> { 2023, 2024, 2025 };

            AddTabCommand = new RelayCommand(_ => AddTab());
            ExportCommand = new RelayCommand(_ => Export());

            AddTab();
        }
        public ObservableCollection<string> AggregateOptions { get; set; }

        public ObservableCollection<int> AvailableYears { get; set; }

        public ObservableCollection<string> GroupingOptions { get; set; }

        public ObservableCollection<ReportTabViewModel> Tabs { get; set; }

        public ICommand AddTabCommand { get; }

        public ICommand ExportCommand { get; }

        private void AddTab()
        {
            var tab = new ReportTabViewModel($"Отчет {_tabCounter++}", _context)
            {
                GroupBy = GroupingOptions[0],
                AggregateBy = AggregateOptions[0],
                YearFilter = AvailableYears[0]
            };
            Tabs.Add(tab);
        }

        private void Export()
        {
            // Тут логика экспорта в Excel
        }
    }
}