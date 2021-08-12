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
    /// Interaction logic for EmployeeAttandanceUserControl.xaml
    /// </summary>
    public partial class EmployeeAttandanceUserControl : UserControl
    {
        private EmployeeAttendanceViewModel viewModel = new EmployeeAttendanceViewModel();
        public EmployeeAttandanceUserControl()
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
    }
}
