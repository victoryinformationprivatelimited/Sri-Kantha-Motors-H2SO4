using ERP.Masters;
using ERP.MastersDetails;
using ERP.Utility;
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
    public partial class MasterButtons : UserControl
    {
        public WrapPanel MDIWrip = new WrapPanel();

        #region Window Members

        CityWindow cityMasterWindow;
        TownMasterWindow townMasterWindow;
        CompanyWindow companyMasterWindow;
        CompanyBranchesWindow companyBranchMasterWindow;
        DepartmentsWindow departmentMasterWindow;
        SectionsWindow sectionMasterWindow;
        DesignationWindow designationMasterWindow;
        EmployeeGradeWindow gradeMasterWindow;
        EmployeeWindow employeeMasterWindow;
        BankWindow bankMasterWindow;
        ERPBankBranchWindow bankBranchMasterWindow;
        BankMasterDetailWindow bankMasterDetailWindow;
        PaymentMethodWindow payMethodWindow;
        EmployeeDeductionPaymentWindow employeeDeductionPaymentWindow;
        LocationMasterWindow locationMasterWindow;
        PeriodWindow periodWindow;
        #endregion

        #region Constructor

        public MasterButtons(MasterUserControl masterusercontrol)
        {
            InitializeComponent();
            MDIWrip = masterusercontrol.Mdi;
        } 

        #endregion

        #region Window close methods

        void closeWindows()
        {
            if (cityMasterWindow != null)
                cityMasterWindow.Close();
            if (townMasterWindow != null)
                townMasterWindow.Close();
            if (companyMasterWindow != null)
                companyMasterWindow.Close();
            if (companyBranchMasterWindow != null)
                companyBranchMasterWindow.Close();
            if (departmentMasterWindow != null)
                departmentMasterWindow.Close();
            if (sectionMasterWindow != null)
                sectionMasterWindow.Close();
            if (designationMasterWindow != null)
                designationMasterWindow.Close();
            if (gradeMasterWindow != null)
                gradeMasterWindow.Close();
            if (employeeMasterWindow != null)
                employeeMasterWindow.Close();
            if (bankMasterWindow != null)
                bankMasterWindow.Close();
            if (bankBranchMasterWindow != null)
                bankBranchMasterWindow.Close();
            if (bankMasterDetailWindow != null)
                bankMasterDetailWindow.Close();
            if (payMethodWindow != null)
                payMethodWindow.Close();
            if (periodWindow != null)
                periodWindow.Close();
        } 

        #endregion

        #region Button Event Handlers

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

        private void City_Masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(201))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                cityMasterWindow = new CityWindow();
                cityMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Town_Masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(202))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                townMasterWindow = new TownMasterWindow();
                townMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Company_masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(203))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                companyMasterWindow = new CompanyWindow();
                companyMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Company_Branches_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(204))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                companyBranchMasterWindow = new CompanyBranchesWindow();
                companyBranchMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Departments_Masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(205))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                departmentMasterWindow = new DepartmentsWindow();
                departmentMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Section_Masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(206))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                sectionMasterWindow = new SectionsWindow();
                sectionMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Designation_masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(207))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                designationMasterWindow = new DesignationWindow();
                designationMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Grade_Mastesr_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(208))
            {
                Hr_Checkbox_two.IsChecked = false; ;
                this.closeWindows();
                gradeMasterWindow = new EmployeeGradeWindow();
                gradeMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Employee_Masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(209))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                employeeMasterWindow = new EmployeeWindow();
                employeeMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Bank_Masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(210))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                bankMasterWindow = new BankWindow();
                bankMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Bank_Branches_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(211))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                bankBranchMasterWindow = new ERPBankBranchWindow();
                bankBranchMasterWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Bank_Masters_Details_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(212))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                bankMasterDetailWindow = new BankMasterDetailWindow();
                bankMasterDetailWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Payments_Masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(213))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                payMethodWindow = new PaymentMethodWindow();
                payMethodWindow.Show(); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Payment_Deduction_Masters_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(214))
            {
                Hr_Checkbox_two.IsChecked = false;
                this.closeWindows();
                employeeDeductionPaymentWindow = new EmployeeDeductionPaymentWindow();
                employeeDeductionPaymentWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }
        #endregion

        private void Location_master_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(215))
            {

                this.closeWindows();
                locationMasterWindow = new LocationMasterWindow();
                locationMasterWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Period_master_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(216))
            {

                this.closeWindows();
                periodWindow = new PeriodWindow();
                periodWindow.Show();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }
    }
}
