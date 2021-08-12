using CustomBusyBox;
using ERP.ERPService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        List<EmployeeNopayView> EmployeeAllDays;
        #endregion

        #region Constructor

        public NopayLetterViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployees();
            RefreshLeaveTypes();
            ReportDate = DateTime.Now.Date;
            FdateSelected = DateTime.Now.Date;
            EdateSelected = DateTime.Now.Date;
        }

        #endregion

        #region Refresh Methods

        private void RefreshEmployees()
        {
            try
            {
                serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
                {
                    Employees = e.Result.Where(c => c.isActive == true && c.isdelete == false).OrderBy(c => c.emp_id);
                };
                serviceClient.GetAllEmployeeDetailAsync();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        private void RefreshLeaveTypes()
        {
            try
            {
                serviceClient.GetAllLeaveTypesCompleted += (s, e) =>
                {
                    LeaveTypes = e.Result;
                };
                serviceClient.GetAllLeaveTypesAsync();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }
        #endregion

        #region Properties
        // private IEnumerable<>


        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); if (CurrentEmployee != null) GetEmployeeName(CurrentEmployee.employee_id); }
        }

        private EmployeeSumarryView _CurrentHOD;
        public EmployeeSumarryView CurrentHOD
        {
            get { return _CurrentHOD; }
            set { _CurrentHOD = value; OnPropertyChanged("CurrentHOD"); if (CurrentHOD != null) GetHODName(CurrentHOD.employee_id); }
        }

        private IEnumerable<EmployeeNopayView> _EmployeeNopayDays;

        public IEnumerable<EmployeeNopayView> EmployeeNopayDays
        {
            get { return _EmployeeNopayDays; }
            set { _EmployeeNopayDays = value; OnPropertyChanged("EmployeeNopayDays"); }
        }

        private EmployeeNopayView _CurrentEmployeeNopayDay;

        public EmployeeNopayView CurrentEmployeeNopayDay
        {
            get { return _CurrentEmployeeNopayDay; }
            set { _CurrentEmployeeNopayDay = value; OnPropertyChanged("CurrentEmployeeNopayDay"); }
        }

        private string _EmployeeName;

        public string EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; OnPropertyChanged("EmployeeName"); }
        }

        private string _HODName;

        public string HODName
        {
            get { return _HODName; }
            set { _HODName = value; OnPropertyChanged("HODName"); }
        }

        private DateTime _ReportDate;
        public DateTime ReportDate
        {
            get { return _ReportDate; }
            set { _ReportDate = value; OnPropertyChanged("ReportDate"); }
        }

        private DateTime fdateSelected;
        public DateTime FdateSelected
        {
            get { return fdateSelected; }
            set { fdateSelected = value; OnPropertyChanged("DateSelected"); }
        }

        private DateTime edateSelected;
        public DateTime EdateSelected
        {
            get { return edateSelected; }
            set { edateSelected = value; OnPropertyChanged("DateSelected"); }
        }

        private IEnumerable<z_LeaveType> _LeaveTypes;

        public IEnumerable<z_LeaveType> LeaveTypes
        {
            get { return _LeaveTypes; }
            set { _LeaveTypes = value; OnPropertyChanged("LeaveTypes"); }
        }

        private z_LeaveType _CurrentLeaveType;

        public z_LeaveType CurrentLeaveType
        {
            get { return _CurrentLeaveType; }
            set { _CurrentLeaveType = value; OnPropertyChanged("CurrentLeaveType"); }
        }

        private IList _CurrentNopayList = new ArrayList();
        public IList CurrentNopayList
        {
            get { return _CurrentNopayList; }
            set { _CurrentNopayList = value; OnPropertyChanged("CurrentNopayList"); }
        }

        #endregion

        #region Commands And Methods

        private void GetEmployeeName(Guid EmpID)
        {
            EmployeeName = serviceClient.GetEmployeeEPFName(EmpID);
        }

        private void GetHODName(Guid EmpID)
        {
            HODName = serviceClient.GetEmployeeEPFName(EmpID);
        }

        public ICommand SetDateButton
        {
            get { return new RelayCommand(SetNopayDates); }
        }

        void SetNopayDates()
        {
            BusyBox.ShowBusy("Please Wait...");
            DateTime StartDate = FdateSelected;
            EmployeeAllDays = new List<EmployeeNopayView>();
            EmployeeNopayDays = serviceClient.GetEmployeeNopayDays(CurrentEmployee.employee_id, FdateSelected, EdateSelected);
            if (EmployeeNopayDays != null && EmployeeNopayDays.Count() > 0)
            {
                while (StartDate <= EdateSelected)
                {
                    EmployeeNopayView CurrentDate = new EmployeeNopayView();
                    if (EmployeeNopayDays.FirstOrDefault(c => c.attend_date == StartDate) != null)
                    {
                        CurrentDate.attend_date = EmployeeNopayDays.FirstOrDefault(c => c.attend_date == StartDate).attend_date;
                        CurrentDate.status_description = EmployeeNopayDays.FirstOrDefault(c => c.attend_date == StartDate).status_description;
                    }
                    else
                    {
                        CurrentDate.attend_date = StartDate;
                        CurrentDate.status_description = "";
                    }
                    StartDate = StartDate.AddDays(1);
                    EmployeeAllDays.Add(CurrentDate);
                }
            }
            else
            {
                while (StartDate <= EdateSelected)
                {
                    EmployeeNopayView CurrentDate = new EmployeeNopayView();
                    CurrentDate.attend_date = StartDate;
                    CurrentDate.status_description = "";
                    StartDate = StartDate.AddDays(1);
                    EmployeeAllDays.Add(CurrentDate);
                }
            }
            EmployeeNopayDays = EmployeeAllDays;
            BusyBox.CloseBusy();
        }

        public ICommand AddData
        {
            get
            {
                return new RelayCommand(add_data);
            }
        }

        private void add_data()
        {
            try
            {
                if (CurrentNopayList.Count > 0)
                {
                    List<EmployeeNopayView> temp = new List<EmployeeNopayView>();
                    foreach (EmployeeNopayView item in CurrentNopayList)
                    {
                        item.status_description = CurrentLeaveType.name + "_UNAUTHORIZED";
                        temp.Add(item);
                    }
                    foreach (var item in EmployeeNopayDays.Where(c => !temp.Any(d => d.attend_date == c.attend_date)))
                    {
                        temp.Add(item);
                    }
                    EmployeeNopayDays = null;
                    EmployeeNopayDays = temp.OrderBy(c => c.attend_date).ToList();
                    CurrentNopayList.Clear();
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Erro Please Try Again...");
            }
        }

        public ICommand ClearData
        {
            get
            {
                return new RelayCommand(clear_data);
            }
        }

        private void clear_data()
        {
            try
            {
                if (CurrentNopayList.Count > 0)
                {
                    List<EmployeeNopayView> temp = new List<EmployeeNopayView>();
                    foreach (EmployeeNopayView item in CurrentNopayList)
                    {
                        item.status_description = "";
                        temp.Add(item);
                    }
                    foreach (var item in EmployeeNopayDays.Where(c => !temp.Any(d => d.attend_date == c.attend_date)))
                    {
                        temp.Add(item);
                    }
                    EmployeeNopayDays = null;
                    EmployeeNopayDays = temp.OrderBy(c => c.attend_date).ToList();
                    CurrentNopayList.Clear();
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Erro Please Try Again...");
            }
        }

        public ICommand PrintBtn
        {
            get { return new RelayCommand(Print, PrintCE); }
        }

        private bool PrintCE()
        {
            if (CurrentEmployee != null && CurrentHOD != null && EmployeeNopayDays != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Print()
        {
            try
            {
                BusyBox.ShowBusy("Please Wait Until Report is Generated...");
                List<rpt_NopayReportData> SaveNopayData = new List<rpt_NopayReportData>();
                foreach (var item in EmployeeNopayDays)
                {
                    rpt_NopayReportData SaveObj = new rpt_NopayReportData();
                    SaveObj.nopay_date = item.attend_date;
                    SaveObj.nopay_day = item.attend_date.Value.DayOfWeek.ToString();
                    if (item.status_description == "MORNING_SHORT_LEAVE_UNAUTHORIZED")
                    {
                        SaveObj.nopay_amount = (decimal)0.25;
                    }
                    else if (item.status_description == "MORNING_HALFDAY_UNAUTHORIZED")
                    {
                        SaveObj.nopay_amount = (decimal)0.50;
                    }
                    else if (item.status_description == "MORNING_FULLDAY_UNAUTHORIZED")
                    {
                        SaveObj.nopay_amount = (decimal)1;
                    }
                    else if (item.status_description == "EVENING_SHORT_LEAVE_UNAUTHORIZED")
                    {
                        SaveObj.nopay_amount = (decimal)0.25;
                    }
                    else if (item.status_description == "EVENING_HALFDAY_UNAUTHORIZED")
                    {
                        SaveObj.nopay_amount = (decimal)0.50;
                    }
                    else if (item.status_description == "EVENING_FULLDAY_UNAUTHORIZED")
                    {
                        SaveObj.nopay_amount = (decimal)1;
                    }
                    else if (item.status_description == "")
                    {
                        SaveObj.nopay_amount = (decimal)0;
                    }
                    SaveNopayData.Add(SaveObj);
                }
                if (serviceClient.SaveEmployeeNopayDataForReport(SaveNopayData.ToArray()))
                {
                    BusyBox.CloseBusy();
                    string path = "";
                    try
                    {
                        string EPF_Name = serviceClient.GetEmployeeEPFName(CurrentEmployee.employee_id);
                        string EPF_No = CurrentEmployee.epf_no;
                        string Designation = CurrentEmployee.designation;
                        string Department = CurrentEmployee.department_name;
                        string HODName = serviceClient.GetEmployeeEPFName(CurrentHOD.employee_id);
                        string HODDesignation = CurrentHOD.designation;
                        DateTime CurrentDate = ReportDate;
                        string LetterMonth = new DateTime(FdateSelected.Year, FdateSelected.Month, FdateSelected.Day).ToString("MMMM", CultureInfo.InvariantCulture);
                        string LetterYear = FdateSelected.Year.ToString();

                        path = "\\Reports\\Documents\\HR_Report\\no_pay_leave";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@LetterDate", CurrentDate);
                        print.setParameterValue("@EFPName", EPF_Name);
                        print.setParameterValue("@EPF_No", EPF_No);
                        print.setParameterValue("@Designation", Designation);
                        print.setParameterValue("@Department", Department);
                        print.setParameterValue("@HODName", HODName);
                        print.setParameterValue("@HODDesignation", HODDesignation);
                        print.setParameterValue("@LetterMonth", LetterMonth);
                        print.setParameterValue("@LetterYear", LetterYear);
                        print.setParameterValue("@FromDate", FdateSelected);
                        print.setParameterValue("@ToDate", EdateSelected);
                        print.LoadToReportViewer();
                    }
                    catch (Exception ex)
                    {
                        clsMessages.setMessage("Report loading is failed: " + ex.Message);
                    }
                }
                else
                {
                    BusyBox.CloseBusy();
                    clsMessages.setMessage("Report Load Failed...");
                }
            }
            catch (Exception)
            {
                BusyBox.CloseBusy();
                clsMessages.setMessage("Error in Employee Attendance Saving...");
            }
        }
        #endregion
    }
}
