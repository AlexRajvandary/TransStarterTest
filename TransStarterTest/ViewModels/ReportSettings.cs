using TransStarterTest.Models.Enums;

namespace TransStarterTest.ViewModels
{
    public class ReportSettings : BaseViewModel
    {
        private AggregateOptions _aggregateBy;
        private GroupingOptions _groupBy;
        private List<string> _models;
        private string? _modelFilter;
        private List<int> _years;
        private int _yearFilter;

        public AggregateOptions AggregateBy
        {
            get => _aggregateBy;
            set => SetProperty(ref _aggregateBy, value);
        }
        public List<string> AvailableModels
        {
            get => _models;
            set => SetProperty(ref _models, value);
        }

        public List<int> AvailableYears
        {
            get => _years;
            set => SetProperty(ref _years, value);
        }

        public GroupingOptions GroupBy
        {
            get => _groupBy;
            set => SetProperty(ref _groupBy, value);
        }

        public string? ModelFilter
        {
            get => _modelFilter;
            set => SetProperty(ref _modelFilter, value);
        }

        public int YearFilter
        {
            get => _yearFilter;
            set => SetProperty(ref _yearFilter, value);
        }

        public void Initialize(List<string> models, List<int> years)
        {
            AvailableModels = models;
            AvailableYears = years;
            YearFilter = AvailableYears.FirstOrDefault();
            ModelFilter = AvailableModels.FirstOrDefault();
        }

        public override string ToString()
        {
            var info = ModelFilter == "(Не выбрано)"
                ? $"{GroupBy.GetString()}_{YearFilter}"
                : $"{GroupBy.GetString()}_{ModelFilter}_{YearFilter}";
            return info;
        }
    }
}