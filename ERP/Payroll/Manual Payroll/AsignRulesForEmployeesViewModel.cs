using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Payroll.ManualPayroll
{
    class AsignRulesForEmployeesViewModel : ViewModelBase
    {
        #region Fields

        private ERPServiceClient serviceClient;
        List<EmployeeSearchView> searchedEmployeeList;
        List<EmployeeSearchView> SearchedEmpListForNonFixedRules;
        List<view_EmployeesAssignedRules> UpdatedEmpRuleQtyAndAmount;
        List<view_EmployeeWiseAssignedRulesForNonFixed> UpdatedEmpNonFixedRuleAmountAndIsspecial;
        List<view_EmployeeWiseAssignedRulesForNonFixed> AllEmpRuleList;

        #endregion

        #region Constructor
        public AsignRulesForEmployeesViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
            NewNonFixed();
        }

        #endregion

        #region Properties

        #region Fixed Rules Properties

        private IEnumerable<view_FixedRules> _FixedRules;
        public IEnumerable<view_FixedRules> FixedRules
        {
            get { return _FixedRules; }
            set { _FixedRules = value; OnPropertyChanged("FixedRules"); }
        }

        private view_FixedRules _CurrentFixedRule;
        public view_FixedRules CurrentFixedRule
        {
            get { return _CurrentFixedRule; }
            set
            {
                _CurrentFixedRule = value;
                OnPropertyChanged("CurrentFixedRule");
                if (CurrentFixedRule != null)
                {
                    RefreshEmployeesAssignedRules();
                    NewListAsign();
                }
            }
        }

        private IEnumerable<view_EmployeesAssignedRules> _EmployeesAssignedRules;
        public IEnumerable<view_EmployeesAssignedRules> EmployeesAssignedRules
        {
            get { return _EmployeesAssignedRules; }
            set { _EmployeesAssignedRules = value; OnPropertyChanged("EmployeesAssignedRules"); }
        }

        private IList _SelectedEmployeesAssignedRuleList = new ArrayList();
        public IList SelectedEmployeesAssignedRuleList
        {
            get { return _SelectedEmployeesAssignedRuleList; }
            set { _SelectedEmployeesAssignedRuleList = value; OnPropertyChanged("SelectedEmployeesAssignedRuleList"); if (SelectedEmployeesAssignedRuleList.Count > 0)SelectedNewEmployeesForRuleList.Clear(); }
        }

        private view_EmployeesAssignedRules _CurrentEmployeeAssignedRule;
        public view_EmployeesAssignedRules CurrentEmployeeAssignedRule
        {
            get { return _CurrentEmployeeAssignedRule; }
            set { _CurrentEmployeeAssignedRule = value; OnPropertyChanged("CurrentEmployeeAssignedRule"); }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; OnPropertyChanged("Amount"); }
        }

        private decimal _Quantity;
        public decimal Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; OnPropertyChanged("Quantity"); }
        }

        private IEnumerable<view_EmployeesAssignedRules> _NewEmployeesFixedRules;
        public IEnumerable<view_EmployeesAssignedRules> NewEmployeesFixedRules
        {
            get { return _NewEmployeesFixedRules; }
            set { _NewEmployeesFixedRules = value; OnPropertyChanged("NewEmployeesFixedRules"); }
        }

        private view_EmployeesAssignedRules _CurrentNewEployeeFixedRules;
        public view_EmployeesAssignedRules CurrentNewEployeeFixedRules
        {
            get { return _CurrentNewEployeeFixedRules; }
            set { _CurrentNewEployeeFixedRules = value; OnPropertyChanged("CurrentNewEployeeFixedRules"); }
        }

        private IList _SelectedNewEmployeesForRuleList = new ArrayList();
        public IList SelectedNewEmployeesForRuleList
        {
            get { return _SelectedNewEmployeesForRuleList; }
            set { _SelectedNewEmployeesForRuleList = value; OnPropertyChanged("SelectedNewEmployeesForRuleList"); if (SelectedNewEmployeesForRuleList.Count > 0)SelectedEmployeesAssignedRuleList.Clear(); }
        }

        #endregion

        #region Non Fixed Rules Properties

        private IEnumerable<mas_CompanyRule> _NonFixedRules;
        public IEnumerable<mas_CompanyRule> NonFixedRules
        {
            get { return _NonFixedRules; }
            set { _NonFixedRules = value; OnPropertyChanged("NonFixedRules"); }
        }

        private mas_CompanyRule _CurrentNonFixedRule;
        public mas_CompanyRule CurrentNonFixedRule
        {
            get { return _CurrentNonFixedRule; }
            set { _CurrentNonFixedRule = value; OnPropertyChanged("CurrentNonFixedRule"); if (CurrentNonFixedRule != null)FilterEmployeesByRule(); }
        }

        private IEnumerable<view_EmployeeWiseAssignedRulesForNonFixed> _EmpCompanyRules;
        public IEnumerable<view_EmployeeWiseAssignedRulesForNonFixed> EmpCompanyRules
        {
            get { return _EmpCompanyRules; }
            set { _EmpCompanyRules = value; OnPropertyChanged("EmpCompanyRules"); }
        }

        private IList _SelectedEmpCompanyRuleList = new ArrayList();
        public IList SelectedEmpCompanyRuleList
        {
            get { return _SelectedEmpCompanyRuleList; }
            set { _SelectedEmpCompanyRuleList = value; OnPropertyChanged("SelectedEmpCompanyRuleList"); if (SelectedEmpCompanyRuleList.Count > 0)SelectedNewEmployeeForNonFixedRuleList.Clear(); }
        }

        private view_EmployeeWiseAssignedRulesForNonFixed _CurrentEmpCompanyRule;
        public view_EmployeeWiseAssignedRulesForNonFixed CurrentEmpCompanyRule
        {
            get { return _CurrentEmpCompanyRule; }
            set { _CurrentEmpCompanyRule = value; OnPropertyChanged("CurrentEmpCompanyRule"); }
        }

        private IEnumerable<view_EmployeeWiseAssignedRulesForNonFixed> _NewEmployeesForNonFixedRules;
        public IEnumerable<view_EmployeeWiseAssignedRulesForNonFixed> NewEmployeesForNonFixedRules
        {
            get { return _NewEmployeesForNonFixedRules; }
            set { _NewEmployeesForNonFixedRules = value; OnPropertyChanged("NewEmployeesForNonFixedRules"); }
        }

        private view_EmployeeWiseAssignedRulesForNonFixed _CurrentNewEmployeeForNonFixedRule;
        public view_EmployeeWiseAssignedRulesForNonFixed CurrentNewEmployeeForNonFixedRule
        {
            get { return _CurrentNewEmployeeForNonFixedRule; }
            set { _CurrentNewEmployeeForNonFixedRule = value; OnPropertyChanged("CurrentNewEmployeeForNonFixedRule"); }
        }

        private IList _SelectedNewEmployeeForNonFixedRuleList = new ArrayList();
        public IList SelectedNewEmployeeForNonFixedRuleList
        {
            get { return _SelectedNewEmployeeForNonFixedRuleList; }
            set { _SelectedNewEmployeeForNonFixedRuleList = value; OnPropertyChanged("SelectedNewEmployeeForNonFixedRuleList"); if (SelectedNewEmployeeForNonFixedRuleList.Count > 0)SelectedEmpCompanyRuleList.Clear(); }
        }

        private decimal _NonFixedRulesSpecialAmount;
        public decimal NonFixedRulesSpecialAmount
        {
            get { return _NonFixedRulesSpecialAmount; }
            set { _NonFixedRulesSpecialAmount = value; OnPropertyChanged("NonFixedRulesSpecialAmount"); }
        }

        private bool _IsSpecial;
        public bool IsSpecial
        {
            get { return _IsSpecial; }
            set { _IsSpecial = value; OnPropertyChanged("IsSpecial"); if (IsSpecial == false)NonFixedRulesSpecialAmount = 0; }
        }


        #endregion

        #endregion

        #region RefreshMethods

        #region Fixed Rules Refresh Methods
        public void RefreshFixedRules()
        {
            serviceClient.GetAllFixedRulesCompleted += (s, e) =>
                {
                    FixedRules = e.Result;
                };
            serviceClient.GetAllFixedRulesAsync();
        }
        public void RefreshEmployeesAssignedRules()
        {
            EmployeesAssignedRules = null;
            serviceClient.GetAllFixedRulesAssigedToEmployeesCompleted += (s, e) =>
                {
                    EmployeesAssignedRules = e.Result.OrderBy(c => c.emp_id);
                };
            serviceClient.GetAllFixedRulesAssigedToEmployeesAsync(CurrentFixedRule.rule_id);
        }

        #endregion

        #region Non Fixed Rules Refresh Methods
        private void RefreshNonFixedRules()
        {
            serviceClient.GetAllNonFixedRulesCompleted += (s, e) =>
                {
                    NonFixedRules = e.Result;
                };
            serviceClient.GetAllNonFixedRulesAsync();
        }
        private void refreshEmployeeCompanyDetailsView()
        {
            this.serviceClient.GetCompanyRulesEmployeeWiseForNonFixedCompleted += (s, e) =>
            {
                if (e.Result != null && e.Result.Count() > 0)
                {
                    AllEmpRuleList = e.Result.ToList();
                }

            };
            this.serviceClient.GetCompanyRulesEmployeeWiseForNonFixedAsync();
        }

        #endregion

        #endregion

        #region Button Commands

        #region FixedRules Button Commands
        public ICommand SelectEmployeesButton
        {
            get { return new RelayCommand(SelectEmployee); }
        }
        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }
        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }
        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        #endregion

        #region Non Fixed Rules Button Commands

        public ICommand SelectEmployeeForNonFixedRulesButton
        {
            get { return new RelayCommand(SelectEmployeeForNonFixedRules); }
        }
        public ICommand AddValueNonFixedRulesButton
        {
            get { return new RelayCommand(AddValueNonFixedRules); }
        }
        public ICommand SaveNonFixedRulesButton
        {
            get { return new RelayCommand(SaveNewNonFixedRules); }
        }
        public ICommand DeleteNonFixedRulesButton
        {
            get { return new RelayCommand(DeleteNonFixedRules); }
        }
        public ICommand NewNonFixButton
        {
            get { return new RelayCommand(NewNonFixed); }
        }

        #endregion

        #endregion

        #region Methods

        #region Fixed Rules Methods
        private void SelectEmployee()
        {
            if (CurrentFixedRule == null)
            {
                clsMessages.setMessage("Please Select A Fixed Rule Before Selecting Employees...");
            }
            else if (CurrentFixedRule.rule_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Fixed Rule Before Selecting Employees...");
            }
            else
            {
                EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
                window.ShowDialog();
                List<view_EmployeesAssignedRules> NotAssignedEmp = new List<view_EmployeesAssignedRules>();
                if (NewEmployeesFixedRules != null)
                {
                    NotAssignedEmp = NewEmployeesFixedRules.ToList();
                }
                if (window.viewModel.SelectedList != null)
                {
                    searchedEmployeeList.Clear();
                    searchedEmployeeList = window.viewModel.SelectedList.ToList();
                    if (searchedEmployeeList.Count > 0)
                    {
                        foreach (var item in searchedEmployeeList)
                        {
                            if (EmployeesAssignedRules.Where(c => c.employee_id == item.employee_id).Count() == 0)
                            {
                                if (NotAssignedEmp.FirstOrDefault(c => c.employee_id == item.employee_id) == null)
                                {
                                    view_EmployeesAssignedRules temp = new view_EmployeesAssignedRules();
                                    temp.employee_id = item.employee_id;
                                    temp.first_name = item.first_name;
                                    temp.surname = item.surname;
                                    temp.emp_id = item.emp_id;
                                    temp.rule_id = CurrentFixedRule.rule_id;
                                    NotAssignedEmp.Add(temp);
                                }
                            }
                        }
                        NewEmployeesFixedRules = NotAssignedEmp;
                    }
                }
                window.Close();
                window = null;
            }

        }
        private void Add()
        {
            if (SelectedEmployeesAssignedRuleList.Count > 0)
            {
                List<view_EmployeesAssignedRules> TempList = new List<view_EmployeesAssignedRules>();
                foreach (view_EmployeesAssignedRules item in SelectedEmployeesAssignedRuleList)
                {
                    item.special_amount = Amount;
                    item.default_qty = Quantity;
                    TempList.Add(item);
                    if (UpdatedEmpRuleQtyAndAmount.Count > 0)
                    {
                        var res = UpdatedEmpRuleQtyAndAmount.FirstOrDefault(c => c.employee_id == item.employee_id && c.rule_id == item.rule_id);
                        if (res != null)
                        {
                            UpdatedEmpRuleQtyAndAmount.Remove(res);
                            UpdatedEmpRuleQtyAndAmount.Add(item);
                        }
                        else
                            UpdatedEmpRuleQtyAndAmount.Add(item);
                    }
                    else
                        UpdatedEmpRuleQtyAndAmount.Add(item);

                }
                foreach (var item in EmployeesAssignedRules.Where(c => !TempList.Any(d => d.employee_id == c.employee_id && d.rule_id == c.rule_id)))
                {
                    TempList.Add(item);
                }
                EmployeesAssignedRules = TempList;
                SelectedEmployeesAssignedRuleList.Clear();
                Amount = 0;
                Quantity = 1;
            }
            else if (SelectedNewEmployeesForRuleList.Count > 0)
            {
                List<view_EmployeesAssignedRules> TempList = new List<view_EmployeesAssignedRules>();
                foreach (view_EmployeesAssignedRules item in SelectedNewEmployeesForRuleList)
                {
                    item.special_amount = Amount;
                    item.default_qty = Quantity;
                    TempList.Add(item);
                }
                foreach (var item in NewEmployeesFixedRules.Where(c => !TempList.Any(d => d.employee_id == c.employee_id)))
                {
                    TempList.Add(item);
                }
                NewEmployeesFixedRules = TempList;
                SelectedEmployeesAssignedRuleList.Clear();
                Amount = 0;
                Quantity = 1;
            }
        }
        private void NewListAsign()
        {
            UpdatedEmpRuleQtyAndAmount = new List<view_EmployeesAssignedRules>();
            NewEmployeesFixedRules = null;
        }
        private void Save()
        {
            if (ValidateSave())
            {
                List<view_EmployeesAssignedRules> SaveObj = new List<view_EmployeesAssignedRules>();
                if (NewEmployeesFixedRules != null)
                {
                    SaveObj = NewEmployeesFixedRules.ToList();
                }
                if (serviceClient.SaveUpdateFixedRules(SaveObj.ToArray(), UpdatedEmpRuleQtyAndAmount.ToArray(), clsSecurity.loggedUser.user_id))
                {
                    clsMessages.setMessage("Employee Rules Save/Update Successful..");
                    New();
                }
                else
                    clsMessages.setMessage("Employee Rules Save/Update Failed..");
            }
        }
        private bool ValidateSave()
        {
            if (CurrentFixedRule == null)
            {
                clsMessages.setMessage("Please Select A Fixed Rule Before Selecting Employees...");
                return false;
            }
            else if (CurrentFixedRule.rule_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Fixed Rule Before Selecting Employees...");
                return false;
            }
            else if (NewEmployeesFixedRules == null && UpdatedEmpRuleQtyAndAmount.Count == 0)
            {
                clsMessages.setMessage("There is No Change Done To Quantity And Amount Fields To Update/ Save Employee Records...");
                return false;
            }
            else if (NewEmployeesFixedRules != null || UpdatedEmpRuleQtyAndAmount.Count > 0)
            {
                if (NewEmployeesFixedRules != null)
                {
                    if (NewEmployeesFixedRules.Where(c => c.default_qty == null || c.special_amount == null).Count() > 0)
                    {
                        clsMessages.setMessage("Please Enter Quantity And Amount For All Newly Selected Employees...");
                        return false;
                    }
                }
                return true;
            }
            else
                return true;
        }
        private void New()
        {
            NewEmployeesFixedRules = null;
            EmployeesAssignedRules = null;
            searchedEmployeeList = new List<EmployeeSearchView>();
            UpdatedEmpRuleQtyAndAmount = new List<view_EmployeesAssignedRules>();
            RefreshFixedRules();
            Amount = 0;
            Quantity = 1;
        }
        private void Delete()
        {
            if (SelectedEmployeesAssignedRuleList.Count > 0)
            {
                List<view_EmployeesAssignedRules> DeleteEmployees = new List<view_EmployeesAssignedRules>();
                foreach (view_EmployeesAssignedRules item in SelectedEmployeesAssignedRuleList)
                {
                    DeleteEmployees.Add(item);
                }
                if (serviceClient.DeleteEmployeesFiedRule(DeleteEmployees.ToArray()))
                {
                    clsMessages.setMessage("Selected Employee's fixed Rule Deleted Successfully...");
                    New();
                }
                else
                    clsMessages.setMessage("Selected Employee's fixed Rule Deleting Process Failed...");
            }
            else if (NewEmployeesFixedRules != null)
            {
                List<view_EmployeesAssignedRules> tempDeleteObj = new List<view_EmployeesAssignedRules>();
                tempDeleteObj = NewEmployeesFixedRules.ToList();
                foreach (view_EmployeesAssignedRules item in SelectedNewEmployeesForRuleList)
                {
                    tempDeleteObj.Remove(item);
                }
                NewEmployeesFixedRules = tempDeleteObj;
            }
            else
                clsMessages.setMessage("Please Select Employee Rules Before You Click The Delete Button...");

        }

        #endregion

        #region Non Fixed Rules Methods
        private void SelectEmployeeForNonFixedRules()
        {
            if (CurrentNonFixedRule == null)
            {
                clsMessages.setMessage("Please Select A Non Fixed Rule Before Selecting Employees...");
            }
            else if (CurrentNonFixedRule.rule_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Non Fixed Rule Before Selecting Employees...");
            }
            else
            {
                EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
                window.ShowDialog();
                List<view_EmployeeWiseAssignedRulesForNonFixed> NotAssignedEmp = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
                if (NewEmployeesForNonFixedRules != null)
                {
                    NotAssignedEmp = NewEmployeesForNonFixedRules.ToList();
                }
                if (window.viewModel.SelectedList != null)
                {
                    SearchedEmpListForNonFixedRules.Clear();
                    SearchedEmpListForNonFixedRules = window.viewModel.SelectedList.ToList();
                    if (SearchedEmpListForNonFixedRules.Count > 0)
                    {
                        foreach (var item in SearchedEmpListForNonFixedRules)
                        {
                            if (EmpCompanyRules.Where(c => c.employee_id == item.employee_id).Count() == 0)
                            {
                                if (NotAssignedEmp.FirstOrDefault(c => c.employee_id == item.employee_id) == null)
                                {
                                    view_EmployeeWiseAssignedRulesForNonFixed temp = new view_EmployeeWiseAssignedRulesForNonFixed();
                                    temp.employee_id = item.employee_id;
                                    temp.first_name = item.first_name;
                                    temp.surname = item.surname;
                                    temp.rule_id = CurrentNonFixedRule.rule_id;
                                    temp.emp_id = item.emp_id;
                                    NotAssignedEmp.Add(temp);
                                }
                            }
                        }
                        NewEmployeesForNonFixedRules = NotAssignedEmp;
                    }
                }
                window.Close();
                window = null;
            }
        }
        private void NewNonFixed()
        {
            EmpCompanyRules = null;
            NewEmployeesForNonFixedRules = null;
            AllEmpRuleList = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
            SearchedEmpListForNonFixedRules = new List<EmployeeSearchView>();
            UpdatedEmpNonFixedRuleAmountAndIsspecial = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
            RefreshNonFixedRules();
            refreshEmployeeCompanyDetailsView();
        }
        private void FilterEmployeesByRule()
        {
            EmpCompanyRules = AllEmpRuleList.Where(c => c.rule_id == CurrentNonFixedRule.rule_id);
        }
        private void AddValueNonFixedRules()
        {
            if (SelectedEmpCompanyRuleList.Count > 0)
            {
                List<view_EmployeeWiseAssignedRulesForNonFixed> TempList = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
                foreach (view_EmployeeWiseAssignedRulesForNonFixed item in SelectedEmpCompanyRuleList)
                {
                    item.special_amount = NonFixedRulesSpecialAmount;
                    item.is_special = IsSpecial;
                    TempList.Add(item);
                    if (UpdatedEmpNonFixedRuleAmountAndIsspecial.Count > 0)
                    {
                        var res = UpdatedEmpNonFixedRuleAmountAndIsspecial.FirstOrDefault(c => c.employee_id == item.employee_id && c.rule_id == item.rule_id);
                        if (res != null)
                        {
                            UpdatedEmpNonFixedRuleAmountAndIsspecial.Remove(res);
                            UpdatedEmpNonFixedRuleAmountAndIsspecial.Add(item);
                        }
                        else
                            UpdatedEmpNonFixedRuleAmountAndIsspecial.Add(item);
                    }
                    else
                        UpdatedEmpNonFixedRuleAmountAndIsspecial.Add(item);

                }
                foreach (var item in EmpCompanyRules.Where(c => !TempList.Any(d => d.employee_id == c.employee_id && d.rule_id == c.rule_id)))
                {
                    TempList.Add(item);
                }
                EmpCompanyRules = TempList;
                SelectedNewEmployeeForNonFixedRuleList.Clear();
                NonFixedRulesSpecialAmount = 0;
                IsSpecial = false;
            }
            else if (SelectedNewEmployeeForNonFixedRuleList.Count > 0)
            {
                List<view_EmployeeWiseAssignedRulesForNonFixed> TempList = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
                foreach (view_EmployeeWiseAssignedRulesForNonFixed item in SelectedNewEmployeeForNonFixedRuleList)
                {
                    item.special_amount = NonFixedRulesSpecialAmount;
                    item.is_special = IsSpecial;
                    TempList.Add(item);
                }
                foreach (var item in NewEmployeesForNonFixedRules.Where(c => !TempList.Any(d => d.employee_id == c.employee_id)))
                {
                    TempList.Add(item);
                }
                NewEmployeesForNonFixedRules = TempList;
                SelectedEmpCompanyRuleList.Clear();
                NonFixedRulesSpecialAmount = 0;
                IsSpecial = false;
            }

        }
        private void SaveNewNonFixedRules()
        {
            if (ValidateSaveNonFixedRules())
            {
                List<view_EmployeeWiseAssignedRulesForNonFixed> SaveObj = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
                if (NewEmployeesForNonFixedRules != null)
                {
                    SaveObj = NewEmployeesForNonFixedRules.ToList(); 
                }
                if (serviceClient.SaveUpdateNonFixedRules(SaveObj.ToArray(), UpdatedEmpNonFixedRuleAmountAndIsspecial.ToArray(), clsSecurity.loggedUser.user_id))
                {
                    clsMessages.setMessage("Employees Non Fixed Rules Save/Update Successful..");
                    NewNonFixed();
                }
                else
                    clsMessages.setMessage("Employee Rules Save/Update Failed..");
            }
        }
        private bool ValidateSaveNonFixedRules()
        {
            if (CurrentNonFixedRule == null)
            {
                clsMessages.setMessage("Please Select A Non Fixed Rule Before Selecting Employees...");
                return false;
            }
            else if (CurrentNonFixedRule.rule_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Non Fixed Rule Before Selecting Employees...");
                return false;
            }
            else if (NewEmployeesForNonFixedRules == null && UpdatedEmpNonFixedRuleAmountAndIsspecial.Count == 0)
            {
                clsMessages.setMessage("There is No Change Done To 'Is Special' And 'Amount' Fields To Update/ Save Employee Records...");
                return false;
            }
            else if (NewEmployeesForNonFixedRules != null)
            {
                if (NewEmployeesForNonFixedRules.Count() > 0 || UpdatedEmpNonFixedRuleAmountAndIsspecial.Count > 0)
                {
                    if (NewEmployeesForNonFixedRules.Count() > 0)
                    {
                        if (NewEmployeesForNonFixedRules.Where(c => c.is_special == null || c.special_amount == null).Count() > 0)
                        {
                            clsMessages.setMessage("Please Enter Amount For All Newly Selected Employees...");
                            return false;
                        }
                    }
                    return true;
                }
                return true;
            }

            else
                return true;
        }
        private void DeleteNonFixedRules()
        {
            if (SelectedEmpCompanyRuleList.Count > 0)
            {
                List<view_EmployeeWiseAssignedRulesForNonFixed> DeleteEmployees = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
                foreach (view_EmployeeWiseAssignedRulesForNonFixed item in SelectedEmpCompanyRuleList)
                {
                    DeleteEmployees.Add(item);
                }
                if (serviceClient.DeleteNonFixedRules(DeleteEmployees.ToArray()))
                {
                    clsMessages.setMessage("Selected Employee's Non fixed Rule Deleted Successfully...");
                    New();
                }
                else
                    clsMessages.setMessage("Selected Employee's Non fixed Rule Deleting Process Failed...");
            }
            else if (NewEmployeesForNonFixedRules != null)
            {
                List<view_EmployeeWiseAssignedRulesForNonFixed> tempDeleteObj = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
                tempDeleteObj = NewEmployeesForNonFixedRules.ToList();
                foreach (view_EmployeeWiseAssignedRulesForNonFixed item in SelectedNewEmployeeForNonFixedRuleList)
                {
                    tempDeleteObj.Remove(item);
                }
                NewEmployeesForNonFixedRules = tempDeleteObj;
            }
            else
                clsMessages.setMessage("Please Select Employee Rules Before You Click The Delete Button...");

        }

        #endregion

        #endregion
    }
}
