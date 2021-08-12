using ERP.AttendanceService;
using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    public class AssignEmployeeHolidayViewModel : ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendService;

        #endregion

        #region Constructor

        public AssignEmployeeHolidayViewModel()
        {
            attendService = new AttendanceServiceClient();
            this.RefreshAttendanceGroups();
            IsCheckAll = true;
        }

        #endregion

        #region Data Members

        public AssignEmployeeHolidayWindow ownerWindow;

        #endregion

        #region List Members

        List<ERP.AttendanceService.mas_Employee> selectedGroupEmployees = new List<ERP.AttendanceService.mas_Employee>();
        List<AttendanceGroupDetailsView> selectedGroupsList = new List<AttendanceGroupDetailsView>();
        List<ERP.AttendanceService.mas_Employee> allGroupEmployeesList = new List<ERP.AttendanceService.mas_Employee>();
        List<EmployeeSearchView> searchedEmployeeList = new List<EmployeeSearchView>();
        List<z_HolidayData> searchedHolidayList = new List<z_HolidayData>();

        #region IList

        IList selectEmpList = new ArrayList();

        #endregion

        #endregion

        #region Properties

        public IList SelectEmpList
        {
            get { return selectEmpList; }
            set { selectEmpList = value; OnPropertyChanged("SelectEmpList"); }
        }

        List<ERP.AttendanceService.mas_Employee> selectedEmployees;
        public List<ERP.AttendanceService.mas_Employee> SelectedEmployees
        {
            get { return selectedEmployees; }
            set { selectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }

        ERP.AttendanceService.mas_Employee currentSelectedEmployee;
        public ERP.AttendanceService.mas_Employee CurrentSelectedEmployee
        {
            get { return currentSelectedEmployee; }
            set { currentSelectedEmployee = value; OnPropertyChanged("CurrentSelectedEmployee"); }
        }

        IEnumerable<AttendanceGroupDetailsView> attendanceGroups;
        public IEnumerable<AttendanceGroupDetailsView> AttendanceGroups
        {
            get { return attendanceGroups; }
            set { attendanceGroups = value; OnPropertyChanged("AttendanceGroups"); }
        }

        AttendanceGroupDetailsView currentAttendanceGroup;
        public AttendanceGroupDetailsView CurrentAttendanceGroup
        {
            get { return currentAttendanceGroup; }
            set
            {
                currentAttendanceGroup = value; OnPropertyChanged("CurrentAttendanceGroup");
                if (currentAttendanceGroup != null)
                {
                    this.RefreshGroupEmployees();
                }
            }
        }

        AttendanceGroupDetailsView removeAttendanceGroup;
        public AttendanceGroupDetailsView RemoveAttendanceGroup
        {
            get { return removeAttendanceGroup; }
            set { removeAttendanceGroup = value; OnPropertyChanged("RemoveAttendanceGroup"); }
        }

        List<AttendanceGroupDetailsView> selectedAttendanceGroups;
        public List<AttendanceGroupDetailsView> SelectedAttendanceGroups
        {
            get { return selectedAttendanceGroups; }
            set { selectedAttendanceGroups = value; OnPropertyChanged("SelectedAttendanceGroups"); }
        }

        IEnumerable<z_HolidayData> selectedHolidays;
        public IEnumerable<z_HolidayData> SelectedHolidays
        {
            get { return selectedHolidays; }
            set { selectedHolidays = value; OnPropertyChanged("SelectedHolidays"); }
        }

        z_HolidayData currentHoliday;
        public z_HolidayData CurrentHoliday
        {
            get { return currentHoliday; }
            set { currentHoliday = value; OnPropertyChanged("CurrentHoliday"); }
        }

        bool isCheckAll;
        public bool IsCheckAll
        {
            get { return isCheckAll; }
            set
            {
                isCheckAll = value; OnPropertyChanged("IsCheckAll");
                this.CheckEmployees();
            }
        }

        #endregion

        #region Refresh Methods

        void RefreshGroupEmployees()
        {
            attendService.GetEmployeeHolidayDetailsCompleted += (s, e) =>
            {
                try
                {
                    IEnumerable<ERP.AttendanceService.mas_Employee> dataList = e.Result;
                    if (dataList != null)
                    {
                        selectedGroupEmployees.Clear();
                        selectedGroupEmployees = dataList.ToList();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance group employees refresh is failed");
                    CurrentAttendanceGroup = null;
                }

            };
            attendService.GetEmployeeHolidayDetailsAsync(currentAttendanceGroup.attendance_group_id);
        }

        void RefreshAttendanceGroups()
        {
            attendService.GetAttendanceGroupsDetailsCompleted += (s, e) =>
            {
                try
                {
                    AttendanceGroups = e.Result;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance groups refresh is failed");
                }
            };
            attendService.GetAttendanceGroupsDetailsAsync();
        }

        #endregion

        #region Button Methods

        #region Add Button

        public ICommand AddButton
        {
            get { return new RelayCommand(AddGroup); }
        }

        private void AddGroup()
        {
            if (selectedGroupEmployees.Count > 0)
            {
                if (!selectedGroupsList.Any(c => selectedGroupEmployees.FirstOrDefault().dtl_AttendanceGroup.attendance_group_id == c.attendance_group_id))
                {
                    foreach (ERP.AttendanceService.mas_Employee emp in selectedGroupEmployees)
                    {
                        allGroupEmployeesList.Add(emp);
                    }
                    SelectedEmployees = null;
                    SelectedEmployees = allGroupEmployeesList;
                    selectedGroupEmployees.Clear();
                    this.FillGroupsListBox();
                }
            }
        }

        #endregion

        #region Assign Holiday Button

        public ICommand AssignHolidayButton
        {
            get { return new RelayCommand(AssignHoliday); }
        }

        private void AssignHoliday()
        {
            if (searchedHolidayList.Count > 0 && allGroupEmployeesList.Count(c => c.isdelete == true) > 0)
            {
                List<ERP.AttendanceService.mas_Employee> assignHolidayList = new List<ERP.AttendanceService.mas_Employee>();
                foreach (ERP.AttendanceService.mas_Employee grpEmp in allGroupEmployeesList.Where(c => c.isdelete == true))
                {
                    ERP.AttendanceService.mas_Employee assignedEmp = new ERP.AttendanceService.mas_Employee();
                    assignedEmp.employee_id = grpEmp.employee_id;
                    //assignedEmp.dtl_AttendanceGroup.attendance_group_id = grpEmp.dtl_AttendanceGroup.attendance_group_id;

                    List<z_HolidayData> newHolidays = searchedHolidayList.Where(c => !grpEmp.z_HolidayData.Any(d => d.holiday_id == c.holiday_id)).ToList();
                    List<z_HolidayData> addedList = new List<z_HolidayData>();
                    foreach (var addingHoliday in newHolidays)
                    {
                        z_HolidayData newHoliday = new z_HolidayData();
                        newHoliday.holiday_id = addingHoliday.holiday_id;
                        newHoliday.holiday_name = addingHoliday.holiday_name;
                        newHoliday.is_active = addingHoliday.is_active;
                        newHoliday.is_delete = addingHoliday.is_delete;
                        addedList.Add(newHoliday);
                    }

                    assignedEmp.z_HolidayData = addedList.ToArray();
                    assignHolidayList.Add(assignedEmp);
                }

                if (clsSecurity.GetSavePermission(309))
                {
                    if (attendService.AssignEmployeeHolidayDetails(assignHolidayList.ToArray()))
                    {
                        clsMessages.setMessage("Holidays are saved successfully");
                    }
                    else
                    {
                        clsMessages.setMessage("Holidays assign is failed");
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to save this form");
            }
            else
            {
                clsMessages.setMessage("Required data should be selected");
            }
        }

        #region New Code For Delete Employee Holiday

        public ICommand BtnDeleteEmployeeHoliday
        {
            get { return new RelayCommand(DeleteEmployeeHoliday); }
        }

        private void DeleteEmployeeHoliday()
        {
            if (CurrentHolidayEmployee != null && SelectedCurrentEmployeeHolidays != null)
            {
                if (attendService.RemoveIndividualEmployeeHoliday(CurrentHolidayEmployee.EmployeeID, SelectedCurrentEmployeeHolidays.holiday_id))
                {
                    RefreshSelectedEmployeeHolidays();
                    clsMessages.setMessage("Employee's Holiday Has Been Removed...");
                }
                else
                    clsMessages.setMessage("Employee's Holiday Removing Process Has Been Faild...");
            }
        }

        #endregion

        #endregion

        #region Remove Holiday Button

        public ICommand RemoveHolidayButton
        {
            get { return new RelayCommand(RemoveHoliday); }
        }

        private void RemoveHoliday()
        {
            if (searchedHolidayList.Count > 0 && allGroupEmployeesList.Count(c => c.isdelete == true) > 0)
            {
                List<ERP.AttendanceService.mas_Employee> removeHolidayList = new List<ERP.AttendanceService.mas_Employee>();
                foreach (ERP.AttendanceService.mas_Employee grpEmp in allGroupEmployeesList.Where(c => c.isdelete == true))
                {
                    ERP.AttendanceService.mas_Employee removedEmp = new ERP.AttendanceService.mas_Employee();
                    removedEmp.dtl_AttendanceGroup = new dtl_AttendanceGroup();
                    removedEmp.dtl_AttendanceGroup.attendance_group_id = grpEmp.dtl_AttendanceGroup.attendance_group_id;
                    removedEmp.employee_id = grpEmp.employee_id;
                    List<z_HolidayData> existHolidays = searchedHolidayList.Where(c => grpEmp.z_HolidayData.Any(d => d.holiday_id == c.holiday_id)).ToList();
                    List<z_HolidayData> removedList = new List<z_HolidayData>();
                    foreach (var removingHoliday in existHolidays)
                    {
                        z_HolidayData removedHoliday = new z_HolidayData();
                        removedHoliday.holiday_id = removingHoliday.holiday_id;
                        removingHoliday.holiday_name = removingHoliday.holiday_name;

                        removedList.Add(removingHoliday);
                    }
                    removedEmp.z_HolidayData = removedList.ToArray();
                    removeHolidayList.Add(removedEmp);
                }

                if (clsSecurity.GetDeletePermission(309))
                {
                    if (attendService.RemoveEmployeeHoliday(removeHolidayList.ToArray()))
                    {
                        clsMessages.setMessage("Holidays are successfully removed");
                    }
                    else
                    {
                        clsMessages.setMessage("Holidays remove is failed");
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to delete this form");
            }
        }

        #endregion

        #region Search Employee Button

        public ICommand SearchEmployeeButton
        {
            get { return new RelayCommand(SearchEmployee); }
        }

        private void SearchEmployee()
        {
            if (allGroupEmployeesList.Count > 0)
            {
                List<Guid> currentEmpIDList = allGroupEmployeesList.Select(c => c.employee_id).ToList();
                EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow(currentEmpIDList);
                window.ShowDialog();
                if (window.viewModel.SelectedList != null)
                {
                    searchedEmployeeList.Clear();
                    searchedEmployeeList = window.viewModel.SelectedList.ToList();
                    this.SetSearchedEmployees();
                }

                window.Close();
                window = null;
            }
            else
            {
                clsMessages.setMessage("At least one group required to be selected");
            }
        }

        #endregion

        #region Search Holiday Button

        public ICommand SearchHolidayButton
        {
            get { return new RelayCommand(SearchHoliday); }
        }

        private void SearchHoliday()
        {
            SearchHolidayWindow searchHolidayWindow = new SearchHolidayWindow(this.ownerWindow);
            searchHolidayWindow.ShowDialog();
            if (searchHolidayWindow.viewModel.selectedHolidayList.Count > 0)
            {
                searchedHolidayList.Clear();
                foreach (var item in searchHolidayWindow.viewModel.selectedHolidayList)
                {
                    z_HolidayData searchedHoliday = new z_HolidayData();
                    searchedHoliday.holiday_id = item.holiday_id;
                    searchedHoliday.holiday_name = item.holiday_name;
                    searchedHoliday.holiday_start = item.holiday_start;
                    searchedHoliday.holiday_end = item.holiday_end;
                    searchedHoliday.is_active = item.is_active;
                    searchedHolidayList.Add(searchedHoliday);
                }

                SelectedHolidays = null;
                SelectedHolidays = searchedHolidayList;
            }
            searchHolidayWindow.Close();
        }

        #endregion

        #region Clear Holiday Button

        public ICommand ClearHolidayButton
        {
            get { return new RelayCommand(clearHolidays); }
        }

        void clearHolidays()
        {
            if (searchedHolidayList.Count > 0)
            {
                searchedHolidayList.Clear();
                SelectedHolidays = null;
            }
        }

        #endregion

        #region View Holiday Button

        public ICommand ViewButton
        {
            get { return new RelayCommand(PreviewEmployeeHoliday); }
        }

        private void PreviewEmployeeHoliday()
        {
            if (allGroupEmployeesList.Count > 0)
            {
                PreviewEmployeeHolidayWindow viewHolidayWindow = new PreviewEmployeeHolidayWindow(this);
                viewHolidayWindow.ParentWindow = ownerWindow;
                SearchStartDate = DateTime.Today;
                SearchEndDate = DateTime.Today;
                IsSearchAll = true;
                this.FillHolidayEmployees();
                viewHolidayWindow.ShowDialog();
                if (viewHolidayWindow != null)
                    viewHolidayWindow.Close();
            }
        }

        #endregion

        #endregion

        #region Data Setting Methods

        #region Group Selection Methods

        void FillGroupsListBox()
        {
            if (currentAttendanceGroup != null && selectedGroupsList.Count(c => c.attendance_group_id == currentAttendanceGroup.attendance_group_id) == 0)
            {
                selectedGroupsList.Add(currentAttendanceGroup);
                SelectedAttendanceGroups = null;
                SelectedAttendanceGroups = selectedGroupsList;
            }
        }

        public void RemoveSelectedGroup()
        {
            if (removeAttendanceGroup != null)
            {
                selectedGroupsList.Remove(removeAttendanceGroup);
                allGroupEmployeesList = allGroupEmployeesList.Except(allGroupEmployeesList.Where(c => c.dtl_AttendanceGroup.attendance_group_id == removeAttendanceGroup.attendance_group_id)).ToList();
                SelectedEmployees = null;
                SelectedEmployees = allGroupEmployeesList;
                SelectedAttendanceGroups = null;
                SelectedAttendanceGroups = selectedGroupsList;
                CurrentAttendanceGroup = null;
            }
        }

        #endregion

        #region Checkbox Selection

        void CheckEmployees()
        {
            if (selectEmpList.Count > 0)
            {
                foreach (ERP.AttendanceService.mas_Employee item in selectEmpList)
                {
                    ERP.AttendanceService.mas_Employee current = allGroupEmployeesList.FirstOrDefault(c => c.employee_id == item.employee_id);
                    current.isdelete = isCheckAll;
                }
            }
        }

        #endregion

        #region Search Employee Methods

        void SetSearchedEmployees()
        {
            if (searchedEmployeeList != null)
            {
                var unselectedList = allGroupEmployeesList.Where(c => c.dtl_AttendanceGroup.attendance_group_id == currentAttendanceGroup.attendance_group_id && !searchedEmployeeList.Any(d => d.employee_id == c.employee_id));
                foreach (var emp in unselectedList)
                {
                    var current = allGroupEmployeesList.FirstOrDefault(c => c.employee_id == emp.employee_id);
                    current.isdelete = false;
                }
                foreach (var emp in searchedEmployeeList)
                {
                    var current = allGroupEmployeesList.FirstOrDefault(c => c.employee_id == emp.employee_id);
                    current.isdelete = true;
                }
            }
        }

        #endregion

        #endregion

        #region View Holiday Section

        #region Data Members

        List<HolidayEmployee> holidayEmployeeList = new List<HolidayEmployee>();

        #endregion

        #region Properties

        IEnumerable<HolidayEmployee> selectedHolidayEmployees;
        public IEnumerable<HolidayEmployee> SelectedHolidayEmployees
        {
            get { return selectedHolidayEmployees; }
            set { selectedHolidayEmployees = value; OnPropertyChanged("SelectedHolidayEmployees"); }
        }

        HolidayEmployee currentHolidayEmployee;
        public HolidayEmployee CurrentHolidayEmployee
        {
            get { return currentHolidayEmployee; }
            set
            {
                currentHolidayEmployee = value; OnPropertyChanged("CurrentHolidayEmployee");
                if (currentHolidayEmployee != null)
                    this.FillEmployeeHolidays();
            }
        }

        IEnumerable<z_HolidayData> currentEmployeeHolidays;
        public IEnumerable<z_HolidayData> CurrentEmployeeHolidays
        {
            get { return currentEmployeeHolidays; }
            set { currentEmployeeHolidays = value; OnPropertyChanged("CurrentEmployeeHolidays"); }
        }

        private z_HolidayData _SelectedCurrentEmployeeHolidays;

        public z_HolidayData SelectedCurrentEmployeeHolidays
        {
            get { return _SelectedCurrentEmployeeHolidays; }
            set { _SelectedCurrentEmployeeHolidays = value; OnPropertyChanged("SelectedCurrentEmployeeHolidays"); }
        }

        //private trns_EmployeeHoliday myVar;

        //public trns_EmployeeHoliday MyProperty
        //{
        //    get { return myVar; }
        //    set { myVar = value; }
        //}



        DateTime searchStartDate;
        public DateTime SearchStartDate
        {
            get { return searchStartDate; }
            set { searchStartDate = value; OnPropertyChanged("SearchStartDate"); }
        }

        DateTime searchEndDate;
        public DateTime SearchEndDate
        {
            get { return searchEndDate; }
            set { searchEndDate = value; OnPropertyChanged("SearchEndDate"); }
        }

        string searchYear;
        public string SearchYear
        {
            get { return searchYear; }
            set { searchYear = value; OnPropertyChanged("SearchYear"); }
        }

        bool isSearchAll;
        public bool IsSearchAll
        {
            get { return isSearchAll; }
            set { isSearchAll = value; OnPropertyChanged("IsSearchAll"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshSelectedEmployeeHolidays()
        {
            attendService.GetEmployeeHolidayDetailsByEmployeeCompleted += (s, e) =>
            {
                IEnumerable<ERP.AttendanceService.mas_Employee> refreshEmployees = e.Result;
                if (refreshEmployees != null)
                {
                    foreach (ERP.AttendanceService.mas_Employee empItem in refreshEmployees)
                    {
                        HolidayEmployee currentEmp = holidayEmployeeList.FirstOrDefault(c => c.EmployeeID == empItem.employee_id);
                        if (currentEmp != null)
                        {
                            currentEmp.AssignedHolidays.Clear();
                            if (empItem.z_HolidayData != null)
                            {
                                foreach (var holidayItem in empItem.z_HolidayData)
                                {
                                    currentEmp.AssignedHolidays.Add(holidayItem);
                                }
                            }
                        }
                    }
                    CurrentEmployeeHolidays = null;
                    SelectedHolidayEmployees = null;
                    SelectedHolidayEmployees = holidayEmployeeList;
                }
            };

            attendService.GetEmployeeHolidayDetailsByEmployeeAsync(holidayEmployeeList.Select(c => c.EmployeeID).ToArray());
        }

        #endregion

        #region Search Employee Holiday Button

        public ICommand SearchEmployeeHolidayButton
        {
            get { return new RelayCommand(SearchCurrentHolidays); }
        }

        private void SearchCurrentHolidays()
        {
            if (currentHolidayEmployee != null)
            {
                List<z_HolidayData> searchingHolidayList = currentHolidayEmployee.AssignedHolidays;
                if (searchingHolidayList.Count > 0)
                {
                    CurrentEmployeeHolidays = searchingHolidayList;
                    if (!isSearchAll)
                    {
                        int year;
                        Int32.TryParse(searchYear, out year);
                        if (year != 0)
                        {
                            CurrentEmployeeHolidays = currentEmployeeHolidays.Where(c => c.holiday_start.Value.Year == year);
                        }

                        if (searchStartDate != null && searchEndDate != null && searchStartDate <= searchEndDate)
                        {
                            CurrentEmployeeHolidays = currentEmployeeHolidays.Where(c => c.holiday_start.Value.Date >= searchStartDate.Date && c.holiday_start.Value.Date <= searchEndDate.Date);
                        }
                    }
                }
            }
        }

        #endregion

        #region Refresh Employee Holiday Buttton

        public ICommand RefreshHolidayButton
        {
            get { return new RelayCommand(refreshCurrentHolidays); }
        }

        private void refreshCurrentHolidays()
        {
            this.RefreshSelectedEmployeeHolidays();
        }

        #endregion

        #region Data Setting Methods

        void FillHolidayEmployees()
        {
            holidayEmployeeList.Clear();
            foreach (ERP.AttendanceService.mas_Employee empItem in allGroupEmployeesList)
            {
                HolidayEmployee holidayEmp = new HolidayEmployee();
                holidayEmp.EmployeeID = empItem.employee_id;
                holidayEmp.EmpID = empItem.emp_id;
                holidayEmp.FirstName = empItem.first_name;
                holidayEmp.SecondName = empItem.second_name;
                holidayEmp.AssignedHolidays = empItem.z_HolidayData == null ? new List<z_HolidayData>() : empItem.z_HolidayData.ToList();
                holidayEmployeeList.Add(holidayEmp);
            }

            SelectedHolidayEmployees = null;
            SelectedHolidayEmployees = holidayEmployeeList;
        }

        void FillEmployeeHolidays()
        {
            if (currentHolidayEmployee.AssignedHolidays.Count > 0)
                CurrentEmployeeHolidays = currentHolidayEmployee.AssignedHolidays;
        }

        #endregion

        #region DeleteButton For PreviewEmployeeHolidayWindow

        private void Delete()
        {
            try
            {
                if (clsSecurity.GetDeletePermission(309))
                {
                    if (SelectedCurrentEmployeeHolidays != null)
                    {
                        z_HolidayData DeleteingAttendanceData = new z_HolidayData();
                        DeleteingAttendanceData.holiday_id = SelectedCurrentEmployeeHolidays.holiday_id;
                        DeleteingAttendanceData.holiday_name = SelectedCurrentEmployeeHolidays.holiday_name;
                        DeleteingAttendanceData.holiday_start = SelectedCurrentEmployeeHolidays.holiday_start;
                        DeleteingAttendanceData.holiday_end = SelectedCurrentEmployeeHolidays.holiday_end;
                        //     DeleteingAttendanceData.

                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to delete this form");
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error in deleting holiday");
            }
            //    if (currentSelectedEmployeeAttendance != null)
            //    {
            //        dtl_AttendanceData deletingAttendance = new dtl_AttendanceData();
            //        deletingAttendance.attendance_data_id = currentSelectedEmployeeAttendance.attendance_data_id;
            //        deletingAttendance.device_id = currentSelectedEmployeeAttendance.device_id;
            //        deletingAttendance.emp_id = currentSelectedEmployeeAttendance.emp_id;
            //        deletingAttendance.delete_datetime = DateTime.Now;
            //        deletingAttendance.delete_user_id = clsSecurity.loggedUser.user_id;
            //        deletingAttendance.isdelete = true;

            //        if (serviceClient.DeleteEmployeeCurrentAttendance(deletingAttendance))
            //        {
            //            clsMessages.setMessage("Employee attendance deleted successfully");
            //            this.FindAttendance();
            //        }
            //        else
            //        {
            //            clsMessages.setMessage("Employee attendance delete is failed");
            //        }
            //    }
            //    else
            //    {
            //        clsMessages.setMessage("Employee attendance should be selected");
            //    }
            //}
        }
        #endregion

        #endregion
    }

    public class HolidayEmployee
    {
        Guid employeeID;
        public Guid EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        string empID;
        public string EmpID
        {
            get { return empID; }
            set { empID = value; }
        }

        string firstName;
        public string FirstName
        {
            set { firstName = value; }
        }

        string secondName;
        public string SecondName
        {
            set { secondName = value; }
        }

        public string FullName
        {
            get { return (firstName == null ? "" : firstName) + " " + (secondName == null ? "" : secondName); }
        }

        int holidayCount;
        public int HolidayCount
        {
            get { return holidayCount; }
            set { holidayCount = value; }
        }

        int sundayHolidayCount;
        public int SundayHolidayCount
        {
            get { return sundayHolidayCount; }
            set { sundayHolidayCount = value; }
        }

        List<z_HolidayData> assignedHolidays;
        public List<z_HolidayData> AssignedHolidays
        {
            get { return assignedHolidays; }
            set { assignedHolidays = value; }
        }
    }
}
