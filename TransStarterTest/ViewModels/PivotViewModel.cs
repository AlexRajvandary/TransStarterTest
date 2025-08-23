using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TransStarterTest.Domain.DTOs;
using TransStarterTest.Domain.Services;
using TransStarterTest.Models.DTOs;

namespace TransStarterTest.ViewModels
{
    /// <summary>
    /// View model для сводки продаж по месяцам
    /// </summary>
    public class PivotViewModel : BaseViewModel
    {
        private readonly AppDbContext _context;
        private readonly PivotCalculator _pivotCalculator;

        public PivotViewModel(AppDbContext context)
        {
            _context = context;
            _pivotCalculator = new PivotCalculator();
        }

        public List<PivotRowViewDto> Rows { get; set; } = new();

        public async Task LoadAsync(ReportSettings reportSettings)
        {
            var sales = await _context.Sales
                .Where(sale => sale.Date.Year == reportSettings.YearFilter)
                .SelectMany(sale => sale.Items)
                .Include(si => si.Car).ThenInclude(c => c.Model)
                .Include(si => si.Car).ThenInclude(c => c.Brand)
                .Include(si => si.Sale).ThenInclude(s => s.Customer)
                .Select(item => new SaleItemDto
                {
                    Date = item.Sale.Date,
                    CustomerFullName = item.Sale.Customer.FirstName + " " + item.Sale.Customer.LastName,
                    BrandName = item.Car.Brand.Name,
                    ModelName = item.Car.Model.Name,
                    Price = (double)item.Price
                })
                .ToListAsync();

            Rows = _pivotCalculator.Calculate(sales, reportSettings);

            OnPropertyChanged(nameof(Rows));
        }
    }
}