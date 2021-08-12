using CustomBusyBox;
using ERP.ERPService;
using ERP.Payroll.SlipTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Payroll.Excel_Sheet_Windows
{
    class ExcelUploadedSlipTransferViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        List<ExcelUploadedSlipTransferView> ExcelUploadedDataList = new List<ExcelUploadedSlipTransferView>();
        #endregion

        #region Constructor
        public ExcelUploadedSlipTransferViewModel()
        {
            serviceClient = new ERPServiceClient();
            SelectDate = DateTime.Now.Date;
            RefreshPeriods();
            RefreshCompanyBranches();
        }
        #endregion

        #region Properties
        private IEnumerable<ExcelUploadedSlipTransferView> _ExcelUploadedData;

        public IEnumerable<ExcelUploadedSlipTransferView> ExcelUploadedData
        {
            get { return _ExcelUploadedData; }
            set { _ExcelUploadedData = value; OnPropertyChanged("ExcelUploadedData"); }
        }

        private ExcelUploadedSlipTransferView _CurrentExcelUploadedData;

        public ExcelUploadedSlipTransferView CurrentExcelUploadedData
        {
            get { return _CurrentExcelUploadedData; }
            set { _CurrentExcelUploadedData = value; OnPropertyChanged("CurrentExcelUploadedData"); }
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
                serviceClient.GetExcelSlipTransferDataCompleted += (s, e) =>
                {
                    ExcelUploadedData = e.Result.OrderBy(c => c.emp_id);
                    if (ExcelUploadedData != null)
                        ExcelUploadedDataList = ExcelUploadedData.ToList();
                };
                serviceClient.GetExcelSlipTransferDataAsync(CurrentPeriod.period_id);
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

        public void RefreshCompanyBranches()
        {
            try
            {
                serviceClient.GetCompanyBranchesCompleted += (s, e) =>
                {
                    CompanyBranches = e.Result.OrderBy(c => c.companyBranch_Name);
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
            BusyBox.ShowBusy("Please Wait Untill Generate Completed...");
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
                if (BANKCODE >= 0 && BANKBRANCH >= 0 && ExcelUploadedDataList.Count() > 0)
                {
                    BankSelector select = new BankSelector(Bank.bank_name, ExcelUploadedDataList, SelectDate, BANKCODE, BANKBRANCH, ACCOUNTNO, COMPANYBRANCHACCOUNTNAME, PAYPERIOD, COMPANYBRANCH, COMPANYBRANCHID, PERIODID);
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
            BusyBox.ShowBusy("Please Wait...");
            CurrentCompanyBranch = null;
            CurrentPeriod = null;
            ExcelUploadedData = null;
            ExcelUploadedDataList = null;
            RefreshCompanyBranches();
            RefreshPeriods();
            BusyBox.CloseBusy();
        }
        #endregion
    }
}