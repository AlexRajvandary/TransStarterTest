using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IExportService
    {
        public Task ExportSales(IEnumerable<SaleItem> sales, string filePath);
    }
}