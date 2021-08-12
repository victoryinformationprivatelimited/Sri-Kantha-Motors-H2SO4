using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using ERP.Payroll.SlipTransfer;

namespace ERP.Payroll.Employee_Third_Party_Payments
{
    class EmployeeThirdPartyPaymentsSlipTransferViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        List<EmployeeThirdPartyPaymentsSlipTransferSP_Result> EmployeeThirdPartyPaymentsList;
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

        private DateTime _FromDate;

        public DateTime FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; OnPropertyChanged("FromDate"); }
        }

        private DateTime _ToDate;

        public DateTime ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; OnPropertyChanged("ToDate"); }
        }

        private IEnumerable<EmployeeThirdPartyPaymentsSlipTransferSP_Result> _EmployeeThirdPartyPayments;

        public IEnumerable<EmployeeThirdPartyPaymentsSlipTransferSP_Result> EmployeeThirdPartyPayments
        {
            get { return _EmployeeThirdPartyPayments; }
            set { _EmployeeThirdPartyPayments = value; OnPropertyChanged("EmployeeThirdPartyPayments"); }
        }

        private IEnumerable<z_EmployeeThirdPartyPayments> _EmployeeThirdPartyCategories;

        public IEnumerable<z_EmployeeThirdPartyPayments> EmployeeThirdPartyCategories
        {
            get { return _EmployeeThirdPartyCategories; }
            set { _EmployeeThirdPartyCategories = value; OnPropertyChanged("EmployeeThirdPartyCategories"); }
        }

        private z_EmployeeThirdPartyPayments _CurrentEmployeeThirdPartyCategories;

        public z_EmployeeThirdPartyPayments CurrentEmployeeThirdPartyCategories
        {
            get { return _CurrentEmployeeThirdPartyCategories; }
            set { _CurrentEmployeeThirdPartyCategories = value; OnPropertyChanged("CurrentEmployeeThirdPartyCategories"); if (CurrentEmployeeThirdPartyCategories != null) RefreshEmployeeThirdPartyPayments(); }
        }
        #endregion

        #region Constructor
        public EmployeeThirdPartyPaymentsSlipTransferViewModel()
        {
            serviceClient = new ERPServiceClient();
            EmployeeThirdPartyPaymentsList = new List<EmployeeThirdPartyPaymentsSlipTransferSP_Result>();
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
            RefreshCompanyBranches();
            RefreshEmployeeThirdPartyCategories();
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

        void RefreshEmployeeThirdPartyCategories()
        {
            try
            {
                serviceClient.GetThirdPartyCategoriesCompleted += (s, e) =>
                {
                    EmployeeThirdPartyCategories = e.Result;
                };
                serviceClient.GetThirdPartyCategoriesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Third Party Payment Categories Refresh Failed");
            }
        }

        void RefreshEmployeeThirdPartyPayments()
        {
            try
            {
                //serviceClient.GetThirdPartyPaymentsDataForSlipTransferCompleted += (s, e) =>
                //{
                EmployeeThirdPartyPayments = serviceClient.GetThirdPartyPaymentsDataForSlipTransfer(CurrentEmployeeThirdPartyCategories.category_name,FromDate, ToDate).OrderBy(c => c.emp_id);
                    if (EmployeeThirdPartyPayments != null)
                        EmployeeThirdPartyPaymentsList = EmployeeThirdPartyPayments.ToList();
                //};
                //serviceClient.GetThirdPartyPaymentsDataForSlipTransferAsync(FromDate, ToDate);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Employee Third Party Payments Refresh Failed");
                //MessageBox.Show(ex.Message);
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
            z_Bank Bank = new z_Bank();
            Bank = serviceClient.GetBanks().Where(i => i.bank_id == CurrentCompanyBranch.bank_id).FirstOrDefault();


            try
            {
                int BANKCODE = 0;
                int BANKBRANCH = 0;
                string ACCOUNTNO = "";
                string COMPANYBRANCHACCOUNTNAME = "";
                DateTime FROMDATE;
                DateTime TODATE;
                string COMPANYBRANCH = "";

                string COMPANYBRANCHID = "";

                BANKCODE = Convert.ToInt16(Bank.bank_code);
                BANKBRANCH = Convert.ToInt16(serviceClient.GetBanckBranch().Where(i => i.branch_id == CurrentCompanyBranch.bank_branch_id).FirstOrDefault().bank_branch_code);
                ACCOUNTNO = CurrentCompanyBranch.account_no.ToString();
                COMPANYBRANCHACCOUNTNAME = CurrentCompanyBranch.account_name;
                FROMDATE = FromDate;
                TODATE = ToDate;
                COMPANYBRANCH = CurrentCompanyBranch.companyBranch_Name;
                COMPANYBRANCHID = CurrentCompanyBranch.companyBranch_id.ToString();

                if (BANKCODE >= 0 && BANKBRANCH >= 0 && EmployeeThirdPartyPaymentsList.Count() > 0)
                {
                    BankSelector select = new BankSelector(Bank.bank_name, EmployeeThirdPartyPaymentsList, FROMDATE, TODATE, BANKCODE, BANKBRANCH, ACCOUNTNO, COMPANYBRANCHACCOUNTNAME, COMPANYBRANCH, COMPANYBRANCHID);


                    select.CheckPath();
                    New();
                }

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        private void New()
        {
            CurrentCompanyBranch = null;
            CurrentEmployeeThirdPartyCategories = null;
            EmployeeThirdPartyPayments = null;
            EmployeeThirdPartyPaymentsList = null;
            RefreshCompanyBranches();
        }
        #endregion
    }
}
