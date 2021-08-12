using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Payroll.ManualPayroll
{
    class RuleWiseAssignEmployeesViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<EmployeeRuleDetailsView> AllEmployeesAssignedRulesForSelectedPeriod;
        List<EmployeeRuleDetailsView> AllEmployeeFixedRules;

        #endregion

        #region Constrouctor
        public RuleWiseAssignEmployeesViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }
        #endregion

        #region Properties

        private IEnumerable<z_Period> _Periods;
        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod != null) { RefreshNonFixedRulesAssignedToCurrentPeriod(); RefreshAllEmployeeFixedRulesAsignedToPeriod(); IsPeriodSelected = true; } }
        }

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
            set { _CurrentNonFixedRule = value; OnPropertyChanged("CurrentNonFixedRule"); if (CurrentNonFixedRule != null)GetAlreadyAssignedEmoployeestoPeriodAndNotAssignedEmployees(); }
        }

        private IEnumerable<view_EmployeeWiseAssignedRulesForNonFixed> _EmployeesAssignedRules;
        public IEnumerable<view_EmployeeWiseAssignedRulesForNonFixed> EmployeesAssignedRules
        {
            get { return _EmployeesAssignedRules; }
            set { _EmployeesAssignedRules = value; OnPropertyChanged("EmployeesAssignedRules"); }
        }

        private view_EmployeeWiseAssignedRulesForNonFixed _CurrentEmployeesAssignedRules;
        public view_EmployeeWiseAssignedRulesForNonFixed CurrentEmployeesAssignedRules
        {
            get { return _CurrentEmployeesAssignedRules; }
            set { _CurrentEmployeesAssignedRules = value; OnPropertyChanged("CurrentEmployeesAssignedRules"); }
        }

        private IEnumerable<EmployeeRuleDetailsView> _EmployeesAssignedRulesForSelectedPeriod;
        public IEnumerable<EmployeeRuleDetailsView> EmployeesAssignedRulesForSelectedPeriod
        {
            get { return _EmployeesAssignedRulesForSelectedPeriod; }
            set { _EmployeesAssignedRulesForSelectedPeriod = value; OnPropertyChanged("EmployeesAssignedRulesForSelectedPeriod"); }
        }

        private EmployeeRuleDetailsView _CurrentEmployeesAssignedRulesForSelectedPeriod;
        public EmployeeRuleDetailsView CurrentEmployeesAssignedRulesForSelectedPeriod
        {
            get { return _CurrentEmployeesAssignedRulesForSelectedPeriod; }
            set { _CurrentEmployeesAssignedRulesForSelectedPeriod = value; OnPropertyChanged("CurrentEmployeesAssignedRulesForSelectedPeriod"); }
        }

        private IEnumerable<view_EmployeesAssignedRules> _EmployeesAssignedFixedRules;
        public IEnumerable<view_EmployeesAssignedRules> EmployeesAssignedFixedRules
        {
            get { return _EmployeesAssignedFixedRules; }
            set { _EmployeesAssignedFixedRules = value; OnPropertyChanged("EmployeesAssignedFixedRules"); }
        }

        private view_EmployeesAssignedRules _CurrentEmployeesAssignedFixedRules;
        public view_EmployeesAssignedRules CurrentEmployeesAssignedFixedRules
        {
            get { return _CurrentEmployeesAssignedFixedRules; }
            set { _CurrentEmployeesAssignedFixedRules = value; OnPropertyChanged("CurrentEmployeesAssignedFixedRules"); }
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

        private IList _CurrentEmployeesAssignedRulesForSelectedPeriodList = new ArrayList();
        public IList CurrentEmployeesAssignedRulesForSelectedPeriodList
        {
            get { return _CurrentEmployeesAssignedRulesForSelectedPeriodList; }
            set { _CurrentEmployeesAssignedRulesForSelectedPeriodList = value; OnPropertyChanged("CurrentEmployeesAssignedRulesForSelectedPeriodList"); }
        }

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
                    // RefreshAllEmployeeFixedRules();
                    GetAlreadyAssignedFixedRulesAndNotAsignedFixedRules();
                }
            }
        }

        private IEnumerable<EmployeeRuleDetailsView> _EmployeeFixedRules;
        public IEnumerable<EmployeeRuleDetailsView> EmployeeFixedRules
        {
            get { return _EmployeeFixedRules; }
            set { _EmployeeFixedRules = value; OnPropertyChanged("EmployeeFixedRules"); }
        }

        private bool _IsPeriodSelected;
        public bool IsPeriodSelected
        {
            get { return _IsPeriodSelected; }
            set { _IsPeriodSelected = value; OnPropertyChanged("IsPeriodSelected"); }
        }

        #endregion

        #region Refresh Methods
        private void RefreshPeriods()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result.OrderBy(c => c.start_date);
            };
            serviceClient.GetPeriodsAsync();
        }
        private void RefreshNonFixedRules()
        {
            serviceClient.GetAllNonFixedRulesCompleted += (s, e) =>
            {
                NonFixedRules = e.Result.OrderBy(c=> c.rule_name);
            };
            serviceClient.GetAllNonFixedRulesAsync();
        }
        private void RefreshNonFixedRulesOfEmployees()
        {
                //serviceClient.GetCompanyRulesEmployeeWiseForNonFixedCompleted += (s, e) =>
                        //{
            EmployeesAssignedRules = serviceClient.GetCompanyRulesEmployeeWiseForNonFixed().OrderBy(c => c.rule_name);
                        //};
                //serviceClient.GetCompanyRulesEmployeeWiseForNonFixedAsync();
        }
        private void RefreshNonFixedRulesAssignedToCurrentPeriod()
        {
            serviceClient.GetEmployeeNonFixedRulesAssignedToPeriodCompleted += (s, e) =>
                {
                    if (e.Result != null && e.Result.Count() > 0)
                    {
                        AllEmployeesAssignedRulesForSelectedPeriod = e.Result.OrderBy(c => c.emp_id).ToList();
                    }
                };
            serviceClient.GetEmployeeNonFixedRulesAssignedToPeriodAsync(CurrentPeriod.period_id);
        }
        public void RefreshFixedRules()
        {
            serviceClient.GetAllFixedRulesCompleted += (s, e) =>
            {
                FixedRules = e.Result.OrderBy(c=> c.rule_name);
            };
            serviceClient.GetAllFixedRulesAsync();
        }
        private void RefreshAllEmployeeFixedRules()
        {
            serviceClient.GetAllFixedRulesAssigedToEmployeesCompleted += (s, e) =>
                {
                    EmployeesAssignedFixedRules = e.Result.OrderBy(c=> c.emp_id);
                };
            serviceClient.GetAllFixedRulesAssigedToEmployeesAsync(CurrentFixedRule.rule_id);
        }
        private void RefreshAllEmployeeFixedRulesAsignedToPeriod()
        {
            //serviceClient.GetEmployeeFixedRulesAsignedtoPeriodCompleted += (s, e) =>
                //{
                    //if (e.Result != null && e.Result.Count() > 0)
                    //{
            AllEmployeeFixedRules = serviceClient.GetEmployeeFixedRulesAsignedtoPeriod(CurrentPeriod.period_id).OrderBy(c => c.emp_id).ToList();
                    //}
                //};
            //serviceClient.GetEmployeeFixedRulesAsignedtoPeriodAsync(CurrentPeriod.period_id);
        }

        #endregion

        #region Button Commands
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }
        public ICommand SaveAllFixedRulesButton
        {
            get { return new RelayCommand(SaveAllFixedRules); }
        }
        public ICommand SaveNonFixedRulesButton
        {
            get { return new RelayCommand(SaveNonFixedRules); }
        }

        #endregion

        #region Methods
        private void New()
        {
            AllEmployeesAssignedRulesForSelectedPeriod = new List<EmployeeRuleDetailsView>();
            AllEmployeeFixedRules = new List<EmployeeRuleDetailsView>();
            IsPeriodSelected = false;
            EmployeesAssignedRulesForSelectedPeriod = null;
            EmployeeFixedRules = null;
            RefreshPeriods();
            RefreshNonFixedRules();
            RefreshFixedRules();
            RefreshNonFixedRulesOfEmployees();

        }
        private void GetAlreadyAssignedEmoployeestoPeriodAndNotAssignedEmployees()
        {
            List<view_EmployeeWiseAssignedRulesForNonFixed> AllNotAssignedEmpforPeriod = EmployeesAssignedRules.Where(c => c.rule_id == CurrentNonFixedRule.rule_id).OrderBy(c => c.emp_id).ToList();
            List<EmployeeRuleDetailsView> AllEmpAsignedForselectedPeriodandSelectedRule = AllEmployeesAssignedRulesForSelectedPeriod.Where(c => c.rule_id == CurrentNonFixedRule.rule_id && c.period_id == CurrentPeriod.period_id).ToList();
            List<view_EmployeeWiseAssignedRulesForNonFixed> GetDifferenceOfNotAsignedEmpRulesWithAsignedRules = new List<view_EmployeeWiseAssignedRulesForNonFixed>();
            if (AllEmpAsignedForselectedPeriodandSelectedRule.Count > 0)
            {
                GetDifferenceOfNotAsignedEmpRulesWithAsignedRules = AllNotAssignedEmpforPeriod.Where(c => !AllEmpAsignedForselectedPeriodandSelectedRule.Any(d => d.employee_id == c.employee_id)).ToList();
                foreach (var item in GetDifferenceOfNotAsignedEmpRulesWithAsignedRules)
                {
                    EmployeeRuleDetailsView temp = new EmployeeRuleDetailsView();
                    temp.employee_id = item.employee_id;
                    temp.emp_id = item.emp_id;
                    temp.first_name = item.first_name;
                    temp.surname = item.surname;
                    temp.special_amount = item.special_amount;
                    temp.quantity = 0;
                    temp.nic = "1";
                    AllEmpAsignedForselectedPeriodandSelectedRule.Add(temp);
                }
                EmployeesAssignedRulesForSelectedPeriod = null;
                EmployeesAssignedRulesForSelectedPeriod = AllEmpAsignedForselectedPeriodandSelectedRule.ToList();
            }
            else
            {
                foreach (var item in AllNotAssignedEmpforPeriod)
                {
                    EmployeeRuleDetailsView temp = new EmployeeRuleDetailsView();
                    temp.employee_id = item.employee_id;
                    temp.emp_id = item.emp_id;
                    temp.first_name = item.first_name;
                    temp.surname = item.surname;
                    temp.special_amount = item.special_amount;
                    temp.quantity = 0;
                    temp.nic = "1";
                    AllEmpAsignedForselectedPeriodandSelectedRule.Add(temp);
                }
                EmployeesAssignedRulesForSelectedPeriod = null;
                EmployeesAssignedRulesForSelectedPeriod = AllEmpAsignedForselectedPeriodandSelectedRule.ToList();
            }

        }
        private void GetAlreadyAssignedFixedRulesAndNotAsignedFixedRules()
        {
            List<view_EmployeesAssignedRules> AllNotAssignedFixedRulesForEmpforPeriod = serviceClient.GetAllFixedRulesAssigedToEmployees(CurrentFixedRule.rule_id).OrderBy(c => c.emp_id).ToList();
            List<EmployeeRuleDetailsView> AllEmpFixedRulesAsignedForselectedPeriodandSelectedRule = AllEmployeeFixedRules.Where(c => c.rule_id == CurrentFixedRule.rule_id).ToList();
            List<view_EmployeesAssignedRules> GetDifferenceOfFixedRulesNotAsignedEmpRulesWithAsignedRules = new List<view_EmployeesAssignedRules>();
            if (AllEmpFixedRulesAsignedForselectedPeriodandSelectedRule.Count > 0)
            {
                GetDifferenceOfFixedRulesNotAsignedEmpRulesWithAsignedRules = AllNotAssignedFixedRulesForEmpforPeriod.Where(c => !AllEmpFixedRulesAsignedForselectedPeriodandSelectedRule.Any(d => d.employee_id == c.employee_id)).ToList();
                foreach (var item in GetDifferenceOfFixedRulesNotAsignedEmpRulesWithAsignedRules)
                {
                    EmployeeRuleDetailsView temp = new EmployeeRuleDetailsView();
                    temp.employee_id = item.employee_id;
                    temp.emp_id = item.emp_id;
                    temp.first_name = item.first_name;
                    temp.surname = item.surname;
                    temp.special_amount = item.special_amount;
                    temp.quantity = item.default_qty;
                    temp.nic = "1";
                    AllEmpFixedRulesAsignedForselectedPeriodandSelectedRule.Add(temp);
                }
                EmployeeFixedRules = null;
                EmployeeFixedRules = AllEmpFixedRulesAsignedForselectedPeriodandSelectedRule.ToList();
            }
            else
            {
                GetDifferenceOfFixedRulesNotAsignedEmpRulesWithAsignedRules = AllNotAssignedFixedRulesForEmpforPeriod.Where(c => !AllEmpFixedRulesAsignedForselectedPeriodandSelectedRule.Any(d => d.employee_id == c.employee_id)).ToList();
                foreach (var item in AllNotAssignedFixedRulesForEmpforPeriod)
                {
                    EmployeeRuleDetailsView temp = new EmployeeRuleDetailsView();
                    temp.employee_id = item.employee_id;
                    temp.emp_id = item.emp_id;
                    temp.first_name = item.first_name;
                    temp.surname = item.surname;
                    temp.special_amount = item.special_amount;
                    temp.quantity = item.default_qty;
                    temp.nic = "1";
                    AllEmpFixedRulesAsignedForselectedPeriodandSelectedRule.Add(temp);
                }
                EmployeeFixedRules = null;
                EmployeeFixedRules = AllEmpFixedRulesAsignedForselectedPeriodandSelectedRule.ToList();
            }

        }
        private void Add()
        {
            if (CurrentEmployeesAssignedRulesForSelectedPeriodList.Count > 0)
            {
                List<EmployeeRuleDetailsView> temp = new List<EmployeeRuleDetailsView>();
                foreach (EmployeeRuleDetailsView item in CurrentEmployeesAssignedRulesForSelectedPeriodList)
                {
                    item.special_amount = Amount;
                    item.quantity = Quantity;
                    temp.Add(item);
                }
                foreach (var item in EmployeesAssignedRulesForSelectedPeriod.Where(c => !temp.Any(d => d.employee_id == c.employee_id)))
                {
                    temp.Add(item);
                }
                EmployeesAssignedRulesForSelectedPeriod = null;
                EmployeesAssignedRulesForSelectedPeriod = temp.ToList();
                CurrentEmployeesAssignedRulesForSelectedPeriodList.Clear();
                Amount = 0;
                Quantity = 0;
            }
        }
        private void SaveAllFixedRules()
        {
            if (ValidateSaveFixedRules())
            {
                clsMessages.setMessage("Are You Sure You Want to Save All Fixed Rules of All Employees?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    if (serviceClient.SaveAllFixedRulesToEmpPeriodQty(CurrentPeriod.period_id, clsSecurity.loggedUser.user_id))
                    {
                        clsMessages.setMessage("All Not Assigned Fixed Rules For Employees Have Been Saved Successfully...");
                        New();
                    }
                    else
                    {
                        clsMessages.setMessage("Fixed Rules Saving Process Failed...");
                    }
                }
            }
        }
        private bool ValidateSaveFixedRules()
        {
            if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please Select A Period...");
                return false;
            }
            else
                return true;
        }
        private void SaveNonFixedRules()
        {
            if (ValidateSaveNonFixedRules())
            {
                clsMessages.setMessage("Are You Sure You Want Save Non Fixed Rules of Employees?", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    List<EmployeeRuleDetailsView> SaveNonFixedRules = new List<EmployeeRuleDetailsView>();
                    List<EmployeeRuleDetailsView> UpdateNonFixedRules = new List<EmployeeRuleDetailsView>();
                    foreach (var item in EmployeesAssignedRulesForSelectedPeriod)
                    {
                        if (item.nic == "1")
                        {
                            item.rule_id = CurrentNonFixedRule.rule_id;
                            item.period_id = CurrentPeriod.period_id;
                            SaveNonFixedRules.Add(item);
                        }
                        else
                        {
                            item.rule_id = CurrentNonFixedRule.rule_id;
                            item.period_id = CurrentPeriod.period_id;
                            UpdateNonFixedRules.Add(item);
                        }
                    }
                    if (serviceClient.SaveEmployeeRulesPeriodWise(SaveNonFixedRules.ToArray(), UpdateNonFixedRules.ToArray(), clsSecurity.loggedUser.user_id))
                    {
                        clsMessages.setMessage("Selected Non Fixed Rules Amount And Quantity Asigned To Employees SuccessFully...");
                        New();
                    }
                    else
                    {
                        clsMessages.setMessage("Selected Non Fixed Rules Amount And Quantity Asigned To Employees Failed...");
                    }
                }
            }
        }
        private bool ValidateSaveNonFixedRules()
        {
            if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please Select A Period...");
                return false;
            }
            else if (CurrentNonFixedRule == null)
            {
                clsMessages.setMessage("Please Select A Non FixedRule...");
                return false;
            }
            else if (EmployeesAssignedRulesForSelectedPeriod == null)
            {
                clsMessages.setMessage("There are No Employees To Save...");
                return false;
            }
            else
                return true;
        }

        #endregion
    }
}