using ERP.BasicSearch;
using ERP.ERPService;
using ERP.HelperClass;
using ERP.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ERP.Payroll
{
    public class CompanyVariableViewModel : ViewModelBase
    {
        #region Service Object

        private ERPServiceClient serviceClient;

        #endregion

        #region List Members

        List<payroll_CompanyVariable_View> AllEmployeeVariable;

        #endregion

        #region Constructor

        public CompanyVariableViewModel()
        {
            AllEmployeeVariable = new List<payroll_CompanyVariable_View>();
            serviceClient = new ERPServiceClient();
            RefreshVariable();
            RefreshEmployeeVariable();
        }

        #endregion

        #region Properties

        private IEnumerable<EmployeeSearchView> _SelectedEmployees;
        public IEnumerable<EmployeeSearchView> SelectedEmployees
        {
            get { return _SelectedEmployees; }
            set { _SelectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }

        private IEnumerable<z_CompanyVariable> _CompanyVariable;
        public IEnumerable<z_CompanyVariable> CompanyVariable
        {
            get { return _CompanyVariable; }
            set { _CompanyVariable = value; OnPropertyChanged("CompanyVariable"); }
        }

        private z_CompanyVariable _CurrentCompanyVariable;
        public z_CompanyVariable CurrentCompanyVariable
        {
            get { return _CurrentCompanyVariable; }
            set { _CurrentCompanyVariable = value; OnPropertyChanged("CurrentCompanyVariable"); }
        }

        private IList _CurrentEmployees = new ArrayList();
        public IList CurrentEmployees
        {
            get { return _CurrentEmployees; }
            set { _CurrentEmployees = value; OnPropertyChanged("CurrentEmployees"); FilterByEmployee(); }
        }

        private IEnumerable<payroll_CompanyVariable_View> _EmployeeVariable;
        public IEnumerable<payroll_CompanyVariable_View> EmployeeVariable
        {
            get { return _EmployeeVariable; }
            set { _EmployeeVariable = value; OnPropertyChanged("EmployeeVariable"); }
        }

        private payroll_CompanyVariable_View _CurrentEmployeeVariable;
        public payroll_CompanyVariable_View CurrentEmployeeVariable
        {
            get { return _CurrentEmployeeVariable; }
            set { _CurrentEmployeeVariable = value; OnPropertyChanged("CurrentEmployeeVariable"); }
        }

        private IList _CurrentCompanyVariableEmployee = new ArrayList();

        public IList CurrentCompanyVariableEmployee
        {
            get { return _CurrentCompanyVariableEmployee; }
            set { _CurrentCompanyVariableEmployee = value; OnPropertyChanged("CurrentCompanyVariableEmployee"); }
        }



        #endregion

        #region Refresh Methods

        public void RefreshVariable()
        {
            serviceClient.GetCompanyVariablesCompleted += (s, e) =>
            {
                CompanyVariable = e.Result;
            };
            serviceClient.GetCompanyVariablesAsync();
        }

        public void RefreshEmployeeVariable()
        {
            try
            {
                serviceClient.GetEmployeeVariableCompleted += (s, e) =>
                {
                    AllEmployeeVariable.Clear();
                    EmployeeVariable = e.Result;
                    if (EmployeeVariable != null && EmployeeVariable.Count() > 0)
                        AllEmployeeVariable = EmployeeVariable.ToList();
                    EmployeeVariable = null;
                };
                serviceClient.GetEmployeeVariableAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Methods & Buttons

        #region Add Employee Button
        public ICommand AddEmployeebtn
        {
            get { return new RelayCommand(AddEmployee, AddEmployeeCE); }
        }

        private bool AddEmployeeCE()
        {
            return true;
        }

        private void AddEmployee()
        {
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            if (window.viewModel.SelectedList != null)
            {
                SelectedEmployees = null;
                SelectedEmployees = window.viewModel.SelectedList.ToList();
            }

            window.Close();
            window = null;
        }

        #endregion

        #region Assign Rule Button

        public ICommand Addbtn
        {
            get { return new RelayCommand(Add, AddCE); }
        }

        private bool AddCE()
        {
            if (CurrentCompanyVariable != null)
                return true;
            else
                return false;
        }

        private void Add()
        {
            if (ValidateAddRule())
            {
                foreach (EmployeeSearchView item in CurrentEmployees)
                {
                    if (AllEmployeeVariable.Count(c => c.employee_id == item.employee_id && c.company_variableID == CurrentCompanyVariable.company_variableID) == 0)
                    {
                        payroll_CompanyVariable_View temp = new payroll_CompanyVariable_View();
                        temp.company_variableID = CurrentCompanyVariable.company_variableID;
                        temp.employee_id = item.employee_id;
                        temp.variable_Name = CurrentCompanyVariable.variable_Name;
                        temp.emp_id = item.emp_id;
                        temp.first_name = item.first_name;
                        temp.isApplicable = true;
                        temp.isdelete = false;
                        AllEmployeeVariable.Add(temp);
                    }
                }

                EmployeeVariable = null;
                EmployeeVariable = AllEmployeeVariable.Where(c => c.save_user_id == null).OrderBy(c => c.company_variableID);
            }
        }

        private bool ValidateAddRule()
        {
            if (CurrentEmployees == null || CurrentEmployees.Count == 0)
            {
                clsMessages.setMessage("Please select employees.");
                return false;
            }
            else
                return true;
        }

        #endregion

        #region Clear Button
        public ICommand Clearbtn
        {
            get { return new RelayCommand(Clear, ClearCE); }
        }

        private bool ClearCE()
        {
            if (CurrentEmployees != null)
                return true;
            else
                return false;
        }

        private void Clear()
        {
            clsMessages.setMessage("Are You Sure Clear All", Visibility.Visible);
            if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
            {
                EmployeeVariable = null;
                SelectedEmployees = null;
                CurrentCompanyVariable = null;
                AllEmployeeVariable.Clear();
                CurrentCompanyVariable = null;
                RefreshVariable();
                RefreshEmployeeVariable();
            }
        }

        #endregion

        #region Save Button

        public ICommand Savebtn
        {
            get { return new RelayCommand(Save, SaveCE); }
        }
        private bool SaveCE()
        {
            if (EmployeeVariable != null)
            {
                return true;
            }
            else
                return false;
        }
        private void Save()
        {
            if (SaveValidate())
            {
                List<dtl_EmployeeCompanyVariable> SaveList = new List<dtl_EmployeeCompanyVariable>();
                
                foreach (var item in AllEmployeeVariable.Where(c => c.save_user_id == null))
                {
                    dtl_EmployeeCompanyVariable temp = new dtl_EmployeeCompanyVariable();
                    temp.company_variableID = item.company_variableID;
                    temp.employee_id = item.employee_id;
                    temp.isApplicable = item.isApplicable;
                    temp.isdelete = item.isdelete;
                    temp.save_datetime = DateTime.Now;
                    temp.save_user_id = clsSecurity.loggedUser.user_id;
                    SaveList.Add(temp);
                }
                if (serviceClient.SaveCompanyVariable(SaveList.ToArray()))
                {
                    RefreshEmployeeVariable();
                    SelectedEmployees = null;
                    AllEmployeeVariable = null;
                    AllEmployeeVariable = new List<payroll_CompanyVariable_View>();
                    clsMessages.setMessage(Properties.Resources.SaveSucess);
                }
                else
                {
                    clsMessages.setMessage(Properties.Resources.SaveFail);
                }

            }
        }
        private bool SaveValidate()
        {
            if (!clsSecurity.GetSavePermission(503))
            {
                clsMessages.setMessage("You don't have permission to save this record");
                return false;
            }
            else if (AllEmployeeVariable.Count(c => c.save_user_id == null) == 0)
            {
                clsMessages.setMessage("No new records to save");
                return false;
            }
            else
                return true;
        }

        #endregion

        #region Delete Button

        public ICommand Deletebtn
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (CurrentCompanyVariableEmployee != null)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(503))
            {
                clsMessages.setMessage("Are You Sure Delete", Visibility.Visible);
                if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                {
                    List<dtl_EmployeeCompanyVariable> DeleteList = new List<dtl_EmployeeCompanyVariable>();
                    foreach (var item in AllEmployeeVariable.Where(c => CurrentCompanyVariableEmployee.Cast<payroll_CompanyVariable_View>().ToList().Count(d => c.company_variableID == d.company_variableID && c.employee_id == d.employee_id) > 0).ToList())
                    {
                        dtl_EmployeeCompanyVariable temp = new dtl_EmployeeCompanyVariable();
                        temp.company_variableID = item.company_variableID;
                        temp.employee_id = item.employee_id;
                        temp.isdelete = true;
                        temp.delete_datetime = DateTime.Now;
                        temp.delete_user_id = clsSecurity.loggedUser.user_id;
                        DeleteList.Add(temp);
                    }
                    if (serviceClient.DeleteCompanyVariable(DeleteList.ToArray()))
                    {
                        RefreshEmployeeVariable();
                        SelectedEmployees = null;
                        AllEmployeeVariable = null;
                        AllEmployeeVariable = new List<payroll_CompanyVariable_View>();
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
            }
            else
                clsMessages.setMessage("You don't have permission to delete this record");
        }

        #endregion

        #region Filtering Method

        private void FilterByEmployee()
        {
            if (CurrentEmployees != null && CurrentEmployees.Cast<EmployeeSearchView>().Count() == 1 && CurrentCompanyVariable == null)
            {
                EmployeeVariable = null;
                EmployeeVariable = AllEmployeeVariable.Where(c => CurrentEmployees.Cast<EmployeeSearchView>().Count(d => d.employee_id == c.employee_id) > 0).ToList();
            }
            else if (CurrentEmployees.Cast<EmployeeSearchView>().Count() > 1 && CurrentCompanyVariable == null)
            {
                EmployeeVariable = null;
            }
        }

        #endregion

        #endregion
    }
}

