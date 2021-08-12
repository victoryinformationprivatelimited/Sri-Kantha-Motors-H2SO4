using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace ERP.Medical
{
    class EmployeeMedicalViewModel : ViewModelBase
    {
        #region Services Object

        ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion Services Object

        #region Constructor

        public EmployeeMedicalViewModel()
        {
            
                refreshEmployees();
                New();
        }

        #endregion Constructor

        #region Properties

        private IEnumerable<mas_Employee> _Employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); refreshEmployeeAllocation(); }
        }

        private IEnumerable<EmployeeMedicalAllocationView> _CurrentEmployeeAllAllocations;
        public IEnumerable<EmployeeMedicalAllocationView> CurrentEmployeeAllAllocations
        {
            get { return _CurrentEmployeeAllAllocations; }
            set { _CurrentEmployeeAllAllocations = value; OnPropertyChanged("CurrentEmployeeAllAllocations"); }
        }

        private EmployeeMedicalAllocationView _CurrentAllocation;
        public EmployeeMedicalAllocationView CurrentAllocation
        {
            get { return _CurrentAllocation; }
            set { _CurrentAllocation = value; OnPropertyChanged("CurrentAllocation"); refreshEmployeeAllocationBalance(); refreshEmployeeMedicalClaims(); }
        }
        
        private decimal _CurrentAllocationBalance;
        public decimal CurrentAllocationBalance
        {
            get { return _CurrentAllocationBalance; }
            set { _CurrentAllocationBalance = value; OnPropertyChanged("CurrentAllocationBalance"); }
        }

        private EmployeeMedicalSummaryView _CurrentAllocationSummary;
        public EmployeeMedicalSummaryView CurrentAllocationSummary
        {
            get { return _CurrentAllocationSummary; }
            set { _CurrentAllocationSummary = value; OnPropertyChanged("CurrentAllocationSummary");  }
        }
        
        private IEnumerable<trn_EmployeeMedical> _EmployeeMedicalClaims;
        public IEnumerable<trn_EmployeeMedical> EmployeeMedicalClaims
        {
            get { return _EmployeeMedicalClaims; }
            set { _EmployeeMedicalClaims = value; OnPropertyChanged("EmployeeMedicalClaims"); }
        }

        private trn_EmployeeMedical _CurrentEmployeeMedicalClaim;
        public trn_EmployeeMedical CurrentEmployeeMedicalClaim
        {
            get { return _CurrentEmployeeMedicalClaim; }
            set { _CurrentEmployeeMedicalClaim = value; OnPropertyChanged("CurrentEmployeeMedicalClaim");  }
        }               

        #endregion Properties

        #region New Method

        void New()
        {
            //if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.MedicalReimbursement), clsSecurity.loggedUser.user_id))
            //{
                this.CurrentEmployeeMedicalClaim = null;
                CurrentEmployeeMedicalClaim = new trn_EmployeeMedical();
                CurrentEmployeeMedicalClaim.emp_med_id = Guid.NewGuid();
            //}
        }

        #endregion

        #region NewButton Class & Property

        bool newCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        #endregion

        #region Save Method

        void Save()
        {
            //if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.MedicalReimbursement), clsSecurity.loggedUser.user_id))
            //{

                bool IsUpdate = false;

                trn_EmployeeMedical newEmployeeMedical = new trn_EmployeeMedical();

                newEmployeeMedical.emp_med_id = CurrentEmployeeMedicalClaim.emp_med_id;
                newEmployeeMedical.period_id = CurrentAllocation.period_id;
                newEmployeeMedical.cat_id = CurrentAllocation.cat_id;
                newEmployeeMedical.employee_id = CurrentEmployee.employee_id;

                newEmployeeMedical.doctor_name = CurrentEmployeeMedicalClaim.doctor_name;
                newEmployeeMedical.pharmacy_name = CurrentEmployeeMedicalClaim.pharmacy_name;
                newEmployeeMedical.receipt_amount = CurrentEmployeeMedicalClaim.receipt_amount;
                newEmployeeMedical.receipt_date = CurrentEmployeeMedicalClaim.receipt_date;

                newEmployeeMedical.is_active = CurrentEmployeeMedicalClaim.is_active;

                IEnumerable<trn_EmployeeMedical> allClaims = this.serviceClient.GetEmployeeMedicalClaims();

                if (allClaims != null)
                {
                    foreach (var value in allClaims)
                    {
                        if (value.emp_med_id == CurrentEmployeeMedicalClaim.emp_med_id)
                        {
                            IsUpdate = true;
                            break;
                        }
                    }
                }

                if (newEmployeeMedical != null && newEmployeeMedical.period_id != null)
                {
                    if (IsUpdate)
                    {
                        if (clsSecurity.GetUpdatePermission(704))
                        {
                            if (this.serviceClient.UpdateMedicalClaim(newEmployeeMedical))
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateFail);
                            }
                        }
                        else
                            clsMessages.setMessage("You Don't Have Permission to Update in this Form...");
                    }
                    else
                    {
                        newEmployeeMedical.is_active = true;
                        newEmployeeMedical.is_delete = false;

                        if (clsSecurity.GetSavePermission(704))
                        {
                            if (this.serviceClient.SaveEmployeeMedicalClaim(newEmployeeMedical))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            }
                        }
                        else
                            clsMessages.setMessage("You Don't Have Permission to Save in this Form...");
                    }

                    refreshEmployeeMedicalClaims();
                    refreshEmployeeAllocationBalance();
                }
            //}
        }

        #endregion

        #region SaveButton Class & Property

        bool saveCanExecute()
        {
            if (CurrentAllocation != null && CurrentEmployeeMedicalClaim != null)
            {
                if (CurrentAllocation.from_date == null)
                    return false;
                if (CurrentAllocation.to_date == null)
                    return false;
                if (!(CurrentEmployeeMedicalClaim.receipt_date >= CurrentAllocation.from_date && CurrentEmployeeMedicalClaim.receipt_date <= CurrentAllocation.to_date))
                    return false;
                if (!(CurrentEmployeeMedicalClaim.receipt_amount <= CurrentAllocationBalance))
                    return false;
                if (!(DateTime.Now >= CurrentAllocation.from_date))
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }

        #endregion

        #region Refresh Methods

        private void refreshEmployees()
        {
            this.serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                this.Employees = e.Result;
            };
            this.serviceClient.GetEmployeesAsync();
        }

        private void refreshEmployeeAllocation()
        {
            this.serviceClient.GetMedicalCategoryAllocationByEmpIdCompleted += (s, e) =>
            {
                this.CurrentEmployeeAllAllocations = e.Result;
            };
            this.serviceClient.GetMedicalCategoryAllocationByEmpIdAsync(CurrentEmployee.employee_id);
        }

        private void refreshEmployeeAllocationBalance()
        {
            if (CurrentAllocation != null)
            {
                this.serviceClient.GetMedicalSummaryByKeyCompleted += (s, e) =>
                {
                    this.CurrentAllocationSummary = e.Result;

                    if (CurrentAllocationSummary != null)
                        CurrentAllocationBalance = Convert.ToDecimal(CurrentAllocationSummary.total_amount - CurrentAllocationSummary.used_amount);
                    else
                        CurrentAllocationBalance = Convert.ToDecimal(CurrentAllocation.total_amount);

                };
                this.serviceClient.GetMedicalSummaryByKeyAsync(CurrentAllocation.period_id, CurrentEmployee.employee_id, CurrentAllocation.cat_id);
            }
            else
            {
                this.CurrentAllocationSummary = null;
                this.CurrentAllocationBalance = decimal.Zero;
            }

        }

        private void refreshEmployeeMedicalClaims()
        {
            if (CurrentAllocation != null && CurrentEmployee != null)
            {
                this.serviceClient.GetEmployeeMedicalClaimsByKeyCompleted += (s, e) =>
                {
                    this.EmployeeMedicalClaims = e.Result;
                };
                this.serviceClient.GetEmployeeMedicalClaimsByKeyAsync(CurrentAllocation.period_id, CurrentEmployee.employee_id, CurrentAllocation.cat_id);
            }

            else
            {
                this.EmployeeMedicalClaims = null;
            }
        }

        #endregion Refresh Methods
    }
}
