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
    /// Interaction logic for CompanyVariableUserControl.xaml
    /// </summary>
    public partial class CompanyVariableUserControl : UserControl
    {
        CompanyVariableViewModel viewModel = new CompanyVariableViewModel();
        public CompanyVariableUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void Searchbox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }
    }
}
