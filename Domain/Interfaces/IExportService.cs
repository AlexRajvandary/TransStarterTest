using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IExportService
    {
        public Task ExportReportAsync<T>(string reportTitle, IEnumerable<T> items, string filePath);
    }
}