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

namespace ERP.Payroll.RI_Allowance
{
    /// <summary>
    /// Interaction logic for EmployeeAllowanceUserControl.xaml
    /// </summary>
    public partial class EmployeeAllowanceUserControl : UserControl
    {
        EmployeeAllowanceViewModel viewModel;
        public EmployeeAllowanceUserControl()
        {
            InitializeComponent();
            viewModel = new EmployeeAllowanceViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)Parent).Children.Clear();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }


    }
}
