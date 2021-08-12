using ERP.HelperClass;
using ERP.Loan_Module.Reports;
using ERP.Reports;
using ERP.Reports.Documents.Attendance;
using ERP.Reports.Documents.Attendance.New_Attendance;
using ERP.Reports.Documents.HR_Report;
using ERP.Reports.Documents.HR_Report.User_Controls;
using ERP.Reports.Documents.Leave.LeaveUserControl;
using ERP.Reports.Documents.Payroll;
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

namespace ERP.Report_ui_UserControlers
{
    /// <summary>
    /// Interaction logic for Report_HR_btn.xaml
    /// </summary>
    public partial class Report_HR_btn : UserControl
    {

        #region Forms
        AppoinmentLetterNewWindow AppoinmentLetter;
        ConfirmationLetterWindow ConfirmationLetter;
        EmployeeUniformOfferingWindow EmployeeUniformOffering;
        SalaryConfirmationLetterWindow SalaryConfirmationLetter;
        EmployeeUniformWindow EmployeeUniformW;
        EmployeeUniformOrderWindow EmployeeUniformOrder;
       
        #endregion

        WrapPanel MDIWrip = new WrapPanel();
        public Report_HR_btn(ReportControllManager rcm)
        {
            InitializeComponent();
            MDIWrip = rcm.MDIRptCatagory;
            Hr_Checkbox_two.IsChecked = true;

        }

