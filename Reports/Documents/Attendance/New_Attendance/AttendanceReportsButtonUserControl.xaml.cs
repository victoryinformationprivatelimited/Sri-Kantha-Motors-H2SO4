using ERP.Reports.Documents.Attendance.Attendance_User_Control;
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

namespace ERP.Reports.Documents.Attendance.New_Attendance
{
    /// <summary>
    /// Interaction logic for AttendanceReportsButtonUserControl.xaml
    /// </summary>
    public partial class AttendanceReportsButtonUserControl : UserControl
    {
        #region Report Window

        BasicReportWindow reportWindow;
        MonthlyAttendanceSumarryWindow attendSummaryWindow;
        UnprocessedAttendanceDataWindow unprocessDataWindow;

        #endregion

        #region Constructor

        public AttendanceReportsButtonUserControl()
        {
            InitializeComponent();
        } 

        #endregion

        #region Window Close Methods

        void closeWindows()
        {
            if (reportWindow != null)
                reportWindow.Close();
            if (attendSummaryWindow != null)
                attendSummaryWindow.Close();
            if (unprocessDataWindow != null)
                unprocessDataWindow.Close();
        }

        #endregion

        #region Attendance Reports Buttons Click Event Handlers

        private void Normal_Attendence_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.NormalAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.closeWindows();
                    AttendanceNewNormalAttendanceViewModel ViewModel = new AttendanceNewNormalAttendanceViewModel();
                    reportWindow = new BasicReportWindow(ViewModel);
                    reportWindow.Show();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Detail_Attendence_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DetailAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.closeWindows();
                    AttendanceNewNormalAttendanceWithStatusViewModel ViewModel = new AttendanceNewNormalAttendanceWithStatusViewModel();
                    reportWindow = new BasicReportWindow(ViewModel);
                }
                catch (Exception)
                {

                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void monthlyearlyout_button_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EarlyOutAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                this.closeWindows();
                AttendanceNewEarlyOutViewModel ViewModel = new AttendanceNewEarlyOutViewModel();
                reportWindow = new BasicReportWindow(ViewModel);
                reportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void monthly_late_in_attendance_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EarlyInAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                this.closeWindows();
                AttendanceNewLateInViewModel ViewModel = new AttendanceNewLateInViewModel();
                reportWindow = new BasicReportWindow(ViewModel);
                reportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void monthly_attendance_sumarry_check_box_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.LateDeductionReport), clsSecurity.loggedUser.user_id))
            {
                this.closeWindows();
                AttendanceNewLateAttendanceViewModel ViewModel = new AttendanceNewLateAttendanceViewModel();
                reportWindow = new BasicReportWindow(ViewModel);
                reportWindow.Show();

            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Employee_Shifts_btn_Click(object sender, RoutedEventArgs e)
        {
            this.closeWindows();
            AttendanceNewEmployeeShiftsViewModel ViewModel = new AttendanceNewEmployeeShiftsViewModel();
            reportWindow = new BasicReportWindow(ViewModel);
            reportWindow.Show();
        }

        private void Employee_Holidays_btn_Click(object sender, RoutedEventArgs e)
        {
            this.closeWindows();
            AttendanceNewEmployeeHolidaysViewModel ViewModel = new AttendanceNewEmployeeHolidaysViewModel();
            reportWindow = new BasicReportWindow(ViewModel);
            reportWindow.Show();
        }

        private void daily_attendance_report_check_box_Click(object sender, RoutedEventArgs e)
        {
            //if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyAttendanceReport), clsSecurity.loggedUser.user_id))
            //{

            //    DailyAttendanceUserControler dailyattendance = new DailyAttendanceUserControler();
            //    // MonthlyEarlyOutReportFilter earlyout = new MonthlyEarlyOutReportFilter(monthlyearlyout);
            //    AttendanceMDI.Children.Clear();
            //    AttendanceMDI.Children.Add(dailyattendance);
            //}
            //else
            //{
            //    clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            //}
        }

        private void daily_overtime_report_Click(object sender, RoutedEventArgs e)
        {
            //if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyOverTimeReport), clsSecurity.loggedUser.user_id))
            //{
            //    DailyOverTimeUserControl DailyOverTime = new DailyOverTimeUserControl();
            //    AttendanceMDI.Children.Clear();
            //    AttendanceMDI.Children.Add(DailyOverTime);
            //}
            //else
            //{
            //    clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            //}
        }

        private void CustomReport1_check_box_Click(object sender, RoutedEventArgs e)
        {
            this.closeWindows();
            AttendanceNewCustomNormalAttendanceViewModel ViewModel = new AttendanceNewCustomNormalAttendanceViewModel();
            reportWindow = new BasicReportWindow(ViewModel);
            reportWindow.Show();
        }

        private void CustomReport2_check_box_Click(object sender, RoutedEventArgs e)
        {
            this.closeWindows();
            AttendanceNewCustomNormalAttendanceWithStatusViewModel ViewModel = new AttendanceNewCustomNormalAttendanceWithStatusViewModel();
            reportWindow = new BasicReportWindow(ViewModel);
            reportWindow.Show();
        }

        #endregion

        private void break_time_report_btn_Click(object sender, RoutedEventArgs e)
        {
            this.closeWindows();
            AttendanceNewBreakTimeViewModel ViewModel = new AttendanceNewBreakTimeViewModel();
            reportWindow = new BasicReportWindow(ViewModel);
            reportWindow.Show();
        }

        private void attendance_Sumarry_buttion_Click(object sender, RoutedEventArgs e)
        {
            this.closeWindows();
            attendSummaryWindow = new MonthlyAttendanceSumarryWindow();
            attendSummaryWindow.Show();
        }

        private void unprocess_data_btn_Click(object sender, RoutedEventArgs e)
        {
            this.closeWindows();
            unprocessDataWindow = new UnprocessedAttendanceDataWindow();
            unprocessDataWindow.Show();
        }

    }
}
