using TransStarterTest.Models.Enums;

namespace TransStarterTest.ViewModels
{
    public class ReportSettings : BaseViewModel
    {
        private GroupingOptions _groupBy;
        private ColumnOptions _columnBy;
        private AggregateOptions _aggregateBy;
        private int _yearFilter;

        public GroupingOptions GroupBy
        {
            get => _groupBy;
            set => SetProperty(ref _groupBy, value);
        }

        public ColumnOptions ColumnBy
        {
            get => _columnBy;
            set => SetProperty(ref _columnBy, value);
        }

        public AggregateOptions AggregateBy
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
