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
    /// Interaction logic for ExternalBankLoanProcessWindow.xaml
    /// </summary>
    public partial class ExternalBankLoanProcessWindow : Window
    {
        ExternalBankLoanProcessViewModel viewModel;
        public ExternalBankLoanProcessWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new ExternalBankLoanProcessViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            //SearchCombo.Items.Add("EPF No");//0
            //SearchCombo.Items.Add("First Name");//1
            //SearchCombo.Items.Add("Last Name");//2
            //SearchCombo.Items.Add("Loan Name");//3
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

         
           