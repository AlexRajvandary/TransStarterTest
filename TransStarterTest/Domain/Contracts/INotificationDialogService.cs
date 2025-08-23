namespace TransStarterTest.Domain.Contracts
{
    public interface INotificationDialogService
    {
        void ShowNotification(string message);
        void ShowError(string message);
    }
}