        private void Attendence_Check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1301))
            {
                Hr_Checkbox_two.IsChecked = false;
                AttendanceReportsButtonUserControl attendance = new AttendanceReportsButtonUserControl();
                MDIWrip.Children.Clear();
                MDIWrip.Children.Add(attendance); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Attendence_Check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1301))
            {
                Hr_Checkbox_two.IsChecked = false;
                AttendanceReportsButtonUserControl attendance = new AttendanceReportsButtonUserControl();
                MDIWrip.Children.Clear();
                MDIWrip.Children.Add(attendance);
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Leave_check_box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1302))
            {
                Hr_Checkbox_two.IsChecked = false;
                LeaveReportControlUseControl lm = new LeaveReportControlUseControl();
                MDIWrip.Children.Clear();
                MDIWrip.Children.Add(lm); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Leave_check_box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1302))
            {
                Hr_Checkbox_two.IsChecked = false;
                LeaveReportControlUseControl lm = new LeaveReportControlUseControl();
                MDIWrip.Children.Clear();
                MDIWrip.Children.Add(lm);
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Payroll_Check_Box_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1303))
            {
                Hr_Checkbox_two.IsChecked = false;
                PayrollReportsOnlyUserControl prc = new PayrollReportsOnlyUserControl();
                MDIWrip.Children.Add(prc); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }

        }

        private void Payroll_Check_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            PayrollReportsOnlyUserControl prc = new PayrollReportsOnlyUserControl();
            MDIWrip.Children.Add(prc);
        }

        private void Permoance_Check_Box_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Permoance_Check_Box_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Assests_Check_Box_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Assests_Check_Box_Unchecked(object sender, RoutedEventArgs e)
        {

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

        private void Employee_Check_Box_Copy_Unchecked(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            HrReportControl dt = new HrReportControl();
            MDIWrip.Children.Add(dt);
        }

        private void Employee_Check_Box_Copy_Checked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1304))
            {
                Hr_Checkbox_two.IsChecked = false;
                HrReportControl drc = new HrReportControl();
                MDIWrip.Children.Add(drc); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void HR_Report_Check_Box_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            AppoinmentLetter = new AppoinmentLetterNewWindow();
            AppoinmentLetter.Show();
        }

        private void HR_Report_Check_Box_Checked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            AppoinmentLetter = new AppoinmentLetterNewWindow();
            AppoinmentLetter.Show();
        }

        private void ConfirmLetter_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            ConfirmationLetter = new ConfirmationLetterWindow();
            ConfirmationLetter.Show();
        }

        private void ConfirmLetter_Checked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            ConfirmationLetter = new ConfirmationLetterWindow();
            ConfirmationLetter.Show();
        }

        private void SalaryConfirmation_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            SalaryConfirmationLetter = new SalaryConfirmationLetterWindow();
            SalaryConfirmationLetter.Show();
            //SalaryConfirmationLetterUserControl frm = new SalaryConfirmationLetterUserControl();
            //MDIWrip.Children.Add(frm);

        }

        private void SalaryConfirmation_Checked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            SalaryConfirmationLetter = new SalaryConfirmationLetterWindow();
            SalaryConfirmationLetter.Show();

        }

        private void EmployeeUniform_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            EmployeeUniformW = new EmployeeUniformWindow();
            EmployeeUniformW.Show();
            //EmployeeUniformUserControl frm = new EmployeeUniformUserControl();
            //MDIWrip.Children.Add(frm);
        }

        private void EmployeeUniform_Checked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            EmployeeUniformW = new EmployeeUniformWindow();
            EmployeeUniformW.Show();
        }

        private void EmployeeUniform_Order_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            EmployeeUniformOrder = new EmployeeUniformOrderWindow();
            EmployeeUniformOrder.Show();
        }

        private void EmployeeUniform_Order_Checked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            EmployeeUniformOrder = new EmployeeUniformOrderWindow();
            EmployeeUniformOrder.Show();
            //EmployeeUniformOrderUserControl frm = new EmployeeUniformOrderUserControl();
            //MDIWrip.Children.Add(frm);
        }

        private void EmployeeUniform_Order_Copy_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            EmployeeUniformOffering = new EmployeeUniformOfferingWindow();
            EmployeeUniformOffering.Show();
        }

        private void EmployeeUniform_Order_Copy_Checked_1(object sender, RoutedEventArgs e)
        {
            Hr_Checkbox_two.IsChecked = false;
            FormClose();
            EmployeeUniformOffering = new EmployeeUniformOfferingWindow();
            EmployeeUniformOffering.Show();
        }

        bool FormClose()
        {
            try
            {
                if (AppoinmentLetter != null)
                    AppoinmentLetter.Close();
                if (ConfirmationLetter != null)
                    ConfirmationLetter.Close();
                if (EmployeeUniformOffering != null)
                    EmployeeUniformOffering.Close();
                if (SalaryConfirmationLetter != null)
                    SalaryConfirmationLetter.Close();
                if (EmployeeUniformW != null)
                    EmployeeUniformW.Close();
                if (EmployeeUniformOrder != null)
                    EmployeeUniformOrder.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private void EmployeeUniform_Order_Unloaded(object sender, RoutedEventArgs e)
        {
            FormClose();
        }

        private void HR_Check_Box_Checked_1(object sender, RoutedEventArgs e)
        {
          //  Hr_Checkbox_two.IsChecked = true;

            if (clsSecurity.GetViewPermission(1305))
            {
                MDIWrip.Children.Clear();
                Hr_Checkbox_two.IsChecked = false;
                HR_ReportsUserControl dt = new HR_ReportsUserControl();
                MDIWrip.Children.Add(dt); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
            
        }

        private void HR_Check_Box_Unchecked_1(object sender, RoutedEventArgs e)
        {
          //  Hr_Checkbox_two.IsChecked = true;
            
            MDIWrip.Children.Clear();
            Hr_Checkbox_two.IsChecked = false;

            HR_ReportsUserControl dt = new HR_ReportsUserControl();
            MDIWrip.Children.Add(dt);
        }

        private void HR_Check_Box_Copy_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void HR_Check_Box_Copy_Unchecked_1(object sender, RoutedEventArgs e)
        {

        }

        private void Loan_Check_Box_Unchecked(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetViewPermission(1306))
            {
                Hr_Checkbox_two.IsChecked = false;
                LoanReportsUserControl Loan = new LoanReportsUserControl();
                MDIWrip.Children.Clear();
                MDIWrip.Children.Add(Loan); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

        private void Loan_Check_Box_Checked(object sender, RoutedEventArgs e)
        {

            if (clsSecurity.GetViewPermission(1306))
            {
                Hr_Checkbox_two.IsChecked = false;
                LoanReportsUserControl Loan = new LoanReportsUserControl();
                MDIWrip.Children.Clear();
                MDIWrip.Children.Add(Loan);  
            }
            else
            {
                clsMessages.setMessage("You don't have permission to view this form");
            }
        }

    }
}