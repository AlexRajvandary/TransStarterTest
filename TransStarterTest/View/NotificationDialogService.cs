using System.Windows;
using TransStarterTest.Domain.Contracts;

namespace TransStarterTest.View
{
    public sealed class NotificationDialogService : INotificationDialogService
    {
        private string notificationHeader = "Уведомление";
        private string errorHeader = "Ошибка";

        public void ShowNotification(string message)
        {
            MessageBox.Show(message, notificationHeader, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, errorHeader, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}