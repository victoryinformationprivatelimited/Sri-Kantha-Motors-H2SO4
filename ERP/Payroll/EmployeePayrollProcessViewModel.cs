using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using System.Threading;
using ERP.Properties;
using CustomBusyBox;


namespace ERP.Payroll
{
    public class EmployeePayrollProcessViewModel : ViewModelBase
    {

        private ERPServiceClient serviceClient = new ERPServiceClient();
        private ERP.AttendanceService.AttendanceServiceClient attendServiceClient = new ERP.AttendanceService.AttendanceServiceClient();

        #region Tempory Lists
        List<Emp_Sp_Employee> SelectedEmp = new List<Emp_Sp_Employee>();
        List<Emp_Sp_Employee> AllEmp = new List<Emp_Sp_Employee>();
        List<Emp_Sp_Employee> virtyfiedEmployee = new List<Emp_Sp_Employee>();
        List<trns_EmployeeBenifitPeriod> CurrentEmployeeBinifit = new List<trns_EmployeeBenifitPeriod>();
        List<trns_EmployeeDeductionPeriod> CurrentEmployeeDeduction = new List<trns_EmployeeDeductionPeriod>();
        List<trns_EmployeePayment> EmployeePayment = new List<trns_EmployeePayment>();
        List<trns_EmployeePeriodQunatity> EmptyDeductionPeriodQunatity = new List<trns_EmployeePeriodQunatity>();
        List<trns_EmployeePeriodQunatity> EmptyBenifitPeriodQunatity = new List<trns_EmployeePeriodQunatity>();
        List<trns_InternalLoanPayment> LoanRemainingList = new List<trns_InternalLoanPayment>();
        List<EmployeeDeductionPaymentView> AllEmployeeDeductionPayments = new List<EmployeeDeductionPaymentView>();
        string ErrorMessage = "";
        #endregion

