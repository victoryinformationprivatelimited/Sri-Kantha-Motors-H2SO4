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

namespace ERP
{
    /// <summary>
    /// Interaction logic for CompanyRulesUserControl.xaml
    /// </summary>
    public partial class CompanyRulesUserControl : UserControl
    {
        private CompanyRulesViewModel viewModel = new CompanyRulesViewModel();
        public CompanyRulesUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void SearchBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            SearchBox.Focus();
        }
        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
