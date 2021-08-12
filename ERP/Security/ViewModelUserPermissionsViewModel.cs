using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace ERP.Security
{
    public class ViewModelUserPermissionsViewModel : ViewModelBase
    {
        ERPServiceClient serviceClient = new ERPServiceClient();
        List<ViewModelUserPermission_View> tempUserPermission = new List<ViewModelUserPermission_View>();
        List<usr_UserPermissionViewModel> TempSaveList = new List<usr_UserPermissionViewModel>();
        List<usr_UserPermissionViewModel> SaveList = new List<usr_UserPermissionViewModel>();
        List<usr_UserPermissionViewModel> UpdateList = new List<usr_UserPermissionViewModel>();


        public ViewModelUserPermissionsViewModel()
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.UserPermissionMaster), clsSecurity.loggedUser.user_id))
            {
                RefreshUserLevel();
                RefreshUser();
                RefreshModule();
                RefreshViewModel();
                RefreshZViewModel();
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private IEnumerable<usr_UserLevel> _UserLevel;
        public IEnumerable<usr_UserLevel> UserLevel
        {
            get { return _UserLevel; }
            set { _UserLevel = value; this.OnPropertyChanged("UserLevel"); }
        }

        private usr_UserLevel _CurrentUserLevel;
        public usr_UserLevel CurrentUserLevel
        {
            get { return _CurrentUserLevel; }
            set { _CurrentUserLevel = value; this.OnPropertyChanged("CurrentUserLevel");

            if (CurrentUserLevel != null)
            {
                Users = Users.Where(a => a.user_level_id == CurrentUserLevel.user_level_id);
            }
           }
        }

        private IEnumerable<usr_User> _Users;
        public IEnumerable<usr_User> Users
        {
            get { return _Users; }
            set { _Users = value; this.OnPropertyChanged("Users"); }
        }

        private usr_User _CurrentUsers;
        public usr_User CurrentUsers
        {
            get { return _CurrentUsers; }
            set { _CurrentUsers = value; this.OnPropertyChanged("CurrentUsers");
            if (CurrentUsers != null)
            {
                ViewMpdels = ViewMpdels.Where(z => z.user_id == CurrentUsers.user_id);
                SelectionItems = null;
                SelectionItems = ViewMpdels.ToList();
                tempUserPermission = ViewMpdels.ToList();
            }
            }
        }

        private IEnumerable<z_module> _Module;
        public IEnumerable<z_module> Module
        {
            get { return _Module; }
            set { _Module = value; this.OnPropertyChanged("Module"); }
        }

        private z_module _CurrentModule;
        public z_module CurrentModule
        {
            get { return _CurrentModule; }
            set { _CurrentModule = value; this.OnPropertyChanged("CurrentModule"); }
        }

        private IEnumerable<z_ViewModel> _ViewModels;
        public IEnumerable<z_ViewModel> ViewModels
        {
            get { return _ViewModels; }
            set { _ViewModels = value; OnPropertyChanged("ViewModels"); }
        }

        private z_ViewModel _CurrentViewModel;
        public z_ViewModel CurrentViewModel
        {
            get { return _CurrentViewModel; }
            set { _CurrentViewModel = value; OnPropertyChanged("CurrentViewModel"); }
        }

        private IEnumerable<ViewModelUserPermission_View> _ViewMpdels;
        public IEnumerable<ViewModelUserPermission_View> ViewMpdels
        {
            get { return _ViewMpdels; }
            set { _ViewMpdels = value; this.OnPropertyChanged("ViewMpdels"); }
        }

        private List<ViewModelUserPermission_View> _SelectionItems = new List<ViewModelUserPermission_View>();
        public List<ViewModelUserPermission_View> SelectionItems
        {
            get { return this._SelectionItems; }
            set
            {
                this._SelectionItems = value;
                this.OnPropertyChanged("SelectionItems");
            }
        }
        

        public bool AddModuleCanExicute()
        {
            if (CurrentUsers == null)
            {
                return false;
            }
            if (CurrentModule == null)
            {
                return false;
            }
            return true;
        }

        public ICommand AddModuleButton
        {
            get { return new RelayCommand(addAll, AddModuleCanExicute);}

        }
        

        private void addAll()
        {
            List<ViewModelUserPermission_View> oldView = new List<ViewModelUserPermission_View>();
            List<ViewModelUserPermission_View> oldtempPermision = new List<ViewModelUserPermission_View>();
          
            oldtempPermision = SelectionItems.Where(z => z.module_id == CurrentModule.module_id).ToList();
            oldView = SelectionItems.Where(z => z.module_id == CurrentModule.module_id).ToList();

            if (oldView.Count > 0)
            {
                MessageBox.Show("Current Module is Allredy in List");
            }
            else
            {
                if (oldtempPermision.Count > 0)
                {
                    MessageBox.Show("Current Module is Allredy in List");
                }
                else
                {
                    foreach (var item in ViewModels.Where(z => z.module_id == CurrentModule.module_id).ToList())
                    {
                        ViewModelUserPermission_View tempView = new ViewModelUserPermission_View();
                        tempView.view_model_id = item.view_model_id;
                        tempView.view_model_name = item.view_model_name;
                        tempView.user_id = CurrentUsers.user_id;
                        tempView.module_id = CurrentModule.module_id;
                        tempView.canview = false;
                        tempView.cansave = false;
                        tempView.canmodify = false;
                        tempView.candelete = false;
                        tempUserPermission.Add(tempView);
                    }
                    SelectionItems = null;
                    SelectionItems = tempUserPermission;
                }
            }

           
           
        }
        
        
        
        public void RefreshUserLevel()
        {
            this.serviceClient.GetUserLevelCompleted += (s, e) =>
            {
                this.UserLevel = e.Result;
            };
            this.serviceClient.GetUserLevelAsync();
        }

        public void RefreshUser()
        {
            this.serviceClient.GetUsersCompleted += (s, e) =>
            {
                this.Users = e.Result;
            };
            this.serviceClient.GetUsersAsync();
        }

        public void RefreshModule()
        {
            this.serviceClient.GetModulesCompleted += (s, e) =>
            {
                this.Module = e.Result;
            };
            this.serviceClient.GetModulesAsync();
        }


        public void RefreshViewModel()
        {
            this.serviceClient.GetViewModelUserPermissiosnsCompleted += (s, e) =>
            {
                this.ViewMpdels = e.Result;
            };
            this.serviceClient.GetViewModelUserPermissiosnsAsync();
        }
           public void RefreshZViewModel()
        {
            this.serviceClient.GetViewModelsCompleted += (s, e) =>
            {
                this.ViewModels = e.Result;
            };
            this.serviceClient.GetViewModelsAsync();
        }



           public bool newButtonCanExicute()
           {
             return true;
           }

           public ICommand newButton
           {
               get { return new RelayCommand(New, newButtonCanExicute); }

           }

           private void New()
           {
               SelectionItems.Clear();
               tempUserPermission.Clear();
               SelectionItems = null;
               SelectionItems = tempUserPermission;
               CurrentModule = null;
               CurrentUsers = null;
               RefreshViewModel();
           }

           public bool BtnSavaCanExicute()
           {
               if (CurrentUsers == null)
               {
                   return false;
               }
               if (CurrentModule == null)
               {
                   return false;
               }
               if (SelectionItems ==null)
               {
                   return false;
               }
             return true;
           }

         public ICommand BtnSava
           {
               get { return new RelayCommand(Save, BtnSavaCanExicute); }

           }

         private void Save()
         {
             if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.UserPermissionMaster), clsSecurity.loggedUser.user_id))
             {
                 List<ViewModelUserPermission_View> CurrntPermissionList = serviceClient.GetViewModelUserPermissiosns().ToList();
                 foreach (var itemSelect in SelectionItems)
                 {
                     ViewModelUserPermission_View currentPermision = CurrntPermissionList.FirstOrDefault(z => z.module_id == itemSelect.module_id &&
                         z.user_id == CurrentUsers.user_id && z.view_model_id == itemSelect.view_model_id);

                     if (currentPermision != null)
                     {
                         usr_UserPermissionViewModel permision = new usr_UserPermissionViewModel();
                         permision.view_model_id = itemSelect.view_model_id;
                         permision.user_id = CurrentUsers.user_id;
                         permision.canview = itemSelect.canview;
                         permision.cansave = itemSelect.cansave;
                         permision.canmodify = itemSelect.canmodify;
                         permision.candelete = itemSelect.candelete;
                         permision.save_user_id = clsSecurity.loggedUser.user_id;
                         permision.save_datetime = System.DateTime.Now;
                         permision.modified_datetime = System.DateTime.Now;
                         permision.modified_user_id = clsSecurity.loggedUser.user_id;
                         permision.delete_datetime = System.DateTime.Now;
                         permision.delete_user_id = clsSecurity.loggedUser.user_id;
                         permision.isdelete = false;
                         UpdateList.Add(permision);

                     }
                     else
                     {
                         usr_UserPermissionViewModel permision = new usr_UserPermissionViewModel();
                         permision.view_model_id = itemSelect.view_model_id;
                         permision.user_id = CurrentUsers.user_id;
                         permision.canview = itemSelect.canview;
                         permision.cansave = itemSelect.cansave;
                         permision.canmodify = itemSelect.canmodify;
                         permision.candelete = itemSelect.candelete;
                         permision.save_user_id = clsSecurity.loggedUser.user_id;
                         permision.save_datetime = System.DateTime.Now;
                         permision.modified_datetime = System.DateTime.Now;
                         permision.modified_user_id = clsSecurity.loggedUser.user_id;
                         permision.delete_datetime = System.DateTime.Now;
                         permision.delete_user_id = clsSecurity.loggedUser.user_id;
                         permision.isdelete = false;
                         SaveList.Add(permision);
                     }

                 }
                 if (SaveList.Count > 0)
                 {
                     if (serviceClient.SaveEmployeePermission(SaveList.ToArray()))
                     {
                         MessageBox.Show(SaveList.Count + "    Recode Save Sussfully");
                     }
                     else
                     {
                         MessageBox.Show("Recode Save Fail");
                     }
                 }
                 if (UpdateList.Count > 0)
                 {
                     if (serviceClient.UpdatePermission(UpdateList.ToArray()))
                     {
                         MessageBox.Show(UpdateList.Count + "   Recode Update Sussfully");
                     }
                     else
                     {
                         MessageBox.Show("Recode Update Fail");
                     }
                 }
                 this.New();

             }
             else
             {
                 clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
             }
         }
        
       
    
        
    }
}
