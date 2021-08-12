/************************************************************************************************************
*   Author     : Akalanka Kasun                                                                                                
*   Date       : 2013-06-19                                                                                                
*   Purpose    : Employee Company Variables View Model                                                                                               
*   Company    : Victory Information                                                                            
*   Module     : ERP System => Detail Masters => Payroll                                                        
*                                                                                                                
************************************************************************************************************/

using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;


namespace ERP.Masters
{
    class EmployeeCompanyVariablesViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        List<EmployeeSumarryView> TempEmployees = new List<EmployeeSumarryView>();
        List<EmployeeSumarryView> vw = new List<EmployeeSumarryView>();
        #endregion

        #region Constructor
        public EmployeeCompanyVariablesViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeCompanyVariable), clsSecurity.loggedUser.user_id))
            {
                refreshDepatments();
                //this.reafreshEmployees();
                //this.viewrefreshEmployees();
                //this.reafreshCompanyVariables();
                //this.reafreshEmployeeCompanyVariables();
                //this.reafreshGetEmployeeCompanyVariables();
                //this.refreshDepatments();
                //this.refreshSections();
                //this.New();
            }
        }
        #endregion

        #region Properties

        private IEnumerable<EmployeeSumarryView> _EmployeesView;
        public IEnumerable<EmployeeSumarryView> EmployeesView
        {
            get { return this._EmployeesView; }
            set { this._EmployeesView = value; OnPropertyChanged("EmployeesView"); }
        }

        private EmployeeSumarryView _CurrentEmplpyeeView;
        public EmployeeSumarryView CurrentEmplopyeeView
        {
            get { return _CurrentEmplpyeeView; }
            set
            {
                _CurrentEmplpyeeView = value; OnPropertyChanged("CurrentEmplopyeeView");
                //if (CurrentEmplopyeeView != null && CurrentEmployee != null && Employees != null && !CurrentEmplopyeeView.employee_id.Equals(new Guid()))
                //{
                //    //this.refreshDepatments();
                //    CurrentEmployee = Employees.First(e => e.employee_id.Equals(CurrentEmplopyeeView.employee_id));
                //IsAplicable = AllCompanyVariables.First(c => c.Is_Active == false);
                //CurrentDepatment = Department.First(c => c.department_id == CurrentEmplopyeeView.department_id);
                // CurrentDepatment = Department.First(c => c.department_id == CurrentEmplopyeeView.department_id);
            }
        }
        private IEnumerable<z_Department> _Departments;
        public IEnumerable<z_Department> Departments    
        {
            get { return _Departments; }
            set { _Departments = value; this.OnPropertyChanged("Department"); }
        }

        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set { _CurrentDepartment = value; }
        }

        private IEnumerable<z_Section> _Sections;
        public IEnumerable<z_Section> Sections
        {
            get { return _Sections; }
            set { _Sections = value; }
        }

        private z_Section _CurrentSection;

        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value; }
        }
        
         private void refreshDepatments()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Departments = e.Result.Where(c => c.isdelete == false);
            };
            this.serviceClient.GetDepartmentsAsync();
        }
        
    }
}

//        #region akalanka code

//        #region old
//        private IEnumerable<mas_Employee> _Employees;
//        public IEnumerable<mas_Employee> Employees
//        {
//            get
//            {
//                return this._Employees;
//            }
//            set
//            {
//                this._Employees = value;
//                this.OnPropertyChanged("Employees");
//            }
//        }

//        private mas_Employee _CurrentEmployee;
//        public mas_Employee CurrentEmployee
//        {
//            get
//            {
//                return this._CurrentEmployee;
//            }
//            set
//            {
//                this._CurrentEmployee = value;
//                this.OnPropertyChanged("CurrentEmployee");
//                reafreshGetEmployeeCompanyVariables();
//            }
//        }

//        private bool _IsAplicable;
//        public bool IsAplicable
//        {
//            get
//            {
//                return this._IsAplicable;
//            }
//            set
//            {
//                this._IsAplicable = value;
//                this.OnPropertyChanged("IsAplicable");
//            }
//        }

