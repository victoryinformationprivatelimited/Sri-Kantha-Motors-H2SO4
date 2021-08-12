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

namespace ERP.Payroll.Employee_Third_Party_Payments
{
    /// <summary>
    /// Interaction logic for EmployeeThirdPartyPaymentsWindow.xaml
    /// </summary>
    public partial class EmployeeThirdPartyPaymentsWindow : Window
    {
        EmployeeThirdPartyPaymentsViewModel viewModel;
        public EmployeeThirdPartyPaymentsWindow()
        {
            InitializeComponent();
            BusyBox.InitializeThread(this);
            viewModel = new EmployeeThirdPartyPaymentsViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
    }
}
