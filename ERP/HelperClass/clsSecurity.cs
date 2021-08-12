using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows;

namespace ERP
{
    public static class clsSecurity
    {
        
        public static usr_User loggedUser = null;
        public static usr_UserEmployee loggedEmployee = null;
        public static IEnumerable<EmployeeSupervisorsView> moduleSupervision;
        public static IEnumerable<usr_UserLevel> loggedUserLevel;
        public static IEnumerable<usr_UserPermission> userPermissions = null;
        public static IEnumerable<z_module> companyModules = null;
        public static IEnumerable<usr_UserPermissionViewModel> userPermissionsViewModules= null;
        public static IEnumerable<dtl_PermissionView> UserPermissions;

        public static bool GetPermissionForView(Guid ViewModel_Id,Guid UserID)
        {
            //if (userPermissionsViewModules != null)
            //{
            //    try
            //    {
            //        bool result = (bool)userPermissionsViewModules.First(e => e.view_model_id.Equals(ViewModel_Id) && e.user_id.Equals(UserID)).canview;
            //        if(!result)
            //        MessageBox.Show(Properties.Resources.NoPermissionForView, Properties.Resources.ERPCaption, MessageBoxButton.OK, MessageBoxImage.Warning);

            //        return result;
            //    }
            //    catch (Exception)
            //    {
            //        return false;    
            //    }
            //}
            //return false;
            return true;
        }

        public static bool GetPermissionForSave(Guid ViewModel_Id, Guid UserID)
        {
            //if (userPermissionsViewModules != null)
            //{
            //    try
            //    {
            //        bool result = (bool)userPermissionsViewModules.First(e => e.view_model_id.Equals(ViewModel_Id) && e.user_id.Equals(UserID)).cansave;
            //        if (!result)
            //            MessageBox.Show(Properties.Resources.NoPermissionForSave, Properties.Resources.ERPCaption, MessageBoxButton.OK, MessageBoxImage.Warning);

            //        return result;
            //    }
            //    catch (Exception)
            //    {
            //        return false;
            //    }
            //}
            //return false;
            return true;
        }

        public static bool GetPermissionForUpdate(Guid ViewModel_Id, Guid UserID)
        {
            //if (userPermissionsViewModules != null)
            //{
            //    try
            //    {
            //        bool result = (bool)userPermissionsViewModules.First(e => e.view_model_id.Equals(ViewModel_Id) && e.user_id.Equals(UserID)).canmodify;
            //        if (!result)
            //            MessageBox.Show(Properties.Resources.NoPermissionForUpdate, Properties.Resources.ERPCaption, MessageBoxButton.OK, MessageBoxImage.Warning);

            //        return result;
            //    }
            //    catch (Exception)
            //    {
            //        return false;
            //    }
            //}
            //return false;
            return true;
        }

        public static bool GetPermissionForDelete(Guid ViewModel_Id, Guid UserID)
        {
            //if (userPermissionsViewModules != null)
            //{
            //    try
            //    {
            //        bool result = (bool)userPermissionsViewModules.First(e => e.view_model_id.Equals(ViewModel_Id) && e.user_id.Equals(UserID)).candelete;
            //        if (!result)
            //            MessageBox.Show(Properties.Resources.NoPermissionForDelete, Properties.Resources.ERPCaption, MessageBoxButton.OK, MessageBoxImage.Warning);

            //        return result;
            //    }
            //    catch (Exception)
            //    {
            //        return false;
            //    }
            //}
            //return false;
            return true;
        }

        public static void SetUserPermissions(IEnumerable<usr_UserPermissionViewModel> permissions)
        {
            userPermissionsViewModules = permissions;
        }

        public static bool GetViewPermission(int PermissionCode) 
        {
            if (UserPermissions != null && UserPermissions.Count() > 0)
            {
                if (UserPermissions.Count(c => c.Permission_Code == PermissionCode && c.Can_View == true) > 0)
                    return true;
                else
                    return false;
            }

            else
                return false;
        }

        public static bool GetSavePermission(int PermissionCode)
        {
            if (UserPermissions != null && UserPermissions.Count() > 0)
            {
                if (UserPermissions.Count(c => c.Permission_Code == PermissionCode && c.Can_Save == true) > 0)
                    return true;
                else
                    return false;
            }

            else
                return false;
        }

        public static bool GetUpdatePermission(int PermissionCode)
        {
            if (UserPermissions != null && UserPermissions.Count() > 0)
            {
                if (UserPermissions.Count(c => c.Permission_Code == PermissionCode && c.Can_Update == true) > 0)
                    return true;
                else
                    return false;
            }

            else
                return false;
        }

        public static bool GetDeletePermission(int PermissionCode)
        {
            if (UserPermissions != null && UserPermissions.Count() > 0)
            {
                if (UserPermissions.Count(c => c.Permission_Code == PermissionCode && c.Can_Delete == true) > 0)
                    return true;
                else
                    return false;
            }

            else
                return false;
        }

    }
}
