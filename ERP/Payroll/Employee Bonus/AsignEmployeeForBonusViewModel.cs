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

namespace ERP.Payroll.Employee_Bonus
{
    class AsignEmployeeForBonusViewModel : ViewModelBase
    {
        #region Fields

        private ERPServiceClient serviceClient;
        List<EmployeeSearchView> SelectedEmployeesList;
        List<EmployeeSumarryView> AllEmployeeList;
        List<AsignedEmployeesForBonusByBonusPeriodView> AllAlreadyAsignedEmployeesForBonus;

        #endregion

        #region Constrouctor
        public AsignEmployeeForBonusViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }

        #endregion

        #region Properties

        private IEnumerable<EmployeeSumarryView> _NewEmployeesForBonus;
        public IEnumerable<EmployeeSumarryView> NewEmployeesForBonus
        {
            get { return _NewEmployeesForBonus; }
            set { _NewEmployeesForBonus = value; OnPropertyChanged("NewEmployeesForBonus"); }
        }

        private IEnumerable<AsignedEmployeesForBonusByBonusPeriodView> _AlreadyAsignedEmployeesForBonus;
        public IEnumerable<AsignedEmployeesForBonusByBonusPeriodView> AlreadyAsignedEmployeesForBonus
        {
            get { return _AlreadyAsignedEmployeesForBonus; }
            set { _AlreadyAsignedEmployeesForBonus = value; OnPropertyChanged("AlreadyAsignedEmployeesForBonus"); }
        }

        private AsignedEmployeesForBonusByBonusPeriodView _CurrentAlreadyAsignedEmployeeForBonus;
        public AsignedEmployeesForBonusByBonusPeriodView CurrentAlreadyAsignedEmployeeForBonus
        {
            get { return _CurrentAlreadyAsignedEmployeeForBonus; }
            set { _CurrentAlreadyAsignedEmployeeForBonus = value; OnPropertyChanged("CurrentAlreadyAsignedEmployeeForBonus"); if (CurrentAlreadyAsignedEmployeeForBonus != null)DisableTextFieldsAndRadioBttonsOnUpdate(); }
        }

        private IEnumerable<z_BonusPeriod> _BonusPeriod;
        public IEnumerable<z_BonusPeriod> BonusPeriod
        {
            get { return _BonusPeriod; }
            set { _BonusPeriod = value; OnPropertyChanged("BonusPeriod"); }
        }

        private z_BonusPeriod _CurrentBonusPeriod;
        public z_BonusPeriod CurrentBonusPeriod
        {
            get { return _CurrentBonusPeriod; }
            set { _CurrentBonusPeriod = value; OnPropertyChanged("CurrentBonusPeriod"); if (CurrentBonusPeriod != null)FilterBonusDetailsByPeriod();}
        }

        private bool _UpdateFalse;
        public bool UpdateFalse
        {
            get { return _UpdateFalse; }
            set { _UpdateFalse = value; OnPropertyChanged("UpdateFalse"); }
        }

        private decimal _Percentage;
        public decimal Percentage
        {
            get { return _Percentage; }
            set { _Percentage = value; OnPropertyChanged("Percentage"); }
        }

