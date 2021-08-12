using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows.Forms;

namespace ERP.Payroll
{
    public class PayrollPaymentViewModel : ViewModelBase
    {

        ERPServiceClient serviceClient = new ERPServiceClient();
        List<TrnsEmployeeCompanyVariableView> CurrenttrnsList = new List<TrnsEmployeeCompanyVariableView>();

        public PayrollPaymentViewModel()
        {
            refreshPeriods();
            refreshPaymentMethod();
            All = true;
        }

        private IEnumerable<TrnsEmployeePaymet> _TrnPayment;
        public IEnumerable<TrnsEmployeePaymet> TrnPayment
        {
            get { return _TrnPayment; }
            set { _TrnPayment = value; OnPropertyChanged("TrnPayment"); }
        }

        private TrnsEmployeePaymet _CurrentTrnPayment;
        public TrnsEmployeePaymet CurrentTrnPayment
        {
            get { return _CurrentTrnPayment; }
            set { _CurrentTrnPayment = value; OnPropertyChanged("CurrentTrnPayment"); refreshEmployeeCompanyVariableByEmployee(); }
        }

        private IEnumerable<TrnsEmployeeCompanyVariableView> _EmployeeCompanyVariable;
        public IEnumerable<TrnsEmployeeCompanyVariableView> EmployeeCompanyVariable
        {
            get { return _EmployeeCompanyVariable; }
            set { _EmployeeCompanyVariable = value; OnPropertyChanged("EmployeeCompanyVariable"); }
        }

