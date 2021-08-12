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

namespace ERP.Attendance
{
    /// <summary>
    /// Interaction logic for DataDownloadUserControl.xaml
    /// </summary>
    public partial class DataDownloadUserControl : UserControl
    {
        private DataDownloadViewModel viewModel = new DataDownloadViewModel();
        public DataDownloadUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Data_Download_Master_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
