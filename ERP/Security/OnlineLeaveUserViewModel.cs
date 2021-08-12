using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Security
{
    class OnlineLeaveUserViewModel:ViewModelBase
    {
        #region Service Client

        ERPServiceClient serviceClient;
        
        #endregion

        #region Constructor

        public OnlineLeaveUserViewModel()
        {
            serviceClient = new ERPServiceClient();
            this.New();
        }

        #endregion

        #region List Members

        List<OnlineLeaveUserDetailsView> allLeaveUserList = new List<OnlineLeaveUserDetailsView>();

        #endregion

        #region Data Members

        EmployeeSearchView selectedEmployee;

        #endregion

        #region Properties

        string employeeName;
        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; OnPropertyChanged("EmployeeName"); }
        }

        string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged("UserName"); }
        }

        string userPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set { userPassword = value; OnPropertyChanged("UserPassword"); }
        }

        IEnumerable<OnlineLeaveUserDetailsView> leaveUsers;
        public IEnumerable<OnlineLeaveUserDetailsView> LeaveUsers
        {
            get { return leaveUsers; }
            set { leaveUsers = value; OnPropertyChanged("LeaveUsers"); }
        }

        OnlineLeaveUserDetailsView currentLeaveUser;
        public OnlineLeaveUserDetailsView CurrentLeaveUser
        {
            get { return currentLeaveUser; }
            set 
            { 
                currentLeaveUser = value; OnPropertyChanged("CurrentLeaveUser");
                if (currentLeaveUser != null)
                    this.SetCurrentLeaveUser();
            }
        }

        bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged("IsActive"); }
        }

        bool isShowChar;
        public bool IsShowChar
        {
            get { return isShowChar; }
            set 
            {
                isShowChar = value; OnPropertyChanged("IsShowChar"); 
                if(isShowChar)
                {
                    ShowCharacters = Visibility.Visible;
                    HideCharacters = Visibility.Hidden;
                }
                else
                {
                    ShowCharacters = Visibility.Hidden;
                    HideCharacters = Visibility.Visible;
                }
            }
        }


        Visibility showCharacters;
        public Visibility ShowCharacters
        {
            get { return showCharacters; }
            set { showCharacters = value; OnPropertyChanged("ShowCharacters"); }
        }

        Visibility hideCharacters;
        public Visibility HideCharacters
        {
            get { return hideCharacters; }
            set { hideCharacters = value; OnPropertyChanged("HideCharacters"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshLeaveUsers()
        {
            serviceClient.GetLeaveUserDetailsCompleted += (s, e) =>
            {
                try
                {
                    LeaveUsers = e.Result;
                    if(leaveUsers != null)
                    {
                        allLeaveUserList = leaveUsers.ToList();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Leave Users refresh is failed");
                }
            };

            serviceClient.GetLeaveUserDetailsAsync();
        }
        
        #endregion

        #region Button Methods

        #region Save Button

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        void Save()
        {
            if (IsValidUser())
            {
                if (currentLeaveUser != null && allLeaveUserList.Count(c => c.employee_id == currentLeaveUser.employee_id) > 0)    // Update existing user
                {
                    dtl_EmployeeLogin updatingUser = new dtl_EmployeeLogin();
                    updatingUser.employee_id = (Guid)currentLeaveUser.employee_id;
                    updatingUser.employee_user_name = userName;
                    updatingUser.employee_user_password = userPassword;
                    updatingUser.isActive = isActive;
                    if (clsSecurity.GetUpdatePermission(106))
                    {
                        if (serviceClient.UpdateLeaveUser(updatingUser))
                        {
                            clsMessages.setMessage("Leave user is updated successfully");
                            this.RefreshLeaveUsers();
                        } 
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update in this form...");
                    }
                }
                else
                {
                    dtl_EmployeeLogin addingUser = new dtl_EmployeeLogin();
                    addingUser.employee_id = selectedEmployee.employee_id;
                    addingUser.employee_user_name = userName;
                    addingUser.employee_user_password = userPassword;
                    addingUser.isActive = isActive;
                    if (clsSecurity.GetSavePermission(106))
                    {
                        if (serviceClient.SaveLeaveUser(addingUser))
                        {
                            clsMessages.setMessage("Leave user is saved successfully");
                            this.New();
                        } 
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save in this form...");
                    }
                } 
            }
        } 

        #endregion

        #region New Button

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        void New()
        {
            this.RefreshLeaveUsers();
            EmployeeName = null;
            UserName = null;
            UserPassword = null;
            CurrentLeaveUser = null;
            CurrentLeaveUser = new OnlineLeaveUserDetailsView();
            IsActive = true;
            IsShowChar = false;
           
        }

        #endregion

        #region Select Employee Button
        
        public ICommand SelectEmployeeButton
        {
            get { return new RelayCommand(SelectEmployee); }
        }

        void SelectEmployee()
        {
            EmployeeMultipleSearchWindow searchEmpWindow = new EmployeeMultipleSearchWindow();
            searchEmpWindow.ShowDialog();
            if(searchEmpWindow.viewModel.selectEmployeeList != null)
            {
                List<EmployeeSearchView> selectEmployees = searchEmpWindow.viewModel.selectEmployeeList;
                if(selectEmployees.Count > 0)
                {
                    selectedEmployee = selectEmployees.FirstOrDefault();
                    if(selectedEmployee != null)
                    {
                        CurrentLeaveUser = null;
                        if (allLeaveUserList.Count(c=>c.employee_id == selectedEmployee.employee_id) > 0)
                        {
                            
                            CurrentLeaveUser = allLeaveUserList.FirstOrDefault(c => c.employee_id == selectedEmployee.employee_id); 
                        }
                        else
                        {
                            CurrentLeaveUser = new OnlineLeaveUserDetailsView { 
                                employee_id = selectedEmployee.employee_id ,
                                EMP_NAME = (selectedEmployee.first_name == null ? "" : selectedEmployee.first_name) + " " + (selectedEmployee.second_name == null ? "" : selectedEmployee.second_name),
                                isActive = true
                            };
                        }
                    }
                }
            }
            searchEmpWindow.Close();
              
        }

        #endregion

        #endregion

        #region Data Setting Methods

        void SetCurrentLeaveUser()
        {
            if(currentLeaveUser != null)
            {
                EmployeeName = currentLeaveUser.EMP_NAME;
                UserName = currentLeaveUser.employee_user_name;
                UserPassword = currentLeaveUser.employee_user_password;
                IsActive = currentLeaveUser.isActive == null ? false: (bool)currentLeaveUser.isActive;
            }
        }

        #endregion

        #region Validate Methods

        bool IsUserNameExists()
        {
            if (allLeaveUserList.Count(c => c.employee_user_name == userName) > 0)
                return true;
            return false;
        }

        bool IsPasswordExists()
        {
            if (allLeaveUserList.Count(c => c.employee_user_password == userPassword) > 0)
                return true;
            return false;
        }

        bool IsValidUser()
        {
            if (userName == null || userName == string.Empty)
            {
                clsMessages.setMessage("User name is required");
                return false;
            }
            if(userPassword == null || userPassword == string.Empty)
            {
                clsMessages.setMessage("User password is required");
                return false;
            }
            if(IsUserNameExists() && IsPasswordExists())
            {
                clsMessages.setMessage("User name and password is already exists");
                return false;
            }
            if(currentLeaveUser != null && currentLeaveUser.employee_id == null && selectedEmployee == null)
            {
                clsMessages.setMessage("Employee should have been selected");
                return false;
            }
            //if (selectedEmployee != null && allLeaveUserList.Count(c => c.employee_id == selectedEmployee.employee_id) > 0)
            //{
            //    clsMessages.setMessage("Selected employee already exists");
            //    return false;
            //}
            return true;

        }

        #endregion
    }
}
