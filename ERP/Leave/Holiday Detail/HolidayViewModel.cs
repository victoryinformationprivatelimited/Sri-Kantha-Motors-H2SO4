using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Leave.Holiday_Detail
{
    public class HolidayViewModel : ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        public HolidayViewModel()
        {
            RefreshHoliday();
        }
        private IEnumerable<z_Holiday> _Holidays;
        public IEnumerable<z_Holiday> Holidays
        {
            get { return _Holidays; }
            set { _Holidays = value; this.OnPropertyChanged("Holidays"); }
        }
        private z_Holiday _CurrentHoliday;
        public z_Holiday CurrentHoliday
        {
            get { return _CurrentHoliday; }
            set { _CurrentHoliday = value; this.OnPropertyChanged("CurrentHoliday"); }
        }

        public ICommand NewBtn
        {
            get { return new RelayCommand(NewHoliday, NewHolidayCanExecute); }
        }
        public ICommand DeleteBtn
        {
            get { return new RelayCommand(DeleteHoliday, DeleteCanExecute); }
        }

        private void DeleteHoliday()
        {
            //if (clsSecurity.GetPermissionForDelete(clsConfig.GetViewModelId(Viewmodels.City), clsSecurity.loggedUser.user_id))
            //{
            z_Holiday holiday = new z_Holiday();
            CurrentHoliday.isActive = false;
            holiday = CurrentHoliday;
             MessageBoxResult Result = new MessageBoxResult();
                Result = MessageBox.Show("Are you sure delete this recode ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    if (serviceClient.DeleteHoliday(holiday))
                    {
                        NewHoliday();
                        MessageBox.Show("Holiday Delete Successfully");

                    }
                }
           // }
        }

        private bool DeleteCanExecute()
        {
            if (CurrentHoliday != null)
            {
                return true;
            }
            return false;
        }

        private void NewHoliday()
        {
            this.CurrentHoliday = null;
            CurrentHoliday = new z_Holiday();
            CurrentHoliday.holiday_id = Guid.NewGuid();
            RefreshHoliday();
        }

        private bool NewHolidayCanExecute()
        {
            return true;
        }

        public ICommand SaveBtn
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }

        private bool SaveCanExecute()
        {
            if (CurrentHoliday != null)
            {
                return true;
            }
            return false;
           
        }

        private void Save()
        {
            bool isUpdate = false;
            CurrentHoliday.isActive = true;
            IEnumerable<z_Holiday> holiday = this.serviceClient.GetHolidays();
            if (holiday != null)
            {
                foreach (z_Holiday holiday_itm in holiday)
                {
                    if (holiday_itm.holiday_id == CurrentHoliday.holiday_id)
                    {
                        isUpdate = true;
                    }
                }
            }

            if (CurrentHoliday != null && CurrentHoliday.holiday_Date != null)
            {
                if (isUpdate)
                {

                   if(serviceClient.UpdateHoliday(CurrentHoliday))
                   {
                       NewHoliday();
                       MessageBox.Show("Holiday Update Successfully");
                   }
                   
                }
                else
                {

                    if (serviceClient.SaveHoliday(CurrentHoliday))
                    {
                        NewHoliday();
                        MessageBox.Show("Holiday added Successfully");
                    }

                }
            }
          
        }

        private void RefreshHoliday()
        {
            this.serviceClient.GetHolidaysCompleted += (s, e) =>
            {
                this.Holidays = e.Result.Where(a => a.isActive == true);
            };
            this.serviceClient.GetHolidaysAsync();
        }

    }
}