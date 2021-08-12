using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.Reports
{
    class ActiveExternalBankLoanEmployeeWiseViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;

        #endregion

        #region Constrouctor
        public ActiveExternalBankLoanEmployeeWiseViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshPeriods();
            RefreshEmployees();
            LBLEmpDetails = Visibility.Hidden;
            LoanNameCMBLBL = Visibility.Hidden;
            RBSummaryDetail = Visibility.Hidden;
            LBLvisibleLoanWiseSummary = Visibility.Hidden;
            LBLvisibleEmpGuaranteedLoan = Visibility.Hidden;
            LBLVisibleEmpPRejectedLoan = Visibility.Hidden;
            LBLVisibleEmpPendingLoan = Visibility.Hidden;
            LBLEmployeeDetails = Visibility.Hidden;
            Bank = true;
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

        private IEnumerable<z_Period> _Periods;

        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;

        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeiod"); }
        }

        private bool _Bank;

        public bool Bank
        {
            get { return _Bank; }
            set { _Bank = value; OnPropertyChanged("Bank"); }
        }

        private bool _Cheque;

        public bool Cheque
        {
            get { return _Cheque; }
            set { _Cheque = value; OnPropertyChanged("Cheque"); }
        }

        private bool _BankWise;

        public bool BankWise
        {
            get { return _BankWise; }
            set { _BankWise = value; OnPropertyChanged("BankWise"); }
        }
        
        
        private Visibility _LBLEmpDetails;
        public Visibility LBLEmpDetails
        {
            get { return _LBLEmpDetails; }
            set { _LBLEmpDetails = value; OnPropertyChanged("LBLEmpDetails"); }
        }

        private Visibility _LoanNameCMBLBL;

        public Visibility LoanNameCMBLBL
        {
            get { return _LoanNameCMBLBL; }
            set { _LoanNameCMBLBL = value; OnPropertyChanged("LoanNameCMBLBL"); }
        }

        private Visibility _RBSummaryDetail;

        public Visibility RBSummaryDetail
        {
            get { return _RBSummaryDetail; }
            set { _RBSummaryDetail = value; OnPropertyChanged("RBSummaryDetail"); }
        }

        private Visibility _LBLvisibleLoanWiseSummary;

        public Visibility LBLvisibleLoanWiseSummary
        {
            get { return _LBLvisibleLoanWiseSummary; }
            set { _LBLvisibleLoanWiseSummary = value; OnPropertyChanged("LBLvisibleLoanWiseSummary"); }
        }

        private Visibility _LBLvisibleEmpGuaranteedLoan;

        public Visibility LBLvisibleEmpGuaranteedLoan
        {
            get { return _LBLvisibleEmpGuaranteedLoan; }
            set { _LBLvisibleEmpGuaranteedLoan = value; OnPropertyChanged("LBLvisibleEmpGuaranteedLoan"); }
        }

        private Visibility _LBLVisibleEmpPRejectedLoan;

        public Visibility LBLVisibleEmpPRejectedLoan
        {
            get { return _LBLVisibleEmpPRejectedLoan; }
            set { _LBLVisibleEmpPRejectedLoan = value; OnPropertyChanged("LBLVisibleEmpPRejectedLoan"); }
        }

        private Visibility _LBLVisibleEmpPendingLoan;

        public Visibility LBLVisibleEmpPendingLoan
        {
            get { return _LBLVisibleEmpPendingLoan; }
            set { _LBLVisibleEmpPendingLoan = value; OnPropertyChanged("LBLVisibleEmpPendingLoan"); }
        }

        private Visibility _LBLEmployeeDetails;

        public Visibility LBLEmployeeDetails
        {
            get { return _LBLEmployeeDetails; }
            set { _LBLEmployeeDetails = value; OnPropertyChanged("LBLEmployeeDetails"); }
        }
        
        #endregion

        #region refresh Methods
        
        private void RefreshPeriods()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result;
            };
            serviceClient.GetPeriodsAsync();
        }

        private void RefreshEmployees()
        {
            serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employees = e.Result;
            };
            serviceClient.GetAllEmployeeDetailAsync();
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
        private void SetEmployeeDetails()
        {
            CurrentEmployees = Employees.FirstOrDefault(c => c.employee_id == CurrentEmployeesForDialogBox.employee_id);
        }
        private void ViewReport()
        {
            string path = "";
            try
            {

                if (!BankWise)
                {
                    path = "\\Loan_Module\\Reports\\ActiveExternalLoanEmployeeWise";
                    ReportPrint print = new ReportPrint(path);
                    print.setParameterValue("@Employee", CurrentEmployees == null ? string.Empty : CurrentEmployees.employee_id.ToString());
                    print.setParameterValue("@PeriodID", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
                    print.setParameterValue("@Bank", Bank);
                    print.setParameterValue("@Cheque", Cheque);
                    print.LoadToReportViewer(); 
                }
                else
                {
                    path = "\\Loan_Module\\Reports\\ActiveExternalLoanBankWise";
                    ReportPrint print = new ReportPrint(path);
                    print.setParameterValue("@Employee", CurrentEmployees == null ? string.Empty : CurrentEmployees.employee_id.ToString());
                    print.setParameterValue("@PeriodID", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
                    print.setParameterValue("@Bank", true);
                    print.setParameterValue("@Cheque", false);
                    print.LoadToReportViewer();
                }

            }
            catch (Exception ex)
            {
                clsMessages.setMessage("Report loading is failed: " + ex.Message);
            }
        }
        private void searchEmp()
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


    }
}
