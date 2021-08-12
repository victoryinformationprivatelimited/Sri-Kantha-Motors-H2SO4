/************************************************************************************************************
*   Author     : Akalanka Kasun                                                                                                
*   Date       : 2013-05-20                                                                                                
*   Purpose    : Employee Rule Details View Model                                                                                                
*   Company    : Victory Information                                                                            
*   Module     : ERP System => Masters => Payroll                                                        
*                                                                                                                
************************************************************************************************************/

using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.MastersDetails
{
    class EmployeeRuleViewModel : ViewModelBase
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        List<EmployeeSumarryView> TempEmployees = new List<EmployeeSumarryView>();
        List<EmployeeSumarryView> vw = new List<EmployeeSumarryView>();
        #endregion

        #region Constructor
        public EmployeeRuleViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.EmployeeRule),clsSecurity.loggedUser.user_id))
            {
                reafreshCompanyRules();
                reafreshEmployeeRuleView();
                reafreshSelectedEmployees();
                refreshDepatments();
                refreshSections();
                viewrefreshEmployees();
                this.New(); 
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
                if (CurrentEmplopyeeView != null && CurrentEmployeeList != null && EmployeesList != null && !CurrentEmplopyeeView.employee_id.Equals(new Guid()))
                {
                    CurrentSelectedEmployee = SelectedEmployee.First(e => e.employee_id.Equals(CurrentEmplopyeeView.employee_id));
                }
            }
        }

        private IEnumerable<dtl_Employee> _SelectedEmployee;
        public IEnumerable<dtl_Employee> SelectedEmployee
        {
            get
            {
                return this._SelectedEmployee;
            }
            set
            {
                this._SelectedEmployee = value;
                this.OnPropertyChanged("SelectedEmployee");
               
            }
        }

        private dtl_Employee _CurrentSelectedEmployee;
        public dtl_Employee CurrentSelectedEmployee
        {
            get
            {
                return this._CurrentSelectedEmployee;
            }
            set
            {
                this._CurrentSelectedEmployee = value;
                this.OnPropertyChanged("CurrentSelectedEmployee");
                EmployeesList = EmployeesList.Where(e => e.employee_id.Equals(CurrentSelectedEmployee.employee_id));
            }
        }

        private IEnumerable<mas_CompanyRule> _Rules;
        public IEnumerable<mas_CompanyRule> Rules
        {
            get
            {
                return this._Rules;
            }
            set
            {
                this._Rules = value;
                this.OnPropertyChanged("Rules");
            }
        }

        private mas_CompanyRule _CurrentRule;
        public mas_CompanyRule CurrentRule
        {
            get
            {
                return this._CurrentRule;
            }
            set
            {
                this._CurrentRule = value;
                this.OnPropertyChanged("CurrentRule");
                
            }
        }

        private IEnumerable<EmployeeRuleView> _EmployeesList;
        public IEnumerable<EmployeeRuleView> EmployeesList
        {
            get
            {
                return this._EmployeesList;
            }
            set
            {
                this._EmployeesList = value;
                this.OnPropertyChanged("EmployeesList");
            }
        }

        private EmployeeRuleView _CurrentEmployeeList;
        public EmployeeRuleView CurrentEmployeeList
        {
            get
            {
                return this._CurrentEmployeeList;
            }
            set
            {
                this._CurrentEmployeeList = value;
                this.OnPropertyChanged("CurrentEmployeeList");
             
                OldEmployeeRule = CurrentEmployeeList;
                
            }
        }

        private EmployeeRuleView _OldEmployeeRule;
        public EmployeeRuleView OldEmployeeRule
        {
            get
            {
                return this._OldEmployeeRule;
            }
            set
            {
                this._OldEmployeeRule = value;
                this.OnPropertyChanged("OldEmployeeRule");               
            }
        }

        private IEnumerable<z_Department> _Department;
        public IEnumerable<z_Department> Department
        {
            get { return _Department; }
            set { this._Department = value; OnPropertyChanged("Department"); }
        }

        private z_Department _CurrentDepatment;
        public z_Department CurrentDepatment
        {
            get { return _CurrentDepatment; }
            set
            {
                _CurrentDepatment = value;
                OnPropertyChanged("CurrentDepatment");
                refreshSections();
                filter();
            }
        }

        private IEnumerable<z_Section> _Sections;
        public IEnumerable<z_Section> Sections
        {
            get { return _Sections; }
            set
            {
                _Sections = value; OnPropertyChanged("Sections");
            }
        }

        private z_Section _CurrentSection;
        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value; OnPropertyChanged("CurrentSection"); filter(); }
        }

        private string _Search;
        public string Search
        {
            get
            {
                return this._Search;
            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");
                if (this._Search == "")
                {
                    reafreshEmployeeRuleView();
                }
                else
                {
                    searchTextChanged();
                }
            }
        }

        #endregion

        #region New Method
        void New()
        {
            this.CurrentEmployeeList = null;
            CurrentEmployeeList = new EmployeeRuleView();
            CurrentEmployeeList.is_special = false;
            reafreshCompanyRules();
            reafreshSelectedEmployees();
            CurrentSection = null;
            CurrentSection = new z_Section();
            CurrentDepatment = null;
            CurrentDepatment = new z_Department();
            CurrentEmplopyeeView = null;
            CurrentEmplopyeeView = new EmployeeSumarryView();
        } 
        #endregion

        #region New Button Class & Property

        bool newCanExecute()
        {
            return true;
        }
        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        } 

        #endregion

        #region Save Method
        void Save()
        {
            bool isUpdate = false;
            dtl_EmployeeRule newEmployeRule = new dtl_EmployeeRule();
            newEmployeRule.employee_id = CurrentSelectedEmployee.employee_id;
            newEmployeRule.rule_id = CurrentEmployeeList.rule_id;
            if ((bool)CurrentEmployeeList.is_special)
            {
                newEmployeRule.special_amount = CurrentEmployeeList.special_amount;
            }
            else
            {
                newEmployeRule.special_amount = 0;
                newEmployeRule.is_special = false;
            }

            newEmployeRule.is_special = CurrentEmployeeList.is_special;

            IEnumerable<dtl_EmployeeRule> allemployeerules = this.serviceClient.GetEmployeeRule();
            if (allemployeerules != null)
            {
                foreach (dtl_EmployeeRule Rule in allemployeerules)
                {
                    if (Rule.employee_id == CurrentSelectedEmployee.employee_id && Rule.rule_id == CurrentEmployeeList.rule_id)
                    {
                        isUpdate = true;
                    }
                }
            }
            if (newEmployeRule != null)
            {
                if (isUpdate)
                {
                    if (clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.EmployeeRule),clsSecurity.loggedUser.user_id))
                    {
                        if (this.serviceClient.UpdateEmployeeRules(OldEmployeeRule, newEmployeRule))
                        {
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                        }
                    }
                }
                else
                {
                    if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.EmployeeRule),clsSecurity.loggedUser.user_id))
                    {
                        if (this.serviceClient.SaveEmployeeRules(newEmployeRule))
                        {
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                        }
                    }
                }
                this.reafreshEmployeeRuleView();
            }
            else
            {
                MessageBox.Show("Please Meantion the Employee Name...!");
            }

           // CurrentEmplopyeeView = null;
           // reafreshEmployeeRuleView();
            //EmployeesList = EmployeesList.Where(a => a.employee_id.Equals(CurrentEmplopyeeView.employee_id));
            CurrentEmplopyeeView = EmployeesView.First(e=> e.employee_id.Equals(newEmployeRule.employee_id));
        } 
        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentSelectedEmployee != null)
            {
                if (CurrentSelectedEmployee.employee_id == null || CurrentSelectedEmployee.employee_id == Guid.Empty)
                    return false;

                try
                {
                    if (CurrentEmployeeList.is_special == true && CurrentEmployeeList.special_amount == 0)
                        return false;
                }
                catch (Exception)
                {
                    
                    
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        } 
        #endregion

        //#region Delete Method
        //void Delete()
        //{
        //    MessageBoxResult Result = new MessageBoxResult();
        //    Result = MessageBox.Show("Do you want to delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (Result == MessageBoxResult.Yes)
        //    {
                
        //        MessageBox.Show("Record Deleted");
        //        reafreshEmployeeRuleView();
        //        reafreshCompanyRules();
        //        reafreshSelectedEmployees();
        //    }
        //}
        //#endregion

        //#region Delete Button Class & Property
        //bool deleteCanExecute()
        //{
        //    if (CurrentEmployeeList == null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public ICommand DeleteButton
        //{
        //    get
        //    {
        //        return new RelayCommand(Delete, deleteCanExecute);
        //    }
        //}

        //#endregion

        #region User Define Methods
        private void reafreshEmployeeRuleView()
        {
            this.serviceClient.GetEmployeeRulesViewCompleted += (s, e) =>
                {
                    this.EmployeesList = e.Result;
                };
            this.serviceClient.GetEmployeeRulesViewAsync();
        }

        private void reafreshSelectedEmployees()
        {
            this.serviceClient.GetSelectedEmployeesCompleted += (s, e) =>
                {
                    this.SelectedEmployee = e.Result;
                };
            this.serviceClient.GetSelectedEmployeesAsync();
        }

        private void reafreshCompanyRules()
        {
            this.serviceClient.GetCompanyRulesCompleted += (s, e) =>
                {
                    this.Rules = e.Result;
                };
            this.serviceClient.GetCompanyRulesAsync();
        }

        private void refreshDepatments()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Department = e.Result;
            };
            this.serviceClient.GetDepartmentsAsync();
        }

        private void refreshSections()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
            {
                if (CurrentDepatment != null)
                    this.Sections = e.Result.Where(z => z.department_id.Equals(CurrentDepatment.department_id));
                else
                    this.Sections = e.Result;
            };
            this.serviceClient.GetSectionsAsync();
        }

        private void filter()
        {
            List<mas_Employee> allemp = new List<mas_Employee>();
            EmployeesView = TempEmployees;

            if (CurrentDepatment != null && !CurrentDepatment.department_id.Equals(new Guid()))
                EmployeesView = EmployeesView.Where(e => e.department_id.Equals(CurrentDepatment.department_id));

            if (CurrentSection != null && !CurrentSection.section_id.Equals(new Guid()))
                EmployeesView = EmployeesView.Where(e => e.section_id.Equals(CurrentSection.section_id));
        }

        private void viewrefreshEmployees()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                this.EmployeesView = e.Result;
                foreach (EmployeeSumarryView emp in EmployeesView)
                {
                    TempEmployees.Add(emp);
                }
            };
            this.serviceClient.GetAllEmployeeDetailAsync();
        }

        #endregion
        
        #region Search Method
        private void searchTextChanged()
        {
            EmployeesList = EmployeesList.Where(e => e.first_name.ToUpper().Contains(Search.ToUpper())).ToList();     
        }
        #endregion

        #region Search Class
        public class relayCommand : ICommand
        {
            readonly Action<object> _Execute;
            readonly Predicate<object> _CanExecute;

            public relayCommand(Action<object> execute)
                : this(execute, null)
            {
            }

            public relayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                if (execute == null)
                    throw new ArgumentNullException("execute");

                _Execute = execute;
                _CanExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _CanExecute == null ? true : _CanExecute(parameter);
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                _Execute(parameter);
            }
        }
        #endregion

        #region Search Property
        relayCommand _OperationCommand;
        public ICommand OperationCommand
        {
            get
            {
                if (_OperationCommand == null)
                {
                    _OperationCommand = new relayCommand(param => this.ExecuteCommand(),
                        param => this.CanExecuteCommand);
                }

                return this._OperationCommand;
            }
        }
        bool CanExecuteCommand
        {
            get { return true; }
        }
        #endregion

        #region Search Command Execute
        public void ExecuteCommand()
        {
            Search = "hh";
            Search = "";
        }
        #endregion
    }
}
