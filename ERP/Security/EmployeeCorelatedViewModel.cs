using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.Security
{
    class EmployeeCorelatedViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<dtl_employee_correlate_task> ListCorrelatedEmployees;
        List<EmployeeSearchView> ListEmployees;

        #endregion

        #region Constructor

        public EmployeeCorelatedViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListCorrelatedEmployees = new List<dtl_employee_correlate_task>();
            ListEmployees = new List<EmployeeSearchView>();
            RefreshEmployeeSearch();
            RefreshCorrelatedEmployees();
        }

        #endregion

        #region Properties

        private IEnumerable<dtl_employee_correlate_task> _CorrelatedEmployees;
        public IEnumerable<dtl_employee_correlate_task> CorrelatedEmployees
        {
            get { return _CorrelatedEmployees; }
            set { _CorrelatedEmployees = value; OnPropertyChanged("CorrelatedEmployees"); }
        }

        private EmployeeSearchView _CurrentEmployee;
        public EmployeeSearchView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private string _EmployeeName;
        public string EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; OnPropertyChanged("EmployeeName"); }
        }


        private IEnumerable<EmployeeSearchView> _Employees;
        public IEnumerable<EmployeeSearchView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSearchView _SelectedEmployee;
        public EmployeeSearchView SelectedEmployee
        {
            get { return _SelectedEmployee; }
            set { _SelectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }




        #endregion

        #region Refresh Methods

        private void RefreshCorrelatedEmployees()
        {
            serviceClient.GetEmployeeCoRelatedTaskCompleted += (s, e) =>
            {
                try
                {
                    ListCorrelatedEmployees.Clear();

                    CorrelatedEmployees = e.Result;
                    if (CorrelatedEmployees != null && CorrelatedEmployees.Count() > 0)
                        ListCorrelatedEmployees = CorrelatedEmployees.ToList();

                    CorrelatedEmployees = null;

                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmployeeCoRelatedTaskAsync();
        }

        private void RefreshEmployeeSearch()
        {
            try
            {
                serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
                {
                    try
                    {
                        ListEmployees.Clear();

                        Employees = e.Result;
                        if (Employees != null && Employees.Count() > 0)
                            ListEmployees = Employees.ToList();

                        Employees = null;
                    }
                    catch (Exception)
                    {

                    }
                };
                serviceClient.GetEmloyeeSearchAsync(clsSecurity.loggedUser.user_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Refresh Error");
            }
        }

        #endregion

        #region Commands and Methods

        #region Manager Method

        public ICommand BtnEmployeeSelect
        {
            get { return new RelayCommand(managaerSelect); }
        }
        void managaerSelect()
        {
            EmployeeSearchWindow window = new EmployeeSearchWindow();
            window.ShowDialog();
            if (window.viewModel.CurrentEmployeeSearchView != null)
                CurrentEmployee = window.viewModel.CurrentEmployeeSearchView;
            window.Close();
            window = null;
            if (CurrentEmployee != null)
            {
                EmployeeName = CurrentEmployee.initials + " " + CurrentEmployee.first_name;

                if (ListCorrelatedEmployees.Count(c => c.employee_id == CurrentEmployee.employee_id) > 0)
                {
                    if (ListEmployees != null && ListEmployees.Count() > 0)
                    {
                        Employees = null;
                        Employees = ListEmployees.Where(c => ListCorrelatedEmployees.Where(d => d.employee_id == CurrentEmployee.employee_id).Count(e => e.employee_id != c.employee_id && e.correlate == c.employee_id) > 0);
                    }
                }

                else
                    Employees = null;
            }
        }
        #endregion

        #region Get Correlated Method


        public ICommand BtnGetCorrelatedEmployees
        {
            get { return new RelayCommand(SelectCorrelatedEmployees, SelectCorrelatedEmployeesCE); }
        }
        private bool SelectCorrelatedEmployeesCE()
        {
            if (CurrentEmployee != null)
                return true;
            else
                return false;
        }
        void SelectCorrelatedEmployees()
        {
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            if (window.viewModel.SelectedList != null && ListEmployees != null && ListEmployees.Count() > 0)
            {

                foreach (EmployeeSearchView item in window.viewModel.SelectedList.Where(c => c.employee_id != CurrentEmployee.employee_id).ToList())
                {
                    if (ListCorrelatedEmployees.Count == 0 && CurrentEmployee.employee_id != item.employee_id)
                    {
                        dtl_employee_correlate_task temp = new dtl_employee_correlate_task();
                        temp.employee_id = CurrentEmployee.employee_id;
                        temp.correlate = item.employee_id;
                        ListCorrelatedEmployees.Add(temp);
                    }
                    else
                    {
                        if (ListCorrelatedEmployees.Where(c => c.employee_id == CurrentEmployee.employee_id).Count(d => d.correlate == item.employee_id) == 0)
                        {
                            dtl_employee_correlate_task temp = new dtl_employee_correlate_task();
                            temp.employee_id = CurrentEmployee.employee_id;
                            temp.correlate = item.employee_id;
                            ListCorrelatedEmployees.Add(temp);
                        }
                    }
                }

                Employees = null;
                Employees = ListEmployees.Where(c => ListCorrelatedEmployees.Where(d => d.employee_id == CurrentEmployee.employee_id).Count(e => e.employee_id != c.employee_id && e.correlate == c.employee_id) > 0);
            }
            window.Close();
            window = null;
        }
        #endregion

        #region Save

        public ICommand BtnSave
        {
            get { return new RelayCommand(Save, SaveCE); }
        }

        private bool SaveCE()
        {
            if (CurrentEmployee != null)
                return true;
            else
                return false;
        }

        private void Save()
        {
            if (clsSecurity.GetSavePermission(107) && clsSecurity.GetUpdatePermission(107))
            {
                if (ListCorrelatedEmployees.Count(c => c.employee_id == CurrentEmployee.employee_id && c.record_id == 0) > 0)
                {
                    if (serviceClient.SaveEmployeeCoRelatedTask(ListCorrelatedEmployees.Where(c => c.employee_id == CurrentEmployee.employee_id && c.record_id == 0).ToArray()))
                    {
                        Employees = null;
                        CurrentEmployee = null;
                        EmployeeName = null;
                        RefreshCorrelatedEmployees();
                        clsMessages.setMessage("Records saved succesfully");
                    }
                    else
                        clsMessages.setMessage("Records save failed");
                }
                else
                    clsMessages.setMessage("No new records to save");
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Save or Update in this form...");
            }
        }

        #endregion

        #region Delete

        public ICommand BtnDelete
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (SelectedEmployee != null)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(107))
            {
                if (ListCorrelatedEmployees.Count(c => c.correlate == SelectedEmployee.employee_id && c.employee_id == CurrentEmployee.employee_id && c.record_id != 0) > 0)
                {
                    dtl_employee_correlate_task DelObj = ListCorrelatedEmployees.FirstOrDefault(c => c.correlate == SelectedEmployee.employee_id && c.employee_id == CurrentEmployee.employee_id);
                    if (serviceClient.DeleteEmployeeCoRelatedTask(DelObj))
                    {
                        Employees = null;
                        CurrentEmployee = null;
                        EmployeeName = null;
                        RefreshCorrelatedEmployees();
                        clsMessages.setMessage("Record deleted Successfully");
                    }
                    else
                    {
                        clsMessages.setMessage("Record delete Failed");
                    }
                }
                else
                {
                    clsMessages.setMessage("Please Select a Saved Record");
                } 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete in this form...");
            }
        }
        

        #endregion

        #endregion

    }
}
