using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ERP.ERPService;
using ERP.Payroll.SlipTransfer;
using CustomBusyBox;

namespace ERP.Loan_Module.New_LoanForms
{
    class ExternalBankLoanSlipViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        List<ExternalBankLoanSlipView> ExternalSlipViewList;
        #endregion

        #region Properties
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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); if (CurrentPeriod != null) RefreshExternalBankLoanSlip(); }
        }

        private IEnumerable<ExternalBankLoanSlipView> _ExternalSlipView;

        public IEnumerable<ExternalBankLoanSlipView> ExternalSlipView
        {
            get { return _ExternalSlipView; }
            set { _ExternalSlipView = value; OnPropertyChanged("ExternalSlipView"); }
        }


        private DateTime _SelectDate;
        public DateTime SelectDate
        {
            get { return _SelectDate; }
            set { _SelectDate = value; OnPropertyChanged("SelectDate"); }
        }

        private IEnumerable<z_Bank> _BankName;
        public IEnumerable<z_Bank> BankName
        {
            get { return _BankName; }
            set { _BankName = value; OnPropertyChanged("BankName"); }
        }
        #endregion

        #region Constructor
        public ExternalBankLoanSlipViewModel()
        {
            serviceClient = new ERPServiceClient();
            ExternalSlipViewList = new List<ExternalBankLoanSlipView>();
            SelectDate = DateTime.Now.Date;
            RefreshBankName();
            RefreshCompanyBranches();
            RefreshPayPeriod();
        }
        #endregion

        #region Refresh Methods
        void RefreshBankName()
        {
            try
            {
                serviceClient.GetBanksCompleted += (s, e) =>
                {
                    this.BankName = e.Result.Where(z => z.isdelete == false);
                };
                this.serviceClient.GetBanksAsync();
            }
            catch (Exception)
            {

                throw null;
            }
        }

        void RefreshExternalBankLoanSlip()
        {

            try
            {

                if (CurrentCompanyBranch != null && CurrentPeriod != null)
                {

                    serviceClient.GetExternalBankLoanProcessedEmployeesCompleted += (s, e) =>
                    {
                        ExternalSlipView = e.Result.OrderBy(c => c.emp_id);
                        if (ExternalSlipView != null)
                        {
                            ExternalSlipViewList = ExternalSlipView.ToList();
                        }
                    };

                    serviceClient.GetExternalBankLoanProcessedEmployeesAsync(CurrentPeriod.period_id);

                }

                else
                {
                    clsMessages.setMessage("you have to select a pay period");
                }

            }
            catch (Exception)
            {



            }
        }

        void RefreshCompanyBranches()
        {
            try
            {
                serviceClient.GetCompanyBranchesCompleted += (s, e) =>
                {
                    this.CompanyBranches = e.Result.Where(c => c.companyBranch_id == new Guid("FD18CFB6-EC4C-4098-B255-29A4C1AA8BF0"));
                };
                serviceClient.GetCompanyBranchesAsync();
            }
            catch (Exception)
            {

                throw null;
            }
        }

        private void RefreshPayPeriod()
        {
            try
            {
                serviceClient.GetPeriodsCompleted += (s, e) =>
                {
                    this.Periods = e.Result;
                };
                serviceClient.GetPeriodsAsync();
            }
            catch (Exception)
            {

                throw null;
            }
        }
        #endregion

        #region Buttons Commands
        public ICommand GenerateButton
        {
            get { return new RelayCommand(Genarate); }
        }
        void Genarate()
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

                if (BANKCODE >= 0 && BANKBRANCH >= 0 && ExternalSlipViewList.Count() > 0)
                {
                    BankSelector select = new BankSelector(Bank.bank_name, ExternalSlipViewList, SelectDate, BANKCODE, BANKBRANCH, ACCOUNTNO, COMPANYBRANCHACCOUNTNAME, PAYPERIOD, COMPANYBRANCH, COMPANYBRANCHID, PERIODID);


                    if (select.CheckPath())
                    {
                        New();
                    }
                }
            }
            catch (Exception e)
            {
                BusyBox.CloseBusy();
                MessageBox.Show(e.ToString());
            }
        }

        void New()
        {
            ExternalSlipViewList.Clear();
            ExternalSlipView = null;
            serviceClient = new ERPServiceClient();
            RefreshBankName();
            RefreshCompanyBranches();
            RefreshPayPeriod();
            SelectDate = DateTime.Now.Date;

        }
        #endregion
    }
}
