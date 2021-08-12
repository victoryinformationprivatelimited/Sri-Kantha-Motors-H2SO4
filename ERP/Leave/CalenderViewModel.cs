using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;

namespace ERP.Leave
{
    public class CalenderViewModel : ViewModelBase
    {
        ERPServiceClient serviceClient = new ERPServiceClient();        
         
        public CalenderViewModel(DateTime Calenderdate)
        {
            NewCalender = new Calender();
            SeletedDateTime = Calenderdate;
            getEmployees();
        }

        private Calender _NewCalender;
        public Calender NewCalender
        {
            get { return _NewCalender; }
            set { _NewCalender = value; OnPropertyChanged("NewCalender"); }
        }

        private DateTime _SeletedDateTime;
        public DateTime SeletedDateTime
        {
            get { return _SeletedDateTime; }
            set { _SeletedDateTime = value; OnPropertyChanged("SeletedDateTime"); if (SeletedDateTime != null) { initializingCalender(); } }
        }

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
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); getEmployeeLeaves(); }
        }

        private void getEmployees()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) => 
            {
                this.Employees = e.Result;
            };
            this.serviceClient.GetEmployeesAsync();
        }

        private void getSelectedMonthHolidays()
        {
            this.serviceClient.getSelectedMonthHolidaysCompleted += (s, e) =>
            {
                NewCalender.Holidays = e.Result;
                NewCalender.setDates();
                NewCalender.setHolidays();
                
            };
            this.serviceClient.getSelectedMonthHolidaysAsync(NewCalender.CalenderDateTime);
        }

        private void getEmployeeLeaves()
        {
            if (CurrentEmployee != null)
            {
                this.serviceClient.GetLeavesByEmpIdCompleted += (s, e) =>
                {
                    NewCalender.Leaves = e.Result;
                    NewCalender.setLeaves();
                };
                this.serviceClient.GetLeavesByEmpIdAsync(CurrentEmployee.employee_id);
            }
        }

        private void initializingCalender()
        {
            NewCalender.CalenderDateTime = SeletedDateTime.Date;
            getSelectedMonthHolidays();
        }

    }
}
