using ERP.Attendance.Attendance_Process;
using ERP.Leave;
using ERP.Leave.Holiday_Detail;
using ERP.Masters;
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
    /// Interaction logic for LeaveButton.xaml
    /// </summary>
    public partial class LeaveButton : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();
        BulkLeaveApplyWindow BulkApplyW;
        HolidayWindow HolidayW;
        ApplyLeaveWindow ApplyLeaveW;
        ApproveLeavesWindow ApproveW;
        EmployeeLeaveDetailWindow EmployeeLeaveDetailW;
        LeaveCatagoryWindow LeaveCategoryW;
        LeavePeriodMasterWindow LeavePeriodW;
        LeaveAmountMasterWindow LeaveAmountW;
        LeaveDetailMasterWindow LeaveMasterW;
        ModifyLeavesUserControlWindow ModifyLeavesUserControlW;
        MigrateLeavesWindow MigrateLeavesW;
        ExecutiveOptionWindow ExecutiveOptionW;
        ExecutivePaymentProcess ExecutivePaymentProcessW;
        ApplyBulkLeaveWindow ApplyBulkLeaveW;
        private void CloseWindow()
        {
            if (ModifyLeavesUserControlW != null)
                ModifyLeavesUserControlW.Close();
            if (BulkApplyW != null)
                BulkApplyW.Close();
            if (HolidayW != null)
                HolidayW.Close();
            if (ApplyLeaveW != null)
                ApplyLeaveW.Close();
            if (ApproveW != null)
                ApproveW.Close();
            if (EmployeeLeaveDetailW != null)
                EmployeeLeaveDetailW.Close();
            if (LeaveCategoryW != null)
                LeaveCategoryW.Close();
            if (LeavePeriodW != null)
                LeavePeriodW.Close();
            if (LeaveAmountW != null)
                LeaveAmountW.Close();
            if (MigrateLeavesW != null)
                MigrateLeavesW.Close();
            if (ExecutiveOptionW != null)
                ExecutiveOptionW.Close();
            if (ExecutivePaymentProcessW != null)
                ExecutivePaymentProcessW.Close();
            if (ApplyBulkLeaveW != null)
                ApplyBulkLeaveW.Close();
        }

        public LeaveButton(Leave.LeaveUserControl leaveUserControl)
        {
            InitializeComponent();
            MDIWrip = leaveUserControl.Mdi;
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
        
        private void Leave_Amount_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(401))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                LeaveAmountW = new LeaveAmountMasterWindow();
                LeaveAmountW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Leave_Period_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(402))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                LeavePeriodW = new LeavePeriodMasterWindow();
                LeavePeriodW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Leave_Category1_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(403))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                LeaveCategoryW = new LeaveCatagoryWindow();
                LeaveCategoryW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Leave_Details1_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(404))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                LeaveMasterW = new LeaveDetailMasterWindow();
                LeaveMasterW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Employee_Leave_details_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(405))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                EmployeeLeaveDetailW = new EmployeeLeaveDetailWindow();
                EmployeeLeaveDetailW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }
        
        private void Leave_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(406))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                ApplyLeaveW = new ApplyLeaveWindow();
                ApplyLeaveW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Leave_Approve_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(407))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                ApproveW = new ApproveLeavesWindow();
                ApproveW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void holiday_detail_btn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(408))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                HolidayW = new HolidayWindow();
                HolidayW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void bulk_leave_apply_btn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(409))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                BulkApplyW = new BulkLeaveApplyWindow();
                BulkApplyW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Modify_Approved_Leaves_btn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(410))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                ModifyLeavesUserControlW = new ModifyLeavesUserControlWindow();
                ModifyLeavesUserControlW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Migrate_Leaves_Click_1(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(411))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                MigrateLeavesW = new MigrateLeavesWindow();
                MigrateLeavesW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Executive_Leiu_Leave_Click_1(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(412))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                ExecutiveOptionW = new ExecutiveOptionWindow();
                ExecutiveOptionW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Executive_Pay_Process_Click_1(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(413))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                ExecutivePaymentProcessW = new ExecutivePaymentProcess();
                ExecutivePaymentProcessW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Leave_Bulk_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(414))
            {
                Hr_Checkbox_two.IsChecked = false;
                CloseWindow();
                ApplyBulkLeaveW = new ApplyBulkLeaveWindow();
                ApplyBulkLeaveW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

    }
}
