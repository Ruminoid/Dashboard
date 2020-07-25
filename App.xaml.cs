using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MetroRadiance.UI;
using Ruminoid.Dashboard.Windows;
using Squirrel;

namespace Ruminoid.Dashboard
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += (sender, args) =>
            {
                args.Handled = true;
                MessageBox.Show(
                    args.Exception.Message,
                    "灾难性故障",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK);
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                MessageBox.Show(
                    ((Exception)args.ExceptionObject)?.Message ?? "Exception",
                    "灾难性故障",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK);
            };

            // SquirrelAware

            using (var mgr = new UpdateManager(Config.Current.UpdateServer + Config.Current.UpdateChannel))
            {
                SquirrelAwareApp.HandleEvents(
                  onInitialInstall: v =>
                  {
                      mgr.CreateShortcutForThisExe();
                      Current.Shutdown(0);
                  },
                  onAppUpdate: v =>
                  {
                      mgr.CreateShortcutForThisExe();
                      Current.Shutdown(0);
                  },
                  onAppUninstall: v =>
                  {
                      mgr.RemoveShortcutForThisExe();
                      Current.Shutdown(0);
                  });
            }

            Dashboard.Properties.Resources.Culture = CultureInfo.CurrentUICulture;

            if (MainWindow is null) MainWindow = new MainWindow();
            MainWindow.Show();

            Current.Dispatcher?.Invoke(() => ThemeService.Current.ChangeTheme(Theme.Dark));
            Current.Dispatcher?.Invoke(() => ThemeService.Current.ChangeAccent(Accent.Blue));

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeService.Current.Register(this, Theme.Windows, Accent.Windows);
        }
    }
}
