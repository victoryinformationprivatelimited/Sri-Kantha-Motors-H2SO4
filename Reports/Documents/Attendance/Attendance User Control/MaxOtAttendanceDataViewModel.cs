using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    class MaxOtAttendanceDataViewModel: ViewModelBase
    {
        ERPServiceClient ServiceClient;

        public MaxOtAttendanceDataViewModel()
        {
            ServiceClient = new ERPServiceClient();
            RefreshPeriods();
            RefreshEmployees();
        }

        #region Properties
        private IEnumerable<z_Period> _PayPeriod;
        public IEnumerable<z_Period> PayPeriod
        {
            get { return _PayPeriod; }
            set { _PayPeriod = value; OnPropertyChanged("PayPeriod"); }
        }

        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }

        private IEnumerable<dtl_MaxOtApprovedEmployees> _Employee;
        public IEnumerable<dtl_MaxOtApprovedEmployees> Employee
        {
            get { return _Employee; }
            set { _Employee = value; OnPropertyChanged("Employee"); }
        }

        private dtl_MaxOtApprovedEmployees _CurrentEmployee;
        public dtl_MaxOtApprovedEmployees CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        #endregion

        #region Refresh Methods

        public void RefreshPeriods()
        {
            this.ServiceClient.GetPeriodsCompleted += (s, e) =>
            {
                PayPeriod = e.Result.ToList();
            };
            this.ServiceClient.GetPeriodsAsync();

        }

        public void RefreshEmployees()
        {
            this.ServiceClient.GetMaxOtApprovedEmployeesCompleted += (s, e) =>
            {
                Employee = e.Result.ToList();
            };
            this.ServiceClient.GetMaxOtApprovedEmployeesAsync();
        }

        #endregion

        #region Print Button

        public ICommand PrintButton
        {
            get { return new RelayCommand(Print); }
        }

        public void Print()
        {

            try
            {
                ReportPrint print = new ReportPrint("\\Reports\\Documents\\Attendance\\MaxOtAttendanceReport");
                if (CurrentPeriod != null)
                {
                    print.setParameterValue("@periodID", CurrentPeriod.period_id.ToString());

                    if (CurrentEmployee != null)
                        print.setParameterValue("@empID", Convert.ToInt32(CurrentEmployee.empID).ToString());
                    else
                        print.setParameterValue("@empID","");
                    print.PrintReportWithReportViewer();
                }
                else
                    clsMessages.setMessage("Please select the pay period !");
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Clear Button

        public ICommand ClearButton
        {
            get { return new RelayCommand(Clear); }
        }

        public void Clear()
        {
            RefreshPeriods();
            RefreshEmployees();
        }

        #endregion

    }
}
