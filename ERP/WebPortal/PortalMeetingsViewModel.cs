using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using ERP.BasicSearch;
using ERP.Properties;

namespace ERP.WebPortal
{
    class PortalMeetingsViewModel : ViewModelBase
    {
        #region Fields

        List<EmployeeMeetingsView> ListAllMeetings;
        ERPServiceClient serviceClient; 

        #endregion

        #region Constructor

        public PortalMeetingsViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListAllMeetings = new List<EmployeeMeetingsView>();
            New();
        } 

        #endregion

        #region Properties

        private IEnumerable<z_Meetings> _Meetings;
        public IEnumerable<z_Meetings> Meetings
        {
            get { return _Meetings; }
            set { _Meetings = value; OnPropertyChanged("Meetings"); }
        }

        private z_Meetings _CurrentMeeting;
        public z_Meetings CurrentMeeting
        {
            get { return _CurrentMeeting; }
            set { _CurrentMeeting = value; OnPropertyChanged("CurrentMeeting"); if (CurrentMeeting != null && ListAllMeetings != null && ListAllMeetings.Count > 0) { EmployeeMeetings = null; EmployeeMeetings = ListAllMeetings.Where(c => c.Meeting_ID == CurrentMeeting.Meeting_ID); } }
        }


        private IEnumerable<EmployeeMeetingsView> _EmployeeMeetings;
        public IEnumerable<EmployeeMeetingsView> EmployeeMeetings
        {
            get { return _EmployeeMeetings; }
            set { _EmployeeMeetings = value; OnPropertyChanged("EmployeeMeetings"); }
        }

        private EmployeeMeetingsView _CurrentEmployeeMeeting;
        public EmployeeMeetingsView CurrentEmployeeMeeting
        {
            get { return _CurrentEmployeeMeeting; }
            set { _CurrentEmployeeMeeting = value; OnPropertyChanged("CurrentEmployeeMeeting"); }
        }

        private IEnumerable<EmployeeSearchView> _SelectedEmployees;
        public IEnumerable<EmployeeSearchView> SelectedEmployees
        {
            get { return _SelectedEmployees; }
            set { _SelectedEmployees = value; OnPropertyChanged("SelectedEmployees"); }
        }


