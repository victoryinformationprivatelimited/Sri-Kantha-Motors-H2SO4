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

namespace ERP.Attendance.Attendance_Process
{
    /// <summary>
    /// Interaction logic for ProsessStartMDI.xaml
    /// </summary>
    public partial class ProsessStartMDI : UserControl
    {
        AttendanceProcessViewModel vie = new AttendanceProcessViewModel();
        //int Step = 1;
        public ProsessStartMDI(AttendanceProcessViewModel viewmodel)
        {

            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
            vie = viewmodel;
            ProcessStartUserControl startUC = new ProcessStartUserControl(viewmodel);
            ProcessMDI.Children.Clear();
            ProcessMDI.Children.Add(startUC);

        }
     
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //switch (Step)
            //{
            //    case 1:
                    
            //        Step = 2;
            //        break;
            //    case 2:

            //        vie.ProcessStatus = "Next";
            //        vie.ProcessStopStatus = "Exit";
            //        vie.ProcessStopButton = true;
            //        EmployeeAttendanceAfterProcess employeeAttendance = new EmployeeAttendanceAfterProcess(vie);
            //        ProcessMDI.Children.Clear();
            //        ProcessMDI.Children.Add(employeeAttendance);
            //        Step = 3;
            //        break;

            //    case 3:
            //          MessageBoxResult result = MessageBox.Show("Do you want to Countinue this Payroll Process", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //          if (result == MessageBoxResult.Yes)
            //          {
            //             // vie.ProcessStart();
            //              AttendanceView attendnceview = new AttendanceView(vie);
            //              ProcessMDI.Children.Clear();
            //              ProcessMDI.Children.Add(attendnceview);
            //              //SaveEmployeePayrollSumarry()
            //              Step = 5;
            //          }
            //        break;

            //    default:
            //        break;
            //}

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Do you want to Discard This Payroll Process", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    ((Grid)this.Parent).Children.Remove(this);
            //    this.vie.nextButtinContain="Exit";
            //    //((WrapPanel)this.Parent).Children.Remove((Grid)this.Parent);
            //}
           
        }
    }
}
