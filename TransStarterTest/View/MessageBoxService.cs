using System.Windows;
using TransStarterTest.Domain.Contracts;

namespace TransStarterTest.View
{
    public sealed class MessageBoxService : IMessageBoxService
    {
        private const string errorHeader = "Ошибка";
        private const string notificationHeader = "Уведомление";

        public void ShowError(string message)
        {
            MessageBox.Show(message, errorHeader, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowNotification(string message)
        {
            MessageBox.Show(message, notificationHeader, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}