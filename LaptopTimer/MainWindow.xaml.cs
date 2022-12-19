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
                Process.Start("shutdown", "/s /t 0");
            }
            threadActive = 1;
        }

        private void CONTINUE_BUTTON_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            time = 0;
            threadActive = 1;
        }

        private void timerThread()
        {
            threadActive = 1;
            while (true)
            {
                Thread.Sleep(10000);
                if (threadActive == 1)
                {
                    if (time >= 60)
                    {
                        this.Dispatcher.Invoke(() => { this.Show(); });
                        threadActive = 0;
                    }
                    time += 10;
                }
            }
        }
    }
}
