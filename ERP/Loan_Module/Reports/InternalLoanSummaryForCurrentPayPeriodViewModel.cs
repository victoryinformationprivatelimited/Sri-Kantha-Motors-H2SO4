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
    class InternalLoanSummaryForCurrentPayPeriodViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<z_Loan> AllLoans;

        #endregion

        #region Constructor
        public InternalLoanSummaryForCurrentPayPeriodViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllLoans = new List<z_Loan>();
            LBLEmpDetails = Visibility.Hidden;
            LBLvisibleEmpGuaranteedLoan = Visibility.Hidden;
            RBSummaryDetail = Visibility.Hidden;
            LBLVisibleEmpPRejectedLoan = Visibility.Hidden;
            LBLVisibleEmpPendingLoan = Visibility.Hidden;
            LBLEmployeeDetails = Visibility.Hidden;
            LBLvisibleLoanWiseSummaryForExt = Visibility.Hidden;
            LBLEmpDetailsForExt = Visibility.Hidden;
            RefreshLoanCatergories();
            RefreshLoans();
            RefreshPeriods();
        }

        #endregion

        #region Properties

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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod");}
        }

        private IEnumerable<z_LoanCatergories> _LoanCategories;

        public IEnumerable<z_LoanCatergories> LoanCategories
        {
            get { return _LoanCategories; }
            set { _LoanCategories = value; OnPropertyChanged("LoanCategories"); }
        }
        private z_LoanCatergories _CurrentLoanCategory;

        public z_LoanCatergories CurrentLoanCategory
        {
            get { return _CurrentLoanCategory; }
            set { _CurrentLoanCategory = value; OnPropertyChanged("CurrentLoanCategory"); if (CurrentLoanCategory != null)FilterLoansByCat();}
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

        private Visibility _LBLEmpDetails;

        public Visibility LBLEmpDetails
        {
            get { return _LBLEmpDetails; }
            set { _LBLEmpDetails = value; OnPropertyChanged("LBLEmpDetails"); }
        }
        private Visibility _LBLvisibleEmpGuaranteedLoan;

        public Visibility LBLvisibleEmpGuaranteedLoan
        {
            get { return _LBLvisibleEmpGuaranteedLoan; }
            set { _LBLvisibleEmpGuaranteedLoan = value; OnPropertyChanged("LBLvisibleEmpGuaranteedLoan"); }
        }

        private Visibility _RBSummaryDetail;
        public Visibility RBSummaryDetail
        {
            get { return _RBSummaryDetail; }
            set { _RBSummaryDetail = value; OnPropertyChanged("RBSummaryDetail"); }
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

        #region RefreshMethods
        private void RefreshPeriods()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result;
            };
            serviceClient.GetPeriodsAsync();
        }

        private void RefreshLoanCatergories()
        {
            serviceClient.GetLoanCatergoriesCompleted += (s, e) =>
                {
                    LoanCategories = e.Result;
                };
            serviceClient.GetLoanCatergoriesAsync();
        }

        private void RefreshLoans()
        {
            serviceClient.GetLoansCompleted += (s, e) =>
                {
                    if(e.Result !=null && e.Result.Count()> 0)
                    AllLoans = e.Result.ToList();
                };
            serviceClient.GetLoansAsync();
        }
        
        #endregion

        #region Button Commands

        public ICommand ViewReportButton
        {
            get { return new RelayCommand(ViewReport); }
        }

               
        #endregion

        #region Methods

        private void FilterLoansByCat()
        {
            Loan = AllLoans.Where(c => c.loan_Catergory_id == CurrentLoanCategory.loan_Catergory_id);
        }

        private void ViewReport()
        {
            string path = "";
            try
            {

                path = "\\Loan_Module\\Reports\\InternalLoanSummaryForCurrentPayPeriod";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@LoanID", CurrentLoan == null ? string.Empty : CurrentLoan.loan_id.ToString());
                print.setParameterValue("@PeriodID", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
                print.LoadToReportViewer();

            }
            catch (Exception ex)
            {
                clsMessages.setMessage("Report loading is failed: " + ex.Message);
            }
        }
        #endregion
    }
}
