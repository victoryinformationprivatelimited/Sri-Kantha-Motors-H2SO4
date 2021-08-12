using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP;
using ERP.ERPService;
using System.Windows.Input;

namespace ERP.Masters
{
    class EmployeeDeductionPaymentViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        List<z_CompanyBranches> AllCompanyBranches = new List<z_CompanyBranches>();
        List<mas_CompanyRule> AllCompanyRules = new List<mas_CompanyRule>();
        List<z_Bank> AllBanks = new List<z_Bank>();
        List<z_BankBranch> AllBankBranches = new List<z_BankBranch>();
        #endregion

        #region Constructor
        public EmployeeDeductionPaymentViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshBankBranches();
            RefreshBanks();
            RefreshEmployeeDeductionPayment();
            RefreshCompanyBranches();
            RefreshCompanyRules();
            CurrentEmployeeDeductionPayment = new EmployeeDeductionPaymentView();
        }
        #endregion

        #region Properties
        private IEnumerable<EmployeeDeductionPaymentView> _EmployeeDeductionPayments;

        public IEnumerable<EmployeeDeductionPaymentView> EmployeeDeductionPayments
        {
            get { return _EmployeeDeductionPayments; }
            set { _EmployeeDeductionPayments = value; OnPropertyChanged("EmployeeDeductionPayments"); }
        }

        private EmployeeDeductionPaymentView _CurrentEmployeeDeductionPayment;

        public EmployeeDeductionPaymentView CurrentEmployeeDeductionPayment
        {
            get { return _CurrentEmployeeDeductionPayment; }
            set { _CurrentEmployeeDeductionPayment = value; OnPropertyChanged("CurrentEmployeeDeductionPayment"); if (CurrentEmployeeDeductionPayment != null) SetValues(); }
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

        private IEnumerable<z_Bank> _Banks;

        public IEnumerable<z_Bank> Banks
        {
            get { return _Banks; }
            set { _Banks = value; OnPropertyChanged("Banks"); }
        }

        private z_Bank _CurrentBank;

        public z_Bank CurrentBank
        {
            get { return _CurrentBank; }
            set { _CurrentBank = value; OnPropertyChanged("CurrentBank"); if (CurrentBank != null) FilterBankBranches(); }
        }

        private IEnumerable<z_BankBranch> _BankBranches;

        public IEnumerable<z_BankBranch> BankBranches
        {
            get { return _BankBranches; }
            set { _BankBranches = value; OnPropertyChanged("BankBranches"); }
        }

        private z_BankBranch _CurrentBankBranch;

        public z_BankBranch CurrentBankBranch
        {
            get { return _CurrentBankBranch; }
            set { _CurrentBankBranch = value; OnPropertyChanged("CurrentBankBranch"); }
        }

        #endregion

        #region Refresh Methods

        public void RefreshEmployeeDeductionPayment()
        {
            try
            {
                serviceClient.GetEmployeeDeductionPaymentsCompleted += (s, e) =>
                    {
                        EmployeeDeductionPayments = e.Result.OrderBy(c => c.rule_name);
                    };
                serviceClient.GetEmployeeDeductionPaymentsAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void RefreshCompanyBranches()
        {
            try
            {
                serviceClient.GetCompanyBranchesCompleted += (s, e) =>
                    {
                        CompanyBranches = e.Result.Where(c => c.companyBranch_id == new Guid("fd18cfb6-ec4c-4098-b255-29a4c1aa8bf0")).OrderBy(c => c.companyBranch_Name);
                        if (CompanyBranches != null)
                            AllCompanyBranches = CompanyBranches.ToList();
                    };
                serviceClient.GetCompanyBranchesAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Company Branches Refresh Failed");
            }
        }

        public void RefreshCompanyRules()
        {
            try
            {
                serviceClient.GetCompanyCompleted += (s, e) =>
                    {
                        CompanyRules = e.Result.Where(c => c.deduction_id != new Guid("00000000-0000-0000-0000-000000000000"));
                        if (CompanyRules != null)
                            AllCompanyRules = CompanyRules.ToList();
                    };
                serviceClient.GetCompanyAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Company Rules Refresh Failed");
            }
        }

        public void RefreshBanks()
        {
            try
            {
                serviceClient.GetBanksCompleted += (s, e) =>
                {
                    Banks = e.Result.OrderBy(c => c.bank_name);
                    if (Banks != null)
                        AllBanks = Banks.ToList();
                };
                serviceClient.GetBanksAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Banks Refresh Failed");
            }
        }

        public void RefreshBankBranches()
        {
            try
            {
                AllBankBranches = serviceClient.GetBranches().ToList();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Bank Branches Refresh Failed");
            }
        }
        #endregion

        #region Commands & Methods

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New);
            }
        }

        public void SetValues()
        {
            CurrentCompanyBranch = null;
            CurrentCompanyBranch = AllCompanyBranches.FirstOrDefault(c => c.companyBranch_id == CurrentEmployeeDeductionPayment.companyBranch_id);
            CurrentCompanyRule = null;
            CurrentCompanyRule = AllCompanyRules.FirstOrDefault(c => c.rule_id == CurrentEmployeeDeductionPayment.deduction_rule_id);
            CurrentBank = null;
            CurrentBank = AllBanks.FirstOrDefault(c => c.bank_id == CurrentEmployeeDeductionPayment.bank_id);
            CurrentBankBranch = null;
            CurrentBankBranch = AllBankBranches.FirstOrDefault(c => c.branch_id == CurrentEmployeeDeductionPayment.branch_id);
        }

        public void Save()
        {
            try
            {
                
                    if (CurrentEmployeeDeductionPayment.deduction_payment_id == Guid.Empty)
                    {
                        if (clsSecurity.GetSavePermission(214))
                        {
                            if (ValidateSave())
                            {
                                mas_EmployeeDeductionPayment SaveEDP = new mas_EmployeeDeductionPayment();
                                SaveEDP.deduction_payment_id = Guid.NewGuid();
                                SaveEDP.companyBranch_id = CurrentCompanyBranch.companyBranch_id;
                                SaveEDP.deduction_rule_id = CurrentCompanyRule.rule_id;
                                SaveEDP.acc_no = CurrentEmployeeDeductionPayment.acc_no;
                                SaveEDP.acc_name = CurrentEmployeeDeductionPayment.acc_name;
                                SaveEDP.bank_id = CurrentBank.bank_id;
                                SaveEDP.branch_id = CurrentBankBranch.branch_id;
                                SaveEDP.is_active = CurrentEmployeeDeductionPayment.is_active;
                                SaveEDP.is_delete = false;
                                SaveEDP.save_user_id = clsSecurity.loggedUser.user_id;
                                SaveEDP.save_datetime = DateTime.Now;
                                if (serviceClient.SaveDeductionPayments(SaveEDP))
                                {
                                    New();
                                    clsMessages.setMessage("Employee Deduction Payment Save Completed");
                                }
                                else
                                    clsMessages.setMessage("Employee Deduction Payment Save Failed");
                            } 
                        }
                        else
                            clsMessages.setMessage("You don't have permission to save this record");
                    }
                    else
                    {
                        if (clsSecurity.GetUpdatePermission(214))
                        {
                            if (ValidateSave())
                            {
                                mas_EmployeeDeductionPayment UpdateEDP = new mas_EmployeeDeductionPayment();
                                UpdateEDP.deduction_payment_id = CurrentEmployeeDeductionPayment.deduction_payment_id;
                                UpdateEDP.companyBranch_id = CurrentCompanyBranch.companyBranch_id;
                                UpdateEDP.deduction_rule_id = CurrentCompanyRule.rule_id;
                                UpdateEDP.acc_no = CurrentEmployeeDeductionPayment.acc_no;
                                UpdateEDP.acc_name = CurrentEmployeeDeductionPayment.acc_name;
                                UpdateEDP.bank_id = CurrentBank.bank_id;
                                UpdateEDP.branch_id = CurrentBankBranch.branch_id;
                                UpdateEDP.is_active = CurrentEmployeeDeductionPayment.is_active;
                                UpdateEDP.is_delete = false;
                                UpdateEDP.modified_user_id = clsSecurity.loggedUser.user_id;
                                UpdateEDP.modified_datetime = DateTime.Now;
                                if (serviceClient.UpdateDeductionPayments(UpdateEDP))
                                {
                                    New();
                                    clsMessages.setMessage("Employee Deduction Payment Update Completed");
                                }
                                else
                                    clsMessages.setMessage("Employee Deduction Payment Update Failed");
                            } 
                        }
                        else
                            clsMessages.setMessage("You don't have permission to update this record");
                    }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error Employee Deduction Payment Saving/Updating");
            }
        }

        public void New()
        {
            EmployeeDeductionPayments = null;
            CompanyBranches = null;
            CompanyRules = null;
            Banks = null;
            CurrentEmployeeDeductionPayment = new EmployeeDeductionPaymentView();
            CurrentCompanyBranch = new z_CompanyBranches();
            CurrentCompanyRule = new mas_CompanyRule();
            CurrentBank = new z_Bank();
            RefreshEmployeeDeductionPayment();
            RefreshCompanyBranches();
            RefreshCompanyRules();
            RefreshBanks();
        }

        public void Delete()
        {
            try
            {
                if (clsSecurity.GetDeletePermission(214))
                {
                    if (CurrentEmployeeDeductionPayment != null)
                    {
                        mas_EmployeeDeductionPayment DeleteEDP = new mas_EmployeeDeductionPayment();
                        DeleteEDP.deduction_payment_id = CurrentEmployeeDeductionPayment.deduction_payment_id;
                        DeleteEDP.is_delete = true;
                        DeleteEDP.delete_user_id = clsSecurity.loggedUser.user_id;
                        DeleteEDP.delete_datetime = DateTime.Now;
                        if (serviceClient.DeleteDeductionPayments(DeleteEDP))
                        {
                            New();
                            clsMessages.setMessage("Employee Deduction Payment Delete Completed");
                        }
                        else
                            clsMessages.setMessage("Employee Deduction Payment Delete Failed");
                    }
                    else
                        clsMessages.setMessage("Please Select a Deduction Payment to Delete"); 
                }
                else
                    clsMessages.setMessage("You don't have permission to delete this record");
            }
            catch (Exception)
            {
                clsMessages.setMessage("Error Employee Deduction Payment Deleting");
            }
        }

        private void FilterBankBranches()
        {
            BankBranches = AllBankBranches.Where(c => c.bank_id == CurrentBank.bank_id);
        }

        private bool ValidateSave()
        {
            if (CurrentCompanyBranch == null)
            {
                clsMessages.setMessage("Please Select a Company Branch");
                return false;
            }
            else if (CurrentCompanyRule == null)
            {
                clsMessages.setMessage("Please Select a Deduction Rule");
                return false;
            }
            else if (CurrentEmployeeDeductionPayment.acc_no == null || CurrentEmployeeDeductionPayment.acc_no == "")
            {
                clsMessages.setMessage("Please Enter an Account Number");
                return false;
            }
            else if (CurrentEmployeeDeductionPayment.acc_name == null || CurrentEmployeeDeductionPayment.acc_name == "")
            {
                clsMessages.setMessage("Please Enter an Account Name");
                return false;
            }
            else if (CurrentBank == null)
            {
                clsMessages.setMessage("Please Select a Bank Name");
                return false;
            }
            else if (CurrentBankBranch == null)
            {
                clsMessages.setMessage("Please Select a Bank Branch Name");
                return false;
            }
            else
                return true;
        }
        #endregion
    }
}
