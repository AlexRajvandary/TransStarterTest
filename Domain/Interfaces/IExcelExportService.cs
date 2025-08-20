using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IExcelExportService
    {
        public Task ExportSales(IEnumerable<Sale> sales, string filePath);
    }
}