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
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : UserControl
    {
        EmployeeViewModel viewModel = new EmployeeViewModel();
        public Employee()
        {
            

            InitializeComponent();

            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            Town.SelectedIndex = 0;
            
        }

        private void searchbox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
