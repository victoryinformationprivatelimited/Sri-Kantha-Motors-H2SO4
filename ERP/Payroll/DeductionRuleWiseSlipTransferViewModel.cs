using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP;
using ERP.ERPService;
using System.Windows.Input;
using ERP.Payroll.SlipTransfer;
using System.Windows;
using CustomBusyBox;

namespace ERP.Payroll
{
    class DeductionRuleWiseSlipTransferViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        List<SlipTransferEmployeeDeductionView> RuleDeductionList = new List<SlipTransferEmployeeDeductionView>();
        #endregion

        #region Constructor
        public DeductionRuleWiseSlipTransferViewModel()
        {
            serviceClient = new ERPServiceClient();
            SelectDate = DateTime.Now.Date;
            RefreshDeductionRules();
            RefreshPeriods();
            RefreshCompanyBranches();
        }
        #endregion

        #region Properties
        private IEnumerable<SlipTransferEmployeeDeductionView> _RuleDeductions;

        public IEnumerable<SlipTransferEmployeeDeductionView> RuleDeductions
        {
            get { return _RuleDeductions; }
            set { _RuleDeductions = value; OnPropertyChanged("RuleDeductions"); }
        }

        private SlipTransferEmployeeDeductionView _CurrentRuleDeduction;

        public SlipTransferEmployeeDeductionView CurrentRuleDeduction
        {
            get { return _CurrentRuleDeduction; }
            set { _CurrentRuleDeduction = value; OnPropertyChanged("CurrentRuleDeduction"); }
        }

        private IEnumerable<z_CompanyBranches> _CompanyBranches;

        public IEnumerable<z_CompanyBranches> CompanyBranches
        {
            get { return _CompanyBranches; }
            set { _CompanyBranches = value; OnPropertyChanged("CompanyBranches"); }
        }

        private z_CompanyBranches _CurrentCompanyBranch;

        public z_CompanyBranches CurrentCompanyBranch
        {
            get { return _CurrentCompanyBranch; }
            set { _CurrentCompanyBranch = value; OnPropertyChanged("CurrentCompanyBranch"); }
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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod != null) RefreshEmployeeRuleDeductions(); }
        }

        private DateTime _SelectDate;

        public DateTime SelectDate
        {
            get { return _SelectDate; }
            set { _SelectDate = value; OnPropertyChanged("SelectDate"); }
        }
        
        #endregion

        #region Refresh Methods
        public void RefreshEmployeeRuleDeductions()
        {
            try
            {
                serviceClient.GetSlipTransferDeductionCompleted += (s, e) =>
                    {
                        RuleDeductions = e.Result;
                        if (RuleDeductions != null)
                            RuleDeductionList = RuleDeductions.ToList();
                    };
                serviceClient.GetSlipTransferDeductionAsync(CurrentCompanyRule, CurrentPeriod);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employee Rule Deductions Loading Failed");
            }
        }

        public void RefreshPeriods()
        {
            try
            {
                serviceClient.GetPeriodsCompleted += (s, e) =>
                    {
                        Periods = e.Result.OrderBy(c => c.start_date);
                    };
                serviceClient.GetPeriodsAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Periods Loading Failed");
            }
        }

        public void RefreshDeductionRules()
        {
            try
            {
                serviceClient.GetCompanyRulesCompleted += (s, e) =>
                    {
                        CompanyRules = e.Result.Where(c => c.deduction_id != new Guid("00000000-0000-0000-0000-000000000000")).OrderBy(c => c.rule_name);
                    };
                serviceClient.GetCompanyRulesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Deduction Rules Loading Failed");
            }
        }

        public void RefreshCompanyBranches()
        {
            try
            {
                serviceClient.GetCompanyBranchesCompleted += (s, e) =>
                    {
                        CompanyBranches = e.Result.Where(c => c.companyBranch_id == new Guid("fd18cfb6-ec4c-4098-b255-29a4c1aa8bf0"));
                    };
                serviceClient.GetCompanyBranchesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Company Branches Loading Failed");
            }
        }
        #endregion

        #region Commands & Methods
        public ICommand GenerateButton
        {
            get { return new RelayCommand(Generate); }
        }

        private void Generate()
        {
            BusyBox.ShowBusy("Please Wait Until Slip Generate Completed");
            z_Bank Bank = new z_Bank();
            Bank = serviceClient.GetBanks().Where(i => i.bank_id == CurrentCompanyBranch.bank_id).FirstOrDefault();


            try
            {
                int BANKCODE = 0;
                int BANKBRANCH = 0;
                string ACCOUNTNO = "";
                string COMPANYBRANCHACCOUNTNAME = "";
                string PAYPERIOD = "";
                string COMPANYBRANCH = "";
                string RULENAME = "";
                string COMPANYBRANCHID = "";
                string PERIODID = "";

                BANKCODE = Convert.ToInt16(Bank.bank_code);
                BANKBRANCH = Convert.ToInt16(serviceClient.GetBanckBranch().Where(i => i.branch_id == CurrentCompanyBranch.bank_branch_id).FirstOrDefault().bank_branch_code);
                ACCOUNTNO = CurrentCompanyBranch.account_no.ToString();
                COMPANYBRANCHACCOUNTNAME = CurrentCompanyBranch.account_name;
                PAYPERIOD = CurrentPeriod.period_name;
                COMPANYBRANCH = CurrentCompanyBranch.companyBranch_Name;
                COMPANYBRANCHID = CurrentCompanyBranch.companyBranch_id.ToString();
                PERIODID = CurrentPeriod.period_id.ToString();
                RULENAME = CurrentCompanyRule.rule_name;
                if (BANKCODE >= 0 && BANKBRANCH >= 0 && RuleDeductionList.Count() > 0)
                {
                    BankSelector select = new BankSelector(Bank.bank_name, RuleDeductionList, SelectDate, BANKCODE, BANKBRANCH, ACCOUNTNO, COMPANYBRANCHACCOUNTNAME, PAYPERIOD, COMPANYBRANCH, COMPANYBRANCHID, PERIODID, RULENAME);
                    select.CheckPath();
                    New();
                }

            }
            catch (Exception e)
            {
                BusyBox.CloseBusy();
                MessageBox.Show(e.ToString());
            }

        }

        private void New()
        {
            CurrentCompanyBranch = null;
            CurrentPeriod = null;
            RuleDeductions = null;
            RuleDeductionList = null;
            RefreshCompanyBranches();
            RefreshPeriods();
        }
        #endregion
    }
}
