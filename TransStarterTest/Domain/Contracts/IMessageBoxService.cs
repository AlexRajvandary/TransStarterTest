namespace TransStarterTest.Domain.Contracts
{
    public interface IMessageBoxService
    {
        void ShowNotification(string message);
        void ShowError(string message);
    }
}