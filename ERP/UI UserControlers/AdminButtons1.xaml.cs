using ERP.Payroll.AdminReversing;
using ERP.Payroll.SendSMS;
using ERP.Payroll.ThakralDB;
using ERP.Security;
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
    /// Interaction logic for AdminButtons1.xaml
    /// </summary>
    public partial class AdminButtons1 : UserControl
    {
        WrapPanel MDIWrip = new WrapPanel();

        #region Window Members

        UserMasterWindow userMasterWindow;
        UserLevelMasterWindow userLevelMasterWindow;
        ViewModelUserPermissionWindow viewModelPermissionWindow;
        UserEmployeeWindow userEmployeeWindow;
        EmployeeManagerWindow empManagerWindow;
        OnlineLeaveUserWindow onlineUserWindow;
        EmployeeCorelatedWindow EmployeeCorelatedW;
        UserPermissionWindow UserPermissionW;
        SMSPaymentsWindow SMSPaymentsW;
        PayrollDataMigrateWindow PayrollDataMigrateW;
        AdminReversingWindow AdminReversingW;
        AdministratorUserControl administratorUserControl;
        Settings.SettingsWindows systemSettingsw;
        #endregion

        public AdminButtons1(AdministratorUserControl AdministratorUserControl)
        {
            InitializeComponent();
            administratorUserControl = AdministratorUserControl;
            MDIWrip = AdministratorUserControl.Mdi;
        }

        void closeForms()
        {
            if (userMasterWindow != null)
                userMasterWindow.Close();
            if (userLevelMasterWindow != null)
                userLevelMasterWindow.Close();
            if (viewModelPermissionWindow != null)
                viewModelPermissionWindow.Close();
            if (userEmployeeWindow != null)
                userEmployeeWindow.Close();
            if (empManagerWindow != null)
                empManagerWindow.Close();
            if (onlineUserWindow != null)
                onlineUserWindow.Close();
            if (EmployeeCorelatedW != null)
                EmployeeCorelatedW.Close();
            if (UserPermissionW != null)
                UserPermissionW.Close();
            if (SMSPaymentsW != null)
                SMSPaymentsW.Close();
            if (PayrollDataMigrateW != null)
                PayrollDataMigrateW.Close();
            if (AdminReversingW != null)
                AdminReversingW.Close();
            if (systemSettingsw != null)
                systemSettingsw.Visibility = Visibility.Hidden;
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
        
        private void Employee_Manager_Click(object sender, RoutedEventArgs e)
        {
            //MDIWrip.Children.Clear();
            if (clsSecurity.GetViewPermission(105))
            {
                Hr_Checkbox_two.IsChecked = false;
                //EmployeeManagerUserControl userEmployee = new EmployeeManagerUserControl();
                //MDIWrip.Children.Add(userEmployee);
                this.closeForms();
                empManagerWindow = new EmployeeManagerWindow();
                empManagerWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void User_Master_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(102))
            {
                Hr_Checkbox_two.IsChecked = false;
                //UserMasterUserControl1 usrmaster = new UserMasterUserControl1();
                //MDIWrip.Children.Add(usrmaster);
                this.closeForms();
                userMasterWindow = new UserMasterWindow();
                userMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void User_level_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(101))
            {
                Hr_Checkbox_two.IsChecked = false;
                //UserLevelMaster usrlevel = new UserLevelMaster();
                //MDIWrip.Children.Add(usrlevel);
                this.closeForms();
                userLevelMasterWindow = new UserLevelMasterWindow();
                userLevelMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void User_Permission_Click(object sender, RoutedEventArgs e)
        {
            //ViewModelUserPermission usrpermission = new ViewModelUserPermission();
            //MDIWrip.Children.Add(usrpermission);
            //viewModelPermissionWindow = new ViewModelUserPermissionWindow();
            //viewModelPermissionWindow.Show();
            if (clsSecurity.GetViewPermission(103))
            {
                Hr_Checkbox_two.IsChecked = false;
                closeForms();
                UserPermissionW = new UserPermissionWindow();
                UserPermissionW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void User_Employee_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(104))
            {
                Hr_Checkbox_two.IsChecked = false;
                //UserEmployeeUserControl userEmployee = new UserEmployeeUserControl();
                //MDIWrip.Children.Add(userEmployee);
                this.closeForms();
                userEmployeeWindow = new UserEmployeeWindow();
                userEmployeeWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void online_user_chkbtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(106))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeForms();
                onlineUserWindow = new OnlineLeaveUserWindow();
                onlineUserWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Employee_Corelate_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(107))
            {
                MDIWrip.Children.Clear();
                Hr_Checkbox_two.IsChecked = false;
                closeForms();
                EmployeeCorelatedW = new EmployeeCorelatedWindow();
                EmployeeCorelatedW.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Employee_SMS_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(108))
            {
                MDIWrip.Children.Clear();
                Hr_Checkbox_two.IsChecked = false;
                closeForms();
                SMSPaymentsW = new SMSPaymentsWindow();
                SMSPaymentsW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Salary_Details_Migrate_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(109))
            {
                MDIWrip.Children.Clear();
                Hr_Checkbox_two.IsChecked = false;
                closeForms();
                PayrollDataMigrateW = new PayrollDataMigrateWindow();
                PayrollDataMigrateW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Admin_Reverse_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(112))
            {
                MDIWrip.Children.Clear();
                Hr_Checkbox_two.IsChecked = false;
                closeForms();
                AdminReversingW = new AdminReversingWindow();
                AdminReversingW.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void SystemSettings_Click(object sender, RoutedEventArgs e)
        {
            administratorUserControl.Mdiwrappanel.Children.Clear();
            administratorUserControl.Mdiwrappanel.Children.Add(new Settings.SettingsWindows(administratorUserControl));
        }
    }
}
