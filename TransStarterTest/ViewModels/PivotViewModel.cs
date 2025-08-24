using TransStarterTest.Domain.DTOs;
using TransStarterTest.Domain.Services;

namespace TransStarterTest.ViewModels
{
    /// <summary>
    /// View model для сводки продаж по месяцам
    /// </summary>
    public class PivotViewModel : BaseViewModel
    {
        private readonly PivotCalculator _pivotCalculator;

        public PivotViewModel()
        {
            _pivotCalculator = new PivotCalculator();
        }

        public List<PivotRowDto> Rows { get; set; } = new();

        public async Task Refresh(ReportSettings reportSettings, IEnumerable<SaleItemDto> saleItems)
        {
            Rows = await _pivotCalculator.CalculateAsync(saleItems, reportSettings);
            OnPropertyChanged(nameof(Rows));
        }
    }
}