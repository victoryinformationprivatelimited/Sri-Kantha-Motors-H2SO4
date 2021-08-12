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

namespace ERP.Reports.Documents.HR_Report
{
    /// <summary>
    /// Interaction logic for HR_ReportsUserControl.xaml
    /// </summary>
    public partial class HR_ReportsUserControl : UserControl
    {
        public HR_ReportsUserControl()
        {
            InitializeComponent();
        }

        private void HR_Letters_Click_1(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            HrLettersReportContorl dt = new HrLettersReportContorl();
            MDIWrip.Children.Add(dt);
        }

        private void HR_Reports_Click_1(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            HrEmployeeReportUserControl dt = new HrEmployeeReportUserControl();
            MDIWrip.Children.Add(dt);
        }
    }
}
