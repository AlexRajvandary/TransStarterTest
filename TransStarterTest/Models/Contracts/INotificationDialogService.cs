namespace TransStarterTest.Models.Contracts
{
    public interface INotificationDialogService
    {
        void ShowNotification(string message);
        void ShowError(string message);
    }
}