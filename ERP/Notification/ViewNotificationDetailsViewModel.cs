using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace ERP.Notification
{
    class ViewNotificationDetailsViewModel : ViewModelBase
    {
        #region Service object

        ERPServiceClient serviceClient;
        clsNotificationViewer NotificationViewer = new clsNotificationViewer();

        #endregion

        #region Constructor

        public ViewNotificationDetailsViewModel()
        {
            serviceClient = new ERPServiceClient();
            ResfershNotification();

        }

        #endregion

        #region PropertiesforDesignation

        private IEnumerable<z_Notifications> notification;
        public IEnumerable<z_Notifications> Notification
        {
            get { return notification; }
            set { notification = value; OnPropertyChanged("Notification");}
        }

        private z_Notifications currentNotifications;
        public z_Notifications CurrentNotifications
        {
            get { return currentNotifications; }
            set
            {
                currentNotifications = value;
            }
        }

        #endregion

        #region refresh method

        private void ResfershNotification()
        {
            serviceClient.GetNotificationsCompleted += (s, e) =>
            {
                Notification = e.Result.Where(c=> c.Status == false).OrderByDescending(c=>c.DateofNot).OrderBy(c=>c.Title);
            };
            serviceClient.GetNotificationsAsync();
        }

        #endregion

        #region Icommand

        public ICommand UpdateBtn
        {
            get{return new RelayCommand(Update, updateCanExecute);}
        }

        public void UpdateRun() 
        {
            if(updateCanExecute())
            Update();
        }

        private void Update()
        {
            if (CurrentNotifications != null)
            {
                z_Notifications notification = new z_Notifications();
                notification.Notification_Id = CurrentNotifications.Notification_Id;
                notification.Notification = CurrentNotifications.Notification;
                notification.Title = CurrentNotifications.Title;
                notification.DateofNot = CurrentNotifications.DateofNot;
                notification.Status = true;
                notification.ViewDate = DateTime.Now;

                if (serviceClient.updateNotification(notification))
                {
                    MessageBox.Show("Do you want to remove this Reminder?", "Help Caption", MessageBoxButton.OK, MessageBoxImage.Question);
                }
                else
                {
                    MessageBox.Show("Remove failed");
                }

                ResfershNotification();
            }
        }

        private bool updateCanExecute()
        {
            if (CurrentNotifications != null)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}

