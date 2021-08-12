using ERP.AdditionalWindows;
using ERP.BasicSearch;
using ERP.ERPService;
using ERP.HelperClass;
using ERP.Loan_Module.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.Basic_Masters
{
    class LoanPaymentViewModel : ViewModelBase
    {
        #region List
        List<AprrovedLoanPaymentView> Loan = new List<AprrovedLoanPaymentView>();
        #endregion

        #region Service Object
        private ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public LoanPaymentViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }
        #endregion

        #region Properties
        private IEnumerable<AprrovedLoanPaymentView> _AprrovedLoanPayment;
        public IEnumerable<AprrovedLoanPaymentView> AprrovedLoanPayment
        {
            get { return _AprrovedLoanPayment; }
            set { _AprrovedLoanPayment = value; OnPropertyChanged("AprrovedLoanPayment"); }
        }

        private AprrovedLoanPaymentView _CurrentAprrovedLoanPayment;
        public AprrovedLoanPaymentView CurrentAprrovedLoanPayment
        {
            get { return _CurrentAprrovedLoanPayment; }
            set { _CurrentAprrovedLoanPayment = value; OnPropertyChanged("CurrentAprrovedLoanPayment"); }
        }

        private EmployeeSearchView _EmployeeSearch;
        public EmployeeSearchView EmployeeSearch
        {
            get { return _EmployeeSearch; }
            set { _EmployeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        #endregion

        #region Refresh Method
        void refreshAprrovedLoanPayment()
        {
            serviceClient.GetAprrovedLoanPaymentViewCompleted += (s, e) =>
            {
                this.AprrovedLoanPayment = e.Result;
                if (AprrovedLoanPayment != null)
                    Loan = AprrovedLoanPayment.ToList();
            };
            serviceClient.GetAprrovedLoanPaymentViewAsync();
        }

        void refreshFilter()
        {
            AprrovedLoanPayment = null;
            AprrovedLoanPayment = Loan;
            AprrovedLoanPayment = AprrovedLoanPayment.Where(r => r.employee_id == EmployeeSearch.employee_id);
        }
        #endregion

        #region Methods
        #region Search
        void search()
        {
            try
            {
                EmployeeSearchWindow Window = new EmployeeSearchWindow();
                Window.ShowDialog();
                EmployeeSearch = Window.viewModel.CurrentEmployeeSearchView;
                if (EmployeeSearch != null)
                    refreshFilter();
                Window.Close();
            }
            catch (Exception)
            {
                
            }
        }
        #endregion

        #region New
        void New()
        {
            refreshAprrovedLoanPayment();
            CurrentAprrovedLoanPayment = null;
            CurrentAprrovedLoanPayment = new AprrovedLoanPaymentView();
        }
        #endregion

        #region Payment
        void Payment()
        {
            if (clsSecurity.GetSavePermission(606))
            {
                if (CurrentAprrovedLoanPayment != null && CurrentAprrovedLoanPayment.approved_loan_id != Guid.Empty)
                {
                    PaymentWindow window = new PaymentWindow(CurrentAprrovedLoanPayment);
                    window.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please Select a Loan", "Question", MessageBoxButton.OK, MessageBoxImage.Question);
                }
            }
            else
                clsMessages.setMessage("You don't have Permission to this button");
        } 
        #endregion
        #endregion

        #region Buttons
        public ICommand ReportButton 
        {
            get { return new RelayCommand(ReportPrint); }
        }
        void ReportPrint() 
        {
            if (clsSecurity.GetSavePermission(606))
            {
                ApprovedAndRejectUserControl userControl = new ApprovedAndRejectUserControl();
                UserControlWindow Uc = new UserControlWindow(userControl);
                Uc.Height = userControl.Height;
                Uc.Width = userControl.Width;
                Uc.Show();
            }
            else
                clsMessages.setMessage("You Don't have Permission to View This Report...");
        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        public ICommand SearchButton
        {
            get { return new RelayCommand(search); }
        }
        public ICommand PaymentButton
        {
            get { return new RelayCommand(Payment); }
        }
        #endregion

    }
}
