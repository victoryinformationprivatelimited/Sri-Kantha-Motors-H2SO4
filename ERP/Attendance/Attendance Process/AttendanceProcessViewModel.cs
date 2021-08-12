using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace ERP.Attendance.Attendance_Process
{

    public class AttendanceProcessViewModel:ViewModelBase
    {
     private ERPServiceClient serviceClient = new ERPServiceClient();
     public List<EmployeeSumarryView> AllEmployee = new List<EmployeeSumarryView>();
     public List<EmployeeSumarryView> SelectedEmployee = new List<EmployeeSumarryView>();
     public List<EmployeeSumarryView> ShiftNotDifineEmployee = new List<EmployeeSumarryView>();
     public List<trns_EmployeeAttendance> EmployeeAttendance = new List<trns_EmployeeAttendance>();
     public List<trns_EmployeeDoubleOT> DoubleOtDetail = new List<trns_EmployeeDoubleOT>();
     public List<dtl_Shift> AllShift = new List<dtl_Shift>();
    // public List<trns_AttendanceSumarry> sumarry = new List<trns_AttendanceSumarry>();
     public List<trns_EmployeeOtPeriod> OtPeriodList = new List<trns_EmployeeOtPeriod>();

     bool isPriviousPayroll = false;
     bool isDeletePayrollProcess = false;
     
     bool isPayrollCompleate = false;

        RelayCommand _IncrementAsBackgroundProcess;
        RelayCommand _ResetCounter;

        DispatcherTimer dis = new DispatcherTimer();

        Double _Value;
        bool _IsInProgress;
        int _Min = 0, _Max = 100;

        public AttendanceProcessViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.AttendanceProcess), clsSecurity.loggedUser.user_id))
            {
                RefreshCompanyBranch();
                refreshDepatments();
                refreshSections();
                refreshDesignation();
                refreshGrade();
                refreshEmployee();
                refreshPeriod();
                refreshShiftCatagory();
                refreshShiftDetaila();
                BreakTime = false;
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }

        }
        private string _StatusString2;
        public string StatusString2
        {
            get { return _StatusString2; }
            set { _StatusString2 = value; this.OnPropertyChanged("StatusString2"); }
        }
        

        private string _StatusString1;
        public string StatusString1
        {
            get { return _StatusString1; }
            set { _StatusString1 = value; this.OnPropertyChanged("StatusString1"); }
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

        private bool _BreakTime;
        public bool BreakTime
        {
            get { return _BreakTime; }
            set { _BreakTime = value; OnPropertyChanged("BreakTime"); }
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
            if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.AttendanceProcess), clsSecurity.loggedUser.user_id))
            {
                dis.Tick += dis_Tick;
                dis.Interval = new TimeSpan(0, 0, 0, 0, 1);
                dis.Start();
            }
            else
            {
                clsMessages.setMessage("No Permission For Attendance Process");
            }
          
        }
        void dis_Tick(object sender, EventArgs e)
        {
           
            Value++;
            if (Value == 2)
            {
                StatusString1 = "Starting Payroll Process........  ";
                dis.Interval = new TimeSpan(0, 0, 0,1,0);
                              
            }
            if (Value == 5)
            {
                StatusString2 = "Checking Previous Attendance Process .....";
                dis.Stop();
                if (serviceClient.CheckAttendanceProcessIsExist(CurrentPeriod.period_id))
                {
                    isPriviousPayroll = true;
                    dis.Start();
                    StatusString1 = "Previous Attendance Process already exist ....";
                    StatusString2 = "";
                }
                else
                {
                    dis.Start();
                    StatusString1 = "No Previous Payroll Process exist ..";
                    StatusString2 = "";
                    isDeletePayrollProcess = true;

                }
            
            }
            if (Value == 8)
            {
                if (isPriviousPayroll == true)
                {
                    dis.Stop();
                    StatusString2 = "Deleting Previous Payroll Process ......";
                    if (serviceClient.deleteEmployeeAttendanceFromTable(CurrentPeriod.period_id))
                    {
                        dis.Start();
                        StatusString2 = "Previous Employee Attendance Process Deleted......";
                        isDeletePayrollProcess = true;

                    }
                    else
                    {
                        StatusString2 = "Previous Employee Attendance Process Delete Fail......";
                        dis.Stop();
                    }
                }

            }
            //if (Value == 11)
            //{
            //    if (isPriviousPayroll == true)
            //    {
            //        if (isDeletePayrollAttendance == true)
            //        {
            //            dis.Stop();
            //            if (serviceClient.deleteEmployeeSumarry(CurrentPeriod.period_id))
            //            {
            //                dis.Start();
            //                StatusString2 = "Previous Employee Summary Process Deleted......";
            //                isDeletePayrollSumarry = true;
            //            }
            //            else
            //            {
            //                StatusString2 = "Previous Employee Attendance Summary Process Delete Fail......";
            //                dis.Stop();
            //            }
            
            //        }
            //    }
            //}
            if (Value == 35)
            {
                dis.Stop();
                if (isDeletePayrollProcess == true )
                {
                    StatusString1 = "Starting Attendance Process......";
                    StatusString2 = ".........";
                    if (serviceClient.AttendanceProcess(SelectedEmployee.ToArray(), CurrentPeriod, BreakTime))
                    {
                        StatusString1 = "Normal Attendance Process Completed ........";
                        //if (serviceClient.RosterProcess(SelectedEmployee.ToArray(), CurrentPeriod))
                        //{
                        //    StatusString1 = "Roster Attendance Process Completed ........";

                            if (serviceClient.GenarateAttendanceSumarryCall(CurrentPeriod.period_id))
                            {
                                StatusString2 = "Monthly Attendance Summary Completed ...";
                                dis.Interval = new TimeSpan(0, 0, 0, 0, 30);
                                isPayrollCompleate = true;
                                dis.Start();
                            }
                        //}
                        //else
                        //{
                        //    StatusString1 = "Roster Payroll Process Fail ........";
                        //}

                    }
                    else
                    {
                        StatusString1 = "Unable to Complete Payroll Process.....";
                        StatusString2 = "Please Try again....";
                    }
                }
                if(Value==70)
                {
                    StatusString1="Finalizing Payroll Process...";
                }
                if(Value==80)
                {
                    StatusString1="Finalize Completed ...";
                }
                if(Value==90)
                {
                    if(isPayrollCompleate==true)
                    {
                        MessageBox.Show("Payroll Process Completed");
                    }
                    else
                    {
                         MessageBox.Show("Payroll Process Fail");
                    }
                }

            }

        }
        
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
      
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsInProgress = false;

        }

       
        private void Reset()
        {
            Value = Min;
        }
       


        #region Ienurable List Properties

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

        #region Employee
        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
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

        #region  Employees
        private IEnumerable<EmployeeSumarryView> _SelectedEmployees;
        public IEnumerable<EmployeeSumarryView> SelectedEmployees
        {
            get { return _SelectedEmployees; }
            set { _SelectedEmployees = value; this.OnPropertyChanged("SelectedEmployees"); }
        } 
        #endregion

        #region AttendanceData
        private IEnumerable<dtl_AttendanceData> _AttendanceData;
        public IEnumerable<dtl_AttendanceData> AttendanceData
        {
            get { return _AttendanceData; }
            set { _AttendanceData = value; this.OnPropertyChanged("AttendanceData"); }
        } 
        #endregion

        #endregion

        #region Curret List Properties

        #region current company branch
        private z_CompanyBranches _CurretCompanyBranch;
        public z_CompanyBranches CurretCompanyBranch
        {
            get { return _CurretCompanyBranch; }
            set { _CurretCompanyBranch = value; this.OnPropertyChanged("CurretCompanyBranch");
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

        #region current employee
        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee"); }
        } 
        #endregion

        #region current period
        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; this.OnPropertyChanged("CurrentPeriod");
           
            }
        } 
        #endregion

        #region current selected employee
        private EmployeeSumarryView _CurrentSelectedEmployee;
        public EmployeeSumarryView CurrentSelectedEmployee
        {
            get { return _CurrentSelectedEmployee; }
            set { _CurrentSelectedEmployee = value; this.OnPropertyChanged("CurrentSelectedEmployee"); }
        } 
        #endregion

      
        
        
        #endregion

        #region Refresh Methords

        private void RefreshCompanyBranch()
        {
            this.serviceClient.GetAllCompanyBranchCompleted += (s, e) =>
            {
                this.CompanyBranches = e.Result;
            };
            this.serviceClient.GetAllCompanyBranchAsync();
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

        private void refreshEmployee()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                AllEmployee.Clear();
                this.Employees = e.Result.Where(z=>z.isdelete==false && z.isActive==true);
                foreach (var item in Employees)
                {
                    AllEmployee.Add(item); 
                }

            };
            this.serviceClient.GetAllEmployeeDetailAsync();
        }

        private void refreshPeriod()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                this.Periods = e.Result.Where(z=>z.isdelete==false);
            };
            this.serviceClient.GetPeriodsAsync();
        }

        private void refreshAttendanceData()
        {
            this.serviceClient.GetAttendanceFromDateRangeCompleted += (s, e) =>
            {
                this.AttendanceData = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetAttendanceFromDateRangeAsync(CurrentPeriod.start_date.Value.Date,CurrentPeriod.end_date.Value.Date);
        }

       
        #endregion

        #region Filter Employee
        public void EmployeeFiltering()
        {
            if (Employees != null)
            {
                if (CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty && CurrentDepartment == null)
                {
                    Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id);
                    Departments = Departments.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id);
                }
                if (CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty && CurretSection == null)
                {
                    if (CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty && CurretSection == null)
                    {
                        Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id);
                        //Sections = Sections.Where(z => z.department_id == CurrentDepartment.department_id);
                    }
                    else
                    {
                        Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id);
                    }
                }
                if (CurretSection != null && CurretSection.section_id != Guid.Empty)
                {

                    if (CurretCompanyBranch != null && CurrentDepartment != null && CurretSection != null)
                    {
                        Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id);
                    }
                    else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty)
                    {
                        Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.department_id == CurrentDepartment.department_id);
                    }
                    else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurretCompanyBranch != null && CurretCompanyBranch.companyBranch_id != Guid.Empty)
                    {
                        Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.branch_id == CurretCompanyBranch.companyBranch_id);
                    }
                    else
                    {
                        Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id);
                    }


                }
                if (CurretDesignation != null && CurretDesignation.designation_id != Guid.Empty)
                {
                    if (CurretDesignation != null && CurretCompanyBranch != null && CurrentDepartment != null && CurretSection != null && CurretDesignation != null)
                    {
                        Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id);
                    }
                    else if (CurretSection != null && CurretSection.section_id != Guid.Empty && CurrentDepartment != null && CurrentDepartment.department_id != Guid.Empty && CurretDesignation != null && CurretDesignation.designation_id == Guid.Empty)
                    {
                        Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id && z.section_id == CurretSection.section_id);
                    }
                    else if (CurretSection != null && CurretDesignation != null)
                    {
                        Employees = AllEmployee.Where(z => z.section_id == CurretSection.section_id && z.designation_id == CurretDesignation.designation_id);
                    }
                    else if (CurretCompanyBranch != null && CurretDesignation != null)
                    {
                        Employees = AllEmployee.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id && z.designation_id == CurretDesignation.designation_id);
                    }
                    else if (CurrentDepartment != null && CurretDesignation != null)
                    {
                        Employees = AllEmployee.Where(z => z.department_id == CurrentDepartment.department_id && z.designation_id == CurretDesignation.designation_id);
                    }
                    else
                    {
                        Employees = AllEmployee.Where(z => z.designation_id == CurretDesignation.designation_id);
                    }
                }
                if (CurretGrade != null && CurretGrade.grade_id != Guid.Empty)
                {
                    Employees = AllEmployee.Where(z => z.grade_id == CurretGrade.grade_id);
                }

            }
        } 
        #endregion

        #region List Selection Methords

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
            foreach (EmployeeSumarryView emp in SelectedEmployee)
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
            foreach (EmployeeSumarryView item in Employees.ToList())
            {
                SelectedEmployee.Add(item);
                AllEmployee.Remove(item);
            }
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

        #endregion

        #region Next Button Classes
        public ICommand Next
        {
            get { return new RelayCommand(next, nextCanExecute); }
        }

        private bool nextCanExecute()
        {
            if (this.CurrentPeriod == null)
                return false;
            if (this.SelectedEmployees == null)
                return false;
            else
                return true;
        }

        private void next()
        {
          //  ProcessStartUserControl usercontrol = new ProcessStartUserControl(this);

        } 
        #endregion

        #region Shift Catagory
        private IEnumerable<z_ShiftCategory> _ShiftCatagory;
        public IEnumerable<z_ShiftCategory> ShiftCatagory
        {
            get { return _ShiftCatagory; }
            set { _ShiftCatagory = value; this.OnPropertyChanged("ShiftCatagory"); }
        }

        private z_ShiftCategory _CurrentShiftCatagory;
        public z_ShiftCategory CurrentShiftCatagory
        {
            get { return _CurrentShiftCatagory; }
            set { _CurrentShiftCatagory = value; this.OnPropertyChanged("CurrentShiftCatagory"); }
        } 
        #endregion

        #region Shift Detail
        private IEnumerable<dtl_Shift> _Shift;
        public IEnumerable<dtl_Shift> Shift
        {
            get { return _Shift; }
            set { _Shift = value; this.OnPropertyChanged("Shift"); }
        }

        private dtl_Shift _CurrentShift;
        public dtl_Shift CurrentShift
        {
            get { return _CurrentShift; }
            set { _CurrentShift = value; this.OnPropertyChanged("CurrentShift"); }
        } 

        #endregion

        private void refreshShiftCatagory()
        {
            this.serviceClient.GetShiftcategoryCompleted += (s, e) =>
            {
                this.ShiftCatagory = e.Result.Where(z => z.is_active == true && z.isdelete == false);
            };
            this.serviceClient.GetShiftcategoryAsync();
        }

        private void refreshShiftDetaila()
        {
            this.serviceClient.GetShiftDetailsCompleted += (s, e) =>
            {
                AllShift.Clear();
                this.Shift = e.Result;
                foreach (var shift in e.Result)
                {
                    AllShift.Add(shift);
                }
            };
            this.serviceClient.GetShiftDetailsAsync();
        } 
       

        //public ICommand btnProcessStart   
        //{
        //    get { return new RelayCommand(ProcessStart, ProcessStartCanExecute); }
        //}

        private bool ProcessStartCanExecute()
        {
            if (this.CurrentPeriod == null)
                return false;
            if (this.SelectedEmployees == null)
                return false;
          
            return true;
        }


        private void DeleteCurrentEmployeeAttendanceProcess()
        {
           
            if (serviceClient.CheckAttendanceProcessIsExist(CurrentPeriod.period_id))
            {
                 DialogResult result = MessageBox.Show("Payroll Process already Exist, do you need to remove it and Process again ?", "ERP Ask", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                 if (DialogResult.Yes == result)
                 {
                     if (serviceClient.deleteEmployeeAttendanceFromTable(CurrentPeriod.period_id))
                     {
                         StatusString2 = "Current Payroll Process Delete Successfully ..... ";
                         dis.Start();
                     }
                     else
                     {
                         StatusString2 = "Current Employee Attendance Delete Fail...... ";
                         StatusString1 = "Stopping Attendance Process..... ";
                         dis.Stop();
                     }
                 }
                 else
                 {
                     dis.Stop();
                     StatusString1 = "Stopping Attendance Process..... ";
                     //((WrapPanel)this.Parent).Children.Remove(this);
                 }
            }
           
        
                         
           
        }
        //public void DeletingAttendanceSumarry()
        //{
        //    if (serviceClient.deleteEmployeeSumarry(CurrentPeriod.period_id))
        //    {
        //        StatusString2 = "Current Employee Summary Process Delete Successfully ..... ";
        //        dis.Start();
        //    }
        //    else
        //    {
        //        StatusString1 = "Current Employee Summary Process Delete Fail ..... ";
        //        StatusString2 = "Stopping Attendance Process..... ";
        //    }
        //}
        //public void Procede()
        //{
        //    //CurrentProgress = true; 
        //    if (SelectedEmployee.Count > 0)
        //    {
        //        if (serviceClient.AttendanceProcess(SelectedEmployee.ToArray(), CurrentPeriod))
        //        {
        //            //CurrentProgress = false; 
        //            MessageBox.Show("Attendance Process Compleated");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Attendance Process Fail");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please Select the Employees");

        //    }
        //}

        #region MyRegion
        private void RosterProcess()
        {
            if (serviceClient.RosterProcess(SelectedEmployee.ToArray(), CurrentPeriod))
                MessageBox.Show("Process Sucess");
            else
            MessageBox.Show("Process Fail");
        }

        public ICommand RosterProcessBtn
        {
            get { return new RelayCommand(RosterProcess); }
        }
        #endregion
     
    }
    
}
