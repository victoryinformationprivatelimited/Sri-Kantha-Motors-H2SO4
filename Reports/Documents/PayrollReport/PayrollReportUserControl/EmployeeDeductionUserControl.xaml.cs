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

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    /// <summary>
    /// Interaction logic for EmployeeDeductionUserControl.xaml
    /// </summary>
    public partial class EmployeeDeductionUserControl : UserControl
    {
        EmployeeDeductionViewModel viewmodel = new EmployeeDeductionViewModel();
        public EmployeeDeductionUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }
    }
}
