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
    /// Interaction logic for ManualInternalLoanPaymentWindow.xaml
    /// </summary>
    public partial class ManualInternalLoanPaymentWindow : Window
    {
        private ManualInternalLoanPaymentViewModel viewModel;
        public ManualInternalLoanPaymentWindow()
        {
            InitializeComponent();
            viewModel = new ManualInternalLoanPaymentViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
