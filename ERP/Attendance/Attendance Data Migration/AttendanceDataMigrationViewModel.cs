using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;


namespace ERP.Attendance.Attendance_Data_Migration
{
    public class AttendanceDataMigrationViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<mas_Employee> AllEmployee = new List<mas_Employee>();
        public List<mas_Employee> SelectedEmployee = new List<mas_Employee>();
        public List<EmployeePeriodQantityView> allQuntityList = new List<EmployeePeriodQantityView>();
        public List<trns_EmployeePeriodQunatity> EmployeeQuntityList = new List<trns_EmployeePeriodQunatity>();
        public List<trns_EmployeePeriodQunatity> UP_EmployeeQuntityList = new List<trns_EmployeePeriodQunatity>();
        public List<trns_EmployeePeriodQunatity> tempRule = new List<trns_EmployeePeriodQunatity>();
        public List<dtl_EmployeeAttendance> Shift=new List<dtl_EmployeeAttendance>();
        public List<EmployeePeriodQantityView> tempQuntity = new List<EmployeePeriodQantityView>();
        public List<trns_EmployeePeriodQunatity> tempQuntityList = new List<trns_EmployeePeriodQunatity>();
        public List<dtl_EmployeeRule> UpdateList = new List<dtl_EmployeeRule>();
        public List<dtl_EmployeeRule> RuleListTemp = new List<dtl_EmployeeRule>();
        RelayCommand _IncrementAsBackgroundProcess;
        RelayCommand _ResetCounter;
        DispatcherTimer dis = new DispatcherTimer();

        Double _Value;
        bool _IsInProgress;
        int _Min = 0, _Max = 100;

