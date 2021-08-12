using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    class UnprocessedAttendanceDataViewModel:ViewModelBase
    {
        ERPServiceClient serviceClient;

        public UnprocessedAttendanceDataViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployees();
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
        }

        private void RefreshEmployees()
        {
            serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                Employees = e.Result;
            };
            serviceClient.GetEmployeesAsync();
        }

        #region Properties

        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private DateTime? _FromDate;
        public DateTime? FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; OnPropertyChanged("FromDate"); }
        }

        private DateTime? _ToDate;
        public DateTime? ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; OnPropertyChanged("ToDate"); }
        }
        
        
        #endregion

        #region Button Commands

        public ICommand PrintBTN 
        {
            get { return new RelayCommand(PrintReport); }
        }

        private void PrintReport()
        {
            try
            {
                string path = "\\Reports\\Documents\\Attendance\\UnprocessedAttendanceData";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@Fromdate", FromDate != null ? FromDate : DateTime.Now);
                print.setParameterValue("@Todate", ToDate != null ? ToDate : DateTime.Now);
                print.setParameterValue("@EmpID", CurrentEmployee != null ? CurrentEmployee.emp_id : "");
                print.LoadToReportViewer();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Report loading is failed: " + ex.Message);
            }
        }

        #endregion
    }
}
