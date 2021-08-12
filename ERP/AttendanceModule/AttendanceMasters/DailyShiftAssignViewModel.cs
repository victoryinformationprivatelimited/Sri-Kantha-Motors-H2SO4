using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.AttendanceService;
using ERP.BasicSearch;
using ERP.ERPService;
using System.Windows.Input;
using System.Collections;
using System.Windows;

namespace ERP.AttendanceModule.AttendanceMasters
{
    public class DailyShiftAssignViewModel : ViewModelBase
    {
        #region Attendance Service Client

        AttendanceServiceClient attendService;

        #endregion

        #region Constructor

        public DailyShiftAssignViewModel()
        {
            attendService = new AttendanceServiceClient();
            this.SetDefaultDays();
            this.RefreshAttendanceGroups();
            this.RefreshShiftWeeks();
            IsCheckAll = true;
            IsRangeDateSelect = true;
        }

        #endregion

        #region Data Members

        ShiftMasterDetailsView selectedShift;
        public DailyShiftAssignWindow viewModelUI;

        #region IList

        IList selectDayList = new ArrayList();
        IList selectEmpList = new ArrayList();

        #endregion

        #endregion

        #region List Members

        List<trns_EmployeeDailyShiftDetails> addingList = new List<trns_EmployeeDailyShiftDetails>();
        List<AttendanceGroupDetailsView> selectedGroupsList = new List<AttendanceGroupDetailsView>();
        List<AttendanceGroupWithEmployee> allGroupEmployeesList = new List<AttendanceGroupWithEmployee>();
        List<AttendanceGroupWithEmployee> selectedGroupEmployees = new List<AttendanceGroupWithEmployee>();
        List<ShiftWeekWithDaysView> weekDays = new List<ShiftWeekWithDaysView>();
        List<EmployeeSearchView> searchedEmployeeList = new List<EmployeeSearchView>();
        List<WeekDayShift> currentShiftDaysList = new List<WeekDayShift>();

        #endregion

        #region Properties

        List<AttendanceGroupWithEmployee> selectedEmployees;
        public List<AttendanceGroupWithEmployee> SelectedEmployees
        {
            get { if (selectedEmployees != null) selectedEmployees = selectedEmployees.OrderBy(c => Convert.ToInt32(c.emp_id)).ToList(); return selectedEmployees; }
            set { selectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
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
            set
            {
                removeAttendanceGroup = value; OnPropertyChanged("RemoveAttendanceGroup");
            }
        }

        IEnumerable<ShiftWeekWithDaysView> selectedShiftWeekDays;
        public IEnumerable<ShiftWeekWithDaysView> SelectedShiftWeekDays
        {
            get { return selectedShiftWeekDays; }
            set { selectedShiftWeekDays = value; OnPropertyChanged("SelectedShiftWeekDays"); }
        }

        IEnumerable<WeekDayShift> shiftDays;
        public IEnumerable<WeekDayShift> ShiftDays
        {
            get { if (shiftDays != null) shiftDays = shiftDays.OrderBy(c => c.DateOfDay.Date); return shiftDays; }
            set { shiftDays = value; OnPropertyChanged("ShiftDays"); }
        }

        IEnumerable<z_ShiftWeek> shiftWeeks;
        public IEnumerable<z_ShiftWeek> ShiftWeeks
        {
            get { return shiftWeeks; }
            set { shiftWeeks = value; OnPropertyChanged("ShiftWeeks"); }
        }

        z_ShiftWeek currentShiftWeek;
        public z_ShiftWeek CurrentShiftWeek
        {
            get { return currentShiftWeek; }
            set
            {
                currentShiftWeek = value; OnPropertyChanged("CurrentShiftWeek");
                if (currentShiftWeek != null)
                    this.RefreshShiftWeekDetails();
            }
        }

        public IList SelectDayList
        {
            get { return selectDayList; }
            set { selectDayList = value; OnPropertyChanged("SelectDayList"); }
        }

        public IList SelectEmpList
        {
            get { return selectEmpList; }
            set { selectEmpList = value; OnPropertyChanged("SelectEmpList"); }
        }

        DateTime? fromDate;
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { fromDate = value; OnPropertyChanged("FromDate"); }
        }

        DateTime? toDate;
        public DateTime? ToDate
        {
            get { return toDate; }
            set { toDate = value; OnPropertyChanged("ToDate"); }
        }

        bool isDailyShift;
        public bool IsDailyShift
        {
            get { return isDailyShift; }
            set { isDailyShift = value; OnPropertyChanged("IsDailyShift"); }
        }

        bool isRosterShift;
        public bool IsRosterShift
        {
            get { return isRosterShift; }
            set { isRosterShift = value; OnPropertyChanged("IsRosterShift"); }
        }

