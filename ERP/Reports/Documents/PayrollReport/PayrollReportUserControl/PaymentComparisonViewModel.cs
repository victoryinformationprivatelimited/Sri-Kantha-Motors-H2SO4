using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    class PaymentComparisonViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        #region Constructor

        public PaymentComparisonViewModel()
        {
            GetPeriodList();
            GetPaymethods();
            SalaryDeferance = true;
        } 

        #endregion

        #region Properties

        private bool _EnablePaymethod;
        public bool EnablePaymethod
        {
            get { return _EnablePaymethod; }
            set { _EnablePaymethod = value; OnPropertyChanged("EnablePaymethod"); }
        }
        

        private IEnumerable<z_PaymentMethod> _Paymethod;
        public IEnumerable<z_PaymentMethod> Paymethod
        {
            get { return _Paymethod; }
            set { _Paymethod = value; OnPropertyChanged("Paymethod"); }
        }

        private z_PaymentMethod _CurrentPaymethod;
        public z_PaymentMethod CurrentPaymethod
        {
            get { return _CurrentPaymethod; }
            set { _CurrentPaymethod = value; OnPropertyChanged("CurrentPaymethod"); }
        }

        private IEnumerable<z_Period> _Periods;
        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; this.OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; this.OnPropertyChanged("CurrentPeriod"); }
        }

        private bool _SalaryDeferance;
        public bool SalaryDeferance
        {
            get { return _SalaryDeferance; }
            set { _SalaryDeferance = value; this.OnPropertyChanged("SalaryDeferance"); if (SalaryDeferance) EnablePaymethod = true; else EnablePaymethod = false; }
        }

        private bool _SalaryDeferanceForBank;
        public bool SalaryDeferanceForBank
        {
            get { return _SalaryDeferanceForBank; }
            set { _SalaryDeferanceForBank = value; this.OnPropertyChanged("SalaryDeferanceForBank"); }
        }

        #endregion

        #region Button Commands

        public ICommand PrintBTN
        {
            get { return new RelayCommand(Print, PrintCanExecute); }
        }

        private bool PrintCanExecute()
        {
            if (CurrentPeriod != null)
                return true;
            else
                return false;
        }

        private void Print()
        {
            if (SalaryDeferance) 
            {
                if (CurrentPaymethod != null)
                {
                    string path = "\\Reports\\Documents\\PayrollReport\\SalaryDifference";
                    ReportPrint print = new ReportPrint(path);
                    print.setParameterValue("@PERODID", CurrentPeriod.period_id.ToString());
                    print.setParameterValue("@PAYMETHODID", CurrentPaymethod.paymet_method_id.ToString());
                    print.LoadToReportViewer();
                }
                else
                    System.Windows.MessageBox.Show("Please Select a Payment Method", "", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Exclamation);
                    
            }
            else if (SalaryDeferanceForBank) 
            {
                string path = "\\Reports\\Documents\\PayrollReport\\UnilakPaymentDetailsDifference";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@PayPeriod_id", CurrentPeriod.period_id.ToString());
                print.LoadToReportViewer();
            }
        }

        #endregion

        #region Refresh Methods

        #region Periods

        public void GetPeriodList()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetPeriodsAsync();

        }

        #endregion

        #region Pymethod

        public void GetPaymethods()
        {
            serviceClient.GetPaymentMethodsCompleted += (s, e) =>
            {
                Paymethod = e.Result;
            };
            serviceClient.GetPaymentMethodsAsync();
        }

        #endregion 
        #endregion
    }
}
