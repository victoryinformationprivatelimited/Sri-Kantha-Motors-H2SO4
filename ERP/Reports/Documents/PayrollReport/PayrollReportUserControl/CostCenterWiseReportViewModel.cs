using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    class CostCenterWiseReportViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public CostCenterWiseReportViewModel()
        {
            serviceClient = new ERPServiceClient();
            Executive = false;
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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }

        private bool _Executive;

        public bool Executive
        {
            get { return _Executive; }
            set { _Executive = value; OnPropertyChanged("Executive"); }
        }
        
        #endregion

        #region Refresh Methods
        private void RefreshPeriods()
        {
            try
            {
                serviceClient.GetPeriodsCompleted += (s, e) =>
                    {
                        Periods = e.Result.OrderBy(c => c.start_date);
                    };
                serviceClient.GetPeriodsAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Periods Refresh Failed");
            }
        }
        #endregion

        #region Commands And Methods
        public ICommand btnPrint
        {
            get { return new RelayCommand(Print); }
        }

        private void Print()
        {
            string path = "\\Reports\\Documents\\PayrollReport\\CostCenterWiseReport";

            try
            {
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@periodID", CurrentPeriod == null ? string.Empty : CurrentPeriod.period_id.ToString());
                print.setParameterValue("@isExecutive", Executive);
                print.LoadToReportViewer();
            }
            catch (Exception ex)
            {
                clsMessages.setMessage("Report loading is failed. " + ex.Message);
            }
        }
        #endregion
    }
}
