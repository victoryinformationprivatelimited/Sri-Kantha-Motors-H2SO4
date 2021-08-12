using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    class EmployeeOtherPaymentViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        
        #endregion

        #region Constrouctor
        public EmployeeOtherPaymentViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
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

        private bool _Report;

        public bool Report
        {
            get { return _Report; }
            set { _Report = value; OnPropertyChanged("Report"); }
        }

        private bool _Slip;

        public bool Slip
        {
            get { return _Slip; }
            set { _Slip = value; OnPropertyChanged("Slip"); }
        }

        private bool _PayeeNotDeducted;

        public bool PayeeNotDeducted
        {
            get { return _PayeeNotDeducted; }
            set { _PayeeNotDeducted = value; OnPropertyChanged("PayeeNotDeducted"); }
        }

        private bool _OverDue;

        public bool OverDue
        {
            get { return _OverDue; }
            set { _OverDue = value; OnPropertyChanged("OverDue"); }
        }
        
        #endregion

        #region Refresh Methods

        private void RefreshPeriod()
        {
            try
            {
                serviceClient.GetAllPeriodCompleted += (s, e) =>
                        {
                            Periods = e.Result;
                        };
                serviceClient.GetAllPeriodAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Periods Refresh Failed");
            }
        }
        
        #endregion

        #region Button Commands
        public ICommand PrintButton
        {
            get { return new RelayCommand(print); }
        }

        #endregion

        #region Methods
        private void print()
        {
            if (ValidatePrint())
            {
                string path = "";
                try
                {
                    if (Report)
                    {

                        path = "\\Reports\\Documents\\PayrollReport\\EmployeeOtherPaymentsReport";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@PeriodID", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
                        print.LoadToReportViewer(); 
                    }
                    else if(Slip)
                    {
                        path = "\\Reports\\Documents\\PayrollReport\\EmployeeOtherPaymentsSlip";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@PeriodID", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
                        print.LoadToReportViewer(); 
                    }
                    else if (PayeeNotDeducted)
                    {
                        path = "\\Reports\\Documents\\PayrollReport\\PayeeNotDeductedReport";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@PeriodID", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
                        print.LoadToReportViewer();
                    }
                    else if (OverDue)
                    {
                        path = "\\Reports\\Documents\\PayrollReport\\OverDueAmountReport";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@PeriodID", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
                        print.LoadToReportViewer();
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("Report loading is failed: " + ex.Message);
                }
            } 
        }
        private bool ValidatePrint()
        {
            if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please Select A Period..");
                return false;
            }
            else if (CurrentPeriod.period_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Period..");
                return false;
            }
            else
                return true;
        }

        private void New()
        {
            RefreshPeriod();
            CurrentPeriod = new z_Period();
            Report = true;
        }
        #endregion
    }
}
