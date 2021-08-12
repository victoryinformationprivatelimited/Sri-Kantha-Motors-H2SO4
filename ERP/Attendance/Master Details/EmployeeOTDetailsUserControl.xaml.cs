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

namespace ERP.MastersDetails
{
    /// <summary>
    /// Interaction logic for EmployeeOTDetailsUserControl.xaml
    /// </summary>
    public partial class EmployeeOTDetailsUserControl : UserControl
    {
        private EmployeeOTDetailsViewModel viewModel = new EmployeeOTDetailsViewModel();
        public EmployeeOTDetailsUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void Employee_Attendence_Close_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void Employee_OT_Details_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
