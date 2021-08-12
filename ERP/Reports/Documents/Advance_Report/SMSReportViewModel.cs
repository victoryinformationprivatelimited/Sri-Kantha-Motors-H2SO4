using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.Advance_Report
{
    class SMSReportViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;

        #endregion

        #region Constrouctor
        public SMSReportViewModel()
        {
            serviceClient = new ERPServiceClient();
            ToDate = System.DateTime.Now;
            FromDate = System.DateTime.Now;
            New();
        }
        #endregion

        #region Properties

        private DateTime _ToDate;

        public DateTime ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; OnPropertyChanged("ToDate"); }
        }

        private DateTime _FromDate;

        public DateTime FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; OnPropertyChanged("FromDate"); }
        }
        #endregion

        #region Refresh Methods

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

                    path = "\\Reports\\Documents\\Advance_Report\\SMSReport";
                    ReportPrint print = new ReportPrint(path);
                    print.setParameterValue("@ToDate", ToDate);
                    print.setParameterValue("@FromDate", FromDate);
                    print.LoadToReportViewer();
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("Report loading is failed: " + ex.Message);
                }
            }
        }
        private bool ValidatePrint()
        {
            return true;
        }
        private void New()
        {
        }

        #endregion
    }
}
