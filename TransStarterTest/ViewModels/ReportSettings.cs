namespace TransStarterTest.ViewModels
{
    public class ReportSettings : BaseViewModel
    {
        private string _groupBy;
        private string _columnBy;
        private string _aggregateBy;
        private int _yearFilter;

        public string GroupBy
        {
            get => _groupBy;
            set => SetProperty(ref _groupBy, value);
        }

        public string ColumnBy
        {
            get => _columnBy;
            set => SetProperty(ref _columnBy, value);
        }

        public string AggregateBy
        {
            get => _aggregateBy;
            set => SetProperty(ref _aggregateBy, value);
        }

        public int YearFilter
        {
            get => _yearFilter;
            set => SetProperty(ref _yearFilter, value);
        }
    }

}
