using ERP.ERPService;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.Basic_Masters
{
    class PaymentViewModel : ViewModelBase
    {

        #region Service Object
        private ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public PaymentViewModel(AprrovedLoanPaymentView CurrentApprovedLoan)
        {
            this.CurrentApprovedLoan = CurrentApprovedLoan;
            serviceClient = new ERPServiceClient();
            New();

        }
        #endregion

        #region Properties

        private z_Period _CurrentPeriod;

        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }
        private IEnumerable<z_Period> _Period;

        public IEnumerable<z_Period> Period
        {
            get { return _Period; }
            set { _Period = value; OnPropertyChanged("Period"); }
        }



        private GETLASTPAYMENT_PROC_Result _GETLASTPAYMENT;
        public GETLASTPAYMENT_PROC_Result GETLASTPAYMENT
        {
            get { return _GETLASTPAYMENT; }
            set { _GETLASTPAYMENT = value; OnPropertyChanged("GETLASTPAYMENT"); }
        }


        private string _Completed;
        public string Completed
        {
            get { return _Completed; }
            set { _Completed = value; OnPropertyChanged("Completed"); }
        }

        private int _NoOfMonths;
        public int NoOfMonths
        {
            get { return _NoOfMonths; }
            set { _NoOfMonths = value; OnPropertyChanged("NoOfMonths"); }
        }

        private double _Payment;
        public double Payment
        {
            get { return _Payment; }
            set { _Payment = value; OnPropertyChanged("Payment"); }
        }

        private double _Balance;
        public double Balance
        {
            get { return _Balance; }
            set { _Balance = value; OnPropertyChanged("Balance"); }
        }

        private bool _Enable;
        public bool Enable
        {
            get { return _Enable; }
            set { _Enable = value; OnPropertyChanged("Enable"); }
        }

        private int _Remaining;
        public int Remaining
        {
            get { return _Remaining; }
            set { _Remaining = value; OnPropertyChanged("Remaining"); }
        }


        private IEnumerable<z_LoanPayment> _LoanPayment;
        public IEnumerable<z_LoanPayment> LoanPayment
        {
            get { return _LoanPayment; }
            set { _LoanPayment = value; OnPropertyChanged("LoanPayment"); }
        }

        private z_LoanPayment _CurrentLoanPayment;
        public z_LoanPayment CurrentLoanPayment
        {
            get { return _CurrentLoanPayment; }
            set
            {
                _CurrentLoanPayment = value; OnPropertyChanged("CurrentLoanPayment");
                refreshDetailedLoanPayment();
            }
        }

        private dtl_LoanPayment _CurrentDetailedLoanPayment;
        public dtl_LoanPayment CurrentDetailedLoanPayment
        {
            get { return _CurrentDetailedLoanPayment; }
            set { _CurrentDetailedLoanPayment = value; OnPropertyChanged("CurrentDetailedLoanPayment"); }
        }

        private IEnumerable<dtl_LoanPayment> _DetailedLoanPayment;
        public IEnumerable<dtl_LoanPayment> DetailedLoanPayment
        {
            get { return _DetailedLoanPayment; }
            set { _DetailedLoanPayment = value; OnPropertyChanged("DetailedLoanPayment"); }
        }

        private AprrovedLoanPaymentView _CurrentApprovedLoan;
        public AprrovedLoanPaymentView CurrentApprovedLoan
        {
            get { return _CurrentApprovedLoan; }
            set { _CurrentApprovedLoan = value; OnPropertyChanged("CurrentApprovedLoan"); }
        }
        #endregion

        #region RefreshMethods
        void RefreshPeriod()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Period = e.Result;
            };
            serviceClient.GetPeriodsAsync();
        }
        void refreshLoanPayment()
        {
            try
            {
                serviceClient.GetLoanPaymentCompleted += (s, e) =>
                    {
                        LoanPayment = e.Result.Where(l => l.employee_loan_id == CurrentApprovedLoan.employee_loan_id);
                    };
                serviceClient.GetLoanPaymentAsync();
            }
            catch (Exception)
            { }
        }

        void GetLastPayment()
        {
            try
            {
                GETLASTPAYMENT = serviceClient.GetLastLoan((Guid)CurrentApprovedLoan.employee_loan_id);
            }
            catch (Exception)
            { }
        }

        void refreshDetailedLoanPayment()
        {
            try
            {
                serviceClient.GetDetailedLoanPaymentCompleted += (s, e) =>
                      { DetailedLoanPayment = e.Result; };
                serviceClient.GetDetailedLoanPaymentAsync(CurrentLoanPayment.payment_id);
            }
            catch (Exception)
            { }
        }
        #endregion

        #region Save
        void Save()
        {
            GetLastPayment();
            CurrentLoanPayment = null;
            CurrentLoanPayment = new z_LoanPayment();
            CurrentLoanPayment.payment_id = Guid.NewGuid();

            #region List
            List<dtl_LoanPayment> dtlLoan = new List<dtl_LoanPayment>();
            #endregion

            z_LoanPayment payment = new z_LoanPayment();

            payment.payment_id = CurrentLoanPayment.payment_id;
            payment.employee_loan_id = (Guid)CurrentApprovedLoan.employee_loan_id;
            payment.paid_date = System.DateTime.Now;
            payment.amount = CurrentApprovedLoan.monthly_installment_with_intrest * NoOfMonths;
            payment.cashed_paid = (decimal)Payment;
            payment.period_id = CurrentPeriod.period_id;
            //_________________________________________________________________________________________________________
            for (int i = 1; i <= NoOfMonths; i++)
            {
                CurrentDetailedLoanPayment = new dtl_LoanPayment();
                CurrentDetailedLoanPayment.trn_loan_id = Guid.NewGuid();
                CurrentDetailedLoanPayment.payment_id = CurrentLoanPayment.payment_id;
                //________________________________________________________________________________________________________
                if (GETLASTPAYMENT == null)
                    CurrentDetailedLoanPayment.installment_no = 1 + dtlLoan.Count;
                else
                    CurrentDetailedLoanPayment.installment_no = GETLASTPAYMENT.installment_no + i;
                //_________________________________________________________________________________________________________
                CurrentDetailedLoanPayment.payment_amount = CurrentApprovedLoan.monthly_installment_with_intrest;
                CurrentDetailedLoanPayment.payment_date = System.DateTime.Now;
                dtlLoan.Add(CurrentDetailedLoanPayment);
            }
            //____________________________________________________________________________________________________________
            payment.save_user_id = clsSecurity.loggedUser.user_id;
            payment.save_datetime = System.DateTime.Now;

            if (serviceClient.SaveLoanPayment(payment, dtlLoan.ToArray()))
            {
                ReportPrint print = new ReportPrint("\\Reports\\Documents\\Loan_Report\\LoanSlip");
                print.setParameterValue("@PAYMENT_ID", payment.payment_id.ToString());
                print.PrintReportWithReportViewer();
                MessageBox.Show("Record Update Successfully");
                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                New();
            }
            else
            {
                MessageBox.Show("Record Update Failed");
                clsMessages.setMessage(Properties.Resources.UpdateFail);
            }


        }
        #endregion

        #region Buttons
        public ICommand ClearButton
        {
            get { return new RelayCommand(New); }
        }
        public ICommand BalanceButton
        {
            get { return new RelayCommand(balance); }
        }
        public ICommand PaymentButton
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }
        public ICommand Reprint
        {
            get { return new RelayCommand(reprint); }
        }
        private String color;
        public String Color
        {
            get { return color; }
            set { color = value; OnPropertyChanged("Color"); }
        }

        bool SaveCanExecute()
        {
            Color = "#FFF7F7F7";
            if (Payment < (double)CurrentApprovedLoan.monthly_installment_with_intrest * NoOfMonths)
                Color = "#FFFFA0A0";
            if (Payment < (double)CurrentApprovedLoan.monthly_installment_with_intrest * NoOfMonths)
                return false;
            if (CurrentPeriod == null || CurrentPeriod.period_id == null ||CurrentPeriod.period_id == Guid.Empty)
                return false;
            else
                return true;
        }
        #endregion

        #region New
        void New()
        {
            RefreshPeriod();
            CurrentLoanPayment = null;
            CurrentLoanPayment = new z_LoanPayment();
            refreshLoanPayment();
            GetLastPayment();
            Payment = 0;
            NoOfMonths = 1;
            Completed = "";
            PaymentCompleted();

        }
        #endregion

        #region Methods
        void balance()
        {
            Balance = Payment - Math.Round(((double)CurrentApprovedLoan.monthly_installment_with_intrest * NoOfMonths), 2);
            Balance = Math.Round(Balance, 2);
        }

        void PaymentCompleted()
        {

            Enable = true;
            if (GETLASTPAYMENT != null)
            {
                Remaining = (int)(CurrentApprovedLoan.no_of_installment - GETLASTPAYMENT.installment_no);

                if (CurrentApprovedLoan.no_of_installment == GETLASTPAYMENT.installment_no)
                {
                    Completed = "Payment Completed";
                    Enable = false;
                }

            }
            else
                Remaining = (int)CurrentApprovedLoan.no_of_installment;
        }

        void reprint()
        {
            ReportPrint print = new ReportPrint("\\Reports\\Documents\\Loan_Report\\LoanSlip");
            print.setParameterValue("@PAYMENT_ID", CurrentLoanPayment.payment_id.ToString());
            print.PrintReportWithReportViewer();
        }
        #endregion
    }
}