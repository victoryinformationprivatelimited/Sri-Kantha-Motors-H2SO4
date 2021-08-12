using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;

namespace ERP.Notification
{
    public class clsNotificationViewer : ViewModelBase
    {
        #region Service Object

        ERPServiceClient serviceClient;

        #endregion

        #region properties
        private IEnumerable<z_Notifications> notification;
        public IEnumerable<z_Notifications> Notification
        {
            get { return notification; }
            set { notification = value; OnPropertyChanged("Notification"); }
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
            set { confirmationDay = value; OnPropertyChanged("ConfimationDay"); }
        }

        #endregion

        #region Refresh method

        private void RefreshNotification()
        {
            try
            {
                serviceClient.GetNotificationsCompleted += (s, e) =>
                {
                    Notification = e.Result;
                };
                serviceClient.GetNotificationsAsync();
            }
            catch (Exception)
            {
            }
        }


        private void RefreshConfirmationDay()
        {
            try
            {
                serviceClient.GetConfirmationCompleted += (s, e) =>
                {
                    ConfirmationDay = e.Result;
                };
                serviceClient.GetConfirmationAsync();
            }
            catch (Exception)
            {
             
            }
        }

        private void RefreshBirthdays()
        {
            try
            {
                serviceClient.GetBirthdayCompleted += (s, e) =>
                {
                    Birthday = e.Result;
                };

                serviceClient.GetBirthdayAsync();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Constructor

        public clsNotificationViewer()
        {
            serviceClient = new ERPServiceClient();
            //RefreshNotification();
            //RefreshConfirmationDay();
            //RefreshBirthdays();
        }
       
        #endregion

        #region Methods

        public bool CheckNotification(string message)
        {
            if (Notification.Count(c => c.Notification == message) == 0)
                return true;
            else
                return false;
        }

        public void SaveNotification(z_Notifications notifications)
        {
            try
            {
                if (Birthday != null || ConfirmationDay != null)
                {
                    if (Notification.Count(c => c.Notification == notifications.Notification.ToString()) == 0)
                    {
                        serviceClient.saveNotification(notifications);
                    }
                }

            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
