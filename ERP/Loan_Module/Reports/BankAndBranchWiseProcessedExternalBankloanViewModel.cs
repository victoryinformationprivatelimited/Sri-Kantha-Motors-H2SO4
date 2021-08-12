using ERP.ERPService;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.Reports
{
    class BankAndBranchWiseProcessedExternalBankloanViewModel : ViewModelBase
    {
        #region Fields

        private ERPServiceClient serviceClient;
        List<z_BankBranch> AllBankBranch;
        List<z_Bank> AllBank;
        
        #endregion

        #region Constructor
        public BankAndBranchWiseProcessedExternalBankloanViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllBankBranch = new List<z_BankBranch>();
            AllBank = new List<z_Bank>();
            EmpDetailsVisibility = Visibility.Hidden;
            RefreshBankBranch();
            RefreshPeriods();
            RefreshBank();
        }
        
        #endregion

        #region Properties

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
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod");}
        }

        private IEnumerable<z_Bank> _Bank;
        public IEnumerable<z_Bank> Bank
        {
            get { return _Bank; }
            set { _Bank = value; OnPropertyChanged("Bank"); }
        }

        private z_Bank _CurrentBank;
        public z_Bank CurrentBank
        {
            get { return _CurrentBank; }
            set { _CurrentBank = value; OnPropertyChanged("CurrentBank"); if (CurrentBank != null)FilterBrankBranchByBank(); }
        }

        private IEnumerable<z_BankBranch> _BankBranch;
        public IEnumerable<z_BankBranch> BankBranch
        {
            get { return _BankBranch; }
            set { _BankBranch = value; OnPropertyChanged("BankBranch"); }
        }

        private z_BankBranch _CurrentBankBranch;
        public z_BankBranch CurrentBankBranch
        {
            get { return _CurrentBankBranch; }
            set { _CurrentBankBranch = value; OnPropertyChanged("CurrentBankBranch"); }
        }

        private Visibility _EmpDetailsVisibility;
        public Visibility EmpDetailsVisibility
        {
            get { return _EmpDetailsVisibility; }
            set { _EmpDetailsVisibility = value; OnPropertyChanged("EmpDetailsVisibility"); }
        }
        
        #endregion

        #region Refresh Methods
        private void RefreshPeriods()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result;
            };
            serviceClient.GetPeriodsAsync();
        }
        private void RefreshBank()
        {
            serviceClient.GetBanksCompleted += (s, e) =>
            {
                Bank = e.Result;
                if (Bank != null)
                    AllBank = Bank.ToList();

            };
            serviceClient.GetBanksAsync();
        }
        private void RefreshBankBranch()
        {
            AllBankBranch.Clear();
            serviceClient.GetBanckBranchCompleted += (s, e) =>
            {
                AllBankBranch = e.Result.ToList();
            };
            serviceClient.GetBanckBranchAsync();
        }
        
        #endregion

        #region Button Commands
        public ICommand BTNPrint
        {
            get { return new RelayCommand(Print); }
        }
        
        #endregion

        #region Methods
        private void FilterBrankBranchByBank()
        {
            BankBranch = null;
            BankBranch = AllBankBranch.Where(c => c.bank_id == CurrentBank.bank_id);
        }
        private void Print()
        {       
            string path = "";
            try
            {
                path = "\\Loan_Module\\Reports\\BankAndBranchWiseProcessedExternalBankloan";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@EmployeeID", string.Empty);
                print.setParameterValue("@PeriodID", CurrentPeriod == null ? string .Empty : CurrentPeriod.period_id.ToString());
                print.setParameterValue("@BankID", CurrentBank == null ? string.Empty : CurrentBank.bank_id.ToString());
                print.setParameterValue("@BankBranchID", CurrentBankBranch == null ? string.Empty : CurrentBankBranch.branch_id.ToString());
                print.LoadToReportViewer();

            }
            catch (Exception ex)
            {
                clsMessages.setMessage("Report loading is failed: " + ex.Message);
            }
        }

        #endregion
    }
}
