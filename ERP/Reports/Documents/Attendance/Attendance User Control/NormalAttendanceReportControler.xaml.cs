using ERP.ERPService;
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

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    /// <summary>
    /// Interaction logic for NormalAttendanceReportControler.xaml
    /// </summary>
    public partial class NormalAttendanceReportControler : UserControl
    {


        public NormalAttendanceReportControler(NormalAttendanceViewModel viewmodel)
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };

        }

        private void print_rep(object sender, RoutedEventArgs e)
        {

        }
    

        private void clr(object sender, RoutedEventArgs e)
        {

        }

        private void CompanyBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((Grid)this.Parent).Children.Remove(this);
        }
    }
}
