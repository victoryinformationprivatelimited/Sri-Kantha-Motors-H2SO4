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
    class PortalEventsViewModel:ViewModelBase
    {
         #region Fields

        List<EmployeeEventsView> ListAllEvents;
        ERPServiceClient serviceClient;

        #endregion

        #region Constructor

        public PortalEventsViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListAllEvents = new List<EmployeeEventsView>();
            New();
        } 

        #endregion

        #region Properties

        private IEnumerable<z_Events> _Events;
        public IEnumerable<z_Events> Events
        {
            get { return _Events; }
            set { _Events = value; OnPropertyChanged("Events"); }
        }

        private z_Events _CurrentEvent;
        public z_Events CurrentEvent
        {
            get { return _CurrentEvent; }
            set { _CurrentEvent = value; OnPropertyChanged("CurrentEvent"); if (CurrentEvent != null && ListAllEvents != null && ListAllEvents.Count > 0) { EmployeeEvents = null; EmployeeEvents = ListAllEvents.Where(c => c.Event_ID == CurrentEvent.Event_ID); } }
        }
        

        private IEnumerable<EmployeeEventsView> _EmployeeEvents;
        public IEnumerable<EmployeeEventsView> EmployeeEvents
        {
            get { return _EmployeeEvents; }
            set { _EmployeeEvents = value; OnPropertyChanged("EmployeeEvents"); }
        }

        private EmployeeEventsView _CurrentEmployeeEvent;
        public EmployeeEventsView CurrentEmployeeEvent
        {
            get { return _CurrentEmployeeEvent; }
            set { _CurrentEmployeeEvent = value; OnPropertyChanged("CurrentEmployeeEvent"); }
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
            ListAllEvents.Clear();
            CurrentEvent = null;
            CurrentEvent = new z_Events();
            CurrentEvent.Event_ID = Guid.NewGuid();
            CurrentEvent.Is_Visible = true;
            RefreshEvents();
            RefreshEmolyeeEvents();
        }

        public ICommand AddEmployeesBtn 
        {
            get { return new RelayCommand(AddEmployees); }
        }

        private void AddEmployees()
        {
            if(CurrentEvent == null)
                clsMessages.setMessage("Please Resfresh and try again.");
            else if (string.IsNullOrEmpty(CurrentEvent.Heading))
                clsMessages.setMessage("Please enter a Name for this Event.");
            else if (string.IsNullOrEmpty(CurrentEvent.Description))
                clsMessages.setMessage("Please enter a Description for this Event.");
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
                        List<EmployeeEventsView> ListTemp = new List<EmployeeEventsView>();

                        if (ListAllEvents.Count(c => c.Event_ID == CurrentEvent.Event_ID) > 0)
                            ListTemp = ListAllEvents.Where(c => c.Event_ID == CurrentEvent.Event_ID).ToList();

                        foreach (var item in EmployeeSearch)
                        {
                            if (ListTemp.Count(c=> c.Employee_ID == item.employee_id)==0)
                            {
                                EmployeeEventsView Temp = new EmployeeEventsView();
                                Temp.Event_ID = CurrentEvent.Event_ID;
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

                        EmployeeEvents = null;
                        EmployeeEvents = ListTemp;
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
            if (clsSecurity.GetSavePermission(1101) && clsSecurity.GetUpdatePermission(1101))
            {
                if (CurrentEvent == null)
                    clsMessages.setMessage("Please Resfresh and try again.");
                else if (EmployeeEvents == null || EmployeeEvents.Count() == 0)
                    clsMessages.setMessage("Please Select employees and try again.");
                else if (string.IsNullOrEmpty(CurrentEvent.Heading))
                    clsMessages.setMessage("Please enter a Name for this Event.");
                else if (string.IsNullOrEmpty(CurrentEvent.Description))
                    clsMessages.setMessage("Please enter a Description for this Event.");
                else
                {
                    if (EmployeeEvents.Count() != EmployeeEvents.Count(c => c.is_active == false))
                    {
                        List<dtl_EmployeeEvents> SaveList = new List<dtl_EmployeeEvents>();

                        foreach (var item in EmployeeEvents)
                        {
                            dtl_EmployeeEvents SaveObj = new dtl_EmployeeEvents();
                            SaveObj.Employee_ID = (Guid)item.Employee_ID;
                            SaveObj.Event_ID = CurrentEvent.Event_ID;
                            SaveObj.is_active = item.is_active;
                            SaveList.Add(SaveObj);
                        }

                        CurrentEvent.SaveUser_ID = clsSecurity.loggedUser.user_id;
                        CurrentEvent.dtl_EmployeeEvents = SaveList.ToArray();

                        if (serviceClient.SaveEmployeeEvents(CurrentEvent))
                            clsMessages.setMessage("Event Saved/Updated Successfully");
                        else
                            clsMessages.setMessage("Event Save/Update Failed");
                        New();
                    }
                    else
                    {
                        clsMessages.setMessage("Removing all the employees from the event deletes the event by default, do yo want to proceed?", Visibility.Visible);
                        if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                        {
                            Delete();
                        }
                    }
                }
            }
            else
                clsMessages.setMessage("You Don't Have Permission to Save or Update in this Form...");
        }

        public ICommand DeleteBtn 
        {
            get { return new RelayCommand(Delete, DeleteCanexecute); }
        }

        private bool DeleteCanexecute()
        {
            if (CurrentEvent != null)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            if (EmployeeEvents != null && EmployeeEvents.Count(c => c.Event_ID == CurrentEvent.Event_ID) > 0)
            {
                if (clsSecurity.GetDeletePermission(1101))
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Event?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (serviceClient.DeleteEvent(CurrentEvent))
                            clsMessages.setMessage("Event Deleted Successfully");
                        else
                            clsMessages.setMessage("Event Delete Failed");
                        New();
                    }
                }
                else
                    clsMessages.setMessage("You Don't Have Permission to Delete in this Form...");
            }
        }

        public ICommand RemoveEmployeeCmd 
        {
            get { return new RelayCommand(RemoveEmployee); }
        }

        private void RemoveEmployee()
        {
            if (CurrentEmployeeEvent != null) 
            {
                CurrentEmployeeEvent.is_active = false;
                CurrentEmployeeEvent = null;
            }
        }

        #endregion

        #region Refresh Methods
        private void RefreshEvents()
        {
            serviceClient.GetEventsCompleted += (s, e) => 
            {
                try
                {
                    Events = e.Result;
                }
                catch (Exception)
                {
                    
                }
            };
            serviceClient.GetEventsAsync();
        }
        private void RefreshEmolyeeEvents()
        {
            serviceClient.GetEmployeeEventsCompleted += (s, e) => 
            {
                try
                {
                    EmployeeEvents = e.Result;
                    if (EmployeeEvents != null && EmployeeEvents.Count() > 0) 
                    {
                        ListAllEvents = EmployeeEvents.ToList();
                        EmployeeEvents = null;
                    }
                }
                catch (Exception)
                {
                   
                }
            };
            serviceClient.GetEmployeeEventsAsync();
        } 

        #endregion
    }
}
