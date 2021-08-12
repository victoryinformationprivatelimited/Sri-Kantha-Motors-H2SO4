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

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    /// <summary>
    /// Interaction logic for DailyAbsentisumDetailWindow.xaml
    /// </summary>
    public partial class DailyAbsentisumDetailWindow : Window
    {
       DailyAbsentisumDetailViewModel viewmodel = new DailyAbsentisumDetailViewModel();
       public DailyAbsentisumDetailWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

       private void daily_absentism_report_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
           this.DragMove();
       }

       private void daily_absentism_report_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
       {
           this.DragMove();
       }

       private void daily_absentism_report_close_btn_Click(object sender, RoutedEventArgs e)
       {
           this.Close();
       }
       
    }
}
