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
    class EmployeeRuleDetailViewModel : ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        private List<z_CompanyBranches> tempBranch = new List<z_CompanyBranches>();
        private List<z_Designation> tempDesignation = new List<z_Designation>();
        private List<z_Section> tempSection = new List<z_Section>();
        private List<z_Grade> tempGrade = new List<z_Grade>();
        private List<z_Department> tempDepartment = new List<z_Department>();
        List<EmployeeSumarryView> SelectedEmp = new List<EmployeeSumarryView>();
        List<EmployeeSumarryView> AllEmp = new List<EmployeeSumarryView>();
        List<NewDetailEmployeeCompanyRuleView> AllCompanyRule = new List<NewDetailEmployeeCompanyRuleView>();
        List<NewDetailEmployeeCompanyRuleView> TempCompanyRule = new List<NewDetailEmployeeCompanyRuleView>();
        List<dtl_EmployeeRule> compleateListForSave = new List<dtl_EmployeeRule>();


        public EmployeeRuleDetailViewModel()
        {
            this.reafreshDepartment();
            this.reafreshSection();
            this.reafreshGrade();
            this.reafreshDesignation();
            this.reafreshEmployee();
            this.reafreshCompanyRule();
            this.reafreshDetailCompanyRule();
            this.reafreshDetailEmployeeRule();
            this.reafreshCompanyBranch();
            this.reafreshCompanyDetailsView();
            IsSpecial = false;
            // this.CompanyRuleView = null;


        }

        private IEnumerable<z_Department> _Departments;
        public IEnumerable<z_Department> Departments
        {
            get { return _Departments; }
            set { _Departments = value; this.OnPropertyChanged("Departments"); }
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
            set
            {
                _SelectedEmployeesWantedToDelete = value; this.OnPropertyChanged("CurrentWantRemoveEmployee");
                if (SelectedEmployeesWantedToDelete != null && (SelectedEmployeesWantedToDelete.Count == 1))
                {
                    foreach (EmployeeSumarryView x in SelectedEmployeesWantedToDelete)
                        EmptyCompanyRuleView = AllCompanyRule.Where(z => z.employee_id == x.employee_id);
                }
            }
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

        private IEnumerable<mas_CompanyRule> _CompanyRules;
        public IEnumerable<mas_CompanyRule> CompanyRules
        {
            get { return _CompanyRules; }
            set { _CompanyRules = value; this.OnPropertyChanged("CompanyRules"); }
        }

        private mas_CompanyRule _CurrentCompanyRule;
        public mas_CompanyRule CurrentCompanyRule
        {
            get { return _CurrentCompanyRule; }
            set
            {
                _CurrentCompanyRule = value; this.OnPropertyChanged("CurrentCompanyRule");
                IsSpecial = false;
                SpecialAmount = null;
            }

        }

        private IEnumerable<NewEmployeeCompanyRuleDetail> _EmployeeCompanyRuleDetails;
        public IEnumerable<NewEmployeeCompanyRuleDetail> EmployeeCompanyRuleDetails
        {
            get { return _EmployeeCompanyRuleDetails; }
            set { _EmployeeCompanyRuleDetails = value; this.OnPropertyChanged("EmployeeCompanyRuleDetails"); }
        }

        private NewEmployeeCompanyRuleDetail _CurrentCompanyRuleDetails;
        public NewEmployeeCompanyRuleDetail CurrentCompanyRuleDetails
        {
            get { return _CurrentCompanyRuleDetails; }
            set
            {
                _CurrentCompanyRuleDetails = value; this.OnPropertyChanged("CurrentCompanyRuleDetails");
            }
        }

        private IEnumerable<dtl_EmployeeRule> _DetailEmployeeRule;
        public IEnumerable<dtl_EmployeeRule> DetailEmployeeRule
        {
            get { return _DetailEmployeeRule; }
            set { _DetailEmployeeRule = value; this.OnPropertyChanged("DetailEmployeeRule"); }
        }

        private dtl_EmployeeRule _CurrentDetailEmployeeRule;
        public dtl_EmployeeRule CurrentDetailEmployeeRule
        {
            get { return _CurrentDetailEmployeeRule; }
            set
            {
                _CurrentDetailEmployeeRule = value; this.OnPropertyChanged("CurrentDetailEmployeeRule");
                //this.Filtering();
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
                    EmptyCompanyRuleView = AllCompanyRule.Where(z => z.employee_id == CurrentWantRemoveEmployee.employee_id);
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
                //if (CurrentWantAddEmployee != null)
                //{
                //    CurrentEmployee = CurrentWantAddEmployee;
                //}

            }
        }

        private IEnumerable<NewDetailEmployeeCompanyRuleView> _CompanyRuleView;
        public IEnumerable<NewDetailEmployeeCompanyRuleView> CompanyRuleView
        {
            get { return _CompanyRuleView; }
            set { _CompanyRuleView = value; this.OnPropertyChanged("CompanyRuleView"); }
        }

        private NewDetailEmployeeCompanyRuleView _CurretCompanyRuleView;
        public NewDetailEmployeeCompanyRuleView CurretCompanyRuleView
        {
            get { return _CurretCompanyRuleView; }
            set
            {
                _CurretCompanyRuleView = value;
                this.OnPropertyChanged("CurretCompanyRuleView");
                if (CurretCompanyRuleView != null)
                {
                    CurrentCompanyRule = CompanyRules.FirstOrDefault(z => z.rule_id == CurretCompanyRuleView.rule_id);
                    IsSpecial = (bool)CurretCompanyRuleView.is_special;
                    Isactive = (bool)CurretCompanyRuleView.isactive;
                    SpecialAmount = CurretCompanyRuleView.special_amount.ToString();
                }
            }
        }

        private IEnumerable<NewDetailEmployeeCompanyRuleView> _EmptyCompanyRuleView;
        public IEnumerable<NewDetailEmployeeCompanyRuleView> EmptyCompanyRuleView
        {
            get { return _EmptyCompanyRuleView; }
            set { _EmptyCompanyRuleView = value; this.OnPropertyChanged("EmptyCompanyRuleView"); }
        }

        private string _SpecialAmount;
        public string SpecialAmount
        {
            get { return _SpecialAmount; }
            set { _SpecialAmount = value; this.OnPropertyChanged("SpecialAmount"); }
        }


        private bool _IsSpecial;
        public bool IsSpecial
        {
            get { return _IsSpecial; }
            set { _IsSpecial = value; this.OnPropertyChanged("IsSpecial"); }
        }

        private bool _Isactive;
        public bool Isactive
        {
            get { return _Isactive; }
            set { _Isactive = value; this.OnPropertyChanged("Isactive"); }
        }


        private void reafreshDepartment()
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

        private void reafreshSection()
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

        private void reafreshDesignation()
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

        private void reafreshGrade()
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

        private void reafreshEmployee()
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

        private void reafreshCompanyRule()
        {
            this.serviceClient.GetCompanyRulesCompleted += (s, e) =>
            {
                this.CompanyRules = e.Result.Where(z => z.isActive == true && z.isdelete == false);
            };
            this.serviceClient.GetCompanyRulesAsync();
        }

        private void reafreshDetailCompanyRule()
        {
            this.serviceClient.GetEmployeeCompanyRuleDetailsCompleted += (s, e) =>
            {
                this.EmployeeCompanyRuleDetails = e.Result.Where(z => z.isdelete == false && z.isActive == true);
            };
            this.serviceClient.GetEmployeeCompanyRuleDetailsAsync();
        }

        private void reafreshDetailEmployeeRule()
        {
            this.serviceClient.GetEmployeeRuleCompleted += (s, e) =>
            {
                this.DetailEmployeeRule = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetEmployeeRuleAsync();
        }

        private void reafreshCompanyBranch()
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

        private void reafreshCompanyDetailsView()
        {
            this.serviceClient.GetCompanyRuleDetailViewCompleted += (s, e) =>
            {
                AllCompanyRule.Clear();
                CompanyRuleView = null;
                this.CompanyRuleView = e.Result;
                foreach (var item in CompanyRuleView)
                {
                    AllCompanyRule.Add(item);
                }
            };
            this.serviceClient.GetCompanyRuleDetailViewAsync();
        }

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

        private bool removeAllCanExecute()
        {
            if (this.SelectedEmployees == null)
                return false;
            else
                return true;
        }

        private void removeAll()
        {
            Employees = null;
            foreach (var item in SelectedEmp)
            {
                AllEmp.Add(item);
            }
            Employees = AllEmp;
            SelectedEmp.Clear();
            SelectedEmployees = null;
            this.Employees = AllEmp;
            EmptyCompanyRuleView = null;
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
            EmptyCompanyRuleView = null;
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

        private bool AddCanExecute()
        {
            if (CurrentCompanyRule == null)
                return false;
            else
                return true;
        }

        private bool RemoveCanExecute()
        {
            if (CurretCompanyRuleView == null)
                return false;
            else
                return true;

        }

        public ICommand Add
        {
            get { return new RelayCommand(addTolist, AddCanExecute); }
        }

        private void addTolist()
        {
            EmptyCompanyRuleView = null;
            NewDetailEmployeeCompanyRuleView quntity = TempCompanyRule.FirstOrDefault(z => z.rule_id == CurrentCompanyRule.rule_id);

            if (quntity == null)
            {
                if (IsSpecial == true)
                {
                    if (SpecialAmount != null)
                    {
                        NewDetailEmployeeCompanyRuleView crv = new NewDetailEmployeeCompanyRuleView();
                        crv.rule_name = CurrentCompanyRule.rule_name;
                        crv.rule_id = CurrentCompanyRule.rule_id;
                        crv.is_special = IsSpecial;
                        crv.isactive = Isactive;
                        if (IsSpecial == true)
                        {
                            crv.special_amount = decimal.Parse(SpecialAmount);
                        }
                        else
                        {
                            crv.special_amount = 0;
                        }
                        TempCompanyRule.Add(crv);
                        EmptyCompanyRuleView = TempCompanyRule;
                    }
                    else
                    {
                        clsMessages.setMessage("Please Mention the Special Amount");
                        EmptyCompanyRuleView = TempCompanyRule;
                    }
                }
                else
                {
                    NewDetailEmployeeCompanyRuleView crv = new NewDetailEmployeeCompanyRuleView();
                    crv.rule_name = CurrentCompanyRule.rule_name;
                    crv.rule_id = CurrentCompanyRule.rule_id;
                    crv.is_special = IsSpecial;
                    crv.isactive = Isactive;
                    if (IsSpecial == true)
                    {
                        crv.special_amount = decimal.Parse(SpecialAmount);
                    }
                    else
                    {
                        crv.special_amount = 0;
                    }
                    TempCompanyRule.Add(crv);
                    EmptyCompanyRuleView = TempCompanyRule;
                }

            }
            else
            {

                clsMessages.setMessage("This Rule Name Already in the List");
                EmptyCompanyRuleView = TempCompanyRule;
            }
        }

        public ICommand Remove
        {
            get { return new RelayCommand(removeTolist, RemoveCanExecute); }
        }

        private void removeTolist()
        {
            //AllEmp.Add(CurrentWantRemoveEmployee);
            TempCompanyRule.RemoveAll(z => z.rule_id == CurretCompanyRuleView.rule_id);
            EmptyCompanyRuleView = null;
            EmptyCompanyRuleView = TempCompanyRule;

        }

        private bool saveCanExecute()
        {
            if (SelectedEmployees == null)
                return false;
            if (EmptyCompanyRuleView == null)
                return false;
            if (TempCompanyRule.Count == 0)
                return false;
            else
                return true;
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, saveCanExecute); }
        }

        private void Save()
        {
            this.addSaveItemToList();
            if (compleateListForSave.Count != 0)
            {
                if (clsSecurity.GetSavePermission(505))
                {
                    if (serviceClient.SaveAllEmployeeCompanyRule(compleateListForSave.ToArray()))
                    {
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                        New();
                        //this.reafreshCompanyDetailsView();
                        //EmptyCompanyRuleView = null;
                        //TempCompanyRule.Clear();
                        //EmptyCompanyRuleView = TempCompanyRule;
                    }
                    else
                    {
                        clsMessages.setMessage("Rule Already Exist.. Try Again");
                        //EmptyCompanyRuleView = null;
                        //TempCompanyRule.Clear();
                        //EmptyCompanyRuleView = TempCompanyRule;
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Save this record(s)");
            }
            else
            {
                clsMessages.setMessage("Company Rules are Empty or Rule Already Exist.. Try Again ");
            }

        }

        private void addSaveItemToList()
        {
            foreach (var emp in SelectedEmp)
            {
                foreach (var rule in TempCompanyRule)
                {
                    CurretCompanyRuleView = CompanyRuleView.FirstOrDefault(z => z.employee_id == emp.employee_id && z.rule_id == rule.rule_id);

                    if (CurretCompanyRuleView == null)
                    {
                        dtl_EmployeeRule empRule = new dtl_EmployeeRule();
                        empRule.employee_id = emp.employee_id;
                        empRule.rule_id = rule.rule_id;
                        empRule.special_amount = rule.special_amount;
                        empRule.is_special = rule.is_special;
                        empRule.isactive = rule.isactive;
                        empRule.save_user_id = clsSecurity.loggedUser.user_id;
                        empRule.save_datetime = System.DateTime.Now;
                        empRule.modified_user_id = clsSecurity.loggedUser.user_id;
                        empRule.modified_datetime = System.DateTime.Now;
                        empRule.delete_datetime = System.DateTime.Now;
                        empRule.delete_user_id = clsSecurity.loggedUser.user_id;
                        empRule.isdelete = false;
                        // empRule.status = rule.status;
                        compleateListForSave.Add(empRule);
                    }


                }
            }
        }

        public bool newCanExecute()
        {
            return true;
        }

        public ICommand newButton
        {
            get { return new RelayCommand(New, newCanExecute); }
        }

        private void New()
        {
            Employees = null;
            Employees = AllEmp;
            EmptyCompanyRuleView = null;
            TempCompanyRule.Clear();
            CurrentCompanyRule = null;
            compleateListForSave.Clear();
            //EmptyCompanyRuleView = TempCompanyRule;
            this.removeAll();
            CurrentDepartment = null;
            CurrentGrade = null;
            CurrentDesignation = null;
            CurrentEmployee = null;
            CurrentSection = null;
            CurretCompanyBranch = null;
            this.reafreshDepartment();
            this.reafreshSection();
            this.reafreshGrade();
            this.reafreshDesignation();
            this.reafreshEmployee();
            this.reafreshCompanyRule();
            this.reafreshDetailCompanyRule();
            this.reafreshDetailEmployeeRule();
            this.reafreshCompanyBranch();
            this.reafreshCompanyDetailsView();
            IsSpecial = false;
        }

        public bool updateCanExicute()
        {
            if (CurrentWantRemoveEmployee == null)
                return false;
            if (CurretCompanyRuleView == null)
                return false;
            else
                return true;

        }

        public ICommand Updatebutton
        {
            get { return new RelayCommand(Update, updateCanExicute); }
        }

        private void Update()
        {
            if (CurrentWantRemoveEmployee != null && CurretCompanyRuleView != null)
            {
                if (clsSecurity.GetUpdatePermission(505))
                {
                    dtl_EmployeeRule updateRule = new dtl_EmployeeRule();
                    updateRule.employee_id = CurrentWantRemoveEmployee.employee_id;
                    updateRule.rule_id = CurretCompanyRuleView.rule_id;
                    updateRule.isactive = Isactive;
                    updateRule.special_amount = decimal.Parse(SpecialAmount);
                    updateRule.is_special = IsSpecial;
                    updateRule.modified_datetime = System.DateTime.Now;
                    updateRule.modified_user_id = clsSecurity.loggedUser.user_id;
                    if (serviceClient.UpdateCompanyRuleDetails(updateRule))
                    {
                        clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        New();
                        //this.reafreshCompanyDetailsView();
                        //EmptyCompanyRuleView = AllCompanyRule;
                        //RefreshAll();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.UpdateFail);
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Update this record(s)");
            }

        }

        //private void RefreshAll()
        //{
        //    SelectedEmployees = null;
        //    CurrentWantRemoveEmployee = new EmployeeSumarryView(); ;
        //    Employees = null;
        //    CurrentWantAddEmployee = new EmployeeSumarryView(); ;
        //    EmptyCompanyRuleView = null;
        //    CurretCompanyRuleView = new NewDetailEmployeeCompanyRuleView(); ;
        //    this.reafreshDepartment();
        //    this.reafreshSection();
        //    this.reafreshGrade();
        //    this.reafreshDesignation();
        //    this.reafreshEmployee();
        //    this.reafreshCompanyRule();
        //    this.reafreshDetailCompanyRule();
        //    this.reafreshDetailEmployeeRule();
        //    this.reafreshCompanyBranch();
        //    this.reafreshCompanyDetailsView();
        //}

    }
}
