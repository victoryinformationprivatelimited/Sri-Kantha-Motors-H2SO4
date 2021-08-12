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
using ERP.AttendanceModule.AttendanceMasters;
using ERP.AttendanceModule.Calendar;
using ERP.AttendanceModule.AttendanceProcessMaster;
using ERP.Attendance.Basic_Masters;
using ERP.Attendance.Attendance_Process;
using ERP.Masters;
using ERP.AttendanceModule.SMS;

namespace ERP.UI_UserControlers
{
    /// <summary>
    /// Interaction logic for AttendanceButtons.xaml
    /// </summary>
    public partial class AttendanceButtons : UserControl
    {
        ShiftCategory shiftCategoryWindow;
        ShiftDetail shiftDetailWindow;
        DailyShiftAssignWindow dailyShiftWindow;
        EmployeeShiftViewCalendarWindow shiftCalendarWindow;
        EmployeeAttendanceGroupWindow attendGroupWindow;
        EmployeeAttendanceGroupAssignWindow assignAttendGroupWindow;
        AttendanceProcessWindow attendProcessWindow;
        ShiftWeek shiftWeekWindow;
        HolidayTypeWindow holidayTypeWindow;
        AddHolidayWindow addHolidayWindow;
        AssignEmployeeHolidayWindow assignHolidayWindow;
        EmployeeMaxOTWindow maxOtWindow;
        AttendanceDataMigrationWindow dataMigrateWindow;
        AttendanceDeviceMasterWindow attendanceDeviceAddWindow;
        DataDownloadWindow attendanceDataDownloadWindow;
        ManualEmployeeAttendanceWindow manualAttendanceWindow;
        PeriodWindow attendancePeriodWindow;
        ManualAttendanceUploadFromTextFileWindow ManualAttendanceUploadFromTextFileW;
        SMSAttendanceWindow SMSAttendanceW;
        AssignEmployeeLocations AssignEmployeeLocationWindows;
        AttendanceSaveFromAPI AttendanceSaveFromAPIW;

        public AttendanceButtons()
        {
            InitializeComponent();
        }

        void closeForms()
        {
            if (shiftCategoryWindow != null)
                shiftCategoryWindow.Close();
            if (shiftDetailWindow != null)
                shiftDetailWindow.Close();
            if (dailyShiftWindow != null)
                dailyShiftWindow.Close();
            if (shiftCalendarWindow != null)
                shiftCalendarWindow.Close();
            if (attendGroupWindow != null)
                attendGroupWindow.Close();
            if (assignAttendGroupWindow != null)
                assignAttendGroupWindow.Close();
            if (attendProcessWindow != null)
                attendProcessWindow.Close();
            if (shiftWeekWindow != null)
                shiftWeekWindow.Close();
            if (holidayTypeWindow != null)
                holidayTypeWindow.Close();
            if (addHolidayWindow != null)
                addHolidayWindow.Close();
            if (assignHolidayWindow != null)
                assignHolidayWindow.Close();
            if (maxOtWindow != null)
                maxOtWindow.Close();
            if (dataMigrateWindow != null)
                dataMigrateWindow.Close();
            if (attendanceDeviceAddWindow != null)
                attendanceDeviceAddWindow.Close();
            if (attendanceDataDownloadWindow != null)
                attendanceDataDownloadWindow.Close();
            if (manualAttendanceWindow != null)
                manualAttendanceWindow.Close();
            if (attendancePeriodWindow != null)
                attendancePeriodWindow.Close();
            if (ManualAttendanceUploadFromTextFileW != null)
                ManualAttendanceUploadFromTextFileW.Close();
            if (SMSAttendanceW != null)
                SMSAttendanceW.Close();
            if (AttendanceSaveFromAPIW != null)
                AttendanceSaveFromAPIW.Close();
        }

