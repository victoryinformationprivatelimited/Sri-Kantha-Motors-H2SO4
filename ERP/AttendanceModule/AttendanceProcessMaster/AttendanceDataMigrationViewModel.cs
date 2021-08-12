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
    class AttendanceDataMigrationViewModel:ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor

        public AttendanceDataMigrationViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshEmployeeSummaryDetails();
            this.RefreshAttendancePeriods();
            this.RefreshAttendanceGroups();
            isPartial = true;
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
            get { return selectedEmployees; }
            set { selectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }

        IEnumerable<AttendEmployeeSummaryView> unselectedEmployees;
        public IEnumerable<AttendEmployeeSummaryView> UnselectedEmployees
        {
            get { return unselectedEmployees; }
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
            get { return attendancePeriods; }
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
                    UnselectedEmployees = null;
                    UnselectedEmployees = unselectedEmployeeList;
                    CurrentAttendanceGroup = null;
                }
                else
                {
                    this.FilterEmployee();
                }

            }
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

        private bool isPartial;

        public bool IsPartial
        {
            get { return isPartial; }
            set { isPartial = value; OnPropertyChanged("IsPartial"); }
        }

        private bool isFull;

        public bool IsFull
        {
            get { return isFull; }
            set { isFull = value; OnPropertyChanged("IsFull"); }
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
                AttendancePeriods = e.Result;
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
                    if (attendanceGroupEmployees != null)
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

        #region Add Button

        void Add()
        {
            if (selectEmpList.Count > 0)
            {
                foreach (AttendEmployeeSummaryView addEmp in selectEmpList)
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
            if (selectEmpList.Count > 0)
            {
                foreach (AttendEmployeeSummaryView removeEmp in selectEmpList)
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

        #region Process Button
        
        public ICommand ProcessButton
        {
            get { return new RelayCommand(Process); }
        }

        private void Process()
        {
            if (clsSecurity.GetSavePermission(312))
            {
                if (IsValidPeriod() && selectedEmployeeList.Count > 0)
                {
                    BusyBox.ShowBusy("Please Wait Until Attendance Data Migrated");
                    List<AttendanceEmployeeDataView> empList = selectedEmployeeList.Select(c => new AttendanceEmployeeDataView { emp_id = c.emp_id.TrimStart('0'), employee_id = c.employee_id, first_name = c.first_name, second_name = c.second_name }).ToList();
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
                            endIndex = startIndex + baseValue;
                        }

                        var processedEMps = empList.Select((c, index) => new { Idx = index, EmpID = c.employee_id }).OrderBy(c => c.Idx);
                        List<AttendanceEmployeeDataView> processingEmpList = empList.Where(c => processedEMps.Where(d => d.Idx >= startIndex && d.Idx < endIndex).Any(d => d.EmpID == c.employee_id)).ToList();
                        try
                        {
                            if (attendServiceClient.BeginAttendanceDataMigration(processingEmpList.ToArray(), currentPeriod.start_date.Value, currentPeriod.end_date.Value, new ERP.AttendanceService.z_Period { period_id = currentPeriod.period_id, period_name = currentPeriod.period_name, start_date = currentPeriod.start_date, end_date = currentPeriod.end_date }, clsSecurity.loggedUser.user_id, isPartial, isFull))
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
                        clsMessages.setMessage("Some part of data migration process not completed");
                    }
                    else
                    {
                        BusyBox.CloseBusy();
                        clsMessages.setMessage("Data migration process completed successfully");
                    }
                }

                else
                {
                    clsMessages.setMessage("At least one employee should be selected");
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to data migrate");
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
            if (unselectedEmployeeList.Count > 0 && !isAllCheck)
            {
                if (currentBranch != null)
                    UnselectedEmployees = unselectedEmployeeList.Where(c => c.companyBranch_id == currentBranch.companyBranch_id);
                if (currentDepartment != null)
                    UnselectedEmployees = unselectedEmployeeList.Where(c => c.department_id == currentDepartment.department_id);
                if (currentDesignation != null)
                    UnselectedEmployees = unselectedEmployeeList.Where(c => c.designation_id == currentDesignation.designation_id);
                if (currentGrade != null)
                    UnselectedEmployees = unselectedEmployeeList.Where(c => c.grade_id == currentGrade.grade_id);
                if (currentAttendanceGroup != null && attendanceGroupEmployees != null)
                    UnselectedEmployees = unselectedEmployeeList.Where(c => attendanceGroupEmployees.Any(d => d.employee_id == c.employee_id));
            }
        }

        void FilterEmployeeByGroup()
        {
            if (attendanceGroupEmployees != null)
            {
                UnselectedEmployees = unselectedEmployeeList.Where(c => attendanceGroupEmployees.Any(d => d.employee_id == c.employee_id));
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

        #region validation Methods

        bool IsValidPeriod()
        {
            if(currentPeriod == null)
            {
                clsMessages.setMessage("Time period should be selected");
                return false;
            }

            return true;
        }

        #endregion
    }
}
