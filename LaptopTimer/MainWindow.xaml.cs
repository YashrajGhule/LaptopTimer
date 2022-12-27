using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.Threading;

namespace LaptopTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int time { get; set; } = 0;
        private int threadActive { get; set; } = 1;
        KeyboardHook keyboardHook = new KeyboardHook();
        public MainWindow()
        {
            InitializeComponent();
            this.Hide();
            Thread thread = new Thread(new ThreadStart(timerThread));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void SHUTDOWN_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to shutdown system", "WARNING", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                terminateLockdown();
                Process.Start("shutdown", "/s /t 10");
            }
            threadActive = 1;
        }

        private void CONTINUE_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            terminateLockdown();
        }

        private void InitiateLockdown()
        {
            keyboardHook.InstallHook();
            threadActive = 0;
            this.Show();
        }

        private void terminateLockdown()
        {
            keyboardHook.UninstallHook();
            threadActive = 1;
            time = 0;
            this.Hide();
        }

        private void timerThread()
        {
            threadActive = 1;
            while (true)
            {
                if (threadActive == 1)
                {
                    if (time >= 3600)
                    {
                        Dispatcher.Invoke(InitiateLockdown);
                        threadActive = 0;
                        continue;
                    }
                    Thread.Sleep(10000);
                    time += 10;
                }
            }
        }
    }
}
