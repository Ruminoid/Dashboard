﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Ruminoid.Common.Helpers;
using Ruminoid.Dashboard.Helpers;
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
            UpdateHelper.Current.Dispose();
        }

        #endregion

        #region Loaded

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AppVersion = $"版本 {Assembly.GetExecutingAssembly().GetName().Version}";

            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd)?.AddHook(WndProc);
        }

        #endregion

        #region CaptionBar Hook

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_NCHITTEST)
            {
                Point p = new Point();
                int pInt = lParam.ToInt32();
                p.X = (pInt << 16) >> 16;
                p.Y = pInt >> 16;
                if (WndIn.PointFromScreen(p).Y > WndIn.ActualHeight) return IntPtr.Zero;
                Point rel = WndOut.PointFromScreen(p);
                if (rel.X >= 0 && rel.X <= WndOut.ActualWidth && rel.Y >= 0 && rel.Y <= WndOut.ActualHeight)
                {
                    return IntPtr.Zero;
                }
                handled = true;
                return new IntPtr(2);
            }

            return IntPtr.Zero;
        }

        private const int WM_NCHITTEST = 0x0084;

        #endregion

        #region Event Processors

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

        private void UpdateChannelButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            switch (button?.Tag as string)
            {
                case "stable":
                    Config.Current.UpdateChannel = "stable";
                    break;
                case "beta":
                    Config.Current.UpdateChannel = "beta";
                    break;
                case "others":
                    Config.Current.UpdateChannel = "";
                    break;
            }
        }

        private void UpdateButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateHelper.Current.TriggerProcess();
        }

        #endregion
    }
}
