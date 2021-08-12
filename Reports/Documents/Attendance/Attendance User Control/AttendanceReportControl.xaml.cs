using ERP.Reports.Documents.Attendance.Attendance_User_Control;
using ERP.Reports.Documents.Attendance.Today_Attendance;
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

namespace ERP.Reports.Documents.Attendance
{
    /// <summary>
    /// Interaction logic for AttendanceReportControl.xaml
    /// </summary>
    public partial class AttendanceReportControl : UserControl
    {
        NormalAttendanceViewModel attendanceViewModel = new NormalAttendanceViewModel();

        #region Member Windows

        NormalAttendanceWindow normalAttendanceReportWindow;
        MonthlyDetailAttendanceWindow monthlyDetailAttendanceReportWindow;
        MonthlyEarlyOutWindow monthlyEarlyOutReportWindow;
        MonthlyLateInWindow monthlyLateInReportWindow;
        MonthlyLateDeductionWindow monthlyLateDeductionReportWindow;
        DailyAttendanceWindow dailyAttendanceReportWindow;
        DailyAbsentisumDetailWindow dailyAbsentismReportWindow;
        TodayAttendanceWindow todayAttendanceReportWindow;
        TodayAbsentismWindow todayAbsentismWindowReport;
        MonthlyOverTimeDetailWindow monthlyOTDetailReportWindow;
        DailyOverTimeWindow dailyOTReportWindow;
        DailyMorningGraceAttendanceWindow dailyMorningGraceReportWindow;
        DailyGraceOutAttendanceWindow dailyEveningGraceReportWindow;
        InvalidDaysWindow invalidDaysReportWindow;
        MonthlyAttendanceSumarryWindow monthlyAttendanceSummaryReportWindow;
        MaxOtAttendanceDataWindow maxOTAttendanceReportWindow;
        UnprocessedAttendanceDataWindow unprocessAttendanceReportWindow;

        IDCardPrintingWindow IDcardDetails;
        AttendanceRegistryWindow AttendanceRegistry;

        #endregion

        #region Constructor

        public AttendanceReportControl()
        {
            InitializeComponent();
        } 

        #endregion

        #region Window Closing Methods

        void formClose()
        {
            if (normalAttendanceReportWindow != null)
                normalAttendanceReportWindow.Close();
            if (monthlyDetailAttendanceReportWindow != null)
                monthlyDetailAttendanceReportWindow.Close();
            if (monthlyEarlyOutReportWindow != null)
                monthlyEarlyOutReportWindow.Close();
            if (monthlyLateInReportWindow != null)
                monthlyLateInReportWindow.Close();
            if (monthlyLateDeductionReportWindow != null)
                monthlyLateDeductionReportWindow.Close();
            if (dailyAttendanceReportWindow != null)
                dailyAttendanceReportWindow.Close();
            if (dailyAbsentismReportWindow != null)
                dailyAbsentismReportWindow.Close();
            if (todayAttendanceReportWindow != null)
                todayAttendanceReportWindow.Close();
            if(todayAbsentismWindowReport != null)
                todayAbsentismWindowReport.Close();
            if (monthlyOTDetailReportWindow != null)
                monthlyOTDetailReportWindow.Close();
            if (dailyOTReportWindow != null)
                dailyOTReportWindow.Close();
            if (dailyMorningGraceReportWindow != null)
                dailyMorningGraceReportWindow.Close();
            if (dailyEveningGraceReportWindow != null)
                dailyEveningGraceReportWindow.Close();
            if (invalidDaysReportWindow != null)
                invalidDaysReportWindow.Close();
            if (monthlyAttendanceSummaryReportWindow != null)
                monthlyAttendanceSummaryReportWindow.Close();
            if (maxOTAttendanceReportWindow != null)
                maxOTAttendanceReportWindow.Close();
            if (unprocessAttendanceReportWindow != null)
                unprocessAttendanceReportWindow.Close();
            if (IDcardDetails != null)
                IDcardDetails.Close();
            if (AttendanceRegistry != null)
                AttendanceRegistry.Close();
            
        }

