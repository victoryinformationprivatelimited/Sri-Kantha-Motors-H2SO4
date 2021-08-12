using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Payroll.ManualPayroll
{
    class ProcessRulePeriodWiseViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<EmployeeSearchView> searchedEmployeeList;
        List<EmployeeRuleDetailsView> AllEmployeeFixedRuleDetails;
        List<EmployeeRuleDetailsView> AllEmployeeNonFixedRuleDetails;
        List<EmployeeSearchView> AllEmployeeList;
        List<EmployeeSearchView> NewlyAssignedEmployee;
        List<view_EmployeeWiseAssignedRulesForNonFixed> AllEmpRuleList;
        List<EmployeeRuleDetailsView> tempNonFixedRulesEmpWise;
        List<EmployeeRuleDetailsView> TempAdd;
        List<EmployeeSearchView> EmployeeUPDownKeyBinding;
        List<EmployeeRuleDetailsView> EmpRuleUpDownKeyBinding;
        List<EmployeeRuleDetailsView> tempNonFixedRulesNotAsignedToPeriod;
        List<EmployeeRuleDetailsView> RemoveNewFixedRulesForEmployee;
        #endregion

        #region Constructor
        public ProcessRulePeriodWiseViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }

        #endregion

        #region Properies

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
            set
            {
                _CurrentPeriod = value;
                OnPropertyChanged("CurrentPeriod");
                if (CurrentPeriod != null)
                {
                    RefreshNonFixedRulesForEmployees();
                    RefreshFixedRulesForEmployees();
                    GetAllEmployeesAssignedToSelectedPeriod();

                }
            }
        }

        private IEnumerable<EmployeeSearchView> _Employees;
        public IEnumerable<EmployeeSearchView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSearchView _CurrentEmployee;
        public EmployeeSearchView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                _CurrentEmployee = value;
                OnPropertyChanged("CurrentEmployee");
                if (CurrentEmployee != null)
                {
                    // RefreshEmployeeFixedRules();
                    FilterFixedRules();
                    FilterNonFixedRules();
                    Amount = 0;
                    Quantity = 0;
                }
            }
        }

        private IEnumerable<view_EmployeesAssignedRules> _EmployeeRules;
        public IEnumerable<view_EmployeesAssignedRules> EmployeeRules
        {
            get { return _EmployeeRules; }
            set { _EmployeeRules = value; OnPropertyChanged("EmployeeRules"); }
        }

        private view_EmployeesAssignedRules _CurrentEmployeeRule;
        public view_EmployeesAssignedRules CurrentEmployeeRule
        {
            get { return _CurrentEmployeeRule; }
            set { _CurrentEmployeeRule = value; OnPropertyChanged("CurrentEmployeeRule"); }
        }

        private IEnumerable<EmployeeRuleDetailsView> _AssignedFixedRulesForEmployee;
        public IEnumerable<EmployeeRuleDetailsView> AssignedFixedRulesForEmployee
        {
            get { return _AssignedFixedRulesForEmployee; }
            set { _AssignedFixedRulesForEmployee = value; OnPropertyChanged("AssignedFixedRulesForEmployee"); }
        }

        private EmployeeRuleDetailsView _CurrentAssignedFixedRulesForEmployee;
        public EmployeeRuleDetailsView CurrentAssignedFixedRulesForEmployee
        {
            get { return _CurrentAssignedFixedRulesForEmployee; }
            set { _CurrentAssignedFixedRulesForEmployee = value; OnPropertyChanged("CurrentAssignedFixedRulesForEmployee"); }
        }

        private IEnumerable<EmployeeRuleDetailsView> _AssignedNonFixedRulesForEmployees;
        public IEnumerable<EmployeeRuleDetailsView> AssignedNonFixedRulesForEmployees
        {
            get { return _AssignedNonFixedRulesForEmployees; }
            set { _AssignedNonFixedRulesForEmployees = value; OnPropertyChanged("AssignedNonFixedRulesForEmployees"); }
        }

        private EmployeeRuleDetailsView _CurrentAssignedNonFixedRulesForEmployee;
        public EmployeeRuleDetailsView CurrentAssignedNonFixedRulesForEmployee
        {
            get { return _CurrentAssignedNonFixedRulesForEmployee; }
            set
            {
                _CurrentAssignedNonFixedRulesForEmployee = value;
                OnPropertyChanged("CurrentAssignedNonFixedRulesForEmployee");
                if (CurrentAssignedNonFixedRulesForEmployee != null)
                {
                    Amount = (decimal)CurrentAssignedNonFixedRulesForEmployee.special_amount;
                    Quantity = (decimal)CurrentAssignedNonFixedRulesForEmployee.quantity;
                }
            }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                OnPropertyChanged("Amount");
                if (Amount != 0)
                    CurrentAssignedNonFixedRulesForEmployee.special_amount = Amount;
            }
        }

        private decimal _Quantity;
        public decimal Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
                OnPropertyChanged("Quantity");
                if (Quantity != 0)
                {
                    CurrentAssignedNonFixedRulesForEmployee.amount = Amount * Quantity;
                    CurrentAssignedNonFixedRulesForEmployee.quantity = Quantity;
                }
            }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null)FilterEmployee(); }
        }

        #endregion

        #region RefreshMethod
        private void RefreshPeriods()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result.OrderBy(c => c.start_date);
            };
            serviceClient.GetPeriodsAsync();
        }
        private void RefreshEmployeeFixedRules()
        {
            serviceClient.GetAllFixedRulesOfSelectedEmployeeCompleted += (s, e) =>
                {
                    EmployeeRules = e.Result;
                };
            serviceClient.GetAllFixedRulesOfSelectedEmployeeAsync();
        }
        private void RefreshFixedRulesForEmployees()
        {
            //serviceClient.GetEmployeeFixedRulesAsignedtoPeriodCompleted += (s, e) =>
            //    {
            //        if (e.Result != null && e.Result.Count() > 0)
            //        {
            //            AllEmployeeFixedRuleDetails = e.Result.ToList();
            //        }
            //    };
            //serviceClient.GetEmployeeFixedRulesAsignedtoPeriodAsync(CurrentPeriod.period_id);
            AllEmployeeFixedRuleDetails = serviceClient.GetEmployeeFixedRulesAsignedtoPeriod(CurrentPeriod.period_id).ToList();

        }
        private void RefreshNonFixedRulesForEmployees()
        {
            AllEmployeeNonFixedRuleDetails = serviceClient.GetEmployeeNonFixedRulesAssignedToPeriod(CurrentPeriod.period_id).ToList();
            //serviceClient.GetEmployeeNonFixedRulesAssignedToPeriodCompleted += (s, e) =>
            //    {
            //        if (e.Result != null && e.Result.Count() > 0)
            //        {
            //            AllEmployeeNonFixedRuleDetails = e.Result.ToList();
            //        }
            //    };
            //serviceClient.GetEmployeeNonFixedRulesAssignedToPeriodAsync(CurrentPeriod.period_id);
        }
        private void RefreshEmployees()
        {
            serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
                {
                    AllEmployeeList = e.Result.ToList();
                };
            serviceClient.GetEmloyeeSearchAsync();
        }
        private void refreshEmployeeCompanyDetailsView()
        {
            this.serviceClient.GetAllNonFixedRulesForAssignedEmployeesCompleted += (s, e) =>
            {
                if (e.Result != null && e.Result.Count() > 0)
                {
                    AllEmpRuleList = e.Result.ToList();
                    AsignAllEmpToEmployeeRuleView();
                }
            };
            this.serviceClient.GetAllNonFixedRulesForAssignedEmployeesAsync();
        }

        #endregion

        #region Button Commands
        public ICommand SelectEmployeeButton
        {
            get { return new RelayCommand(SelectEmployee); }
        }
        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }
        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }
        public ICommand NextEmployeeButton
        {
            get { return new RelayCommand(NextEmployee); }
        }
        public ICommand PreviousEmployeeButton
        {
            get { return new RelayCommand(PreviousEmployee); }
        }
        public ICommand NextRuleButton
        {
            get { return new RelayCommand(NextRule); }
        }
        public ICommand PreviousRuleButton
        {
            get { return new RelayCommand(PreviousRule); }
        }
        public ICommand DeleteNewFixesAsignedToEmployeeButton
        {
            get { return new RelayCommand(DeleteNewFixesAsignedToEmployee); }
        }

        #endregion

        #region Methods
        private void New()
        {
            Employees = null;
            AssignedNonFixedRulesForEmployees = null;
            AssignedFixedRulesForEmployee = null;
            searchedEmployeeList = new List<EmployeeSearchView>();
            AllEmployeeFixedRuleDetails = new List<EmployeeRuleDetailsView>();
            AllEmployeeNonFixedRuleDetails = new List<EmployeeRuleDetailsView>();
            AllEmployeeList = new List<EmployeeSearchView>();
            NewlyAssignedEmployee = new List<EmployeeSearchView>();
            AllEmployeeList = new List<EmployeeSearchView>();
            tempNonFixedRulesEmpWise = new List<EmployeeRuleDetailsView>();
            EmpRuleUpDownKeyBinding = new List<EmployeeRuleDetailsView>();
            EmployeeUPDownKeyBinding = new List<EmployeeSearchView>();
            RemoveNewFixedRulesForEmployee = new List<EmployeeRuleDetailsView>();
            //TempAdd = new List<EmployeeRuleDetailsView>();
            RefreshEmployees();
            RefreshPeriods();
            refreshEmployeeCompanyDetailsView();
            RefreshEmployeeFixedRules();
        }
        private void Save()
        {
            if (ValidateSave())
            {
                List<EmployeeRuleDetailsView> SaveRuleList = new List<EmployeeRuleDetailsView>();
                List<EmployeeRuleDetailsView> UpdateRuleList = new List<EmployeeRuleDetailsView>();
                foreach (var item in Employees)
                {
                    if (item.nic == "1")
                    {
                        foreach (var NonFixedNulesrules in tempNonFixedRulesEmpWise.Where(c => c.employee_id == item.employee_id))
                        {
                            NonFixedNulesrules.period_id = CurrentPeriod.period_id;
                            SaveRuleList.Add(NonFixedNulesrules);
                        }
                        foreach (var FixedRules in EmployeeRules.Where(c => c.employee_id == item.employee_id))
                        {
                            EmployeeRuleDetailsView ConvertFixedRules = new EmployeeRuleDetailsView();
                            ConvertFixedRules.rule_id = FixedRules.rule_id;
                            ConvertFixedRules.employee_id = FixedRules.employee_id;
                            ConvertFixedRules.period_id = CurrentPeriod.period_id;
                            ConvertFixedRules.special_amount = FixedRules.special_amount;
                            ConvertFixedRules.quantity = FixedRules.default_qty;
                            SaveRuleList.Add(ConvertFixedRules);
                        }
                    }
                    else
                    {
                        foreach (var NonFixedRules in AllEmployeeNonFixedRuleDetails.Where(c => c.employee_id == item.employee_id))
                        {
                            if (NonFixedRules.rate == 1000)
                            {
                                NonFixedRules.period_id = CurrentPeriod.period_id;
                                SaveRuleList.Add(NonFixedRules);
                            }
                            else
                            {
                                NonFixedRules.period_id = CurrentPeriod.period_id;
                                UpdateRuleList.Add(NonFixedRules);
                            }
                        }
                        foreach (var FixedRules in AllEmployeeFixedRuleDetails.Where(c => c.employee_id == item.employee_id))
                        {
                            if (FixedRules.rate == 1000)
                            {
                                FixedRules.period_id = CurrentPeriod.period_id;
                                SaveRuleList.Add(FixedRules);
                            }
                            else
                            {
                                FixedRules.period_id = CurrentPeriod.period_id;
                                UpdateRuleList.Add(FixedRules);
                            }
                        }
                    }
                }
                if (serviceClient.SaveEmployeeRulesPeriodWise(SaveRuleList.ToArray(), UpdateRuleList.ToArray(), clsSecurity.loggedUser.user_id))
                {
                    clsMessages.setMessage("Rules Of Selected Employees Have Been Successfully Saved And Updated... Now You Can Process The Payroll...");
                    New();
                }
                else
                {
                    clsMessages.setMessage("Rules Of Selected Employees Save And Update Process Failed...");
                }
            }
        }
        private bool ValidateSave()
        {
            if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please Select A Period Before Clicking The Select Employee Button...");
                return false;
            }
            else if (CurrentPeriod.period_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Period Before Clicking The Select Employee Button...");
                return false;
            }
            else if (Employees == null || Employees.Count() == 0)
            {
                clsMessages.setMessage("There Aren't Any Employees Selected. Please Select Employees...");
                return false;
            }
            else
                return true;

        }
        private void AsignAllEmpToEmployeeRuleView()
        {
            foreach (var item in AllEmpRuleList)
            {
                EmployeeRuleDetailsView temp = new EmployeeRuleDetailsView();
                temp.employee_id = item.employee_id;
                temp.rule_id = item.rule_id;
                temp.rule_name = item.rule_name;
                temp.special_amount = item.special_amount;
                temp.quantity = 0;
                temp.rate = 10000;
                tempNonFixedRulesEmpWise.Add(temp);
            }
        }
        private void SelectEmployee()
        {
            if (ValidateSelectedEmployee())
            {
                EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
                window.ShowDialog();
                List<EmployeeSearchView> NotAssignedEmp = new List<EmployeeSearchView>();
                List<EmployeeSearchView> AllNewEmptempList = new List<EmployeeSearchView>();
                if (Employees != null)
                {
                    NotAssignedEmp = Employees.ToList();
                }
                if (window.viewModel.SelectedList != null)
                {
                    // Employees = null;
                    //  searchedEmployeeList.Clear();
                    AllNewEmptempList = window.viewModel.SelectedList.ToList();
                    if (AllNewEmptempList != null && AllNewEmptempList.Count() > 0)
                    {
                        foreach (var item in AllNewEmptempList)
                        {
                            if (Employees.Where(c => c.employee_id == item.employee_id).Count() == 0)
                            {
                                EmployeeSearchView temp = new EmployeeSearchView();
                                temp.employee_id = item.employee_id;
                                temp.first_name = item.first_name;
                                temp.surname = item.surname;
                                temp.emp_id = item.emp_id;
                                temp.nic = "1";
                                NotAssignedEmp.Add(temp);
                                NewlyAssignedEmployee.Add(temp);
                            }
                        }
                        Employees = null;
                        Employees = NotAssignedEmp;
                    }
                }
                window.Close();
                window = null;
            }
        }
        private bool ValidateSelectedEmployee()
        {
            if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please Select A Period Before Clicking The 'Select Employee'Button...");
                return false;
            }
            else if (CurrentPeriod.period_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Period Before Clicking The 'Select Employee'Button...");
                return false;
            }
            else
                return true;
        }
        private void FilterNonFixedRules()
        {
            // List<EmployeeRuleDetailsView> tempNonFixed = new List<EmployeeRuleDetailsView>();
            if (CurrentEmployee.nic == "1")
            {
                List<EmployeeRuleDetailsView> temp = new List<EmployeeRuleDetailsView>();
                temp = tempNonFixedRulesEmpWise.Where(c => c.employee_id == CurrentEmployee.employee_id).ToList();
                foreach (var item in temp)
                {
                    item.rate = 1000;
                }
                AssignedNonFixedRulesForEmployees = temp;
                //AssignedNonFixedRulesForEmployees = tempNonFixedRulesEmpWise.Where(c => c.employee_id == CurrentEmployee.employee_id);
                //foreach (var item in AllEmpRuleList.Where(c=> c.employee_id == CurrentEmployee.employee_id))
                //{
                //    EmployeeRuleDetailsView temp = new EmployeeRuleDetailsView();
                //    temp.employee_id = item.employee_id;
                //    temp.rule_name = item.rule_name;
                //    temp.special_amount = item.special_amount;
                //    temp.quantity = 0;
                //    tempNonFixed.Add(temp);
                //}
                //AssignedNonFixedRulesForEmployees = null;
                //AssignedNonFixedRulesForEmployees = tempNonFixed;
            }
            else
            {
                List<EmployeeRuleDetailsView> tempNonFixedRules = new List<EmployeeRuleDetailsView>();
                tempNonFixedRulesNotAsignedToPeriod = new List<EmployeeRuleDetailsView>();
                tempNonFixedRules = AllEmployeeNonFixedRuleDetails.Where(c => c.employee_id == CurrentEmployee.employee_id).ToList();
                // tempNonFixedRulesNotAsignedToPeriod = tempNonFixedRulesEmpWise.Where(c => !tempNonFixedRules.Any(d => d.employee_id == c.employee_id && d.rule_id == c.rule_id && d.period_id == c.period_id)).ToList();
                tempNonFixedRulesNotAsignedToPeriod = tempNonFixedRulesEmpWise.Where(c => c.employee_id == CurrentEmployee.employee_id).ToList();
                tempNonFixedRulesNotAsignedToPeriod = tempNonFixedRulesNotAsignedToPeriod.Where(c => !tempNonFixedRules.Any(d => c.rule_id == d.rule_id)).ToList();
                foreach (var item in tempNonFixedRulesNotAsignedToPeriod)
                {
                    item.rate = 1000;
                    tempNonFixedRules.Add(item);
                    AllEmployeeNonFixedRuleDetails.Add(item);
                }
                AssignedNonFixedRulesForEmployees = tempNonFixedRules;
                // AssignedNonFixedRulesForEmployees = AllEmployeeNonFixedRuleDetails.Where(c => c.employee_id == CurrentEmployee.employee_id);
            }
        }
        private void FilterFixedRules()
        {
            if (CurrentEmployee.nic == "1")
            {
                TempAdd = new List<EmployeeRuleDetailsView>();
                //  RefreshEmployeeFixedRules();
                foreach (var item in EmployeeRules.Where(c => c.employee_id == CurrentEmployee.employee_id))
                {
                    EmployeeRuleDetailsView temp = new EmployeeRuleDetailsView();
                    temp.employee_id = CurrentEmployee.employee_id;
                    temp.rule_id = item.rule_id;
                    temp.rule_name = item.rule_name;
                    temp.special_amount = item.special_amount;
                    temp.quantity = item.default_qty;
                    temp.rate = 1000;
                    var result = RemoveNewFixedRulesForEmployee.FirstOrDefault(c => c.employee_id == item.employee_id && c.rule_id == item.rule_id);
                    TempAdd.Add(temp);

                }
                AssignedFixedRulesForEmployee = null;
                AssignedFixedRulesForEmployee = TempAdd;
            }
            else
            {
                List<EmployeeRuleDetailsView> TempFixedRules = new List<EmployeeRuleDetailsView>();
                List<EmployeeRuleDetailsView> TempNotAsignedFixedRules = new List<EmployeeRuleDetailsView>();
                List<EmployeeRuleDetailsView> TempMergedRulesOfEmp = new List<EmployeeRuleDetailsView>();
                foreach (var item in EmployeeRules.Where(c => c.employee_id == CurrentEmployee.employee_id))
                {
                    EmployeeRuleDetailsView temp = new EmployeeRuleDetailsView();
                    temp.employee_id = CurrentEmployee.employee_id;
                    temp.rule_id = item.rule_id;
                    temp.rule_name = item.rule_name;
                    temp.special_amount = item.special_amount;
                    temp.quantity = item.default_qty;
                    temp.rate = 1000;
                    TempFixedRules.Add(temp);
                }
                TempNotAsignedFixedRules = AllEmployeeFixedRuleDetails.Where(c => c.employee_id == CurrentEmployee.employee_id).ToList();
                TempMergedRulesOfEmp = TempFixedRules.Where(c => !TempNotAsignedFixedRules.Any(d => d.rule_id == c.rule_id && d.employee_id == c.employee_id)).ToList();
                foreach (var item in TempMergedRulesOfEmp)
                {

                    TempNotAsignedFixedRules.Add(item);
                    AllEmployeeFixedRuleDetails.Add(item);

                }
                AssignedFixedRulesForEmployee = TempNotAsignedFixedRules;
                //TempNotAsignedFixedRules  AssignedFixedRulesForEmployee = AllEmployeeFixedRuleDetails.Where(c => c.employee_id == CurrentEmployee.employee_id);
            }
        }
        private void GetAllEmployeesAssignedToSelectedPeriod()
        {
            try
            {
                List<Guid> EmployeeID = new List<Guid>();
                List<EmployeeSearchView> EmployeesAssignedToSelectedPeriod = new List<EmployeeSearchView>();
                List<EmployeeRuleDetailsView> EmpsOFFixedRules = AllEmployeeFixedRuleDetails.GroupBy(c => c.employee_id).Select(x => x.First()).ToList();
                List<EmployeeRuleDetailsView> EmpsOFNonFixedRules = AllEmployeeNonFixedRuleDetails.GroupBy(c => c.employee_id).Select(x => x.First()).ToList();

                if (EmpsOFFixedRules != null && EmpsOFFixedRules.Count > 0)
                {
                    foreach (var item in EmpsOFFixedRules)
                    {
                        EmployeeSearchView temp1 = new EmployeeSearchView();
                        EmployeeID.Add(item.employee_id);
                    } 
                }
                if (EmpsOFNonFixedRules != null && EmpsOFNonFixedRules.Count > 0)
                {
                    foreach (var item in EmpsOFNonFixedRules.Where(c => !EmployeeID.Any(d => d == c.employee_id)))
                    {
                        EmployeeSearchView temp1 = new EmployeeSearchView();
                        EmployeeID.Add(item.employee_id);
                    }
                    List<EmployeeSearchView> temp = AllEmployeeList.Where(c => EmployeeID.Any(x => x == c.employee_id)).ToList();
                    Employees = null;
                    Employees = temp;
                }
            }
            catch (Exception)
            {

            }

        }
        private void Delete()
        {
            if (ValidateDelete())
            {
                if (CurrentEmployee.nic == "1")
                {
                    clsMessages.setMessage("Are You Sure You Want To Delete Newly Selected Employee?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        List<EmployeeSearchView> Temp = new List<EmployeeSearchView>();
                        Temp = Employees.ToList();
                        Temp.Remove(CurrentEmployee);
                        Employees = Temp;
                        AssignedNonFixedRulesForEmployees = null;
                        AssignedFixedRulesForEmployee = null;
                    }
                }
                //else
                //{
                //    clsMessages.setMessage("Are You Sure You Want To Delete Already Exist Employee To Selected Period? This Will Delete All Quantities of This Employee...", Visibility.Visible);
                //    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                //    {
                //        if (serviceClient.DeleteEmployeeRuleFromPeriodQuantity(CurrentEmployee.employee_id, CurrentPeriod.period_id))
                //        {
                //            clsMessages.setMessage("Selected Employees Rule Quantities Have Been Cleared Successfully From Database...");
                //            New();
                //        }
                //        else
                //            clsMessages.setMessage("Selected Employees Rule Quantities Clearing Process Failed...");
                //    }

                //}
            }



        }
        private bool ValidateDelete()
        {
            if (CurrentEmployee == null)
            {
                clsMessages.setMessage("Please Select An Employee To Remove...");
                return false;
            }
            else if (CurrentEmployee.employee_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select An Employee To Remove...");
                return false;
            }
            else if (CurrentEmployee.nic != "1")
            {
                clsMessages.setMessage("Sorry You cannot Remove This Employee Because he is already Saved in Database...");
                return false;
            }
            else
                return true;
        }
        private void FilterEmployee()
        {
            Employees = Employees.Where(c => c.emp_id != null && c.emp_id.ToUpper().Contains(Search.ToUpper()));
        }
        private void NextEmployee()
        {
            if (Employees != null)
            {
                int ElementNo = 0;
                EmployeeUPDownKeyBinding.Clear();
                EmployeeUPDownKeyBinding = Employees.ToList();
                if (CurrentEmployee == null)
                {
                    CurrentEmployee = (EmployeeSearchView)Employees.ElementAt(ElementNo);
                }
                else
                {
                    ElementNo = EmployeeUPDownKeyBinding.IndexOf(CurrentEmployee);
                    ElementNo++;
                    if (ElementNo <= (EmployeeUPDownKeyBinding.Count - 1))
                    {
                        CurrentEmployee = (EmployeeSearchView)Employees.ElementAt(ElementNo);
                    }
                }
            }
        }
        private void PreviousEmployee()
        {
            if (Employees != null)
            {
                int ElementNo = 0;
                EmployeeUPDownKeyBinding.Clear();
                EmployeeUPDownKeyBinding = Employees.ToList();
                if (CurrentEmployee == null)
                {

                    CurrentEmployee = (EmployeeSearchView)Employees.ElementAt(0);
                }
                else
                {
                    ElementNo = EmployeeUPDownKeyBinding.IndexOf(CurrentEmployee);
                    ElementNo--;
                    if (ElementNo != -1)
                    {
                        CurrentEmployee = (EmployeeSearchView)Employees.ElementAt(ElementNo);
                    }
                }
            }
        }
        private void NextRule()
        {
            if (AssignedNonFixedRulesForEmployees != null)
            {
                int ElementNo = 0;
                EmpRuleUpDownKeyBinding.Clear();
                EmpRuleUpDownKeyBinding = AssignedNonFixedRulesForEmployees.ToList();
                if (CurrentEmployee == null)
                {
                    CurrentAssignedNonFixedRulesForEmployee = (EmployeeRuleDetailsView)AssignedNonFixedRulesForEmployees.ElementAt(ElementNo);
                }
                else
                {
                    ElementNo = EmpRuleUpDownKeyBinding.IndexOf(CurrentAssignedNonFixedRulesForEmployee);
                    ElementNo++;
                    if (ElementNo <= (EmpRuleUpDownKeyBinding.Count - 1))
                    {
                        CurrentAssignedNonFixedRulesForEmployee = (EmployeeRuleDetailsView)AssignedNonFixedRulesForEmployees.ElementAt(ElementNo);
                    }
                }
            }
        }
        private void PreviousRule()
        {
            if (AssignedNonFixedRulesForEmployees != null)
            {
                int ElementNo = 0;
                EmpRuleUpDownKeyBinding.Clear();
                EmpRuleUpDownKeyBinding = AssignedNonFixedRulesForEmployees.ToList();
                if (CurrentAssignedNonFixedRulesForEmployee == null)
                {
                    CurrentAssignedNonFixedRulesForEmployee = (EmployeeRuleDetailsView)AssignedNonFixedRulesForEmployees.ElementAt(ElementNo);
                }
                else
                {
                    ElementNo = EmpRuleUpDownKeyBinding.IndexOf(CurrentAssignedNonFixedRulesForEmployee);
                    ElementNo--;
                    if (ElementNo != -1)
                    {
                        CurrentAssignedNonFixedRulesForEmployee = (EmployeeRuleDetailsView)AssignedNonFixedRulesForEmployees.ElementAt(ElementNo);
                    }
                }
            }
        }
        private void DeleteNewFixesAsignedToEmployee()
        {

            //RemoveNewFixedRulesForEmployee = AssignedFixedRulesForEmployee.ToList();
            //RemoveNewFixedRulesForEmployee.Remove(CurrentAssignedFixedRulesForEmployee);
            //AssignedFixedRulesForEmployee = RemoveNewFixedRulesForEmployee;
        }


        #endregion

    }
}
