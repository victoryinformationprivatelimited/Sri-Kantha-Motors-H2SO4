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

namespace ERP.Reports.Documents.Leave.LeaveUserControl
{
    public partial class LeaveReportControlUseControl : UserControl
    {

        #region Member Windows
  
        LeaveRegisterReport leaveRegister;
        DailyLeaveWindow dailyLeaveReportWindow;
        DailyApprovedLeaveWindow dailyApprovedLeavesReportWindow;
        DailyPandingLeaveWindow dailyPendingLeaveReportWindow;
        DailyRejectedWindow dailyRejectedLeaveReportWindow;
        LeaveEntitlementWindow leaveEntitleReportWindow;
        LeaveSummaryReportWindow leaveSummaryWindow;

        #endregion

        #region Constructor

        public LeaveReportControlUseControl()
        {
            InitializeComponent();
        } 

        #endregion

        #region Window Closing Methods

        void formClose()
        {
            if (dailyLeaveReportWindow != null)
                dailyLeaveReportWindow.Close();
            if (dailyApprovedLeavesReportWindow != null)
                dailyApprovedLeavesReportWindow.Close();
            if (dailyPendingLeaveReportWindow != null)
                dailyPendingLeaveReportWindow.Close();
            if (dailyRejectedLeaveReportWindow != null)
                dailyRejectedLeaveReportWindow.Close();
            if (leaveEntitleReportWindow != null)
                leaveEntitleReportWindow.Close();
            if (leaveSummaryWindow != null)
                leaveSummaryWindow.Close();

            if (leaveRegister != null)
                leaveRegister.Close();
        }

        #endregion

        #region Leave Reports Buttons Click Event Handlers

        private void daily_leave_report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyLeaveReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyLeaveReportWindow = new DailyLeaveWindow();
                dailyLeaveReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void daily_approved_leave_report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyApprovedReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyApprovedLeavesReportWindow = new DailyApprovedLeaveWindow();
                dailyApprovedLeavesReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void daily_pending_leave_report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyPendingReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyPendingLeaveReportWindow = new DailyPandingLeaveWindow();
                dailyPendingLeaveReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void daily_rejected_leave_report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyRejectedReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                dailyRejectedLeaveReportWindow = new DailyRejectedWindow();
                dailyRejectedLeaveReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void entitlement_leave_report_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.DailyEntitlementReport), clsSecurity.loggedUser.user_id))
            {
                this.formClose();
                leaveEntitleReportWindow = new LeaveEntitlementWindow();
                leaveEntitleReportWindow.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void daily_leave_report_check_box_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.formClose();
            leaveSummaryWindow = new LeaveSummaryReportWindow();
            leaveSummaryWindow.Show();
        }

        private void daily_leave_report_check_box_Copy1_Click(object sender, RoutedEventArgs e)
        {

            formClose();
            leaveRegister = new LeaveRegisterReport();
            leaveRegister.Show();

        } 

        #endregion
    }
}
