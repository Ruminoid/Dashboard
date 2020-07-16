using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Ruminoid.Common.Helpers;
using Ruminoid.Dashboard.Models;
using Path = System.IO.Path;

namespace Ruminoid.Dashboard.Windows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;

            Closing += OnClosing;

            Closed += (sender, args) => Application.Current.Shutdown(0);
        }

        #region Closing

        private void OnClosing(object sender, CancelEventArgs e)
        {
            ConfigHelper<Config>.SaveConfig(Config.Current);
        }

        #endregion

        #region Loaded

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void ProductButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (!(button?.Tag is Product product)) return;
            button.IsEnabled = false;
            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Ruminoid.{product.Id}.exe"));
            _ = new DispatcherTimer(
                TimeSpan.FromSeconds(2),
                DispatcherPriority.Normal,
                (o, args) => button.IsEnabled = true,
                Dispatcher.CurrentDispatcher);
        }
    }
}