//        private IEnumerable<dtl_EmployeeCompanyVariable> _CurrentEmployeeCompanyRuleList;
//        public IEnumerable<dtl_EmployeeCompanyVariable> CurrentEmployeeCompanyRuleList
//        {
//            get
//            {
//                return this._CurrentEmployeeCompanyRuleList;
//            }
//            set
//            {
//                this._CurrentEmployeeCompanyRuleList = value;
//                this.OnPropertyChanged("CurrentEmployeeCompanyRuleList");

//            }
//        }

//        private dtl_EmployeeCompanyVariable _CurrentEmployeeRuleList;
//        public dtl_EmployeeCompanyVariable CurrentEmployeeRuleList
//        {
//            get
//            {
//                return this._CurrentEmployeeRuleList;
//            }
//            set
//            {
//                this._CurrentEmployeeRuleList = value;
//                this.OnPropertyChanged("CurrentEmployeeRuleList");

//            }
//        }

//        private IEnumerable<z_CompanyVariable> _AllCompanyVariables;
//        public IEnumerable<z_CompanyVariable> AllCompanyVariables
//        {
//            get
//            {
//                return this._AllCompanyVariables;
//            }
//            set
//            {
//                this._AllCompanyVariables = value;
//                this.OnPropertyChanged("AllCompanyVariables");
//            }
//        }

//        private z_CompanyVariable _CurrentCompanyVariable;
//        public z_CompanyVariable CurrentCompanyVariable
//        {
//            get { return _CurrentCompanyVariable; }
//            set { _CurrentCompanyVariable = value; OnPropertyChanged("CurrentCompanyVariable"); }
//        }

//        private IEnumerable<dtl_EmployeeCompanyVariable> _EmployeeCompanyVariables;
//        public IEnumerable<dtl_EmployeeCompanyVariable> EmployeeCompanyVariables
//        {
//            get
//            {
//                return this._EmployeeCompanyVariables;
//            }
//            set
//            {
//                this._EmployeeCompanyVariables = value;
//                this.OnPropertyChanged("EmployeeCompanyVariables");
//            }
//        }

//        private dtl_EmployeeCompanyVariable _CurrentEmployeeCompanyVariable;
//        public dtl_EmployeeCompanyVariable CurrentEmployeeCompanyVariable
//        {
//            get
//            {
//                return this._CurrentEmployeeCompanyVariable;
//            }
//            set
//            {
//                this._CurrentEmployeeCompanyVariable = value;
//                this.OnPropertyChanged("CurrentEmployeeCompanyVariable");

//            }
//        }

//        private IEnumerable<z_Department> _Department;
//        public IEnumerable<z_Department> Department
//        {
//            get { return _Department; }
//            set
//            {
//                this._Department = value; OnPropertyChanged("Department");


//            }
//        }

//        private z_Department _CurrentDepatment;
//        public z_Department CurrentDepatment
//        {
//            get { return _CurrentDepatment; }
//            set
//            {
//                _CurrentDepatment = value;
//                OnPropertyChanged("CurrentDepatment");
//                refreshSections();
//                if (CurrentDepatment != null && CurrentDepatment.department_id != Guid.Empty)
//                {
//                    CurrentSection = Sections.First(c => c.department_id == CurrentDepatment.department_id);
//                }
//                filter();
//            }
//        }

//        private IEnumerable<z_Section> _Sections;
//        public IEnumerable<z_Section> Sections
//        {
//            get { return _Sections; }
//            set
//            {
//                _Sections = value; OnPropertyChanged("Sections");


//            }
//        }

//        private z_Section _CurrentSection;
//        public z_Section CurrentSection
//        {
//            get { return _CurrentSection; }
//            set { _CurrentSection = value; OnPropertyChanged("CurrentSection"); filter(); }
//        }


//        #endregion

//        #region New Method
//        void New()
//        {
//            CurrentEmployee = null;
//            CurrentEmployee = new mas_Employee();
//            CurrentCompanyVariable = new z_CompanyVariable();
//            reafreshCompanyVariables();
//            reafreshEmployees();
//            CurrentDepatment = null;
//            CurrentDepatment = new z_Department();
//            CurrentSection = null;
//            CurrentSection = new z_Section();
//            CurrentEmplopyeeView = null;
//            CurrentEmplopyeeView = new EmployeeSumarryView();

