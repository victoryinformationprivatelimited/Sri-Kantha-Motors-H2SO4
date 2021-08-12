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

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    /// <summary>
    /// Interaction logic for EmployeeDetailReportUserControl.xaml
    /// </summary>
    public partial class EmployeeDetailReportUserControl : UserControl
    {
        EmployeeDetailViewModel viewmodel = new EmployeeDetailViewModel();
        public EmployeeDetailReportUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}