        #endregion

        #region Reports buttons click event handlers

        private void Normal_Attendence_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.NormalAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                try
                {
                    this.formClose();
                    normalAttendanceReportWindow = new NormalAttendanceWindow(attendanceViewModel);
                    normalAttendanceReportWindow.Show();
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
                    this.formClose();
                    monthlyDetailAttendanceReportWindow = new MonthlyDetailAttendanceWindow();
                    monthlyDetailAttendanceReportWindow.Show();
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
                MonthlyEarlyOutViewModel monthlyearlyoutViewModel = new MonthlyEarlyOutViewModel();
                this.formClose();
                monthlyEarlyOutReportWindow = new MonthlyEarlyOutWindow(monthlyearlyoutViewModel);
                monthlyEarlyOutReportWindow.Show();
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
                MonthilyLateInViewModel monthlyLateInViewModel = new MonthilyLateInViewModel();
                this.formClose();
                monthlyLateInReportWindow = new MonthlyLateInWindow(monthlyLateInViewModel);
                monthlyLateInReportWindow.Show();
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
                this.formClose();
                monthlyLateDeductionReportWindow = new MonthlyLateDeductionWindow();
                monthlyLateDeductionReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void daily_attendance_report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyAttendanceReportWindow = new DailyAttendanceWindow();
                dailyAttendanceReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void daily_absentisum_detail_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyAbsentisumReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyAbsentismReportWindow = new DailyAbsentisumDetailWindow();
                dailyAbsentismReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void today_attendance_report_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.TodayAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                todayAttendanceReportWindow = new TodayAttendanceWindow();
                todayAttendanceReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void today_absentism_report_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.TodayAbsentismReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                todayAbsentismWindowReport = new TodayAbsentismWindow();
                todayAbsentismWindowReport.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void monthly_overtime_summary_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.OverTimeSummaryReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                monthlyOTDetailReportWindow = new MonthlyOverTimeDetailWindow();
                monthlyOTDetailReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void daily_overtime_report_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyOverTimeReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyOTReportWindow = new DailyOverTimeWindow();
                dailyOTReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void monthly_grace_in_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.GraceInAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyMorningGraceReportWindow = new DailyMorningGraceAttendanceWindow();
                dailyMorningGraceReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void gracein_button_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.GraceOutAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyEveningGraceReportWindow = new DailyGraceOutAttendanceWindow();
                dailyEveningGraceReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Invalid_attendance_buttion_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.InvalidAttendanceReport), clsSecurity.loggedUser.user_id))
            {
                formClose();
                invalidDaysReportWindow = new InvalidDaysWindow();
                invalidDaysReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void attendance_Sumarry_buttion_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.AttendanceSummarryReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                monthlyAttendanceSummaryReportWindow = new MonthlyAttendanceSumarryWindow();
                monthlyAttendanceSummaryReportWindow.Show();

            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void attendance_Sumarry_buttion_Copy_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            IDcardDetails = new IDCardPrintingWindow();
            IDcardDetails.Show();
        }

        private void attendance_Sumarry_buttion_Copy1_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            AttendanceRegistry = new AttendanceRegistryWindow();
            AttendanceRegistry.Show();
        }

        private void Max_OT_Attendance_Data_btn_Click(object sender, RoutedEventArgs e)
        {
            this.formClose();
            maxOTAttendanceReportWindow = new MaxOtAttendanceDataWindow();
            maxOTAttendanceReportWindow.Show();
        }

        private void Unprocessed_Attendance_Data_btn_Click(object sender, RoutedEventArgs e)
        {
            this.formClose();
            unprocessAttendanceReportWindow = new UnprocessedAttendanceDataWindow();
            unprocessAttendanceReportWindow.Show();
        } 

        #endregion
    }
}
