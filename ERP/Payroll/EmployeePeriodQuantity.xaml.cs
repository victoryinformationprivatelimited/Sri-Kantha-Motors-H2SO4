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
    /// Interaction logic for EmployeePeriodQuantity.xaml
    /// </summary>
    public partial class EmployeePeriodQuantity : UserControl
    {
        EmployeePeriodQuantityViewModel viewModel = new EmployeePeriodQuantityViewModel();
        public EmployeePeriodQuantity()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }

        private void Searchbox_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
