using TransStarterTest.Domain.Contracts;

namespace TransStarterTest.View
{
    public sealed class FolderPickerService : IFolderPickerService
    {
        public Task<string?> PickFolderAsync(string? initialFolder = null)
        {
            return Task.FromResult(Show(initialFolder));
        }

        private static string? Show(string? initialFolder)
        {
            Microsoft.Win32.OpenFolderDialog dialog = new Microsoft.Win32.OpenFolderDialog();
            dialog.Title = "Выберите папку для экспорта отчёта";
            bool? result = dialog.ShowDialog();

            return result != null ? result == true ? dialog.FolderName : null : null;
        }
    }
}