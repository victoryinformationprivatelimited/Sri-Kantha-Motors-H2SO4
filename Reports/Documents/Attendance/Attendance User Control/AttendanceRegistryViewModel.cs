using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;



namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    class AttendanceRegistryViewModel:ViewModelBase
    {

        #region ServiceClient

        ERPServiceClient Serviceclient;

        #endregion
        public AttendanceRegistryViewModel()
        {
            Serviceclient = new ERPServiceClient();
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
        }


        #region StartDate

        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged("SelectDate"); }
        }

        #endregion

        #region EndDate

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; OnPropertyChanged("EndDate"); }
        }
        
        #endregion

        #region GenerateButton

        public ICommand GenerateButton
        {
            get { return new RelayCommand(Generate); }
        }

        void Generate() 
        {
            try
            {
                ReportPrint print = new ReportPrint("\\Reports\\Documents\\Attendance\\EmployeeAttendanceRegistry");
                print.setParameterValue("@StartDate", StartDate.Date);
                print.setParameterValue("@EndDate", EndDate.Date);
                print.PrintReportWithReportViewer();

            }
            catch (Exception)
            {
                
                throw;
            }
        }


        #endregion
    }
}
