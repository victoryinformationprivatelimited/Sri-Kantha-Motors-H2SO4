using ERP.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class HolidayTypeViewModel:ViewModelBase
    {
        #region Attendance Service Client

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor

        public HolidayTypeViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshHolidayTypes();
            this.New();
        }
        
        #endregion

        #region Properties

        IEnumerable<z_HolidayType> holidayTypes;
        public IEnumerable<z_HolidayType> HolidayTypes
        {
            get { return holidayTypes; }
            set { holidayTypes = value; OnPropertyChanged("HolidayTypes"); }
        }

        z_HolidayType currentHolidayType;
        public z_HolidayType CurrentHolidayType
        {
            get { return currentHolidayType; }
            set { currentHolidayType = value; OnPropertyChanged("CurrentHolidayType"); }
        }

        string holidayTypeName;
        public string HolidayTypeName
        {
            get { return holidayTypeName; }
            set { holidayTypeName = value; OnPropertyChanged("HolidayTypeName"); }
        }

        bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged("IsActive"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshHolidayTypes()
        {
            attendServiceClient.GetHolidayTypesBasicDetailsCompleted += (s, e) =>
            {
                HolidayTypes = e.Result;
            };
            attendServiceClient.GetHolidayTypesBasicDetailsAsync();
        }

        #endregion

        #region Button Methods

        #region Save Button
        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if(currentHolidayType != null)
            {
                if(currentHolidayType.holiday_type_id == 0)         // adding new holiday type
                {
                    if(IsValidHolidayTypeName())
                    {
                        if (clsSecurity.GetSavePermission(307))
                        {
                            z_HolidayType addingType = new z_HolidayType();
                            addingType.holiday_type = holidayTypeName;
                            addingType.is_active = isActive;
                            addingType.is_delete = false;
                            addingType.save_datetime = DateTime.Now;
                            addingType.save_user_id = clsSecurity.loggedUser.user_id;

                            if (attendServiceClient.SaveHolidayType(addingType))
                            {
                                clsMessages.setMessage("Holiday type is saved successfully");
                                this.RefreshHolidayTypes();
                                this.New();
                            }
                            else
                            {
                                clsMessages.setMessage("Holiday type save is failed");
                            }
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }
                else                                              // update existing holiday type
                {
                    if(IsValidHolidayTypeName())
                    {
                        if (clsSecurity.GetUpdatePermission(307))
                        {
                            z_HolidayType updatingType = new z_HolidayType();
                            updatingType.holiday_type_id = currentHolidayType.holiday_type_id;
                            updatingType.holiday_type = holidayTypeName;
                            updatingType.is_active = isActive;
                            updatingType.modified_datetime = DateTime.Now;
                            updatingType.modified_user_id = clsSecurity.loggedUser.user_id;

                            if (attendServiceClient.SaveHolidayType(updatingType))
                            {
                                clsMessages.setMessage("Holiday type is updated successfully");
                                this.RefreshHolidayTypes();
                                this.New();
                            }
                            else
                            {
                                clsMessages.setMessage("Holiday type update is failed");
                            }
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                }
            }
        } 
        #endregion

        #region Delete Button

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete); }
        }

        private void Delete()
        {
            if(currentHolidayType != null && currentHolidayType.holiday_type_id > 0)
            {
                if (clsSecurity.GetDeletePermission(307))
                {
                    z_HolidayType deletingType = new z_HolidayType();
                    deletingType.holiday_type_id = currentHolidayType.holiday_type_id;
                    deletingType.is_delete = false;
                    deletingType.delete_datetime = DateTime.Now;
                    deletingType.delete_user_id = clsSecurity.loggedUser.user_id;

                    if (attendServiceClient.DeleteHolidayType(deletingType))
                    {
                        clsMessages.setMessage("Holiday type is deleted successfully");
                        this.RefreshHolidayTypes();
                        this.New();
                    }
                    else
                    {
                        clsMessages.setMessage("Holiday type delete is failed");
                    }
                }
                else
                    clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }
        #endregion

        #region New Button

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            HolidayTypeName = null;
            IsActive = true;
            CurrentHolidayType = null;
            CurrentHolidayType = new z_HolidayType();
        }

        #endregion
        
        #endregion

        #region Validation Methods

        bool IsValidHolidayTypeName()
        {
            if (holidayTypeName == null || holidayTypeName == string.Empty)
            {
                clsMessages.setMessage("Holiday type name is required");
                return false;
            }
            return true;
        }

        #endregion

        #region Data Setting Methods

        void PopulateCurrentHolidayType()
        {
            HolidayTypeName = currentHolidayType.holiday_type;
            IsActive = currentHolidayType.is_active;
        }

        #endregion
    }
}
