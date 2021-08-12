using ERP.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ERP.BasicSearch;
using ERP.ERPService;
using ERP.AttendanceModule.Calendar;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class EmployeeShiftViewCalendarViewModel:ViewModelBase
    {
        #region Attend Service Client

        AttendanceServiceClient attendClient;

        #endregion

        #region Constructor
        public EmployeeShiftViewCalendarViewModel()
        {
            attendClient = new AttendanceServiceClient();

        } 

        #endregion

        #region List Members

        List<EmployeeSearchView> selectedEmployeeList = new List<EmployeeSearchView>();
        

        #endregion

        #region Properties

        IEnumerable<EmployeeSearchView> selectedEmployees;
        public IEnumerable<EmployeeSearchView> SelectedEmployees
        {
            get { return selectedEmployees; }
            set { selectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }

        EmployeeSearchView currentSelectedEmployee;
        public EmployeeSearchView CurrentSelectedEmployee
        {
            get { return currentSelectedEmployee; }
            set { currentSelectedEmployee = value; OnPropertyChanged("CurrentSelectedEmployee");}
        }

        #endregion

        #region Button Methods

        #region Search Employee Button

        public ICommand SearchEmployeeButton
        {
            get { return new RelayCommand(SearchEmployee); }
        }

        private void SearchEmployee()
        {
            EmployeeMultipleSearchWindow searchWindow = new EmployeeMultipleSearchWindow();
            searchWindow.ShowDialog();
            if (searchWindow.viewModel.selectEmployeeList != null)
            {
                selectedEmployeeList.Clear();
                selectedEmployeeList = searchWindow.viewModel.selectEmployeeList;
                SelectedEmployees = null;
                SelectedEmployees = selectedEmployeeList;
            }
            searchWindow.Close();
            searchWindow = null;
        } 

        #endregion

        #region View Calendar Button

        public ICommand ViewCalendarButton
        {
            get { return new RelayCommand(ViewCalendar); }
        }

        void ViewCalendar()
        {
            try
            {
                if (currentSelectedEmployee != null)
                {
                    CalendarWindow employeeCalendar = new CalendarWindow(currentSelectedEmployee.employee_id);
                    employeeCalendar.ShowDialog();
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Calendar couldn't created");
            }
        }

        #endregion
        
        #endregion
    }
}
