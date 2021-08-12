using ERP.AttendanceService;
using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class EmployeeMaxOTViewModel:ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendServiceClient;
        #endregion

        #region Constructor

        public EmployeeMaxOTViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshAttendanceGroups();
            this.RefreshShifts();
            IsCheckAll = true;
            IsRangeDateSelect = true;
        }

        #endregion

        #region Data Members

        #region IList

        IList selectEmpList = new ArrayList();
        IList selectMaxOtList = new ArrayList();

        #endregion

        #region List Memebers

        List<AttendanceGroupDetailsView> selectedGroupsList = new List<AttendanceGroupDetailsView>();
        List<AttendanceGroupWithEmployee> allGroupEmployeesList = new List<AttendanceGroupWithEmployee>();
        List<AttendanceGroupWithEmployee> selectedGroupEmployees = new List<AttendanceGroupWithEmployee>();
        List<EmployeeSearchView> searchedEmployeeList = new List<EmployeeSearchView>();

        List<dtl_Shift_Master> employeeShiftList = new List<dtl_Shift_Master>();
        List<DateTime> currentSelectedDates = new List<DateTime>();

        List<trns_EmployeeDailyShiftDetails> allAssignedShiftList = new List<trns_EmployeeDailyShiftDetails>();
        List<trns_EmployeeDailyShiftDetails> selectedAssginedShifts = new List<trns_EmployeeDailyShiftDetails>();

        List<trns_EmployeeDailyShiftDetails> refreshAssignedList = new List<trns_EmployeeDailyShiftDetails>();
        List<DateTime> refreshDateList = new List<DateTime>();
        List<AttendanceGroupWithEmployee> refreshEmpList = new List<AttendanceGroupWithEmployee>();
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
        #endregion 

        #endregion

        #region Properties

        List<AttendanceGroupWithEmployee> selectedEmployees;
        public List<AttendanceGroupWithEmployee> SelectedEmployees
        {
            get { if (selectedEmployees != null) selectedEmployees = selectedEmployees.OrderBy(c => Convert.ToInt32(c.emp_id)).ToList(); return selectedEmployees; }
            set { selectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }

        AttendanceGroupWithEmployee currentSelectedEmployee;
        public AttendanceGroupWithEmployee CurrentSelectedEmployee
        {
            get { return currentSelectedEmployee; }
            set { currentSelectedEmployee = value; OnPropertyChanged("CurrentSelectedEmployee"); this.PopulateEmployeeMaxOT(); }
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

        List<AttendanceGroupDetailsView> selectedAttendanceGroups;
        public List<AttendanceGroupDetailsView> SelectedAttendanceGroups
        {
            get { return selectedAttendanceGroups; }
            set { selectedAttendanceGroups = value; OnPropertyChanged("SelectedAttendanceGroups"); }
        }

        IEnumerable<DateTime> selectedDates;
        public IEnumerable<DateTime> SelectedDates
        {
            get { if (selectedDates != null) return selectedDates.OrderBy(c => c.Date); return selectedDates; }
            set { selectedDates = value; OnPropertyChanged("SelectedDates"); }
        }

        List<trns_EmployeeDailyShiftDetails> employeeAssignedShifts;
        public List<trns_EmployeeDailyShiftDetails> EmployeeAssignedShifts
        {
            get { if (employeeAssignedShifts != null) employeeAssignedShifts = employeeAssignedShifts.OrderBy(c =>c.employee_id).ThenBy(c=>c.date).ToList(); return employeeAssignedShifts; }
            set { employeeAssignedShifts = value; OnPropertyChanged("EmployeeAssignedShifts"); }
        }

        trns_EmployeeDailyShiftDetails currentEmployeeSelectedShift;
        public trns_EmployeeDailyShiftDetails CurrentEmployeeSelectedShift
        {
            get { return currentEmployeeSelectedShift; }
            set { currentEmployeeSelectedShift = value; OnPropertyChanged("CurrentEmployeeSelectedShift"); }
        }

        DateTime? currentSelectedDate;
        public DateTime? CurrentSelectedDate
        {
            get { return currentSelectedDate; }
            set
            { 
                currentSelectedDate = value; OnPropertyChanged("CurrentSelectedDate");
                //if (currentSelectedDate != null)
                //    this.FilterAssignedShiftsByDate();
            }
        }

        TimeSpan morningOtLimit;
        public TimeSpan MorningOtLimit
        {
            get { return morningOtLimit; }
            set { morningOtLimit = value; OnPropertyChanged("MorningOtLimit"); }
        }

        TimeSpan eveningOtLimit;
        public TimeSpan EveningOtLimit
        {
            get { return eveningOtLimit; }
            set { eveningOtLimit = value; OnPropertyChanged("EveningOtLimit"); }
        }

        public IList SelectEmpList
        {
            get { return selectEmpList; }
            set { selectEmpList = value; OnPropertyChanged("SelectEmpList"); }
        }

        public IList SelectMaxOtList
        {
            get { return selectMaxOtList; }
            set 
            {
                selectMaxOtList = value; OnPropertyChanged("SelectMaxOtList");
            }
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

        bool isMaxOtCheckAll;
        public bool IsMaxOtCheckAll
        {
            get { return isMaxOtCheckAll; }
            set 
            { 
                isMaxOtCheckAll = value; OnPropertyChanged("IsMaxOtCheckAll");
                this.CheckMaxOtShifts();
            }
        }

        #endregion

        #region Refresh Methods

        // Attendance group employees are refreshed when one selected from Attendance Group drop-down
        void RefreshGroupEmployees()
        {
            attendServiceClient.GetAttendanceGroupWithEmployeesDetailsCompleted += (s, e) =>
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
                attendServiceClient.GetAttendanceGroupWithEmployeesDetailsAsync(currentAttendanceGroup.attendance_group_id);
            }
        }

        void RefreshAttendanceGroups()
        {
            attendServiceClient.GetAttendanceGroupsDetailsCompleted += (s, e) =>
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
            attendServiceClient.GetAttendanceGroupsDetailsAsync();
        }

        void RefreshShifts()
        {
            try
            {
                attendServiceClient.GetShiftNamesCompleted += (s, e) =>
                    {
                        employeeShiftList = e.Result.ToList();
                    };
                attendServiceClient.GetShiftNamesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Shifts refresh is failed");
            }
        }

        // Selected employees' currently assigned shifts and max-ot amounts if assigned are retrieved based on selected date range/dates.
        void RefreshEmployeeAssignedShifts()
        {
            try
            {
                // check if new employees and new dates are selected for refresh
                if (refreshEmpList.Count > 0 && refreshDateList.Count > 0)
                {
                    attendServiceClient.GetEmployeeDailyShiftDetailsWithOTCompleted += (s, e) =>
                               {
                                   refreshAssignedList = e.Result.ToList();
                                   if (refreshAssignedList.Count > 0)
                                   {
                                       this.RefreshAssignedShiftsData();
                                       refreshDateList.Clear();
                                       refreshEmpList.Clear();
                                       this.FilterAssignedShiftsByEmployees();
                                   }

                               };
                    attendServiceClient.GetEmployeeDailyShiftDetailsWithOTAsync(refreshEmpList.Select(c => c.employee_id).ToArray(), refreshDateList.ToArray()); 
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Refreshing assigned shifts is failed");
            }
        }

        #endregion

        #region Button Methods

        #region Select Employee Button

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
                    searchedEmployeeList.Where(c => !allGroupEmployeesList.Any(d => d.employee_id == c.employee_id)).ToList().ForEach(delegate(EmployeeSearchView c){
                        allGroupEmployeesList.Add(new AttendanceGroupWithEmployee { emp_id = c.emp_id, employee_id = c.employee_id, first_name = c.first_name, second_name = c.second_name, is_active = true });
                        refreshEmpList.Add(new AttendanceGroupWithEmployee { emp_id = c.emp_id, employee_id = c.employee_id, first_name = c.first_name, second_name = c.second_name, is_active = true });
                    });

                    if (refreshEmpList.Count > 0)
                        currentSelectedDates.ForEach(c => refreshDateList.Add(c));
                    SelectedEmployees = null;
                    SelectedEmployees = allGroupEmployeesList;
                    this.RefreshEmployeeAssignedShifts();
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

        #region Get Employee Button

        void Get()
        {
            if (selectedGroupEmployees.Count > 0)
            {
                foreach (AttendanceGroupWithEmployee emp in selectedGroupEmployees)
                {
                    allGroupEmployeesList.Add(emp);
                    refreshEmpList.Add(emp);
                }
                if (refreshEmpList.Count > 0)
                    currentSelectedDates.ForEach(c => refreshDateList.Add(c)); 
               
                SelectedEmployees = null;
                SelectedEmployees = allGroupEmployeesList;
                selectedGroupEmployees.Clear();
                this.FillGroupsListBox();
                this.RefreshEmployeeAssignedShifts();
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
            get { return new RelayCommand(SetSelectedDates); }
        }

        void SetSelectedDates()
        {
            if (fromDate != null)
            {
                if (isRangeDateSelect)
                {
                    if (toDate >= fromDate)
                    {
                        int incr = 0;
                        DateTime nextDate = fromDate.Value;
                        while (toDate.Value.Date >= nextDate.Date)
                        {
                            if (!currentSelectedDates.Any(c => c.Date == nextDate.Date))
                            {
                                currentSelectedDates.Add(nextDate);
                                refreshDateList.Add(nextDate);
                            }
                            incr++;
                            nextDate = fromDate.Value.Date.AddDays(incr);
                        }
                    }
                }
                else
                {
                    if (!currentSelectedDates.Any(c => c.Date == fromDate.Value.Date))
                    {
                        currentSelectedDates.Add(fromDate.Value);
                        refreshDateList.Add(fromDate.Value.Date);
                    }
                }
                SelectedDates = null;
                SelectedDates = currentSelectedDates;
                if (refreshDateList.Count > 0)
                    allGroupEmployeesList.ForEach(c => refreshEmpList.Add(c));
                this.RefreshEmployeeAssignedShifts();
            }
        }

        #endregion

        #region Assign Max OT Button

        public ICommand SetMaxOtButton
        {
            get { return new RelayCommand(SetMaxOt); }
        }

        private void SetMaxOt()
        {
            if(selectMaxOtList.Count > 0)
            {
                foreach(trns_EmployeeDailyShiftDetails item in selectMaxOtList)
                {
                    var maxOtShift = selectedAssginedShifts.FirstOrDefault(c => c.trns_id == item.trns_id);
                    maxOtShift.trns_EmployeeMaxOTDetails = new trns_EmployeeMaxOTDetails();
                    maxOtShift.trns_EmployeeMaxOTDetails.morning_ot_limit = (int)morningOtLimit.TotalSeconds;
                    maxOtShift.trns_EmployeeMaxOTDetails.evening_ot_limit = (int)eveningOtLimit.TotalSeconds;
                    maxOtShift.dtl_Shift_Master.is_delete = true;
                }
            }
        }

        #endregion

        #region Save Button

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        void Save()
        {
            if(isDateRangeSelected() && isValidEmployees() && isValidAssignedShifts())
            {
                if (clsSecurity.GetSavePermission(311))
                {
                    List<trns_EmployeeMaxOTDetails> addingList = this.SetCurrentMaxOtDetails();
                    if (addingList.Count > 0)
                    {
                        if (attendServiceClient.SaveEmployeeMaxOtDetails(addingList.ToArray()))
                        {
                            clsMessages.setMessage("Max OT details are saved successfully");
                            EmployeeAssignedShifts = null;
                            CurrentSelectedEmployee = null;
                            allAssignedShiftList = allAssignedShiftList.Where(c => !addingList.Any(d => d.shift_assigned_id == c.trns_id)).ToList();
                            this.RefreshCurrentData();
                        }
                        else
                        {
                            clsMessages.setMessage("Max OT details saving is failed");
                        }
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Save this record(s)");
            }
        }

        #endregion

        #region Delete Button

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }

        void Delete()
        {
            if(selectMaxOtList.Count > 0)
            {
                if (clsSecurity.GetDeletePermission(311))
                {
                    List<trns_EmployeeMaxOTDetails> deleteOtList = new List<trns_EmployeeMaxOTDetails>();
                    foreach (trns_EmployeeDailyShiftDetails delItem in selectMaxOtList)
                    {
                        trns_EmployeeMaxOTDetails deletingOt = new trns_EmployeeMaxOTDetails();
                        deletingOt.shift_assigned_id = delItem.trns_id;
                        deleteOtList.Add(deletingOt);
                    }
                    if (attendServiceClient.DeleteEmployeeMaxOtDetails(deleteOtList.ToArray()))
                    {
                        clsMessages.setMessage("Max OT details are deleted successfully");
                        EmployeeAssignedShifts = null;
                        CurrentSelectedEmployee = null;
                        allAssignedShiftList = allAssignedShiftList.Where(c => !deleteOtList.Any(d => d.shift_assigned_id == c.trns_id)).ToList();
                        this.RefreshCurrentData();
                    }
                    else
                    {
                        clsMessages.setMessage("Max OT details delete is failed");
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        #endregion

        #region Remove Employee Key (delete key)

        public ICommand RemoveEmployeeKey
        {
            get { return new RelayCommand(RemoveEmployee); }
        }

        private void RemoveEmployee()
        {
            // Already selected employees can be removed by using delete key
            if (selectEmpList != null && selectEmpList.Count > 0)
            {
                foreach (AttendanceGroupWithEmployee emp in selectEmpList)
                {
                    selectedEmployees.FirstOrDefault(c => c.employee_id == emp.employee_id).is_active = false;
                }

                selectedAssginedShifts = allAssignedShiftList.Where(c => selectedEmployees.Where(d=>d.is_active == true).Any(d => d.employee_id == c.employee_id)).ToList();
                EmployeeAssignedShifts = null;
                EmployeeAssignedShifts = selectedAssginedShifts;
            }
        }

        #endregion

        #region Remove Date Key (delete key)

        public ICommand DeleteDateKey
        {
            get { return new RelayCommand(RemoveSelectedGroup); }
        }
        #endregion

        #endregion

        #region Data Setting Methods

        void PopulateEmployeeMaxOT()
        {
            if (currentSelectedEmployee != null && currentSelectedEmployee.is_active == true)
            {
                selectedAssginedShifts.ForEach(c => c.dtl_Shift_Master.is_roster = false);
                foreach (trns_EmployeeDailyShiftDetails item in selectedAssginedShifts.Where(c => c.employee_id == currentSelectedEmployee.employee_id))
                {
                    trns_EmployeeDailyShiftDetails current = selectedAssginedShifts.FirstOrDefault(c => c.trns_id == item.trns_id);
                    current.dtl_Shift_Master = new dtl_Shift_Master();
                    current.dtl_Shift_Master.shift_detail_name = employeeShiftList.FirstOrDefault(c => c.shift_detail_id == current.shift_detail_id).shift_detail_name;
                    current.dtl_Shift_Master.is_roster = true;
                }
            }
        }

        List<trns_EmployeeMaxOTDetails> SetCurrentMaxOtDetails()
        {
            List<trns_EmployeeMaxOTDetails> maxOtSaveList = new List<trns_EmployeeMaxOTDetails>();
            try
            {
                foreach(trns_EmployeeDailyShiftDetails assignShift in selectedAssginedShifts.Where(c=>c.dtl_Shift_Master.is_delete == true))
                {
                    trns_EmployeeMaxOTDetails addingMaxOt = new trns_EmployeeMaxOTDetails();
                    addingMaxOt.shift_assigned_id = assignShift.trns_id;
                    addingMaxOt.morning_ot_limit = assignShift.trns_EmployeeMaxOTDetails.morning_ot_limit;
                    addingMaxOt.evening_ot_limit = assignShift.trns_EmployeeMaxOTDetails.evening_ot_limit;
                    maxOtSaveList.Add(addingMaxOt);
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Saving cannot be initiated");
            }
            return maxOtSaveList;
        }
        
        void RefreshAssignedShiftsData()
        {
            // Retrieved assigned shifts are filtered to get only newly assigned shifts
            refreshAssignedList = refreshAssignedList.Where(c=>!allAssignedShiftList.Any(d=>d.trns_id == c.trns_id)).ToList();
            foreach (trns_EmployeeDailyShiftDetails item in refreshAssignedList)
            {
                item.dtl_Shift_Master = new dtl_Shift_Master();
                item.dtl_Shift_Master.shift_detail_name = employeeShiftList.FirstOrDefault(c => c.shift_detail_id == item.shift_detail_id).shift_detail_name;
                allAssignedShiftList.Add(item);
            }

            List<AttendanceGroupWithEmployee> currentEmpList = allGroupEmployeesList.Where(c=> c.is_active == true).ToList();
            selectedAssginedShifts = allAssignedShiftList.Where(c => currentEmpList.Any(d => d.employee_id == c.employee_id)).ToList();
            EmployeeAssignedShifts = null;
            EmployeeAssignedShifts = selectedAssginedShifts;
            refreshAssignedList.Clear();
        }

        void RefreshCurrentData()
        {
            currentSelectedDates.ForEach(c => refreshDateList.Add(c));
            refreshEmpList = allGroupEmployeesList.Where(c => c.is_active == true).ToList();
            this.RefreshEmployeeAssignedShifts();
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
                this.FilterAssignedShiftsByEmployees();
            }
        }

        #endregion

        #region Date Range Selection Methods

        public void RemoveSelectedDate()
        {
            if(currentSelectedDate != null)
            {
                currentSelectedDates.Remove(currentSelectedDate.Value);
                SelectedDates = null;
                SelectedDates = currentSelectedDates;
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

                var currentEmpList = allGroupEmployeesList.Where(c => c.is_active == true);
                selectedAssginedShifts = allAssignedShiftList.Where(c => currentEmpList.Any(d => d.employee_id == c.employee_id)).ToList();
                EmployeeAssignedShifts = null;
                EmployeeAssignedShifts = selectedAssginedShifts;
            }
        }

        void CheckMaxOtShifts()
        {
            if(selectMaxOtList.Count > 0)
            {
                foreach(trns_EmployeeDailyShiftDetails shift in selectMaxOtList)
                {
                    var current = employeeAssignedShifts.FirstOrDefault(c => c.trns_id == shift.trns_id);
                    current.dtl_Shift_Master.is_delete = isMaxOtCheckAll;
                }
            }
        }

        #endregion

        #region Validation Methods

        bool isDateRangeSelected()
        {
            if(currentSelectedDates.Count == 0)
            {
                clsMessages.setMessage("Max ot should be assigned for date range/multiple days");
                return false;
            }
            return true;
        }

        bool isValidEmployees()
        {
            if(allGroupEmployeesList.Count(c=>c.is_active == true)== 0)
            {
                clsMessages.setMessage("At least one employee should be selected");
                return false;
            }
            return true;
        }

        bool isValidAssignedShifts()
        {
            if(allAssignedShiftList.Count(c=>c.dtl_Shift_Master.is_delete == true) == 0)
            {
                clsMessages.setMessage("At least one assigned shift should be selected");
                return false;
            }
            return true;
        }

        #endregion

        #region Filter Methods

        void FilterAssignedShiftsByEmployees()
        {
            List<AttendanceGroupWithEmployee> currentEmpList = allGroupEmployeesList.Where(c => c.is_active == true).ToList();
            selectedAssginedShifts = allAssignedShiftList.Where(c => currentEmpList.Any(d => d.employee_id == c.employee_id)).ToList();
            EmployeeAssignedShifts = null;
            EmployeeAssignedShifts = selectedAssginedShifts;
        }

        void FilterAssignedShiftsByDate()
        {
            if(currentSelectedDate != null)
            {
                EmployeeAssignedShifts = null;
                EmployeeAssignedShifts = selectedAssginedShifts.Where(c => c.date == currentSelectedDate).ToList();
            }
        }

        #endregion
    }

    class SecondsToTimeConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
                return TimeSpan.FromSeconds((int)value);
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
