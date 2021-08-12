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
    /// Interaction logic for UnilakPayrollUserControl.xaml
    /// </summary>
    public partial class UnilakPayrollUserControl : UserControl
    {
        UnilakPayrollSumarryViewModel viewmodel = new UnilakPayrollSumarryViewModel();
        public UnilakPayrollUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }
    }
}
