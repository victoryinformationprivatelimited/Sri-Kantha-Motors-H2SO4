using CustomBusyBox;
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
    /// Interaction logic for ExternalBankLoanSlipWindow.xaml
    /// </summary>
    public partial class ExternalBankLoanSlipWindow : Window
    {
        ExternalBankLoanSlipViewModel viewModel;
        public ExternalBankLoanSlipWindow()
        {
            viewModel = new ExternalBankLoanSlipViewModel();
            InitializeComponent();
            BusyBox.InitializeThread(this);
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void button2_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
