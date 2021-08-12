using ERP.HelperClass;
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

namespace ERP.Loan_Module.Reports
{
    /// <summary>
    /// Interaction logic for EmployeeLoanDetailsReportWindow.xaml
    /// </summary>
    public partial class EmployeeLoanDetailsReportWindow : Window
    {
       // private EmployeeLoanDetailsReportViewModel viewModel;
        public EmployeeLoanDetailsReportWindow(ViewModelBase viewModel)
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
