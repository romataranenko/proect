using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Interfaces;
using TaskManager.Services;
using TaskManager.ViewModels;
using TaskManager.Views;

namespace TaskManager
{
    public partial class App : Application
    {
        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IDatabaseHelper, DatabaseHelper>();
            services.AddTransient<MainWindowViewModel>();

            var provider = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}