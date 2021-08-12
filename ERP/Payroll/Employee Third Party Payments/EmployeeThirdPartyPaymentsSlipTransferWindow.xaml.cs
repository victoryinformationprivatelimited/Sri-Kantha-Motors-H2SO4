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

namespace ERP.Payroll.Employee_Third_Party_Payments
{
    /// <summary>
    /// Interaction logic for EmployeeThirdPartyPaymentsSlipTransferWindow.xaml
    /// </summary>
    public partial class EmployeeThirdPartyPaymentsSlipTransferWindow : Window
    {
        EmployeeThirdPartyPaymentsSlipTransferViewModel viewModel;
        public EmployeeThirdPartyPaymentsSlipTransferWindow()
        {
            viewModel = new EmployeeThirdPartyPaymentsSlipTransferViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}
