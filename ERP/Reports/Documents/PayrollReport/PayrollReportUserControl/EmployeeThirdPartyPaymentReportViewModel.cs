using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    class EmployeeThirdPartyPaymentReportViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        
        #endregion

        #region Constrouctor
        public EmployeeThirdPartyPaymentReportViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }
        #endregion

        #region Properties

        private IEnumerable<z_EmployeeThirdPartyPayments> _ThirdPartyPaymentCategories;

        public IEnumerable<z_EmployeeThirdPartyPayments> ThirdPartyPaymentCategories
        {
            get { return _ThirdPartyPaymentCategories; }
            set { _ThirdPartyPaymentCategories = value; OnPropertyChanged("ThirdPartyPaymentCategories"); }
        }

        private z_EmployeeThirdPartyPayments _CurrentThirdPartyPaymentCategories;

        public z_EmployeeThirdPartyPayments CurrentThirdPartyPaymentCategories
        {
            get { return _CurrentThirdPartyPaymentCategories; }
            set { _CurrentThirdPartyPaymentCategories = value; OnPropertyChanged("CurrentThirdPartyPaymentCategories"); }
        }

        private DateTime _FromDate;

        public DateTime FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; OnPropertyChanged("FromDate"); }
        }

        private DateTime _ToDate;

        public DateTime ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; OnPropertyChanged("ToDate"); }
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
        #endregion

        #region Refresh Methods

        private void RefreshThirdPartyCategories()
        {
            serviceClient.GetThirdPartyCategoriesCompleted += (s, e) =>
            {
                ThirdPartyPaymentCategories = e.Result;
            };
            serviceClient.GetThirdPartyCategoriesAsync();
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

                        path = "\\Reports\\Documents\\PayrollReport\\EmployeeThirdPartyPaymentsReport";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@Category", CurrentThirdPartyPaymentCategories == null ? string.Empty : CurrentThirdPartyPaymentCategories.category_name.ToString());
                        print.setParameterValue("@FromDate", FromDate);
                        print.setParameterValue("@ToDate", ToDate);
                        print.LoadToReportViewer(); 
                    }
                    else if(Slip)
                    {
                        path = "\\Reports\\Documents\\PayrollReport\\EmployeeThirdPartyPaymentsSlip";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@Category", CurrentThirdPartyPaymentCategories == null ? string.Empty : CurrentThirdPartyPaymentCategories.category_name.ToString());
                        print.setParameterValue("@FromDate", FromDate);
                        print.setParameterValue("@ToDate", ToDate);
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
            if (CurrentThirdPartyPaymentCategories == null)
            {
                clsMessages.setMessage("Please Select A Period..");
                return false;
            }
            else if (CurrentThirdPartyPaymentCategories.category_id == 0)
            {
                clsMessages.setMessage("Please Select A Period..");
                return false;
            }
            else
                return true;
        }

        private void New()
        {
            RefreshThirdPartyCategories();
            CurrentThirdPartyPaymentCategories = new z_EmployeeThirdPartyPayments();
            Report = true;
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
        }
        #endregion
    }
}
