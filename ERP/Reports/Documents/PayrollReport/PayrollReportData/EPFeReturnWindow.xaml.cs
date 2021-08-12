using ERP.HelperClass;
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

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    /// <summary>
    /// Interaction logic for EPFeReturnWindow.xaml
    /// </summary>
    public partial class EPFeReturnWindow : Window
    {
        EPFeReturnViewModel viewModel;
        public EPFeReturnWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new EPFeReturnViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void epf_E_Return_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void epf_E_Return_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void epf_E_Return_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
