using Infrastructure.Data;
using TransStarterTest.Domain.Contracts;
using TransStarterTest.Domain.Enums;
using TransStarterTest.ViewModels;

namespace TransStarterTest.Domain.Services;

public class ReportTabFactory : IReportTabFactory
{
    private readonly AppDbContext _context;
    private readonly IMessageBoxService _messageBoxService;

    public ReportTabFactory(AppDbContext context, IMessageBoxService notificationDialogService)
    {
        _context = context;
        _messageBoxService = notificationDialogService;
    }

    public ReportTabViewModel Create(string title, ReportViewMode viewMode)
    {
        return new ReportTabViewModel(_context, _messageBoxService, title, viewMode);
    }
}