using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.IO;
using System.Windows;
using TransStarterTest.Domain.Contracts;
using TransStarterTest.View;
using TransStarterTest.ViewModels;

namespace TransStarterTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public ServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var culture = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var services = new ServiceCollection();

            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            MessageBox.Show($"Фатальная ошибка {ex?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show($"Необработанное исключение в Task: {e.Exception.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            e.SetObserved();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Oшибка {e?.Exception}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var solutionDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)
                                       .Parent?.Parent?.Parent?.Parent?.FullName;

            if (solutionDir == null)
                throw new Exception("Не удалось определить путь к решению");

            var dbFolder = Path.Combine(solutionDir, "Database");
            if (!Directory.Exists(dbFolder))
                Directory.CreateDirectory(dbFolder);

            var dbPath = Path.Combine(dbFolder, "sales.db");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            services.AddSingleton<MainViewModel>();
            services.AddTransient<ReportTabViewModel>();

            services.AddSingleton<MainWindow>(provider =>
            {
                var vm = provider.GetRequiredService<MainViewModel>();
                return new MainWindow { DataContext = vm };
            });

            services.AddSingleton<IExportService, ExcelExportService>();
            services.AddSingleton<IFolderPickerService, FolderPickerService>();
            services.AddSingleton<INotificationDialogService, NotificationDialogService>();
        }
    }
}
