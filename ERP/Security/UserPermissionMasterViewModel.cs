using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;

namespace ERP
{
   public class UserPermissionMasterViewModel : ViewModelBase
    {

        ERPServiceClient serviceClient = new ERPServiceClient();

        public UserPermissionMasterViewModel()
        {
            this.refreshUsers();
            this.refreshUserLevels();
            this.refreshModules();            
        }

        private IEnumerable<usr_User> _Users;
        public IEnumerable<usr_User> Users
        {
            get { return this._Users; }
            set { this._Users = value; OnPropertyChanged("Users"); }
        }
        
        private usr_User _CurrentUser;
        public usr_User CurrentUser
        {
            get             
            { return this._CurrentUser; }

            set
            {
                this._CurrentUser = value; OnPropertyChanged("CurrentUser");
                refreshUserPermissionByEmployee();
            }
        }
                
        private IEnumerable<z_module> _Modules;
        public IEnumerable<z_module> Modules
        {
            get { return this._Modules; }
            set { this._Modules = value; OnPropertyChanged("Modules"); }
        }        

        private IEnumerable<usr_UserLevel> _UserLevels;
        public IEnumerable<usr_UserLevel> UserLevels
        {
            get{return this._UserLevels;}
            set{this._UserLevels = value; OnPropertyChanged("UserLevels");}
        }

        private usr_UserLevel _CurrentUserLevel;
        public usr_UserLevel CurrentUserLevel
        {
            get { return this._CurrentUserLevel; }
            set { this._CurrentUserLevel = value; OnPropertyChanged("CurrentUserLevel");
            refreshUserPermissionByUserLevel();
            
            }
        }

        private IEnumerable<UserPermissionsMasterView> _UserPermissions;
        public IEnumerable<UserPermissionsMasterView> UserPermissions
        {
            get { return this._UserPermissions; }
            set { this._UserPermissions = value; OnPropertyChanged("UserPermissions"); }
        }

        private UserPermissionsMasterView _CurrentUserPermission;
        public UserPermissionsMasterView CuurentUserPermission
        {
            get { return this._CurrentUserPermission;}
            set { this._CurrentUserPermission = value; OnPropertyChanged("CuurentUserPermission");                
            }
        }

        private bool _Userwice;
        public bool Userwice
        {
            get { return this._Userwice; }
            set { this._Userwice = value; OnPropertyChanged("Userwice"); CuurentUserPermission = null; 
           }
        }
       
        private bool _UserLevelwice;
        public bool UserLevelwice
        {
            get { return this._UserLevelwice; }
            set { this._UserLevelwice = value; OnPropertyChanged("UserLevelwice"); CuurentUserPermission = null; }
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(saveRecord, saveRecordCanExecute);
            }
        }

        public ICommand NewButton
        {
            get 
            {
                return new RelayCommand(newRecord, newRecordCanExecute);
            }
        }

        public ICommand DeleteButton
        {
            get 
            {
                return new RelayCommand(deleteRecord, deleteRecordCanExecute);
            }
        }

        public ICommand AddPermissionButton
        {
            get 
            {
                return new RelayCommand(addPermission, addPermissionCanExecute);
            }
        }

        public ICommand RemovePermission
        {
            get 
            {
                return new RelayCommand(removePermission, removePermissionCanExecute);
            }
        }

        void saveRecord()
        { }

        bool saveRecordCanExecute()
        { return true; }

        void newRecord()
        { }

        bool newRecordCanExecute()
        { return true; }

        void deleteRecord()
        { }

        bool deleteRecordCanExecute()
        { return true; }

        void addPermission()
        { }

        bool addPermissionCanExecute()
        { return true; }

        void removePermission()
        { }

        bool removePermissionCanExecute()
        { return true; }

        private void refreshUserPermissionByEmployee()
        {
            this.serviceClient.GetUserPermissionsByEmployeeCompleted += (s, e) =>
            {
                this.UserPermissions = e.Result;
            };
            this.serviceClient.GetUserPermissionsByEmployeeAsync(CurrentUser.user_id);

        }

        private void refreshUserPermissionByUserLevel()
        {
            this.serviceClient.GetUserPermissionsByUserLevelCompleted += (s, e) =>
                {
                    this.UserPermissions = e.Result;
                };
            this.serviceClient.GetUserPermissionsByUserLevelAsync(CurrentUserLevel.user_level_id);
        }

        private void refreshUsers()
        {
            this.serviceClient.GetUsersCompleted += (s, e) =>
                {
                    this.Users = e.Result;
                };
            this.serviceClient.GetUsersAsync();
        }

        private void refreshUserLevels()
        {
            this.serviceClient.GetUserLevelCompleted += (s, e) =>
                {
                    this.UserLevels = e.Result;
                };
            this.serviceClient.GetUserLevelAsync();
        }

        private void refreshModules()
        {
            this.serviceClient.GetModulesCompleted += (s, e) =>
                {
                    this.Modules = e.Result;
                };
            this.serviceClient.GetModulesAsync();
        }
    }
}
