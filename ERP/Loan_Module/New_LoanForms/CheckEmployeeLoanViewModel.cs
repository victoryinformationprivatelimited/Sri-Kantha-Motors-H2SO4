using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ERP.Loan_Module.Reports;

namespace ERP.Loan_Module.New_LoanForms
{
    class CheckEmployeeLoanViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<LoanApprovedView> AllLoanApprovedView;
        List<rpt_EPFEReturnView> AllSalaryCheckForLoan;
        List<EmployeeSumarryView> AllEmployee;
        List<InternalLoanWithoutGurantorsView> AllActiveLoans;
        List<InternalLoanMainView> AllGuranteedLoans;
        #endregion

        #region Constrouctor
        public CheckEmployeeLoanViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();

        }

        #endregion

        #region Properties

        private IEnumerable<LoanApprovedView> _ApprovedLoan;
        public IEnumerable<LoanApprovedView> ApprovedLoan
        {
            get { return _ApprovedLoan; }
            set { _ApprovedLoan = value; OnPropertyChanged("ApprovedLoan"); }
        }

        private LoanApprovedView _CurrentApprovedLoan;
        public LoanApprovedView CurrentApprovedLoan
        {
            get { return _CurrentApprovedLoan; }
            set { _CurrentApprovedLoan = value; OnPropertyChanged("CurrentApprovedLoan"); if (CurrentApprovedLoan != null) FilterApprovedLoan(); }
        }

        //private int _CMBSelectedIndex;

        //public int CMBSelectedIndex
        //{
        //    get { return _CMBSelectedIndex; }
        //    set { _CMBSelectedIndex = value; OnPropertyChanged("CMBSelectedIndex"); }
        //}

        //private string _Search;

        //public string Search
        //{
        //    get { return _Search; }
        //    set
        //    {
        //        _Search = value;
        //        OnPropertyChanged("Search");
        //        if (Search != null && CMBSelectedIndex == 0)
        //            searchTextChangedByEPF();
        //        else if (Search != null && CMBSelectedIndex == 1)
        //            searchTextChangedByNIC();
        //    }
        //}

        private IEnumerable<rpt_EPFEReturnView> _SalaryCheckForLoan;
        public IEnumerable<rpt_EPFEReturnView> SalaryCheckForLoan
        {
            get { return _SalaryCheckForLoan; }
            set { _SalaryCheckForLoan = value; OnPropertyChanged("SalaryCheckForLoan"); }
        }

        private rpt_EPFEReturnView _CurrentSalaryCheckForLoan;
        public rpt_EPFEReturnView CurrentSalaryCheckForLoan
        {
            get { return _CurrentSalaryCheckForLoan; }
            set { _CurrentSalaryCheckForLoan = value; OnPropertyChanged("CurrentSalaryCheckForLoan"); }
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

        private IEnumerable<z_Period> _PeriodForGurantor;
        public IEnumerable<z_Period> PeriodForGurantor
        {
            get { return _PeriodForGurantor; }
            set { _PeriodForGurantor = value; OnPropertyChanged("PeriodForGurantor"); }
        }

        private z_Period _CurrentPeriodForGurantor;
        public z_Period CurrentPeriodForGurantor
        {
            get { return _CurrentPeriodForGurantor; }
            set { _CurrentPeriodForGurantor = value; OnPropertyChanged("CurrentPeriodForGurantor"); }
        }

        private decimal? _ChkLoanPersentage;
        public decimal? ChkLoanPersentage
        {
            get { return _ChkLoanPersentage; }
            set { _ChkLoanPersentage = value; OnPropertyChanged("ChkLoanPersentage"); }
        }

        private IEnumerable<EmployeeSumarryView> _Employee;
        public IEnumerable<EmployeeSumarryView> Employee
        {
            get { return _Employee; }
            set { _Employee = value; OnPropertyChanged("Employee"); }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee");
                if (CurrentEmployee != null)
                    //searchTextChangedByEPF();
                    FilterApprovedLoan();
            }
        }

        private EmployeeSearchView _CurrentEmployeesForDialogBox;
        public EmployeeSearchView CurrentEmployeesForDialogBox
        {
            get { return _CurrentEmployeesForDialogBox; }
            set { _CurrentEmployeesForDialogBox = value; OnPropertyChanged("CurrentEmployeesForDialogBox"); if (CurrentEmployeesForDialogBox != null) { SetEmployeeDetails(); GetActiveLoansOfLoanApplicant(); } }
        }

        private EmployeeSearchView _CurrentGurantorForDialogBox;
        public EmployeeSearchView CurrentGurantorForDialogBox
        {
            get { return _CurrentGurantorForDialogBox; }
            set { _CurrentGurantorForDialogBox = value; OnPropertyChanged("CurrentGurantorForDialogBox"); if (CurrentGurantorForDialogBox != null) { SetGurantorDetails(); GetActiveLoansOfLoanGurantor(); } }
        }

        private IEnumerable<EmployeeSumarryView> _Gurantor;
        public IEnumerable<EmployeeSumarryView> Gurantor
        {
            get { return _Gurantor; }
            set { _Gurantor = value; OnPropertyChanged("Gurantor"); }
        }

        private EmployeeSumarryView _CurrentGurantor;
        public EmployeeSumarryView CurrentGurantor
        {
            get { return _CurrentGurantor; }
            set { _CurrentGurantor = value; OnPropertyChanged("CurrentGurantor"); }
        }

        private IEnumerable<LoanWithRulesView> _Loans;
        public IEnumerable<LoanWithRulesView> Loans
        {
            get { return _Loans; }
            set { _Loans = value; OnPropertyChanged("Loans"); }
        }

        private LoanWithRulesView _CurrentLoan;
        public LoanWithRulesView CurrentLoan
        {
            get { return _CurrentLoan; }
            set { _CurrentLoan = value; OnPropertyChanged("CurrentLoan"); if (CurrentLoan != null)SetEmployeeServicePeriodType(); }
        }

        private IEnumerable<LoanWithRulesView> _GurontorLoan;
        public IEnumerable<LoanWithRulesView> GurontorLoan
        {
            get { return _GurontorLoan; }
            set { _GurontorLoan = value; OnPropertyChanged("GurontorLoan"); }
        }

        private LoanWithRulesView _CurrentGurontorLoan;
        public LoanWithRulesView CurrentGurontorLoan
        {
            get { return _CurrentGurontorLoan; }
            set { _CurrentGurontorLoan = value; OnPropertyChanged("CurrentGurontorLoan"); if (CurrentGurontorLoan != null)SetGurantorServicePeriodType(); }
        }

        private IEnumerable<InternalLoanWithoutGurantorsView> _ActiveLoans;
        public IEnumerable<InternalLoanWithoutGurantorsView> ActiveLoans
        {
            get { return _ActiveLoans; }
            set { _ActiveLoans = value; OnPropertyChanged("ActiveLoans"); }
        }

        private InternalLoanWithoutGurantorsView _CurrentActiveLoans;
        public InternalLoanWithoutGurantorsView CurrentActiveLoans
        {
            get { return _CurrentActiveLoans; }
            set { _CurrentActiveLoans = value; OnPropertyChanged("CurrentActiveLoans"); }
        }

        private IEnumerable<InternalLoanWithoutGurantorsView> _GurantorActiveLoans;
        public IEnumerable<InternalLoanWithoutGurantorsView> GurantorActiveLoans
        {
            get { return _GurantorActiveLoans; }
            set { _GurantorActiveLoans = value; OnPropertyChanged("GurantorActiveLoans"); }
        }

        private InternalLoanWithoutGurantorsView _CurrentGurantorActiveLoans;
        public InternalLoanWithoutGurantorsView CurrentGurantorActiveLoans
        {
            get { return _CurrentGurantorActiveLoans; }
            set { _CurrentGurantorActiveLoans = value; OnPropertyChanged("CurrentGurantorActiveLoans"); }
        }

        private IEnumerable<InternalLoanMainView> _GetGuranteedLoansByApplicant;
        public IEnumerable<InternalLoanMainView> GetGuranteedLoansByApplicant
        {
            get { return _GetGuranteedLoansByApplicant; }
            set { _GetGuranteedLoansByApplicant = value; OnPropertyChanged("GetGuranteedLoansByApplicant"); }
        }

        private InternalLoanMainView _CurrentGetGuranteedLoansByApplicant;
        public InternalLoanMainView CurrentGetGuranteedLoansByApplicant
        {
            get { return _CurrentGetGuranteedLoansByApplicant; }
            set { _CurrentGetGuranteedLoansByApplicant = value; OnPropertyChanged("CurrentGetGuranteedLoansByApplicant"); }
        }

        private IEnumerable<InternalLoanMainView> _GetGuranteedLoansByGurantor;
        public IEnumerable<InternalLoanMainView> GetGuranteedLoansByGurantor
        {
            get { return _GetGuranteedLoansByGurantor; }
            set { _GetGuranteedLoansByGurantor = value; OnPropertyChanged("GetGuranteedLoansByGurantor"); }
        }

        private InternalLoanMainView _CurrentGetGuranteedLoansByGurantor;
        public InternalLoanMainView CurrentGetGuranteedLoansByGurantor
        {
            get { return _CurrentGetGuranteedLoansByGurantor; }
            set { _CurrentGetGuranteedLoansByGurantor = value; OnPropertyChanged("CurrentGetGuranteedLoansByGurantor"); }
        }

        #endregion

        #region RefreshMethods
        void RefreshCheckEmployeeLoan()
        {
            try
            {
                serviceClient.GetApprovedActiveLoanForEmployeeCompleted += (s, e) =>
                {
                    ApprovedLoan = e.Result;
                    if (ApprovedLoan != null)
                        AllLoanApprovedView = ApprovedLoan.ToList();
                    CurrentApprovedLoan = new LoanApprovedView();
                };
                serviceClient.GetApprovedActiveLoanForEmployeeAsync();
            }
            catch (Exception)
            {

            }
        }
        //void RefreshEmpSalaryDetailsForLastmonth()
        //{
        //    try
        //    {
        //        SalaryCheckForLoan = serviceClient.GetEmpSalaryDetailsForLastmonth((Guid)CurrentEmployee.employee_id, (Guid)CurrentPeriod.period_id);
        //        if (SalaryCheckForLoan != null && SalaryCheckForLoan.Count() > 0)
        //            AllSalaryCheckForLoan = SalaryCheckForLoan.ToList();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        void RefreshPeriod()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result;
                PeriodForGurantor = e.Result;
            };
            serviceClient.GetPeriodsAsync();
        }
        public void RefreshEmployees()
        {
            serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employee = e.Result.Where(c => c.isActive == true);
                if (Employee != null && Employee.Count() > 0)
                    AllEmployee = Employee.ToList();
            };
            serviceClient.GetAllEmployeeDetailAsync();
        }
        private void RefreshLoans()
        {
            serviceClient.GetLoansWithRulesCompleted += (s, e) =>
            {
                Loans = e.Result;
                GurontorLoan = e.Result;
            };
            serviceClient.GetLoansWithRulesAsync();
        }
        private void ReFreshActiveLoans()
        {
            serviceClient.GetInternalBankLoanWithoutGurantorsCompleted += (s, e) =>
            {
                if (e.Result != null && e.Result.Count() > 0)
                    AllActiveLoans = e.Result.ToList();

            };
            serviceClient.GetInternalBankLoanWithoutGurantorsAsync();
        }
        private void RefreshGurenteedLoans()
        {
            serviceClient.GetInternalBankLoanCompleted += (s, e) =>
            {
                if (e.Result != null && e.Result.Count() > 0)
                {
                    AllGuranteedLoans = e.Result.ToList();
                }

            };
            serviceClient.GetInternalBankLoanAsync();
        }

        #endregion

        #region Button Commands

        public ICommand ButtonRefreshApplicant
        {
            get { return new RelayCommand(New); }
        }
        public ICommand ButtonRefreshGuarantor
        {
            get { return new RelayCommand(New); }
        }
        public ICommand ButtonCalculateApplicant
        {
            get { return new RelayCommand(CalculateApplicant); }
        }
        public ICommand ButtonAplyLoanRpt
        {
            get { return new RelayCommand(AplyLoanRpt); }
        }
        public ICommand SearchEmpButton
        {
            get { return new RelayCommand(searchEmp); }
        }
        public ICommand SearchGurantorButton
        {
            get { return new RelayCommand(SearchGurantor); }
        }
        public ICommand ButtonCalculateGurantor
        {
            get { return new RelayCommand(CalculateGurantor); }
        }

        #endregion

        #region Methods

        #region Gurantor Methods
        private void SetGurantorDetails()
        {
            if (CurrentGurantorForDialogBox != null)
            {
                CurrentGurantor = null;
                CurrentGurantor = AllEmployee.FirstOrDefault(c => c.employee_id == CurrentGurantorForDialogBox.employee_id);
            }
        }
        private void SetGurantorServicePeriodType()
        {
            if (CurrentGurontorLoan.Guarantor_MinServicePeriod_Type == "Y")
            {
                CurrentGurontorLoan.Guarantor_MinServicePeriod_Type = "Years";
            }
            else if (CurrentGurontorLoan.Guarantor_MinServicePeriod_Type == "M")
            {
                CurrentGurontorLoan.Guarantor_MinServicePeriod_Type = "Months";
            }
        }
        private void GetActiveLoansOfLoanGurantor()
        {
            GurantorActiveLoans = null;
            GurantorActiveLoans = AllActiveLoans.Where(c => c.employee_id == CurrentGurantor.employee_id);
        }
        //  CurrentGurantorActiveLoans
        public double GetServicePeriodGurantor()
        {
            double Result = 0.00;
            if (CurrentGurontorLoan.Guarantor_MinServicePeriod_Type == "Y" || CurrentGurontorLoan.Guarantor_MinServicePeriod_Type == "Years")
            {
                var temp = AllEmployee.FirstOrDefault(c => c.employee_id == CurrentGurantor.employee_id).join_date;
                if (temp == null)
                {
                    int date = (int)(DateTime.Now - DateTime.Now).TotalDays;
                    double DevideDaysByMoths = 30.44;
                    double DevideMonthsByYears = 12;
                    Result = (date / DevideDaysByMoths) / DevideMonthsByYears;
                    if (Result == null)
                    {
                        Result = 0;
                        return Result;
                    }
                    else
                        return Result;
                }
                else
                {
                    int date = (int)(DateTime.Now - temp).Value.TotalDays;
                    double DevideDaysByMoths = 30.44;
                    double DevideMonthsByYears = 12;
                    Result = (date / DevideDaysByMoths) / DevideMonthsByYears;
                    if (Result == null)
                    {
                        Result = 0;
                        return Result;
                    }
                    else
                        return Result;
                }

            }
            else if (CurrentGurontorLoan.Guarantor_MinServicePeriod_Type == "M" || CurrentGurontorLoan.Guarantor_MinServicePeriod_Type == "Months")
            {
                var temp = AllEmployee.FirstOrDefault(c => c.employee_id == CurrentGurantor.employee_id).join_date;
                if (temp == null)
                {
                    int date = (int)(DateTime.Now - DateTime.Now).TotalDays;
                    double DevideDaysByMoths = 30.44;
                    Result = (date / DevideDaysByMoths);
                    if (Result == null)
                    {
                        Result = 0;
                        return Result;
                    }
                    else
                        return Result;
                }
                else
                {
                    int date = (int)(DateTime.Now - temp).Value.TotalDays;
                    double DevideDaysByMoths = 30.44;
                    Result = (date / DevideDaysByMoths);
                    if (Result == null)
                    {
                        Result = 0;
                        return Result;
                    }
                    else
                        return Result;
                }
            }                //clsMessages.setMessage(Result.ToString());
            return Result;
        }
        public int GetActiveLoanCountOFGurantor()
        {
            try
            {
                int count = 0;
                count = AllActiveLoans.Where(c => c.employee_id == CurrentGurantor.employee_id).Count();
                if (count == null)
                {
                    count = 0;
                    return count;
                }
                else
                    return count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int GetTotalGuranteedLoansByGurantor()
        {
            try
            {
                GetGuranteedLoansByGurantor = null;
                GetGuranteedLoansByGurantor = AllGuranteedLoans.Where(c => c.GuarantorID == CurrentGurantor.employee_id);
                int count = GetGuranteedLoansByApplicant.Count();
                if (count == null)
                {
                    count = 0;
                    return count;
                }
                else
                    return count;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        //private int GetTotalGuranteedLoansByGurantor()
        //{
        //    GetGuranteedLoansByGurantor = null;
        //    GetGuranteedLoansByGurantor = AllGuranteedLoans.Where(c => c.employee_id == CurrentGurantorActiveLoans.employee_id);
        //    int count = GetGuranteedLoansByApplicant.Count();
        //    clsMessages.setMessage(count.ToString());
        //    return count;
        //}
        private decimal? GetBasicSalaryForGurantor()
        {
            try
            {
                SalaryCheckForLoan = null;
                SalaryCheckForLoan = AllSalaryCheckForLoan.Where(e => e.epf_no == CurrentGurantor.epf_no && e.period_id == CurrentPeriod.period_id && e.rule_id == new Guid("00000001-0000-0000-0000-000000000000")).ToList();
                decimal? BasicSalary = SalaryCheckForLoan.FirstOrDefault().total_amount;
                return BasicSalary;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private decimal? GetTotBenifitForGurantor()
        {
            try
            {
                SalaryCheckForLoan = null;
                SalaryCheckForLoan = AllSalaryCheckForLoan.Where(e => e.epf_no == CurrentGurantor.epf_no && e.period_id == CurrentPeriod.period_id && e.rule_id == new Guid("00000002-0000-0000-0000-000000000000")).ToList();
                decimal? BasicSalary = SalaryCheckForLoan.FirstOrDefault().total_amount;
                return BasicSalary;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private decimal? GetTotDeductionForGurantor()
        {
            try
            {
                SalaryCheckForLoan = null;
                SalaryCheckForLoan = AllSalaryCheckForLoan.Where(e => e.epf_no == CurrentGurantor.epf_no && e.period_id == CurrentPeriod.period_id && e.rule_id == new Guid("00000003-0000-0000-0000-000000000000")).ToList();
                decimal? BasicSalary = SalaryCheckForLoan.FirstOrDefault().total_amount;
                return BasicSalary;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private decimal? GetDeductionPercentageForGurantor()
        {
            try
            {
                decimal? TotSalWithBenifit = GetBasicSalaryForGurantor() + GetTotBenifitForGurantor();
                decimal? DeductionPercentage = (GetTotDeductionForGurantor() / TotSalWithBenifit) * 100;
                if (DeductionPercentage == null)
                {
                    DeductionPercentage = 0;
                    return DeductionPercentage;
                }
                else
                    return DeductionPercentage;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public bool? GetPermenentGurantor()
        {
            var Permenent = AllEmployee.FirstOrDefault(c => c.employee_id == CurrentGurantor.employee_id).prmernant_active_date;
            if (Permenent != null)
                return true;
            else
                return false;
        }
        void SearchGurantor()
        {
            EmployeeSearchWindow searchControl = new EmployeeSearchWindow();

            bool? b = searchControl.ShowDialog();
            if (b != null)
            {
                CurrentGurantorForDialogBox = searchControl.viewModel.CurrentEmployeeSearchView;
            }
            searchControl.Close();
        }
        private void CalculateGurantor()
        {
            if (clsSecurity.GetViewPermission(607))
            {
                if (ValidateGurantor())
                {
                    List<EligibilityForGurantorCLS> x2 = new List<EligibilityForGurantorCLS>();
                    EligibilityForGurantorCLS temp = new EligibilityForGurantorCLS();
                    temp.DeductionPercentageOfPayrollRule = (double)CurrentGurontorLoan.Guarantor_DeductionPercentageinPayroll;
                    temp.DeductionPercentageOfPayrollActual = (double)GetDeductionPercentageForGurantor();
                    if (temp.DeductionPercentageOfPayrollActual <= temp.DeductionPercentageOfPayrollRule)
                        temp.DeductionPercentageOfPayrollEligibility = "Eligible";
                    else
                        temp.DeductionPercentageOfPayrollEligibility = "Not Eligible";
                    temp.MaxNoGuranteedLoansRule = (int)CurrentGurontorLoan.Guarantor_MaxguaranteedLoans;
                    temp.MaxNoGuranteedLoansActual = GetTotalGuranteedLoansByGurantor();
                    if (temp.MaxNoGuranteedLoansActual <= temp.MaxNoGuranteedLoansRule)
                        temp.MaxNoGuranteedLoansEligibility = "Eligible";
                    else
                        temp.MaxNoGuranteedLoansEligibility = "Not Eligible";
                    temp.MaxNoLoansInProgressRule = (int)CurrentGurontorLoan.Guarantor_NoLoansInProgress;
                    temp.MaxNoLoansInProgressActual = GetActiveLoanCountOFGurantor();
                    if (temp.MaxNoLoansInProgressActual <= temp.MaxNoLoansInProgressRule)
                        temp.MaxNoLoansInProgressEligibility = "Eligible";
                    else
                        temp.MaxNoLoansInProgressEligibility = "Not Eligible";
                    temp.MinServicePeriodRule = (int)CurrentGurontorLoan.Guarantor_MinServicePeriod;
                    temp.MinServicePeriodActual = GetServicePeriodGurantor();
                    if (temp.MinServicePeriodActual >= temp.MinServicePeriodRule)
                        temp.MinServicePeriodEligibility = "Eligible";
                    else
                        temp.MinServicePeriodEligibility = "Not Eligible";
                    temp.PermentemployeeRule = (bool)CurrentGurontorLoan.Guarantor_Permanent;
                    temp.PermentemployeeActual = (bool)GetPermenentGurantor();
                    if (temp.PermentemployeeActual == temp.PermentemployeeRule || temp.PermentemployeeActual == true)
                        temp.PermentemployeeEligibility = "Eligible";
                    else
                        temp.PermentemployeeEligibility = "Not Eligible";
                    temp.GurantorEPF = CurrentGurantor.epf_no;
                    temp.GurantorFirstName = CurrentGurantor.first_name;
                    temp.GurantorLastName = CurrentGurantor.surname;
                    temp.GurantorNIC = CurrentGurantor.nic;
                    temp.LoanName = CurrentGurontorLoan.loan_name;
                    x2.Add(temp);
                    EligibilityForGurantor report = new EligibilityForGurantor();
                    report.SetDataSource(x2);
                    ReportViewer viewer = new ReportViewer(report, false);
                    viewer.Show();
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to view");
        }
        private bool ValidateGurantor()
        {
            if (CurrentGurantor == null)
            {
                clsMessages.setMessage("Please Select An Guarantor To Process...");
                return false;
            }
            else if (CurrentGurantor.employee_id == null || CurrentGurantor.employee_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select An Guarantor To Process...");
                return false;
            }
            else if (CurrentGurontorLoan.loan_id == null || CurrentGurontorLoan.loan_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select An Loan To Process...");
                return false;
            }
            else if (CurrentPeriodForGurantor == null)
            {
                clsMessages.setMessage("Please Select An Period To Process...");
                return false;
            }
            else if (CurrentPeriodForGurantor.period_id == null || CurrentPeriodForGurantor.period_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select An Period To Process...");
                return false;
            }
            else
                return true;
        }

        #endregion

        #region Aplicant Methods
        private void SetEmployeeDetails()
        {
            if (CurrentEmployeesForDialogBox != null)
            {
                CurrentEmployee = null;
                CurrentEmployee = AllEmployee.FirstOrDefault(e => e.employee_id == CurrentEmployeesForDialogBox.employee_id);

                //CurrentApprovedLoan.employee_id = CurrentEmployee.employee_id;
                //CurrentApprovedLoan.epf_no = CurrentEmployee.epf_no;
                //CurrentApprovedLoan.first_name = CurrentEmployee.first_name;
                //CurrentApprovedLoan.nic = CurrentEmployee.nic;
                //CurrentApprovedLoan.surname = CurrentEmployee.surname;

            }
            else
            {
                CurrentEmployee = null;
            }
        }
        public void SetEmployeeServicePeriodType()
        {
            if (CurrentLoan.Emp_MinServicePeriod_Type == "Y")
            {
                CurrentLoan.Emp_MinServicePeriod_Type = "Years";
            }
            else if (CurrentLoan.Emp_MinServicePeriod_Type == "M")
            {
                CurrentLoan.Emp_MinServicePeriod_Type = "Months";
            }
        }
        private void GetActiveLoansOfLoanApplicant()
        {
            ActiveLoans = null;
            ActiveLoans = AllActiveLoans.Where(c => c.employee_id == CurrentEmployee.employee_id);
        }
        public double GetServicePeriodApplicant()
        {
            try
            {
                double Result = 0.00;
                if (CurrentLoan.Emp_MinServicePeriod_Type == "Years" || CurrentLoan.Emp_MinServicePeriod_Type == "Y")
                {
                    var temp = AllEmployee.FirstOrDefault(c => c.employee_id == CurrentEmployee.employee_id).join_date;
                    if (temp == null)
                    {
                        int date = (int)(DateTime.Now - DateTime.Now).TotalDays;
                        double DevideDaysByMoths = 30.44;
                        double DevideMonthsByYears = 12;
                        Result = (date / DevideDaysByMoths) / DevideMonthsByYears;
                        if (Result == null)
                        {
                            Result = 0;
                            return Result;
                        }
                        else
                            return Result;
                    }
                    else
                    {
                        int date = (int)(DateTime.Now - temp).Value.TotalDays;
                        double DevideDaysByMoths = 30.44;
                        double DevideMonthsByYears = 12;
                        Result = (date / DevideDaysByMoths) / DevideMonthsByYears;
                        if (Result == null)
                        {
                            Result = 0;
                            return Result;
                        }
                        else
                            return Result;
                    }

                }
                else if (CurrentLoan.Emp_MinServicePeriod_Type == "Months" || CurrentLoan.Emp_MinServicePeriod_Type == "M")
                {
                    var temp = AllEmployee.FirstOrDefault(c => c.employee_id == CurrentEmployee.employee_id).join_date;
                    if (temp == null)
                    {
                        int date = (int)(DateTime.Now - DateTime.Now).TotalDays;
                        double DevideDaysByMoths = 30.44;
                        Result = (date / DevideDaysByMoths);
                        if (Result == null)
                        {
                            Result = 0;
                            return Result;
                        }
                        else
                            return Result;
                    }
                    else
                    {
                        int date = (int)(DateTime.Now - temp).Value.TotalDays;
                        double DevideDaysByMoths = 30.44;
                        Result = (date / DevideDaysByMoths);
                        if (Result == null)
                        {
                            Result = 0;
                            return Result;
                        }
                        else
                            return Result;
                    }
                }
                else
                    //clsMessages.setMessage(Result.ToString());
                    return Result;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public int GetActiveLoanCountOFApplicant()
        {
            try
            {
                int count = 0;
                count = AllActiveLoans.Where(c => c.employee_id == CurrentEmployee.employee_id).Count();
                //clsMessages.setMessage(count.ToString());
                if (count == null)
                {
                    count = 0;
                    return count;
                }
                else
                    return count;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public int GetTotalGuranteedLoansByApplicant()
        {
            try
            {
                GetGuranteedLoansByApplicant = null;
                GetGuranteedLoansByApplicant = AllGuranteedLoans.Where(c => c.GuarantorID == CurrentEmployee.employee_id);
                int count = GetGuranteedLoansByApplicant.Count();
                if (count == null)
                {
                    count = 0;
                    return count;
                }
                else
                    return count;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        private decimal? GetBasicSalary()
        {
            try
            {
                SalaryCheckForLoan = null;
                AllSalaryCheckForLoan = serviceClient.GetEmpSalaryDetailsForLastmonth(CurrentEmployee.employee_id, CurrentPeriod.period_id).ToList();
                SalaryCheckForLoan = AllSalaryCheckForLoan.Where(e => e.epf_no == CurrentEmployee.epf_no && e.period_id == CurrentPeriod.period_id && e.rule_id == new Guid("00000001-0000-0000-0000-000000000000")).ToList();
                decimal? BasicSalary = SalaryCheckForLoan.FirstOrDefault().total_amount;
                return BasicSalary;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private decimal? GetTotBenifit()
        {
            try
            {
                SalaryCheckForLoan = null;
                SalaryCheckForLoan = AllSalaryCheckForLoan.Where(e => e.epf_no == CurrentEmployee.epf_no && e.period_id == CurrentPeriod.period_id && e.rule_id == new Guid("00000002-0000-0000-0000-000000000000")).ToList();
                decimal? BasicSalary = SalaryCheckForLoan.FirstOrDefault().total_amount;
                return BasicSalary;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private decimal? GetTotDeduction()
        {
            try
            {
                SalaryCheckForLoan = null;
                SalaryCheckForLoan = AllSalaryCheckForLoan.Where(e => e.epf_no == CurrentEmployee.epf_no && e.period_id == CurrentPeriod.period_id && e.rule_id == new Guid("00000003-0000-0000-0000-000000000000")).ToList();
                decimal? BasicSalary = SalaryCheckForLoan.FirstOrDefault().total_amount;
                return BasicSalary;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        private decimal? GetDeductionPercentage()
        {
            try
            {
                decimal? TotSalWithBenifit = GetBasicSalary() + GetTotBenifit();
                decimal? DeductionPercentage = (GetTotDeduction() / TotSalWithBenifit) * 100;
                if (DeductionPercentage == null)
                {
                    DeductionPercentage = 0;
                    return DeductionPercentage;
                }
                else
                    return DeductionPercentage;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public bool? GetPermenentApplicant()
        {
            try
            {
                var Permenent = AllEmployee.FirstOrDefault(c => c.employee_id == CurrentEmployee.employee_id);
                if (Permenent != null)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {

                return false;
            }

        }
        private void CalculateApplicant()
        {
            if (ValidateApplicant())
            {
                //if (CurrentLoan.Emp_DeductionPercentageOfPayroll >= GetDeductionPercentage() && CurrentLoan.Emp_NoLoansInProgress >= GetActiveLoanCountOFApplicant() && CurrentLoan.Emp_MaxguaranteedLoans >= GetTotalGuranteedLoansByApplicant() && CurrentLoan.Emp_MinServicePeriod <= GetServicePeriodApplicant() && CurrentLoan.Emp_Permanent == GetPermenentApplicant())
                //{
                //CurrentLoan.Emp_NoLoansInProgress <= GetActiveLoanCountOFApplicant() && CurrentLoan.Emp_MinServicePeriod <= GetServicePeriodApplicant() &&  && CurrentLoan.Emp_Permanent == GetPermenentApplicant() && CurrentLoan.Emp_MaxguaranteedLoans == GetTotalGuranteedLoansByApplicant()
                //clsMessages.setMessage("The Applicant Is Elegible For A Loan...");
                PrintEligibility();
                //}
                //else
                //{
                //    clsMessages.setMessage("The Applicant is Not Elegible For A Loan...");
                //    PrintEligibility();
                //}

            }
        }
        private bool ValidateApplicant()
        {
            if (CurrentEmployee == null)
            {
                clsMessages.setMessage("Please Select An Employee To Process...");
                return false;
            }
            else if (CurrentEmployee.employee_id == null || CurrentEmployee.employee_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select An Employee To Process...");
                return false;
            }
            else if (CurrentLoan.loan_id == null || CurrentLoan.loan_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Loan To Process...");
                return false;
            }
            else if (CurrentPeriod == null )
            {
                clsMessages.setMessage("Please Select A Period To Process...");
                return false;
            }
            else if (CurrentPeriod.period_id == null || CurrentPeriod.period_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Period To Process...");
                return false;
            }
            else
                return true;
        }
        //private void New()
        //{
        //    RefreshCheckEmployeeLoan();
        //    // RefreshEmpSalaryDetailsForLastmonth();
        //    RefreshLoans();
        //    ChkLoanPersentage = null;
        //    RefreshPeriod();
        //    RefreshEmployees();
        //}
        void searchEmp()
        {
            EmployeeSearchWindow searchControl = new EmployeeSearchWindow();

            bool? b = searchControl.ShowDialog();
            if (b != null)
            {
                CurrentEmployeesForDialogBox = searchControl.viewModel.CurrentEmployeeSearchView;
            }
            searchControl.Close();
        }
        private void PrintEligibility()
        {
            try
            {
                if (clsSecurity.GetViewPermission(607))
                {
                    List<EligibilityForApplicantCLS> x2 = new List<EligibilityForApplicantCLS>();
                    EligibilityForApplicantCLS temp = new EligibilityForApplicantCLS();
                    temp.DeductionPercentageOfPayrollRule = (double)CurrentLoan.Emp_DeductionPercentageOfPayroll;
                    temp.DeductionPercentageOfPayrollActual = (double)GetDeductionPercentage();
                    if (temp.DeductionPercentageOfPayrollActual > temp.DeductionPercentageOfPayrollRule)
                        temp.DeductionPercentageOfPayrollEligibility = "Not Eligible";
                    else
                        temp.DeductionPercentageOfPayrollEligibility = "Eligible";
                    temp.MaxNoGuranteedLoansRule = (int)CurrentLoan.Emp_MaxguaranteedLoans;
                    temp.MaxNoGuranteedLoansActual = GetTotalGuranteedLoansByApplicant();
                    if (temp.MaxNoGuranteedLoansActual > temp.MaxNoGuranteedLoansRule)
                        temp.MaxNoGuranteedLoansEligibility = "Not Eligible";
                    else
                        temp.MaxNoGuranteedLoansEligibility = "Eligible";
                    temp.MaxNoLoansInProgressRule = (int)CurrentLoan.Emp_NoLoansInProgress;
                    temp.MaxNoLoansInProgressActual = GetActiveLoanCountOFApplicant();
                    if (temp.MaxNoLoansInProgressActual > temp.MaxNoLoansInProgressActual)
                        temp.MaxNoLoansInProgressEligibility = "Not Eligible";
                    else
                        temp.MaxNoLoansInProgressEligibility = "Eligible";
                    temp.MinServicePeriodRule = (int)CurrentLoan.Emp_MinServicePeriod;
                    temp.MinServicePeriodActual = GetServicePeriodApplicant();
                    if (temp.MinServicePeriodActual < temp.MinServicePeriodRule)
                        temp.MinServicePeriodEligibility = "Not Eligible";
                    else
                        temp.MinServicePeriodEligibility = "Eligible";
                    temp.PermentemployeeRule = (bool)CurrentLoan.Emp_Permanent;
                    temp.PermentemployeeActual = (bool)GetPermenentApplicant();
                    if (temp.PermentemployeeActual == temp.PermentemployeeRule || temp.PermentemployeeActual == true)
                        temp.PermentemployeeEligibility = "Eligible";
                    else
                        temp.PermentemployeeEligibility = "Not Eligible";
                    temp.ApplicantEPF = CurrentEmployee.epf_no;
                    temp.ApplicantFirstName = CurrentEmployee.first_name;
                    temp.ApplicantLastName = CurrentEmployee.surname;
                    temp.ApplicantNIC = CurrentEmployee.nic;
                    temp.LoanName = CurrentLoan.loan_name;
                    x2.Add(temp);
                    EligibilityForApplicant report = new EligibilityForApplicant();
                    report.SetDataSource(x2);
                    ReportViewer viewer = new ReportViewer(report, false);
                    viewer.Show(); 
                }
                else
                    clsMessages.setMessage("You don't have permission to view");
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error...");
            }

        }

        #endregion

        private void AplyLoanRpt()
        {
            try
            {
                if (clsSecurity.GetViewPermission(607))
                {
                    string path = "\\Reports\\Documents\\Loan_Report\\Apply_For_Loan";
                    ReportPrint print = new ReportPrint(path);
                    print.LoadToReportViewer(); 
                }
                else
                    clsMessages.setMessage("You don't have permission to view");
            }
            catch (Exception)
            {

            }

        }
        private void FilterApprovedLoan()
        {
            ApprovedLoan = null;
            // ApprovedLoan = AllLoanApprovedView;
            ApprovedLoan = AllLoanApprovedView.Where(c => c.epf_no == CurrentApprovedLoan.epf_no);


        }
        private void New()
        {
            AllLoanApprovedView = new List<LoanApprovedView>();
            AllSalaryCheckForLoan = new List<rpt_EPFEReturnView>();
            CurrentSalaryCheckForLoan = new rpt_EPFEReturnView();
            CurrentApprovedLoan = new LoanApprovedView();
            AllEmployee = new List<EmployeeSumarryView>();
            AllActiveLoans = new List<InternalLoanWithoutGurantorsView>();
            AllGuranteedLoans = new List<InternalLoanMainView>();
            RefreshCheckEmployeeLoan();
            RefreshPeriod();
            RefreshEmployees();
            RefreshLoans();
            RefreshGurenteedLoans();
            //    RefreshEmpSalaryDetailsForLastmonth();
            ReFreshActiveLoans();
            CurrentPeriod = new z_Period();
            CurrentGurantorActiveLoans = new InternalLoanWithoutGurantorsView();
            //      CurrentEmployee = new EmployeeSumarryView();
        }

        #endregion
    }
}