        #region Constructor
        public EmployeePayrollProcessViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.PayProcess), clsSecurity.loggedUser.user_id))
            {
                // refreshAllEmployee();
                refreshAllEmployeeFromView();
                refreshCompanyRule();
                refreshAllDepartment();
                refreshPeriods();
                //refreshEmployeeRules();
                refreEmployeefund();
                //refreshPeriodQuantity();
                refreshCompanyVariable();
                refreshEmployeeCompanyVariables();
                refreshSlaps();
                refreshFlatSlap();
                refreshVariableRules();
                refreEmployeeDetails();
                reafreshEmployeeBankBranchView();
                MaxValue = 0;
                Minvalue = 0;
            }
        }
        #endregion

        #region Propertices
        private IEnumerable<Emp_Sp_Employee> _Employees;
        public IEnumerable<Emp_Sp_Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private IEnumerable<Emp_Sp_Employee> _SelectedEmployees;
        public IEnumerable<Emp_Sp_Employee> SelectedEmployees
        {
            get { return _SelectedEmployees; }
            set { _SelectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }

        private IEnumerable<dtl_EmployeeRule> _EmployeeRules;
        public IEnumerable<dtl_EmployeeRule> EmployeeRules
        {
            get { return _EmployeeRules; }
            set { _EmployeeRules = value; }
        }

        private IEnumerable<dtl_Employee> _EmployeeDetails;
        public IEnumerable<dtl_Employee> EmployeeDetails
        {
            get { return _EmployeeDetails; }
            set { _EmployeeDetails = value; }
        }

        private IEnumerable<mas_CompanyRule> _CompanyRule;
        public IEnumerable<mas_CompanyRule> CompanyRule
        {
            get { return _CompanyRule; }
            set { _CompanyRule = value; }
        }

        private Emp_Sp_Employee _CurrentWantAddEmployee;
        public Emp_Sp_Employee CurrentWantAddEmployee
        {
            get { return this._CurrentWantAddEmployee; }
            set { this._CurrentWantAddEmployee = value; OnPropertyChanged("CurrentWantAddEmployee"); }
        }

        private Emp_Sp_Employee _CurrentWantRemoveEmployee;
        public Emp_Sp_Employee CurrentWantRemoveEmployee
        {
            get { return this._CurrentWantRemoveEmployee; }
            set { this._CurrentWantRemoveEmployee = value; OnPropertyChanged("CurrentWantRemoveEmployee"); }
        }

        private IEnumerable<trns_EmployeePayment> _TrnsEmployeePayment;
        public IEnumerable<trns_EmployeePayment> TrnsEmployeePayment
        {
            get { return _TrnsEmployeePayment; }
            set { _TrnsEmployeePayment = value; OnPropertyChanged("TrnsEmployeePayment"); }
        }

        private IEnumerable<trns_EmployeePeriodQunatity> _PeriodQuantity;
        public IEnumerable<trns_EmployeePeriodQunatity> PeriodQuantity
        {
            get { return _PeriodQuantity; }
            set { _PeriodQuantity = value; }
        }

        private IEnumerable<z_CompanyVariable> _CompanyVariables;
        public IEnumerable<z_CompanyVariable> CompanyVariables
        {
            get { return _CompanyVariables; }
            set { _CompanyVariables = value; OnPropertyChanged("CompanyVariables"); }
        }

        private IEnumerable<dtl_EmployeeCompanyVariable> _EmployeeCompanyVariables;
        public IEnumerable<dtl_EmployeeCompanyVariable> EmployeeCompanyVariables
        {
            get { return _EmployeeCompanyVariables; }
            set { _EmployeeCompanyVariables = value; OnPropertyChanged("EmployeeCompanyVariables"); }
        }

        private IEnumerable<z_Slap> _Slaps;
        public IEnumerable<z_Slap> Slaps
        {
            get { return _Slaps; }
            set { _Slaps = value; OnPropertyChanged("Slaps"); }
        }

        private IEnumerable<z_FlatSlap> _FlatSlaps;
        public IEnumerable<z_FlatSlap> FlatSlaps
        {
            get { return _FlatSlaps; }
            set { _FlatSlaps = value; OnPropertyChanged("FlatSlaps"); }
        }

        private Emp_Sp_Employee _Selectforvirtify;
        public Emp_Sp_Employee Selectforvirtify
        {
            get { return _Selectforvirtify; }
            set { _Selectforvirtify = value; OnPropertyChanged("Selectforvirtify"); refreshForVirtify(); }
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

        private z_EmployeesFund _EmployeeFund;
        public z_EmployeesFund EmployeeFund
        {
            get { return _EmployeeFund; }
            set { _EmployeeFund = value; }
        }

        private IEnumerable<EmployeeQuantityVertify> _EmployeeQuanitys;
        public IEnumerable<EmployeeQuantityVertify> EmployeeQuanitys
        {
            get { return _EmployeeQuanitys; }
            set { _EmployeeQuanitys = value; OnPropertyChanged("EmployeeQuanitys"); }
        }

        private IEnumerable<dtl_companyVariableCompanyRules> _VariableRules;
        public IEnumerable<dtl_companyVariableCompanyRules> VariableRules
        {
            get { return _VariableRules; }
            set { _VariableRules = value; OnPropertyChanged("VariableRules"); }
        }

        public IEnumerable<dtl_EmployeeBasicSalary> _EmployeeBasicSalary;
        public IEnumerable<dtl_EmployeeBasicSalary> EmployeeBasicSalary
        {
            get { return _EmployeeBasicSalary; }
            set { _EmployeeBasicSalary = value; }
        }

        private IEnumerable<trns_InternalLoanPayment> _LoanRemaining;

        public IEnumerable<trns_InternalLoanPayment> LoanRemaining
        {
            get { return _LoanRemaining; }
            set { _LoanRemaining = value; OnPropertyChanged("LoanRemaining"); }
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

        private IEnumerable<EmployeeDeductionPaymentView> _EmployeeDeductionPayments;

        public IEnumerable<EmployeeDeductionPaymentView> EmployeeDeductionPayments
        {
            get { return _EmployeeDeductionPayments; }
            set { _EmployeeDeductionPayments = value; OnPropertyChanged("EmployeeDeductionPayments"); }
        }
        private IEnumerable<EmployeeBankBranchView> _EmployeeBankBranch;

        public IEnumerable<EmployeeBankBranchView> EmployeeBankBranch
        {
            get { return this._EmployeeBankBranch; }
            set { this._EmployeeBankBranch = value; this.OnPropertyChanged("EmployeeBankBranch"); }
        }
        #endregion

        #region Props for notify
        private bool _Vertify;
        public bool Vertify
        {
            get { return _Vertify; }
            set { _Vertify = value; OnPropertyChanged("Vertify"); vertifySingle(); }
        }

        private bool _Vertifyall;
        public bool Vertifyall
        {
            get { return _Vertifyall; }
            set { _Vertifyall = value; OnPropertyChanged("Vertifyall"); vertifyAll(); }
        }

        private bool _BasicSalary;
        public bool BasicSalary
        {
            get { return _BasicSalary; }
            set { _BasicSalary = value; OnPropertyChanged("BasicSalary"); }
        }

        private bool _Deductions;
        public bool Deductions
        {
            get { return _Deductions; }
            set { _Deductions = value; OnPropertyChanged("Deductions"); }
        }

        private bool _Benifits;
        public bool Benifits
        {
            get { return _Benifits; }
            set { _Benifits = value; OnPropertyChanged("Benifits"); }
        }

        private bool _Vertifiy;
        public bool Vertifiy
        {
            get { return _Vertifiy; }
            set { _Vertifiy = value; OnPropertyChanged("Vertifiy"); }
        }

        private bool _Finalizing;
        public bool Finalizing
        {
            get { return _Finalizing; }
            set { _Finalizing = value; OnPropertyChanged("Finalizing"); }
        }

        private string _EmployeeName;
        public string EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; OnPropertyChanged("EmployeeName"); }
        }

        private int _MaxValue;
        public int MaxValue
        {
            get { return _MaxValue; }
            set { _MaxValue = value; OnPropertyChanged("MaxValue"); }
        }

        private int _MinValue;
        public int Minvalue
        {
            get { return _MinValue; }
            set { _MinValue = value; OnPropertyChanged("Minvalue"); }
        }

        private int _Progress;
        public int Progress
        {
            get { return _Progress; }
            set { _Progress = value; OnPropertyChanged("Progress"); }
        }
        #endregion

        #region Employee Selection Page Buttons

        public ICommand ProcessBtn
        {
            get { return new RelayCommand(Process, nextCanExecute); }
        }

        //Implement Process
        private void Process()
        {
            //throw new NotImplementedException();
            startPayrollcalculations();
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
            SelectedEmployees = null;
            foreach (Emp_Sp_Employee emp in SelectedEmp)
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
            Employees = null;
            SelectedEmployees = null;
            foreach (Emp_Sp_Employee item in AllEmp)
            {
                SelectedEmp.Add(item);
            }
            AllEmp.Clear();
            this.Employees = AllEmp;
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
                foreach (Emp_Sp_Employee item in SelectedEmployeesWantedToDelete)
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
                foreach (Emp_Sp_Employee item in SelectedEmployeesWantedToAdd)
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

        private bool nextCanExecute()
        {
            if (this.SelectedEmployees == null)
                return false;
            if (this.CurrentPeriod == null)
                return false;
            else
                return true;
        }

        #endregion

        #region Final Process Buttons
        public ICommand StartProcess
        {
            get
            {
                return new RelayCommand(startPayrollcalculations, startCanExecute);
            }
        }

        public ICommand StopProcess
        {
            get
            {
                return new RelayCommand(stopPropcess, stopCanExecute);
            }
        }

        private bool stopCanExecute()
        {
            return true;
        }

        private void stopPropcess()
        {

        }

        private bool startCanExecute()
        {
            if (virtyfiedEmployee.Count > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region Refresh Methods
        //private void refreshAllEmployee()
        //{
        //    this.serviceClient.GetEmployeesCompleted += (s, e) =>
        //        {
        //            this.Employees = e.Result;
        //            foreach (Emp_Sp_Employee item in Employees)
        //            {
        //                AllEmp.Add(item);
        //            }
        //        };
        //    this.serviceClient.GetEmployeesAsync();
        //}
        private void refreshAllEmployeeFromView()
        {
            this.serviceClient.GetMasEmployeeFromViewCompleted += (s, e) =>
            {
                this.Employees = e.Result;
                foreach (Emp_Sp_Employee item in Employees)
                {
                    AllEmp.Add(item);
                }
            };
            this.serviceClient.GetMasEmployeeFromViewAsync();
        }
        private void refreshPeriods()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                this.Periods = e.Result.Where(z => z.isdelete == false && z.is_proceed == false);
            };
            this.serviceClient.GetPeriodsAsync();
        }

        private void refreshForVirtify()
        {
            this.serviceClient.GetEmployeeQuantityForVertifyCompleted += (s, e) =>
            {
                this.EmployeeQuanitys = e.Result;
            };
            this.serviceClient.GetEmployeeQuantityForVertifyAsync(Selectforvirtify.employee_id, CurrentPeriod.period_id);
        }

        private void refreshEmployeeRules()
        {
            //this.serviceClient.GetEmployeeRuleCompleted += (s, e) =>
            //{
            this.EmployeeRules = serviceClient.GetEmployeeRule().Where(a => a.isactive == true);
            //};
            //this.serviceClient.GetEmployeeRuleAsync();
        }

        private void refreEmployeeDetails()
        {
            //this.serviceClient.GetEmployeeDetailsCompleted += (s, e) =>
            //{
            //    this.EmployeeDetails = e.Result.Where(z => z.isdelete == false && z.isActive == true);
            //    if (EmployeeDetails != null)
            //        refreshAllDepartment();
            //};
            //this.serviceClient.GetEmployeeDetailsAsync();
            //this.serviceClient.GetEmployeeDetailsCompleted += (s, e) =>
            //{
            this.EmployeeDetails = serviceClient.GetEmployeeDetails();
            //};
            //this.serviceClient.GetEmployeeDetailsAsync();
        }

        private void refreEmployeefund()
        {

        }

        private void refreshCompanyRule()
        {
            this.serviceClient.GetCompanyRulesCompleted += (s, e) =>
            {
                this.CompanyRule = e.Result;
            };
            this.serviceClient.GetCompanyRulesAsync();
        }

        private void refreshCompanyVariable()
        {
            this.serviceClient.GetCompanyVariablesCompleted += (s, e) =>
                {
                    CompanyVariables = e.Result;
                };
            this.serviceClient.GetCompanyVariablesAsync();
        }

        private void refreshEmployeeCompanyVariables()
        {
            this.serviceClient.GetdtlEmployeeCompanyVariablesCompleted += (s, e) =>
                {
                    this.EmployeeCompanyVariables = e.Result;
                };
            this.serviceClient.GetdtlEmployeeCompanyVariablesAsync();
        }

        private void refreshPeriodQuantity()
        {
            //this.serviceClient.GetAllTrnsPeriodQuantityCompleted += (s, e) =>
            //{
            this.PeriodQuantity = serviceClient.GetAllTrnsPeriodQuantity(CurrentPeriod.period_id);
            //};
            //this.serviceClient.GetAllTrnsPeriodQuantityAsync();
        }

        private void refreshSlaps()
        {
            this.serviceClient.GetSalpsCompleted += (s, e) =>
                {
                    this.Slaps = e.Result;
                };
            this.serviceClient.GetSalpsAsync();
        }

        private void refreshFlatSlap()
        {
            this.serviceClient.GetFlatSlapsCompleted += (s, e) =>
                {
                    this.FlatSlaps = e.Result;
                };
            this.serviceClient.GetFlatSlapsAsync();
        }

        private void refreshVariableRules()
        {
            this.serviceClient.GetAllCompanyVariableRulesCompleted += (s, e) =>
            {
                VariableRules = e.Result;
            };
            this.serviceClient.GetAllCompanyVariableRulesAsync();
        }

        public void refreEmployeeBasicSalary()
        {
            this.EmployeeBasicSalary = serviceClient.GetEmployeeBasicSalary();
        }

        private void refreshLoanRemaining()
        {

            LoanRemaining = serviceClient.GetLoanRemainingAmount(CurrentPeriod.period_id);
            if (LoanRemaining != null && LoanRemaining.Count() > 0)
                LoanRemainingList = LoanRemaining.ToList();
            //this.serviceClient.GetLoanRemainingAmountCompleted += (s, e) =>
            //{
            //    LoanRemaining = e.Result;
            //    if (LoanRemaining != null && LoanRemaining.Count() > 0)
            //        LoanRemainingList = LoanRemaining.ToList();

            //};
            //this.serviceClient.GetLoanRemainingAmountAsync(CurrentPeriod.period_id);
        }

        public void RefreshEmployeeDeductionPayment()
        {
            EmployeeDeductionPayments = serviceClient.GetEmployeeDeductionPayments();
            if (EmployeeDeductionPayments != null)
                AllEmployeeDeductionPayments = EmployeeDeductionPayments.ToList();
        }

        private void reafreshEmployeeBankBranchView()
        {
            EmployeeBankBranch = serviceClient.GetEmployeeBankBranchView();
            //this.serviceClient.GetEmployeeBankBranchViewCompleted += (s, e) =>
            //{
            //    this.EmployeeBankBranch = e.Result.OrderBy(c => c.emp_id);
            //};
            //this.serviceClient.GetEmployeeBankBranchViewAsync();
        }

        #endregion

        #region Vertify Methods
        private void vertifySingle()
        {
            foreach (EmployeeQuantityVertify item in EmployeeQuanitys)
            {
                item.Vertify = Vertify;
            }

            if (Vertify)
            {
                virtyfiedEmployee.Add(Selectforvirtify);
            }
        }

        private void vertifyAll()
        {
            if (Vertifyall)
            {
                virtyfiedEmployee.Clear();
                virtyfiedEmployee = SelectedEmployees.ToList();

            }
            else
            {
                if (virtyfiedEmployee.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to unvertify all employees", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        virtyfiedEmployee.Clear();
                    }
                }
            }
        }
        #endregion


        #region Payroll Process

        private void startPayrollcalculations()
        {
            if (clsSecurity.GetSavePermission(507))
            {
                MessageBoxResult result = MessageBox.Show("Do you want to Start Payroll Process ", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    BusyBox.ShowBusy("Please Wait Until Payroll Process Completed");
                    refreEmployeeDetails();
                    refreshEmployeeRules();
                    refreshPeriodQuantity();
                    refreEmployeeBasicSalary();
                    refreshLoanRemaining();
                    RefreshEmployeeDeductionPayment();
                    try
                    {
                        virtyfiedEmployee = SelectedEmployees.ToList();
                        if (virtyfiedEmployee.Count > 0)
                        {
                            MaxValue = virtyfiedEmployee.Count;
                            foreach (Emp_Sp_Employee item in virtyfiedEmployee)
                            {
                                BasicSalary = false;
                                Benifits = false;
                                Deductions = false;
                                Vertifiy = false;
                                Finalizing = false;

                                EmployeeName = item.emp_id + " " + item.initials + " " + item.first_name + " " + item.second_name;

                                BasicSalary = true;
                                decimal basicSalary = 0;
                                decimal payeeVal = 0;
                                decimal CalculationSalaryWithCompanyRules = basicSalary;

                                // m 2020-08-12
                                decimal nopayamount = 0;
                                decimal nopayquantity = 0;
                                decimal lateamount = 0;
                                decimal latequantity = 0;

                                if (item != null)
                                {

                                    var CurrEmp = EmployeeDetails.First(e => e.employee_id == item.employee_id);
                                    basicSalary = (decimal)CurrEmp.basic_salary;

                                    #region MCN change ,
                                    /*
                                    First need ALTER TABLE dbo.trns_EmployeePayment ADD basic_Salary_without_BR decimal(18, 6) 
                                 * Get Rule Id in z_EmployeeBasicRuleName table
                                * */

                                    //var CurEmpBasicSalary = basicSalary;
                                    //var CurEmpBR1 = 0;
                                    //var CurEmpBR2 = 0;
                                    //var CurEmpBR3 = 0;
                                    var CurEmpBasicSalary = EmployeeBasicSalary.FirstOrDefault(e => e.employee_id == item.employee_id && e.assigningRuleId == Guid.Parse("00000000-0000-0000-0000-000000000000"));
                                    var CurEmpBR1 = EmployeeBasicSalary.FirstOrDefault(e => e.employee_id == item.employee_id && e.assigningRuleId == Guid.Parse("e1255054-42fc-4b95-b38a-63ce8bfa20a8"));
                                    var CurEmpBR2 = EmployeeBasicSalary.FirstOrDefault(e => e.employee_id == item.employee_id && e.assigningRuleId == Guid.Parse("c10e58aa-cb79-4778-ba91-ba2171a4ff58"));
                                    var CurEmpBR3 = EmployeeBasicSalary.FirstOrDefault(e => e.employee_id == item.employee_id && e.assigningRuleId == Guid.Parse("32d54c1f-6024-445f-b3bd-ca4992d9bdd6"));

                                    #endregion

                                    if (basicSalary > 0)
                                    {
                                        List<dtl_EmployeeRule> CurrentEmployeeRules = EmployeeRules.Where(e => e.employee_id.Equals(item.employee_id) && e.isactive == true).ToList();

                                        foreach (dtl_EmployeeRule EmpRule in CurrentEmployeeRules)
                                        {
                                            var CurrentCompanyRule = CompanyRule.First(e => e.rule_id.Equals(EmpRule.rule_id));

                                            if (CurrentCompanyRule != null)
                                            {
                                                #region Deduction
                                                if (!CurrentCompanyRule.deduction_id.Equals(new Guid()))
                                                {
                                                    // not entered period quantity
                                                    var CurrentEmployeeRuleQuantity = PeriodQuantity.FirstOrDefault(e => e.employee_id.Equals(item.employee_id) && e.rule_id.Equals(CurrentCompanyRule.rule_id) && e.period_id.Equals(CurrentPeriod.period_id));

                                                    if (CurrentEmployeeRuleQuantity != null)
                                                    {
                                                        trns_EmployeeDeductionPeriod newDeduction = new trns_EmployeeDeductionPeriod();
                                                        newDeduction.employee_id = item.employee_id;
                                                        newDeduction.rule_id = (Guid)CurrentCompanyRule.rule_id;
                                                        newDeduction.period_id = CurrentPeriod.period_id;
                                                        newDeduction.units = CurrentEmployeeRuleQuantity.quantity;

                                                        if (!(bool)EmpRule.is_special)
                                                        {
                                                            newDeduction.amount = CurrentCompanyRule.rate * CurrentEmployeeRuleQuantity.quantity;
                                                            newDeduction.amount_per_unit = CurrentCompanyRule.rate;
                                                        }
                                                        else
                                                        {
                                                            newDeduction.amount = EmpRule.special_amount * CurrentEmployeeRuleQuantity.quantity;
                                                            newDeduction.amount_per_unit = EmpRule.special_amount;
                                                        }

                                                        // H 2021-06-22
                                                        //if (newDeduction.rule_id == new Guid("10000000-0000-0000-0000-000000000001") || newDeduction.rule_id == new Guid("10000000-0000-0000-0000-000000000002"))
                                                        //{
                                                        //    nopayamount = (decimal)newDeduction.amount;
                                                        //    nopayquantity = (decimal)newDeduction.units;
                                                        //    if (CurrentPeriod.nopay_base != null && CurrentPeriod.nopay_base > 0)
                                                        //    {
                                                        //        newDeduction.amount = (basicSalary / CurrentPeriod.nopay_base) * CurrentEmployeeRuleQuantity.quantity;
                                                        //        newDeduction.amount_per_unit = basicSalary / CurrentPeriod.nopay_base;
                                                        //    }
                                                        //}
                                                        //if (newDeduction.rule_id == new Guid("20000000-0000-0000-0000-000000000001") || newDeduction.rule_id == new Guid("20000000-0000-0000-0000-000000000002"))
                                                        //{
                                                        //    lateamount = (decimal)newDeduction.amount;
                                                        //    latequantity = (decimal)newDeduction.units;
                                                        //    if (CurrentPeriod.late_base != null && CurrentPeriod.late_base > 0)
                                                        //    {
                                                        //        newDeduction.amount = (basicSalary / CurrentPeriod.late_base) * CurrentEmployeeRuleQuantity.quantity;
                                                        //        newDeduction.amount_per_unit = basicSalary / CurrentPeriod.late_base;
                                                        //    }
                                                        //}

                                                        #region m 2020-06-18

                                                        //if (newDeduction.rule_id == new Guid("10000000-0000-0000-0000-000000000001") || newDeduction.rule_id == new Guid("10000000-0000-0000-0000-000000000002"))
                                                        //{
                                                        //    trns_EmployeeQuantityPeriod noPayQuan = new trns_EmployeeQuantityPeriod();
                                                        //    noPayQuan.employee_id = item.employee_id;
                                                        //    noPayQuan.period_id = CurrentPeriod.period_id;
                                                        //    noPayQuan.rule_id = new Guid("10000000-0000-0000-1111-000000000001"); //nopay
                                                        //    noPayQuan.amount = newDeduction.units;
                                                        //    CurrentEmployeeQuantities.Add(noPayQuan);

                                                        //    // h 2020-06-19 get nopay n act
                                                        //    if(newDeduction.rule_id == new Guid("10000000-0000-0000-0000-000000000001"))
                                                        //    {
                                                        //        actDays = 30;
                                                        //        nopaydays = Math.Round((decimal)newDeduction.units, 2);
                                                        //    }
                                                        //    if (newDeduction.rule_id == new Guid("10000000-0000-0000-0000-000000000002"))
                                                        //    {
                                                        //        actDays = 26;
                                                        //        nopaydays = Math.Round((decimal)newDeduction.units, 2);
                                                        //    }
                                                        //}

                                                        #endregion

                                                        CurrentEmployeeDeduction.Add(newDeduction);
                                                        Benifits = true; 
                                                    }
                                                }
                                                #endregion

                                                #region Benifit
                                                if (!CurrentCompanyRule.benifit_id.Equals(new Guid()))
                                                {
                                                    //not enterd deduction quntity

                                                    var CurrentEmployeeRuleQuantity = PeriodQuantity.FirstOrDefault(e => e.employee_id.Equals(item.employee_id) && e.rule_id.Equals(CurrentCompanyRule.rule_id) && e.period_id.Equals(CurrentPeriod.period_id));

                                                    if (CurrentEmployeeRuleQuantity != null)
                                                    {
            
                                                        trns_EmployeeBenifitPeriod newBenifit = new trns_EmployeeBenifitPeriod();
                                                        newBenifit.employee_id = item.employee_id;
                                                        newBenifit.rule_id = (Guid)CurrentCompanyRule.rule_id;
                                                        newBenifit.period_id = CurrentPeriod.period_id;
                                                        newBenifit.units = CurrentEmployeeRuleQuantity.quantity;

                                                        if (!(bool)EmpRule.is_special)
                                                        {
                                                            newBenifit.amount = CurrentCompanyRule.rate * CurrentEmployeeRuleQuantity.quantity;
                                                            newBenifit.amount_per_unit = CurrentCompanyRule.rate;
                                                        }
                                                        else
                                                        {
                                                            newBenifit.amount = EmpRule.special_amount * CurrentEmployeeRuleQuantity.quantity;
                                                            newBenifit.amount_per_unit = EmpRule.special_amount;
                                                        }

                                                        #region m 2020-06-18

                                                        //if (newBenifit.rule_id == new Guid("00000000-0000-0000-0000-000000000001") || newBenifit.rule_id == new Guid("00000000-0000-0000-0000-000000000004"))
                                                        //{
                                                        //    trns_EmployeeQuantityPeriod OTQuan = new trns_EmployeeQuantityPeriod();
                                                        //    OTQuan.employee_id = item.employee_id;
                                                        //    OTQuan.period_id = CurrentPeriod.period_id;
                                                        //    OTQuan.rule_id = new Guid("10000000-0000-0000-3333-000000000001"); //ot
                                                        //    OTQuan.amount = newBenifit.units;
                                                        //    CurrentEmployeeQuantities.Add(OTQuan);
                                                        //}
                                                        #endregion

                                                        CurrentEmployeeBinifit.Add(newBenifit);
                                                        Deductions = true;
                                                    }
                                                }
                                                #endregion
                                            }
                                        }                                     
                                    }

                                    #region Add Benifits and Deduction

                                    trns_EmployeePayment trnpay = new trns_EmployeePayment();
                                    trn_EmployeeCompanyVariable empvari = new trn_EmployeeCompanyVariable();
                                    trnpay.employee_id = item.employee_id;
                                    trnpay.basic_salary = CurrEmp.basic_salary;
                                    trnpay.period_id = CurrentPeriod.period_id;
                                    trnpay.total_benifit = 0;
                                    trnpay.total_deduction = 0;
                                    trnpay.total_salary = 0;
                                    trnpay.isPaied = false;
                                    trnpay.payment_method_id = new Guid();
                                    if(item.department_id != null)
                                        trnpay.department_id = item.department_id;
                                    if (item.designation_id != null)
                                        trnpay.designation_id = item.designation_id;
                                    if (item.branch_id != null)
                                        trnpay.companyBranch_id = item.branch_id;
                                    trnpay.nopay_amount = nopayamount;
                                    trnpay.nopay_qty = nopayquantity;
                                    trnpay.late_amount = lateamount;
                                    trnpay.late_qty = latequantity;

                                    decimal SlapTotal = 0;
                                    decimal FlatSlapTotal = 0;

                                    foreach (trns_EmployeeBenifitPeriod Benifititem in CurrentEmployeeBinifit.Where(e => e.employee_id.Equals(item.employee_id)))
                                    {
                                        Benifits = true;
                                        trnpay.total_benifit += Benifititem.amount;
                                    }

                                    foreach (trns_EmployeeDeductionPeriod Deductionitem in CurrentEmployeeDeduction.Where(e => e.employee_id.Equals(item.employee_id)))
                                    {
                                        Deductions = true;
                                        trnpay.total_deduction = trnpay.total_deduction + Deductionitem.amount;
                                    }

                                    Vertifiy = true;

                                    decimal rule_deduction = decimal.Parse(trnpay.total_deduction.ToString());
                                    decimal CalculationSalary = 0;
                                    var new_grossSalary = basicSalary + trnpay.total_benifit;
                                    trnpay.grossSalary = basicSalary + trnpay.total_benifit;

                                    #region MCN

                                    //trnpay.basic_Salary_without_BR = basicSalary;
                                    //trnpay.BR1 = 0;
                                    //trnpay.BR2 = 0;
                                    //trnpay.BR3 = 0;
                                    trnpay.basic_Salary_without_BR = CurEmpBasicSalary == null ? basicSalary : CurEmpBasicSalary.amount;
                                    trnpay.BR1 = CurEmpBR1 == null ? 0 : CurEmpBR1.amount;
                                    trnpay.BR2 = CurEmpBR2 == null ? 0 : CurEmpBR2.amount;
                                    trnpay.BR3 = CurEmpBR3 == null ? 0 : CurEmpBR3.amount;

                                    #endregion

                                    #endregion

                                    #region Comany Variable
                                    // decimal company_rule_deduction_value = 0;
                                    foreach (dtl_EmployeeCompanyVariable Variableitem in EmployeeCompanyVariables.Where(e => e.employee_id.Equals(CurrEmp.employee_id)))
                                    {
                                        if ((bool)Variableitem.isApplicable)
                                        {
                                            foreach (z_CompanyVariable comvariaitem in CompanyVariables.Where(e => e.company_variableID.Equals(Variableitem.company_variableID)))
                                            {
                                                if ((bool)comvariaitem.isCalculateForBasicSalary)
                                                    CalculationSalary = basicSalary;
                                                else if ((bool)comvariaitem.isCalculatewithBasicPlusCompanyRules)
                                                    CalculationSalary = GetCalCulationSalaryForCompanyVariables(comvariaitem.company_variableID, CurrEmp, CurrentEmployeeBinifit, CurrentEmployeeDeduction);
                                                else
                                                    CalculationSalary = (decimal)trnpay.grossSalary;

                                                empvari.employee_id = CurrEmp.employee_id;
                                                empvari.company_variableID = Variableitem.company_variableID;
                                                empvari.period_id = CurrentPeriod.period_id;
                                                empvari.calculate_salary = CalculationSalary;
                                                var epf10 = new Guid("00000000-0000-0000-0000-000000000001");
                                                if (comvariaitem.company_variableID.Equals(epf10))
                                                {
                                                    trnpay.calculate_salary = CalculationSalary;
                                                }

                                                if ((bool)comvariaitem.isSlapCalcution)
                                                {

                                                    SlapTotal = SlapCalculation(CalculationSalary, comvariaitem);

                                                    empvari.value = SlapTotal;

                                                    var id = new Guid("00000000-0000-0000-0000-000000000004");
                                                    if (comvariaitem.company_variableID.Equals(id))
                                                    {
                                                        payeeVal = SlapTotal;
                                                    }
                                                    if ((bool)comvariaitem.Is_Benifit)
                                                        trnpay.total_salary = trnpay.total_salary + (decimal)SlapTotal;

                                                    if ((bool)comvariaitem.Is_Deduct)
                                                    {
                                                        trnpay.total_salary = trnpay.total_salary - (decimal)SlapTotal;
                                                        if (comvariaitem.company_variableID.Equals("00000000-0000-0000-0000-000000000002"))
                                                        {
                                                        }
                                                        else if (comvariaitem.company_variableID.Equals("00000000-0000-0000-0000-000000000003"))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            rule_deduction = rule_deduction + (decimal)SlapTotal;
                                                        }
                                                    }
                                                }
                                                else if ((bool)comvariaitem.isFlatSlapCalculation)
                                                {
                                                    FlatSlapTotal = FlatSlapCalculation(CalculationSalary, comvariaitem);

                                                    empvari.value = FlatSlapTotal;

                                                    if ((bool)comvariaitem.Is_Benifit)
                                                    {
                                                        trnpay.total_salary = trnpay.total_salary + (decimal)FlatSlapTotal;
                                                    }

                                                    if ((bool)comvariaitem.Is_Deduct)
                                                    {
                                                        trnpay.total_salary = trnpay.total_salary - (decimal)FlatSlapTotal;
                                                        if (comvariaitem.company_variableID.Equals("00000000-0000-0000-0000-000000000002"))
                                                        {
                                                        }
                                                        else if (comvariaitem.company_variableID.Equals("00000000-0000-0000-0000-000000000003"))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            rule_deduction = rule_deduction + (decimal)FlatSlapTotal;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if ((bool)comvariaitem.Is_Pracentage)
                                                        empvari.value = CalculationSalary * comvariaitem.variable_Value / 100;

                                                    var id = new Guid("00000000-0000-0000-0000-000000000006");
                                                    if (comvariaitem.company_variableID.Equals(id))
                                                    {
                                                        if (payeeVal != 0)
                                                        {
                                                            empvari.value = payeeVal * comvariaitem.variable_Value / 100;
                                                        }
                                                    }

                                                    if ((bool)comvariaitem.Is_Benifit)
                                                        trnpay.total_salary = trnpay.total_salary + (decimal)empvari.value;

                                                    if ((bool)comvariaitem.Is_Deduct)
                                                    {
                                                        trnpay.total_salary = trnpay.total_salary - (decimal)empvari.value;
                                                        if (comvariaitem.company_variableID.Equals("00000000-0000-0000-0000-000000000002"))
                                                        {
                                                        }
                                                        else if (comvariaitem.company_variableID.Equals("00000000-0000-0000-0000-000000000003"))
                                                        {
                                                        }
                                                        else
                                                        {
                                                            rule_deduction = rule_deduction + (decimal)empvari.value;
                                                        }

                                                    }
                                                }
                                                saveEmployeeCompanyVariables(empvari);
                                            }
                                        }
                                    }

                                    #endregion

                                    //foreach (trns_EmployeeDeductionPeriod Deductionitem in CurrentEmployeeDeduction.Where(e => e.employee_id.Equals(item.employee_id)))
                                    //{
                                    //    Deductions = true;
                                    //    rule_deduction = rule_deduction + decimal.Parse( Deductionitem.amount.ToString());
                                    //    //decimal dt = rule_deduction;
                                    //}
                                    trnpay.total_deduction = rule_deduction;

                                    #region loan
                                    trns_InternalLoanPayment personalbal = LoanRemainingList.FirstOrDefault(c => c.EmployeeID == CurrEmp.employee_id && c.period_id == CurrentPeriod.period_id && c.loan_id == new Guid("D436CD40-92F4-4CFD-82F3-0BF4B930A33F"));
                                    if (personalbal != null)
                                    {
                                        trnpay.personal_loan_balance = personalbal.Loan_RemainingAmount;
                                        trnpay.personal_loan_amount = personalbal.PaidAmount_WithoutIntrest;
                                        trnpay.personal_loan_interest = personalbal.Paid_IntrestAmount;
                                        trnpay.total_deduction = trnpay.total_deduction + trnpay.personal_loan_amount + trnpay.personal_loan_interest;
                                    }
                                    else
                                    {
                                        trnpay.personal_loan_balance = 0;
                                        trnpay.personal_loan_amount = 0;
                                        trnpay.personal_loan_interest = 0;
                                    }

                                    trns_InternalLoanPayment festivalbal = LoanRemainingList.FirstOrDefault(c => c.EmployeeID == CurrEmp.employee_id && c.period_id == CurrentPeriod.period_id && (c.loan_id == new Guid("9BD8CB72-FE08-4C0B-A6C4-C3BBD63783DF")));// || c.loan_id == new Guid("E322D8C7-6812-4BC8-94BD-8E1E6A88093C")));
                                    if (festivalbal != null)
                                    {
                                        trnpay.festival_loan_balance = festivalbal.Loan_RemainingAmount;
                                        trnpay.festival_loan_amount = festivalbal.PaidAmount_WithoutIntrest;
                                        trnpay.festival_loan_interest = festivalbal.Paid_IntrestAmount;
                                        trnpay.total_deduction = trnpay.total_deduction + trnpay.festival_loan_amount + trnpay.festival_loan_interest;
                                    }
                                    else
                                    {
                                        trnpay.festival_loan_balance = 0;
                                        trnpay.festival_loan_amount = 0;
                                        trnpay.festival_loan_interest = 0;
                                    }

                                    trns_InternalLoanPayment bicyclebal = LoanRemainingList.FirstOrDefault(c => c.EmployeeID == CurrEmp.employee_id && c.period_id == CurrentPeriod.period_id && c.loan_id == new Guid("1828F76F-0925-4649-9A57-A3EC362773DA"));
                                    if (bicyclebal != null)
                                    {
                                        trnpay.bicycle_loan_balance = bicyclebal.Loan_RemainingAmount;
                                        trnpay.bicyclel_loan_amount = bicyclebal.PaidAmount_WithoutIntrest;
                                        trnpay.bicyclel_loan_interest = bicyclebal.Paid_IntrestAmount;
                                        trnpay.total_deduction = trnpay.total_deduction + trnpay.bicyclel_loan_amount + trnpay.bicyclel_loan_interest;
                                    }
                                    else
                                    {
                                        trnpay.bicycle_loan_balance = 0;
                                        trnpay.bicyclel_loan_amount = 0;
                                        trnpay.bicyclel_loan_interest = 0;
                                    }

                                    trns_InternalLoanPayment floodbal = LoanRemainingList.FirstOrDefault(c => c.EmployeeID == CurrEmp.employee_id && c.period_id == CurrentPeriod.period_id && c.loan_id == new Guid("CF87D36A-72D2-4A0E-BFF6-8EAAC7220F43"));
                                    if (floodbal != null)
                                    {
                                        trnpay.flood_loan_balance = floodbal.Loan_RemainingAmount;
                                        trnpay.flood_loan_amount = floodbal.PaidAmount_WithoutIntrest;
                                        trnpay.floodl_loan_interest = floodbal.Paid_IntrestAmount;
                                        trnpay.total_deduction = trnpay.total_deduction + trnpay.flood_loan_amount + trnpay.floodl_loan_interest;
                                    }
                                    else
                                    {
                                        trnpay.flood_loan_balance = 0;
                                        trnpay.flood_loan_amount = 0;
                                        trnpay.floodl_loan_interest = 0;
                                    }

                                    trns_InternalLoanPayment guarantorbal = LoanRemainingList.FirstOrDefault(c => c.EmployeeID == CurrEmp.employee_id && c.period_id == CurrentPeriod.period_id && c.loan_id == new Guid("3115C8DD-A739-4615-97F2-AE08AA2F2E09"));
                                    if (guarantorbal != null)
                                    {
                                        trnpay.guarantor_loan_balance = guarantorbal.Loan_RemainingAmount;
                                        trnpay.guarantor_loan_amount = guarantorbal.PaidAmount_WithoutIntrest;
                                        trnpay.guarantor_loan_interest = guarantorbal.Paid_IntrestAmount;
                                        trnpay.total_deduction = trnpay.total_deduction + trnpay.guarantor_loan_amount + trnpay.guarantor_loan_interest;
                                    }
                                    else
                                    {
                                        trnpay.guarantor_loan_balance = 0;
                                        trnpay.guarantor_loan_amount = 0;
                                        trnpay.guarantor_loan_interest = 0;
                                    } 
                                    #endregion

                                    trnpay.grossSalary = trnpay.grossSalary - trnpay.total_deduction;
                                    trnpay.total_salary = Math.Round((decimal)trnpay.grossSalary,2);
                                    trnpay.grossSalary = new_grossSalary;

                                    // h 2020-10-10 edit gross
                                    trnpay.grossSalary = (trnpay.total_benifit == null ? 0 : trnpay.total_benifit) + (trnpay.calculate_salary == null ? 0 : trnpay.calculate_salary);                                   

                                    EmployeePayment.Add(trnpay);



                                    TrnsEmployeePayment = null;
                                    TrnsEmployeePayment = EmployeePayment;

                                }
                                Finalizing = true;
                                Progress++;
                            }

                            #region Save Employee Deduction Payments
                            decimal tot = 0;
                            foreach(var EDP in EmployeeDeductionPayments)
                            {
                                trns_EmployeeDeductionPayment TEDP = new trns_EmployeeDeductionPayment();
                                foreach (Emp_Sp_Employee emp in virtyfiedEmployee)
                                {
                                    foreach (var rule in EmployeeRules.Where(c => c.rule_id == EDP.deduction_rule_id && c.employee_id == emp.employee_id))
                                    {
                                        tot = tot + Convert.ToDecimal(rule.special_amount);
                                    } 
                                }
                                TEDP.trns_payment_id = Guid.NewGuid();
                                TEDP.rule_id = EDP.deduction_rule_id;
                                TEDP.period_id = CurrentPeriod.period_id;
                                TEDP.total_amount = tot;
                                TEDP.save_user_id = clsSecurity.loggedUser.user_id;
                                TEDP.save_datetime = DateTime.Now;
                                tot = 0;
                                saveEmployeeDeductionPayments(TEDP);
                            }
                            #endregion

                            #region Save Trnasactions
                            saveEmployeeBenifits();
                            saveEmployeeDeductions();
                            saveEmployeePayments();
                            // m 2020-06-18
                            //saveEmployeeQuantities();
                            #endregion
                            BusyBox.CloseBusy();
                            MessageBox.Show("Payroll process complete..", "ERP Says..", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception)
                    {
                        BusyBox.CloseBusy();
                        MessageBoxResult exresult = MessageBox.Show("Payroll Process Start With  Errors.. Please Check the Pay Period ! Do you need to Continue ?", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (exresult == MessageBoxResult.Yes)
                        {
                            this.errorFix();
                            FillErrorMessage();
                            // EmptyDeductionPeriodQunatity.GroupBy(z => z.employee_id);
                            MessageBox.Show("Check  Monthly Quntity in this Employees " + "\n" + ErrorMessage);


                        }
                    }
                }
            }
            else
                clsMessages.setMessage("You don't have permission to process");
        }

        #endregion

        #region Saving Methods
        private void saveEmployeePayments()
        {
            this.serviceClient.SaveEmployeeTrnPayment(EmployeePayment.ToArray<trns_EmployeePayment>());
        }

        private void saveEmployeeDeductions()
        {
            this.serviceClient.SaveEmployeeTrnDeductions(CurrentEmployeeDeduction.ToArray<trns_EmployeeDeductionPeriod>());
        }

        private void saveEmployeeBenifits()
        {
            this.serviceClient.SaveEmployeeTrnBenifits(CurrentEmployeeBinifit.ToArray<trns_EmployeeBenifitPeriod>());
        }

        private void saveEmployeeCompanyVariables(trn_EmployeeCompanyVariable EmployeeCompanyVariable)
        {
            this.serviceClient.SavetrnsEmployeeVariable(EmployeeCompanyVariable);
        }

        private void saveEmployeeDeductionPayments(trns_EmployeeDeductionPayment TrnsEDP)
        {
            this.serviceClient.SaveEmployeeDeductionPayments(TrnsEDP);
        }

        //private void saveEmployeeQuantities()
        //{
        //    this.serviceClient.SaveEmployeeTrnQuantities(CurrentEmployeeQuantities.ToArray<trns_EmployeeQuantityPeriod>());
        //}
        #endregion

        #region Salp Calculation
        private decimal SlapCalculation(decimal calculationSalary, z_CompanyVariable CurrentCompanyVariable)
        {
            decimal SlapTotal = 0;

            foreach (z_Slap SlapItem in Slaps.Where(e => e.company_variableID.Equals(CurrentCompanyVariable.company_variableID)))
            {
                decimal Slapvalue = 0;

                if (calculationSalary >= SlapItem.slapStart && calculationSalary < SlapItem.slapEnd)
                {
                    Slapvalue = calculationSalary * (decimal)SlapItem.slapValue / 100;
                    Slapvalue = Slapvalue - (decimal)SlapItem.deductValue;
                    SlapTotal += Slapvalue;
                }
            }
            return SlapTotal;
        }

        private decimal FlatSlapCalculation(decimal calculationSalary, z_CompanyVariable CurrentCompanyVariable)
        {
            decimal SlapTotal = 0;

            foreach (z_FlatSlap FlatSlapItem in FlatSlaps.Where(e => e.company_variableID.Equals(CurrentCompanyVariable.company_variableID)))
            {
                decimal Slapvalue = 0;

                if (calculationSalary >= FlatSlapItem.flatSlapStart && calculationSalary < FlatSlapItem.flatSlapEnd)
                {
                    Slapvalue = (decimal)FlatSlapItem.flatslapValue;
                    SlapTotal += Slapvalue;
                }
            }
            return SlapTotal;
        }
        #endregion

        #region Clear Payroll Process

        public ICommand ClearPayroll
        {
            get
            {
                return new RelayCommand(ClearPayrollProcess, ClearPayrollCanExecute);
            }
        }

        private void ClearPayrollProcess()
        {
            if (clsSecurity.GetDeletePermission(507))
            {
                MessageBoxResult result = MessageBox.Show("Do you want to Delete Payroll Process ", "ERP asking", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    BusyBox.ShowBusy("Please wait until payroll is clearing.");
                    if (serviceClient.DeletePayrollProcess(CurrentPeriod.period_id, SelectedEmployees.Select(c => c.employee_id).ToArray()))
                    {
                        BusyBox.CloseBusy();
                        MessageBox.Show(" Payroll Process Delete Successfully ");
                    }
                    else
                    {
                        BusyBox.CloseBusy();
                        MessageBox.Show(" Payroll Process Delete Fail ");
                    }
                }
            }
            else
                clsMessages.setMessage("You don't have permission to clear this record(s)");
        }

        private bool ClearPayrollCanExecute()
        {
            if (CurrentPeriod == null || SelectedEmployees == null || SelectedEmployees.Count() <= 0)
                return false;
            else
                return true;
        }

        /*private bool ClearPayrollCanExecute()
        {
            if (CurrentPeriod == null)
                return false;
            else
                return true;
        }*/

        #endregion

        private decimal GetCalculationSalaryForRules(dtl_Employee CurrentEmployee)
        {
            decimal CalcSalary = (decimal)CurrentEmployee.basic_salary;

            foreach (trns_EmployeeDeductionPeriod DeductItem in CurrentEmployeeDeduction.Where(e => e.employee_id.Equals(CurrentEmployee.employee_id)))
            {
                foreach (mas_CompanyRule comtuleItem in CompanyRule.Where(z => z.rule_id.Equals(DeductItem.rule_id) && z.isEffecToTheCompanyVariable == true))
                {

                    CalcSalary -= (decimal)DeductItem.amount;
                }
            }

            foreach (trns_EmployeeBenifitPeriod BenifitItem in CurrentEmployeeBinifit.Where(e => e.employee_id.Equals(CurrentEmployee.employee_id)))
            {
                foreach (mas_CompanyRule comtuleItem in CompanyRule.Where(z => z.rule_id.Equals(BenifitItem.rule_id) && z.isEffecToTheCompanyVariable == true))
                {

                    CalcSalary += (decimal)BenifitItem.amount;
                }
            }

            return CalcSalary;
        }

        private decimal GetCalCulationSalaryForCompanyVariables(Guid Variable, dtl_Employee CurrentEmployee, List<trns_EmployeeBenifitPeriod> TrnsBenifits, List<trns_EmployeeDeductionPeriod> TrnsDeductions)
        {
            decimal calculationSalary = (decimal)CurrentEmployee.basic_salary;

            if (Variable != null && !Variable.Equals(Guid.Empty) && !Variable.Equals(new Guid()))
            {
                foreach (dtl_companyVariableCompanyRules item in VariableRules.Where(e => e.company_varible_id.Equals(Variable)))
                {
                    foreach (trns_EmployeeBenifitPeriod BenifitItem in TrnsBenifits.Where(e => e.employee_id.Equals(CurrentEmployee.employee_id) && e.rule_id.Equals(item.company_Rule_id)))
                    {
                        calculationSalary += (decimal)BenifitItem.amount;
                    }

                    foreach (trns_EmployeeDeductionPeriod DeductionItem in TrnsDeductions.Where(e => e.employee_id.Equals(CurrentEmployee.employee_id) && e.rule_id.Equals(item.company_Rule_id)))
                    {
                        calculationSalary -= (decimal)DeductionItem.amount;
                    }
                }
            }
            return calculationSalary;
        }

        public void errorFix()
        {

            if (virtyfiedEmployee != null && CurrentPeriod != null)
            {
                foreach (var emp in virtyfiedEmployee)
                {
                    List<dtl_EmployeeRule> fixEmployeeRule = new List<dtl_EmployeeRule>();
                    fixEmployeeRule = EmployeeRules.Where(z => z.employee_id == emp.employee_id).ToList();
                    this.ValidationRule(fixEmployeeRule, emp.employee_id);

                }


            }

        }
        public void ValidationRule(List<dtl_EmployeeRule> RuleListPerEmployee, Guid EmpID)
        {
            foreach (var Rule in RuleListPerEmployee)
            {
                trns_EmployeePeriodQunatity EmpPeriodQunatity = new trns_EmployeePeriodQunatity();
                EmpPeriodQunatity = PeriodQuantity.FirstOrDefault(z => z.rule_id == Rule.rule_id && z.period_id == CurrentPeriod.period_id && z.employee_id == EmpID);
                if (EmpPeriodQunatity == null)
                {
                    trns_EmployeePeriodQunatity epq = new trns_EmployeePeriodQunatity();
                    epq.employee_id = EmpID;
                    epq.rule_id = Rule.rule_id;
                    epq.period_id = CurrentPeriod.period_id;
                    epq.quantity = 0;
                    epq.is_proceed = false;
                    epq.save_user_id = clsSecurity.loggedUser.user_id;
                    epq.save_datetime = System.DateTime.Now;
                    epq.modified_user_id = clsSecurity.loggedUser.user_id;
                    epq.modified_datetime = System.DateTime.Now;
                    epq.delete_user_id = clsSecurity.loggedUser.user_id;
                    epq.delete_datetime = System.DateTime.Now;
                    EmptyDeductionPeriodQunatity.Add(epq);
                }

            }
            RuleListPerEmployee.Clear();

        }
        public void FillErrorMessage()
        {
            foreach (var em in virtyfiedEmployee)
            {
                trns_EmployeePeriodQunatity pqt = new trns_EmployeePeriodQunatity();
                pqt = EmptyDeductionPeriodQunatity.FirstOrDefault(z => z.employee_id == em.employee_id);
                if (pqt != null)
                {
                    ErrorMessage += em.emp_id + " " + em.initials + " " + em.first_name + " " + em.second_name + "\n";
                }

            }
        }

        private IEnumerable<z_Department> _Department;
        public IEnumerable<z_Department> Department
        {
            get { return _Department; }
            set { _Department = value; this.OnPropertyChanged("Department"); }
        }

        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }

            set
            {
                _CurrentDepartment = value; this.OnPropertyChanged("CurrentDepartment");
                if (CurrentDepartment != null)
                    FilterEmployeesDepartmentWise();
            }
        }

        private void refreshAllDepartment()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Department = e.Result;
            };
            this.serviceClient.GetDepartmentsAsync();
        }

        private void FilterEmployeesDepartmentWise()
        {
            List<dtl_Employee> TempList = EmployeeDetails.Where(c => c.department_id == CurrentDepartment.department_id).ToList();
            List<Emp_Sp_Employee> result = AllEmp.Where(c => TempList.Any(d => d.employee_id == c.employee_id)).ToList();
            Employees = null;
            Employees = result;
        }
        //{

        //}
        //    try
        //    {
        //        if (CurrentDepartment != null && AllEmp.Count() > 0)
        //        {
        //            List<Emp_Sp_Employee> Temp = new List<Emp_Sp_Employee>();
        //            ////EmployeeDetails
        //            foreach (var item in EmployeeDetails)
        //            {
        //                if (item.department_id != null && CurrentDepartment != null)
        //                {
        //                    if (item.department_id == CurrentDepartment.department_id)
        //                        Temp.Add(AllEmp.Where(z => z.employee_id == item.employee_id).FirstOrDefault());
        //                }
        //            }
        //            Employees = null;
        //            Employees = Temp;

        //        }
        //    }
        //    catch (Exception)
        //    {


        //    }
        //}
    }
}