        bool isBothShift;
        public bool IsBothShift
        {
            get { return isBothShift; }
            set { isBothShift = value; OnPropertyChanged("IsBothShift"); }
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

        bool isRemoveDuplicates;
        public bool IsRemoveDuplicates
        {
            get { return isRemoveDuplicates; }
            set { isRemoveDuplicates = value; OnPropertyChanged("IsRemoveDuplicates"); }
        }

        bool isRangeDateSelect;
        public bool IsRangeDateSelect
        {
            get { return isRangeDateSelect; }
            set
            {
                isRangeDateSelect = value; OnPropertyChanged("IsRangeDateSelect");
                if (isRangeDateSelect)
                    IsToDateEnabled = true;
                else
                    IsToDateEnabled = false;
            }
        }

        bool isToDateEnabled;
        public bool IsToDateEnabled
        {
            get { return isToDateEnabled; }
            set { isToDateEnabled = value; OnPropertyChanged("IsToDateEnabled"); }
        }

        List<AttendanceGroupDetailsView> selectedAttendanceGroups;
        public List<AttendanceGroupDetailsView> SelectedAttendanceGroups
        {
            get { return selectedAttendanceGroups; }
            set { selectedAttendanceGroups = value; OnPropertyChanged("SelectedAttendanceGroups"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshGroupEmployees()
        {
            attendService.GetAttendanceGroupWithEmployeesDetailsCompleted += (s, e) =>
            {
                try
                {
                    IEnumerable<AttendanceGroupWithEmployee> dataList = e.Result;
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
            if (!selectedGroupsList.Any(c => c.attendance_group_id == currentAttendanceGroup.attendance_group_id))
            {
                attendService.GetAttendanceGroupWithEmployeesDetailsAsync(currentAttendanceGroup.attendance_group_id);
            }
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

        void RefreshShiftWeekDetails()
        {
            attendService.GetShiftWeekWithDaysByWeekCompleted += (s, e) =>
            {
                try
                {
                    SelectedShiftWeekDays = e.Result;
                    if (selectedShiftWeekDays != null)
                        this.SetShiftDayByShiftWeek();
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Shift week details refresh is failed");
                    CurrentShiftWeek = null;
                }
            };

            attendService.GetShiftWeekWithDaysByWeekAsync(currentShiftWeek.week_id);
        }

        void RefreshShiftWeeks()
        {
            attendService.GetShiftWeeksCompleted += (s, e) =>
            {
                try
                {
                    ShiftWeeks = e.Result;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Shift weeks refresh is failed");
                }
            };
            attendService.GetShiftWeeksAsync();
        }

        #endregion

        #region Button Methods

        #region Select Employee Buttons

        void SelectEmployee()
        {
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            if (window.viewModel.SelectedList != null)
            {
                searchedEmployeeList.Clear();
                searchedEmployeeList = window.viewModel.SelectedList.ToList();
                if (searchedEmployeeList.Count > 0)
                {
                    searchedEmployeeList.Where(c => !allGroupEmployeesList.Any(d => d.employee_id == c.employee_id)).ToList().ForEach(c => allGroupEmployeesList.Add(new AttendanceGroupWithEmployee { emp_id = c.emp_id, employee_id = c.employee_id, first_name = c.first_name, second_name = c.second_name, is_active = true, attendance_group_id = c.attendance_group_id==null?0:c.attendance_group_id.Value }));
                    SelectedEmployees = null;
                    SelectedEmployees = allGroupEmployeesList;
                }
            }

            window.Close();
            window = null;
        }

        public ICommand SelectEmployeeButton
        {
            get { return new RelayCommand(SelectEmployee); }
        }

        #endregion

        #region Select Shift Button

        void SelectShift()
        {
            if (viewModelUI != null)
            {
                SearchShiftWindow shiftWindow = new SearchShiftWindow(viewModelUI);
                shiftWindow.ShowDialog();
                if (shiftWindow.viewModel.CurrentShiftDetail != null)
                {
                    selectedShift = shiftWindow.viewModel.CurrentShiftDetail;
                    dtl_Shift_Master current = new dtl_Shift_Master();
                    current.shift_detail_id = selectedShift.shift_detail_id;
                    current.shift_detail_name = selectedShift.shift_detail_name;
                    this.SetShiftDay(current);
                }
                shiftWindow.Close();
            }
        }

        public ICommand SelectShiftButton
        {
            get { return new RelayCommand(SelectShift); }
        }

        #endregion

        #region Assign Shift Button

        void AssignShift()
        {
            try
            {
                if (clsSecurity.GetSavePermission(306))
                {
                    bool noBreakSelection = false;
                    bool isBreakCancel = false;
                    this.FillShiftBreakEmployees();
                    if (MessageBoxResult.Yes == MessageBox.Show("Do you want to set break options for assigning shifts", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information))
                    {
                        AssignEmployeeBreakWindow empBreakWindow = new AssignEmployeeBreakWindow(this);
                        empBreakWindow.ShowDialog();
                        if (empBreakWindow.DialogResult.HasValue)
                        {
                            if (empBreakWindow.DialogResult.Value)
                            {
                                MessageBox.Show("Break options are set. Continue with saving....", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                isBreakCancel = true;
                            }
                        }
                        empBreakWindow = null;
                    }
                    else
                    {
                        noBreakSelection = true;
                    }

                    if (!isBreakCancel)
                    {
                        List<trns_EmployeeDailyShiftDetails> dailyShiftList = new List<trns_EmployeeDailyShiftDetails>();

                        foreach (ShiftEmployee currentEmp in breakAssignEmpList)
                        {
                            foreach (EmployeeAssignedShift selectShift in currentEmp.EmpShifts)
                            {
                                trns_EmployeeDailyShiftDetails item = new trns_EmployeeDailyShiftDetails();
                                item.employee_id = currentEmp.EmployeeID;
                                item.shift_detail_id = selectShift.ShiftID;
                                item.date = selectShift.DateOfDay;
                                item.attendance_group_id = currentEmp.AttendanceGroupID;
                                item.day_id = selectShift.DayID;
                                if (noBreakSelection)
                                    item.trns_EmployeeShiftBreakStatus = new trns_EmployeeShiftBreakStatus { is_shift_break = false, is_free_break = false, is_no_break = true };
                                else
                                    item.trns_EmployeeShiftBreakStatus = new trns_EmployeeShiftBreakStatus { is_shift_break = selectShift.IsShiftBreak, is_free_break = selectShift.IsFreeBreak, is_no_break = selectShift.IsNoShift };
                                dailyShiftList.Add(item);
                            }
                            if (dailyShiftList.Count > 0)
                            {
                                if (!attendService.SaveEmployeeDailyShiftDetails(dailyShiftList.ToArray(), isRemoveDuplicates))
                                {
                                    clsMessages.setMessage("Shift details of Emp No: " + currentEmp.EmpID + " not saved correctly");
                                }
                                dailyShiftList.Clear();
                            }
                        }

                        clsMessages.setMessage("Shift details saving completed");
                        //allGroupEmployeesList.Clear();
                        FromDate = null;
                        ToDate = null;

                    }
                    else
                    {
                        clsMessages.setMessage("User skip the required selection, Please try again");
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Save this record(s)");
            }
            catch (Exception)
            {
                clsMessages.setMessage("Saving operation cannot be proceed...");
            }

        }

        public ICommand AssignShiftButton
        {
            get { return new RelayCommand(AssignShift); }
        }

        #endregion

        #region Get Employee Button

        void Get()
        {
            if (selectedGroupEmployees.Count > 0)
            {
                //if (!selectedGroupsList.Any(c => selectedGroupEmployees.FirstOrDefault().attendance_group_id == c.attendance_group_id))
                //{
                foreach (AttendanceGroupWithEmployee emp in selectedGroupEmployees.Where(c => !allGroupEmployeesList.Any(d => d.employee_id == c.employee_id)))
                {
                    allGroupEmployeesList.Add(emp);
                }
                SelectedEmployees = null;
                SelectedEmployees = allGroupEmployeesList;
                selectedGroupEmployees.Clear();
                this.FillGroupsListBox();
                //}
            }
        }

        public ICommand GetButton
        {
            get { return new RelayCommand(Get); }
        }

        #endregion

        #region Set Dates Button

        public ICommand SetDateButton
        {
            get { return new RelayCommand(SetShiftDates); }
        }

        void SetShiftDates()
        {
            if (fromDate != null)
            {
                SelectDayList = null;
                //currentShiftDaysList.Clear();
                if (isRangeDateSelect)
                {
                    if (toDate >= fromDate)
                    {
                        int incr = 0;
                        DateTime changedDate = fromDate.Value;
                        while (toDate.Value.Date >= changedDate.Date)
                        {
                            if (!currentShiftDaysList.Any(c => c.DateOfDay.Date == changedDate.Date))
                            {
                                WeekDayShift selection = new WeekDayShift();
                                selection.DateOfDay = changedDate;
                                currentShiftDaysList.Add(selection);
                            }
                            incr++;
                            changedDate = fromDate.Value.Date.AddDays(incr);
                        }
                    }
                }
                else
                {
                    if (!currentShiftDaysList.Any(c => c.DateOfDay.Date == fromDate.Value.Date))
                    {
                        WeekDayShift selection = new WeekDayShift();
                        selection.DateOfDay = fromDate.Value;
                        currentShiftDaysList.Add(selection);
                    }
                }

                ShiftDays = null;
                ShiftDays = currentShiftDaysList;
            }
        }

        #endregion

        #region Clear Dates Button

        public ICommand ClearDateButton
        {
            get { return new RelayCommand(ClearDates); }
        }

        private void ClearDates()
        {
            if (selectDayList != null && selectDayList.Count > 0)
            {
                foreach (WeekDayShift item in selectDayList)
                {
                    currentShiftDaysList.Remove(item);
                }
                ShiftDays = null;
                ShiftDays = currentShiftDaysList;
                SelectDayList = null;
            }
            else
            {
                clsMessages.setMessage("No selection of dates has been made");
            }
        }

        #endregion

        #region Preview Button

        public ICommand PreviewShiftButton
        {
            get { return new RelayCommand(PreviewShifts); }
        }

        private void PreviewShifts()
        {
            AssignedShiftPreviewWindow prvWindow = new AssignedShiftPreviewWindow();
            prvWindow.ShiftDays = shiftDays;
            prvWindow.ShowDialog();
            prvWindow.Close();
        }

        #endregion

        #region View Shifts Button

        public ICommand ViewShiftsButton
        {
            get { return new RelayCommand(ViewShifts); }
        }

        private void ViewShifts()
        {
            if (selectedEmployees != null && selectedEmployees.Count > 0 && fromDate != null && toDate != null)
            {
                EmployeeWorkSchedulePreviewWindow assignedScheduleWindow = new EmployeeWorkSchedulePreviewWindow(this);
                assignedScheduleWindow.parentWindow = viewModelUI;
                RefreshStartDate = fromDate.Value;
                RefreshEndDate = toDate.Value;
                this.FillShiftEmployees();
                this.InitializeShiftDateRange();
                assignedScheduleWindow.ShowDialog();
            }
        }

        #endregion

        #endregion

        #region Data Setting Methods

        void SetShiftDay(dtl_Shift_Master shft)
        {
            if (selectDayList != null && selectDayList.Count > 0)
            {
                foreach (WeekDayShift item in selectDayList)
                {
                    WeekDayShift current = currentShiftDaysList.FirstOrDefault(c => c.DateOfDay == item.DateOfDay);
                    current.Shift = shft;
                }
                ShiftDays = null;
                ShiftDays = currentShiftDaysList;
            }
        }

        void SetShiftDayByShiftWeek()
        {
            foreach (WeekDayShift item in selectDayList)
            {
                WeekDayShift current = currentShiftDaysList.FirstOrDefault(c => c.DateOfDay == item.DateOfDay);
                ShiftWeekWithDaysView dayShift = selectedShiftWeekDays.FirstOrDefault(c => c.day_id.ToString() == current.Day.ToString());
                if (dayShift != null)
                {
                    dtl_Shift_Master selectedShift = new dtl_Shift_Master();
                    selectedShift.shift_detail_id = (int)dayShift.shift_detail_id;
                    selectedShift.shift_detail_name = dayShift.shift_detail_name;
                    current.Shift = selectedShift;
                    current.DayID = (int)dayShift.day_id;
                }
                else
                {
                    current.Shift = null;
                }
            }
            ShiftDays = null;
            ShiftDays = currentShiftDaysList;
            CurrentShiftWeek = null;
        }

        void SetDefaultDays()
        {
            List<ShiftWeekWithDaysView> defaultDays = new List<ShiftWeekWithDaysView>() { 
                    new ShiftWeekWithDaysView{day_id=Days.Monday},
                    new ShiftWeekWithDaysView{day_id=Days.Tuesday},
                    new ShiftWeekWithDaysView{day_id=Days.Wednesday},
                    new ShiftWeekWithDaysView{day_id=Days.Thursday},
                    new ShiftWeekWithDaysView{day_id=Days.Friday},
                    new ShiftWeekWithDaysView{day_id=Days.Saturday},
                    new ShiftWeekWithDaysView{day_id=Days.Sunday}
                };
            //SelectedShiftWeekDays = defaultDays;
        }

        #endregion

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
                allGroupEmployeesList = allGroupEmployeesList.Except(allGroupEmployeesList.Where(c => c.attendance_group_id == removeAttendanceGroup.attendance_group_id)).ToList();
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
                foreach (AttendanceGroupWithEmployee item in selectEmpList)
                {
                    AttendanceGroupWithEmployee current = allGroupEmployeesList.FirstOrDefault(c => c.employee_id == item.employee_id);
                    current.is_active = isCheckAll;
                }
            }
        }

        #endregion

        #region Preview Shift Window

        #region List Members

        List<EmployeeAssignedShiftsView> assignedShiftList = new List<EmployeeAssignedShiftsView>();
        List<EmployeeAssignedShift> employeeShiftList = new List<EmployeeAssignedShift>();
        List<ShiftEmployee> shiftAssignEmpList = new List<ShiftEmployee>();

        #endregion

        #region Properties

        IEnumerable<EmployeeAssignedShift> employeeAssignedShifts;
        public IEnumerable<EmployeeAssignedShift> EmployeeAssignedShifts
        {
            get { return employeeAssignedShifts; }
            set { employeeAssignedShifts = value; OnPropertyChanged("EmployeeAssignedShifts"); }
        }

        EmployeeAssignedShift currentAssignedShift;
        public EmployeeAssignedShift CurrentAssignedShift
        {
            get { return currentAssignedShift; }
            set { currentAssignedShift = value; OnPropertyChanged("CurrentAssignedShift"); }
        }

        IEnumerable<ShiftEmployee> shiftEmployees;
        public IEnumerable<ShiftEmployee> ShiftEmployees
        {
            get { return shiftEmployees; }
            set { shiftEmployees = value; OnPropertyChanged("ShiftEmployees"); }
        }

        ShiftEmployee currentShiftEmployee;
        public ShiftEmployee CurrentShiftEmployee
        {
            get { return currentShiftEmployee; }
            set
            {
                currentShiftEmployee = value; OnPropertyChanged("CurrentShiftEmployee");
                //if (currentShiftEmployee != null)
                //    this.FillEmployeeAssignedShifts();
            }
        }

        DateTime refreshStartDate;
        public DateTime RefreshStartDate
        {
            get { return refreshStartDate; }
            set { refreshStartDate = value; OnPropertyChanged("RefreshStartDate"); }
        }

        DateTime refreshEndDate;
        public DateTime RefreshEndDate
        {
            get { return refreshEndDate; }
            set { refreshEndDate = value; OnPropertyChanged("RefreshEndDate"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshAssignedShifts(List<Guid> empList)
        {
            if (refreshStartDate != null && refreshEndDate != null)
            {
                //attendService.GetEmployeeAssignedShiftDetailsCompleted += (s, e) =>
                //        {
                            IEnumerable<EmployeeAssignedShiftsView> dataList = attendService.GetEmployeeAssignedShiftDetails(empList.ToArray(), refreshStartDate, refreshEndDate);
                            if (dataList != null)
                            {
                                assignedShiftList.Clear();
                                assignedShiftList = dataList.ToList();
                                InitializeShiftDateRange();
                                InitializeShiftEmployees();
                                FillAllEmployeeAssignedShifts();
                            }
                //        };
                //attendService.GetEmployeeAssignedShiftDetailsAsync(empList.ToArray(), refreshStartDate, refreshEndDate);
            }
        }

        #endregion

        #region Button Methods

        #region Delete Shift Button

        public void DeleteAssignedShift()
        {
            try
            {
                if (clsSecurity.GetDeletePermission(306))
                {
                    if (currentAssignedShift != null && currentAssignedShift.TransactID > 0)
                    {
                        trns_EmployeeDailyShiftDetails deletingShift = new trns_EmployeeDailyShiftDetails();
                        deletingShift.trns_id = currentAssignedShift.TransactID;
                        if (attendService.DeleteAssignedShift(deletingShift))
                        {
                            // remove from user defined EmployeeAssignedShift list
                            //var current = employeeShiftList.FirstOrDefault(c => c.TransactID == deletingShift.trns_id);
                            //employeeShiftList.Remove(current);

                            // remove from entity type EmployeeAssignedShiftsView list
                            var assignShift = assignedShiftList.FirstOrDefault(c => c.trns_id == deletingShift.trns_id);
                            assignedShiftList.Remove(assignShift);

                            this.InitializeShiftEmployees();
                            CurrentShiftEmployee = shiftAssignEmpList.FirstOrDefault(c => c.EmployeeID == assignShift.employee_id);
                            this.FillEmployeeAssignedShifts();
                            CurrentAssignedShift = null;
                        }
                    } 
                }
                else
                    clsMessages.setMessage("You don't have permission to delete this form");
            }
            catch (Exception)
            {
                clsMessages.setMessage("Assigned shift delete cannot be proceed");
            }
        } 

        #endregion

        #region Refresh Button

        public ICommand RefreshButton
        {
            get { return new RelayCommand(RefreshShifts); }
        }

        private void RefreshShifts()
        {
            List<Guid> employeeList = selectedEmployees.Select(c => c.employee_id).ToList();
            if (employeeList != null && employeeList.Count > 0)
            {
                this.RefreshAssignedShifts(employeeList);
            }
        }

        #endregion

        #region Update Break Button

        public ICommand UpdateBreaksButton
        {
            get { return new RelayCommand(updateBreakTimeStatus); }
        }

        private void updateBreakTimeStatus()
        {
            AssignEmployeeBreakWindow updateBreakWindow = null;
            try
            {   
                if (assignedShiftList.Count > 0)
                {
                    if (clsSecurity.GetUpdatePermission(306))
                    {
                        FromBreakDate = fromDate.Value;
                        ToBreakDate = toDate.Value;
                        updateBreakWindow = new AssignEmployeeBreakWindow(this);
                        ShiftBreakEmployees = shiftEmployees.ToList();
                        updateBreakWindow.ShowDialog();
                        if (updateBreakWindow.DialogResult.HasValue)
                        {
                            if (updateBreakWindow.DialogResult.Value)
                            {
                                // User intend to update break options
                                List<trns_EmployeeShiftBreakStatus> updatingList = new List<trns_EmployeeShiftBreakStatus>();
                                foreach (ShiftEmployee brkEmp in shiftBreakEmployees)
                                {
                                    foreach (var currentShift in brkEmp.EmpShifts.Where(c => c.IsNoShift == false))
                                    {
                                        trns_EmployeeShiftBreakStatus updatingBreak = new trns_EmployeeShiftBreakStatus();
                                        updatingBreak.trns_id = currentShift.TransactID;
                                        updatingBreak.is_free_break = currentShift.IsFreeBreak;
                                        updatingBreak.is_no_break = currentShift.IsNoBreak;
                                        updatingBreak.is_shift_break = currentShift.IsShiftBreak;

                                        updatingList.Add(updatingBreak);
                                    }
                                }

                                if (updatingList.Count > 0)
                                {
                                    // updating changes to database
                                    trns_EmployeeShiftBreakStatus[] updArray = updatingList.ToArray();
                                    if (attendService.UpdateShiftBreakOptions(updArray))
                                    {
                                        clsMessages.setMessage("Employee break options are saved successfully");
                                        this.RefreshAssignedShifts(shiftBreakEmployees.Select(c => c.EmployeeID).ToList());

                                    }
                                    else
                                    {
                                        clsMessages.setMessage("Update is failed");
                                    }
                                }
                            }
                            else
                            {
                                clsMessages.setMessage("User canceled update");
                            }
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                }
                else
                {
                    clsMessages.setMessage("No shifts to update, Please refresh again!");
                }

            }
            catch (Exception)
            {
                
            }
            finally
            {
                if (updateBreakWindow != null)
                    updateBreakWindow = null;
            }
        }

        #endregion

        #endregion

        #region Data Setting Methods

        void FillEmployeeAssignedShifts()
        {
            this.InitializeShiftDateRange();
            if (currentShiftEmployee != null && employeeShiftList.Count > 0)
            {
                currentShiftEmployee.EmpShifts.Clear();
                List<EmployeeAssignedShift> multiShiftList = new List<EmployeeAssignedShift>();
                List<EmployeeAssignedShiftsView> currentEmployeeShifts = assignedShiftList.Where(c => c.employee_id == currentShiftEmployee.EmployeeID).ToList();
                if (currentEmployeeShifts != null)
                {
                    foreach (EmployeeAssignedShift selectedShift in employeeShiftList)
                    {
                        List<EmployeeAssignedShiftsView> empShifts = currentEmployeeShifts.Where(c => c.date == selectedShift.DateOfDay.Date).ToList();
                        foreach (EmployeeAssignedShiftsView item in empShifts)
                        {
                            var currentShift = employeeShiftList.FirstOrDefault(c => c.DateOfDay.Date == selectedShift.DateOfDay.Date && c.IsNoShift == true);
                            if (currentShift != null)
                            {
                                currentShift.TransactID = item.trns_id;
                                currentShift.EmpID = item.emp_id;
                                currentShift.ShiftName = item.shift_detail_name;
                                currentShift.IsNoShift = false;
                                currentShift.IsShiftBreak = item.is_shift_break.Value;
                                currentShift.IsFreeBreak = item.is_free_break.Value;
                                currentShift.IsNoBreak = item.is_no_break.Value;
                            }
                            else
                            {
                                multiShiftList.Add(new EmployeeAssignedShift { TransactID = item.trns_id, EmpID = item.emp_id, ShiftName = item.shift_detail_name, DateOfDay = item.date, IsShiftBreak = item.is_shift_break.Value, IsFreeBreak = item.is_free_break.Value, IsNoBreak = item.is_no_break.Value });
                            }
                        }
                    }

                    if (multiShiftList.Count > 0)
                        multiShiftList.ForEach(c => employeeShiftList.Add(c));

                    employeeShiftList.ForEach(c => CurrentShiftEmployee.EmpShifts.Add(c));
                    
                }
            }
        }

        void FillAllEmployeeAssignedShifts()
        {
            foreach (ShiftEmployee emp in shiftAssignEmpList)
            {
                this.InitializeShiftDateRange();
                if (employeeShiftList.Count > 0)
                {
                    emp.EmpShifts.Clear();
                    List<EmployeeAssignedShift> multiShiftList = new List<EmployeeAssignedShift>();
                    List<EmployeeAssignedShiftsView> currentEmployeeShifts = assignedShiftList.Where(c => c.employee_id == emp.EmployeeID).ToList();
                    if (currentEmployeeShifts != null)
                    {
                        foreach (EmployeeAssignedShift selectedShift in employeeShiftList)
                        {
                            List<EmployeeAssignedShiftsView> empShifts = currentEmployeeShifts.Where(c => c.date == selectedShift.DateOfDay.Date).ToList();
                            foreach (EmployeeAssignedShiftsView item in empShifts)
                            {
                                var currentShift = employeeShiftList.FirstOrDefault(c => c.DateOfDay.Date == selectedShift.DateOfDay.Date && c.IsNoShift == true);
                                if (currentShift != null)
                                {
                                    currentShift.TransactID = item.trns_id;
                                    currentShift.EmpID = item.emp_id;
                                    currentShift.ShiftName = item.shift_detail_name;
                                    currentShift.IsNoShift = false;
                                    currentShift.IsShiftBreak = item.is_shift_break.Value;
                                    currentShift.IsFreeBreak = item.is_free_break.Value;
                                    currentShift.IsNoBreak = item.is_no_break.Value;
                                }
                                else
                                {
                                    multiShiftList.Add(new EmployeeAssignedShift { TransactID = item.trns_id, EmpID = item.emp_id, ShiftName = item.shift_detail_name, DateOfDay = item.date, IsShiftBreak = item.is_shift_break.Value, IsFreeBreak = item.is_free_break.Value, IsNoBreak = item.is_no_break.Value });
                                }
                            }
                        }

                        if (multiShiftList.Count > 0)
                            multiShiftList.ForEach(c => employeeShiftList.Add(c));

                        employeeShiftList.ForEach(c => emp.EmpShifts.Add(c));
                    }
                } 
            }
        }

        void FillShiftEmployees()
        {
            shiftAssignEmpList.Clear();
            foreach (AttendanceGroupWithEmployee emp in selectedEmployees)
            {
                ShiftEmployee shiftEmp = new ShiftEmployee();
                shiftEmp.EmployeeID = emp.employee_id;
                shiftEmp.EmpID = emp.emp_id;
                shiftEmp.FirstName = emp.first_name;
                shiftEmp.SecondName = emp.second_name;
                shiftEmp.IsMultiShift = false;
                shiftAssignEmpList.Add(shiftEmp);
            }

            ShiftEmployees = shiftAssignEmpList;
        }

        void InitializeShiftDateRange()
        {
            if (refreshStartDate != null && refreshEndDate != null)
            {
                if (employeeShiftList.Count > 0)
                    employeeShiftList.Clear();
                for (int i = 0; refreshEndDate >= refreshStartDate.Date.AddDays(i); i++)
                {
                    EmployeeAssignedShift shift = new EmployeeAssignedShift();
                    shift.DateOfDay = refreshStartDate.Date.AddDays(i);
                    shift.IsNoShift = true;
                    shift.IsShiftBreak = false;
                    shift.IsFreeBreak = false;
                    shift.IsNoBreak = false;
                    employeeShiftList.Add(shift);
                }
                EmployeeAssignedShifts = employeeShiftList;
            }
        }

        void InitializeShiftEmployees()
        {
            if (shiftAssignEmpList.Count > 0)
            {
                foreach (ShiftEmployee empItem in shiftAssignEmpList)
                {
                    var currentEmp = shiftAssignEmpList.FirstOrDefault(c => c.EmployeeID == empItem.EmployeeID);
                    List<EmployeeAssignedShiftsView> shiftList = assignedShiftList.Where(c => c.employee_id == empItem.EmployeeID).ToList();
                    if (shiftList.GroupBy(c => c.date).Any(g => g.Count() > 1))
                    {
                        // employee has been assigned multishifts for selected period
                        currentEmp.IsMultiShift = true;
                    }
                    else
                    {
                        currentEmp.IsMultiShift = false;
                    }

                    // searching for no shift assigned days
                    List<DateTime> nonAssignList = employeeAssignedShifts.Select(c => c.DateOfDay.Date).Distinct().Except(shiftList.Select(d => d.date.Date).Distinct()).ToList();
                    if (nonAssignList.Count > 0)
                    {
                        currentEmp.IsNoShiftDay = true;
                        currentEmp.NoShiftDays = nonAssignList.Count;
                    }
                    else
                    {
                        currentEmp.IsNoShiftDay = false;
                        currentEmp.NoShiftDays = 0;
                    }
                }

                ShiftEmployees = null;
                ShiftEmployees = shiftAssignEmpList;
            }
        }

        #endregion

        #endregion

        #region Employee Break Assign Window

        #region List

        List<ShiftEmployee> breakAssignEmpList = new List<ShiftEmployee>();
        IList multiSelectBreakEmps = new ArrayList();

        #endregion

        #region Properties

        List<ShiftEmployee> shiftBreakEmployees = new List<ShiftEmployee>();
        public List<ShiftEmployee> ShiftBreakEmployees
        {
            get { return shiftBreakEmployees; }
            set { shiftBreakEmployees = value; OnPropertyChanged("ShiftBreakEmployees"); }
        }

        List<DateTime> selectedBreakDates = new List<DateTime>();
        public List<DateTime> SelectedBreakDates
        {
            get { return selectedBreakDates; }
            set { selectedBreakDates = value; OnPropertyChanged("SelectedBreakDates"); }
        }

        IList MultiSelectBreakEmps
        {
            get { return multiSelectBreakEmps; }
            set { multiSelectBreakEmps = value; OnPropertyChanged("MultiSelectBreakEmps"); }
        }

        DateTime fromBreakDate;
        public DateTime FromBreakDate
        {
            get { return fromBreakDate; }
            set { fromBreakDate = value; }

        }

        DateTime toBreakDate;
        public DateTime ToBreakDate
        {
            get { return toBreakDate; }
            set { toBreakDate = value; }
        }

        bool isDateSelectionPopUp;
        public bool IsDateSelectionPopUp
        {
            get { return isDateSelectionPopUp; }
            set { isDateSelectionPopUp = value; OnPropertyChanged("IsDateSelectionPopUp"); }
        }

        bool isBreakOptionSelectionPopUp;
        public bool IsBreakOptionSelectionPopUp
        {
            get { return isBreakOptionSelectionPopUp; }
            set { isBreakOptionSelectionPopUp = value; OnPropertyChanged("IsBreakOptionSelectionPopUp"); }
        }

        bool isShiftBreakOption;
        public bool IsShiftBreakOption
        {
            get { return isShiftBreakOption; }
            set 
            { 
                isShiftBreakOption = value; OnPropertyChanged("IsShiftBreakOption");
                if (isShiftBreakOption)
                    IsNoBreakOption = false;
            }
        }

        bool isFreeBreakOption;
        public bool IsFreeBreakOption
        {
            get { return isFreeBreakOption; }
            set 
            { 
                isFreeBreakOption = value; OnPropertyChanged("IsFreeBreakOption");
                if (isFreeBreakOption)
                    IsNoBreakOption = false;
            }
        }

        bool isNoBreakOption;
        public bool IsNoBreakOption
        {
            get { return isNoBreakOption; }
            set 
            { 
                isNoBreakOption = value; OnPropertyChanged("IsNoBreakOption");
                if (IsNoBreakOption) { IsShiftBreakOption = IsFreeBreakOption = false; }
            }
        }

        #endregion

        #region Button Methods

        #region Select Date Button

        public ICommand SelectDateButton
        {
            get { return new RelayCommand(makeDatePopUp); }
        }

        private void makeDatePopUp()
        {

            IsDateSelectionPopUp = true;
        }

        #endregion

        #region Set Date Range Button

        public ICommand SetDateRangeButton
        {
            get { return new RelayCommand(setDateRangeSelection); }
        }

        void setDateRangeSelection()
        {
            if (fromBreakDate != null && toBreakDate != null)
            {
                if (toBreakDate >= fromBreakDate)
                {
                    List<DateTime> filterDateList = new List<DateTime>();
                    for (int i = 0; fromBreakDate.AddDays(i) <= toBreakDate; i++)
                    {
                        filterDateList.Add(fromBreakDate.AddDays(i));
                    }

                    SelectedBreakDates = null;
                    SelectedBreakDates = filterDateList;
                }
            }
            IsDateSelectionPopUp = false;
        }

        #endregion

        #region Select Break Option Button

        public ICommand SelectBreakOptionButton
        {
            get { return new RelayCommand(makeBreakPopUp); }
        }

        private void makeBreakPopUp()
        {
            IsBreakOptionSelectionPopUp = true;
        }

        #endregion

        #region Set Break Option Button
        
        public ICommand SetBreakOptionButton
        {
            get { return new RelayCommand(setBreakOption); }
        }

        private void setBreakOption()
        {
            IsBreakOptionSelectionPopUp = false;
            if(selectedBreakDates != null && selectedBreakDates.Count > 0)
            {
                foreach(ShiftEmployee selectedEmp in shiftBreakEmployees)
                {
                    foreach(DateTime shiftDate in selectedBreakDates)
                    {
                        var filteredShifts = selectedEmp.EmpShifts.Where(c => c.DateOfDay.Date == shiftDate.Date);
                        if(filteredShifts != null)
                        {
                            foreach(EmployeeAssignedShift selectedShift in filteredShifts)
                            {
                                selectedShift.IsShiftBreak = isShiftBreakOption;
                                selectedShift.IsFreeBreak = isFreeBreakOption;
                                selectedShift.IsNoBreak = isNoBreakOption;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Data Setting Methods

        void FillShiftBreakEmployees()
        {
            breakAssignEmpList.Clear();
            SelectedBreakDates.Clear();

            if (currentShiftDaysList.Count > 0 && !currentShiftDaysList.Any(c => c.Shift == null))
            {
                FromBreakDate = ShiftDays.FirstOrDefault().DateOfDay;
                ToBreakDate = ShiftDays.LastOrDefault().DateOfDay;

                if (allGroupEmployeesList.Count(c => c.is_active == true) > 0)
                {
                    foreach (AttendanceGroupWithEmployee currentEmp in allGroupEmployeesList.Where(c => c.is_active == true ))
                    {
                        ShiftEmployee shiftEmp = new ShiftEmployee();
                        shiftEmp.EmployeeID = currentEmp.employee_id;
                        shiftEmp.EmpID = currentEmp.emp_id;
                        shiftEmp.AttendanceGroupID = currentEmp.attendance_group_id;
                        shiftEmp.FirstName = currentEmp.first_name;
                        shiftEmp.SecondName = currentEmp.second_name;

                        foreach (WeekDayShift selectShift in currentShiftDaysList)
                        {
                            EmployeeAssignedShift item = new EmployeeAssignedShift();
                            item.ShiftID = selectShift.Shift.shift_detail_id;
                            item.DateOfDay = selectShift.DateOfDay;
                            item.DayID = selectShift.DayID;
                            item.ShiftName = selectShift.ShiftName;
                            shiftEmp.EmpShifts.Add(item);
                        }

                        breakAssignEmpList.Add(shiftEmp);
                    }

                    ShiftBreakEmployees = null;
                    ShiftBreakEmployees = breakAssignEmpList;

                }
                else
                {
                    clsMessages.setMessage("No employees are selected");
                }
            }
            else
            {
                clsMessages.setMessage("Shift details are incomplete");
            }
        }

        #endregion

        #endregion
    }

    public class WeekDayShift
    {
        DayOfWeek day;
        public DayOfWeek Day
        {
            get { return day; }
            set { day = value; }
        }

        DateTime dateOfDay;
        public DateTime DateOfDay
        {
            get { return dateOfDay; }
            set
            {
                dateOfDay = value;
                day = dateOfDay.DayOfWeek;
            }
        }

        dtl_Shift_Master shift;
        public dtl_Shift_Master Shift
        {
            get { return shift; }
            set { shift = value; }
        }

        public string ShiftName
        {
            get { if (shift != null) return shift.shift_detail_name; else return ""; }

        }

        bool isMultiShift;
        public bool IsMultiShift
        {
            get { return isMultiShift; }
            set { isMultiShift = value; }
        }

        int dayID;
        public int DayID
        {
            get { return dayID; }
            set { dayID = value; }
        }

        public override string ToString()
        {
            return String.Format("Date:{0}\nDay:{1}\nShift:{2}", dateOfDay == null ? "" : dateOfDay.ToShortDateString(), day.ToString(), ShiftName);
        }

    }

    public class EmployeeAssignedShift
    {
        int transactID;
        public int TransactID
        {
            get { return transactID; }
            set { transactID = value; }
        }

        string empID;
        public string EmpID
        {
            get { return empID; }
            set { empID = value; }
        }

        string shiftName;
        public string ShiftName
        {
            get { return shiftName; }
            set { shiftName = value; }
        }

        int shiftID;
        public int ShiftID
        {
            get { return shiftID; }
            set { shiftID = value; }
        }

        DayOfWeek day;
        public DayOfWeek Day
        {
            get { return day; }
            set { day = value; }
        }

        int dayID;
        public int DayID
        {
            get { return dayID; }
            set { dayID = value; }
        }

        DateTime dateOfDay;
        public DateTime DateOfDay
        {
            get { return dateOfDay; }
            set
            {
                dateOfDay = value;
                day = dateOfDay.DayOfWeek;
            }
        }

        bool isNoShift;
        public bool IsNoShift
        {
            get { return isNoShift; }
            set { isNoShift = value; }
        }

        bool isShiftBreak;
        public bool IsShiftBreak
        {
            get { return isShiftBreak; }
            set { isShiftBreak = value; }
        }

        bool isFreeBreak;
        public bool IsFreeBreak
        {
            get { return isFreeBreak; }
            set { isFreeBreak = value; }
        }

        bool isNoBreak;
        public bool IsNoBreak
        {
            get { return isNoBreak; }
            set { isNoBreak = value; }
        }

    }

    public class ShiftEmployee
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

        int attendanceGroupID;
        public int AttendanceGroupID
        {
            get { return attendanceGroupID; }
            set { attendanceGroupID = value; }
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

        bool isMultiShift;
        public bool IsMultiShift
        {
            get { return isMultiShift; }
            set { isMultiShift = value; }
        }

        int noShiftDays;
        public int NoShiftDays
        {
            get { return noShiftDays; }
            set { noShiftDays = value; }
        }

        bool isNoShiftDay;
        public bool IsNoShiftDay
        {
            get { return isNoShiftDay; }
            set { isNoShiftDay = value; }
        }

        List<EmployeeAssignedShift> empShifts = new List<EmployeeAssignedShift>();
        public List<EmployeeAssignedShift> EmpShifts
        {
            get { return empShifts; }
            set { empShifts = value; }
        }
    }

}
