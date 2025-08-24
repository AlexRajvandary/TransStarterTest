using TransStarterTest.Domain.Enums;
using TransStarterTest.ViewModels;

namespace TransStarterTest.Domain.Contracts
{
    public interface IReportTabFactory
    {
        ReportTabViewModel Create(string title, ReportViewMode viewMode);
    }
}