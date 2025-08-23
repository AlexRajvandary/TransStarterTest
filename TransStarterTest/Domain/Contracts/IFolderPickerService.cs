namespace TransStarterTest.Domain.Contracts
{
    public interface IFolderPickerService
    {
        Task<string?> PickFolderAsync(string? initialFolder = null);
    }
}