namespace TransStarterTest.Models.Contracts
{
    public interface IFolderPickerService
    {
        Task<string?> PickFolderAsync(string? initialFolder = null);
    }
}