using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ERP
{
    /// <summary>
    /// Interaction logic for PoSWindow.xaml
    /// </summary>
        

    public partial class PoSWindow : Window
    {
        DispatcherTimer dis = new DispatcherTimer();

        public PoSWindow()
        {
            InitializeComponent();
        }

        private void Rectangle_Loaded_1(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main_progress.Minimum = 0;
            main_progress.Maximum = 100;
            dis.Tick += dis_Tick;
            dis.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dis.Start();
            lblStatus.Text = "Going to open the browser.......";
        }

        void dis_Tick(object sender, EventArgs e)
        {
            main_progress.Value++;
            if (main_progress.Value == 40)
            {
                
                lblStatus.Text = "Getting redy with online....";
                dis.Stop();
                Process.Start("http://victoryinformation.com/product-description.php?prcode=101");
                dis.Start();
                
            }
            if (main_progress.Value > 40)
            {
                lblStatus.Text = "Opening Browser...";

            }
            if (main_progress.Value > 80)
            {
                lblStatus.Text = "Getting Ready..";
            
            }
            if (main_progress.Value == 100)
            {
                this.Close();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
