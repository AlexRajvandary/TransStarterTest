namespace TransStarterTest.Domain.Contracts
{
    public interface IMessageBoxService
    {
        void ShowError(string message);
        void ShowNotification(string message);
    }
}