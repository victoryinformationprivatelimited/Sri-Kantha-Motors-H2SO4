using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using ERP.Payroll.SlipTransfer;
using System.Windows;
using CustomBusyBox;

namespace ERP.Payroll.Employee_Bonus
{
    class BonusSlipTransferViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        List<BonusSlipTransferView> BonusAssignedEmployeeList;
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
            set { _CurrentCompanyBranch = value; OnPropertyChanged("CurrentCompnyBranch"); }
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
            set { _CurrentBonusPeriod = value; OnPropertyChanged("CurrentBonusPeriod"); if (CurrentCompanyBranch != null && CurrentBonusPeriod != null) RefreshBonusAssignedEmployees(); }
        }

        private DateTime _SelectDate;

        public DateTime SelectDate
        {
            get { return _SelectDate; }
            set { _SelectDate = value; OnPropertyChanged("SelectDate"); }
        }

        private IEnumerable<BonusSlipTransferView> _BonusAssignedEmployees;

        public IEnumerable<BonusSlipTransferView> BonusAssignedEmployees
        {
            get { return _BonusAssignedEmployees; }
            set { _BonusAssignedEmployees = value; OnPropertyChanged("BonusAssignedEmployees"); }
        }

        private BonusSlipTransferView _CurrentBonusAssignedEmployee;

        public BonusSlipTransferView CurrentBonusAssignedEmployee
        {
            get { return _CurrentBonusAssignedEmployee; }
            set { _CurrentBonusAssignedEmployee = value; OnPropertyChanged("CurrentBonusAssignedEmployee"); }
        }

        #endregion

        #region Constructor
        public BonusSlipTransferViewModel()
        {
            serviceClient = new ERPServiceClient();
            BonusAssignedEmployeeList = new List<BonusSlipTransferView>();
            SelectDate = DateTime.Now.Date;
            RefreshCompanyBranches();
            RefreshBonusPeriod();
        }
        #endregion

        #region Refresh Methods
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
                clsMessages.setMessage("Company Branches Refresh Failed");
            }
        }

        void RefreshBonusPeriod()
        {
            try
            {
                serviceClient.GetBonusPeriodCompleted += (s, e) =>
                {
                    BonusPeriod = e.Result;
                };
                serviceClient.GetBonusPeriodAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Bonus Period Refresh Failed");
            }
        }

        void RefreshBonusAssignedEmployees()
        {
            try
            {
                serviceClient.GetBonusAssignedEmployeesCompleted += (s, e) =>
                {
                    BonusAssignedEmployees = e.Result.OrderBy(c => c.emp_id);
                    if (BonusAssignedEmployees != null)
                        BonusAssignedEmployeeList = BonusAssignedEmployees.ToList();
                };
                serviceClient.GetBonusAssignedEmployeesAsync(CurrentBonusPeriod.Bonus_Period_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Bonus Period Refresh Failed");
            }
        }
        #endregion

        #region Commands And Button Methods
        public ICommand GenerateBTN
        {
            get { return new RelayCommand(Generate); }
        }

        private void Generate()
        {
            BusyBox.ShowBusy("Please Wait Until Slip Transfer Generated");
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
                PAYPERIOD = CurrentBonusPeriod.Bonus_Period_Name;
                COMPANYBRANCH = CurrentCompanyBranch.companyBranch_Name;
                COMPANYBRANCHID = CurrentCompanyBranch.companyBranch_id.ToString();
                PERIODID = CurrentBonusPeriod.Bonus_Period_id.ToString();

                if (BANKCODE >= 0 && BANKBRANCH >= 0 && BonusAssignedEmployeeList.Count() > 0)
                {
                    BankSelector select = new BankSelector(Bank.bank_name, BonusAssignedEmployeeList, SelectDate, BANKCODE, BANKBRANCH, ACCOUNTNO, COMPANYBRANCHACCOUNTNAME, PAYPERIOD, COMPANYBRANCH, COMPANYBRANCHID, PERIODID);


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
            CurrentBonusPeriod = null;
            BonusAssignedEmployees = null;
            BonusAssignedEmployeeList = null;
            RefreshCompanyBranches();
            RefreshBonusPeriod();
        }
        #endregion
    }
}