        private decimal _Amount;
        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; OnPropertyChanged("Amount"); }
        }

        private bool _FromBasicSalaryPercentage;
        public bool FromBasicSalaryPercentage
        {
            get { return _FromBasicSalaryPercentage; }
            set { _FromBasicSalaryPercentage = value; OnPropertyChanged("FromBasicSalaryPercentage"); if (FromBasicSalaryPercentage == false)Percentage = 0; }
        }

        private bool _FromStaticAmount;
        public bool FromStaticAmount
        {
            get { return _FromStaticAmount; }
            set { _FromStaticAmount = value; OnPropertyChanged("FromStaticAmount"); if (FromStaticAmount == false)Amount = 0; }
        }

        private bool _IsBonusAmountEnabled;
        public bool IsBonusAmountEnabled
        {
            get { return _IsBonusAmountEnabled; }
            set { _IsBonusAmountEnabled = value; OnPropertyChanged("IsBonusAmountEnabled"); }
        }

        private bool _IsBonusPercentageEnabled;
        public bool IsBonusPercentageEnabled
        {
            get { return _IsBonusPercentageEnabled; }
            set { _IsBonusPercentageEnabled = value; OnPropertyChanged("IsBonusPercentageEnabled"); }
        }

        private string _Search;
        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null)FilterEmployeeBonusByEmpID(); }
        }


        
        #endregion

        #region Refresh Methods

        private void RefreshAlreadyAsigedEmployeesForBonus()
        {
            serviceClient.GetAsignedEmployeesForBounsDetailsCompleted += (s, e) =>
                {
                    if (e.Result != null && e.Result.Count() > 0)
                    {
                        AllAlreadyAsignedEmployeesForBonus = e.Result.ToList();
                    }
                };
            serviceClient.GetAsignedEmployeesForBounsDetailsAsync();
        }
        private void RefreshAllEmployees()
        {
            serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
                {
                    if (e.Result != null && e.Result.Count() > 0)
                    {
                        AllEmployeeList = e.Result.ToList();
                    }
                };
            serviceClient.GetAllEmployeeDetailAsync();
        }
        private void RefreshBonusPeriod()
        {
            serviceClient.GetBonusPeriodCompleted += (s, e) =>
            {
                BonusPeriod = e.Result;
            };
            serviceClient.GetBonusPeriodAsync();
        }

        #endregion

        #region Button Commands
        public ICommand SelectEmployeesButtons
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
        public ICommand UpdateButton
        {
            get { return new RelayCommand(Update); }
        }

        #endregion

        #region Methods
        private void SelectEmployee()
        {
            if (ValidateSelectedEmployees())
            {
                EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
                window.ShowDialog();
                List<EmployeeSumarryView> NotAssignedEmp = new List<EmployeeSumarryView>();
                List<EmployeeSumarryView> TempEmpList = new List<EmployeeSumarryView>();
                if (NewEmployeesForBonus != null)
                {
                    NotAssignedEmp = NewEmployeesForBonus.ToList();  
                }
                if (window.viewModel.SelectedList != null)
                {
                    SelectedEmployeesList.Clear();
                    SelectedEmployeesList = window.viewModel.SelectedList.ToList();
                    TempEmpList = AllEmployeeList.Where(c => SelectedEmployeesList.Any(d => d.employee_id == c.employee_id)).ToList();
                    if (SelectedEmployeesList.Count > 0)
                    {
                        foreach (var item in TempEmpList)
                        {
                            if (AlreadyAsignedEmployeesForBonus.Where(c => c.employee_id == item.employee_id).Count() == 0)
                            {
                                if (NewEmployeesForBonus != null)
                                {
                                    if (NewEmployeesForBonus.Where(c => c.employee_id == item.employee_id).Count() == 0)
                                    {
                                        NotAssignedEmp.Add(item);
                                    } 
                                }
                                else
                                {
                                    NotAssignedEmp.Add(item);
                                }
                            }
                        }
                        NewEmployeesForBonus = NotAssignedEmp;
                    }
                }
                window.Close();
                window = null;
            }
        }
        private bool ValidateSelectedEmployees()
        {
            if (CurrentBonusPeriod == null)
            {
                clsMessages.setMessage("Please Select Bonus Period Before Selecting Employees...");
                return false;
            }
            else if (CurrentBonusPeriod.Bonus_Period_id == 0)
            {
                clsMessages.setMessage("Please Select Bonus Period Before Selecting Employees...");
                return false;
            }
            else
                return true;
        }
        private void New()
        {
            SelectedEmployeesList = new List<EmployeeSearchView>();
            AllEmployeeList = new List<EmployeeSumarryView>();
            AllAlreadyAsignedEmployeesForBonus = new List<AsignedEmployeesForBonusByBonusPeriodView>();
            RefreshAllEmployees();
            RefreshAlreadyAsigedEmployeesForBonus();
            NewEmployeesForBonus = null;
            AlreadyAsignedEmployeesForBonus = null;
           // CurrentAlreadyAsignedEmployeeForBonus = new AsignedEmployeesForBonusByBonusPeriodView();
            FromBasicSalaryPercentage = true;
            FromStaticAmount = false;
            RefreshBonusPeriod();
            UpdateFalse = false;
            IsBonusAmountEnabled = false;
            IsBonusPercentageEnabled = false;
            Amount = 0;
            Percentage = 0;
        }
        private void Save()
        {
            if (clsSecurity.GetSavePermission(518))
            {
                if (ValidateSave())
                {
                    clsMessages.setMessage("Are You Sure You Want to Save All Fixed Rules of All Employees?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        List<AsignedEmployeesForBonusByBonusPeriodView> AddEmpDetails = new List<AsignedEmployeesForBonusByBonusPeriodView>();
                        foreach (var item in NewEmployeesForBonus)
                        {
                            AsignedEmployeesForBonusByBonusPeriodView temp = new AsignedEmployeesForBonusByBonusPeriodView();
                            temp.employee_id = item.employee_id;
                            temp.AsignedDate = DateTime.Now;
                            temp.Bonus_Period_id = CurrentBonusPeriod.Bonus_Period_id;
                            if (FromBasicSalaryPercentage == true)
                            {
                                temp.BonusAmount = (item.basic_salary / 100) * Percentage;
                            }
                            else if (FromStaticAmount == true)
                            {
                                temp.BonusAmount = Amount;
                            }
                            temp.BonusPercentage = Percentage;
                            temp.FromBasicSalaryPercentage = FromBasicSalaryPercentage;
                            temp.FromStaticAmount = FromStaticAmount;
                            temp.Is_Processed = false;
                            temp.isactive = true;
                            temp.isdelete = false;
                            temp.save_datetime = DateTime.Now;
                            temp.save_user_id = clsSecurity.loggedUser.user_id;
                            temp.Employee_Basic_Salary = item.basic_salary;
                            AddEmpDetails.Add(temp);
                        }
                        if (serviceClient.SaveEmployeeWiseBonusToSelectedPeriod(AddEmpDetails.ToArray()))
                        {
                            clsMessages.setMessage("Employees were Successfully Assigned For Selected Bonus Period...");
                            New();
                        }
                        else
                            clsMessages.setMessage("Employees Assigning Process For Selected Bonus Period Failed...");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to save this record");
        }
        private bool ValidateSave()
        {
            if (CurrentBonusPeriod == null || CurrentBonusPeriod.Bonus_Period_id == 0)
            {
                clsMessages.setMessage("Please Select A Bonus Period Before Clicking The Save Button...");
                return false;
            }
            else if (NewEmployeesForBonus == null || NewEmployeesForBonus.Count() == 0)
            {
                clsMessages.setMessage("Please Select Employees Before Clicking The Save Button...");
                return false;
            }
            else if (NewEmployeesForBonus == null || NewEmployeesForBonus.Count() == 0)
            {
                clsMessages.setMessage("Please Select Employees Before Clicking The Save Button...");
                return false;
            }
            else if (FromBasicSalaryPercentage == true)
            {
                if (Percentage == null || Percentage == 0)
                {
                    clsMessages.setMessage("Please Enter Bonus Percentage...");
                    return false;
                }
                else if (Percentage<0)
                {
                    clsMessages.setMessage("Percentage Value Cannot be minus...");
                    return false;
                }
                return true;
            }
            else if (FromStaticAmount == true)
            {
                if (Amount == null || Amount == 0)
                {
                    clsMessages.setMessage("Please Enter Bonus Amount...");
                    return false;
                }
                else if (Amount < 0)
                {
                    clsMessages.setMessage("Bonus Value Cannot be minus...");
                    return false;
                }
                return true;
            }
            else
                return true;

        }
        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(518))
            {
                if (ValidateDelete())
                {
                    clsMessages.setMessage("Are You Sure You Want to Delete Selected Employees Bonus Details?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        CurrentAlreadyAsignedEmployeeForBonus.isactive = false;
                        CurrentAlreadyAsignedEmployeeForBonus.isdelete = true;
                        CurrentAlreadyAsignedEmployeeForBonus.delete_datetime = DateTime.Now;
                        CurrentAlreadyAsignedEmployeeForBonus.delete_user_id = clsSecurity.loggedUser.user_id;
                        if (serviceClient.DeleteEmployeeWiseBonusToSelectedPeriod(CurrentAlreadyAsignedEmployeeForBonus))
                        {
                            clsMessages.setMessage("You Have Successfully Deleted Employee's Bonus For The Current Period...");
                            New();
                        }
                        else
                            clsMessages.setMessage("Employee's Bonus Deletion Process Failed...");
                    }

                } 
            }
            else
                clsMessages.setMessage("You don't have permission to delete this record");
        }
        private bool ValidateDelete()
        {
            if (CurrentAlreadyAsignedEmployeeForBonus == null || CurrentAlreadyAsignedEmployeeForBonus.Employee_Bonus_id == 0)
            {
                clsMessages.setMessage("Please Select An Employee From 'Already Asigned Employees To Selected Period'...");
                return false;
            }
            else if (CurrentAlreadyAsignedEmployeeForBonus.Is_Processed == true)
            {
                clsMessages.setMessage("Sorry You Cant Delete Already Processed Bonus For Employees...");
                return false;
            }
            else
                return true;
        }
        private void FilterBonusDetailsByPeriod()
        {
            AlreadyAsignedEmployeesForBonus = AllAlreadyAsignedEmployeesForBonus.Where(c => c.Bonus_Period_id == CurrentBonusPeriod.Bonus_Period_id);
        }
        private void DisableTextFieldsAndRadioBttonsOnUpdate()
        {
            UpdateFalse = false;
           if(CurrentAlreadyAsignedEmployeeForBonus.Is_Processed == false)
           {
               if (CurrentAlreadyAsignedEmployeeForBonus.FromStaticAmount == true || CurrentAlreadyAsignedEmployeeForBonus.BonusPercentage == 0)
               {
                   CurrentAlreadyAsignedEmployeeForBonus.FromStaticAmount = true;
                   IsBonusAmountEnabled = true;
                   IsBonusPercentageEnabled = false;
               }
               else if (CurrentAlreadyAsignedEmployeeForBonus.FromBasicSalaryPercentage == true)
               {
                   IsBonusPercentageEnabled = true;
                   IsBonusAmountEnabled = false;
               }
               else
               {
                   IsBonusPercentageEnabled = false;
                   IsBonusAmountEnabled = false;
               }
           }
           else
           {
               IsBonusPercentageEnabled = false;
               IsBonusAmountEnabled = false;
           }
        }
        private void Update()
        {
            if (clsSecurity.GetUpdatePermission(518))
            {
                if (ValidateUpdate())
                {
                    clsMessages.setMessage("Are You Sure You Want to Update Selected Employees Bonus Details?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (CurrentAlreadyAsignedEmployeeForBonus.FromBasicSalaryPercentage == true)
                        {
                            CurrentAlreadyAsignedEmployeeForBonus.BonusAmount = (CurrentAlreadyAsignedEmployeeForBonus.Employee_Basic_Salary / 100) * CurrentAlreadyAsignedEmployeeForBonus.BonusPercentage;
                        }
                        if (serviceClient.UpdateEmployeeBonus(CurrentAlreadyAsignedEmployeeForBonus))
                        {
                            clsMessages.setMessage("Employee's Bonus Amount Updated Successfully...");
                            New();
                        }
                        else
                        {
                            clsMessages.setMessage("Employee's Bonus Amount Update Process Failed...");
                        }
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to update");
        }
        private bool ValidateUpdate()
        {
            if (CurrentAlreadyAsignedEmployeeForBonus == null)
            {
                clsMessages.setMessage("Please Select An Employee Bonus To Update...");
                return false;
            }
            else if (CurrentAlreadyAsignedEmployeeForBonus.Is_Processed == true)
            {
                clsMessages.setMessage("Sorry You Cannot Update This Record Because It Has Been Processed...");
                return false;
            }
            else
                return true;
        }
        private void FilterEmployeeBonusByEmpID()
        {
            AlreadyAsignedEmployeesForBonus = AlreadyAsignedEmployeesForBonus.Where(c => c.emp_id != null && c.emp_id.ToUpper().Contains(Search.ToUpper()));
        }

        #endregion
    }
}
