using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ERP.BasicSearch;
using ERP.AdditionalWindows;
using ERP.Loan_Module.Basic_Masters;
using ERP.Reports;
using ERP.HelperClass;

namespace ERP.Loan_Module.Detail_Masters
{
    class EmployeeLoansViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient;
        #endregion

        #region Lists
        List<EmployeeLoansView> listEmployeeLoanView = new List<EmployeeLoansView>();
        List<mas_Employee> listEmployee = new List<mas_Employee>();
        List<z_Loan> listLoan = new List<z_Loan>();
        #endregion

        #region Constructor
        public EmployeeLoansViewModel()
        {
            try
            {
                serviceClient = new ERPServiceClient();
                refreshEmployeeView();
                refreshLoans();
                LoanRateReadOnly = true;
                CurrentEmployeeView = new EmployeeLoansView();
                New();
            }
            catch (NullReferenceException)
            { }
        }
        #endregion

        #region Properties
        private String _TestSearch;
        public String TestSearch
        {
            get { return _TestSearch; }
            set { _TestSearch = value; OnPropertyChanged("TestSearch"); }
        }

        private string employeeName;
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; OnPropertyChanged("EmployeeName"); }
        }

        private EmployeeSearchView searchView;
        public EmployeeSearchView SearchView
        {
            get { return searchView; }
            set
            {
                searchView = value; OnPropertyChanged("SearchView");

                if (SearchView != null)
                {
                    CurrentEmployeeView.first_name = SearchView.first_name;
                    CurrentEmployeeView.employee_id = SearchView.employee_id;
                }

            }
        }

        private IEnumerable<z_Loan> loanValidation;
        public IEnumerable<z_Loan> LoanValidation
        {
            get { return loanValidation; }
            set { loanValidation = value; OnPropertyChanged("LoanValidation"); }
        }

        private z_Loan currentLoanValidation;
        public z_Loan CurrentLoanValidation
        {
            get { return currentLoanValidation; }
            set { currentLoanValidation = value; OnPropertyChanged("CurrentLoanValidation"); }
        }

        private mas_Employee currentSaveEmployee;
        public mas_Employee CurrentSaveEmployee
        {
            get { return currentSaveEmployee; }
            set { currentSaveEmployee = value; OnPropertyChanged("CurrentSaveEmployee"); }
        }

        private int searchIndex;
        public int SearchIndex
        {
            get { return searchIndex; }
            set { searchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private bool loanRateReadOnly;
        public bool LoanRateReadOnly
        {
            get { return loanRateReadOnly; }
            set { loanRateReadOnly = value; OnPropertyChanged("LoanRateReadOnly"); }
        }

        private bool? _AutoDeduct;
        public bool? AutoDeduct
        {
            get { return _AutoDeduct; }
            set { _AutoDeduct = value; OnPropertyChanged("AutoDeduct"); }
        }


        private bool? specialRate;
        public bool? SpecialRate
        {
            get { return specialRate; }
            set
            {
                specialRate = value; OnPropertyChanged("SpecialRate");
                if (CurrentEmployeeView != null)
                {

                    CurrentEmployeeView.is_special_rate = SpecialRate;
                    if (SpecialRate == true)
                        LoanRateReadOnly = false;
                    else
                        LoanRateReadOnly = true;
                }
            }
        }

        private IEnumerable<EmployeeLoansView> employeeLoansView;
        public IEnumerable<EmployeeLoansView> EmployeeLoansView
        {
            get { return employeeLoansView; }
            set { employeeLoansView = value; OnPropertyChanged("EmployeeLoansView"); }
        }

        private EmployeeLoansView currentEmployeeLoanView;
        public EmployeeLoansView CurrentEmployeeView
        {
            get { return currentEmployeeLoanView; }
            set
            {
                currentEmployeeLoanView = value; OnPropertyChanged("CurrentEmployeeView");
                if (CurrentEmployeeView != null)
                {
                    if (CurrentEmployeeView.is_auto_deduct != null)
                        AutoDeduct = (bool)CurrentEmployeeView.is_auto_deduct;
                    else
                        AutoDeduct = false;

                    if (CurrentEmployeeView.is_special_rate != null)
                        SpecialRate = (bool)CurrentEmployeeView.is_special_rate;
                    else
                        SpecialRate = false;
                }
            }
        }

        private IEnumerable<z_Loan> loans;
        public IEnumerable<z_Loan> Loans
        {
            get { return loans; }
            set
            {
                loans = value; OnPropertyChanged("Loans");



            }
        }

        private z_Loan currentLoan;
        public z_Loan CurrentLoan
        {
            get { return currentLoan; }
            set
            {
                currentLoan = value; OnPropertyChanged("CurrentLoan");
                if (CurrentLoan != null)
                {
                    CurrentEmployeeView.rate = CurrentLoan.default_rate;
                    CurrentEmployeeView.loan_id = CurrentLoan.loan_id;
                }
            }
        }
        #endregion

        #region Refresh Methods
        public void refereshSaveEmployee(Guid Gid)
        {
            try
            {
                CurrentSaveEmployee = serviceClient.GetSaveEmployee(Gid).FirstOrDefault();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void refreshEmployeeView()
        {
            try
            {
                listEmployeeLoanView.Clear();
                serviceClient.GetEmployeeLoansViewCompleted += (s, e) =>
                    {
                        EmployeeLoansView = e.Result;
                        if (EmployeeLoansView != null)
                            listEmployeeLoanView = EmployeeLoansView.ToList();
                    };
                serviceClient.GetEmployeeLoansViewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refreshLoans()
        {
            serviceClient.GetLoansCompleted += (s, e) =>
            {
                Loans = e.Result;
                listLoan = Loans.ToList();
            };
            serviceClient.GetLoansAsync();
        }
        #endregion

        #region New Method
        public void New()
        {

            try
            {
                refreshEmployeeView();
                CurrentEmployeeView = null;
                CurrentEmployeeView = new EmployeeLoansView();
                CurrentEmployeeView.employee_loan_id = Guid.NewGuid();
                refreshLoans();
                //refreshEmployees();

            }
            catch (NullReferenceException)
            { }
        }
        #endregion

        #region New Button Class & Property
        bool newCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }
        #endregion

        #region Save Method
        public void Save()
        {


            #region Message Box
            refereshSaveEmployee((Guid)CurrentEmployeeView.employee_id);
            MessageBoxResult Result = new MessageBoxResult();
            Result = MessageBox.Show("Do you want to save this recode ?"
                + "\n" + "Employee ID :" + CurrentSaveEmployee.emp_id.ToString()
                + "\n" + "First Name :" + CurrentSaveEmployee.first_name.ToString()
                + "\n" + "Last Name :" + CurrentSaveEmployee.surname.ToString()
                + "\n" + "No Of Installment :" + CurrentEmployeeView.no_of_installment.ToString()
                + "\n" + "Complete Deduction :" + CurrentEmployeeView.monthly_installment_with_intrest.ToString() + "(per Month)"
                , "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            #endregion


            if (Result == MessageBoxResult.Yes)
            {
                try
                {
                    bool IsUpdate = false;
                    dtl_EmployeeLoans newEmployeeLoan = new dtl_EmployeeLoans();
                    if (CurrentEmployeeView.employee_loan_id == Guid.Empty)
                    {
                        CurrentEmployeeView.employee_loan_id = Guid.NewGuid();
                        newEmployeeLoan.employee_loan_id = CurrentEmployeeView.employee_loan_id;
                    }
                    else
                        newEmployeeLoan.employee_loan_id = CurrentEmployeeView.employee_loan_id;
                    newEmployeeLoan.employee_id = CurrentEmployeeView.employee_id;
                    newEmployeeLoan.loan_id = CurrentEmployeeView.loan_id;
                    newEmployeeLoan.loan_amount = CurrentEmployeeView.loan_amount;
                    newEmployeeLoan.loan_start_date = CurrentEmployeeView.loan_start_date;
                    newEmployeeLoan.loan_end_date = CurrentEmployeeView.loan_end_date;
                    newEmployeeLoan.is_special_rate = SpecialRate;
                    newEmployeeLoan.rate = CurrentEmployeeView.rate;
                    newEmployeeLoan.monthly_installment = CurrentEmployeeView.monthly_installment;
                    newEmployeeLoan.is_auto_deduct = AutoDeduct;
                    newEmployeeLoan.monthly_installment_with_intrest = CurrentEmployeeView.monthly_installment_with_intrest;
                    newEmployeeLoan.intrest_amount = CurrentEmployeeView.intrest_amount;
                    newEmployeeLoan.final_installment = CurrentEmployeeView.final_installment;
                    newEmployeeLoan.no_of_installment = CurrentEmployeeView.no_of_installment;
                    newEmployeeLoan.pending = true;
                    newEmployeeLoan.is_delete = false;
                    IEnumerable<dtl_EmployeeLoans> allEmployeeLoans = serviceClient.GetEmployeeLoans();

                    if (allEmployeeLoans != null)
                    {
                        foreach (var item in allEmployeeLoans)
                        {
                            if (item.employee_loan_id == CurrentEmployeeView.employee_loan_id)
                            {
                                IsUpdate = true;
                                break;
                            }
                        }
                    }
                    if (newEmployeeLoan != null && newEmployeeLoan.employee_loan_id != null)
                    {
                        if (IsUpdate)
                        {
                            newEmployeeLoan.modified_user_id = clsSecurity.loggedUser.user_id;
                            newEmployeeLoan.modified_datetime = System.DateTime.Now;

                            if (clsSecurity.GetUpdatePermission(603))
                            {
                                if (serviceClient.UpdateEmployeeLoans(newEmployeeLoan))
                                {
                                    MessageBox.Show("Record Update Successfully", "Loan Module Says", MessageBoxButton.OK, MessageBoxImage.None);
                                    clsMessages.setMessage(Properties.Resources.UpdateSucess);

                                    ReportPrint print = new ReportPrint("\\Reports\\Documents\\Loan_Report\\LoanRequest");
                                    print.setParameterValue("@EMP_LOAN_ID", newEmployeeLoan.employee_loan_id.ToString());
                                    print.setParameterValue("Signature", "Company");
                                    print.setParameterValue("CopyType", "Company Copy");
                                    print.PrintReportWithReportViewer();

                                    ReportPrint print1 = new ReportPrint("\\Reports\\Documents\\Loan_Report\\LoanRequest");
                                    print1.setParameterValue("@EMP_LOAN_ID", newEmployeeLoan.employee_loan_id.ToString());
                                    print1.setParameterValue("Signature", "Employee Signature");
                                    print1.setParameterValue("CopyType", "Employee Copy");
                                    print1.PrintReportWithReportViewer();

                                    refreshEmployeeView();
                                }
                                else
                                {
                                    MessageBox.Show("Record Update Failed", "Loan Module Says", MessageBoxButton.OK, MessageBoxImage.None);
                                    clsMessages.setMessage(Properties.Resources.UpdateFail);
                                }
                            }
                            else
                                clsMessages.setMessage("You don't have Permission to Update in this Form...");
                        }
                        else
                        {
                            newEmployeeLoan.save_user_id = clsSecurity.loggedUser.user_id;
                            newEmployeeLoan.save_datetime = System.DateTime.Now;

                            if (clsSecurity.GetSavePermission(603))
                            {
                                if (serviceClient.SaveEmployeeLoans(newEmployeeLoan))
                                {

                                    ReportPrint print = new ReportPrint("\\Reports\\Documents\\Loan_Report\\LoanRequest");
                                    print.setParameterValue("@EMP_LOAN_ID", newEmployeeLoan.employee_loan_id.ToString());
                                    print.setParameterValue("Signature", "Company");
                                    print.setParameterValue("CopyType", "Company Copy");
                                    print.PrintReportWithReportViewer();

                                    ReportPrint print1 = new ReportPrint("\\Reports\\Documents\\Loan_Report\\LoanRequest");
                                    print1.setParameterValue("@EMP_LOAN_ID", newEmployeeLoan.employee_loan_id.ToString());
                                    print1.setParameterValue("Signature", "Employee Signature");
                                    print1.setParameterValue("CopyType", "Employee Copy");
                                    print1.PrintReportWithReportViewer();

                                    MessageBox.Show("Record Save Successfully", "Loan Module Says", MessageBoxButton.OK, MessageBoxImage.None);
                                    clsMessages.setMessage(Properties.Resources.SaveSucess);
                                    //  print
                                    refreshEmployeeView();
                                    New();
                                }
                                else
                                {
                                    MessageBox.Show("Record SaveFailed", "Loan Module Says", MessageBoxButton.OK, MessageBoxImage.None);
                                    clsMessages.setMessage(Properties.Resources.SaveFail);
                                }
                            }
                            else
                                clsMessages.setMessage("You don't have Permission to Save in this Form...");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion

        #region Calculate Button

        public void CalculateMethod()
        {
            // refreshLoans();
            Calculation();
        }

        public bool calculateCanExecute()
        {
            try
            {

                return (CurrentEmployeeView.loan_amount == null
                    && CurrentEmployeeView.loan_id == null) ? false : true;
            }
            catch (NullReferenceException)
            {

                return false;
            }
        }

        public ICommand CalculateButton
        {
            get
            {
                return new RelayCommand(CalculateMethod, calculateCanExecute);
            }
        }
        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentEmployeeView == null)
                return false;
            if (CurrentEmployeeView.loan_id == null)
                return false;
            if (CurrentEmployeeView.rate == null)
                return false;
            if (CurrentEmployeeView.employee_id == null)
                return false;
            if (CurrentEmployeeView.monthly_installment_with_intrest == null)
                return false;
            return true;


        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }
        #endregion

        #region Delete Method

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }


        public void Delete()
        {
            try
            {
                MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Do you want to delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    CurrentEmployeeView.delete_user_id = clsSecurity.loggedUser.user_id;
                    CurrentEmployeeView.delete_datetime = System.DateTime.Now;
                    if (clsSecurity.GetDeletePermission(603))
                    {

                        if (serviceClient.DeleteEmployeeLoans(CurrentEmployeeView))
                        {
                            MessageBox.Show("Record Deleted");
                            clsMessages.setMessage(Properties.Resources.DeleteSucess);
                            refreshEmployeeView();
                        }
                        else
                        {
                            MessageBox.Show("Record Delete Fail...", "Loan Module Says", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            clsMessages.setMessage(Properties.Resources.DeleteFail);
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have Permission to Save in this Form...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region SearchEmp Open
        public ICommand SearchEmpButton
        {
            get { return new RelayCommand(searchEmp); }
        }
        void searchEmp()
        {
            EmployeeSearchWindow searchControl = new EmployeeSearchWindow();

            bool? b = searchControl.ShowDialog();
            if (b != null)
            {
                SearchView = searchControl.viewModel.CurrentEmployeeSearchView;
            }
            searchControl.Close();
        }
        #endregion

        #region Loan Load Button
        public ICommand LoanLoad
        {
            get { return new RelayCommand(loanLoad); }
        }
        void loanLoad()
        {
            LoansWindow Window = new LoansWindow();
            Window.ShowDialog();
            refreshLoans();
            // New();
        }
        #endregion

        #region Validation
        public bool Validation()
        {
            try
            {

                LoanValidation = null;
                LoanValidation = listLoan;

                if (CurrentEmployeeView.loan_start_date == null && CurrentEmployeeView.loan_end_date == null)
                {
                    MessageBox.Show("Please select date range!");
                    return false;
                }
                if (CurrentLoan == null)
                {
                    MessageBox.Show("Please Select the Loan Type Again !");
                    return false;
                }

                CurrentLoanValidation = LoanValidation.FirstOrDefault(c => c.loan_id == CurrentLoan.loan_id);

                if (CurrentEmployeeView.loan_amount > CurrentLoanValidation.maximum_amount && CurrentEmployeeView.loan_amount != CurrentLoanValidation.maximum_amount)
                {
                    MessageBox.Show("Loan Amount Should not be Grater Than Loan Maximum Amount.. " + CurrentLoanValidation.maximum_amount, "Loan Module Says.", MessageBoxButton.OK, MessageBoxImage.None);
                    return false;
                }
                if (CurrentEmployeeView.loan_amount < CurrentLoanValidation.minimum_amount && CurrentEmployeeView.loan_amount != CurrentLoanValidation.minimum_amount)
                {
                    MessageBox.Show("Loan Amount Should not be Less Than Loan Minimum Amount.. " + CurrentLoanValidation.minimum_amount, "Loan Module Says.", MessageBoxButton.OK, MessageBoxImage.None);
                    return false;
                }
                int month = ((DateTime)CurrentEmployeeView.loan_end_date).Year * 12
                    + ((DateTime)CurrentEmployeeView.loan_end_date).Month
                    - (((DateTime)CurrentEmployeeView.loan_start_date).Year * 12
                    + ((DateTime)CurrentEmployeeView.loan_start_date).Month);
                if (CurrentLoanValidation.minimum_time_duration_type == "M" && CurrentLoanValidation.minimum_time_duration_period > month && CurrentLoanValidation.minimum_time_duration_period != month)
                {
                    MessageBox.Show("Loan Time  Should not be Less Than Loan Minimum Time..(in months) " + CurrentLoanValidation.minimum_time_duration_period, "Loan Module Says.", MessageBoxButton.OK, MessageBoxImage.None);
                    return false;
                }

                if (CurrentLoanValidation.maximum_time_duration_type == "M" && CurrentLoanValidation.maximum_time_duration_period < month && CurrentLoanValidation.minimum_time_duration_period != month)
                {
                    MessageBox.Show("Loan Time Should not be Greater Than Loan Maximum Time.. (in months)" + CurrentLoanValidation.maximum_time_duration_period, "Loan Module Says.", MessageBoxButton.OK, MessageBoxImage.None);
                    return false;
                }
                if (CurrentLoanValidation.minimum_time_duration_type == "Y" && (CurrentLoanValidation.minimum_time_duration_period * 12) > month && (CurrentLoanValidation.minimum_time_duration_period * 12) == month)
                {
                    MessageBox.Show("Loan Time  Should not be Less Than Loan Minimum Time..(in years) " + CurrentLoanValidation.minimum_time_duration_period, "Loan Module Says.", MessageBoxButton.OK, MessageBoxImage.None);
                    return false;
                }
                if (CurrentLoanValidation.maximum_time_duration_type == "Y" && (CurrentLoanValidation.maximum_time_duration_period * 12) < month && (CurrentLoanValidation.maximum_time_duration_period * 12) == month)
                {
                    MessageBox.Show("Loan Time Should not be Greater Than Loan Maximum Time..(in years) " + CurrentLoanValidation.maximum_time_duration_period, "Loan Module Says.", MessageBoxButton.OK, MessageBoxImage.None);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Validation Method Error (" + ex.ToString() + ")");

                return false;
            }
        }
        #endregion

        #region Calculation
        public void Calculation()
        {
            if (Validation())
            {
                try
                {
                    if (CurrentEmployeeView.loan_end_date != null && CurrentEmployeeView.loan_start_date != null)
                    {
                        double loan_amount = (double)CurrentEmployeeView.loan_amount;
                        double monthly_installment = 0;
                        double intrest_amount = 0;
                        double total_monthly_installment = 0;

                        int months = ((DateTime)CurrentEmployeeView.loan_end_date).Year * 12 + ((DateTime)CurrentEmployeeView.loan_end_date).Month - (((DateTime)CurrentEmployeeView.loan_start_date).Year * 12 + ((DateTime)CurrentEmployeeView.loan_start_date).Month);
                        monthly_installment = Math.Round(loan_amount / months, 2);
                        intrest_amount = Math.Round((loan_amount / 12) * (double)CurrentEmployeeView.rate / 100, 2);
                        total_monthly_installment = Math.Round(monthly_installment + intrest_amount, 2);

                        CurrentEmployeeView.no_of_installment = months;
                        CurrentEmployeeView.intrest_amount = (decimal)intrest_amount;
                        CurrentEmployeeView.monthly_installment = (decimal)monthly_installment;
                        CurrentEmployeeView.monthly_installment_with_intrest = (decimal)total_monthly_installment;
                        CurrentEmployeeView.final_installment = (decimal)(total_monthly_installment * months);
                    }
                    else
                    {
                        MessageBox.Show("Please !Select Time Range");
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString() + "Calculation error");
                }
            }
        }
        #endregion

        #region Save
        void Search()
        {
            EmployeeSearchWindow Window = new EmployeeSearchWindow();
            Window.ShowDialog();
            if (Window.viewModel.CurrentEmployeeSearchView != null && Window.viewModel.CurrentEmployeeSearchView.employee_id != Guid.Empty)
                RefreshSearch((Guid)Window.viewModel.CurrentEmployeeSearchView.employee_id);
            Window.Close();
        }

        void RefreshSearch(Guid ID)
        {

            EmployeeLoansView = null;
            EmployeeLoansView = listEmployeeLoanView;
            EmployeeLoansView = EmployeeLoansView.Where(r => r.employee_id == ID);
        }

        public ICommand SearchButton
        {
            get { return new RelayCommand(Search); }
        }

        #endregion

        #region Reprint
        public ICommand ReprintButton
        {
            get { return new RelayCommand(Reprint); }
        }
        void Reprint()
        {
            ReportPrint print = new ReportPrint("\\Reports\\Documents\\Loan_Report\\LoanRequest");
            print.setParameterValue("@EMP_LOAN_ID", CurrentEmployeeView.employee_loan_id.ToString());
            print.setParameterValue("Signature", "Company");
            print.setParameterValue("CopyType", "Company Copy");
            print.PrintReportWithReportViewer();

            ReportPrint print1 = new ReportPrint("\\Reports\\Documents\\Loan_Report\\LoanRequest");
            print1.setParameterValue("@EMP_LOAN_ID", CurrentEmployeeView.employee_loan_id.ToString());
            print1.setParameterValue("Signature", "Employee Signature");
            print1.setParameterValue("CopyType", "Employee Copy");
            print1.PrintReportWithReportViewer();

        }
        #endregion

    }
}