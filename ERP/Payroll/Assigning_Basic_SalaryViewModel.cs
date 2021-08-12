using ERP.ERPService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Payroll
{
    public class Assigning_Basic_SalaryViewModel : ViewModelBase
    {

        #region Service
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion


        #region List
        private List<z_CompanyBranches> tempBranch = new List<z_CompanyBranches>();
        private List<z_Designation> tempDesignation = new List<z_Designation>();
        private List<z_Section> tempSection = new List<z_Section>();
        private List<z_Grade> tempGrade = new List<z_Grade>();
        private List<z_Department> tempDepartment = new List<z_Department>();
        private List<z_EmployeeBasicRuleName> tempBasicRuleName = new List<z_EmployeeBasicRuleName>();
        private List<dtl_EmployeeBasicSalary> tempBasicSalary = new List<dtl_EmployeeBasicSalary>();

        List<ViewBasicSalary> AllBasicSalary = new List<ViewBasicSalary>();
        List<ViewBasicSalary> TempBasicSalary = new List<ViewBasicSalary>();
        List<EmployeeSumarryView> SelectedEmp = new List<EmployeeSumarryView>();
        List<EmployeeSumarryView> AllEmp = new List<EmployeeSumarryView>();
        List<dtl_EmployeeBasicSalary> compleateListForSave = new List<dtl_EmployeeBasicSalary>();
        #endregion

        public Assigning_Basic_SalaryViewModel()
        {
            this.refreshDepartment();
            this.refreshSection();
            this.refreshGrade();
            this.refreshDesignation();
            this.refreshEmployee();
            this.refreshCompanyBranch();
            this.refreshEmployeeBasicSalary();
            this.refreshAllBasicSalaryDetailsView();
            // this.CompanyRuleView = null;
        }


        private IEnumerable<z_Department> _Departments;
        public IEnumerable<z_Department> Departments
        {
            get { return _Departments; }
            set { _Departments = value; this.OnPropertyChanged("Departments"); }
        }

        private IEnumerable<z_EmployeeBasicRuleName> _EmpBasicRuleName;

        public IEnumerable<z_EmployeeBasicRuleName> EmpBasicRuleName
        {
            get { return _EmpBasicRuleName; }
            set { _EmpBasicRuleName = value; this.OnPropertyChanged("EmpBasicRuleName"); }
        }


        private z_EmployeeBasicRuleName _CurrentEmpBasicRuleName;

        public z_EmployeeBasicRuleName CurrentEmpBasicRuleName
        {
            get { return _CurrentEmpBasicRuleName; }
            set
            {
                _CurrentEmpBasicRuleName = value; this.OnPropertyChanged("CurrentEmpBasicRuleName");

            }
        }


        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set
            {
                _CurrentDepartment = value; this.OnPropertyChanged("CurrentDepartment");
                this.FilteringEmployees();
            }
        }

        private IEnumerable<z_Designation> _Designations;
        public IEnumerable<z_Designation> Designation
        {
            get { return _Designations; }
            set { _Designations = value; this.OnPropertyChanged("Designation"); }
        }

        private z_Designation _CurrentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return _CurrentDesignation; }
            set
            {
                _CurrentDesignation = value; this.OnPropertyChanged("CurrentDesignation");
                this.FilteringEmployees();

            }
        }

        private IEnumerable<z_Section> _Sections;
        public IEnumerable<z_Section> Sections
        {
            get { return _Sections; }
            set { _Sections = value; this.OnPropertyChanged("Sections"); }
        }

        private z_Section _CurrentSection;
        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set
            {
                _CurrentSection = value; this.OnPropertyChanged("CurrentSection");
                this.FilteringEmployees();
            }
        }

        private IEnumerable<z_Grade> _Grades;
        public IEnumerable<z_Grade> Grades
        {
            get { return _Grades; }
            set { _Grades = value; this.OnPropertyChanged("Grades"); }
        }

        private z_Grade _CurrentGrade;
        public z_Grade CurrentGrade
        {
            get { return _CurrentGrade; }
            set
            {
                _CurrentGrade = value; this.OnPropertyChanged("CurrentGrade");
                this.FilteringEmployees();

            }
        }

        private IEnumerable<dtl_EmployeeBasicSalary> _EmployeeBasicSalary;

        public IEnumerable<dtl_EmployeeBasicSalary> EmployeeBasicSalary
        {
            get { return _EmployeeBasicSalary; }
            set { _EmployeeBasicSalary = value; this.OnPropertyChanged("EmployeeBasicSalary"); }
        }


        private dtl_EmployeeBasicSalary _CurrentEmployeeBasicSalary;

        public dtl_EmployeeBasicSalary CurrentEmployeeBasicSalary
        {
            get { return _CurrentEmployeeBasicSalary; }
            set
            {
                _CurrentEmployeeBasicSalary = value; this.OnPropertyChanged("CurrentEmployeeBasicSalary");

            }
        }


        private dtl_EmployeeBasicSalary _EmptyEmployeeBasicSalary;

        public dtl_EmployeeBasicSalary EmptyEmployeeBasicSalary
        {
            get { return _EmptyEmployeeBasicSalary; }
            set
            {
                _EmptyEmployeeBasicSalary = value; this.OnPropertyChanged("EmptyEmployeeBasicSalary");

            }
        }

        private IList _SelectedEmployeesWantedToAdd = new ArrayList();
        public IList SelectedEmployeesWantedToAdd
        {
            get { return _SelectedEmployeesWantedToAdd; }
            set { _SelectedEmployeesWantedToAdd = value; OnPropertyChanged("SelectedEmployeesWantedToAdd"); }
        }

        private IList _SelectedEmployeesWantedToDelete = new ArrayList();
        public IList SelectedEmployeesWantedToDelete
        {
            get { return _SelectedEmployeesWantedToDelete; }
            set { _SelectedEmployeesWantedToDelete = value; OnPropertyChanged("SelectedEmployeesWantedToDelete"); }
        }

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee");
                if (CurrentEmployee != null)
                {
                    CurrentWantAddEmployee = Employees.First(z => z.employee_id == CurrentEmployee.employee_id);
                }

            }
        }

        private IEnumerable<z_CompanyBranches> _CompanyBranch;
        public IEnumerable<z_CompanyBranches> CompanyBranch
        {
            get { return _CompanyBranch; }
            set
            {
                _CompanyBranch = value; this.OnPropertyChanged("CompanyBranch");
            }
        }

        private z_CompanyBranches _CurretCompanyBranch;
        public z_CompanyBranches CurretCompanyBranch
        {
            get { return _CurretCompanyBranch; }
            set
            {
                _CurretCompanyBranch = value; this.OnPropertyChanged("CurretCompanyBranch");
                this.FilteringEmployees();

            }
        }

        private IEnumerable<EmployeeSumarryView> _SelectedEmployeeList;
        public IEnumerable<EmployeeSumarryView> SelectedEmployeeList
        {
            get { return _SelectedEmployeeList; }
            set { _SelectedEmployeeList = value; this.OnPropertyChanged("SelectedEmployeeList"); }
        }

        private IEnumerable<EmployeeSumarryView> _SelectedEmployees;
        public IEnumerable<EmployeeSumarryView> SelectedEmployees
        {
            get { return _SelectedEmployees; }
            set { _SelectedEmployees = value; this.OnPropertyChanged("SelectedEmployees"); }
        }

        private EmployeeSumarryView _CurrentWantRemoveEmployee;
        public EmployeeSumarryView CurrentWantRemoveEmployee
        {
            get { return _CurrentWantRemoveEmployee; }
            set
            {
                _CurrentWantRemoveEmployee = value; this.OnPropertyChanged("CurrentWantRemoveEmployee");
                if (CurrentWantRemoveEmployee != null)
                {
                    EmptyCurreViewBasicSalary = AllBasicSalary.Where(z => z.employee_id == CurrentWantRemoveEmployee.employee_id);
                }
            }
        }

        private EmployeeSumarryView _CurrentWantAddEmployee;
        public EmployeeSumarryView CurrentWantAddEmployee
        {
            get { return this._CurrentWantAddEmployee; }
            set
            {
                this._CurrentWantAddEmployee = value; OnPropertyChanged("CurrentWantAddEmployee");


            }
        }

        private string _SpecialAmount;
        public string SpecialAmount
        {
            get { return _SpecialAmount; }
            set { _SpecialAmount = value; this.OnPropertyChanged("SpecialAmount"); }
        }


        private ViewBasicSalary _CurreViewBasicSalary;

        public ViewBasicSalary CurreViewBasicSalary
        {

            get { return _CurreViewBasicSalary; }
            set
            {
                _CurreViewBasicSalary = value; this.OnPropertyChanged("CurreViewBasicSalary");
                if (CurreViewBasicSalary != null)
                {
                    try
                    {
                        CurrentEmpBasicRuleName = EmpBasicRuleName.FirstOrDefault(z => z.EmployeeBasicSalaryRuleId == CurreViewBasicSalary.assigningRuleId);
                        SpecialAmount = CurreViewBasicSalary.amount.ToString();
                    }
                    catch { }

                }
            }

        }

        //Empty

        private IEnumerable<ViewBasicSalary> _EmptyCurreViewBasicSalary;

        public IEnumerable<ViewBasicSalary> EmptyCurreViewBasicSalary
        {

            get { return _EmptyCurreViewBasicSalary; }
            set
            {
                _EmptyCurreViewBasicSalary = value; this.OnPropertyChanged("EmptyCurreViewBasicSalary");
            }
        }


        //private bool _IsSpecial;
        //public bool IsSpecial
        //{
        //    get { return _IsSpecial; }
        //    set { _IsSpecial = value; this.OnPropertyChanged("IsSpecial"); }
        //}

        private bool _Isactive;
        public bool Isactive
        {
            get { return _Isactive; }
            set { _Isactive = value; this.OnPropertyChanged("Isactive"); }
        }

        #region refresh

        private void refreshDepartment()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Departments = e.Result.Where(z => z.isdelete == false);
                foreach (var department in Departments)
                {
                    tempDepartment.Add(department);
                }
            };
            this.serviceClient.GetDepartmentsAsync();
        }

        private void refreshEmployeeBasicSalary()
        {
            this.serviceClient.GetEmployeeBasicRuleNameCompleted += (s, e) =>
            {
                this.EmpBasicRuleName = e.Result;
                if (EmpBasicRuleName != null && EmpBasicRuleName.Count() > 0)
                {
                    //EmpBasicRuleName = EmpBasicRuleName.Where(c => c.EmployeeBasicSalaryRuleId == Guid.Empty);

                    foreach (var item in EmpBasicRuleName)
                    {
                        tempBasicRuleName.Add(item);
                    }
                }
            };
            this.serviceClient.GetEmployeeBasicRuleNameAsync();

        }

        private void refreshAllBasicSalaryDetailsView()
        {
            this.serviceClient.GetAllBasicSalaryCompleted += (s, e) =>
            {
                AllBasicSalary.Clear();
                EmptyCurreViewBasicSalary = null;
                this.EmptyCurreViewBasicSalary = e.Result;
                foreach (var item in EmptyCurreViewBasicSalary)
                {
                    AllBasicSalary.Add(item);
                }
            };
            this.serviceClient.GetAllBasicSalaryAsync();
        }

        private void refreshSection()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
            {
                this.Sections = e.Result.Where(z => z.isdelete == false);
                foreach (var section in Sections)
                {
                    tempSection.Add(section);
                }
            };
            this.serviceClient.GetSectionsAsync();
        }

        private void refreshDesignation()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                this.Designation = e.Result.Where(z => z.isdelete == false);
                foreach (var designation in Designation)
                {
                    tempDesignation.Add(designation);
                }
            };
            this.serviceClient.GetDesignationsAsync();
        }

        private void refreshGrade()
        {
            this.serviceClient.GetGradeCompleted += (s, e) =>
            {
                this.Grades = e.Result.Where(z => z.isdelete == false);
                foreach (var grade in Grades)
                {
                    tempGrade.Add(grade);
                }
            };
            this.serviceClient.GetGradeAsync();
        }

        private void refreshEmployee()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                this.Employees = e.Result.Where(z => z.isdelete == false).OrderBy(c => c.emp_id);
                foreach (var employee in Employees)
                {
                    AllEmp.Add(employee);
                }
            };
            this.serviceClient.GetAllEmployeeDetailAsync();
        }


        private void refreshCompanyBranch()
        {
            this.serviceClient.GetCompanyBranchesCompleted += (s, e) =>
            {
                this.CompanyBranch = e.Result.Where(z => z.isdelete == false);
                foreach (var branch in CompanyBranch)
                {
                    tempBranch.Add(branch);
                }
            };
            this.serviceClient.GetCompanyBranchesAsync();
        }

        private void refreshGetEmployeeBasicSalaryDetail()
        {
            this.serviceClient.GetEmployeeBasicSalaryCompleted += (s, e) =>
            {
                this.EmployeeBasicSalary = e.Result.Where(z => z.isactive == true);
                foreach (var item in EmployeeBasicSalary)
                {
                    tempBasicSalary.Add(item);
                }

            };
            this.serviceClient.GetEmployeeBasicSalaryAsync();
        }

        #endregion

        public void FilteringEmployees()
        {

            if (CurretCompanyBranch != null && CurrentDepartment == null && Employees != null && CurretCompanyBranch.companyBranch_id != Guid.Empty)
            {
                Employees = null;
                Employees = AllEmp;
                Employees = Employees.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id);
                Departments = Departments.Where(z => z.branch_id == CurretCompanyBranch.companyBranch_id);
            }
            if (CurrentDepartment != null && CurrentSection == null)
            {
                Sections = Sections.Where(z => z.department_id == CurrentDepartment.department_id);
                Employees = Employees.Where(z => z.department_id == CurrentDepartment.department_id);
            }
            if (CurrentSection != null && CurrentDepartment == null)
            {
                Employees = Employees.Where(z => z.section_id == CurrentSection.section_id);
            }
            if (CurrentGrade != null)
            {
                Employees = null;
                Employees = AllEmp;
                Employees = Employees.Where(z => z.grade_id == CurrentGrade.grade_id);
            }
            if (CurrentDesignation != null)
            {
                Employees = null;
                Employees = AllEmp;
                Employees = Employees.Where(z => z.designation_id == CurrentDesignation.designation_id);
            }
            //if (CurrentEmployee != null)
            //{
            //    CurrentEmployee = Employees.First(z => z.employee_id == CurrentEmployee.employee_id);
            //}

        }

        private bool removeAllCanExecute()
        {
            if (this.SelectedEmployees == null)
                return false;
            else
                return true;
        }

        private void removeAll()
        {
            /*Employees = null;
            foreach (var item in SelectedEmp)
            {
                AllEmp.Add(item);
            }
            Employees = AllEmp;
            SelectedEmp.Clear();
            SelectedEmployees = null;
            this.Employees = AllEmp;*/
            Employees = null;
            SelectedEmployees = null;
            foreach (var emp in SelectedEmp)
            {
                AllEmp.Add(emp);
            }
            SelectedEmp.Clear();
            this.Employees = AllEmp;
            this.SelectedEmployees = SelectedEmp;
        }

        private bool addAllCanExecute()
        {
            if (Employees == null)
                return false;
            else
                return true;
        }

        private void addAll()
        {
            SelectedEmployees = null;
            foreach (EmployeeSumarryView item in Employees)
            {
                SelectedEmp.Add(item);
            }
            AllEmp.Clear();
            this.Employees = null;
            Employees = AllEmp;
            this.SelectedEmployees = SelectedEmp;
        }

        private bool removeOneCanExecute()
        {
            if (CurrentWantRemoveEmployee == null)
                return false;
            else
                return true;
        }

        private void removeOne()
        {

            /*AllEmp.Add(CurrentWantRemoveEmployee);
            SelectedEmp.Remove(CurrentWantRemoveEmployee);
            Employees = null;
            SelectedEmployees = null;
            Employees = AllEmp;
            SelectedEmployees = SelectedEmp;*/
            if (SelectedEmployeesWantedToDelete.Count > 0)
            {
                foreach (EmployeeSumarryView item in SelectedEmployeesWantedToDelete)
                {
                    AllEmp.Add(item);
                    SelectedEmp.Remove(item);
                }
            }

            //AllEmp.Add(CurrentWantRemoveEmployee);
            //SelectedEmp.Remove(CurrentWantRemoveEmployee);
            Employees = null;
            SelectedEmployees = null;
            Employees = AllEmp;
            SelectedEmployees = SelectedEmp;
        }

        private bool addOneCanExecute()
        {
            if (CurrentWantAddEmployee == null)
                return false;
            else
                return true;
        }

        private void addOne()
        {
            if (SelectedEmployeesWantedToAdd.Count > 0)
            {
                foreach (EmployeeSumarryView item in SelectedEmployeesWantedToAdd)
                {
                    SelectedEmp.Add(item);
                    AllEmp.Remove(item);
                }
            }
            Employees = null;
            SelectedEmployees = null;
            Employees = AllEmp;
            SelectedEmployees = SelectedEmp;

            //SelectedEmp.Add(CurrentWantAddEmployee);
            //AllEmp.Remove(CurrentWantAddEmployee);
            //Employees = null;
            //SelectedEmployees = null;
            //Employees = AllEmp;
            //SelectedEmployees = SelectedEmp;
        }


        public ICommand AddOne
        {
            get { return new RelayCommand(addOne); }
        }

        public ICommand RemoveOne
        {
            get { return new RelayCommand(removeOne); }
        }

        public ICommand AddAll
        {
            get { return new RelayCommand(addAll, addAllCanExecute); }
        }

        public ICommand RemoveAll
        {
            get { return new RelayCommand(removeAll, removeAllCanExecute); }
        }

        public ICommand Add
        {
            get { return new RelayCommand(addTolist, AddCanExecute); }
        }

        private void addTolist()
        {
            EmptyCurreViewBasicSalary = null;
            ViewBasicSalary test = TempBasicSalary.FirstOrDefault(z => z.assigningRuleId == CurrentEmpBasicRuleName.EmployeeBasicSalaryRuleId);

            if (test == null)
            {
                ViewBasicSalary newBasic = new ViewBasicSalary();
                //newBasic.amount = CurrentEmployeeBasicSalary.amount;
                newBasic.EmployeeBasicSalaryRuleName = CurrentEmpBasicRuleName.EmployeeBasicSalaryRuleName;
                newBasic.assigningRuleId = CurrentEmpBasicRuleName.EmployeeBasicSalaryRuleId;
                newBasic.amount = decimal.Parse(SpecialAmount);
                TempBasicSalary.Add(newBasic);
                EmptyCurreViewBasicSalary = TempBasicSalary;
                SpecialAmount = "";
            }
            else
            {
                SpecialAmount = "";
                clsMessages.setMessage("This Basic Rule Name Already in the List");
                EmptyCurreViewBasicSalary = TempBasicSalary;
            }
        }

        private bool AddCanExecute()
        {
            if (CurrentEmpBasicRuleName == null)
                return false;
            else
                return true;
        }

        public ICommand newButton
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            Employees = null;
            Employees = AllEmp;
            EmpBasicRuleName = null;
            TempBasicSalary.Clear();
            CurrentEmployeeBasicSalary = null;
            compleateListForSave.Clear();
            EmptyCurreViewBasicSalary = null;
            //EmptyCompanyRuleView = TempCompanyRule;
            this.removeAll();
            CurrentDepartment = null;
            CurrentGrade = null;
            CurrentDesignation = null;
            CurrentEmployee = null;
            CurrentSection = null;
            CurretCompanyBranch = null;


            this.refreshEmployeeBasicSalary();
            this.refreshAllBasicSalaryDetailsView();
            SpecialAmount = "";
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        private void addSaveItemToList()
        {
            foreach (var emp in SelectedEmp)
            {
                foreach (var rule in TempBasicSalary)
                {


                    if (EmptyCurreViewBasicSalary != null && SelectedEmp != null)
                    {
                        dtl_EmployeeBasicSalary empRule = new dtl_EmployeeBasicSalary();

                        empRule.employee_id = emp.employee_id;
                        empRule.amount = rule.amount;
                        empRule.assigningRuleId = rule.assigningRuleId;
                        empRule.update_user_id = clsSecurity.loggedUser.user_id;
                        compleateListForSave.Add(empRule);

                    }


                }
            }
        }

        private void Save()
        {
            if (clsSecurity.GetSavePermission(515))
            {
                int i = 0;
                if (EmptyCurreViewBasicSalary != null)
                {
                    foreach (var emp in SelectedEmp)
                    {
                        foreach (var rule in TempBasicSalary)
                        {


                            if (EmptyCurreViewBasicSalary != null && SelectedEmp != null)
                            {
                                dtl_EmployeeBasicSalary empRule = new dtl_EmployeeBasicSalary();

                                empRule.employee_id = emp.employee_id;
                                empRule.amount = rule.amount;
                                empRule.assigningRuleId = rule.assigningRuleId;
                                empRule.update_user_id = clsSecurity.loggedUser.user_id;
                                empRule.affective_date = System.DateTime.Now;
                                empRule.isactive = true;
                                if (serviceClient.SaveEmployeeBasicSalary(empRule))
                                {
                                    i = 2;
                                }
                                else
                                {
                                    i = 0;
                                }
                            }



                        }
                    }

                    if (i == 2)
                    {
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                        New();
                    }
                    else
                    {
                        clsMessages.setMessage("Rule Error.. Try Again");
                    }

                    //if (serviceClient.SaveEmployeeBasicSalary(compleateListForSave.ToArray()))
                    //{

                }

            }
            else
                clsMessages.setMessage("You don't have permission to Save this record(s)");
        }

    }
}
