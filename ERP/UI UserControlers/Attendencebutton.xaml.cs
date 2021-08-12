using ERP.Attendance;
using ERP.Attendance.Attendance_Data_Migration;
using ERP.Attendance.Attendance_Process;
using ERP.Attendance.Basic_Masters;
using ERP.Attendance.Lieu;
using ERP.Attendance.Master_Details;
using ERP.Attendance.Rosters;
using ERP.Attendence;
using ERP.MastersDetails;
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


namespace ERP.UI_UserControlers
{
    /// <summary>
    /// Interaction logic for Attendencebutton.xaml
    /// </summary>
    public partial class Attendencebutton : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();
        LieuLeaveWindow LieuLeaveW;
        AttendanceDataUploadWindow AttendanceDataUploadW;
        ShiftDetailWindow ShiftDetailW;
        ManualAttendanceWindow ManualAttendanceW;
        MainAttendanceProcessWindow MainAttendanceProcessW;
        EmployeeAttandanceWindow EmployeeAttandanceW;
        RosterDetailWindow RosterDetailW;
        RosterGroupWindow RosterGroupW;
        AttendanceDeviceMasterWindow AttendanceDeviceMasterW;
        ShiftCatergoryMasterWindow ShiftCatergoryMasterW;
        DataDownloadWindow DataDownloadW;
        DailyAttendanceProcessWindow DailyAttendanceProcessW;
        ManualEmployeeAttendanceWindow ManualEmployeeAttendanceW;
        MaxOtApprovedDaysWindow MaxOtApprovedDaysW;
        ManualAttendanceUploadFromTextFileWindow ManualAttendanceUploadFromTextFileW;
        private void WindowClose()
        {
            if (ManualAttendanceUploadFromTextFileW != null)
                ManualAttendanceUploadFromTextFileW.Close();
            if (AttendanceDeviceMasterW != null)
                AttendanceDeviceMasterW.Close();
            if (EmployeeAttandanceW != null)
                EmployeeAttandanceW.Close();
            if (LieuLeaveW != null)
                LieuLeaveW.Close();
            if (ShiftDetailW != null)
                ShiftDetailW.Close();
            if (ManualAttendanceW != null)
                ManualAttendanceW.Close();
            if (MainAttendanceProcessW != null)
                MainAttendanceProcessW.Close();
            if (RosterDetailW != null)
                RosterDetailW.Close();
            if (RosterGroupW != null)
                RosterGroupW.Close();
            if (ShiftCatergoryMasterW != null)
                ShiftCatergoryMasterW.Close();
            if (DataDownloadW != null)
                DataDownloadW.Close();
            if (ManualEmployeeAttendanceW != null)
                ManualEmployeeAttendanceW.Close();
            if (MaxOtApprovedDaysW != null)
                MaxOtApprovedDaysW.Close();
        }


        public Attendencebutton(Attendance.AttendenceUserControl attUserControl)
        {
            InitializeComponent();
            MDIWrip = attUserControl.Mdi;
        }

        private void Hr_Checkbox_two_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Hr_Checkbox_two_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                MDIWrip.Children.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void Shift_Details_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            ShiftDetailW = new ShiftDetailWindow();
            ShiftDetailW.Show();
        }

        private void Employee_Attendence1_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            EmployeeAttandanceW = new EmployeeAttandanceWindow();
            EmployeeAttandanceW.Show();
        }

        private void manual_attendance_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            ManualAttendanceW = new ManualAttendanceWindow();
            ManualAttendanceW.Show();
        }

        private void Attendence_device_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            AttendanceDeviceMasterW = new AttendanceDeviceMasterWindow();
            AttendanceDeviceMasterW.Show();
        }

        private void Data_Download_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            DataDownloadW = new DataDownloadWindow();
            DataDownloadW.Show();
        }

        private void Daily_attendance_process_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            DailyAttendanceProcessW = new DailyAttendanceProcessWindow();
            DailyAttendanceProcessW.Show();
        }

        private void attendanceProcess_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            MainAttendanceProcessW = new MainAttendanceProcessWindow();
            MainAttendanceProcessW.Show();
        }

        private void attendance_textfile_upload_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            ManualAttendanceUploadFromTextFileW = new ManualAttendanceUploadFromTextFileWindow();
            ManualAttendanceUploadFromTextFileW.Show();
        }

        private void attendance_data_migration_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            AttendanceDataUploadW = new AttendanceDataUploadWindow();
            AttendanceDataUploadW.Show();
        }

        private void roster_group_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            RosterGroupW = new RosterGroupWindow();
            RosterGroupW.Show();
        }

        private void roster_details1_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            RosterDetailW = new RosterDetailWindow();
            RosterDetailW.Show();
        }

        private void employee_roster_detail_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            NewRosterCalenderWindow Emp_Roster = new NewRosterCalenderWindow();
            Emp_Roster.ShowDialog();
        }

        private void manual_attendance_upload_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            ManualEmployeeAttendanceW = new ManualEmployeeAttendanceWindow();
            ManualEmployeeAttendanceW.Show();
        }

        private void MaxOT_Approved_Days_btn_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            MaxOtApprovedDaysW = new MaxOtApprovedDaysWindow();
            MaxOtApprovedDaysW.Show();
        }

        private void modify_attendance_Copy_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            LieuLeaveW = new LieuLeaveWindow();
            LieuLeaveW.Show();
        }

        private void Shift_Category_Click(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            WindowClose();
            ShiftCatergoryMasterW = new ShiftCatergoryMasterWindow();
            ShiftCatergoryMasterW.Show();
        }

        private void Attendance_ot_approve_level_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
