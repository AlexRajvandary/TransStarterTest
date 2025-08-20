using Infrastructure.Data;
using System.Collections.ObjectModel;

namespace TransStarterTest.ViewModels
{
    public class ReportTabViewModel : BaseViewModel
    {
        private string _aggregateBy;
        private string _groupBy;
        private string _title;
        private int _yearFilter;

        private readonly AppDbContext _context;

        public ReportTabViewModel(string title, AppDbContext context)
        {
            Title = title;
            _context = context;
        }
        public string AggregateBy
        {
            get => _aggregateBy;
            set { SetProperty(ref _aggregateBy, value); UpdateReport(); }
        }

        public string GroupBy
        {
            get => _groupBy;
            set { SetProperty(ref _groupBy, value); UpdateReport(); }
        }

        public string Title
        {
            get => _title;
            set { SetProperty(ref _title, value); }
        }

        public int YearFilter
        {
            get => _yearFilter;
            set { SetProperty(ref _yearFilter, value); UpdateReport(); }
        }

        public ObservableCollection<SaleItemViewModel> ReportData { get; set; } = new ObservableCollection<SaleItemViewModel>();

        private void UpdateReport()
        {
            ReportData = new ObservableCollection<SaleItemViewModel>(
     _context.Sales
         .SelectMany(s => s.Items)
         .Select(si => new SaleItemViewModel
         {
             ModelName = si.Car.Model.Name,
             CustomerFullName = si.Sale.Customer.FirstName + " " + si.Sale.Customer.LastName,
             Date = si.Sale.Date,
             Price = si.Price
         })
         .ToList()
 );
            OnPropertyChanged(nameof(ReportData));
        }
    }

    public class SaleItemViewModel
    {
        public string ModelName { get; set; }
        public string CustomerFullName { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
