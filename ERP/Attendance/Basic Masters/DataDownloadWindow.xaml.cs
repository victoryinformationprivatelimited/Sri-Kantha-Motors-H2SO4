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
using System.Windows.Shapes;

namespace ERP.Attendance.Basic_Masters
{
    /// <summary>
    /// Interaction logic for DataDownloadWindow.xaml
    /// </summary>
    public partial class DataDownloadWindow : Window
    {
        DataDownloadViewModel viewModel;
        public DataDownloadWindow()
        {
            InitializeComponent();
            viewModel = new DataDownloadViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Data_Download_Master_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}