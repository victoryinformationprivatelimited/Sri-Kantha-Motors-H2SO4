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
    class EmployeeUniformOfferingViewModel : ViewModelBase
    {
        #region Service Object
        ERPServiceClient serviceClient;
        #endregion

        #region Constructor

        public EmployeeUniformOfferingViewModel()
        {
            try
            {
                serviceClient = new ERPServiceClient();
                RefreshOrders();
                RefreshEmployees();
                SearchIndex = 0;
                IsOffice = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Lists
        List<Employee_Uniform_Order_View> listsEmp = new List<Employee_Uniform_Order_View>();
        List<Appointment_Letter_View> listDtlEmp = new List<Appointment_Letter_View>();
        #endregion

        #region Properties

        private IEnumerable<Employee_Uniform_Order_View> _Employee;
        public IEnumerable<Employee_Uniform_Order_View> Employee
        {
            get { return _Employee; }
            set { _Employee = value; OnPropertyChanged("Employee"); }
        }

        private Employee_Uniform_Order_View _CurrentEmployee;
        public Employee_Uniform_Order_View CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private bool isOffice;
        public bool IsOffice
        {
            get { return isOffice; }
            set { isOffice = value; OnPropertyChanged("IsOffice"); }
        }

        private bool isGeneral;
        public bool IsGeneral
        {
            get { return isGeneral; }
            set { isGeneral = value; OnPropertyChanged("IsGeneral"); }
        }

        private DateTime? offeredDate;
        public DateTime? OfferedDate
        {
            get { return offeredDate; }
            set { offeredDate = value; OnPropertyChanged("OfferedDate"); }
        }

        private DateTime? effectDate;
        public DateTime? EffectDate
        {
            get { return effectDate; }
            set { effectDate = value; OnPropertyChanged("EffectDate"); }
        }

        private DateTime? joinDate;
        public DateTime? JoinDate
        {
            get { return joinDate; }
            set { joinDate = value; OnPropertyChanged("JoinDate"); }
        }




        #endregion

        #region Resfresh Methods

        private void RefreshOrders()
        {
            try
            {
                listsEmp.Clear();
                IEnumerable<Employee_Uniform_Order_View> ie;
                serviceClient.GetUniformOrdersViewCompleted += (s, e) =>
                {
                    ie = e.Result.OrderBy(c => c.emp_id);
                    Employee = ie;
                    if (ie != null)
                        listsEmp = ie.ToList();
                };
                serviceClient.GetUniformOrdersViewAsync();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void RefreshEmployees()
        {
            try
            {
                listDtlEmp.Clear();
                IEnumerable<Appointment_Letter_View> ie;
                serviceClient.GetAppointmentViewCompleted += (s, e) =>
                {
                    ie = e.Result;
                    listDtlEmp = ie.ToList();
                };
                serviceClient.GetAppointmentViewAsync();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
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
                    Employee = Employee.Where(c => c.emp_id != null && c.emp_id.ToUpper().Contains(Search.ToUpper()));
                else if (SearchIndex == 1)
                    Employee = Employee.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));
                else if (SearchIndex == 2)
                    Employee = Employee.Where(c => c.Uni_Name != null && c.Uni_Name.Contains(Search.ToUpper()));
                else
                    Employee = Employee.Where(c => c.designation != null && c.designation.Contains(Search.ToUpper()));
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }
        #endregion

        #region Button Commands

        public ICommand PrintBtn
        {
            get { return new RelayCommand(Print); }
        }

        private void Print()
        {
            try
            {
                if (OfferedDate == null)
                    MessageBox.Show("Please Select an Offered Date", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (EffectDate == null)
                    MessageBox.Show("Please Select an Effect Date", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (OfferedDate > EffectDate)
                    MessageBox.Show("Offered Date Cannot Be Greater Than the Effect Date", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (CurrentEmployee == null)
                    MessageBox.Show("Please Select an Employee", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    if (IsOffice)
                    {

                        if ((DateTime.Now - (DateTime)(listDtlEmp.Where(c => c.emp_id == CurrentEmployee.emp_id).FirstOrDefault().join_date)).Days <= 365)
                        {
                            string path = "\\Reports\\Documents\\HR_Report\\OfficeStaffUni2";
                            ReportPrint print = new ReportPrint(path);
                            print.setParameterValue("Branchname", listDtlEmp.Where(c => c.emp_id == CurrentEmployee.emp_id).FirstOrDefault().companyBranch_Name);
                            print.setParameterValue("@Emp_id", CurrentEmployee.emp_id);
                            print.setParameterValue("@AppDate", OfferedDate);
                            print.setParameterValue("@Man_Name", null);
                            print.setParameterValue("@Man_Designation", null);
                            print.setParameterValue("@EffDate", EffectDate);
                            print.LoadToReportViewer();
                        }

                        else
                        {
                            string path = "\\Reports\\Documents\\HR_Report\\OfficeStaffUni";
                            ReportPrint print = new ReportPrint(path);
                            print.setParameterValue("Branchname", listDtlEmp.Where(c => c.emp_id == CurrentEmployee.emp_id).FirstOrDefault().companyBranch_Name);
                            print.setParameterValue("@Emp_id", CurrentEmployee.emp_id);
                            print.setParameterValue("@AppDate", OfferedDate);
                            print.setParameterValue("@Man_Name", null);
                            print.setParameterValue("@Man_Designation", null);
                            print.setParameterValue("@EffDate", EffectDate);
                            print.LoadToReportViewer();
                        }

                    }

                    else if (IsGeneral)
                    {
                        string path = "\\Reports\\Documents\\HR_Report\\GeneralStaffUni";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("Branchname", listDtlEmp.Where(c => c.emp_id == CurrentEmployee.emp_id).FirstOrDefault().companyBranch_Name);
                        print.setParameterValue("@Emp_id", CurrentEmployee.emp_id);
                        print.setParameterValue("@AppDate", OfferedDate);
                        print.setParameterValue("@Man_Name", null);
                        print.setParameterValue("@Man_Designation", null);
                        print.setParameterValue("@EffDate", EffectDate);
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
