using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ERP.Loan_Module.Basic_Masters
{
    class LoanProcessViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient;
        #endregion

        #region Global Variables and Lists
        List<AprrovedLoanPaymentView> ListLoan = new List<AprrovedLoanPaymentView>();
        string LoanComplete = "";
        #endregion

        #region Constructor
        public LoanProcessViewModel()
        {
            serviceClient = new ERPServiceClient();
            Minimum = 0;
            Maximum = 100;
            RefereshPeriod();
            RefreshLoanApprovedView();
        }
        #endregion

        #region Properties
        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set
            {
                _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod");
                if (value != null) GetPeriodLoan();
            }
        }

        private IEnumerable<z_Period> _Periods;
        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private IEnumerable<GetPayPeriodLoan_PROC_Result> _GetPayPeriods_Proc;
        public IEnumerable<GetPayPeriodLoan_PROC_Result> GetPayPeriods_Proc
        {
            get { return _GetPayPeriods_Proc; }
            set { _GetPayPeriods_Proc = value; OnPropertyChanged("GetPayPeriods_Proc"); }
        }

        private IEnumerable<AprrovedLoanPaymentView> _LoanApprovedViews;
        public IEnumerable<AprrovedLoanPaymentView> LoanApprovedViews
        {
            get { return _LoanApprovedViews; }
            set { _LoanApprovedViews = value; OnPropertyChanged("LoanApprovedViews"); }
        }

        private AprrovedLoanPaymentView _CurrentApprovedLoan;
        public AprrovedLoanPaymentView CurrentApprovedLoan
        {
            get { return _CurrentApprovedLoan; }
            set { _CurrentApprovedLoan = value; OnPropertyChanged("CurrentApprovedLoan"); }
        }

        private IEnumerable<AprrovedLoanPaymentView> _RemoveFromProcess;
        public IEnumerable<AprrovedLoanPaymentView> RemoveFromProcess
        {
            get { return _RemoveFromProcess; }
            set { _RemoveFromProcess = value; OnPropertyChanged("RemoveFromProcess"); }
        }


        private GETLASTPAYMENT_PROC_Result _GETLASTPAYMENT;
        public GETLASTPAYMENT_PROC_Result GETLASTPAYMENT
        {
            get { return _GETLASTPAYMENT; }
            set { _GETLASTPAYMENT = value; OnPropertyChanged("GETLASTPAYMENT"); }
        }

        private int _Maximum;
        public int Maximum
        {
            get { return _Maximum; }
            set { _Maximum = value; OnPropertyChanged("Maximum"); }
        }

        private int _Minimum;
        public int Minimum
        {
            get { return _Minimum; }
            set { _Minimum = value; OnPropertyChanged("Minimum"); }
        }

        private int _Progress;
        public int Progress
        {
            get { return _Progress; }
            set { _Progress = value; OnPropertyChanged("Progress"); }
        }


        #endregion

        #region Process
        void ThreadProgress()
        {
            if (clsSecurity.GetSavePermission(606))
            {
                DispatcherTimer tick = new DispatcherTimer();
                tick.Interval = new TimeSpan(0, 0, 0, 0, 10);
                tick.Start();
                tick.Tick += (s, e) =>
                {

                    Progress++;
                    if (Progress == 5)
                    {
                        tick.Stop();
                        Processe();
                        tick.Start();
                    }
                    if (Progress == 100)
                    {
                        if (LoanComplete != "")
                            Xceed.Wpf.Toolkit.MessageBox.Show(LoanComplete + "\n", "Completed List", MessageBoxButton.OK, MessageBoxImage.Information);
                        clsMessages.setMessage("Do you want to refresh employees");
                        if (clsMessages.Messagebox.viewModel.Result == "OK")
                            New();
                    }
                };
            }
            else
                clsMessages.setMessage("You don't have Permission to Loan Process...");
        }

        void Processe()
        {
            if (LoanApprovedViews != null)
            {



                if (LoanApprovedViews.ToList().Count() != 0)
                {
                    List<AprrovedLoanPaymentView> SaveList = new List<AprrovedLoanPaymentView>();

                    List<AprrovedLoanPaymentView> RemoveList = new List<AprrovedLoanPaymentView>();

                    SaveList.AddRange(LoanApprovedViews.OrderBy(c => c.first_name));

                    RemoveList.AddRange(LoanApprovedViews.OrderBy(c => c.first_name));

                    foreach (var item in SaveList)
                    {
                        z_LoanPayment Payment = new z_LoanPayment();

                        Payment.payment_id = Guid.NewGuid();
                        Payment.amount = item.monthly_installment_with_intrest;
                        Payment.cashed_paid = item.monthly_installment_with_intrest;
                        Payment.employee_loan_id = (Guid)item.employee_loan_id;
                        Payment.period_id = CurrentPeriod.period_id;
                        Payment.save_user_id = clsSecurity.loggedUser.user_id;
                        Payment.save_datetime = System.DateTime.Now;

                        List<dtl_LoanPayment> dtlLoan = new List<dtl_LoanPayment>();

                        dtl_LoanPayment pay = new dtl_LoanPayment();

                        pay.payment_id = Payment.payment_id;
                        pay.trn_loan_id = Guid.NewGuid();
                        pay.payment_date = Payment.paid_date;
                        pay.payment_amount = item.monthly_installment_with_intrest;

                        GETLASTPAYMENT = serviceClient.GetLastLoan((Guid)item.employee_loan_id);

                        if (GETLASTPAYMENT != null)
                            pay.installment_no = GETLASTPAYMENT.installment_no + 1;
                        else
                            pay.installment_no = 1;

                        dtlLoan.Add(pay);

                        if (serviceClient.SaveLoanPayment(Payment, dtlLoan.ToArray()))
                        {
                            serviceClient.SaveLoanToPeriodQuantity
                                (
                                (decimal)item.monthly_installment_with_intrest,
                                (Guid)item.loan_id,
                                (Guid)item.employee_id,
                                clsSecurity.loggedUser.user_id,
                                (DateTime)Payment.save_datetime,
                                (Guid)CurrentPeriod.period_id
                                );

                            if (serviceClient.GetLoanCompletelyEnd((Guid)item.employee_loan_id))
                            {
                                LoanComplete += item.emp_id + " " + item.first_name + " " + item.surname + " " + "Complete " + "\n";
                            }

                            RemoveList.Remove(item);

                            LoanApprovedViews = null;

                            LoanApprovedViews = RemoveList.OrderBy(c => c.first_name);
                        }
                        else
                        {


                        }
                    }
                }
            }
        }
        #endregion

        #region New
        void New()
        {
            CurrentPeriod = null;
            RefereshPeriod();
            RefreshLoanApprovedView();
        }
        #endregion

        #region Process Button
        public ICommand ProcessButton
        {
            get { return new RelayCommand(ThreadProgress, ProcesseCanExecute); }
        }
        bool ProcesseCanExecute()
        {
            if (CurrentPeriod == null)
                return false;
            else
                return true;
        }
        #endregion

        #region Refresh Method
        void RefereshPeriod()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                try
                {
                    Periods = e.Result;
                }
                catch (Exception)
                {
                }
            };
            serviceClient.GetPeriodsAsync();
        }

        void RefreshLoanApprovedView()
        {
            serviceClient.GetAprrovedLoanPaymentViewCompleted += (s, e) =>
            {
                try
                {
                    LoanApprovedViews = e.Result.Where(c => c.is_auto_deduct == true && c.is_completed != true);
                    if (LoanApprovedViews != null)
                    {
                        ListLoan = LoanApprovedViews.ToList();
                    }
                }
                catch (Exception)
                {
                }
            };
            serviceClient.GetAprrovedLoanPaymentViewAsync();
        }
        #endregion

        #region Filters
        void PeriodFilter()
        {
            try
            {
                List<AprrovedLoanPaymentView> ExceptLists = new List<AprrovedLoanPaymentView>();
                foreach (var item in GetPayPeriods_Proc)
                {
                    AprrovedLoanPaymentView CurrentExcept = LoanApprovedViews.FirstOrDefault(c => c.employee_loan_id == item.employee_loan_id);
                    if (CurrentExcept != null && CurrentExcept.employee_loan_id != null)
                        ExceptLists.Add(CurrentExcept);
                }
                LoanApprovedViews = ListLoan.Except(ExceptLists).OrderBy(c => c.first_name);
            }
            catch (Exception)
            {
            }
        }

        async void GetPeriodLoan()
        {
            try
            {
                Task a = new Task(() => GetPayPeriods_Proc = serviceClient.GetLoanPayPeriod(CurrentPeriod.period_id.ToString()));
                a.Start();
                await a;
                PeriodFilter();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Period Error");
            }
        }
        #endregion

        void Remove()
        {
            List<AprrovedLoanPaymentView> Temp = new List<AprrovedLoanPaymentView>();
            if (CurrentApprovedLoan != null)
            {
                if (RemoveFromProcess != null)
                    Temp = RemoveFromProcess.ToList();
                Temp.Add(CurrentApprovedLoan);
                ListLoan.Remove(CurrentApprovedLoan);
                LoanApprovedViews = LoanApprovedViews.Except(Temp);
                RemoveFromProcess = null;
                RemoveFromProcess = Temp;
            }
        }
        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove); }
        }

        void Add()
        {
            List<AprrovedLoanPaymentView> Temp = new List<AprrovedLoanPaymentView>();
            if (CurrentApprovedLoan != null)
            {
                if (LoanApprovedViews != null)
                    Temp = LoanApprovedViews.ToList();
                Temp.Add(CurrentApprovedLoan);
                ListLoan.Add(CurrentApprovedLoan);
                RemoveFromProcess = RemoveFromProcess.Except(Temp);
                LoanApprovedViews = null;
                LoanApprovedViews = Temp;

            }
        }
        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }


        void Search()
        {
            EmployeeMultipleSearchWindow Search = new EmployeeMultipleSearchWindow();
            Search.ShowDialog();
            List<AprrovedLoanPaymentView> Temp = new List<AprrovedLoanPaymentView>();
            if (Search.viewModel.SelectedList != null)
            {

                try
                {
                    foreach (var item in  Search.viewModel.SelectedList.ToList())
                    {
                        if(ListLoan.FirstOrDefault(z => z.employee_id == item.employee_id) != null ) Temp.Add(ListLoan.FirstOrDefault(z => z.employee_id == item.employee_id));
                       
                    }
                }
                catch (Exception)
                {
                }
            }

            LoanApprovedViews = null;
            LoanApprovedViews = Temp;
        }
        public ICommand SearchButton
        {
            get { return new RelayCommand(Search); }
        }


    }
}
