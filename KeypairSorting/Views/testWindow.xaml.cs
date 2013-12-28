using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Entities.BackgroundWorkers;

namespace KeypairSorting.Views
{
    /// <summary>
    /// Interaction logic for testWindow.xaml
    /// </summary>
    public partial class testWindow : Window
    {
        public testWindow()
        {
            InitializeComponent();
            _backgroundWorker = BackgroundWorker.Make<string, string>
            (
                s =>
                {
                    var k = 1;
                    Thread.Sleep(8000);
                    //for (var i = 0; i < 1000000; i++)
                    //{
                    //    k++;
                    //}
                    return s + k;
                }
            );
        }


        private readonly IBackgroundWorker<string, string> _backgroundWorker; 

        private async void b1_Click(object sender, RoutedEventArgs e)
        {
            t1.Text = string.Empty;
            t1.Text = await _backgroundWorker.RunAsync("youuyw");
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            _backgroundWorker.Cancel();
        }

        private void bs_Click(object sender, RoutedEventArgs e)
        {
            t1.Text = _backgroundWorker.BackgroundWorkerStatus.ToString();
        }
    }
}
