using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows;
using System.Windows.Input;
using ERP.BasicSearch;


namespace ERP.Payroll.RI_Allowance
{
    class EmployeeAllowanceViewModel : ViewModelBase
    {
        #region Service Client

        ERPServiceClient serviceClient;
        #endregion

        #region Constructor

        public EmployeeAllowanceViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshAllowanceTypes();
            New();
        }
        #endregion

        #region Properties

        private IEnumerable<mas_Allowance> allowanceTypes;
        public IEnumerable<mas_Allowance> AllowanceTypes
        {
            get { return allowanceTypes; }
            set { allowanceTypes = value; OnPropertyChanged("AllowanceTypes"); }
        }

        private mas_Allowance currentAllowanceType;
        public mas_Allowance CurrentAllowanceType
        {
            get { return currentAllowanceType; }
            set
            {
                currentAllowanceType = value; OnPropertyChanged("CurrentAllowanceType");
                if (CurrentAllowance != null && CurrentAllowance.amount == null) CurrentAllowance.amount = CurrentAllowanceType.default_amount;
            }
        }

        private IEnumerable<EmployeeAllowanceView> allowances;
        public IEnumerable<EmployeeAllowanceView> Allowances
        {
            get { return allowances; }
            set { allowances = value; OnPropertyChanged("Allowances"); }
        }

        private EmployeeAllowanceView currentAllowance;
        public EmployeeAllowanceView CurrentAllowance
        {
            get { return currentAllowance; }
            set { currentAllowance = value; OnPropertyChanged("CurrentAllowance"); }
        }

        private EmployeeSearchView selectedEmployee;
        public EmployeeSearchView SelectedEmployee
        {
            get { return selectedEmployee; }
            set { selectedEmployee = value; }
        }

        #endregion

        #region Refresh Methods

        private void RefreshAllowances()
        {
            try
            {
                serviceClient.GetEmployeeAllowancesCompleted += (s, e) =>
                {
                    Allowances = e.Result;
                };

                serviceClient.GetEmployeeAllowancesAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Allowances refresh failed", "Refresh Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshAllowanceTypes()
        {
            try
            {
                serviceClient.GetAllowanceTypesCompleted += (s, e) =>
                {
                    AllowanceTypes = e.Result;
                };

                serviceClient.GetAllowanceTypesAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Allowance Types refresh failed", "Refresh Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        #endregion

        #region Button methods

        #region New Method

        private void New()
        {
            RefreshAllowances();
            CurrentAllowance = null;
            SelectedEmployee = null;
            CurrentAllowance = new EmployeeAllowanceView();
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }
        #endregion

        #region Save Method

        private void Save()
        {
            try
            {
                if (CurrentAllowance != null)
                {
                    dtl_EmployeeAllowance addingAllowance = new dtl_EmployeeAllowance();
                    addingAllowance.employee_id = CurrentAllowance.employee_id;
                    addingAllowance.allowance_id = CurrentAllowanceType.allowance_id;
                    addingAllowance.amount = CurrentAllowance.amount;
                    addingAllowance.is_active = CurrentAllowance.is_active;

                    if (Allowances.ToList().Count(c => c.employee_id == CurrentAllowance.employee_id && c.allowance_id == CurrentAllowance.allowance_id)  < 1)
                    {
                        if (clsSecurity.GetSavePermission(513))
                        {
                            addingAllowance.save_datetime = DateTime.Now;
                            addingAllowance.save_user_id = clsSecurity.loggedUser.user_id;
                            if (serviceClient.SaveEmployeeAllowance(addingAllowance))
                            {
                                MessageBox.Show("Record is saved", "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                                New();
                            }
                            else
                            {
                                MessageBox.Show("Save is failed", "Save Unsuccessful", MessageBoxButton.OK, MessageBoxImage.Error);
                            } 
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                    else
                    {
                        if (MessageBoxResult.Yes == MessageBox.Show("Allowance is already assigned to employee, Are you want to update record?", "User Infromation", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
                        {
                            if (clsSecurity.GetUpdatePermission(513))
                            {
                                addingAllowance.modified_datetime = DateTime.Now;
                                addingAllowance.modified_user_id = clsSecurity.loggedUser.user_id;

                                if (serviceClient.UpdateEmployeeAllowance(addingAllowance))
                                {
                                    MessageBox.Show("Record is updated", "Update Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                                    New();
                                }
                                else
                                {
                                    MessageBox.Show("Update is failed", "Update Unsuccessful", MessageBoxButton.OK, MessageBoxImage.Error);
                                } 
                            }
                            else
                                clsMessages.setMessage("You don't have permission to Update this record(s)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool SaveCanExecute()
        {
            if (CurrentAllowance == null)
                return false;
            if (CurrentAllowance.employee_id == null)
                return false;
            if (CurrentAllowance.amount == null)
                return false;

            return true;
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }

        #endregion
        #endregion

        #region Employee Search

        private void EmployeeSearchButton()
        {
            EmployeeSearchWindow searchWindow = new EmployeeSearchWindow();
            searchWindow.ShowDialog();

            this.SelectedEmployee = searchWindow.viewModel.CurrentEmployeeSearchView;
            searchWindow.Close();
            if (SelectedEmployee != null)
            {
                CurrentAllowance.employee_id = (Guid)SelectedEmployee.employee_id;
                CurrentAllowance.emp_id = SelectedEmployee.emp_id;
                CurrentAllowance.first_name = SelectedEmployee.first_name;
                CurrentAllowance.second_name = SelectedEmployee.second_name;
            }
        }

        public ICommand EmployeeSearch
        {
            get { return new RelayCommand(EmployeeSearchButton); }
        }
        #endregion
    }
}
