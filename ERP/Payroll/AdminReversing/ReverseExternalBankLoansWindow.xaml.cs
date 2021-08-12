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

namespace ERP.Payroll.AdminReversing
{
    /// <summary>
    /// Interaction logic for ReverseExternalBankLoansWindow.xaml
    /// </summary>
    public partial class ReverseExternalBankLoansWindow : Window
    {
        ReverseExternalBankLoansViewModel viewModel;
        public ReverseExternalBankLoansWindow()
        {
            viewModel = new ReverseExternalBankLoansViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}
