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

namespace ERP.Masters
{
    /// <summary>
    /// Interaction logic for EmployeeDeductionPaymentWindow.xaml
    /// </summary>
    public partial class EmployeeDeductionPaymentWindow : Window
    {
        EmployeeDeductionPaymentViewModel viewModel = new EmployeeDeductionPaymentViewModel();
        public EmployeeDeductionPaymentWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
