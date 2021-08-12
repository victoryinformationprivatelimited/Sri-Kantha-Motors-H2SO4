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

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    /// <summary>
    /// Interaction logic for EmployeeReports.xaml
    /// </summary>
    public partial class EmployeeReportsUserControl : UserControl
    {
        EmployeeReportsViewModel ViewModel;

        public EmployeeReportsUserControl()
        {
            InitializeComponent();

            ViewModel = new EmployeeReportsViewModel();
            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