        private void ShiftCategoryChkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(301))
            {
                this.closeForms();
                shiftCategoryWindow = new ShiftCategory();
                shiftCategoryWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void ShiftDetailChkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(302))
            {
                this.closeForms();
                shiftDetailWindow = new ShiftDetail();
                shiftDetailWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void dailyShiftBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(306))
            {
                this.closeForms();
                dailyShiftWindow = new DailyShiftAssignWindow();
                dailyShiftWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void EmployeeCalendar_Click(object sender, RoutedEventArgs e)
        {
            CalendarWindow Cal = new CalendarWindow();
            Cal.Show();
        }

        private void empShiftCalendarBtn_Click(object sender, RoutedEventArgs e)
        {
            this.closeForms();
            shiftCalendarWindow = new EmployeeShiftViewCalendarWindow();
            shiftCalendarWindow.Show();
        }

        private void empAttendanceGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(303))
            {
                this.closeForms();
                attendGroupWindow = new EmployeeAttendanceGroupWindow();
                attendGroupWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void assignAttendanceGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(304))
            {
                this.closeForms();
                assignAttendGroupWindow = new EmployeeAttendanceGroupAssignWindow();
                assignAttendGroupWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void attendanceProcessBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(310))
            {
                this.closeForms();
                attendProcessWindow = new AttendanceProcessWindow();
                attendProcessWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void shiftWeekBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(305))
            {
                this.closeForms();
                shiftWeekWindow = new ShiftWeek();
                shiftWeekWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void holidayTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(307))
            {
                this.closeForms();
                holidayTypeWindow = new HolidayTypeWindow();
                holidayTypeWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void holidayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(308))
            {
                this.closeForms();
                addHolidayWindow = new AddHolidayWindow();
                addHolidayWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void assignHolidayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(309))
            {
                this.closeForms();
                assignHolidayWindow = new AssignEmployeeHolidayWindow();
                assignHolidayWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

       

        private void empMaxOTBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(311))
            {
                this.closeForms();
                maxOtWindow = new EmployeeMaxOTWindow();
                maxOtWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void empDataMigrateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(312))
            {
                this.closeForms();
                dataMigrateWindow = new AttendanceDataMigrationWindow();
                dataMigrateWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Attendence_device_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(313))
            {
                this.closeForms();
                attendanceDeviceAddWindow = new AttendanceDeviceMasterWindow();
                attendanceDeviceAddWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Data_Download_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(314))
            {
                closeForms();
                attendanceDataDownloadWindow = new DataDownloadWindow();
                attendanceDataDownloadWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void manual_attendance_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(315))
            {
                closeForms();
                manualAttendanceWindow = new ManualEmployeeAttendanceWindow();
                manualAttendanceWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void attendance_period_btn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(316))
            {
                closeForms();
                attendancePeriodWindow = new PeriodWindow();
                attendancePeriodWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void attendance_textfile_upload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(317))
            {
                closeForms();
                ManualAttendanceUploadFromTextFileW = new ManualAttendanceUploadFromTextFileWindow();
                ManualAttendanceUploadFromTextFileW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        //private void attendance_ot_approve_Click(object sender, RoutedEventArgs e)
        //{
        //    if (clsSecurity.GetViewPermission(318))
        //    {
        //        closeForms();
        //        OTApprovalsW = new OTApprovalsWindow();
        //        OTApprovalsW.Show();
        //    }
        //    else
        //    {
        //        clsMessages.setMessage("You don't have permission to view this form");
        //    }
        //}

        //private void attendance_ot_approve_level_Click(object sender, RoutedEventArgs e)
        //{
        //    if (clsSecurity.GetViewPermission(319))
        //    {
        //        closeForms();
        //        OTApprovalRulesW = new OTApprovalRulesWindow();
        //        OTApprovalRulesW.Show();
        //    }
        //    else
        //    {
        //        clsMessages.setMessage("You don't have permission to view this form");
        //    }
        //}

        //private void employee_attendance_ot_approve_level_Click(object sender, RoutedEventArgs e)
        //{
        //    if (clsSecurity.GetViewPermission(320))
        //    {
        //        closeForms();
        //        EmployeeOTApprovalRulesW = new EmployeeOTApprovalRulesWindow();
        //        EmployeeOTApprovalRulesW.Show();
        //    }
        //    else
        //    {
        //        clsMessages.setMessage("You don't have permission to view this form");
        //    }
        //}

        private void SMSAttendance_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(321))
            {
                closeForms();
                SMSAttendanceW = new SMSAttendanceWindow();
                SMSAttendanceW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void attendance_ot_approve_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(322))
            {
                closeForms();
                AssignEmployeeLocationWindows = new AssignEmployeeLocations();
                AssignEmployeeLocationWindows.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void attendance_From_API_Click(object sender, RoutedEventArgs e)
        {
            closeForms();
            AttendanceSaveFromAPIW = new AttendanceSaveFromAPI();
            AttendanceSaveFromAPIW.Show();
        }

        private void attendance_ot_approve_level_Click(object sender, RoutedEventArgs e)
        {

        }

        private void attendance_ot_approve_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
