using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Reports;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Loan_Module.Reports
{
    class EmployeeLoanDetailsReportViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public EmployeeLoanDetailsReportViewModel()
        {
            serviceClient = new ERPServiceClient();
            LBLVisibleEmpPendingLoan = Visibility.Hidden;
            LBLVisibleEmpPRejectedLoan = Visibility.Hidden;
            LBLvisibleEmpGuaranteedLoan = Visibility.Hidden;
            LBLvisibleLoanWiseSummary = Visibility.Hidden;
            LBLvisibleLoanWiseSummaryForExt = Visibility.Hidden;
            LBLEmpDetailsForExt = Visibility.Hidden;
            RefreshEmployees();
            RefreshLoan();
        }

        #endregion

        #region Properties

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployees;
        public EmployeeSumarryView CurrentEmployees
        {
            get { return _CurrentEmployees; }
            set { _CurrentEmployees = value; OnPropertyChanged("CurrentEmployees"); }
        }

        private EmployeeSearchView _CurrentEmployeesForDialogBox;
        public EmployeeSearchView CurrentEmployeesForDialogBox
        {
            get { return _CurrentEmployeesForDialogBox; }
            set { _CurrentEmployeesForDialogBox = value; OnPropertyChanged("CurrentEmployeesForDialogBox"); if (CurrentEmployeesForDialogBox != null)SetEmployeeDetails(); }
        }

        private IEnumerable<z_Loan> _Loan;
        public IEnumerable<z_Loan> Loan
        {
            get { return _Loan; }
            set { _Loan = value; OnPropertyChanged("Loan"); }
        }

        private z_Loan _CurrentLoan;
        public z_Loan CurrentLoan
        {
            get { return _CurrentLoan; }
            set { _CurrentLoan = value; OnPropertyChanged("CurrentLoan"); }
        }

        private Visibility _LBLVisibleEmpPendingLoan;
        public Visibility LBLVisibleEmpPendingLoan
        {
            get { return _LBLVisibleEmpPendingLoan; }
            set { _LBLVisibleEmpPendingLoan = value; OnPropertyChanged("LBLVisibleEmpPendingLoan"); }
        }

        private Visibility _LBLVisibleEmpPRejectedLoan;
        public Visibility LBLVisibleEmpPRejectedLoan
        {
            get { return _LBLVisibleEmpPRejectedLoan; }
            set { _LBLVisibleEmpPRejectedLoan = value; OnPropertyChanged("LBLVisibleEmpPRejectedLoan"); }
        }

        private Visibility _LBLvisibleEmpGuaranteedLoan;
        public Visibility LBLvisibleEmpGuaranteedLoan
        {
            get { return _LBLvisibleEmpGuaranteedLoan; }
            set { _LBLvisibleEmpGuaranteedLoan = value; OnPropertyChanged("LBLvisibleEmpGuaranteedLoan"); }
        }
        
        private bool _SummaryChk;
        public bool SummaryChk
        {
            get { return _SummaryChk; }
            set { _SummaryChk = value; OnPropertyChanged("SummaryChk"); }
        }

        private bool _DetailChk;
        public bool DetailChk
        {
            get { return _DetailChk; }
            set { _DetailChk = value; OnPropertyChanged("DetailChk"); }
        }

        private Visibility _LBLvisibleLoanWiseSummary;
        public Visibility LBLvisibleLoanWiseSummary
        {
            get { return _LBLvisibleLoanWiseSummary; }
            set { _LBLvisibleLoanWiseSummary = value; OnPropertyChanged("LBLvisibleLoanWiseSummary"); }
        }

        private Visibility _LBLvisibleLoanWiseSummaryForExt;

        public Visibility LBLvisibleLoanWiseSummaryForExt
        {
            get { return _LBLvisibleLoanWiseSummaryForExt; }
            set { _LBLvisibleLoanWiseSummaryForExt = value; OnPropertyChanged("LBLvisibleLoanWiseSummaryForExt"); }
        }

        private Visibility _LBLEmpDetailsForExt;

        public Visibility LBLEmpDetailsForExt
        {
            get { return _LBLEmpDetailsForExt; }
            set { _LBLEmpDetailsForExt = value; OnPropertyChanged("LBLEmpDetailsForExt"); }
        }
        
        #endregion

        #region Refresh Methods
        private void RefreshEmployees()
        {
            serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employees = e.Result;
            };
            serviceClient.GetAllEmployeeDetailAsync();
        }
        private void RefreshLoan()
        {
            serviceClient.GetLoansCompleted += (s, e) =>
            {
                Loan = e.Result;
            };
            serviceClient.GetLoansAsync();
        }

        #endregion

        #region Button Commands
        public ICommand ViewReportButton
        {
            get { return new RelayCommand(ViewReport); }
        }
        public ICommand ButtonSearchEmployee
        {
            get { return new RelayCommand(searchEmp); }
        }

        #endregion

        #region Methods

        #region SearchEmp Open
        void searchEmp()
        {
            EmployeeSearchWindow searchControl = new EmployeeSearchWindow();

            bool? b = searchControl.ShowDialog();
            if (b != null)
            {
                CurrentEmployeesForDialogBox = searchControl.viewModel.CurrentEmployeeSearchView;
            }
            searchControl.Close();
        }
        #endregion
        private void SetEmployeeDetails()
        {
            CurrentEmployees = Employees.FirstOrDefault(c => c.employee_id == CurrentEmployeesForDialogBox.employee_id);
        }
        private void ViewReport()
        {
            string path = "";
            try
            {

                if (SummaryChk != false || DetailChk != false)
                {
                    if (SummaryChk)
                    {
                        path = "\\Loan_Module\\Reports\\EmployeeWiseLoanDetails";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@EmployeeID", CurrentEmployees == null ? string.Empty : CurrentEmployees.employee_id.ToString());
                        print.setParameterValue("@LoanID", CurrentLoan == null ? string.Empty : CurrentLoan.loan_id.ToString());
                        print.LoadToReportViewer();
                    }
                    else if (DetailChk)
                    {
                        path = "\\Loan_Module\\Reports\\EmployeeInternalLoanDetailsFull";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@EmployeeID", CurrentEmployees == null ? string.Empty : CurrentEmployees.employee_id.ToString());
                        print.setParameterValue("@LoanID", CurrentLoan == null ? string.Empty : CurrentLoan.loan_id.ToString());
                        print.LoadToReportViewer();
                    }
                }
                else
                    clsMessages.setMessage("Please Select Detail Or Summary To View The Report...");

            }
            catch (Exception ex)
            {
                clsMessages.setMessage("Report loading is failed: " + ex.Message);
            }
        }

        #endregion
    }
}