//        }

//        #endregion

//        #region New Button Class & Property
//        bool newCanExecute()
//        {
//            return true;
//        }

//        public ICommand NewButton
//        {
//            get
//            {
//                return new RelayCommand(New, newCanExecute);
//            }
//        }
//        #endregion

//        #region Save Method
//        void Save()
//        {
//            bool isCanSave = true;
//            dtl_EmployeeCompanyVariable neWDtlComapanyvari = new dtl_EmployeeCompanyVariable();

//            neWDtlComapanyvari.employee_id = CurrentEmployee.employee_id;
//            neWDtlComapanyvari.company_variableID = CurrentCompanyVariable.company_variableID;
//            neWDtlComapanyvari.isApplicable = IsAplicable;
//            neWDtlComapanyvari.save_user_id = clsSecurity.loggedUser.user_id;
//            neWDtlComapanyvari.save_datetime = System.DateTime.Now;
//            neWDtlComapanyvari.modified_user_id = clsSecurity.loggedUser.user_id;
//            neWDtlComapanyvari.modified_datetime = System.DateTime.Now;
//            neWDtlComapanyvari.delete_datetime = System.DateTime.Now;
//            neWDtlComapanyvari.delete_user_id = clsSecurity.loggedUser.user_id;
//            neWDtlComapanyvari.isdelete = false;
//            foreach (dtl_EmployeeCompanyVariable item in CurrentEmployeeCompanyRuleList)
//            {
//                if (item.company_variableID == CurrentCompanyVariable.company_variableID)
//                {
//                    isCanSave = false;
//                }
//            }

//            if (neWDtlComapanyvari != null && isCanSave)
//            {
//                if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.EmployeeCompanyVariable), clsSecurity.loggedUser.user_id))
//                {
//                    this.serviceClient.SaveEmployeeCompanyVariables(neWDtlComapanyvari);
//                    System.Windows.MessageBox.Show("Record Save successfully");
//                    reafreshGetEmployeeCompanyVariables();
//                }
//            }
//            else
//            {
//                System.Windows.MessageBox.Show("Please Fill the Required Fields...!");
//            }
//        }

//        #endregion

//        #region Save Button Class & Property
//        bool saveCanExecute()
//        {
//            if (CurrentEmployee != null)
//            {
//                if (CurrentEmployee.employee_id == null || CurrentEmployee.employee_id == Guid.Empty)
//                    return false;
//            }
//            else if (CurrentCompanyVariable != null)
//            {
//                if (CurrentCompanyVariable.company_variableID == null || CurrentCompanyVariable.company_variableID == Guid.Empty)
//                    return false;
//            }
//            else if (CurrentEmployeeRuleList != null)
//            {
//                if (CurrentEmployeeRuleList.isApplicable == null)
//                    return false;
//            }
//            else
//            {
//                return false;
//            }
//            return true;
//        }

//        public ICommand SaveButton
//        {
//            get
//            {
//                return new RelayCommand(Save, saveCanExecute);
//            }
//        }
//        #endregion

//        #region Delete Method
//        void Delete()
//        {
//            if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.EmployeeCompanyVariable), clsSecurity.loggedUser.user_id))
//            {
//                MessageBoxResult Result = new MessageBoxResult();
//                Result = System.Windows.MessageBox.Show("Do you want to Delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
//                if (Result == MessageBoxResult.Yes)
//                {

//                    if (serviceClient.DeleteEmployeeCompanyVariables(CurrentEmployeeCompanyVariable))
//                    {
//                        System.Windows.MessageBox.Show("Record Deleted");
//                        reafreshCompanyVariables();
//                        reafreshEmployees();
//                        reafreshGetEmployeeCompanyVariables();
//                    }
//                    else
//                    {
//                        System.Windows.MessageBox.Show("You Cannot Delete This Rule,Because This Rule Related to another Operations...");
//                    }
//                }
//            }
//        }
//        #endregion

