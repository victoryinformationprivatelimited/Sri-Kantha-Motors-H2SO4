using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Payroll
{
    class EmployeePayRuleDetailsViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<EmployeeRuleDetailsView> ListEmployeeRuleDetails;

        #endregion

        #region Constructor
        public EmployeePayRuleDetailsViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListEmployeeRuleDetails = new List<EmployeeRuleDetailsView>();
            RefreshEmployeeActRules();
            RefreshCompanyRules();
            RefreshPeriods();
            RefreshEmployees();
        }

        #endregion

        #region Properties

        private IEnumerable<EmployeeSearchView> _Employees;
        public IEnumerable<EmployeeSearchView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private IEnumerable<EmployeeSearchView> _SelectedEmployees;
        public IEnumerable<EmployeeSearchView> SelectedEmployees
        {
            get { return _SelectedEmployees; }
            set { _SelectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }

        private IList _CurrentEmployees = new ArrayList();
        public IList CurrentEmployees
        {
            get { return _CurrentEmployees; }
            set { _CurrentEmployees = value; OnPropertyChanged("CurrentEmployees"); if (CurrentEmployees != null && CurrentEmployees.Count > 0) FilterRuleDetails(); }
        }

        private EmployeeSearchView _CurrentEmployee;
        public EmployeeSearchView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }


        private IEnumerable<mas_CompanyRule> _CompanyRules;
        public IEnumerable<mas_CompanyRule> CompanyRules
        {
            get { return _CompanyRules; }
            set { _CompanyRules = value; OnPropertyChanged("CompanyRules"); }
        }

        private mas_CompanyRule _CurrentCompanyRule;
        public mas_CompanyRule CurrentCompanyRule
        {
            get { return _CurrentCompanyRule; }
            set { _CurrentCompanyRule = value; OnPropertyChanged("CurrentCompanyRule"); }
        }

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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }

        private IEnumerable<EmployeeRuleDetailsView> _EmployeeRuleDetails;
        public IEnumerable<EmployeeRuleDetailsView> EmployeeRuleDetails
        {
            get { return _EmployeeRuleDetails; }
            set { _EmployeeRuleDetails = value; OnPropertyChanged("EmployeeRuleDetails"); }
        }

        private IList _CurrentEmployeeRuleDetails = new ArrayList();
        public IList CurrentEmployeeRuleDetails
        {
            get { return _CurrentEmployeeRuleDetails; }
            set { _CurrentEmployeeRuleDetails = value; OnPropertyChanged("CurrentEmployeeRuleDetails"); }
        }

        private IEnumerable<EmployeeRuleDetailSummaryView> _SavedRuleDetails;
        public IEnumerable<EmployeeRuleDetailSummaryView> SavedRuleDetails
        {
            get { return _SavedRuleDetails; }
            set { _SavedRuleDetails = value; OnPropertyChanged("SavedRuleDetails"); }
        }

        private IList _CurrentSavedRuleDetails = new ArrayList();
        public IList CurrentSavedRuleDetails
        {
            get { return _CurrentSavedRuleDetails; }
            set { _CurrentSavedRuleDetails = value; OnPropertyChanged("CurrentSavedRuleDetails"); if (CurrentSavedRuleDetails != null) PopulateRuleDetails(); }
        }

        private IEnumerable<dtl_EmployeeRule> _EmployeeActRules;
        public IEnumerable<dtl_EmployeeRule> EmployeeActRules
        {
            get { return _EmployeeActRules; }
            set { _EmployeeActRules = value; OnPropertyChanged("EmployeeActRules"); }
        }


        private decimal? _Quantity;
        public decimal? Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; OnPropertyChanged("Quantity"); if (CurrentEmployees != null && CurrentEmployeeRuleDetails != null && Quantity != null) SetQuantity(); }
        }

        private decimal? _SpAmount;
        public decimal? SpAmount
        {
            get { return _SpAmount; }
            set { _SpAmount = value; OnPropertyChanged("SpAmount"); if (CurrentEmployees != null && CurrentEmployeeRuleDetails != null && SpAmount != null) SetSpAmount(); }
        }

        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; OnPropertyChanged("Active"); if (CurrentEmployees != null && CurrentEmployeeRuleDetails != null) SetActive(); }
        }


        #endregion

        #region RefreshMethods

        private void RefreshPeriods()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                try
                {
                    Periods = e.Result.Where(c => c.isdelete == false);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshPeriods() \n\n" + ex.Message);
                }
            };
            serviceClient.GetPeriodsAsync();
        }

        private void RefreshCompanyRules()
        {
            serviceClient.GetCompanyRulesCompleted += (s, e) =>
            {
                try
                {
                    CompanyRules = e.Result.Where(c => c.isActive == true && c.isdelete == false).OrderBy(c => c.deduction_id);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshCompanyRules() \n\n" + ex.Message);
                }
            };
            serviceClient.GetCompanyRulesAsync();
        }

        private void RefreshEmployees()
        {
            serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
            {
                try
                {
                    Employees = e.Result.OrderBy(c => c.emp_id);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshEmployeeSearch() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEmloyeeSearchAsync();
        }

        private void RefreshEmployeeRules()
        {
            serviceClient.GetEmployeeRuleDetailsByPeriodCompleted += (s, e) =>
            {
                try
                {
                    IEnumerable<EmployeeRuleDetailsView> ie = e.Result;
                    if (ie != null)
                        ListEmployeeRuleDetails = ie.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmployeeRuleDetailsByPeriodAsync(CurrentPeriod.period_id);
        }

        private void RefreshEmployeeRulesSummary()
        {
            serviceClient.GetEmployeeRuleDetailSummaryByPeriodCompleted += (s, e) =>
            {
                try
                {
                    SavedRuleDetails = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmployeeRuleDetailSummaryByPeriodAsync(CurrentPeriod.period_id);
        }


        private void RefreshEmployeeActRules()
        {
            serviceClient.GetFilteredEmployeeRulesCompleted += (s, e) =>
            {
                try
                {
                    EmployeeActRules = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetFilteredEmployeeRulesAsync();
        }

        #endregion

        #region Commands & Methods

        public ICommand BtnGetEmployees
        {
            get { return new RelayCommand(GetEmployees, GetEmployeesCE); }
        }

        private bool GetEmployeesCE()
        {
            if (CurrentPeriod != null && Employees != null && Employees.Count() > 0)
                return true;
            else
                return false;
        }

        private void GetEmployees()
        {
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
            {
                List<EmployeeSearchView> TempSelectedEmployees = new List<EmployeeSearchView>();
                if (SelectedEmployees != null && SelectedEmployees.Count() > 0)
                    TempSelectedEmployees = SelectedEmployees.ToList();

                foreach (var employee in window.viewModel.selectEmployeeList)
                {
                    if (TempSelectedEmployees.Count(c => c.employee_id == employee.employee_id) == 0)
                    {
                        EmployeeSearchView Temp = new EmployeeSearchView();
                        Temp.employee_id = employee.employee_id;
                        Temp.emp_id = employee.emp_id;
                        Temp.first_name = employee.first_name;
                        Temp.surname = employee.surname;

                        TempSelectedEmployees.Add(Temp);

                        if (EmployeeActRules != null && EmployeeActRules.Count(c => c.employee_id == Temp.employee_id) > 0)
                        {
                            foreach (var item in EmployeeActRules.Where(c => c.employee_id == Temp.employee_id))
                            {
                                EmployeeRuleDetailsView tempRule = new EmployeeRuleDetailsView();
                                tempRule.employee_id = employee.employee_id;
                                tempRule.detach_emp = Guid.Empty;
                                tempRule.rule_id = item.rule_id;
                                tempRule.rule_name = CompanyRules == null ? string.Empty : CompanyRules.FirstOrDefault(c => c.rule_id == item.rule_id).rule_name;
                                tempRule.period_id = CurrentPeriod.period_id;
                                tempRule.benifit_id = CompanyRules == null ? Guid.Empty : CompanyRules.FirstOrDefault(c => c.rule_id == item.rule_id).benifit_id;
                                tempRule.is_special = item.is_special;
                                tempRule.isactive = item.isactive;
                                tempRule.isdelete = item.isdelete;
                                tempRule.rate = CompanyRules == null ? 0 : CompanyRules.FirstOrDefault(c => c.rule_id == item.rule_id).rate;
                                tempRule.quantity = 0;
                                tempRule.amount = 0;
                                tempRule.special_amount = item.special_amount;
                                ListEmployeeRuleDetails.Add(tempRule);
                            }
                        }

                    }

                    SelectedEmployees = null;
                    SelectedEmployees = TempSelectedEmployees;
                }
            }
            window.Close();
        }

        public ICommand BtnAddCompanyRule
        {
            get { return new RelayCommand(AddCompanyRule, AddCompanyRuleCE); }
        }

        private bool AddCompanyRuleCE()
        {
            if (CurrentCompanyRule != null && CurrentEmployees != null && CurrentEmployees.Count > 0)
                return true;
            else
                return false;
        }

        private void AddCompanyRule()
        {
            foreach (EmployeeSearchView employee in CurrentEmployees)
            {
                if (ListEmployeeRuleDetails.Count(c => c.employee_id == employee.employee_id && c.rule_id == CurrentCompanyRule.rule_id) == 0)
                {
                    if (EmployeeActRules != null && EmployeeActRules.Count(c => c.employee_id == employee.employee_id && c.rule_id == CurrentCompanyRule.rule_id) > 0)
                    {
                        EmployeeRuleDetailsView Temp = new EmployeeRuleDetailsView();
                        Temp.employee_id = employee.employee_id;
                        Temp.detach_emp = Guid.Empty;
                        Temp.rule_id = CurrentCompanyRule.rule_id;
                        Temp.rule_name = CurrentCompanyRule.rule_name;
                        Temp.period_id = CurrentPeriod.period_id;
                        Temp.benifit_id = CurrentCompanyRule.benifit_id;
                        Temp.is_special = true;
                        Temp.isactive = true;
                        Temp.isdelete = false;
                        Temp.rate = CurrentCompanyRule.rate;
                        Temp.quantity = 0;
                        Temp.amount = 0;
                        Temp.special_amount = EmployeeActRules.FirstOrDefault(c => c.employee_id == employee.employee_id && c.rule_id == CurrentCompanyRule.rule_id).special_amount; ;
                        ListEmployeeRuleDetails.Add(Temp);
                    }
                    else
                    {
                        EmployeeRuleDetailsView Temp = new EmployeeRuleDetailsView();
                        Temp.employee_id = employee.employee_id;
                        Temp.detach_emp = Guid.Empty;
                        Temp.rule_id = CurrentCompanyRule.rule_id;
                        Temp.rule_name = CurrentCompanyRule.rule_name;
                        Temp.period_id = CurrentPeriod.period_id;
                        Temp.benifit_id = CurrentCompanyRule.benifit_id;
                        Temp.is_special = true;
                        Temp.isactive = true;
                        Temp.isdelete = false;
                        Temp.rate = CurrentCompanyRule.rate;
                        Temp.quantity = 0;
                        Temp.amount = 0;
                        Temp.special_amount = CurrentCompanyRule.rate;
                        ListEmployeeRuleDetails.Add(Temp);
                    }
                }
            }

            FilterRuleDetails();
        }

        private void FilterRuleDetails()
        {
            if (ListEmployeeRuleDetails != null && ListEmployeeRuleDetails.Count > 0)
            {
                try
                {
                    List<EmployeeRuleDetailsView> TempList = new List<EmployeeRuleDetailsView>();

                    foreach (EmployeeSearchView employee in CurrentEmployees)
                    {
                        foreach (EmployeeRuleDetailsView rule in ListEmployeeRuleDetails.Where(c => c.employee_id == employee.employee_id))
                        {
                            if (TempList.Count(c => c.rule_id == rule.rule_id) == 0)
                            {
                                EmployeeRuleDetailsView Temp = ListEmployeeRuleDetails.FirstOrDefault(c => c.employee_id == rule.employee_id && c.rule_id == rule.rule_id);
                                TempList.Add(Temp);
                            }
                        }
                    }

                    EmployeeRuleDetails = null;
                    EmployeeRuleDetails = TempList;

                    Quantity = SpAmount = null;
                    Active = false;
                }
                catch (Exception)
                {

                }
            }
        }

        private void SetQuantity()
        {
            foreach (EmployeeRuleDetailsView rule in CurrentEmployeeRuleDetails)
            {
                foreach (EmployeeSearchView employee in CurrentEmployees)
                {
                    if (ListEmployeeRuleDetails.Count(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id) > 0)
                    {
                        EmployeeRuleDetailsView temp = ListEmployeeRuleDetails.FirstOrDefault(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id);
                        temp.quantity = Quantity <= 0 ? 0 : Quantity.Value;
                        temp.amount = temp.quantity * temp.special_amount;
                    }
                }
            }
        }

        private void SetSpAmount()
        {
            foreach (EmployeeRuleDetailsView rule in CurrentEmployeeRuleDetails)
            {
                foreach (EmployeeSearchView employee in CurrentEmployees)
                {
                    if (ListEmployeeRuleDetails.Count(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id) > 0)
                    {
                        EmployeeRuleDetailsView temp = ListEmployeeRuleDetails.FirstOrDefault(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id);
                        temp.special_amount = SpAmount <= 0 ? 0 : SpAmount.Value;
                        temp.amount = temp.special_amount * temp.quantity;

                    }
                }
            }
        }

        private void SetActive()
        {
            foreach (EmployeeRuleDetailsView rule in CurrentEmployeeRuleDetails)
            {
                foreach (EmployeeSearchView employee in CurrentEmployees)
                {
                    if (ListEmployeeRuleDetails.Count(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id) > 0)
                    {
                        EmployeeRuleDetailsView temp = ListEmployeeRuleDetails.FirstOrDefault(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id);
                        temp.isactive = Active;
                    }
                }
            }
        }

        public ICommand BtnNew
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            SavedRuleDetails = null;
            SelectedEmployees = null;
            EmployeeRuleDetails = null;
            ListEmployeeRuleDetails.Clear();
            CurrentCompanyRule = null;
            CurrentPeriod = null;
            Quantity = null;
            SpAmount = null;
            Active = false;
        }

        public ICommand BtnSave
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave())
            {
                List<dtl_EmployeeRule> ListSaveRules = new List<dtl_EmployeeRule>();
                List<trns_EmployeePeriodQunatity> ListSavePeriodQty = new List<trns_EmployeePeriodQunatity>();

                foreach (var employee in SelectedEmployees)
                {
                    foreach (var trRule in ListEmployeeRuleDetails.Where(c => c.employee_id == employee.employee_id))
                    {
                        dtl_EmployeeRule tempRule = new dtl_EmployeeRule();
                        tempRule.employee_id = (Guid)trRule.employee_id;
                        tempRule.rule_id = (Guid)trRule.rule_id;
                        tempRule.special_amount = trRule.special_amount;
                        tempRule.is_special = trRule.is_special;
                        tempRule.save_datetime = DateTime.Now;
                        tempRule.save_user_id = clsSecurity.loggedUser.user_id;
                        tempRule.isactive = trRule.isactive;
                        tempRule.isdelete = trRule.isdelete;
                        ListSaveRules.Add(tempRule);

                        trns_EmployeePeriodQunatity tempQty = new trns_EmployeePeriodQunatity();
                        tempQty.employee_id = (Guid)trRule.employee_id;
                        tempQty.rule_id = (Guid)trRule.rule_id;
                        tempQty.period_id = CurrentPeriod.period_id;
                        tempQty.quantity = trRule.quantity;
                        tempQty.is_proceed = false;
                        tempQty.save_datetime = DateTime.Now;
                        tempQty.save_user_id = clsSecurity.loggedUser.user_id;
                        tempQty.isdelete = trRule.isdelete;
                        ListSavePeriodQty.Add(tempQty);
                    }
                }

                if (serviceClient.SaveUpdateEmployeeRuleDetails(ListSaveRules.ToArray(), ListSavePeriodQty.ToArray()))
                {
                    ReloadData();
                    clsMessages.setMessage("Records saved/updated successfully");
                }
                else
                    clsMessages.setMessage("Records save/update failed");
            }
        }

        private void ReloadData()
        {
            SavedRuleDetails = null;
            SelectedEmployees = null;
            EmployeeRuleDetails = null;
            ListEmployeeRuleDetails.Clear();
            CurrentCompanyRule = null;
            Quantity = null;
            SpAmount = null;
            Active = false;
            RefreshEmployeeRules();
            RefreshEmployeeRulesSummary();
        }

        private bool ValidateSave()
        {
            if(!clsSecurity.GetSavePermission(516))
            {
                clsMessages.setMessage("You don't have permission to save this record");
                return false;
            }
            else if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please select a pay period and try again..");
                return false;
            }
            else if (SelectedEmployees == null || SelectedEmployees.Count() == 0)
            {
                clsMessages.setMessage("Please select employees and try again..");
                return false;
            }
            else if (ListEmployeeRuleDetails == null || ListEmployeeRuleDetails.Count == 0)
            {
                clsMessages.setMessage("Please set rules to each employee and try again..");
                return false;
            }
            else
            {
                bool status = false;

                foreach (var employee in SelectedEmployees)
                {
                    if (ListEmployeeRuleDetails.Count(c => c.employee_id == employee.employee_id) > 0)
                    {
                        foreach (var rule in ListEmployeeRuleDetails.Where(c => c.employee_id == employee.employee_id))
                        {
                            if (rule.rate == null || rule.quantity == null || rule.special_amount == null)
                            {
                                clsMessages.setMessage("Invalid Rate,Quantity or Special Amount");
                                CurrentEmployees = null;
                                CurrentEmployees = SelectedEmployees.Where(c => c.employee_id == employee.employee_id).Cast<EmployeeSearchView>().ToList();
                                status = false;
                                break;
                            }
                            else
                            {
                                status = true;
                            }
                        }

                        if (status == false)
                            break;
                    }
                    else
                    {
                        clsMessages.setMessage("Employee with no rule details exists, please check and try again..");
                        IList Temp = new ArrayList();
                        Temp.Add(SelectedEmployees.FirstOrDefault(c => c.employee_id == employee.employee_id));
                        CurrentEmployees = null;
                        CurrentEmployee = null;
                        CurrentEmployees = Temp;
                        CurrentEmployee = (EmployeeSearchView)Temp[0];
                        status = false;
                        break;
                    }
                }

                if (ListEmployeeRuleDetails.Count(c => c.period_id != CurrentPeriod.period_id) > 0)
                {
                    clsMessages.setMessage("Current period doesn't seem to match the Rule detail periods, do you want to proceed?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        status = true;
                    }
                    else
                        status = false;
                }

                return status;
            }
        }

        public ICommand BtnDelete
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (CurrentEmployeeRuleDetails != null && CurrentEmployeeRuleDetails.Count > 0)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(516))
            {
                List<trns_EmployeePeriodQunatity> TempDeleteList = new List<trns_EmployeePeriodQunatity>();

                foreach (EmployeeSearchView employee in CurrentEmployees)
                {
                    foreach (EmployeeRuleDetailsView rule in CurrentEmployeeRuleDetails)
                    {
                        if (ListEmployeeRuleDetails.Count(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id && c.detach_emp != Guid.Empty) > 0)
                        {
                            EmployeeRuleDetailsView Temp = ListEmployeeRuleDetails.FirstOrDefault(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id && c.detach_emp != Guid.Empty);
                            trns_EmployeePeriodQunatity TempDel = new trns_EmployeePeriodQunatity();
                            TempDel.employee_id = Temp.employee_id;
                            TempDel.rule_id = Temp.rule_id;
                            TempDel.period_id = Temp.period_id;
                            TempDel.isdelete = true;
                            TempDel.delete_datetime = DateTime.Now;
                            TempDel.delete_user_id = clsSecurity.loggedUser.user_id;
                            TempDeleteList.Add(TempDel);
                        }
                    }
                }

                if (TempDeleteList.Count > 0)
                {
                    if (serviceClient.DeleteEmployeeRuleDetails(TempDeleteList.ToArray()))
                    {
                        ReloadData();
                        clsMessages.setMessage("Record(s) deleted successfully");
                    }
                    else
                        clsMessages.setMessage("Record(s) delete failed");
                }
            }
            else
                clsMessages.setMessage("You don't have permission to delete this record");

        }

        public ICommand BtnGetData
        {
            get { return new RelayCommand(GetData, GetDataCE); }
        }

        private bool GetDataCE()
        {
            if (CurrentPeriod != null)
                return true;
            else
                return false;
        }

        private void GetData()
        {
            SelectedEmployees = null;
            EmployeeRuleDetails = null;
            ListEmployeeRuleDetails.Clear();
            CurrentCompanyRule = null;
            Quantity = null;
            SpAmount = null;
            Active = false;

            RefreshEmployeeRules();
            RefreshEmployeeRulesSummary();
        }

        private void PopulateRuleDetails()
        {
            if (ListEmployeeRuleDetails != null && ListEmployeeRuleDetails.Count > 0 && Employees != null && Employees.Count() > 0)
            {
                List<EmployeeSearchView> TempSelectedEmployees = new List<EmployeeSearchView>();
                int errorCount = 0;

                foreach (EmployeeRuleDetailSummaryView saveRule in CurrentSavedRuleDetails)
                {
                    foreach (var empRule in ListEmployeeRuleDetails.Where(c => c.rule_id == saveRule.rule_id))
                    {
                        if (TempSelectedEmployees.Count(c => c.employee_id == empRule.employee_id) == 0)
                        {
                            try
                            {
                                TempSelectedEmployees.Add(Employees.FirstOrDefault(c => c.employee_id == empRule.employee_id));
                            }
                            catch (Exception)
                            {
                                errorCount++;
                            }
                        }
                    }
                }

                if (errorCount > 0)
                    clsMessages.setMessage("Deleted or inactive employees will not be loaded..");

                SelectedEmployees = null;
                EmployeeRuleDetails = null;
                SelectedEmployees = TempSelectedEmployees;
            }
        }

        public ICommand CmdRemoveEmployee
        {
            get { return new RelayCommand(RemoveEmployee, RemoveEmployeeCE); }
        }

        private bool RemoveEmployeeCE()
        {
            if (CurrentEmployees != null && CurrentEmployees.Count > 0)
                return true;
            else
                return false;
        }

        private void RemoveEmployee()
        {
            List<EmployeeSearchView> TempSelectedEmployees = new List<EmployeeSearchView>();
            TempSelectedEmployees = SelectedEmployees.ToList();

            foreach (EmployeeSearchView employee in CurrentEmployees)
            {
                foreach (var rule in ListEmployeeRuleDetails.Where(c => c.employee_id == employee.employee_id).ToList())
                {
                    ListEmployeeRuleDetails.Remove(ListEmployeeRuleDetails.FirstOrDefault(c => c.employee_id == rule.employee_id && c.rule_id == rule.rule_id));
                }

                TempSelectedEmployees.Remove(TempSelectedEmployees.FirstOrDefault(c => c.employee_id == employee.employee_id));
            }

            SelectedEmployees = null;
            SelectedEmployees = TempSelectedEmployees;
            EmployeeRuleDetails = null;
        }

        public ICommand CmdRemoveRule
        {
            get { return new RelayCommand(RemoveRule, RemoveRuleCE); }
        }

        private bool RemoveRuleCE()
        {
            if (CurrentEmployeeRuleDetails != null && CurrentEmployeeRuleDetails.Count > 0)
                return true;
            else
                return false;
        }

        private void RemoveRule()
        {
            foreach (EmployeeSearchView employee in CurrentEmployees)
            {
                foreach (EmployeeRuleDetailsView rule in CurrentEmployeeRuleDetails)
                {
                    if (ListEmployeeRuleDetails.Count(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id) > 0)
                    {
                        ListEmployeeRuleDetails.Remove(ListEmployeeRuleDetails.FirstOrDefault(c => c.employee_id == employee.employee_id && c.rule_id == rule.rule_id));
                    }
                }
            }

            EmployeeRuleDetails = null;
            FilterRuleDetails();
        }

        #endregion
    }
}