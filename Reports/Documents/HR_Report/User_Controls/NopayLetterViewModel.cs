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
    class NopayLetterViewModel : ViewModelBase
    {
        #region Service Object
        ERPServiceClient serviceClient;
        #endregion

        #region Lists
        List<Appointment_Letter_View> listsEmp = new List<Appointment_Letter_View>();
        #endregion

        #region Constructor

        public NopayLetterViewModel()
        {
            serviceClient = new ERPServiceClient();
            Index = 0;
            RefreshAppointmentLetter();
        } 

        #endregion

        #region Refresh Methods

        private void RefreshAppointmentLetter()
        {
            try
            {
                listsEmp.Clear();
                IEnumerable<Appointment_Letter_View> ie;
                serviceClient.GetAppointmentViewCompleted += (s, e) =>
                {
                    ie = e.Result.OrderBy(c => c.emp_id);
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

        #region Properties
        // private IEnumerable<>


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

        private DateTime? fdateSelected;
        public DateTime? FdateSelected
        {
            get { return fdateSelected; }
            set { fdateSelected = value; OnPropertyChanged("DateSelected"); }
        }

        private DateTime? edateSelected;
        public DateTime? EdateSelected
        {
            get { return edateSelected; }
            set { edateSelected = value; OnPropertyChanged("DateSelected"); }
        }

        private string search;
        public string Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged("Search"); SearchMethod(); }
        }

        private int index;
        public int Index
        {
            get { return index; }
            set { index = value; OnPropertyChanged("Index"); }
        }

        #endregion

        #region Search

        private void SearchMethod()
        {
            Employee = null;
            Employee = listsEmp;
            try
            {
                if (Index == 0)
                    Employee = Employee.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));
                else if (index == 1)
                    Employee = Employee.Where(c => c.second_name != null && c.second_name.ToUpper().Contains(Search.ToUpper()));
                else if (index == 2)
                    Employee = Employee.Where(c => c.emp_id != null && c.emp_id.ToUpper().Contains(Search.ToUpper()));
                else
                    Employee = Employee.Where(c => c.designation != null && c.designation.ToUpper().Contains(Search.ToUpper()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
 
        #endregion

        #region Print

        public ICommand PrintBtn 
        {
            get { return new RelayCommand(Print);}
        }

    private void Print()
    {
        try
        {
            if (EdateSelected == null && FdateSelected == null)
            {
                if (CurrentEmployee == null)
                    MessageBox.Show("Please select an Employee", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    string path = "\\Reports\\Documents\\HR_Report\\NopayLetter";
                    ReportPrint print = new ReportPrint(path);
                    print.setParameterValue("@Emp_id", CurrentEmployee.emp_id);
                    print.setParameterValue("@AppDate", null);
                    print.setParameterValue("@Man_Name", null);
                    print.setParameterValue("@Man_Designation", null);
                    print.setParameterValue("@EffDate", null);
                    print.setParameterValue("@EmpID", CurrentEmployee.employee_id.ToString());
                    print.setParameterValue("@FromDate", "");
                    print.setParameterValue("@ToDate", DateTime.Now);
                    print.LoadToReportViewer();
                }
            }
            else 
            {
                if(EdateSelected == null)
                    MessageBox.Show("Please select a end Date", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (FdateSelected == null)
                    MessageBox.Show("Please select an start Date", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if(CurrentEmployee == null)
                    MessageBox.Show("Please select an Employee", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else 
                {
                    string path = "\\Reports\\Documents\\HR_Report\\NopayLetter";
                    ReportPrint print = new ReportPrint(path);
                    print.setParameterValue("@Emp_id", CurrentEmployee.emp_id);
                    print.setParameterValue("@AppDate", null);
                    print.setParameterValue("@Man_Name", null);
                    print.setParameterValue("@Man_Designation", null);
                    print.setParameterValue("@EffDate", null);
                    print.setParameterValue("@EmpID", CurrentEmployee.employee_id.ToString());
                    print.setParameterValue("@FromDate", FdateSelected);
                    print.setParameterValue("@ToDate", EdateSelected);
                    print.LoadToReportViewer();
                }
            }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
    #endregion
    }
}
