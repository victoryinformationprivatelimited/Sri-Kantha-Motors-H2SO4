using ERP.ERPService;
using ERP.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.Basic_Masters
{
    class LoansViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        List<z_LoanCatergories> AllLoanCategories;

        #endregion

        #region Constructor
        public LoansViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllLoanCategories = new List<z_LoanCatergories>();
            New();
        }

        #endregion

        #region Properties

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
            set { _CurrentLoan = value; OnPropertyChanged("CurrentLoan"); if (CurrentLoan != null) { GetMindurationType(); GetMaxdurationType(); GetEmpServiceType(); GetGurantorServiceType(); SetLoanCategory(); } }
        }

        private IEnumerable<z_LoanCatergories> _LoanCategories;
        public IEnumerable<z_LoanCatergories> LoanCategories
        {
            get { return _LoanCategories; }
            set { _LoanCategories = value; OnPropertyChanged("LoanCategories"); }
        }

        private z_LoanCatergories _CurrentLoanCategory;
        public z_LoanCatergories CurrentLoanCategory
        {
            get { return _CurrentLoanCategory; }
            set { _CurrentLoanCategory = value; OnPropertyChanged("CurrentLoanCategory"); }
        }

        private int _EmpServiceType;
        public int EmpServiceType
        {
            get { return _EmpServiceType; }
            set { _EmpServiceType = value; OnPropertyChanged("EmpServiceType"); }
        }

        private int _GurantorServiceType;
        public int GurantorServiceType
        {
            get { return _GurantorServiceType; }
            set { _GurantorServiceType = value; OnPropertyChanged("GurantorServiceType"); }
        }

        private int _MinDurationType;
        public int MinDurationType
        {
            get { return _MinDurationType; }
            set { _MinDurationType = value; OnPropertyChanged("MinDurationType"); }
        }

        private int _MaxDurationType;
        public int MaxDurationType
        {
            get { return _MaxDurationType; }
            set { _MaxDurationType = value; OnPropertyChanged("MaxDurationType"); }
        }

        private bool _ISGurantorEnabled;
        public bool ISGurantorEnabled
        {
            get { return _ISGurantorEnabled; }
            set { _ISGurantorEnabled = value; OnPropertyChanged("ISGurantorEnabled"); }
        }

        private string _Search;

        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null)FilterLoans(); }
        }
        


        #endregion

        #region Refresh Method

        private void RefreshLoans()
        {
            serviceClient.GetLoansWithRulesCompleted += (s, e) =>
                {
                    Loans = e.Result;
                };
            serviceClient.GetLoansWithRulesAsync();
        }
        private void RefreshLoanCategories()
        {
            serviceClient.GetLoanCatergoriesCompleted += (s, e) =>
                {
                    LoanCategories = e.Result;
                    if (LoanCategories != null && LoanCategories.Count() > 0)
                        AllLoanCategories = LoanCategories.ToList();
                };
            serviceClient.GetLoanCatergoriesAsync();
        }

        #endregion

        #region Button Commands

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }



        #endregion

        #region Methods

        private void GetMindurationType()
        {
            if (CurrentLoan.minimum_time_duration_type == null)
            {
                MinDurationType = -1;
            }
            else if (CurrentLoan.minimum_time_duration_type == "Y")
            {
                MinDurationType = 0;
            }
            else if (CurrentLoan.minimum_time_duration_type == "M")
            {
                MinDurationType = 1;
            }
            else
                MinDurationType = 2;
        }
        private void GetMaxdurationType()
        {
            if (CurrentLoan.maximum_time_duration_type == null)
            {
                MaxDurationType = -1;
            }
            else if (CurrentLoan.maximum_time_duration_type == "Y")
            {
                MaxDurationType = 0;
            }
            else if (CurrentLoan.maximum_time_duration_type == "M")
            {
                MaxDurationType = 1;
            }
            else
                MaxDurationType = 2;
        }
        private void GetEmpServiceType()
        {
            if (CurrentLoan.Emp_MinServicePeriod_Type == null)
            {
                EmpServiceType = -1;
            }
            else if (CurrentLoan.Emp_MinServicePeriod_Type == "Y")
            {
                EmpServiceType = 0;
            }
            else if (CurrentLoan.Emp_MinServicePeriod_Type == "M")
            {
                EmpServiceType = 1;
            }
            else
                EmpServiceType = 2;
        }
        private void GetGurantorServiceType()
        {
            if (CurrentLoan.Guarantor_MinServicePeriod_Type == null)
            {
                GurantorServiceType = -1;
            }
            else if (CurrentLoan.Guarantor_MinServicePeriod_Type == "Y")
            {
                GurantorServiceType = 0;
            }
            else if (CurrentLoan.Guarantor_MinServicePeriod_Type == "M")
            {
                GurantorServiceType = 1;
            }
            else
                GurantorServiceType = 2;
        }
        private void Save()
        {
            if (clsSecurity.GetSavePermission(602))
            {
                if (ValidateSave())
                {
                    MessageBoxResult result = new MessageBoxResult();
                    result = MessageBox.Show("Do You Want To Delete This Record...?", "Loan Module Says", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        LoanWithRulesView TempLoanViewWithRules = new LoanWithRulesView();
                        TempLoanViewWithRules.loan_id = Guid.NewGuid();
                        TempLoanViewWithRules.loan_no = CurrentLoan.loan_no;
                        TempLoanViewWithRules.loan_name = CurrentLoan.loan_name;
                        TempLoanViewWithRules.loan_Catergory_id = CurrentLoanCategory.loan_Catergory_id;
                        TempLoanViewWithRules.maximum_amount = CurrentLoan.maximum_amount;
                        TempLoanViewWithRules.minimum_amount = CurrentLoan.minimum_amount;
                        TempLoanViewWithRules.minimum_time_duration_period = CurrentLoan.minimum_time_duration_period;
                        if (MinDurationType == 0)
                        {
                            TempLoanViewWithRules.minimum_time_duration_type = "Y";
                        }
                        else if (MinDurationType == 1)
                            TempLoanViewWithRules.minimum_time_duration_type = "M";
                        TempLoanViewWithRules.maximum_time_duration_period = CurrentLoan.maximum_time_duration_period;
                        if (MaxDurationType == 0)
                        {
                            TempLoanViewWithRules.maximum_time_duration_type = "Y";
                        }
                        else if (MaxDurationType == 1)
                            TempLoanViewWithRules.maximum_time_duration_type = "M";
                        TempLoanViewWithRules.save_datetime = DateTime.Now;
                        TempLoanViewWithRules.save_user_id = clsSecurity.loggedUser.user_id;
                        TempLoanViewWithRules.is_delete = false;
                        TempLoanViewWithRules.is_active = true;
                        TempLoanViewWithRules.default_rate = CurrentLoan.default_rate;
                        TempLoanViewWithRules.Emp_DeductionPercentageOfPayroll = (int)CurrentLoan.Emp_DeductionPercentageOfPayroll;
                        TempLoanViewWithRules.Emp_MaxguaranteedLoans = (int)CurrentLoan.Emp_MaxguaranteedLoans;
                        TempLoanViewWithRules.Emp_MinServicePeriod = (int)CurrentLoan.Emp_MinServicePeriod;
                        if (EmpServiceType == 0)
                        {
                            TempLoanViewWithRules.Emp_MinServicePeriod_Type = "Y";
                        }
                        else if (EmpServiceType == 1)
                            TempLoanViewWithRules.Emp_MinServicePeriod_Type = "M";
                        TempLoanViewWithRules.Emp_NoLoansInProgress = (int)CurrentLoan.Emp_NoLoansInProgress;
                        TempLoanViewWithRules.Emp_NoOfGurantors = (int)CurrentLoan.Emp_NoOfGurantors;
                        TempLoanViewWithRules.Emp_Permanent = (bool)CurrentLoan.Emp_Permanent;
                        TempLoanViewWithRules.Guarantor_DeductionPercentageinPayroll = CurrentLoan.Guarantor_DeductionPercentageinPayroll;
                        TempLoanViewWithRules.Guarantor_MaxguaranteedLoans = CurrentLoan.Guarantor_MaxguaranteedLoans;
                        TempLoanViewWithRules.Guarantor_MinServicePeriod = CurrentLoan.Guarantor_MinServicePeriod;
                        if (GurantorServiceType == 0)
                        {
                            TempLoanViewWithRules.Guarantor_MinServicePeriod_Type = "Y";
                        }
                        else if (GurantorServiceType == 1)
                            TempLoanViewWithRules.Guarantor_MinServicePeriod_Type = "M";
                        TempLoanViewWithRules.Guarantor_NoLoansInProgress = CurrentLoan.Guarantor_NoLoansInProgress;
                        TempLoanViewWithRules.Guarantor_Permanent = CurrentLoan.Guarantor_Permanent;

                        if (serviceClient.SaveCreateLoan(TempLoanViewWithRules))
                        {
                            New();
                            clsMessages.setMessage("Loan Has Been Saved Successfully....");
                        }
                        else
                            clsMessages.setMessage("Loan Saved Process Failed....");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to save this record");
        }
        private bool ValidateSave()
        {
            if (CurrentLoan.loan_id != Guid.Empty)
            {
                clsMessages.setMessage("You Cannot Update Loan Details");
                return false;
            }
            else if (CurrentLoanCategory.loan_Catergory_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Loan Category...");
                return false;
            }
            else if (CurrentLoan.loan_name == null)
            {
                clsMessages.setMessage("Please Enter Loan Name...");
                return false;
            }
            else if (CurrentLoan.minimum_amount == null)
            {
                clsMessages.setMessage("Please Enter Loan Minimum Amount...");
                return false;
            }
            else if (CurrentLoan.maximum_amount == 0)
            {
                clsMessages.setMessage("Please Enter Loan Maximum Amount...");
                return false;
            }
            else if (CurrentLoan.minimum_amount > CurrentLoan.maximum_amount)
            {
                clsMessages.setMessage("Please Enter Loan Minimum Amount Less than Loan Maximum Amount...");
                CurrentLoan.minimum_amount = 0;
                CurrentLoan.maximum_amount = 0;
                return false;
            }
            else if (CurrentLoan.default_rate == null)
            {
                clsMessages.setMessage("Please Enter Loan Intrest Amount...");
                return false;
            }
            else if (CurrentLoan.minimum_time_duration_period == 0)
            {
                clsMessages.setMessage("Please Enter Loan Minimum Duration...");
                return false;
            }
            else if (MinDurationType == -1)
            {
                clsMessages.setMessage("Please Select Minimum Time Duration Type...");
                return false;
            }
            else if (CurrentLoan.maximum_time_duration_period == 0)
            {
                clsMessages.setMessage("Please Enter Loan Maximum Duration...");
                return false;
            }
            else if (MaxDurationType == -1)
            {
                clsMessages.setMessage("Please Select Maximum Time Duration Type...");
                return false;
            }
            else if (CalculateMinimumTimeDuration() > CalculateMaximumTimeDuration())
            {
                clsMessages.setMessage("Please Enter Loan Minimum Duration Less Than Maximum Time Duration...");
                return false;
            }
            else if (CurrentLoan.Emp_NoLoansInProgress == null)
            {
                clsMessages.setMessage("Please Enter Number of Loans in Progress for Loan Applicant...");
                return false;
            }
            else if (CurrentLoan.Emp_MinServicePeriod == 0 || CurrentLoan.Emp_MinServicePeriod == null)
            {
                clsMessages.setMessage("Please Enter Minimum Service Period for Loan Applicant...");
                return false;
            }
            else if (EmpServiceType == -1)
            {
                clsMessages.setMessage("Please Select A service Period Duration Type...");
                return false;
            }
            else if (CurrentLoan.Emp_NoOfGurantors == null)
            {
                clsMessages.setMessage("Please Enter How Many Loan Gurantors Should Be There For this Loan. If No Gurantors Needed Then Enter '0'...");
                return false;
            }
            // else if (CurrentLoan.Emp_DeductionPercentageOfPayroll == 0 || CurrentLoan.Emp_DeductionPercentageOfPayroll == null)
            else if (CurrentLoan.Emp_DeductionPercentageOfPayroll == null)
            {
                clsMessages.setMessage("Please Enter Deduction Percentage of Payroll For Loan Applicant..");
                return false;
            }
            else if (CurrentLoan.Emp_MaxguaranteedLoans == null)
            {
                clsMessages.setMessage("Please Enter Maximum Number of Loans Allowed For Loan Applicant. If it's Not Requied Please Enter '0'...");
                return false;
            }
            else if (CurrentLoan.Emp_Permanent == null)
            {
                clsMessages.setMessage("Please Select If The Applicant Should Be Perment or Not...");
                return false;
            }
            else if (ISGurantorEnabled == false && CurrentLoan.Emp_NoOfGurantors > 0)
            {
                clsMessages.setMessage("Please Enable 'IS Gurantor Needed' Checkbox.. Because Gurantors Needed To this Loan is More Than '0'...");
                return false;
            }
            else if (ISGurantorEnabled == true)
            {
                if (CurrentLoan.Guarantor_NoLoansInProgress == null)
                {
                    clsMessages.setMessage("Please Enter Number Of Loans in Process For Loan Gurantor. If it's Not Requied Please Enter '0'...");
                    return false;
                }
                else if (CurrentLoan.Guarantor_MinServicePeriod == 0)
                {
                    clsMessages.setMessage("Please Enter Minimum Number Of Service Period For Loan Gurantor....");
                    return false;
                }
                else if (GurantorServiceType == -1)
                {
                    clsMessages.setMessage("Please Select Service Period Duration Type For Loan Gurantor....");
                    return false;
                }
                else if (CurrentLoan.Guarantor_MaxguaranteedLoans == null)
                {
                    clsMessages.setMessage("Please Enter Maximum Number Of Loans Guranteed by the Gurantor. If it's Not Requied Please Enter '0'...");
                    return false;
                }
                // else if (CurrentLoan.Guarantor_DeductionPercentageinPayroll == 0 || CurrentLoan.Guarantor_DeductionPercentageinPayroll == null)
                else if (CurrentLoan.Guarantor_DeductionPercentageinPayroll == null)
                {
                    clsMessages.setMessage("Please Enter Deduction Percentage of Payroll For Gurantor..");
                    return false;
                }
                else if (CurrentLoan.Guarantor_Permanent == null)
                {
                    clsMessages.setMessage("Please Select If The Gurantor Should Be Perment or Not...");
                    return false;
                }
                return true;
            }
            else
                return true;
        }
        private int CalculateMinimumTimeDuration()
        {
            int TempMinDutation = 0;
            if (MinDurationType == 1)
            {
                TempMinDutation = CurrentLoan.minimum_time_duration_period;
            }
            else if (MinDurationType == 0)
            {
                TempMinDutation = CurrentLoan.minimum_time_duration_period * 12;
            }
            return TempMinDutation;
        }
        private int CalculateMaximumTimeDuration()
        {
            int TempMaxDuration = 0;
            if (MaxDurationType == 1)
            {
                TempMaxDuration = CurrentLoan.maximum_time_duration_period;
            }
            else if (MaxDurationType == 0)
            {
                TempMaxDuration = CurrentLoan.maximum_time_duration_period * 12;
            }
            return TempMaxDuration;
        }
        private void New()
        {
            RefreshLoans();
            RefreshLoanCategories();
            Loans = null;
            CurrentLoan = new LoanWithRulesView();
            CurrentLoanCategory = new z_LoanCatergories();
            CurrentLoan.loan_no = serviceClient.GetLastLoanNo();
            ISGurantorEnabled = true;
        }
        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(602))
            {
                MessageBoxResult result = new MessageBoxResult();
                result = MessageBox.Show("Do You Want To Delete This Record...?", "Loan Module Says", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (ValidateDelete())
                {
                    if (result == MessageBoxResult.Yes)
                    {
                        LoanWithRulesView temp = new LoanWithRulesView();
                        temp.loan_id = CurrentLoan.loan_id;
                        temp.is_active = false;
                        temp.is_delete = true;
                        temp.delete_datetime = DateTime.Now;
                        temp.delete_user_id = clsSecurity.loggedUser.user_id;
                        serviceClient.DeleteLoan(temp);
                        New();
                        clsMessages.setMessage("Successfully Deleted...");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to delete this record");
        }
        private bool ValidateDelete()
        {
            if (CurrentLoan.loan_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Loan To Delete...");
                return false;
            }
            else
                return true;
        }
        private void SetLoanCategory()
        {
            CurrentLoanCategory = null;
            CurrentLoanCategory = AllLoanCategories.FirstOrDefault(c => c.loan_Catergory_id == CurrentLoan.loan_Catergory_id);
        }

        private void FilterLoans()
        {
            Loans = Loans.Where(c => c.loan_name != null && c.loan_name.ToUpper().Contains(Search.ToUpper()));
        }

        #endregion

    }
}
