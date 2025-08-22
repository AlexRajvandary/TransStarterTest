using TransStarterTest.Models.Contracts;
using System.Windows;

namespace TransStarterTest.View
{
    public sealed class NotificationDialogService : INotificationDialogService
    {
        public void ShowNotification(string message)
        {
            MessageBox.Show(message);
        }
    }
}