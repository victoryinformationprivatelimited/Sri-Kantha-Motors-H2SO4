using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.IO;
using ERP.Properties;
using ERP.Notification;
using System.Diagnostics;
using System.Threading;
using ERP.Security.MACSecurity;

namespace ERP
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private const double topOffset = 70;
        private const double leftOffset = 380;
        readonly GrowlNotifiactions growlNotifications = new GrowlNotifiactions();
        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        MACSecurityEncryption M;
        public string Description = "";
        clsNotificationViewer NotificationViewer = new clsNotificationViewer();
        ERPServiceClient serviceclient;

        public MainWindowViewModel()
        {
            ScaleSize = 1;
            serviceclient = new ERPServiceClient();

            RefreshBirthdays();
            RefreshConfirmationDay();
            RefreshNotification();

            M = new MACSecurityEncryption();
            this.setUserToSystem();
            this._LogOutBtn = new LogOutButton(this);
            growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
            growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;


            TimeSpan systemtime = DateTime.Now.TimeOfDay;
            if (systemtime <= new TimeSpan(12, 0, 0))
            {
                Description = "Good Morning";
            }
            else
            {
                Description = "Good Evening";
            }
            HelperClass.clsNotification.AddNotificationToList(1, Description + " " + clsSecurity.loggedUser.user_name.ToString(), "pack://application:,,,/Resources/notification-icon.png", "", "", "", "Welcome To H2SO4 ERP System." + System.Environment.NewLine + " System is Ready to Use .");
            myTimer.Tick += new EventHandler(TimerEventProcessor);
            myTimer.Interval = 5000;
            myTimer.Start();
            securityCheck();
            scale();
        }




        void securityCheck()
        {
            if (M.ADMIN == false)
                AdminV = Visibility.Collapsed;
            if (M.INVENTORY == false)
                Inventory = Visibility.Collapsed;
            if (M.MASTERS == false)
                Masters = Visibility.Collapsed;
            if (M.PRODUCTION == false)
                Production = Visibility.Collapsed;
            if (M.REPORTS == false)
                Report = Visibility.Collapsed;
            if (M.SALES == false)
                Sales = Visibility.Collapsed;
            if (M.HR == false)
                Hr = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool admin;
        public bool Admin
        {
            get
            {
                return this.admin;
            }
            set
            {
                this.admin = value;
            }
        }

        private bool payroll;
        public bool Payroll
        {
            get
            {
                return this.payroll;
            }
            set
            {
                this.payroll = value;
            }
        }

        private Visibility _Admin;
        public Visibility AdminV
        {
            get { return _Admin; }
            set { _Admin = value; OnPropertyChanged("AdminV"); }
        }

        private Visibility _Hr;
        public Visibility Hr
        {
            get { return _Hr; }
            set { _Hr = value; OnPropertyChanged("Hr"); }
        }

        private Visibility _Sales;
        public Visibility Sales
        {
            get { return _Sales; }
            set { _Sales = value; OnPropertyChanged("Sales"); }
        }

        private Visibility _Production;
        public Visibility Production
        {
            get { return _Production; }
            set { _Production = value; OnPropertyChanged("Production"); }
        }

        private Visibility _Report;
        public Visibility Report
        {
            get { return _Report; }
            set { _Report = value; OnPropertyChanged("Report"); }
        }

        private Visibility _Masters;
        public Visibility Masters
        {
            get { return _Masters; }
            set { _Masters = value; OnPropertyChanged("Masters"); }
        }

        private Visibility _Inventory;
        public Visibility Inventory
        {
            get { return _Inventory; }
            set { _Inventory = value; OnPropertyChanged("Inventory"); }
        }


        private Visibility _WindowVisibility;
        public Visibility WindowVisibility
        {
            get
            {
                return this._WindowVisibility;
            }
            set
            {
                this._WindowVisibility = value;
                OnPropertyChanged("WindowVisibility");
            }
        }

        private usr_User _LoggedUser;
        public usr_User LoggedUser
        {
            get
            {
                return this._LoggedUser;
            }
            set
            {
                this._LoggedUser = value;
                OnPropertyChanged("LoggedUser");
            }
        }

        private IEnumerable<usr_UserPermission> _LoggedUserPermossions;
        public IEnumerable<usr_UserPermission> LoggedUserPermossions
        {
            get { return this._LoggedUserPermossions; }
            set
            {
                this._LoggedUserPermossions = value;
                OnPropertyChanged("LoggedUserPermossions");
            }
        }

        private usr_UserLevel _LoggedUserLevel;
        public usr_UserLevel LoggedUserLevel
        {
            get
            {
                return this._LoggedUserLevel;
            }
            set
            {
                this._LoggedUserLevel = value;
                OnPropertyChanged("LoggedUser");
            }
        }

        private String _MessageLine;
        public String MessageLine
        {
            get { return this._MessageLine; }
            set { this._MessageLine = value; OnPropertyChanged("MessageLine"); }
        }

        private string _MessageImage;
        public string MessageImage
        {
            get
            {
                return this._MessageImage;
            }
            set
            {
                this._MessageImage = value;
                OnPropertyChanged("MessageImage");
            }
        }

        private LogOutButton _LogOutBtn;
        public LogOutButton LogOutBtn
        {
            get { return this._LogOutBtn; }
        }

        public class LogOutButton : ICommand
        {
            private MainWindowViewModel ViewModel;

            public LogOutButton(MainWindowViewModel ViewModel)
            {
                this.ViewModel = ViewModel;
                this.ViewModel.PropertyChanged += (s, e) =>
                {
                    if (this.CanExecuteChanged != null)
                    {
                        this.CanExecuteChanged(this, new EventArgs());
                    }
                };
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                this.ViewModel.logOutSystem();
            }
        }

        private void setUserToSystem()
        {
            clsMessages.Model = this;
            this.LoggedUser = clsSecurity.loggedUser;
            this.LoggedUserPermossions = clsSecurity.userPermissions;
            foreach (usr_UserLevel level in clsSecurity.loggedUserLevel)
            {
                this.LoggedUserLevel = level;
            }
            MessageLine = "Hi " + clsSecurity.loggedUser.name + " " + Resources.SystemReady;
        }

        private void logOutSystem()
        {
            HelperClass.clsNotification.NotificationList.Clear();
            ERPLogin login = new ERPLogin();
            login.Show();
        }

        public void NofificationView()
        {
            #region CreateBirthdayString

            StringBuilder sb = new StringBuilder();
            StringBuilder display = new StringBuilder();

            if (Birthday != null && Birthday.Count() != 0)
            {
                string day;
                string month;
                string year;
                int count = 0;

                foreach (var item in Birthday)
                {
                    day = item.birthday.Value.Day.ToString();
                    month = item.birthday.Value.Month.ToString();
                    year = item.birthday.Value.Year.ToString();
                    sb.AppendFormat(item.first_name + " " + item.second_name + " : " + month + "/" + day + "/" + year + "\n");

                    if (count < 3)
                    {
                        display.AppendFormat(item.first_name + " " + item.second_name + " : " + month + "/" + day + "/" + year + "\n");
                        count++;
                    }
                }

                if (count == 3 && Birthday.Count() > 3)
                    display.AppendFormat("And " + (Birthday.Count() - 3).ToString() + " Other(s)");

                z_Notifications notification = new ERPService.z_Notifications();
                notification.Notification = sb.ToString();
                notification.Title = "Birthday";
                notification.Status = false;
                notification.DateofNot = DateTime.Now;

                NotificationViewer.SaveNotification(notification);
            }

            #endregion

            #region CreateConfirmation
            StringBuilder sb1 = new StringBuilder();
            StringBuilder display1 = new StringBuilder();

            if (ConfirmationDay != null && ConfirmationDay.Count() != 0)
            {
                string day;
                string month;
                string year;
                int count = 0;

                foreach (var item in ConfirmationDay)
                {
                    day = item.prmernant_active_date.Value.Day.ToString();
                    month = item.prmernant_active_date.Value.Month.ToString();
                    year = item.prmernant_active_date.Value.Year.ToString();

                    sb1.AppendFormat(item.first_name + " " + item.second_name + " : " + month + "/" + day + "/" + year + "\n");

                    if (count < 3)
                    {
                        display1.AppendFormat(item.first_name + " " + item.second_name + " : " + month + "/" + day + "/" + year + "\n");
                        count++;
                    }
                }

                if (count == 3 && ConfirmationDay.Count() > 3)
                    display1.AppendFormat("And " + (ConfirmationDay.Count() - 3).ToString() + " Other(s)");

                z_Notifications notification = new ERPService.z_Notifications();
                notification.Notification = sb1.ToString();
                notification.Title = "Confirmation";
                notification.Status = false;
                notification.DateofNot = DateTime.Now;
                NotificationViewer.SaveNotification(notification);
            }

            #endregion

            #region DisplayNotification

            foreach (var item in HelperClass.clsNotification.NotificationList)
            {
                growlNotifications.AddNotification(new Notification.Notification { Title = item.Hedder, ImageUrl = item.Url, Message = item.Description });
            }

            if (Birthday != null && Birthday.Count() != 0)
            {
                //if (NotificationViewer.CheckNotification(sb.ToString()) == true)
                //{
                //    foreach (var item in HelperClass.clsNotification.NotificationList)
                //    {
                //        growlNotifications.AddNotification(new Notification.Notification { Title = "Birthday(s) for this week", ImageUrl = "pack://application:,,,/Resources/notification-icon.png", Message = sb.ToString() });
                //    }
                //}
                foreach (var item in HelperClass.clsNotification.NotificationList)
                {
                    growlNotifications.AddNotification(new Notification.Notification { Title = "Birthday(s) for this week", ImageUrl = "pack://application:,,,/Resources/notification-icon.png", Message = display.ToString() });
                }
            }

            if (ConfirmationDay != null && ConfirmationDay.Count() != 0)
            {
                //if (NotificationViewer.CheckNotification(sb1.ToString()) == true)
                //{
                //    foreach (var item in HelperClass.clsNotification.NotificationList)
                //    {
                //        growlNotifications.AddNotification(new Notification.Notification { Title = "Permanent in this month", ImageUrl = "pack://application:,,,/Resources/notification-icon.png", Message = sb1.ToString() });
                //    }
                //}
                foreach (var item in HelperClass.clsNotification.NotificationList)
                {
                    growlNotifications.AddNotification(new Notification.Notification { Title = "Permanent in this month", ImageUrl = "pack://application:,,,/Resources/notification-icon.png", Message = display1.ToString() });
                }
            }
            #endregion
        }

        #region refreshmethod
        private void RefreshNotification()
        {
            try
            {
                serviceclient.GetNotificationsCompleted += (s, e) =>
                {
                    Notification = e.Result;
                    NotificationViewer.Notification = Notification;
                };
                serviceclient.GetNotificationsAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RefreshConfirmationDay()
        {
            try
            {
                serviceclient.GetConfirmationCompleted += (s, e) =>
                {
                    ConfirmationDay = e.Result;
                    NotificationViewer.ConfirmationDay = ConfirmationDay;
                };
                serviceclient.GetConfirmationAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RefreshBirthdays()
        {
            try
            {
                serviceclient.GetBirthdayCompleted += (s, e) =>
                {
                    try
                    {
                        Birthday = e.Result;
                        NotificationViewer.Birthday = Birthday;
                    }
                    catch (Exception)
                    {

                    }
                };
                serviceclient.GetBirthdayAsync();
            }
            catch (Exception)
            {
            }
        }


        private IEnumerable<GetBirthDaysWeek> birthday;
        public IEnumerable<GetBirthDaysWeek> Birthday
        {
            get { return birthday; }
            set { birthday = value; OnPropertyChanged("Birthday"); }
        }

        private IEnumerable<GetConfirmationDetail> confirmationDay;
        public IEnumerable<GetConfirmationDetail> ConfirmationDay
        {
            get { return confirmationDay; }
            set { confirmationDay = value; OnPropertyChanged("ConfirmationDay"); }
        }

        private IEnumerable<z_Notifications> notification;
        public IEnumerable<z_Notifications> Notification
        {
            get { return notification; }
            set { notification = value; OnPropertyChanged("Notification"); }
        }

        #endregion

        private void TimerEventProcessor(object sender, EventArgs e)
        {
            myTimer.Stop();
            NofificationView();
            //myTimer.Interval = 50000;
            //myTimer.Start();

        }

        private double _scaleSize;

        public double ScaleSize
        {
            get { return _scaleSize; }
            set { _scaleSize = value; OnPropertyChanged("ScaleSize"); }
        }



        public void scale()
        {
            double w = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (w <= 1024)
                ScaleSize = 0.85;
            if (w == 1366)
                ScaleSize = 0.98;
        }
    }
}
