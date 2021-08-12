using ERP.Reports.Documents.HR_Report.User_Controls;
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

namespace ERP.Reports.Documents.HR_Report
{
    public partial class HrLettersReportContorl : UserControl
    {
        #region Constructor

        public HrLettersReportContorl()
        {
            InitializeComponent();
        } 

        #endregion

        #region Forms

        AppointmentLetterWindow AppoinmentLetterNewW;
        //ConfirmationLetterWindow ConfirmationLetterW;
        ServiceLetterWindow ServiceLetterW;
        //SalaryConfirmationLetterWindow SalaryConfirmationLetterW;
        SalaryIncrementLetterWindow SalaryIncrementLetterW;
        EmployeeUniformOrderWindow EmployeeUniformOrderW;
        EmployeeUniformOfferingWindow EmployeeUniformOfferingW;
        EmployeeUniformWindow EmployeeUniformW;
        NopayLetterWindow NopayLetterW;
        WarningLetterWnidow WarningLetterW;
        LoanBondReportWindow LoanBondReportW;
        LoanCheckListReportWindow LoanCheckListReportW;

        #endregion

        #region Window Closing Methods

        void FormClose()
        {
            if (AppoinmentLetterNewW != null)
                AppoinmentLetterNewW.Close();
            //if (ConfirmationLetterW != null)
            //    ConfirmationLetterW.Close();
            //if (SalaryConfirmationLetterW != null)
            //    SalaryConfirmationLetterW.Close();
            if (SalaryIncrementLetterW != null)
                SalaryIncrementLetterW.Close();
            if (EmployeeUniformW != null)
                EmployeeUniformW.Close();
            if (EmployeeUniformOfferingW != null)
                EmployeeUniformOfferingW.Close();
            if (EmployeeUniformOrderW != null)
                EmployeeUniformOrderW.Close();
            if (WarningLetterW != null)
                WarningLetterW.Close();
            if (NopayLetterW != null)
                NopayLetterW.Close();
            if (ServiceLetterW != null)
                ServiceLetterW.Close();
            if (LoanBondReportW != null)
                LoanBondReportW.Close();
            if (LoanCheckListReportW != null)
                LoanCheckListReportW.Close();
        } 

        #endregion

        private void WrapPanel_Unloaded_1(object sender, RoutedEventArgs e)
        {
            FormClose();
        }

        #region HR Letter Buttons Click Event Handlers

        private void Appointment_Letter_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.AppointmentLetter), clsSecurity.loggedUser.user_id))
            {
                FormClose();
                //AppoinmentLetterNewW = new AppoinmentLetterNewWindow();
                AppoinmentLetterNewW = new AppointmentLetterWindow();
                DetailFilteringMDI.Children.Clear();
                AppoinmentLetterNewW.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Confirmation_Letter_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.ConfirmationLetter), clsSecurity.loggedUser.user_id))
            {
                FormClose();
                //ConfirmationLetterW = new ConfirmationLetterWindow();
                ServiceLetterW = new ServiceLetterWindow();
                DetailFilteringMDI.Children.Clear();
                ServiceLetterW.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Salary_Confirmation_Letter_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.SalaryConfirmationLetter), clsSecurity.loggedUser.user_id))
            {
                FormClose();
                //SalaryConfirmationLetterW = new SalaryConfirmationLetterWindow();
                SalaryIncrementLetterW = new SalaryIncrementLetterWindow();
                DetailFilteringMDI.Children.Clear();
                SalaryIncrementLetterW.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Add_Uniform_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.AddUniform), clsSecurity.loggedUser.user_id))
            {
                FormClose();
                LoanBondReportW = new LoanBondReportWindow();
                LoanBondReportW.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Uniform_Order_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.UniformOrder), clsSecurity.loggedUser.user_id))
            {
                FormClose();
                EmployeeUniformOrderW = new EmployeeUniformOrderWindow();
                DetailFilteringMDI.Children.Clear();
                EmployeeUniformOrderW.Show();

            }

            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Uniform_Offering_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.UniformOffering), clsSecurity.loggedUser.user_id))
            {
                FormClose();
                EmployeeUniformOfferingW = new EmployeeUniformOfferingWindow();
                DetailFilteringMDI.Children.Clear();
                EmployeeUniformOfferingW.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Warning_Letter_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.WarningLetter), clsSecurity.loggedUser.user_id))
            {
                FormClose();
                WarningLetterW = new WarningLetterWnidow();
                DetailFilteringMDI.Children.Clear();
                WarningLetterW.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void Nopay_Letter_check_box_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.NopayLetter), clsSecurity.loggedUser.user_id))
            {
                FormClose();
                NopayLetterW = new NopayLetterWindow();
                DetailFilteringMDI.Children.Clear();
                NopayLetterW.Show();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        } 

        #endregion

        private void loan_check_list_check_box_Click(object sender, RoutedEventArgs e)
        {
            FormClose();
            //SalaryConfirmationLetterW = new SalaryConfirmationLetterWindow();
            LoanCheckListReportW = new LoanCheckListReportWindow();
            DetailFilteringMDI.Children.Clear();
            LoanCheckListReportW.Show();
        }
    }
}
