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
    /// Interaction logic for EmployeeSelectionUserControl.xaml
    /// </summary>
    public partial class EmployeeSelectionUserControl : UserControl
    {
        public EmployeeSelectionUserControl(EmployeePayrollProcessViewModel viewModel)
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }

        private void searchbox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void searchbox_TextChanged_5(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }
    }
}
