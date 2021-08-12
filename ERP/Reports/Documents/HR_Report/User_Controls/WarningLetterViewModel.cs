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
    class WarningLetterViewModel : ViewModelBase
    {
        #region Service Object
        ERPServiceClient serviceClient;
        #endregion

        #region Lists

        List<Appointment_Letter_View> listsEmp = new List<Appointment_Letter_View>();
        List<mas_WarningLetter> list_mas_warning = new List<mas_WarningLetter>();
        List<dtl_EmployeeWarning> list_dtl_warning = new List<dtl_EmployeeWarning>();

        #endregion

        #region Constructor

        public WarningLetterViewModel()
        {
            try
            {
                serviceClient = new ERPServiceClient();
                SearchIndex = 0;
                RefreshAppointmentletter();
                RefreshWarnings();
                RefershDtlWarning();
                Desctb = false;
                RecipientCheck = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Refreshmethods

        private void RefreshAppointmentletter()
        {
            try
            {
                listsEmp.Clear();
                IEnumerable<Appointment_Letter_View> ie;
                serviceClient.GetAppointmentViewCompleted += (s, e) =>
                {
                    ie = e.Result;
                    listsEmp = ie.ToList();
                    Employee = e.Result;
                };
                serviceClient.GetAppointmentViewAsync();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void RefreshWarnings()
        {
            try
            {
                list_mas_warning.Clear();
                IEnumerable<mas_WarningLetter> ie;
                serviceClient.GetZWarningLetterCompleted += (s, e) =>
                {
                    ie = e.Result;
                    list_mas_warning = ie.ToList();
                    Warning = ie;
                };
                serviceClient.GetZWarningLetterAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void RefershDtlWarning()
        {
            list_dtl_warning.Clear();
            IEnumerable<dtl_EmployeeWarning> ie;
            try
            {
                serviceClient.GetDtlWarninletterCompleted += (s, e) =>
                {
                    ie = e.Result;
                    list_dtl_warning = ie.ToList();
                    DtlWarning = ie;
                };
                serviceClient.GetDtlWarninletterAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Properties


        private IEnumerable<Appointment_Letter_View> employee;
        public IEnumerable<Appointment_Letter_View> Employee
        {
            get { return employee; }
            set { employee = value; OnPropertyChanged("Employee"); }
        }

        private Appointment_Letter_View currentEmployee;
        public Appointment_Letter_View CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); if (CurrentEmployee != null) { if (RecipientCheck == true) Recipient = CurrentEmployee; if (IssuerCheck == true) Issuer = CurrentEmployee; } }
        }

        private IEnumerable<mas_WarningLetter> warning;
        public IEnumerable<mas_WarningLetter> Warning
        {
            get { return warning; }
            set { warning = value; OnPropertyChanged("Warning"); }
        }

        private mas_WarningLetter currentWarning;
        public mas_WarningLetter CurrentWarning
        {
            get { return currentWarning; }
            set { currentWarning = value; OnPropertyChanged("CurrentWarning"); if (CurrentWarning != null)FilterWarnings(); }
        }

        private IEnumerable<dtl_EmployeeWarning> dtlWarning;
        public IEnumerable<dtl_EmployeeWarning> DtlWarning
        {
            get { return dtlWarning; }
            set { dtlWarning = value; OnPropertyChanged("DtlWarning"); }
        }

        private dtl_EmployeeWarning currentDtlWarning;
        public dtl_EmployeeWarning CurrentDtlWarning
        {
            get { return currentDtlWarning; }
            set { currentDtlWarning = value; OnPropertyChanged("CurrentDtlWarning"); }
        }

        private string empfullName;
        public string EmpfullName
        {
            get { return empfullName; }
            set { empfullName = value; OnPropertyChanged("EmpfullName"); }
        }

        private string empNo;
        public string EmpNo
        {
            get { return empNo; }
            set { empNo = value; OnPropertyChanged("EmpNo"); }
        }

        private string warnerFullName;
        public string WarnerFullName
        {
            get { return warnerFullName; }
            set { warnerFullName = value; OnPropertyChanged("WarnerFullName"); }
        }

        private string noOfWarning;
        public string NoOfWarning
        {
            get { return noOfWarning; ; }
            set { noOfWarning = value; OnPropertyChanged("NoOfWarning"); }
        }

        private bool desctb;
        public bool Desctb
        {
            get { return desctb; }
            set { desctb = value; OnPropertyChanged("Desctb"); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        private bool recipientCheck;
        public bool RecipientCheck
        {
            get { return recipientCheck; }
            set { recipientCheck = value; OnPropertyChanged("RecipientCheck"); }
        }

        private bool issuerCheck;
        public bool IssuerCheck
        {
            get { return issuerCheck; }
            set { issuerCheck = value; OnPropertyChanged("IssuerCheck"); }
        }

        private Appointment_Letter_View recipient;
        public Appointment_Letter_View Recipient
        {
            get { return recipient; }
            set { recipient = value; OnPropertyChanged("Recipient"); }
        }

        private Appointment_Letter_View issuer;
        public Appointment_Letter_View Issuer
        {
            get { return issuer; }
            set { issuer = value; OnPropertyChanged("Issuer"); }
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
                    Employee = Employee.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(Search.ToUpper()));
                else if (SearchIndex == 1)
                    Employee = Employee.Where(c => c.second_name != null && c.second_name.ToUpper().Contains(Search.ToUpper()));
                else if (SearchIndex == 2)
                    Employee = Employee.Where(c => c.emp_id != null && c.emp_id.Contains(Search.ToUpper()));
                else
                    Employee = Employee.Where(c => c.department_name != null && c.designation.ToUpper().Contains(Search.ToUpper()));
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        #endregion

        #region FilterMehtod;

        private void FilterWarnings()
        {
            CurrentDtlWarning = null;
            currentDtlWarning = new dtl_EmployeeWarning();

            try
            {
                CurrentDtlWarning = list_dtl_warning.Where(c => c.WarningLetterId == CurrentWarning.Warning_Letter_id).FirstOrDefault();
                EmpNo = listsEmp.Where(c => c.employee_id == CurrentDtlWarning.Emp_id).FirstOrDefault().emp_id.ToString();
                EmpfullName = listsEmp.Where(c => c.employee_id == CurrentDtlWarning.Emp_id).FirstOrDefault().first_name + " " + listsEmp.Where(c => c.employee_id == CurrentDtlWarning.Emp_id).FirstOrDefault().second_name;
                WarnerFullName = listsEmp.Where(c => c.employee_id == CurrentWarning.Warnedby).FirstOrDefault().first_name + " " + listsEmp.Where(c => c.employee_id == CurrentWarning.Warnedby).FirstOrDefault().second_name + " (" + listsEmp.Where(c => c.employee_id == CurrentWarning.Warnedby).FirstOrDefault().designation + ")";
                NoOfWarning = list_dtl_warning.Count(c => c.Emp_id == CurrentDtlWarning.Emp_id).ToString();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region btnMethods

        public ICommand Newbtn
        {
            get { return new RelayCommand(New); }
        }

        public ICommand Savebtn
        {
            get { return new RelayCommand(Save); }
        }

        public ICommand PrintBtn
        {
            get { return new RelayCommand(Print); }
        }

        public ICommand Deletebtn
        {
            get { return new RelayCommand(Delete); }
        }

        private void New()
        {
            try
            {
                CurrentEmployee = null;
                Recipient = null;
                Issuer = null;
                Description = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Save()
        {
            try
            {
                if (Recipient == null)
                    MessageBox.Show("Please Select the Warning Letter Recipient", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (Description == null)
                    MessageBox.Show("Please enter a Warning description", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (Description.Length == 0)
                    MessageBox.Show("Description Cannot be Empty", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (Issuer == null)
                    MessageBox.Show("Please Select the Warning Letter Issuer from the Employee list", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (Recipient == Issuer)
                    MessageBox.Show("Warning Letter Issuer and the Recipient Cannot be the same", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("You are going to issue Employee no : " + Recipient.emp_id.ToString() + ", The warning letter no : " + (list_dtl_warning.Count(c => c.Emp_id == Recipient.employee_id) + 1).ToString() + "\n" + "Do you wish to Proceed?");
                    MessageBoxResult res = MessageBox.Show(sb.ToString(), "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        mas_WarningLetter masSaveObj = new mas_WarningLetter();
                        masSaveObj.Warning_Letter_id = serviceClient.GetLastWarningLettetrID();
                        masSaveObj.WarningDate = DateTime.Now;
                        masSaveObj.Warnedby = CurrentEmployee.employee_id;
                        masSaveObj.Description = Description;
                        masSaveObj.save_user_id = clsSecurity.loggedUser.user_id;
                        masSaveObj.save_datetime = DateTime.Now;
                        masSaveObj.isdelete = false;

                        dtl_EmployeeWarning dtlSaveObj = new dtl_EmployeeWarning();
                        dtlSaveObj.WarningLetterId = masSaveObj.Warning_Letter_id;
                        dtlSaveObj.Emp_id = Recipient.employee_id;
                        dtlSaveObj.save_datetime = DateTime.Now;
                        dtlSaveObj.save_user_id = clsSecurity.loggedUser.user_id;
                        dtlSaveObj.isActive = true;
                        dtlSaveObj.isdelete = false;

                        if (serviceClient.SaveWarningLetter(masSaveObj, dtlSaveObj))
                        {
                            MessageBox.Show("Information Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            MessageBoxResult res1 = MessageBox.Show("Do you want to Print the Warning Letter?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (res1 == MessageBoxResult.Yes)
                                Print(true);
                            RecipientCheck = true;
                            RefreshWarnings();
                            RefershDtlWarning();
                            New();
                        }

                        else
                        {
                            MessageBox.Show("Save Failed", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            New();
                        }
                    }
                    else
                    {
                        New();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Print()
        {
            if (CurrentWarning == null)
                MessageBox.Show("Please Select a Warning", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                CurrentDtlWarning = list_dtl_warning.FirstOrDefault(c => c.WarningLetterId == CurrentWarning.Warning_Letter_id);

                string path = "\\Reports\\Documents\\HR_Report\\WarningLetter";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@Emp_id", listsEmp.Where(c => c.employee_id == CurrentDtlWarning.Emp_id).FirstOrDefault().emp_id.ToString());
                print.setParameterValue("@AppDate", null);
                print.setParameterValue("@Man_Name", null);
                print.setParameterValue("@Man_Designation", null);
                print.setParameterValue("@EffDate", null);
                print.setParameterValue("LteerNo", list_dtl_warning.Count(c => c.Emp_id == CurrentDtlWarning.Emp_id));
                print.LoadToReportViewer();
            }

        }

        private void Print(bool x)
        {
            if (x == true)
            {
                string path = "\\Reports\\Documents\\HR_Report\\WarningLetter";
                ReportPrint print = new ReportPrint(path);
                print.setParameterValue("@Emp_id", Recipient.emp_id);
                print.setParameterValue("@AppDate", null);
                print.setParameterValue("@Man_Name", null);
                print.setParameterValue("@Man_Designation", null);
                print.setParameterValue("@EffDate", null);
                print.setParameterValue("LteerNo", list_dtl_warning.Count(c => c.Emp_id == Recipient.employee_id) + 1);
                print.LoadToReportViewer();
            }
        }

        private void Delete()
        {
            if (CurrentWarning == null)
                MessageBox.Show("Please select a warning to Delete", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                MessageBoxResult res = MessageBox.Show("Do you really want to Delete this Warning?", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                if (res == MessageBoxResult.OK)
                {
                    try
                    {
                        mas_WarningLetter masDelObj = new mas_WarningLetter();
                        masDelObj.Warning_Letter_id = CurrentWarning.Warning_Letter_id;
                        masDelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                        masDelObj.delete_datetime = DateTime.Now;

                        dtl_EmployeeWarning dtlDelObj = new dtl_EmployeeWarning();
                        dtlDelObj.WarningLetterId = CurrentWarning.Warning_Letter_id;
                        dtlDelObj.delete_user_id = masDelObj.delete_user_id;
                        dtlDelObj.delete_datetime = masDelObj.delete_datetime;

                        if (serviceClient.DeleteWarningLetter(masDelObj, dtlDelObj))
                            MessageBox.Show("Delete Was Successfull", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show("Delete Was not Successfull", "", MessageBoxButton.OK, MessageBoxImage.Information);

                        CurrentWarning = null;
                        RefershDtlWarning();
                        RefreshWarnings();
                        New();
                        EmpfullName = null;
                        WarnerFullName = null;
                        NoOfWarning = null;
                        EmpNo = null;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        #endregion
    }
}