        private IEnumerable<TrnsEmployeeCompanyVariableView> _CurrntEmployeeCompanyVariable;
        public IEnumerable<TrnsEmployeeCompanyVariableView> CurrntEmployeeCompanyVariable
        {
            get { return _CurrntEmployeeCompanyVariable; }
            set { _CurrntEmployeeCompanyVariable = value; OnPropertyChanged("CurrntEmployeeCompanyVariable"); }
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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); refreshPayment(); refreshEmployeeCompanyVariables(); }
        }

        private IEnumerable<z_PaymentMethod> _paymentMethods;
        public IEnumerable<z_PaymentMethod> paymentMethods
        {
            get { return _paymentMethods; }
            set { _paymentMethods = value; OnPropertyChanged("paymentMethods"); }
        }

        private z_PaymentMethod _CurrentPaymentMethod;
        public z_PaymentMethod CurrentPaymentMethod
        {
            get { return _CurrentPaymentMethod; }
            set { _CurrentPaymentMethod = value; OnPropertyChanged("CurrentPaymentMethod"); }
        }

        private bool _All;
        public bool All
        {
            get { return _All; }
            set { _All = value; OnPropertyChanged("All"); }
        }

        private bool _Single;
        public bool Single
        {
            get { return _Single; }
            set { _Single = value; OnPropertyChanged("Single"); }
        }

        public ICommand Priview
        {
            get
            {
                return new RelayCommand(ButtonPriview, PriviewCanExecute);
            }
        }

        public ICommand Print
        {
            get
            {
                return new RelayCommand(print, printCanExecute);

            }
        }

        public ICommand Pay
        {
            get
            {
                return new RelayCommand(PayPayment, PayPaymentCanExecute);
            }
        }

        public ICommand Clear
        {
            get 
            {
                return new RelayCommand(clear, clearCanExecute);
            }
        }

        private bool clearCanExecute()
        {
            return true;
        }

        private void clear()
        {
            TrnPayment = null;
            CurrentPeriod = null;
            CurrentPaymentMethod = null;
            All = true;
            Single = false;
        }       

        private bool PayPaymentCanExecute()
        {
            if (CurrentPaymentMethod != null && CurrentPeriod != null && All == true || Single == true)
                return true;
            else
                return false;
        }

        private void PayPayment()
        {
            if (CurrentPaymentMethod != null)
            {
                if (clsSecurity.GetSavePermission(508))
                {
                    if (All)
                    {
                        DialogResult result = MessageBox.Show("Are you sure for payment all employees ?", "ERP Ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (DialogResult.Yes == result)
                        {
                            List<TrnsEmployeePaymet> invalidList = new List<TrnsEmployeePaymet>();
                            List<TrnsEmployeePaymet> validList = new List<TrnsEmployeePaymet>();
                            this.TrnPayment = this.serviceClient.GetPaymetsByPeriodID(this.CurrentPeriod.period_id);

                            foreach (TrnsEmployeePaymet item in TrnPayment.Where(e => e.isPaied == true))
                            {
                                invalidList.Add(item);
                            }
                            foreach (TrnsEmployeePaymet item in TrnPayment.Where(e => e.isPaied == false))
                            {
                                validList.Add(item);
                            }
                            foreach (TrnsEmployeePaymet item in validList)
                            {
                                item.isPaied = true;
                                item.payment_method_id = CurrentPaymentMethod.paymet_method_id;
                                item.paydate = DateTime.Now;
                            }

                            this.TrnPayment = null;
                            this.TrnPayment = validList;

                            this.serviceClient.PayPaymentAll(TrnPayment.ToArray());
                            this.serviceClient.GenaratePayrollReoprt(CurrentPeriod.period_id);
                            printAll();
                        }


                    }
                    if (Single)
                    {

                        this.serviceClient.PayPaymentOne(CurrentTrnPayment);
                        if (!(bool)CurrentTrnPayment.isPaied)
                        {
                            CurrentTrnPayment.isPaied = true;
                            CurrentTrnPayment.payment_method_id = CurrentPaymentMethod.paymet_method_id;
                            CurrentTrnPayment.paydate = DateTime.Now;
                            this.serviceClient.PayPaymentOne(CurrentTrnPayment);
                            this.serviceClient.GenaratePayrollReoprt(CurrentPeriod.period_id);
                            printSingle();
                        }
                        else
                        {
                            MessageBox.Show("This employee already paid", "ERP Says !", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to pay");
            }
            else
            {
                MessageBox.Show("Please mension the payment method..!", "ERP Says !!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printSingle()
        {
            string filter = "{Rpt_MasEmployeeTransaction.employee_id}='" + CurrentTrnPayment.employee_id.ToString("D") + "' and {Rpt_MasEmployeeTransaction.period_id}='" + CurrentPeriod.period_id.ToString("D") + "'";

            ReportViewer newReport = new ReportViewer();
            newReport.ReportLoad("Payroll", "PaySlip", filter.Trim(), "");
            newReport.Show();
        }

        private void printAll()
        {
            MessageBox.Show("Now Generated Reports", "Payment Completed", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //string filter = "{Rpt_MasEmployeeTransaction.period_id}='" + CurrentPeriod.period_id.ToString("D") + "'";
            //ReportViewer newReport = new ReportViewer();
            //newReport.ReportLoad("Payroll", "PaySlip", filter.Trim(),"");
            //newReport.Show();
        }

        private bool printCanExecute()
        {
            if (CurrentPaymentMethod != null && CurrentPeriod != null && All == true || Single == true)
                return true;
            else
                return false;
        }

        private void print()
        {
            if (Single)
            {
                printSingle();
            }
            if (All)
            {
                printAll();
            }
        }

        private bool PriviewCanExecute()
        {
            if (CurrentPaymentMethod != null && CurrentPeriod != null && All == true || Single == true)
                return true;
            else
                return false;
        }

        private void ButtonPriview()
        {
            //ReportViewer newReport = new ReportViewer();
            //newReport.ReportLoad("Payroll", "PaySlip",filter.Trim(), "");
            //newReport.Show();
        }

        private void refreshPaymentMethod()
        {
            this.serviceClient.GetPaymentMethodsCompleted += (s, e) =>
            {
                this.paymentMethods = e.Result;
            };
            this.serviceClient.GetPaymentMethodsAsync();
        }

        private void refreshPayment()
        {
            this.serviceClient.GetPaymetsByPeriodIDCompleted += (s, e) =>
            {
                this.TrnPayment = e.Result;
            };

            if(CurrentPeriod != null)
            this.serviceClient.GetPaymetsByPeriodIDAsync(CurrentPeriod.period_id);
        }

        private void refreshPeriods()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                this.Periods = e.Result.Where(z=>z.isdelete==false);
            };
            this.serviceClient.GetPeriodsAsync();
        }

        private void refreshEmployeeCompanyVariables()
        {
            this.serviceClient.GettrnEmployeeCompanyvariablesCompleted += (s, e) =>
                {
                    this.EmployeeCompanyVariable = e.Result;
                };
            this.serviceClient.GettrnEmployeeCompanyvariablesAsync(CurrentPeriod.period_id);    
        }

        private void refreshEmployeeCompanyVariableByEmployee()
        {
            CurrenttrnsList.Clear();
            CurrntEmployeeCompanyVariable = null;
            if (EmployeeCompanyVariable != null && CurrentTrnPayment != null)
            {
                foreach (TrnsEmployeeCompanyVariableView item in EmployeeCompanyVariable.Where(e => e.employee_id == CurrentTrnPayment.employee_id))
                {
                    CurrenttrnsList.Add(item);
                }
            }
            CurrntEmployeeCompanyVariable = CurrenttrnsList;
        }
    }
}
