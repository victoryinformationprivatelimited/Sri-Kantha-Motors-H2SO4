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
    class UserPermissionViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<mas_Permission> AllPermission;
        List<dtl_PermissionView> AllPermissionView;
        
        #endregion

        #region Constructors

        public UserPermissionViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllPermission = new List<mas_Permission>();
            AllPermissionView = new List<dtl_PermissionView>();
            New();
        }
        
        #endregion

        #region Properties

        private IEnumerable<usr_UserLevel> _UserLevels;
        public IEnumerable<usr_UserLevel> UserLevels
        {
            get { return _UserLevels; }
            set { _UserLevels = value; OnPropertyChanged("UserLevels"); }
        }

        private usr_UserLevel _CurrentUserLevel;

        public usr_UserLevel CurrentUserLevel
        {
            get { return _CurrentUserLevel; }
            set { _CurrentUserLevel = value; OnPropertyChanged("CurrentUserLevel"); if (CurrentUserLevel != null) { CurrentPermissionGroup = null; Permission = null; PermissionView = null; SelectedPermission = new dtl_PermissionView(); } }
        }

        private IEnumerable<z_Permission_Group> _PermissionGroup;

        public IEnumerable<z_Permission_Group> PermissionGroup
        {
            get { return _PermissionGroup; }
            set { _PermissionGroup = value; OnPropertyChanged("PermissionGroup"); }
        }

        private z_Permission_Group _CurrentPermissionGroup;
        public z_Permission_Group CurrentPermissionGroup
        {
            get { return _CurrentPermissionGroup; }
            set { _CurrentPermissionGroup = value; OnPropertyChanged("CurrentPermissionGroup"); if (CurrentPermissionGroup != null) { FilterPermissions(); FilterPermissionView(); } }
        }

       

        private IEnumerable<mas_Permission> _Permission;

        public IEnumerable<mas_Permission> Permission
        {
            get { return _Permission; }
            set { _Permission = value; OnPropertyChanged("Permission"); }
        }

        private mas_Permission _CurrentPermission;

        public mas_Permission CurrentPermission
        {
            get { return _CurrentPermission; }
            set { _CurrentPermission = value; OnPropertyChanged("CurrentPermission"); }
        }

        private dtl_PermissionView _SelectedPermission;
        public dtl_PermissionView SelectedPermission
        {
            get { return _SelectedPermission; }
            set { _SelectedPermission = value; OnPropertyChanged("SelectedPermission"); }
        }

        private IEnumerable<dtl_PermissionView> _PermissionView;
        public IEnumerable<dtl_PermissionView> PermissionView
        {
            get { return _PermissionView; }
            set { _PermissionView = value; OnPropertyChanged("PermissionView"); }
        }

        private dtl_PermissionView _CurrentPermissionView;
        public dtl_PermissionView CurrentPermissionView
        {
            get { return _CurrentPermissionView; }
            set { _CurrentPermissionView = value; OnPropertyChanged("CurrentPermissionView"); }
        }
       
        
        #endregion

        #region Refresh Methods

        private void RefreshUserLevels()
        {
            serviceClient.GetUserLevelCompleted += (s, e) =>
            {
                try
                {
                    UserLevels = e.Result;
                }
                catch (Exception)
                {
                 
                }
            };
            serviceClient.GetUserLevelAsync();
        }

        private void ResfreshPermissionGroup()
        {
            serviceClient.GetPermissionGroupsCompleted += (s, e) =>
                {
                    PermissionGroup = e.Result;     
                };
            serviceClient.GetPermissionGroupsAsync();
        }

        private void RefreshPermission()
        {
            serviceClient.GetPermissionsCompleted += (s, e) =>
                {
                    IEnumerable<mas_Permission> temp = e.Result;
                    if (temp != null)
                    {
                        Permission = null;
                        AllPermission.Clear();
                        AllPermission = temp.ToList();
                    }
                 
                };
            serviceClient.GetPermissionsAsync();
        }

        private void RefreshAllPermissionsView()
        {
            serviceClient.GetDetailPermissionViewCompleted += (s, e) => 
            {
                IEnumerable<dtl_PermissionView> temp = e.Result;
                if(temp != null)
                {
                    PermissionView = null;
                    AllPermissionView = temp.ToList();
                }    
            };
            serviceClient.GetDetailPermissionViewAsync();
        }

        #endregion

        #region Button Commands & Methods

        private void FilterPermissions()
        {
            if (AllPermission != null && AllPermission.Count() > 0)
            {
                Permission = null;
                Permission = AllPermission.Where(c => c.Permission_Group_Code == CurrentPermissionGroup.Permission_Group_Code);
            }
        }

        private void FilterPermissionView()
        {
            PermissionView = null;
            PermissionView = AllPermissionView.Where(c =>c.user_level_id == CurrentUserLevel.user_level_id && c.Permission_Group_Code == CurrentPermissionGroup.Permission_Group_Code);
        }

        public ICommand BtnAddPermission 
        {
            get { return new RelayCommand(AddPermission, AddPermissionCE); }
        }

        private bool AddPermissionCE()
        {
            if (CurrentPermissionGroup != null && CurrentPermission != null && CurrentUserLevel != null)
                return true;
            else
                return false;
        }

        private void AddPermission()
        {
            if (AllPermissionView.Count(c=> c.user_level_id == CurrentUserLevel.user_level_id && c.Permission_Code == CurrentPermission.Permission_Code) == 0)
            {
                dtl_PermissionView temp = new dtl_PermissionView();
                temp.user_level_id = CurrentUserLevel.user_level_id;
                temp.Permission_Group_Code = CurrentPermission.Permission_Group_Code;
                temp.Permission_Code = CurrentPermission.Permission_Code;
                temp.Permission_Name = CurrentPermission.Permission_Name;
                temp.Can_View = SelectedPermission.Can_View;
                temp.Can_Save = SelectedPermission.Can_Save;
                temp.Can_Update = SelectedPermission.Can_Update;
                temp.Can_Delete = SelectedPermission.Can_Delete;

                PermissionView = null;
                AllPermissionView.Add(temp);
                PermissionView = AllPermissionView.Where(c => c.user_level_id == CurrentUserLevel.user_level_id && c.Permission_Group_Code == CurrentPermissionGroup.Permission_Group_Code);
                SelectedPermission = new dtl_PermissionView(); 
            }

        }

        public ICommand BtnNew
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            SelectedPermission = new dtl_PermissionView();
            RefreshUserLevels();
            ResfreshPermissionGroup();
            RefreshPermission();
            RefreshAllPermissionsView();
        }

       public ICommand BtnSave
        {
            get { return new RelayCommand(Save, SaveCE); }
        }

       private bool SaveCE()
       {
           if (CurrentUserLevel != null && CurrentPermissionGroup != null)
               return true;
           else
               return false;
       }

       private void Save()
       {
           if (clsSecurity.GetSavePermission(103) && clsSecurity.GetUpdatePermission(103))
           {
               if (AllPermissionView.Count(c => c.user_level_id == CurrentUserLevel.user_level_id && c.Permission_Group_Code == CurrentPermissionGroup.Permission_Group_Code) > 0)
               {
                   List<dtl_Permission> Savelist = new List<dtl_Permission>();

                   foreach (var permission in AllPermissionView.Where(c => c.user_level_id == CurrentUserLevel.user_level_id && c.Permission_Group_Code == CurrentPermissionGroup.Permission_Group_Code))
                   {
                       dtl_Permission saveObj = new dtl_Permission();
                       saveObj.Permission_Code = (int)permission.Permission_Code;
                       saveObj.user_level_id = permission.user_level_id;
                       saveObj.Can_View = permission.Can_View;
                       saveObj.Can_Save = permission.Can_Save;
                       saveObj.Can_Update = permission.Can_Update;
                       saveObj.Can_Delete = permission.Can_Delete;
                       Savelist.Add(saveObj);
                   }

                   if (serviceClient.SavePermissions(Savelist.ToArray()))
                   {
                       New();
                       clsMessages.setMessage("Records saved/updated successfully");
                       clsSecurity.UserPermissions = serviceClient.GetDetailPermissionViewByUserLevel(clsSecurity.loggedUser == null ? Guid.Empty : (Guid)clsSecurity.loggedUser.user_level_id);
                   }
                   else
                   {
                       clsMessages.setMessage("Records save/update failed");
                   }
               }
               else
                   clsMessages.setMessage("Please select permissions and try again"); 
           }
           else
           {
               clsMessages.setMessage("You don't have permission to Save Or Update in this form...");
           }
       }

        public ICommand BtnDelete
       {
           get { return new RelayCommand(Delete, DeleteCE); }
       }

        private bool DeleteCE()
        {
            if (CurrentPermissionView != null)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(103))
            {
                if (serviceClient.DeletePermission(CurrentPermissionView))
                {
                    New();
                    clsMessages.setMessage("Record deleted successfully");
                    clsSecurity.UserPermissions = serviceClient.GetDetailPermissionViewByUserLevel(clsSecurity.loggedUser == null ? Guid.Empty : (Guid)clsSecurity.loggedUser.user_level_id);
                }
                else
                    clsMessages.setMessage("Record delete failed"); 
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete in this form...");
            }
        }

        
        #endregion

    }
}