        public AttendanceDataMigrationViewModel()
        {
            RefreshCompanyBranch();
            refreshSections();
            refreshDepatments();
            refreshDesignation();
            refreshGrade();
            refreshEmployee();
            refreshPeriod();
            refreshEmployeeShift();
            refreshEmployeeAttendanceRules();
            //refreshEmployeeRules();
            refreshDataMigrationConfiguration();
            //refreshEmployeeAttendanceShift();
            RefreshEmployeeAttendanceBonus();
            RefreshEmployeeRule();
        }
        #region Company Branches
        private IEnumerable<z_CompanyBranches> _CompanyBranches;
        public IEnumerable<z_CompanyBranches> CompanyBranches
        {
            get { return _CompanyBranches; }
            set
            {
                _CompanyBranches = value;
                this.OnPropertyChanged("CompanyBranches");
            }
        }
        #endregion

        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }

        private IEnumerable<dtl_EmployeeRule> _RuleList;
        public IEnumerable<dtl_EmployeeRule> RuleList
        {
            get { return _RuleList; }
            set { _RuleList = value; this.OnPropertyChanged("RuleList"); }
        }
        

        #region Department

        private IEnumerable<z_Department> _Departments;
        public IEnumerable<z_Department> Departments
        {
            get { return _Departments; }
            set { _Departments = value; this.OnPropertyChanged("Departments"); }
        }
        #endregion

        #region Sections
        private IEnumerable<z_Section> _Sections;
        public IEnumerable<z_Section> Sections
        {
            get { return _Sections; }
            set { _Sections = value; this.OnPropertyChanged("Sections"); }
        }
        #endregion

        #region Designation
        private IEnumerable<z_Designation> _Designations;
        public IEnumerable<z_Designation> Designations
        {
            get { return _Designations; }
            set { _Designations = value; this.OnPropertyChanged("Designations"); }
        }

        #endregion

        #region Grades
        private IEnumerable<z_Grade> _Grades;
        public IEnumerable<z_Grade> Grades
        {
            get { return _Grades; }
            set { _Grades = value; this.OnPropertyChanged("Grades"); }
        }
        #endregion

        #region Periods
        private IEnumerable<z_Period> _Periods;
        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; this.OnPropertyChanged("Periods"); }
        }

        #endregion

        private IEnumerable<dtl_EmployeeAttendance> _EmployeeShift;
        public IEnumerable<dtl_EmployeeAttendance> EmployeeShift
        {
            get { return _EmployeeShift; }
            set { _EmployeeShift = value; this.OnPropertyChanged("EmployeeShift"); }
        }

        private dtl_EmployeeAttendance _CurrentEmployeeShift;
        public dtl_EmployeeAttendance CurrentEmployeeShift
        {
            get { return _CurrentEmployeeShift; }
            set { _CurrentEmployeeShift = value; this.OnPropertyChanged("CurrentEmployeeShift"); }
        }

        private IEnumerable<z_AttendanceAllowance> _AttendanceAllowanceDetail;
        public IEnumerable<z_AttendanceAllowance> AttendanceAllowanceDetail
        {
            get { return _AttendanceAllowanceDetail; }
            set { _AttendanceAllowanceDetail = value; this.OnPropertyChanged("AttendanceAllowanceDetail"); }
        }

        private z_AttendanceAllowance _CurrentAttendanceAllowanceDetail;
        public z_AttendanceAllowance CurrentAttendanceAllowanceDetail
        {
            get { return _CurrentAttendanceAllowanceDetail; }
            set { _CurrentAttendanceAllowanceDetail = value; this.OnPropertyChanged("CurrentAttendanceAllowanceDetail"); }
        }
        private NotifyingProperty cursor = new NotifyingProperty("Cursor", typeof(System.Windows.Input.Cursor), System.Windows.Input.Cursors.Arrow);
        public System.Windows.Input.Cursor Cursor
        {
            get { return (System.Windows.Input.Cursor)GetValue(cursor); }
            set { SetValue(cursor, value); }
        }

        #region current company branch
        private z_CompanyBranches _CurretCompanyBranch;
        public z_CompanyBranches CurretCompanyBranch
        {
            get { return _CurretCompanyBranch; }
            set
            {
                _CurretCompanyBranch = value; this.OnPropertyChanged("CurretCompanyBranch");
                EmployeeFiltering();
            }
        }

        #endregion

        #region current department
        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set { _CurrentDepartment = value; this.OnPropertyChanged("CurrentDepartment"); EmployeeFiltering(); }
        }

        #endregion

        #region current designation
        private z_Designation _CurretDesignation;
        public z_Designation CurretDesignation
        {
            get { return _CurretDesignation; }
            set { _CurretDesignation = value; this.OnPropertyChanged("CurretDesignation"); EmployeeFiltering(); }
        }
        #endregion

        #region current section
        private z_Section _CurretSection;
        public z_Section CurretSection
        {
            get { return _CurretSection; }
            set { _CurretSection = value; this.OnPropertyChanged("CurretSection"); EmployeeFiltering(); }
        }
        #endregion

        #region current grade
        private z_Grade _CurretGrade;
        public z_Grade CurretGrade
        {
            get { return _CurretGrade; }
            set { _CurretGrade = value; this.OnPropertyChanged("CurretGrade"); }
        }
        #endregion

        #region current period
        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set
            {
                _CurrentPeriod = value; this.OnPropertyChanged("CurrentPeriod");
                if (CurrentPeriod != null && CurrentPeriod.period_id != Guid.Empty)
                {
                    refreshEmployeeAttendanceSumarry();
                }
            }
        }
        #endregion

        private IEnumerable<trns_EmployeeAttendanceSumarry> _EmployeeAttendanceSumarry;
        public IEnumerable<trns_EmployeeAttendanceSumarry> EmployeeAttendanceSumarry
        {
            get { return _EmployeeAttendanceSumarry; }
            set { _EmployeeAttendanceSumarry = value; this.OnPropertyChanged("EmployeeAttendanceSumarry"); }
        }

        private trns_EmployeeAttendanceSumarry _CurrentEmployeeAttendanceSumarry;
        public trns_EmployeeAttendanceSumarry CurrentEmployeeAttendanceSumarry
        {
            get { return _CurrentEmployeeAttendanceSumarry; }
            set { _CurrentEmployeeAttendanceSumarry = value; this.OnPropertyChanged("CurrentEmployeeAttendanceSumarry"); }
        }

        private IEnumerable<EmployeePeriodQantityView> _PeriodQuntity;
        public IEnumerable<EmployeePeriodQantityView> PeriodQuntity
        {
            get { return _PeriodQuntity; }
            set { _PeriodQuntity = value; this.OnPropertyChanged("CurrentPeriodQuntity"); }
        }

        private EmployeePeriodQantityView _CurrentPeriodQuntity;
        public EmployeePeriodQantityView CurrentPeriodQuntity
        {
            get { return _CurrentPeriodQuntity; }
            set { _CurrentPeriodQuntity = value; this.OnPropertyChanged("CurrentPeriodQuntity"); }
        }


        //private IEnumerable<EmployeePeriodQantityView> _QuantityPeriods;
        //public IEnumerable<EmployeePeriodQantityView> QuantityPeriods
        //{
        //    get { return _QuantityPeriods; }
        //    set { _QuantityPeriods = value; OnPropertyChanged("QuantityPeriods"); }
        //}

        //private EmployeePeriodQantityView _CurrentQuantityPeriod;
        //public EmployeePeriodQantityView CurrentQuantityPeriod
        //{
        //    get { return _CurrentQuantityPeriod; }
        //    set { _CurrentQuantityPeriod = value; OnPropertyChanged("CurrentQuantityPeriod"); }
        //}


        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee"); }
        }

        private IEnumerable<mas_Employee> _SelectedEmployees;
        public IEnumerable<mas_Employee> SelectedEmployees
        {
            get { return _SelectedEmployees; }
            set { _SelectedEmployees = value; this.OnPropertyChanged("SelectedEmployees"); }
        }

        private mas_Employee _CurrentSelectedEmployee;
        public mas_Employee CurrentSelectedEmployee
        {
            get { return _CurrentSelectedEmployee; }
            set { _CurrentSelectedEmployee = value; this.OnPropertyChanged("CurrentSelectedEmployee"); }
        }

        private IEnumerable<dtl_AttendanceBonus> _AttendanceBonus;
        public IEnumerable<dtl_AttendanceBonus> AttendanceBonus
        {
            get { return _AttendanceBonus; }
            set { _AttendanceBonus = value; this.OnPropertyChanged("AttendanceBonus"); }
        }
        
        private void RefreshCompanyBranch()
        {
            this.serviceClient.GetAllCompanyBranchCompleted += (s, e) =>
            {
                this.CompanyBranches = e.Result;
            };
            this.serviceClient.GetAllCompanyBranchAsync();
        }
        private void RefreshEmployeeRule()
        {
            RuleList = this.serviceClient.GetEmployeeRule().Where(z => z.isactive == true);
            foreach (var item in RuleList)
            {
                RuleListTemp.Add(item);
            }
        }

        private void RefreshEmployeeAttendanceBonus()
        {
            this.serviceClient.GetAttendanceBonusDetailCompleted += (s, e) =>
            {
                this.AttendanceBonus = e.Result;
            };
            this.serviceClient.GetAttendanceBonusDetailAsync();
        }

        private void refreshDepatments()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Departments = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetDepartmentsAsync();
        }

        private void refreshSections()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
            {
                this.Sections = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetSectionsAsync();
        }

        private void refreshDesignation()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                this.Designations = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetDesignationsAsync();
        }

        private void refreshGrade()
        {
            this.serviceClient.GetGradeCompleted += (s, e) =>
            {
                this.Grades = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetGradeAsync();
        }

        private void refreshPeriod()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                this.Periods = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetPeriodsAsync();
        }

        private void refreshEmployee()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                AllEmployee.Clear();
                this.Employees = e.Result;
                foreach (var item in Employees)
                {
                    AllEmployee.Add(item);
                }
            };
            this.serviceClient.GetEmployeesAsync();
        }

        private void refreshEmployeeShift()
        {
            this.serviceClient.GetEmployeeAttendanceCompleted += (s, e) =>
            {
                Shift.Clear();
                this.EmployeeShift = e.Result;
                foreach (var item in e.Result)
	             {
                    Shift.Add(item);
		 
	            }
            };
            this.serviceClient.GetEmployeeAttendanceAsync();
        }

        #region Add and Remove Button Command

        public ICommand AddOne
        {
            get { return new RelayCommand(addOne, addOneCanExecute); }
        }

        public ICommand RemoveOne
        {
            get { return new RelayCommand(removeOne, removeOneCanExecute); }
        }

        public ICommand AddAll
        {
            get { return new RelayCommand(addAll, addAllCanExecute); }
        }

        public ICommand RemoveAll
        {
            get { return new RelayCommand(removeAll, removeAllCanExecute); }
        }

        #endregion

        #region Add and Remove Button CanExecute

        private bool removeAllCanExecute()
        {
            if (this.SelectedEmployees == null)
                return false;
            else
                return true;
        }

        private bool addAllCanExecute()
        {
            if (Employees == null)
                return false;
            else
                return true;
        }

        private bool removeOneCanExecute()
        {
            if (CurrentSelectedEmployee == null)
                return false;
            else
                return true;
        }

        private bool addOneCanExecute()
        {
            if (CurrentEmployee == null)
                return false;
            else
                return true;
        }

        #endregion

        #region Add and remove Methord

        private void removeAll()
        {
            Employees = null;
            SelectedEmployees = null;
            foreach (mas_Employee emp in SelectedEmployee)
            {
                AllEmployee.Add(emp);
            }
            SelectedEmployee.Clear();
            this.Employees = AllEmployee;
            this.SelectedEmployees = SelectedEmployee;
            this.RefreshEmploeesForSelection();
        }

        private void addAll()
        {
            SelectedEmployees = null;
            foreach (mas_Employee item in Employees.ToList())
            {
                SelectedEmployee.Add(item);
                AllEmployee.Remove(item);
            }
            Employees = null;
            //AllEmployee = null;
            this.Employees = AllEmployee;
            this.SelectedEmployees = SelectedEmployee;
            this.RefreshEmploeesForSelection();
        }

        private void removeOne()
        {
            AllEmployee.Add(CurrentSelectedEmployee);
            SelectedEmployee.Remove(CurrentSelectedEmployee);
            Employees = null;
            SelectedEmployees = null;
            Employees = AllEmployee;
            SelectedEmployees = SelectedEmployee;
            this.RefreshEmploeesForSelection();
        }

        private void addOne()
        {

            SelectedEmployee.Add(CurrentEmployee);
            AllEmployee.Remove(CurrentEmployee);
            Employees = null;
            SelectedEmployees = null;
            Employees = AllEmployee;
            SelectedEmployees = SelectedEmployee;
            this.RefreshEmploeesForSelection();

        }

        public void RefreshEmploeesForSelection()
        {
            CurretCompanyBranch = null;
            CurrentDepartment = null;
            CurretSection = null;
            CurretDesignation = null;
            CurretGrade = null;
        }

        #endregion

        #region Filter Employee

        public void EmployeeFiltering()
        {
        //    if (Employees != null)
        //    {
        //        if (CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty && CurrentDepartment == null)
        //        {
        //            Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id);
        //            Departments = Departments.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id);
        //        }
        //        if (CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty && CurretSection == null)
        //        {
        //            if (CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty && CurretSection == null)
        //            {
        //                Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id);
        //                //Sections = Sections.Where(z => z.department_id == CurrentDepartment.department_id);
        //            }
        //            else
        //            {
        //                Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id);
        //            }
        //        }
        //        if (CurretSection != null && CurretSection.section_id != Guid.Empty)
        //        {

        //            if (CurretCompanyBranch != null && CurrentDepartment != null && CurretSection != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id);
        //            }
        //            else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty)
        //            {
        //                Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.department_id == CurrentDepartment.department_id);
        //            }
        //            else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty)
        //            {
        //                Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.branch_id == CurretCompanyBranch.companyBranch_id);
        //            }
        //            else
        //            {
        //                Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id);
        //            }


        //        }
        //        if (CurretDesignation != null && CurretDesignation.designation_id != Guid.Empty)
        //        {
        //            if (CurretDesignation != null && CurretCompanyBranch != null && CurrentDepartment != null && CurretSection != null && CurretDesignation != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id);
        //            }
        //            else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty && CurretDesignation != null && CurretDesignation.designation_id == Guid.Empty)
        //            {
        //                Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id && z.section_id == CurretSection.section_id);
        //            }
        //            else if (CurretSection != null && CurretDesignation != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.designation_id == CurretDesignation.designation_id);
        //            }
        //            else if (CurretCompanyBranch != null && CurretDesignation != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.designation_id == CurretDesignation.designation_id);
        //            }
        //            else if (CurrentDepartment != null && CurretDesignation != null)
        //            {
        //                Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id);
        //            }
        //            else
        //            {
        //                Employees = AllEmployee.Where(z => z.designation_id == CurretDesignation.designation_id);
        //            }
        //        }
        //        if (CurretGrade != null && CurretGrade.grade_id != Guid.Empty)
        //        {
        //            Employees = AllEmployee.Where(z => z.grade_id == CurretGrade.grade_id);
        //        }

       //  }
        }

        #endregion


        private void refreshEmployeeRules()
        {
            this.serviceClient.GetAllEmnployeePeriodQuantityCompleted += (s, e) =>
            {
                allQuntityList.Clear();
                this.PeriodQuntity = e.Result.Where(t=>t.isActive==true);
                foreach (var item in PeriodQuntity)
                {
                    allQuntityList.Add(item);
                }
            };
            this.serviceClient.GetAllEmnployeePeriodQuantityAsync();
        }

        public ICommand ProcessBtn
        {
            get { return new RelayCommand(Process, ProcessCanExecute); }
        }

        private IEnumerable<z_Datamigration_Configuration> _DataMigrationConfiguration;
        public IEnumerable<z_Datamigration_Configuration> DataMigrationConfiguration
        {
            get { return _DataMigrationConfiguration; }
            set { _DataMigrationConfiguration = value; this.OnPropertyChanged("DataMigrationConfiguration"); }
        }

      
        
        
        private void Process()
        {
            if (clsSecurity.GetSavePermission(312))
            {
                refreshEmployeeRules();

                Cursor = System.Windows.Input.Cursors.Wait;
                {
                    foreach (var Emp in SelectedEmployees)
                    {
                        decimal normalovertime = 0;
                        decimal doubleovertime = 0;
                        decimal nopayfullday = 0;
                        decimal nopayhelfday = 0;
                        decimal nopayshortday = 0;
                        decimal leavesortleave = 0;
                        decimal latein = 0;
                        decimal eaelyout = 0;
                        decimal allnopay = 0;
                        decimal extradays = 0;
                        decimal totallatehours = 0;
                        decimal tripleot = 0;
                        decimal poyaot = 0;
                        decimal allLeaveDays = 0;
                        decimal allAbsentForAttendanceAllowance = 0;
                        decimal extraHalfdayComingFromShortleave = 0;


                        RuleList = RuleListTemp;
                        CurrentEmployeeShift = EmployeeShift.FirstOrDefault(z => z.employee_id == Emp.employee_id);
                        CurrentEmployeeAttendanceSumarry = EmployeeAttendanceSumarry.FirstOrDefault(s => s.employee_id == Emp.employee_id);
                        if (CurrentEmployeeAttendanceSumarry != null)
                        {
                            EmployeePeriodQantityView authorizeNopayList = new EmployeePeriodQantityView();
                            EmployeePeriodQantityView unauthorizeNopayList = new EmployeePeriodQantityView();
                            //authorizeNopayList=PeriodQuntity.FirstOrDefault(x => x.employee_id == Emp.employee_id && x.rule_id== HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.AllowanceAuthorizednopay)) ;
                            //unauthorizeNopayList = PeriodQuntity.FirstOrDefault(x => x.employee_id == Emp.employee_id && x.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.AllowanceUnauthorizedNopay2));
                            RuleList = RuleListTemp;
                            RuleList = RuleList.Where(x => x.employee_id == Emp.employee_id);


                            normalovertime = decimal.Parse((getTotalMinits(CurrentEmployeeAttendanceSumarry.actual_ot_outtime) + getTotalMinits(CurrentEmployeeAttendanceSumarry.actual_ot_intime) + getTotalMinits(CurrentEmployeeAttendanceSumarry.freeday_work_time)).ToString()) / 60;
                            doubleovertime = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.holiday_work_time).ToString()) / 60;
                            tripleot = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.mercantile_work_time).ToString()) / 60;
                            poyaot = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.poya_work_time).ToString()) / 60;

                            latein = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.late_in_time).ToString());


                            eaelyout = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.early_out_time).ToString());

                            nopayfullday = (decimal.Parse(CurrentEmployeeAttendanceSumarry.nopay_fulldays_count));//+ (decimal.Parse(CurrentEmployeeAttendanceSumarry.evening_halfday_nopay_count)) / 2 + decimal.Parse(CurrentEmployeeAttendanceSumarry.nopay_fulldays_count);
                            nopayhelfday = (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_halfday_nopay_count)) / 2 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.evening_halfday_nopay_count)) / 2;
                            nopayshortday = (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_short_day_nopay)) / 4 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_short_day_nopay)) / 4;
                            leavesortleave = (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_short_day_leave)) / 4 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.evening_short_day_leave)) / 4;

                            allLeaveDays = (decimal.Parse(CurrentEmployeeAttendanceSumarry.leave_fulldays_count)) + (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_halfday_leave_count)) / 2 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.evening_halfday_leave_count)) / 2 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_short_day_leave)) / 4 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.evening_short_day_leave)) / 4;
                            extradays = (decimal.Parse(CurrentEmployeeAttendanceSumarry.aditional_day_count));
                            totallatehours = latein + eaelyout;

                            if (leavesortleave + nopayshortday > 1)
                            {
                                extraHalfdayComingFromShortleave = ((leavesortleave + nopayshortday) - 1) * 2;
                                nopayshortday = 0;
                            }
                            else
                            {
                                nopayshortday = 0;
                            }
                            allnopay = nopayfullday + nopayhelfday + nopayshortday;

                            allAbsentForAttendanceAllowance = allnopay + allLeaveDays + nopayshortday;

                            foreach (var period_quntityitem in RuleList.ToList())
                            {
                                bool isApply = false;
                                trns_EmployeePeriodQunatity qty = new trns_EmployeePeriodQunatity();
                                qty.employee_id = Emp.employee_id;
                                qty.period_id = CurrentPeriod.period_id;
                                qty.rule_id = period_quntityitem.rule_id;

                                if (period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.NormalOverTimeShopAndOffice) || period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.NormalOverTimeWagesBoards))
                                {
                                    qty.quantity = normalovertime;
                                }
                                else if ((period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.DoubleOverTimeShopAndOffice) || period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.DoubleOverTimeWagesBoards)))
                                {
                                    qty.quantity = doubleovertime;
                                }
                                else if ((period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.TripleOTShopAndOffice) || period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.TripleOTWagesBoards)))
                                {
                                    qty.quantity = tripleot;
                                }
                                else if ((period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.NoPayShopAndOffice) || period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.NoPayWagesBoards)))
                                {
                                    qty.quantity = allnopay;
                                }
                                else if ((period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.extaraDayShopAndOffice) || period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.extarDayWagesBord)))
                                {
                                    qty.quantity = extradays;
                                }
                                else if ((period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.LateShopAndOffice) || period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.LateWagesBoards)))
                                {
                                    qty.quantity = latein;
                                }
                                else if ((period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.ShortLiveHalfdaywages) || period_quntityitem.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.ShortLiveHalfdayShopandOffice)))
                                {
                                    qty.quantity = extraHalfdayComingFromShortleave;
                                }
                                else
                                {
                                    z_Datamigration_Configuration dym = DataMigrationConfiguration.FirstOrDefault(z => z.rule_id == period_quntityitem.rule_id);
                                    if (dym == null)
                                    {
                                        qty.quantity = 0;
                                    }
                                    else
                                    {
                                        if (dym.is_active == true)
                                        {
                                            qty.quantity = dym.default_qty;

                                            if (dym.is_update == true)
                                            {
                                                trns_EmployeePeriodQunatity up_qty = new trns_EmployeePeriodQunatity();
                                                up_qty.employee_id = Emp.employee_id;
                                                up_qty.period_id = CurrentPeriod.period_id;
                                                up_qty.save_user_id = clsSecurity.loggedUser.user_id;
                                                up_qty.save_datetime = DateTime.Now;
                                                up_qty.modified_user_id = clsSecurity.loggedUser.user_id;
                                                up_qty.modified_datetime = DateTime.Now;
                                                up_qty.delete_user_id = clsSecurity.loggedUser.user_id;
                                                up_qty.delete_datetime = DateTime.Now;
                                                up_qty.is_proceed = false;
                                                up_qty.isdelete = false;
                                                UP_EmployeeQuntityList.Add(up_qty);
                                                isApply = true;

                                            }
                                        }
                                        else
                                        {
                                            qty.quantity = 0;
                                            isApply = true;
                                        }
                                    }

                                }
                                qty.save_user_id = clsSecurity.loggedUser.user_id;
                                qty.save_datetime = DateTime.Now;
                                qty.modified_user_id = clsSecurity.loggedUser.user_id;
                                qty.modified_datetime = DateTime.Now;
                                qty.delete_user_id = clsSecurity.loggedUser.user_id;
                                qty.delete_datetime = DateTime.Now;
                                qty.is_proceed = false;
                                qty.isdelete = false;
                                if (isApply == false)
                                {
                                    EmployeeQuntityList.Add(qty);
                                    isApply = false;
                                }

                            }
                        }

                    }
                    if (EmployeeQuntityList.Count > 0)
                    {
                        foreach (var item in tempRule)
                        {
                            EmployeeQuntityList.Add(item);
                        }

                        if (serviceClient.saveQuantityFromAttendanceDataMigration(EmployeeQuntityList.ToArray(), CurrentPeriod.period_id))
                        {
                            Cursor = System.Windows.Input.Cursors.Arrow;
                            MessageBox.Show("Attendance Migration Process Successfully ");
                            MessageBoxResult result = MessageBox.Show("Do you want to Start Attendance Bonus Process ", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {


                                AttendenceBonusCalculation();

                            }
                        }
                        else
                        {
                            MessageBox.Show("Attendance Migration Process Fail ");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Employee Period Quantity Exist");
                    }



                }
            }
            else
                clsMessages.setMessage("You don't have permission to Migrate this record(s)");
        }

        public void AttendenceBonusCalculation()
        {
            Cursor = System.Windows.Input.Cursors.Wait;

            foreach (var Emp in SelectedEmployees)
            {
                
                decimal nopayfullday = 0;
                decimal nopayhelfday = 0;
                decimal nopayshortday = 0;
                decimal latein = 0;
                decimal allnopay = 0;
                decimal poyaot = 0;
                decimal extraday = 0;
                decimal allLeaveDays = 0;
                decimal allAbsentForAttendanceAllowance = 0;


                RuleList = RuleListTemp;
                CurrentEmployeeShift = EmployeeShift.FirstOrDefault(z => z.employee_id == Emp.employee_id);
                CurrentEmployeeAttendanceSumarry = EmployeeAttendanceSumarry.FirstOrDefault(s => s.employee_id == Emp.employee_id);
                if (CurrentEmployeeAttendanceSumarry != null)
                {
                    PeriodQuntity = allQuntityList;
                    PeriodQuntity = PeriodQuntity.Where(x => x.employee_id == Emp.employee_id);
                    tempQuntity.Clear();
                    foreach (var Qt_temp_item in PeriodQuntity)
                    {
                        tempQuntity.Add(Qt_temp_item);
                    }

                    poyaot = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.poya_work_time).ToString()) / 60;
                    extraday = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.poya_work_time).ToString());

                    latein = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.late_in_time).ToString());

                
                    latein = decimal.Parse(getTotalMinits(CurrentEmployeeAttendanceSumarry.late_in_time).ToString());
                    
                    decimal halfday_from_short_live = 0;
                    nopayfullday = (decimal.Parse(CurrentEmployeeAttendanceSumarry.nopay_fulldays_count));
                    nopayhelfday = (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_halfday_nopay_count)) / 2 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.evening_halfday_nopay_count)) / 2;
                    //nopayshortday = (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_short_day_nopay)) / 4 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_short_day_nopay)) / 4 ;
                  
                    allLeaveDays = (decimal.Parse(CurrentEmployeeAttendanceSumarry.leave_fulldays_count)) + (decimal.Parse(CurrentEmployeeAttendanceSumarry.morning_halfday_leave_count)) / 2 + (decimal.Parse(CurrentEmployeeAttendanceSumarry.evening_halfday_leave_count)) / 2 ;
                   
                   
                    allnopay = nopayfullday + nopayhelfday + nopayshortday;

                    allAbsentForAttendanceAllowance = allnopay + allLeaveDays + nopayshortday + halfday_from_short_live/2;

                    dtl_EmployeeRule tem_att_bonus_3500 = RuleListTemp.FirstOrDefault(z => z.rule_id == Guid.Parse("ce921800-e219-42a7-8fb5-6498abef6f26"));
                   

                    List<dtl_AttendanceBonus> Att_Bonus_3500 = new List<dtl_AttendanceBonus>();
                   // List<dtl_AttendanceBonus> Att_Bonus_1000 = new List<dtl_AttendanceBonus>();
                    List<dtl_AttendanceBonus> Att_Bonus = new List<dtl_AttendanceBonus>();

                    if (tem_att_bonus_3500 != null)
                    {
                        Att_Bonus_3500 = AttendanceBonus.OrderByDescending(z => z.value).ToList().Where(c => c.is_attendance == true && c.benifit_rule_id == tem_att_bonus_3500.rule_id).ToList();
                        Att_Bonus = Att_Bonus_3500;
                    }
                  
                    List<dtl_AttendanceBonus> Pro_Bonus = AttendanceBonus.OrderByDescending(z => z.value).ToList().Where(c => c.is_attendance == false).ToList();
                    bool is_ok_attendanceBonus = false;
                 

                    foreach (var item_bonus in Att_Bonus)
                    {
                        EmployeePeriodQantityView deduction_qty_view = tempQuntity.FirstOrDefault(z=>z.rule_id==item_bonus.deduction_rule_id);
                        EmployeePeriodQantityView benifit_qty_view = tempQuntity.FirstOrDefault(z => z.rule_id == item_bonus.benifit_rule_id);
                       // EmployeePeriodQantityView Proformance_qty_view = tempQuntity.FirstOrDefault(z => z.rule_id == HelperClass.AttendanceRuleData.GetAttendanceRule(HelperClass.AttendanceRuleName.ProformanceBonus));

                        if (deduction_qty_view != null && benifit_qty_view !=null)
                        {
                            if (item_bonus.is_presentage == false)
                            {
                                if (is_ok_attendanceBonus == false)
                                {
                                    if (item_bonus.value <= allAbsentForAttendanceAllowance)
                                    {
                                        dtl_EmployeeRule rule = new dtl_EmployeeRule();
                                        rule.rule_id = (Guid)item_bonus.deduction_rule_id;
                                        rule.employee_id = Emp.employee_id;

                                        
                                         
                                                if (allAbsentForAttendanceAllowance == decimal.Parse("0"))
                                                {
                                                    rule.special_amount = 0;
                                                }
                                                else if (allAbsentForAttendanceAllowance == decimal.Parse("0.5"))
                                                {
                                                    rule.special_amount = item_bonus.deduct_value;
                                                }
                                                else if (allAbsentForAttendanceAllowance == decimal.Parse("1"))
                                                {
                                                    rule.special_amount = item_bonus.deduct_value;
                                                }
                                                else if (allAbsentForAttendanceAllowance == decimal.Parse("1.5"))
                                                {
                                                    rule.special_amount = item_bonus.deduct_value;
                                                }
                                                else
                                                {
                                                    dtl_AttendanceBonus att = Att_Bonus.LastOrDefault();
                                                    if (att != null)
                                                    {
                                                        rule.special_amount = item_bonus.deduct_value;
                                                    }


                                                }
                                        rule.is_special = true;
                                        rule.save_user_id = clsSecurity.loggedUser.user_id;
                                        rule.save_datetime = System.DateTime.Now;
                                        rule.modified_datetime = System.DateTime.Now;
                                        rule.modified_user_id = clsSecurity.loggedUser.user_id;
                                        rule.delete_datetime = System.DateTime.Now;
                                        rule.delete_user_id = clsSecurity.loggedUser.user_id;
                                        UpdateList.Add(rule);
                                        is_ok_attendanceBonus = true;


                                    }
                                    else if (allAbsentForAttendanceAllowance <=2)
                                    {
                                        dtl_EmployeeRule rule = new dtl_EmployeeRule();
                                        rule.rule_id = (Guid)item_bonus.deduction_rule_id;
                                        rule.employee_id = Emp.employee_id;
                                        rule.special_amount = 0;
                                        rule.is_special = true;
                                        rule.save_user_id = clsSecurity.loggedUser.user_id;
                                        rule.save_datetime = System.DateTime.Now;
                                        rule.modified_datetime = System.DateTime.Now;
                                        rule.modified_user_id = clsSecurity.loggedUser.user_id;
                                        rule.delete_datetime = System.DateTime.Now;
                                        rule.delete_user_id = clsSecurity.loggedUser.user_id;
                                        UpdateList.Add(rule);
                                        is_ok_attendanceBonus = true;
                                    }
                                }
                            }
                            else
                            {
                                // if attendance bonus is presentage 
                            }
                        }
                    }

                }
            }
            if (UpdateList.Count > 0)
            {
                if (serviceClient.AttendanceBonusUpdate(UpdateList.ToArray()))
                {
                    MessageBox.Show("Attendance Bonus Update Successfully ");
                    Cursor = System.Windows.Input.Cursors.Arrow;
                }
                else
                {
                    MessageBox.Show("Attendance Bonus Update fail ");
                }
            }
            else
            {
                MessageBox.Show("No Recode for Update ");
            }

        }

        private bool ProcessCanExecute()
        {
            if (this.SelectedEmployee.Count == 0)
                return false;
            if (this.CurrentPeriod == null)
                return false;
            else
                return true;
        }

        private void refreshEmployeeAttendanceSumarry()
        {
            this.EmployeeAttendanceSumarry = this.serviceClient.GetEmployeePayrollSumarryForPeriod(CurrentPeriod.period_id);
        }

        private void refreshEmployeeAttendanceRules()
        {
            this.serviceClient.GetEmployeeAttendanceAllowanceDetailCompleted += (s, e) =>
            {
                this.AttendanceAllowanceDetail = e.Result;
            };
            this.serviceClient.GetEmployeeAttendanceAllowanceDetailAsync();
        }

        private void refreshDataMigrationConfiguration()
        {
            this.serviceClient.GetDatamigrationConfigurationCompleted += (s, e) =>
            {
                this.DataMigrationConfiguration = e.Result;
            };
            this.serviceClient.GetDatamigrationConfigurationAsync();
        }

        public int getTotalMinits(string totaltime)
        {
            int minit = 0;
            try
            {

                string[] words = totaltime.ToString().Split(':');
                string hours = words[0].ToString();
                string minits = words[1];
                int inthours = int.Parse(hours.ToString());
                int intminits = int.Parse(minits.ToString());
                minit = (inthours * 60 + intminits);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.InnerException.Message);
            }
            return minit;
        }

        public bool IsInProgress
        {
            get { return _IsInProgress; }
            set
            {
                _IsInProgress = value;
                this.OnPropertyChanged("IsInProgress");
                this.OnPropertyChanged("IsNotInProgress");
            }
        }

        public bool IsNotInProgress
        {
            get { return !IsInProgress; }
        }

        public int Max
        {
            get { return _Max; }
            set { _Max = value; this.OnPropertyChanged("Max"); }
        }

        public int Min
        {
            get { return _Min; }
            set { _Min = value; this.OnPropertyChanged("Min"); }
        }

        public Double Value
        {
            get { return _Value; }
            set
            {
                if (value <= _Max)
                {
                    if (value >= _Min) { _Value = value; }
                    else { _Value = _Min; }
                }
                else { _Value = _Max; }
                this.OnPropertyChanged("Value");
            }
        }

        bool incrementCanExecute()
        {
            return true;
        }


        public ICommand IncrementAsBackgroundProcess
        {
            get
            {
                if (_IncrementAsBackgroundProcess == null)
                {
                    _IncrementAsBackgroundProcess = new RelayCommand(IncrementProgressBackgroundWorker, incrementCanExecute);
                }
                return _IncrementAsBackgroundProcess;
            }
        }

        public ICommand ResetCounter
        {
            get
            {
                if (_ResetCounter == null)
                {
                    _ResetCounter = new RelayCommand(Reset, incrementCanExecute);
                }
                return _ResetCounter;
            }
        }

        public void IncrementProgressBackgroundWorker()
        {
            if (IsInProgress)
                return;

            Reset();
            IsInProgress = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            dis.Tick += dis_Tick;
            dis.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dis.Start();

        }

        private void Reset()
        {
            Value = Min;
        }

        void dis_Tick(object sender, EventArgs e)
        {
            Value++;
            //if (Value == 5)
            //{
            //    StatusString1 = "Starting Daily Attendance Process ....";
            //    dis.Stop();
            //    StatusString1 = null;
            //    StatusString2 = "Getting All Employees ...";
            //    Employees = this.serviceClient.GetAllEmployeeDetail();
            //    dis.Start();
            //    // dis.Interval = new TimeSpan(0, 0, 0, 1, 0);
            //    StatusString1 = "Employee Download Sussfully ...";

            //}
            //if (Value == 10)
            //{
            //    dis.Stop();
            //    StatusString1 = "";
            //    StatusString2 = "Downloading Attendance Data ...";
            //    AttendanceData = this.serviceClient.GetAttendanceFromDateRange(CurretDate.Date.AddDays(-1), CurretDate.Date.AddDays(1));
            //    dis.Start();
            //    StatusString1 = "Attendance Data Download Sussfully ...";
            //}
            //if (Value == 25)
            //{
            //    dis.Stop();
            //    if (StartProcess())
            //    {
            //        StatusString1 = "Daily Attendance Process Sussfully ...";
            //    }
            //    else
            //    {
            //        StatusString1 = "Daily Attendance Process Fail ...";
            //        StatusString2 = "Please Tryagain ..";
            //    }
            //    dis.Interval = new TimeSpan(0, 0, 0, 0, 1);
                dis.Start();
            //}
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsInProgress = false;
            // MessageBox.Show("Process Compleated");

        }
        public bool CheckLeaveEntitleMentWorker(Guid empid)
        {
           if(Shift.Count>0)
           {
               dtl_EmployeeAttendance attendance_shift=Shift.FirstOrDefault(z=>z.employee_id==empid && z.is_leave_applicable==true);
               if(attendance_shift !=null )
               {
                   return true;
               }
               else
               {
                   return false;
               }
           }
            return false;
        }
       
    
	
    }
}
