using ERP.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ERP.AttendanceModule.AttendanceMasters
{
    class EmployeeAttendanceGroupViewModel : ViewModelBase
    {
        #region Attendance Service Client

        AttendanceServiceClient attendClient;

        #endregion

        #region Constructor

        public EmployeeAttendanceGroupViewModel()
        {
            attendClient = new AttendanceServiceClient();
            this.RefreshAttendanceGroups();
            this.New();
        }

        #endregion

        #region List Members

        List<z_AttendanceGroup> allGroupList = new List<z_AttendanceGroup>();

        #endregion

        #region Properties

        IEnumerable<z_AttendanceGroup> attendanceGroups;
        public IEnumerable<z_AttendanceGroup> AttendanceGroups
        {
            get { return attendanceGroups; }
            set { attendanceGroups = value; OnPropertyChanged("AttendanceGroups"); }
        }

        z_AttendanceGroup currentAttendanceGroup;
        public z_AttendanceGroup CurrentAttendanceGroup
        {
            get { return currentAttendanceGroup; }
            set
            {
                currentAttendanceGroup = value; OnPropertyChanged("CurrentAttendanceGroup");
                this.FillCurrentAttendanceGroupData();
            }
        }

        string groupName;
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; OnPropertyChanged("GroupName"); }
        }

        bool isRosterGroup;
        public bool IsRosterGroup
        {
            get { return isRosterGroup; }
            set { isRosterGroup = value; OnPropertyChanged("IsRosterGroup"); }
        }

        bool isDailyShiftGroup;
        public bool IsDailyShiftGroup
        {
            get { return isDailyShiftGroup; }
            set { isDailyShiftGroup = value; OnPropertyChanged("IsDailyShiftGroup"); }
        }

        bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; OnPropertyChanged("IsActive"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshAttendanceGroups()
        {
            attendClient.GetAttendanceGroupsCompleted += (s, e) =>
            {
                try
                {
                    AttendanceGroups = null;
                    AttendanceGroups = e.Result;
                    if (attendanceGroups != null)
                        allGroupList = attendanceGroups.ToList();
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Attendance group details refresh is failed");
                }
            };

            attendClient.GetAttendanceGroupsAsync();
        }

        #endregion

        #region Button Methods

        #region New Button

        void New()
        {
            GroupName = null;
            IsActive = true;
            IsDailyShiftGroup = true;
            IsRosterGroup = false;
            CurrentAttendanceGroup = null;
            CurrentAttendanceGroup = new z_AttendanceGroup();
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        #endregion

        #region Save Button

        void Save()
        {
            this.SetCurrentGroupData();
            if (currentAttendanceGroup.attendance_group_id == 0)        // Adding new group
            {
                if (isValidGroupName())
                {
                    if (clsSecurity.GetSavePermission(303))
                    {
                        z_AttendanceGroup addingGroup = new z_AttendanceGroup();
                        addingGroup.attendance_group_name = currentAttendanceGroup.attendance_group_name;
                        addingGroup.is_active = currentAttendanceGroup.is_active;
                        addingGroup.is_roster_group = currentAttendanceGroup.is_roster_group;
                        addingGroup.is_shift_group = currentAttendanceGroup.is_shift_group;
                        addingGroup.save_datetime = DateTime.Now;
                        addingGroup.save_user_id = clsSecurity.loggedUser.user_id;


                        if (attendClient.SaveAttendanceGroup(addingGroup))
                        {
                            clsMessages.setMessage("Attendance group is saved successfully");
                            this.New();
                            this.RefreshAttendanceGroups();
                        }
                        else
                        {
                            clsMessages.setMessage("Attendance group save is failed");
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Save this record(s)");

                }
                else
                {
                    clsMessages.setMessage("saving group name is already exists");
                }

            }
            else                                                       // updating existing group
            {

                if (clsSecurity.GetUpdatePermission(303))
                {
                    z_AttendanceGroup updatingGroup = new z_AttendanceGroup();
                    updatingGroup.attendance_group_id = currentAttendanceGroup.attendance_group_id;
                    updatingGroup.attendance_group_name = currentAttendanceGroup.attendance_group_name;
                    updatingGroup.is_active = currentAttendanceGroup.is_active;
                    updatingGroup.is_roster_group = currentAttendanceGroup.is_roster_group;
                    updatingGroup.is_shift_group = currentAttendanceGroup.is_shift_group;
                    updatingGroup.modified_datetime = DateTime.Now;
                    updatingGroup.modified_user_id = clsSecurity.loggedUser.user_id;

                    if (attendClient.SaveAttendanceGroup(updatingGroup))
                    {
                        clsMessages.setMessage("Attendance group is updated successfully");
                        this.New();
                        this.RefreshAttendanceGroups();
                    }
                    else
                    {
                        clsMessages.setMessage("Attendance group update is failed");
                    }

                }
                else
                    clsMessages.setMessage("You don't have permission to Update this record(s)");
            }
        }

        bool SaveCanExecute()
        {
            if (currentAttendanceGroup == null)
                return false;
            if (groupName == null || groupName == string.Empty)
                return false;

            return true;
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }

        #endregion

        #region Delete Button

        void Delete()
        {
            if (currentAttendanceGroup != null && currentAttendanceGroup.attendance_group_id != 0)
            {
                if (clsSecurity.GetDeletePermission(303))
                {
                    z_AttendanceGroup deletingGroup = new z_AttendanceGroup();
                    deletingGroup.attendance_group_id = currentAttendanceGroup.attendance_group_id;
                    deletingGroup.delete_datetime = DateTime.Now;
                    deletingGroup.delete_user_id = clsSecurity.loggedUser.user_id;
                    deletingGroup.is_delete = true;
                    if (attendClient.DeleteAttendanceGroup(deletingGroup))
                    {
                        clsMessages.setMessage("Attendance group delete is successful");
                        this.RefreshAttendanceGroups();
                    }
                    else
                    {
                        clsMessages.setMessage("Attendance group delete us failed");
                    }
                }
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        bool DeleteCanExecute()
        {
            if (currentAttendanceGroup == null)
                return false;
            if (currentAttendanceGroup.attendance_group_id == 0)
                return false;
            return true;
        }

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }

        #endregion

        #endregion

        #region Data Setting Methods

        void FillCurrentAttendanceGroupData()
        {
            if (currentAttendanceGroup != null && currentAttendanceGroup.attendance_group_id != 0)
            {
                GroupName = currentAttendanceGroup.attendance_group_name;
                IsRosterGroup = currentAttendanceGroup.is_roster_group;
                IsDailyShiftGroup = currentAttendanceGroup.is_shift_group;
                IsActive = currentAttendanceGroup.is_active;
            }
        }

        void SetCurrentGroupData()
        {
            if (currentAttendanceGroup != null)
            {
                currentAttendanceGroup.attendance_group_name = groupName;
                currentAttendanceGroup.is_roster_group = isRosterGroup;
                currentAttendanceGroup.is_shift_group = isDailyShiftGroup;
                currentAttendanceGroup.is_active = isActive;
            }
        }

        #endregion

        #region Validation Methods

        bool isValidGroupName()
        {
            if (allGroupList.Count > 0 && allGroupList.Count(c => c.attendance_group_name == groupName) > 0)
                return false;
            return true;

        }

        #endregion
    }
}
