using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    class AppoinmentLetterNewViewModel : ViewModelBase
    {
        #region Service Object
        ERPServiceClient serviceClient;
        #endregion

        #region Lists
        List<Appointment_Letter_View> listsEmp = new List<Appointment_Letter_View>();
        #endregion

        #region Constructor
        public AppoinmentLetterNewViewModel()
        {
            try
            {
                serviceClient = new ERPServiceClient();
                SearchIndex = 0;
                RefreshAppointmentLetter();
                RefreshDesignations();
                NormalAppointment = true;
            }
            catch (Exception)
            { }
        }
        #endregion

        #region Properties
        // private IEnumerable<>

        private bool  normalAppointment;
        public bool  NormalAppointment
        {
            get { return normalAppointment; }
            set { normalAppointment = value; OnPropertyChanged("NormalAppointment"); }
        }

        private bool cutterAppointment;
        public bool CutterAppointment
        {
            get { return cutterAppointment; }
            set { cutterAppointment = value; OnPropertyChanged("CutterAppointment"); }
        }
        
        

        private IEnumerable<z_Designation> designations;
        public IEnumerable<z_Designation> Designations
        {
            get { return designations; }
            set { designations = value; OnPropertyChanged("Designations"); }
        }

        private z_Designation currentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return currentDesignation; }
            set { currentDesignation = value; OnPropertyChanged("CurrentDesignation"); FilterManagers(); }
        }

        private IEnumerable<Appointment_Letter_View> _Manager;
        public IEnumerable<Appointment_Letter_View> Manager
        {
            get { return _Manager; }
            set { _Manager = value; OnPropertyChanged("Manager"); }
        }

        private Appointment_Letter_View _CurrentManager;
        public Appointment_Letter_View CurrentManager
        {
            get { return _CurrentManager; }
            set { _CurrentManager = value; OnPropertyChanged("CurrentManager"); }
        }

        private IEnumerable<Appointment_Letter_View> _Employee;
        public IEnumerable<Appointment_Letter_View> Employee
        {
            get { return _Employee; }
            set { _Employee = value; OnPropertyChanged("Employee"); }
        }

        private Appointment_Letter_View _CurrentEmployee;
        public Appointment_Letter_View CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private DateTime? dateSelected;
        public DateTime? DateSelected
        {
            get { return dateSelected; }
            set { dateSelected = value; OnPropertyChanged("DateSelected"); }
        }

        #endregion

        #region Refresh Methods
        private void RefreshDesignations()
        {
            serviceClient.GetDesignationsCompleted += (s, e) =>
                {
                    Designations = e.Result.Where(c => c.isdelete == false);
                };
            serviceClient.GetDesignationsAsync();
        }

        private void RefreshAppointmentLetter()
        {
            try
            {
                listsEmp.Clear();
                IEnumerable<Appointment_Letter_View> ie;
                serviceClient.GetAppointmentViewCompleted += (s, e) =>
                {
                    ie = e.Result.OrderBy(c=>c.emp_id);
                    Employee = ie;
                    if (ie != null)
                        listsEmp = ie.ToList();
                };
                serviceClient.GetAppointmentViewAsync();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        #endregion

        #region Filter Methods
        private void FilterManagers()
        {
            try
            {
                Manager = null;
                if (CurrentDesignation != null)
                {
                    Manager = listsEmp;
                    Manager = Manager.Where(c => c.designation_id == CurrentDesignation.designation_id);
                }
            }
            catch (Exception)
            { }
        }
        #endregion

        #region PirntMethods
        public ICommand PrintBtn
        {
            get { return new RelayCommand(Print); }
        }

        public void Print()
        {
            if (DateSelected == null)
                MessageBox.Show("Please Select a Date", "", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (CurrentDesignation == null)
                MessageBox.Show("Please Select a Designation", "", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (CurrentManager == null)
                MessageBox.Show("Please Select a Manager", "", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (CurrentEmployee == null)
                MessageBox.Show("Please Select an Employee", "", MessageBoxButton.OK, MessageBoxImage.Information);
            else if (CurrentEmployee.emp_id == CurrentManager.emp_id)
                MessageBox.Show("Employee and the Manager cannot be the Same", "", MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                if (NormalAppointment == true)
                {
                    string path = "\\Reports\\Documents\\HR_Report\\Appointmentletter";
                    ReportPrint print = new ReportPrint(path);
                    print.setParameterValue("@Emp_id", CurrentEmployee.emp_id);
                    print.setParameterValue("@AppDate", DateSelected);
                    print.setParameterValue("@Man_Name", CurrentManager.first_name +" "+ CurrentManager.second_name);
                    print.setParameterValue("@Man_Designation", CurrentDesignation.designation);
                    print.setParameterValue("@EffDate", null);
                    print.LoadToReportViewer();
                    //Appointmentletter report = new Appointmentletter();
                    //report.SetParameterValue("@Emp_id", CurrentEmployee.emp_id.ToString());
                    //report.SetParameterValue("@AppDate", DateSelected);
                    //report.SetParameterValue("@Man_Name", CurrentManager.first_name);
                    //report.SetParameterValue("@Man_Designation", CurrentDesignation.designation);
                    //report.SetParameterValue("@EffectDate", null);
                    //ReportPrint print = new ReportPrint(report);
                    //print.LoadToReportViewer();
                }
                else 
                {
                    string path = "\\Reports\\Documents\\HR_Report\\CutterAppointmentLetter";
                    ReportPrint print = new ReportPrint(path);
                    print.setParameterValue("@Emp_id", CurrentEmployee.emp_id);
                    print.setParameterValue("@AppDate", DateSelected);
                    print.setParameterValue("@Man_Name", CurrentManager.first_name + " " + CurrentManager.second_name);
                    print.setParameterValue("@Man_Designation", CurrentDesignation.designation);
                    print.setParameterValue("@EffDate", null);
                    print.LoadToReportViewer();
                }
              
            }

        }
        #endregion

        #region Search
        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged("Search"); SearchMethod(); }
        }

        private int searchIndex;
        public int SearchIndex
        {
            get { return searchIndex; }
            set { searchIndex = value; OnPropertyChanged("SearchIndex"); }
        }
        
        private void SearchMethod()
        {
            Employee = null;
            Employee = listsEmp;
            try
            {
                if (SearchIndex == 0)
                    Employee = Employee.Where(c =>c.first_name !=null && c.first_name.ToUpper().Contains(Search.ToUpper()));
                else if (SearchIndex == 1)
                    Employee = Employee.Where(c =>c.second_name !=null && c.second_name.ToUpper().Contains(Search.ToUpper()));
                else if (SearchIndex == 2)
                    Employee = Employee.Where(c => c.emp_id != null && c.emp_id.Contains(Search.ToUpper()));
                else
                    Employee = Employee.Where(c =>c.department_name!=null && c.designation.ToUpper().Contains(Search.ToUpper()));
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }
        #endregion
    }
}