        private IEnumerable<EmployeeSearchView> _employeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _employeeSearch; }
            set { _employeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        #endregion

        #region Methods

        public ICommand NewBtn
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            ListAllMeetings.Clear();
            CurrentMeeting = null;
            CurrentMeeting = new z_Meetings();
            CurrentMeeting.Meeting_ID = Guid.NewGuid();
            CurrentMeeting.is_visible = true;
            RefreshEvents();
            RefreshEmolyeeEvents();
        }

        public ICommand AddEmployeesBtn
        {
            get { return new RelayCommand(AddEmployees); }
        }

        private void AddEmployees()
        {
            if (CurrentMeeting == null)
                clsMessages.setMessage("Please Resfresh and try again.");
            else if (string.IsNullOrEmpty(CurrentMeeting.Name))
                clsMessages.setMessage("Please enter a Name for this Meeting.");
            else if (string.IsNullOrEmpty(CurrentMeeting.Venue))
                clsMessages.setMessage("Please enter a Venue for this Meeting.");
            else if (CurrentMeeting.Meeting_Date == null)
                clsMessages.setMessage("Please enter a date for this Meeting.");
            else if (CurrentMeeting.Meeting_Start == null)
                clsMessages.setMessage("Please enter a Start time for this Meeting.");
            else if (CurrentMeeting.Meeting_End == null)
                clsMessages.setMessage("Please enter a End time for this Meeting.");
            else
            {
                try
                {
                    EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
                    window.ShowDialog();
                    EmployeeSearch = null;
                    if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                        EmployeeSearch = window.viewModel.selectEmployeeList;
                    window.Close();
                    window = null;

                    if (EmployeeSearch != null && EmployeeSearch.Count() > 0)
                    {
                        List<EmployeeMeetingsView> ListTemp = new List<EmployeeMeetingsView>();

                        if (ListAllMeetings.Count(c => c.Meeting_ID == CurrentMeeting.Meeting_ID) > 0)
                            ListTemp = ListAllMeetings.Where(c => c.Meeting_ID == CurrentMeeting.Meeting_ID).ToList();

                        foreach (var item in EmployeeSearch)
                        {
                            if (ListTemp.Count(c => c.Employee_ID == item.employee_id) == 0)
                            {
                                EmployeeMeetingsView Temp = new EmployeeMeetingsView();
                                Temp.Meeting_ID = CurrentMeeting.Meeting_ID;
                                Temp.Employee_ID = item.employee_id;
                                Temp.emp_id = item.emp_id;
                                Temp.initials = item.initials;
                                Temp.first_name = item.first_name;
                                Temp.second_name = item.second_name;
                                Temp.surname = item.surname;
                                Temp.is_active = true;
                                ListTemp.Add(Temp);
                            }
                        }

                        EmployeeMeetings = null;
                        EmployeeMeetings = ListTemp;
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("An error Occured while loading the list");
                }
            }
        }

        public ICommand SaveBtn
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (clsSecurity.GetSavePermission(1102) && clsSecurity.GetUpdatePermission(1102))
            {
                if (CurrentMeeting == null)
                    clsMessages.setMessage("Please Resfresh and try again.");
                else if (EmployeeMeetings == null || EmployeeMeetings.Count() == 0)
                    clsMessages.setMessage("Please Select employees and try again.");
                else if (string.IsNullOrEmpty(CurrentMeeting.Name))
                    clsMessages.setMessage("Please enter a Name for this Meeting.");
                else if (string.IsNullOrEmpty(CurrentMeeting.Venue))
                    clsMessages.setMessage("Please enter a Venue for this Meeting.");
                else if (CurrentMeeting.Meeting_Date == null)
                    clsMessages.setMessage("Please enter a date for this Meeting.");
                else if (CurrentMeeting.Meeting_Start == null)
                    clsMessages.setMessage("Please enter a Start time for this Meeting.");
                else if (CurrentMeeting.Meeting_End == null)
                    clsMessages.setMessage("Please enter a End time for this Meeting.");
                else
                {
                    if (EmployeeMeetings.Count() != EmployeeMeetings.Count(c => c.is_active == false))
                    {
                        List<dtl_EmployeeMeetings> SaveList = new List<dtl_EmployeeMeetings>();

                        foreach (var item in EmployeeMeetings)
                        {
                            dtl_EmployeeMeetings SaveObj = new dtl_EmployeeMeetings();
                            SaveObj.Employee_ID = (Guid)item.Employee_ID;
                            SaveObj.Meeting_ID = CurrentMeeting.Meeting_ID;
                            SaveObj.is_active = item.is_active;
                            SaveList.Add(SaveObj);
                        }

                        CurrentMeeting.dtl_EmployeeMeetings = SaveList.ToArray();

                        if (serviceClient.SaveEmployeeMeetings(CurrentMeeting))
                            clsMessages.setMessage("Meeting Saved/Updated Successfully");
                        else
                            clsMessages.setMessage("Meeting Save/Update Failed");
                        New();
                    }
                    else
                    {
                        clsMessages.setMessage("Removing all the employees from the event deletes the Meeting by default, do yo want to proceed?", Visibility.Visible);
                        if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                        {
                            Delete();
                        }
                    }
                }
            }
            else
                clsMessages.setMessage("You Don't Have Permission to Save or Update in this form...");
        }

        public ICommand DeleteBtn
        {
            get { return new RelayCommand(Delete, DeleteCanexecute); }
        }

        private bool DeleteCanexecute()
        {
            if (CurrentMeeting != null)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(1102))
            {
                if (EmployeeMeetings != null && EmployeeMeetings.Count(c => c.Meeting_ID == CurrentMeeting.Meeting_ID) > 0)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Meeting?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (serviceClient.DeleteMeeting(CurrentMeeting))
                            clsMessages.setMessage("Meeting Deleted Successfully");
                        else
                            clsMessages.setMessage("Meeting Delete Failed");
                        New();
                    }
                }
            }
            else
                clsMessages.setMessage("You Don't Have Permission to delete in this Form...");
        }

        public ICommand RemoveEmployeeCmd
        {
            get { return new RelayCommand(RemoveEmployee); }
        }

        private void RemoveEmployee()
        {
            if (CurrentEmployeeMeeting != null)
            {
                CurrentEmployeeMeeting.is_active = false;
                CurrentEmployeeMeeting = null;
            }
        }

        #endregion

        #region Refresh Methods

        private void RefreshEvents()
        {
            serviceClient.GetMeetingsCompleted += (s, e) =>
            {
                try
                {
                    Meetings = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetMeetingsAsync();
        }
        private void RefreshEmolyeeEvents()
        {
            serviceClient.GetEmployeeMeetingsCompleted += (s, e) =>
            {
                try
                {
                    EmployeeMeetings = e.Result;
                    if (EmployeeMeetings != null && EmployeeMeetings.Count() > 0)
                    {
                        ListAllMeetings = EmployeeMeetings.ToList();
                        EmployeeMeetings = null;
                    }
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmployeeMeetingsAsync();
        }

        #endregion
    }
}