//        #region Delete Button Class & Property
//        bool deleteCanExecute()
//        {
//            if (CurrentEmployeeCompanyRuleList == null)
//            {
//                return false;
//            }
//            return true;
//        }

//        public ICommand DeleteButton
//        {
//            get
//            {
//                return new RelayCommand(Delete, deleteCanExecute);
//            }
//        }
//        #endregion

//        #region Employee Company Variables List
//        private void reafreshEmployeeCompanyVariables()
//        {
//            this.serviceClient.GetEmployeeCompanyVariablesCompleted += (s, e) =>
//                {
//                    this.EmployeeCompanyVariables = e.Result;
//                };
//            this.serviceClient.GetEmployeeCompanyVariablesAsync();
//        }
//        #endregion

//        #region Company Variable List form Employee
//        private void reafreshGetEmployeeCompanyVariables()
//        {

//            this.serviceClient.GetEmployeeCompanyVariablesForGridCompleted += (s, e) =>
//                   {
//                       this.CurrentEmployeeCompanyRuleList = e.Result;
//                   };
//            if (CurrentEmployee != null)
//            {
//                this.serviceClient.GetEmployeeCompanyVariablesForGridAsync(CurrentEmployee.employee_id);
//            }
//        }
//        #endregion

//        #region Employee List
//        private void reafreshEmployees()
//        {
//            this.serviceClient.GetEmployeesCompleted += (s, e) =>
//                {
//                    this.Employees = e.Result;
//                };
//            this.serviceClient.GetEmployeesAsync();
//        }
//        #endregion

//        private void refreshDepatments()
//        {
//            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
//            {
//                this.Department = e.Result.Where(c => c.isdelete == false);
//            };
//            this.serviceClient.GetDepartmentsAsync();
//        }

//        private void refreshSections()
//        {
//            this.serviceClient.GetSectionsCompleted += (s, e) =>
//            {
//                if (CurrentDepatment != null)
//                    this.Sections = e.Result.Where(z => z.department_id.Equals(CurrentDepatment.department_id) && z.isdelete == false);
//                else
//                    this.Sections = e.Result.Where(c => c.isdelete == false);
//            };
//            this.serviceClient.GetSectionsAsync();
//        }

//        private void viewrefreshEmployees()
//        {
//            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
//            {
//                this.EmployeesView = e.Result.Where(c => c.isdelete == false);
//                foreach (EmployeeSumarryView emp in EmployeesView)
//                {
//                    TempEmployees.Add(emp);
//                }
//            };
//            this.serviceClient.GetAllEmployeeDetailAsync();
//        }

//        #region Get All Company Variable List
//        private void reafreshCompanyVariables()
//        {
//            this.serviceClient.GetCompanyVariablesCompleted += (s, e) =>
//                {
//                    this.AllCompanyVariables = e.Result.Where(c => c.isdelete == false);
//                };
//            this.serviceClient.GetCompanyVariablesAsync();
//        }
//        #endregion

//        #region Is Active Validation Method
//        private bool validation()
//        {
//            string Message = "ERP System says..! please mention \n";
//            bool isValidate = true;

//            if (CurrentEmployeeCompanyVariable.isApplicable == null)
//            {
//                Message += "Method\n";
//                isValidate = false;
//            }
//            if (!isValidate)
//            {
//                System.Windows.MessageBox.Show(Message, "ERP", MessageBoxButton.OK, MessageBoxImage.Warning);
//            }
//            return isValidate;

//        }
//        #endregion

//        private void filter()
//        {
//            List<mas_Employee> allemp = new List<mas_Employee>();
//            EmployeesView = TempEmployees;

//            if (CurrentDepatment != null && !CurrentDepatment.department_id.Equals(new Guid()))
//                EmployeesView = EmployeesView.Where(e => e.department_id.Equals(CurrentDepatment.department_id));

//            if (CurrentSection != null && !CurrentSection.section_id.Equals(new Guid()))
//                EmployeesView = EmployeesView.Where(e => e.section_id.Equals(CurrentSection.section_id));
//        }
//    }
//}

//        #endregion


        #endregion