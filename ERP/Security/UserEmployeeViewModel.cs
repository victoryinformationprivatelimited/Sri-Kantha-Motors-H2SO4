using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;

using ERP.BasicSearch;
using System.Windows;
using System.Windows.Input;

namespace ERP.Security
{
    public class UserEmployeeViewModel : ViewModelBase
    {
        Guid Id;

        #region ServiceCilent

        ERPServiceClient serviceClient;

        #endregion

        #region Constructor

        public UserEmployeeViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshUsers();
            RefreshEmployee();
            RefreshUserEmployee();
        }

        #endregion

        #region Properties

        private IEnumerable<usr_User> users;
        public IEnumerable<usr_User> Users
        {
            get { return users; }
            set { users = value; OnPropertyChanged("Users"); }
        }

        private usr_User currentUser;
        public usr_User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; OnPropertyChanged("CurrentUser"); if (CurrentUser != null) FilterEmployeeUser(); }
        }

        private IEnumerable<mas_Employee> employees;
        public IEnumerable<mas_Employee> Employees
        {
            get { return employees; }
            set { employees = value; OnPropertyChanged("Employees"); }
        }

        private IEnumerable<usr_UserEmployee> userEmployee;
        public IEnumerable<usr_UserEmployee> UserEmployee
        {
            get { return userEmployee; }
            set { userEmployee = value; OnPropertyChanged("UserEmployee"); }
        }

        private string empName;
        public string EmpName
        {
            get { return empName; }
            set { empName = value; OnPropertyChanged("EmpName"); }
        }


        #endregion

        #region RefreshMethods

        private void RefreshUsers()
        {
            serviceClient.GetUsersCompleted += (s, e) =>
            {
                Users = e.Result.OrderBy(c => c.user_name);
            };
            serviceClient.GetUsersAsync();
        }

        private void RefreshEmployee()
        {
            serviceClient.GetEmployeesCompleted += (s, e) =>
            {
                Employees = e.Result;
            };
            serviceClient.GetEmployeesAsync();
        }

        private void RefreshUserEmployee()
        {
            serviceClient.GetUserEmployeesCompleted += (s, e) =>
            {
                UserEmployee = e.Result;
            };
            serviceClient.GetUserEmployeesAsync();
        }

        #endregion

        #region Methods

        private void getEmployee()
        {
            EmployeeSearchWindow window = new EmployeeSearchWindow();
            window.ShowDialog();
            try
            {
                if (window.viewModel.CurrentEmployeeSearchView != null)
                {
                    Id = (Guid)window.viewModel.CurrentEmployeeSearchView.employee_id;
                    if (Id != Guid.Empty)
                        EmpName = Employees.FirstOrDefault(c => c.employee_id == Id).initials + " " + Employees.FirstOrDefault(c => c.employee_id == Id).first_name + " " + Employees.FirstOrDefault(c => c.employee_id == Id).second_name;
                }
            }
            catch (Exception)
            {

                clsMessages.setMessage("Please Select an Employee");
            }
        }

        private void SaveUser()
        {
            usr_UserEmployee saveobject = new usr_UserEmployee();
            bool isupdate = (UserEmployee.Where(c => c.user_id == CurrentUser.user_id).Count() > 0);
            if (isupdate == false)
            {
                //if ()
                {
                    if (MessageBoxResult.OK == MessageBox.Show("Are you sure you want to Assign this employee?", "", MessageBoxButton.OKCancel, MessageBoxImage.Question))
                    {
                        saveobject.user_id = CurrentUser.user_id;
                        saveobject.employee_id = Id;
                        //_________________________________________________________________________________________________
                        if (clsSecurity.GetSavePermission(104))
                        {
                            if (serviceClient.SaveUserEmployee(saveobject))
                            {
                                RefreshUserEmployee();
                                clsSecurity.loggedEmployee = this.serviceClient.GetUserEmployee(CurrentUser.user_id);
                                clsSecurity.moduleSupervision = this.serviceClient.GetAllEmployeeSupervisorByUserEmployee(clsSecurity.loggedEmployee == null ? Guid.Empty : (Guid)clsSecurity.loggedEmployee.employee_id);
                                MessageBox.Show("Record Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                                MessageBox.Show("Record Save Failed", "", MessageBoxButton.OK, MessageBoxImage.Information); 
                        }
                        else
                        {
                            clsMessages.setMessage("You don't have permission to Save in this form...");
                        }
                        //__________________________________________________________________________________________________
                    } 
                }

            }

            else
            {
                if (MessageBoxResult.OK == MessageBox.Show("Are you sure you want to Update this User?", "", MessageBoxButton.OKCancel, MessageBoxImage.Question))
                {
                    saveobject.user_id = CurrentUser.user_id;
                    saveobject.employee_id = Id;
                    //_______________________________________________________________________________________________________
                    if (clsSecurity.GetUpdatePermission(104))
                    {
                        if (serviceClient.UpdateUserEmployee(saveobject))
                        {
                            RefreshUserEmployee();
                            clsSecurity.loggedEmployee = this.serviceClient.GetUserEmployee(CurrentUser.user_id);
                            clsSecurity.moduleSupervision = this.serviceClient.GetAllEmployeeSupervisorByUserEmployee(clsSecurity.loggedEmployee == null ? Guid.Empty : (Guid)clsSecurity.loggedEmployee.employee_id);
                            MessageBox.Show("Record Updated Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            MessageBox.Show("Record Update Failed", "", MessageBoxButton.OK, MessageBoxImage.Information); 
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update in this form...");
                    }
                    //________________________________________________________________________________________________________
                }
            }
        }

        private void FilterEmployeeUser()
        {
            if (UserEmployee != null && Employees != null)
            {
                if (UserEmployee.Where(c => c.user_id == CurrentUser.user_id).Count() == 1)
                {
                    Id = (Guid)UserEmployee.FirstOrDefault(c => c.user_id == CurrentUser.user_id).employee_id;
                    if (Id != null && Id != Guid.Empty)
                        EmpName = Employees.FirstOrDefault(c => c.employee_id == Id).initials + " " + Employees.FirstOrDefault(c => c.employee_id == Id).first_name + " " + Employees.FirstOrDefault(c => c.employee_id == Id).second_name;
                }
                else
                {
                    EmpName = "";
                    MessageBox.Show("No assigned Employee for this User", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        #endregion

        #region CanExecutes

        private bool getEmployeeCanExecute()
        {
            if (CurrentUser == null)
                return false;
            else
                return true;
        }

        private bool SaveCanExecute()
        {
            if (CurrentUser != null && EmpName != string.Empty)
                return true;
            else
                return false;
        }


        #endregion

        #region Commands

        public ICommand empSearch
        {
            get { return new RelayCommand(getEmployee, getEmployeeCanExecute); }
        }

        public ICommand SaveUserEmp
        {
            get { return new RelayCommand(SaveUser, SaveCanExecute); }
        }

        #endregion
    }
}
