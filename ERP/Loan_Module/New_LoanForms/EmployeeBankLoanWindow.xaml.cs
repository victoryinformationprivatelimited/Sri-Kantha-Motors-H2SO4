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

namespace ERP.Loan_Module.New_LoanForms
{
    /// <summary>
    /// Interaction logic for EmployeeBankLoanWindow.xaml
    /// </summary>
    public partial class EmployeeBankLoanWindow : Window
    {
        private EmployeeBankLoanViewModel viewModel;
        public EmployeeBankLoanWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new EmployeeBankLoanViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
