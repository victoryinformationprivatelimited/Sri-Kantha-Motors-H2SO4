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

namespace ERP.Payroll
{
    /// <summary>
    /// Interaction logic for EmployeeRulePerPersonWindow.xaml
    /// </summary>
    public partial class EmployeeRulePerPersonWindow : Window
    {
        EmployeeRulePerPersonViewModel viewModel;
        public EmployeeRulePerPersonWindow()
        {
            viewModel = new EmployeeRulePerPersonViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
