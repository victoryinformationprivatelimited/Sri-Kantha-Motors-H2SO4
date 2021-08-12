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

namespace ERP.Payroll
{
    /// <summary>
    /// Interaction logic for EmployeeMultiplePaymentsUserControl.xaml
    /// </summary>
    public partial class EmployeeMultiplePaymentsUserControl : UserControl
    {
        EmployeeMultiplePaymentsViewModel viewModel;
        public EmployeeMultiplePaymentsUserControl()
        {
            viewModel = new EmployeeMultiplePaymentsViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
