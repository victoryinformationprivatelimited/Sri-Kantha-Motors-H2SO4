using CustomBusyBox;
using ERP.AttendanceService;
using ERP.ERPService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceProcessMaster
{
    class AttendanceProcessViewModel : ViewModelBase
    {
        #region Attendance Service Client

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor

        public AttendanceProcessViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshEmployeeSummaryDetails();
            this.RefreshAttendancePeriods();
            this.RefreshAttendanceGroups();
            IsFilterUnselected = true;
            CheckLeiuLeave = true;
        }

        #endregion

        #region IList Members

        IList selectEmpList = new ArrayList();

        #endregion

        #region List Members

        List<AttendEmployeeSummaryView> allEmployeeList = new List<AttendEmployeeSummaryView>();
        List<AttendEmployeeSummaryView> unselectedEmployeeList = new List<AttendEmployeeSummaryView>();
        List<AttendEmployeeSummaryView> selectedEmployeeList = new List<AttendEmployeeSummaryView>();
        
        #endregion

        #region Properties

        IEnumerable<AttendEmployeeSummaryView> selectedEmployees;
        public IEnumerable<AttendEmployeeSummaryView> SelectedEmployees
        {
            get { if (selectedEmployees != null) selectedEmployees = selectedEmployees.OrderBy(c => Convert.ToInt32(c.emp_id)); return selectedEmployees; }
            set { selectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }

        IEnumerable<AttendEmployeeSummaryView> unselectedEmployees;
        public IEnumerable<AttendEmployeeSummaryView> UnselectedEmployees
        {
            get { if (unselectedEmployees != null) unselectedEmployees = unselectedEmployees.OrderBy(c => Convert.ToInt32(c.emp_id)); return unselectedEmployees; }
            set { unselectedEmployees = value; OnPropertyChanged("UnselectedEmployees"); }
        }

        AttendEmployeeSummaryView currentAttendanceEmployee;
        public AttendEmployeeSummaryView CurrentAttendanceEmployee
        {
            get { return currentAttendanceEmployee; }
            set { currentAttendanceEmployee = value; OnPropertyChanged("CurrentAttendanceEmployee"); }
        }

        IEnumerable<AttendEmployeeSummaryView> attendanceEmployees;
        public IEnumerable<AttendEmployeeSummaryView> AttendanceEmployees
        {
            get { return attendanceEmployees; }
            set { attendanceEmployees = value; }
        }

        IEnumerable<z_CompanyBranches> branches;
        public IEnumerable<z_CompanyBranches> Branches
        {
            get { return branches; }
            set { branches = value; OnPropertyChanged("Branches"); }
        }

        z_CompanyBranches currentBranch;
        public z_CompanyBranches CurrentBranch
        {
            get { return currentBranch; }
            set
            { 
                currentBranch = value; OnPropertyChanged("CurrentBranch");
                if (currentBranch != null)
                    this.FilterEmployee();
            }
        }

        IEnumerable<z_Designation> designations;
        public IEnumerable<z_Designation> Designations
        {
            get { return designations; }
            set { designations = value; OnPropertyChanged("Designations"); }
        }

        z_Designation currentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return currentDesignation; }
            set
            { 
                currentDesignation = value; OnPropertyChanged("CurrentDesignation");
                if (currentDesignation != null)
                    this.FilterEmployee();
            }
        }

        IEnumerable<z_Department> departments;
        public IEnumerable<z_Department> Departments
        {
            get { return departments; }
            set { departments = value; OnPropertyChanged("Departments"); }
        }

        z_Department currentDepartment;
        public z_Department CurrentDepartment
        {
            get { return currentDepartment; }
            set 
            { 
                currentDepartment = value; OnPropertyChanged("CurrentDepartment");
                if (currentDepartment != null)
                    this.FilterEmployee();
            }
        }

        IEnumerable<z_Grade> grades;
        public IEnumerable<z_Grade> Grades
        {
            get { return grades; }
            set { grades = value; OnPropertyChanged("Grades"); }
        }

        z_Grade currentGrade;
        public z_Grade CurrentGrade
        {
            get { return currentGrade; }
            set 
            { 
                currentGrade = value; OnPropertyChanged("CurrentGrade");
                if (currentGrade != null)
                    this.FilterEmployee();
            }
        }

        IEnumerable<AttendancePeriodView> attendancePeriods;
        public IEnumerable<AttendancePeriodView> AttendancePeriods
        {
            get { if (attendancePeriods != null) attendancePeriods = attendancePeriods.OrderBy(c => c.start_date.Value.Date); return attendancePeriods; }
            set { attendancePeriods = value; OnPropertyChanged("AttendancePeriods"); }
        }

        AttendancePeriodView currentPeriod;
        public AttendancePeriodView CurrentPeriod
        {
            get { return currentPeriod; }
            set
            { 
                currentPeriod = value; OnPropertyChanged("CurrentPeriod");
                if (currentPeriod != null)
                    this.SetDataRange();
            }
        }

        IEnumerable<AttendanceGroupWithEmployee> attendanceGroupEmployees;
        public IEnumerable<AttendanceGroupWithEmployee> AttendanceGroupEmployees
        {
            get { return attendanceGroupEmployees; }
            set { attendanceGroupEmployees = value; OnPropertyChanged("AttendanceGroupEmployees"); }
        }

        AttendanceGroupWithEmployee currentGroupEmployee;
        public AttendanceGroupWithEmployee CurrentGroupEmployee
        {
            get { return currentGroupEmployee; }
            set { currentGroupEmployee = value; OnPropertyChanged("CurrentGroupEmployee"); }
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


        public IList SelectEmpList
        {
            get { return selectEmpList; }
            set { selectEmpList = value; OnPropertyChanged("SelectEmpList"); }
        }

        bool isAllCheck;
        public bool IsAllCheck
        {
            get { return isAllCheck; }
            set
            { 
                isAllCheck = value; OnPropertyChanged("IsAllCheck");
                if (isAllCheck)
                {
                    if (isFilterUnselected)
                    {
                        UnselectedEmployees = null;
                        UnselectedEmployees = unselectedEmployeeList; 
                    }
                    else if(isFilterSelected)
                    {
                        SelectedEmployees = null;
                        SelectedEmployees = selectedEmployeeList;
                    }
                }
                else
                {
                    this.FilterEmployee();
                }
                    
            }
        }

        bool isFilterSelected;
        public bool IsFilterSelected
        {
            get { return isFilterSelected; }
            set { isFilterSelected = value; OnPropertyChanged("IsFilterSelected"); }
        }

        bool isFilterUnselected;
        public bool IsFilterUnselected
        {
            get { return isFilterUnselected; }
            set { isFilterUnselected = value; OnPropertyChanged("IsFilterUnselected"); }
        }

        bool checkLeiuLeave;
        public bool CheckLeiuLeave
        {
            get { return checkLeiuLeave; }
            set { checkLeiuLeave = value; OnPropertyChanged("CheckLeiuLeave"); }
        }

        bool doNotCheckLeiuLeave;
        public bool DoNotCheckLeiuLeave
        {
            get { return doNotCheckLeiuLeave; }
            set { doNotCheckLeiuLeave = value; OnPropertyChanged("DoNotCheckLeiuLeave"); }
        }

        DateTime startDateRange;
        public DateTime StartDateRange
        {
            get { return startDateRange; }
            set { startDateRange = value; OnPropertyChanged("StartDateRange"); }
        }

        DateTime endDateRange;
        public DateTime EndDateRange
        {
            get { return endDateRange; }
            set { endDateRange = value; OnPropertyChanged("EndDateRange"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshEmployeeSummaryDetails()
        {
            attendServiceClient.GetAttendanceEmployeeSummaryDetailsCompleted += (s, e) =>
            {
                try
                {
                    AttendanceEmployees = e.Result;
                    if (attendanceEmployees != null)
                    {
                        allEmployeeList = attendanceEmployees.ToList();
                        this.PopulateData();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Employee details refresh is failed");
                }
                    
            };
            attendServiceClient.GetAttendanceEmployeeSummaryDetailsAsync();
        }

        void RefreshAttendancePeriods()
        {
            attendServiceClient.GetAttendancePeriodDetailsCompleted += (s, e) =>
            {
                try
                {
                    AttendancePeriods = e.Result;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance period details refresh is failed");
                }
            };
            attendServiceClient.GetAttendancePeriodDetailsAsync();
        }

        void RefreshGroupEmployees()
        {
            attendServiceClient.GetAttendanceGroupEmployeeIDsCompleted += (s, e) =>
            {
                try
                {
                    AttendanceGroupEmployees = e.Result;
                    if(attendanceGroupEmployees != null)
                    {
                        this.FilterEmployee();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance group employee IDs refresh is failed");
                }
            };

            attendServiceClient.GetAttendanceGroupEmployeeIDsAsync(currentAttendanceGroup.attendance_group_id);
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
                    clsMessages.setMessage("Attendance group details refresh is failed");
                }
            };

            attendServiceClient.GetAttendanceGroupsDetailsAsync();
        }

        #endregion

        #region Button Methods

        #region Process Button

        void Process()
        {
            if (selectedEmployeeList.Count > 0 && currentPeriod != null)
            {
                if (clsSecurity.GetSavePermission(310))
                {
                    BusyBox.ShowBusy("Please Wait Until Attendance Process Completed");
                    List<AttendanceEmployeeDataView> empList = new List<AttendanceEmployeeDataView>();
                    foreach (AttendEmployeeSummaryView selectEmp in selectedEmployeeList)
                    {
                        AttendanceEmployeeDataView addingEmployee = new AttendanceEmployeeDataView();
                        addingEmployee.emp_id = selectEmp.emp_id.TrimStart('0');
                        addingEmployee.employee_id = selectEmp.employee_id;
                        addingEmployee.first_name = selectEmp.first_name;
                        addingEmployee.second_name = selectEmp.second_name;
                        //addingEmployee.fingerprint_device_ID = selectEmp
                        empList.Add(addingEmployee);
                    }

                    bool isOnceFailed = false;
                    int baseValue = 10;
                    int remainEmps = empList.Count;
                    if (remainEmps < baseValue)
                    {
                        baseValue = remainEmps;
                    }
                    int startIndex = 0;
                    int endIndex = startIndex + baseValue;
                    while (remainEmps > 0)
                    {
                        if (remainEmps < baseValue)
                        {
                            baseValue = remainEmps;
                            //startIndex = endIndex;
                            endIndex = startIndex + baseValue;
                        }
                        //else
                        //{
                        //    startIndex = endIndex;
                        //    endIndex = startIndex + baseValue;
                        //}

                        var processedEmps = empList.Select((c, index) => new { Idx = index, EmpID = c.employee_id }).OrderBy(c => c.Idx);
                        List<AttendanceEmployeeDataView> processingEmpList = empList.Where(c => processedEmps.Where(d => d.Idx >= startIndex && d.Idx < endIndex).Any(d => d.EmpID == c.employee_id)).ToList();
                        try
                        {
                            if (attendServiceClient.BeginAttendanceProcess(processingEmpList.ToArray(), startDateRange.Date, endDateRange.Date, new ERP.AttendanceService.z_Period { period_id = currentPeriod.period_id, period_name = currentPeriod.period_name, start_date = currentPeriod.start_date, end_date = currentPeriod.end_date }, checkLeiuLeave))
                            {
                                remainEmps -= baseValue;
                                startIndex = endIndex;
                                endIndex = startIndex + baseValue;
                            }
                            else
                            {
                                isOnceFailed = true;
                                break;
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (isOnceFailed)
                    {
                        BusyBox.CloseBusy();
                        clsMessages.setMessage("Some part of attendance process not completed");
                    }
                    else
                    {
                        BusyBox.CloseBusy();
                        clsMessages.setMessage("Attendance process completed successfully");
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Process this record(s)");
            }
        }

        public ICommand ProcessButton
        {
           get { return new RelayCommand(Process);}
        }

        private bool ValidateProcess()
        {
            if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please Select A Period...");
                return false;
            }
            else if (selectedEmployeeList.Count == 0)
            {
                clsMessages.setMessage("There Are No Employees Selected...");
                return false;
            }
            else if (StartDateRange > EndDateRange)
            {
                clsMessages.setMessage("Please Select A Start Date Less Than End Date...");
                return false;
            }
            else if (StartDateRange < currentPeriod.start_date)
            {
                clsMessages.setMessage("Please Select A Start Date Within The Selected Period....");
                return false;
            }
            else if (endDateRange > currentPeriod.end_date)
            {
                clsMessages.setMessage("Please Select A End Date Within The Selected Period...");
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region Add Button

        void Add()
        {
            if(selectEmpList.Count > 0)
            {
                foreach(AttendEmployeeSummaryView addEmp in selectEmpList)
                {
                    selectedEmployeeList.Add(addEmp);
                    unselectedEmployeeList.Remove(addEmp);
                }

                SelectedEmployees = null;
                SelectedEmployees = selectedEmployeeList;
                UnselectedEmployees = null;
                UnselectedEmployees = unselectedEmployeeList;
            }
        }

        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }

        #endregion

        #region Remove Button

        void Remove()
        {
            if(selectEmpList.Count > 0)
            {
                foreach(AttendEmployeeSummaryView removeEmp in selectEmpList)
                {
                    unselectedEmployeeList.Add(removeEmp);
                    selectedEmployeeList.Remove(removeEmp);
                }

                SelectedEmployees = null;
                SelectedEmployees = selectedEmployeeList;
                UnselectedEmployees = null;
                UnselectedEmployees = unselectedEmployeeList;
            }
        }

        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove); }
        }

        #endregion

        #endregion

        #region Data Filtering Methods

        void FilterBranches()
        {
            if (attendanceEmployees != null)
            {
                Branches = attendanceEmployees.Where(c => c.companyBranch_id != null).GroupBy(d => d.companyBranch_id).Select(grp => grp.First()).Select(c => new z_CompanyBranches { companyBranch_id = (Guid)c.companyBranch_id, companyBranch_Name = c.companyBranch_Name });
            } 
        }

        void FilterDesignations()
        {
            if (attendanceEmployees != null)
            {
                Designations = attendanceEmployees.Where(c => c.designation_id != null).GroupBy(d => d.designation_id).Select(grp => grp.First()).Select(c => new z_Designation { designation_id = (Guid)c.designation_id, designation = c.designation }); 
            }
        }

        void FilterDepartments()
        {
            if (attendanceEmployees != null)
            {
                Departments = attendanceEmployees.Where(c => c.department_id != null).GroupBy(d => d.department_id).Select(grp => grp.First()).Select(c => new z_Department { department_id = (Guid)c.department_id, department_name = c.department_name }); 
            }
        }

        void FilterGrades()
        {
            if (attendanceEmployees != null)
            {
                Grades = attendanceEmployees.Where(c => c.grade_id != null).GroupBy(d => d.grade_id).Select(grp => grp.First()).Select(c => new z_Grade { grade_id = (Guid)c.grade_id, grade = c.grade }); 
            }
        }

        void FilterEmployee()
        {
            List<AttendEmployeeSummaryView> filterList = new List<AttendEmployeeSummaryView>();
            if(isFilterUnselected && unselectedEmployeeList.Count > 0)
            {
                filterList = unselectedEmployeeList;
            }
            else if(isFilterSelected && selectedEmployeeList.Count > 0)
            {
                filterList = selectedEmployeeList;
            }

            if(filterList.Count > 0 && !isAllCheck)
            {
                IEnumerable<AttendEmployeeSummaryView> filterEmployees = filterList;
                if (currentBranch != null)
                    filterEmployees = filterEmployees.Where(c => c.companyBranch_id == currentBranch.companyBranch_id);
                if (currentDepartment != null)
                    filterEmployees = filterEmployees.Where(c => c.department_id == currentDepartment.department_id);
                if (currentDesignation != null)
                    filterEmployees = filterEmployees.Where(c => c.designation_id == currentDesignation.designation_id);
                if (currentGrade != null)
                    filterEmployees = filterEmployees.Where(c => c.grade_id == currentGrade.grade_id);
                if (currentAttendanceGroup != null && attendanceGroupEmployees != null)
                    filterEmployees = filterEmployees.Where(c => attendanceGroupEmployees.Any(d => d.employee_id == c.employee_id));

                if(isFilterUnselected)
                {
                    UnselectedEmployees = filterEmployees;
                }
                else if(isFilterSelected)
                {
                    SelectedEmployees = filterEmployees;
                }
            }
        }

        void FilterEmployeeByGroup()
        {
            if(attendanceGroupEmployees != null)
            {
                if(isFilterUnselected)
                {
                    UnselectedEmployees = unselectedEmployeeList.Where(c => attendanceGroupEmployees.Any(d => d.employee_id == c.employee_id));
                }
                else if(isFilterSelected)
                {
                    SelectedEmployees = selectedEmployeeList.Where(c => attendanceGroupEmployees.Any(d => d.employee_id == c.employee_id));
                }
                
            }
        }

        #endregion

        #region Data Setting Methods

        void PopulateData()
        {
            this.FilterBranches();
            this.FilterDesignations();
            this.FilterDepartments();
            this.FilterGrades();
            unselectedEmployeeList = allEmployeeList;
            UnselectedEmployees = allEmployeeList;
        }

        void SetDataRange()
        {
            StartDateRange = ((DateTime)currentPeriod.start_date).Date;
            EndDateRange = ((DateTime)currentPeriod.end_date).Date;
        }

        #endregion

        #region Delete Method for Attendance Process

        public ICommand DeleteButton
        {
            get { return new RelayCommand(DeleteAtt); }
        }

        private void DeleteAtt()
        {
            try
            {
                if (ValidateProcess())
                {
                    List<Guid> EmployeeIDList = new List<Guid>();
                    foreach (var item in SelectedEmployees)
                    {
                        EmployeeIDList.Add(item.employee_id);
                    }
                    if (attendServiceClient.DeleteAttendanceProcessEmployeeAndAttendDayWise(EmployeeIDList.ToArray(), StartDateRange, EndDateRange))
                    {
                        clsMessages.setMessage("Previous Processed Attendance Of Selected Employees Have Been Deleted Successfully For The Selected Date Range....");
                    }
                    else
                        clsMessages.setMessage("Previous Processed Attendance Of Selected Employees Deleting Process Failed For The Selected Date Range....");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Previous Processed Attendance Of Selected Employees Deleting Process Failed For The Selected Date Range....");
            }
        }

        #endregion
    }
}
