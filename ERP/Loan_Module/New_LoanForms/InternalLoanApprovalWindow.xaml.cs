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
    /// Interaction logic for InternalLoanApprovalWindow.xaml
    /// </summary>
    public partial class InternalLoanApprovalWindow : Window
    {
        private InternalLoanApprovalViewModel viewModel;
        public InternalLoanApprovalWindow()
        {
             this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new InternalLoanApprovalViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            SearchCombo.Items.Add("First Name");
            SearchCombo.Items.Add("Surname");
            SearchCombo.Items.Add("Loan Name");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
          