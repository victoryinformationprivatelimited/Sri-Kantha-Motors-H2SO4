using ERP.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERP.HelperClass
{
    public static class clsNotification
    {
        public static List<clsNotificationList> NotificationList=new List<clsNotificationList>();
        //private const double topOffset = 70;
        //private const double leftOffset = 380;
        //readonly GrowlNotifiactions growlNotifications = new GrowlNotifiactions();

        //public clsNotification()
        //{
        //    growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
        //    growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;
        //}
        public static bool AddNotificationToList( int id,string hedder,string url, string empid, string employeename, string applydate, string description)
        {
            try
            {
                clsNotificationList Nlist = new clsNotificationList();
                Nlist.Id = id;
                Nlist.Url = url;
                Nlist.Hedder = hedder;
                Nlist.EmployeeId = empid;
                Nlist.EmployeeName = employeename;
                Nlist.Applyeddate = applydate;
                Nlist.Description = description;
                NotificationList.Add(Nlist);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public static bool RemoveNotification(int id)
        {
            try
            {
                foreach (var item in NotificationList)
                {
                    if (item.Id == id)
                    {
                        NotificationList.Remove(item);
                    }

                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public static List<clsNotificationList> GetReturnNotificationList()
        {
            return NotificationList;
        }

        //public static void AddNotify()
        //{
        //    growlNotifications.AddNotification(new Notification.Notification { Title = "Mesage #1", ImageUrl = "pack://application:,,,/Resources/notification-icon.png", Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
        //    growlNotifications.AddNotification(new Notification.Notification { Title = "Mesage #1", ImageUrl = "pack://application:,,,/Resources/facebook-button.png", Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
        //    growlNotifications.AddNotification(new Notification.Notification { Title = "Mesage #1", ImageUrl = "pack://application:,,,/Resources/facebook-button.png", Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
        //    growlNotifications.AddNotification(new Notification.Notification { Title = "Mesage #1", ImageUrl = "pack://application:,,,/Resources/notification-icon.png", Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
        //    growlNotifications.AddNotification(new Notification.Notification { Title = "Mesage #1", ImageUrl = "pack://application:,,,/Resources/facebook-button.png", Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
        //    growlNotifications.AddNotification(new Notification.Notification { Title = "Mesage #1", ImageUrl = "pack://application:,,,/Resources/facebook-button.png", Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
        //}

    }
}
