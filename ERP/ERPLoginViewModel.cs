/// <summary of Creation>
/// Create By : CHAMARA HERATH.
/// Date : 2013-04-02.
/// Perpose : ERP User Login.
/// </summary of Creation>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using ERP.HelperClass;
using ERP.Base;
using System.Configuration;

namespace ERP
{
    class ERPLoginViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion

        #region

        public ERPLoginViewModel()
        {
            Refresh();
            CurrentUser = new usr_User();
            Windowvisible = Visibility.Visible;
            
        }

        #endregion

        #region Properties

        private IEnumerable<usr_User> _User;
        public IEnumerable<usr_User> User
        {
            get { return _User; }
            set { _User = value; OnPropertyChanged("User"); }
        }

        private usr_User _CurrentUser;
        public usr_User CurrentUser
        {
            get { return _CurrentUser; }
            set { _CurrentUser = value; OnPropertyChanged("CurrentUser"); }
        }

        private Visibility _Windowvisible;
        public Visibility Windowvisible
        {
            get { return _Windowvisible; }
            set { _Windowvisible = value; OnPropertyChanged("Windowvisible"); }
        }


        #endregion

        #region Refresh Method
        void Refresh()
        {
            try
            {
                // this.serviceClient.GetUsersCompleted += (s, e) =>
                //  {
                // User = e.Result;
                User = serviceClient.GetUsers();
                // };
                // this.serviceClient.GetUsersAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessageLogin("Please Restart H2SO4");
            }
        }

        #endregion

        #region Commands & Methods

        public ICommand btnLogin
        {
            get { return new RelayCommand(CheckLogin); }
        }

        private void CheckLogin()
        {
            if (CurrentUser.user_name != null && CurrentUser.user_password != null)
            {
                usr_User user = User.FirstOrDefault(c => c.user_name == CurrentUser.user_name);
                if (user != null)
                {
                    string password = StringCipher.Decrypt(user.user_password, "12");
                    if (password == CurrentUser.user_password)
                    {
                        CurrentUser = User.FirstOrDefault(c => c.user_name == CurrentUser.user_name);
                        assignUser();
                        MainWindow mv = new MainWindow();
                        this.Windowvisible = Visibility.Hidden;


                        string cryptedDate = serviceClient.GET_LOGIN_date();

                        //---------------------------------------------------------------------------------

                        var lastLoginDate_dey = StringCipher.Decrypt(cryptedDate, "12");

                        DateTime dd1 = DateTime.Parse(lastLoginDate_dey);
                        var Decryptcode = StringCipher.Decrypt(ConfigurationManager.AppSettings["EXP_Key"], "12");

                        DateTime EXPdate = DateTime.Parse(Decryptcode);


                        if (dd1 < DateTime.Now)
                        {
                            if (EXPdate > DateTime.Now)
                            {
                                // MainWindow mv = new MainWindow();
                                this.Windowvisible = Visibility.Hidden;
                                mv.Show();
                                Refresh();
                                var last_login_date = StringCipher.Encrypt(DateTime.Now.ToString(), "12");
                                serviceClient.updateLOGIN_date(last_login_date);
                            }
                            else
                            {
                                MessageBox.Show("System Expired. Please contact System Administrator");

                                ERPLogin sh = new ERPLogin();
                                sh.Show();
                            }

                        }

                        else
                        {
                            MessageBox.Show("Please Check your PC date");
                            ERPLogin sh = new ERPLogin();
                            sh.Show();

                        }


                        //mv.Show();
                        Refresh();
                    }
                    else
                    {
                        clsMessages.setMessageLogin("The Password Which You Were Entered Was Incorrect. Please Check Your Password And Enter Again...");
                        // MessageBox.Show("The Password Which You Were Entered Was Incorrect. Please Check Your Password And Enter Again...");
                    }
                }
                else
                {
                    clsMessages.setMessageLogin("Your Username Is Invalid.. Please Enter A Valid Username To Login");
                    // MessageBox.Show("Your Username Is Invalid.. Please Enter A Valid Username To Login");
                }
            }
            else
            {
                clsMessages.setMessageLogin("Please Enter Username And Password Before Clicking The Login Button");
                // MessageBox.Show("Please Enter Username And Password Before Clicking The Login Button");
            }
        }

        //private bool CheckLoginCE(object obj)
        //{
        //    if (User != null && CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.user_name))
        //        return true;
        //    else
        //        return false;
        //}

        //private void CheckLogin(object password)
        //{
        //    if (User.Count(c => c.user_name == CurrentUser.user_name) > 0)
        //    {
        //        if (User.Count(c => c.user_name == CurrentUser.user_name && c.user_password == (password == null ? string.Empty : ((PasswordBox)password).Password)) > 0)
        //        {
        //            CurrentUser = User.FirstOrDefault(c => c.user_name == CurrentUser.user_name);
        //            assignUser();
        //            MainWindow mv = new MainWindow();
        //            this.Windowvisible = Visibility.Hidden;
        //            mv.Show();
        //            Refresh();
        //        }
        //        else
        //            MessageBox.Show("Invalid Username or Password...");
        //    }
        //    else
        //        MessageBox.Show("Invalid Username or Password...");
        //    //clsMessages.setMessage("Invalid Username or Password");

        //    // MessageBox.Show(((PasswordBox)password).Password);
        //}

        private void assignUser()
        {
            clsSecurity.loggedUser = CurrentUser;
            clsSecurity.loggedEmployee = this.serviceClient.GetUserEmployee(CurrentUser.user_id);
            clsSecurity.loggedUserLevel = this.serviceClient.GetUserLevelById((Guid)CurrentUser.user_level_id);
            clsSecurity.moduleSupervision = this.serviceClient.GetAllEmployeeSupervisorByUserEmployee(clsSecurity.loggedEmployee == null ? Guid.Empty : (Guid)clsSecurity.loggedEmployee.employee_id);
            clsSecurity.SetUserPermissions(this.serviceClient.GetUserPermissionForBack());
            clsSecurity.UserPermissions = serviceClient.GetDetailPermissionViewByUserLevel(clsSecurity.loggedUser == null ? Guid.Empty : (Guid)clsSecurity.loggedUser.user_level_id);

        }

        #endregion
    }
}

