using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using CustomBusyBox;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace ERP.HR.HR_Report_Window
{
    class LPEmployeeDetailsViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        string DirectoryPath = "";
        #endregion

        #region Constructor
        public LPEmployeeDetailsViewModel()
        {
            serviceClient = new ERPServiceClient();
            BasicInfo = true;
            FromDate = DateTime.Now.Date;
            ToDate = DateTime.Now.Date;
        }
        #endregion

        #region Properties
        private IEnumerable<EmployeeBasicDetailsView> _EmployeeBasicDetails;

        public IEnumerable<EmployeeBasicDetailsView> EmployeeBasicDetails
        {
            get { return _EmployeeBasicDetails; }
            set { _EmployeeBasicDetails = value; OnPropertyChanged("EmployeeBasicDetails"); }
        }

        private IEnumerable<EmployeeAQDetailsView> _EmployeeAQDetails;

        public IEnumerable<EmployeeAQDetailsView> EmployeeAQDetails
        {
            get { return _EmployeeAQDetails; }
            set { _EmployeeAQDetails = value; OnPropertyChanged("EmployeeAQDetails"); }
        }

        private IEnumerable<EmployeePQDetailsView> _EmployeePQDetails;

        public IEnumerable<EmployeePQDetailsView> EmployeePQDetails
        {
            get { return _EmployeePQDetails; }
            set { _EmployeePQDetails = value; OnPropertyChanged("EmployeePQDetails"); }
        }

        private bool _BasicInfo;

        public bool BasicInfo
        {
            get { return _BasicInfo; }
            set { _BasicInfo = value; OnPropertyChanged("BasicInfo"); }
        }

        private bool _ActiveInactive;

        public bool ActiveInactive
        {
            get { return _ActiveInactive; }
            set { _ActiveInactive = value; OnPropertyChanged("ActiveInactive"); }
        }

        private bool _ReportTypeActiveInactive;

        public bool ReportTypeActiveInactive
        {
            get { return _ReportTypeActiveInactive; }
            set { _ReportTypeActiveInactive = value; OnPropertyChanged("ReportTypeActiveInactive"); }
        }

        private bool _ExNonex;

        public bool ExNonex
        {
            get { return _ExNonex; }
            set { _ExNonex = value; OnPropertyChanged("ExNonex"); }
        }

        private bool _ReportTypeExNonex;

        public bool ReportTypeExNonex
        {
            get { return _ReportTypeExNonex; }
            set { _ReportTypeExNonex = value; OnPropertyChanged("ReportTypeExNonex"); }
        }

        private bool _ResignInfo;

        public bool ResignInfo
        {
            get { return _ResignInfo; }
            set { _ResignInfo = value; OnPropertyChanged("ResignInfo"); }
        }

        private DateTime _FromDate;

        public DateTime FromDate
        {
            get { return _FromDate; }
            set { _FromDate = value; OnPropertyChanged("FromDate"); }
        }

        private DateTime _ToDate;

        public DateTime ToDate
        {
            get { return _ToDate; }
            set { _ToDate = value; OnPropertyChanged("ToDate"); }
        }

        private bool _EPFNo;

        public bool EPFNo
        {
            get { return _EPFNo; }
            set { _EPFNo = value; OnPropertyChanged("EPFNo"); }
        }

        private bool _EPFName;

        public bool EPFName
        {
            get { return _EPFName; }
            set { _EPFName = value; OnPropertyChanged("EPFName"); }
        }

        private bool _Gender;

        public bool Gender
        {
            get { return _Gender; }
            set { _Gender = value; OnPropertyChanged("Gender"); }
        }

        private bool _Title;

        public bool Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); }
        }

        private bool _NIC;

        public bool NIC
        {
            get { return _NIC; }
            set { _NIC = value; OnPropertyChanged("NIC"); }
        }

        private bool _Race;

        public bool Race
        {
            get { return _Race; }
            set { _Race = value; OnPropertyChanged("Race"); }
        }

        private bool _ReportColumnActiveInactive;

        public bool ReportColumnActiveInactive
        {
            get { return _ReportColumnActiveInactive; }
            set { _ReportColumnActiveInactive = value; OnPropertyChanged("ReportColumnActiveInactive"); }
        }

        private bool _ReportColumnExNonex;

        public bool ReportColumnExNonex
        {
            get { return _ReportColumnExNonex; }
            set { _ReportColumnExNonex = value; OnPropertyChanged("ReportColumnExNonex"); }
        }

        private bool _Religion;

        public bool Religion
        {
            get { return _Religion; }
            set { _Religion = value; OnPropertyChanged("Religion"); }
        }

        private bool _District;

        public bool District
        {
            get { return _District; }
            set { _District = value; OnPropertyChanged("District"); }
        }

        private bool _PAddress;

        public bool PAddress
        {
            get { return _PAddress; }
            set { _PAddress = value; OnPropertyChanged("PAddress"); }
        }

        private bool _CAddress;

        public bool CAddress
        {
            get { return _CAddress; }
            set { _CAddress = value; OnPropertyChanged("CAddress"); }
        }

        private bool _Mobile;

        public bool Mobile
        {
            get { return _Mobile; }
            set { _Mobile = value; OnPropertyChanged("Mobile"); }
        }

        private bool _CivilStatus;

        public bool CivilStatus
        {
            get { return _CivilStatus; }
            set { _CivilStatus = value; OnPropertyChanged("CivilStatus"); }
        }

        private bool _Birthday;

        public bool Birthday
        {
            get { return _Birthday; }
            set { _Birthday = value; OnPropertyChanged("Birthday"); }
        }

        private bool _Branch;

        public bool Branch
        {
            get { return _Branch; }
            set { _Branch = value; OnPropertyChanged("Branch"); }
        }

        private bool _Department;

        public bool Department
        {
            get { return _Department; }
            set { _Department = value; OnPropertyChanged("Department"); }
        }

        private bool _Section;

        public bool Section
        {
            get { return _Section; }
            set { _Section = value; OnPropertyChanged("Section"); }
        }

        private bool _Designation;

        public bool Designation
        {
            get { return _Designation; }
            set { _Designation = value; OnPropertyChanged("Designation"); }
        }

        private bool _Grade;

        public bool Grade
        {
            get { return _Grade; }
            set { _Grade = value; OnPropertyChanged("Grade"); }
        }

        private bool _BasicSalary;

        public bool BasicSalary
        {
            get { return _BasicSalary; }
            set { _BasicSalary = value; OnPropertyChanged("BasicSalary"); }
        }

        private bool _JoinDate;

        public bool JoinDate
        {
            get { return _JoinDate; }
            set { _JoinDate = value; OnPropertyChanged("JoinDate"); }
        }

        private bool _ResignDate;

        public bool ResignDate
        {
            get { return _ResignDate; }
            set { _ResignDate = value; OnPropertyChanged("ResignDate"); }
        }

        private bool _Service;

        public bool Service
        {
            get { return _Service; }
            set { _Service = value; OnPropertyChanged("Service"); }
        }

        private bool _AQ;

        public bool AQ
        {
            get { return _AQ; }
            set { _AQ = value; OnPropertyChanged("AQ"); }
        }

        private bool _PQ;

        public bool PQ
        {
            get { return _PQ; }
            set { _PQ = value; OnPropertyChanged("PQ"); }
        }
        #endregion

        #region Refresh Methods
        private void RefreshEmployeeBasicDetails()
        {
            try
            {
                EmployeeBasicDetails = serviceClient.GetEmployeeBasicDetailsForExcel().OrderBy(c => c.emp_id);
            }
            catch (Exception)
            {
                EmployeeBasicDetails = null;
            }
        }

        private void RefreshEmployeeAQDetails()
        {
            try
            {
                EmployeeAQDetails = serviceClient.GetEmployeeAQDetailsForExcel().OrderBy(c => c.emp_id);
            }
            catch (Exception)
            {
                EmployeeAQDetails = null;
            }
        }

        private void RefreshEmployeePQDetails()
        {
            try
            {
                EmployeePQDetails = serviceClient.GetEmployeePQDetailsForExcel().OrderBy(c => c.emp_id);
            }
            catch (Exception)
            {
                EmployeePQDetails = null;
            }
        }
        #endregion

        #region Commands And Methods
        public ICommand Print
        {
            get
            {
                return new RelayCommand(GenerateExcel);
            }
        }

        private void GenerateExcel()
        {
            try
            {
                if (Validate())
                {
                    RefreshEmployeeBasicDetails();
                    if (EmployeeBasicDetails != null && EmployeeBasicDetails.Count() > 0)
                    {
                        if (AQ)
                        {
                            RefreshEmployeeAQDetails();
                        }
                        if (PQ)
                        {
                            RefreshEmployeePQDetails();
                        }
                        BusyBox.ShowBusy("Please Wait Until Generate Completed...");
                        List<string> ColumnList = new List<string>();
                        if (EPFNo)
                        {
                            ColumnList.Add("EPF No");
                        }
                        if (EPFName)
                        {
                            ColumnList.Add("EPF Name");
                        }
                        if (Gender)
                        {
                            ColumnList.Add("Gender");
                        }
                        if (Title)
                        {
                            ColumnList.Add("Title");
                        }
                        if (NIC)
                        {
                            ColumnList.Add("NIC");
                        }
                        if (Race)
                        {
                            ColumnList.Add("Race");
                        }
                        if (ReportColumnActiveInactive)
                        {
                            ColumnList.Add("Active/Inactive");
                        }
                        if (ReportColumnExNonex)
                        {
                            ColumnList.Add("Executive/Nonexecutive");
                        }
                        if (Religion)
                        {
                            ColumnList.Add("Religion");
                        }
                        if (District)
                        {
                            ColumnList.Add("District");
                        }
                        if (PAddress)
                        {
                            ColumnList.Add("Permanent Address");
                        }
                        if (CAddress)
                        {
                            ColumnList.Add("Current Address");
                        }
                        if (Mobile)
                        {
                            ColumnList.Add("Mobile");
                        }
                        if (CivilStatus)
                        {
                            ColumnList.Add("Civil Status");
                        }
                        if (Birthday)
                        {
                            ColumnList.Add("Birthday");
                        }
                        if (Branch)
                        {
                            ColumnList.Add("Branch");
                        }
                        if (Department)
                        {
                            ColumnList.Add("Department");
                        }
                        if (Section)
                        {
                            ColumnList.Add("Section");
                        }
                        if (Designation)
                        {
                            ColumnList.Add("Designation");
                        }
                        if (Grade)
                        {
                            ColumnList.Add("Grade");
                        }
                        if (BasicSalary)
                        {
                            ColumnList.Add("Basic Salary");
                        }
                        if (JoinDate)
                        {
                            ColumnList.Add("Join Date");
                        }
                        if (ResignDate)
                        {
                            ColumnList.Add("Resign Date");
                        }
                        if (Service)
                        {
                            ColumnList.Add("Service");
                        }
                        if (AQ)
                        {
                            ColumnList.Add("Academic Qualifications");
                        }
                        if (PQ)
                        {
                            ColumnList.Add("Professional Qualification");
                        }

                        if (BasicInfo)
                        {
                            DirectoryPath = @"C:\\H2SO4\\HRExcelReports\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            char c = 'A';
                            int i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                ws.Range[c + "1"].Value = ColumnListItem;
                                c++;
                            }
                            c = 'A';
                            i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                if (ColumnListItem.Equals("EPF No"))
                                {
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_no != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_no;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("EPF Name"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Gender"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.gender != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.gender;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Title"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.title_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.title_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("NIC"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.nic != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.nic;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Race"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.race_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.race_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Active/Inactive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.isActive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Active";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "In-Active";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Executive/Nonexecutive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.isExecutive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isExecutive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Executive";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Non-Executive";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Religion"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.religen_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.religen_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("District"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.city != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.city;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Permanent Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Current Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Mobile"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.mobile_number1 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.mobile_number1;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Civil Status"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.civil_status != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.civil_status;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Birthday"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Branch"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.companyBranch_Name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.companyBranch_Name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Department"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.department_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.department_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Section"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.section_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.section_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Designation"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.designation != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.designation;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Grade"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.grade != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.grade;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Basic Salary"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.basic_salary != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.basic_salary;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Join Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.join_date;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Resign Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.resign_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.resign_date;
                                        }
                                        else if (CurrentEmployeeBasicDetail.birthday != null && CurrentEmployeeBasicDetail.resign_date == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday.Value.AddYears(60);
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Birthday Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Service"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = (DateTime.Now.Date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                            }
                                            else
                                            {
                                                if (CurrentEmployeeBasicDetail.resign_date != null)
                                                {
                                                    ws.Range[c + i.ToString()].Value = ((DateTime)CurrentEmployeeBasicDetail.resign_date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Resigned Date Not Entered";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Join Date Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Academic Qualifications"))
                                {
                                    if (EmployeeAQDetails != null && EmployeeAQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                        {
                                            List<EmployeeAQDetailsView> AQList = EmployeeAQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (AQList != null)
                                            {
                                                string AQLine = "";
                                                foreach (var CurrentAQ in AQList)
                                                {
                                                    AQLine += CurrentAQ.school_name + ", " + CurrentAQ.school_qualifiaction_type + ", " + CurrentAQ.subject + ", " + CurrentAQ.school_grade_type + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = AQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        } 
                                    }
                                }
                                if (ColumnListItem.Equals("Professional Qualification"))
                                {
                                    if (EmployeePQDetails != null && EmployeePQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                        {
                                            List<EmployeePQDetailsView> PQList = EmployeePQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (PQList != null)
                                            {
                                                string PQLine = "";
                                                foreach (var CurrentPQ in PQList)
                                                {
                                                    PQLine += CurrentPQ.univercity_name + ", " + CurrentPQ.univercity_Course_name + ", " + CurrentPQ.univercity_Course_type + ", " + CurrentPQ.grade + ", " + ", " + CurrentPQ.gpa + ", " + CurrentPQ.duration + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = PQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        } 
                                    }
                                }
                                c++;
                            }
                            wb.SaveAs("C:\\H2SO4\\HRExcelReports\\HRReport.xlsx");
                            Marshal.ReleaseComObject(app);
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("Report Generated Successfully");
                        }
                        else if (ActiveInactive && ReportTypeActiveInactive)
                        {
                            DirectoryPath = @"C:\\H2SO4\\HRExcelReports\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            char c = 'A';
                            int i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                ws.Range[c + "1"].Value = ColumnListItem;
                                c++;
                            }
                            c = 'A';
                            i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                if (ColumnListItem.Equals("EPF No"))
                                {
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_no != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_no;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("EPF Name"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Gender"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.gender != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.gender;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Title"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.title_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.title_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("NIC"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.nic != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.nic;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Race"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.race_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.race_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Active/Inactive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.isActive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Active";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "In-Active";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Executive/Nonexecutive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.isExecutive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isExecutive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Executive";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Non-Executive";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Religion"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.religen_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.religen_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("District"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.city != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.city;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Permanent Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Current Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Mobile"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.mobile_number1 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.mobile_number1;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Civil Status"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.civil_status != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.civil_status;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Birthday"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Branch"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.companyBranch_Name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.companyBranch_Name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Department"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.department_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.department_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Section"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.section_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.section_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Designation"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.designation != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.designation;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Grade"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.grade != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.grade;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Basic Salary"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.basic_salary != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.basic_salary;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Join Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.join_date;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Resign Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.resign_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.resign_date;
                                        }
                                        else if (CurrentEmployeeBasicDetail.birthday != null && CurrentEmployeeBasicDetail.resign_date == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday.Value.AddYears(60);
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Birthday Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Service"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = (DateTime.Now.Date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                            }
                                            else
                                            {
                                                if (CurrentEmployeeBasicDetail.resign_date != null)
                                                {
                                                    ws.Range[c + i.ToString()].Value = ((DateTime)CurrentEmployeeBasicDetail.resign_date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Resigned Date Not Entered";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Join Date Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Academic Qualifications"))
                                {
                                    if (EmployeeAQDetails != null && EmployeeAQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                        {
                                            List<EmployeeAQDetailsView> AQList = EmployeeAQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (AQList != null)
                                            {
                                                string AQLine = "";
                                                foreach (var CurrentAQ in AQList)
                                                {
                                                    AQLine += CurrentAQ.school_name + ", " + CurrentAQ.school_qualifiaction_type + ", " + CurrentAQ.subject + ", " + CurrentAQ.school_grade_type + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = AQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Professional Qualification"))
                                {
                                    if (EmployeePQDetails != null && EmployeePQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == true))
                                        {
                                            List<EmployeePQDetailsView> PQList = EmployeePQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (PQList != null)
                                            {
                                                string PQLine = "";
                                                foreach (var CurrentPQ in PQList)
                                                {
                                                    PQLine += CurrentPQ.univercity_name + ", " + CurrentPQ.univercity_Course_name + ", " + CurrentPQ.univercity_Course_type + ", " + CurrentPQ.grade + ", " + ", " + CurrentPQ.gpa + ", " + CurrentPQ.duration + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = PQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        }
                                    }
                                }
                                c++;
                            }
                            wb.SaveAs("C:\\H2SO4\\HRExcelReports\\HRReport.xlsx");
                            Marshal.ReleaseComObject(app);
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("Report Generated Successfully");
                        }
                        else if (ActiveInactive && !ReportTypeActiveInactive)
                        {
                            DirectoryPath = @"C:\\H2SO4\\HRExcelReports\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            char c = 'A';
                            int i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                ws.Range[c + "1"].Value = ColumnListItem;
                                c++;
                            }
                            c = 'A';
                            i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                if (ColumnListItem.Equals("EPF No"))
                                {
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_no != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_no;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("EPF Name"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Gender"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.gender != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.gender;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Title"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.title_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.title_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("NIC"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.nic != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.nic;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Race"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.race_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.race_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Active/Inactive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.isActive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Active";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "In-Active";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Executive/Nonexecutive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.isExecutive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isExecutive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Executive";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Non-Executive";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Religion"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.religen_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.religen_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("District"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.city != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.city;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Permanent Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Current Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Mobile"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.mobile_number1 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.mobile_number1;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Civil Status"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.civil_status != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.civil_status;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Birthday"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Branch"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.companyBranch_Name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.companyBranch_Name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Department"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.department_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.department_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Section"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.section_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.section_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Designation"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.designation != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.designation;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Grade"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.grade != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.grade;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Basic Salary"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.basic_salary != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.basic_salary;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Join Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.join_date;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Resign Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.resign_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.resign_date;
                                        }
                                        else if (CurrentEmployeeBasicDetail.birthday != null && CurrentEmployeeBasicDetail.resign_date == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday.Value.AddYears(60);
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Birthday Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Service"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = (DateTime.Now.Date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                            }
                                            else
                                            {
                                                if (CurrentEmployeeBasicDetail.resign_date != null)
                                                {
                                                    ws.Range[c + i.ToString()].Value = ((DateTime)CurrentEmployeeBasicDetail.resign_date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Resigned Date Not Entered";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Join Date Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Academic Qualifications"))
                                {
                                    if (EmployeeAQDetails != null && EmployeeAQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                        {
                                            List<EmployeeAQDetailsView> AQList = EmployeeAQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (AQList != null)
                                            {
                                                string AQLine = "";
                                                foreach (var CurrentAQ in AQList)
                                                {
                                                    AQLine += CurrentAQ.school_name + ", " + CurrentAQ.school_qualifiaction_type + ", " + CurrentAQ.subject + ", " + CurrentAQ.school_grade_type + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = AQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Professional Qualification"))
                                {
                                    if (EmployeePQDetails != null && EmployeePQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isActive == false))
                                        {
                                            List<EmployeePQDetailsView> PQList = EmployeePQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (PQList != null)
                                            {
                                                string PQLine = "";
                                                foreach (var CurrentPQ in PQList)
                                                {
                                                    PQLine += CurrentPQ.univercity_name + ", " + CurrentPQ.univercity_Course_name + ", " + CurrentPQ.univercity_Course_type + ", " + CurrentPQ.grade + ", " + ", " + CurrentPQ.gpa + ", " + CurrentPQ.duration + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = PQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        }
                                    }
                                }
                                c++;
                            }
                            wb.SaveAs("C:\\H2SO4\\HRExcelReports\\HRReport.xlsx");
                            Marshal.ReleaseComObject(app);
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("Report Generated Successfully");
                        }
                        else if (ExNonex && ReportTypeExNonex)
                        {
                            DirectoryPath = @"C:\\H2SO4\\HRExcelReports\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            char c = 'A';
                            int i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                ws.Range[c + "1"].Value = ColumnListItem;
                                c++;
                            }
                            c = 'A';
                            i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                if (ColumnListItem.Equals("EPF No"))
                                {
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_no != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_no;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("EPF Name"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Gender"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.gender != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.gender;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Title"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.title_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.title_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("NIC"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.nic != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.nic;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Race"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.race_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.race_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Active/Inactive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.isActive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Active";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "In-Active";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Executive/Nonexecutive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.isExecutive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isExecutive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Executive";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Non-Executive";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Religion"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.religen_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.religen_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("District"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.city != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.city;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Permanent Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Current Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Mobile"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.mobile_number1 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.mobile_number1;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Civil Status"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.civil_status != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.civil_status;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Birthday"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Branch"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.companyBranch_Name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.companyBranch_Name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Department"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.department_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.department_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Section"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.section_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.section_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Designation"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.designation != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.designation;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Grade"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.grade != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.grade;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Basic Salary"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.basic_salary != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.basic_salary;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Join Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.join_date;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Resign Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.resign_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.resign_date;
                                        }
                                        else if (CurrentEmployeeBasicDetail.birthday != null && CurrentEmployeeBasicDetail.resign_date == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday.Value.AddYears(60);
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Birthday Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Service"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = (DateTime.Now.Date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                            }
                                            else
                                            {
                                                if (CurrentEmployeeBasicDetail.resign_date != null)
                                                {
                                                    ws.Range[c + i.ToString()].Value = ((DateTime)CurrentEmployeeBasicDetail.resign_date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Resigned Date Not Entered";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Join Date Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Academic Qualifications"))
                                {
                                    if (EmployeeAQDetails != null && EmployeeAQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                        {
                                            List<EmployeeAQDetailsView> AQList = EmployeeAQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (AQList != null)
                                            {
                                                string AQLine = "";
                                                foreach (var CurrentAQ in AQList)
                                                {
                                                    AQLine += CurrentAQ.school_name + ", " + CurrentAQ.school_qualifiaction_type + ", " + CurrentAQ.subject + ", " + CurrentAQ.school_grade_type + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = AQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Professional Qualification"))
                                {
                                    if (EmployeePQDetails != null && EmployeePQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == true))
                                        {
                                            List<EmployeePQDetailsView> PQList = EmployeePQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (PQList != null)
                                            {
                                                string PQLine = "";
                                                foreach (var CurrentPQ in PQList)
                                                {
                                                    PQLine += CurrentPQ.univercity_name + ", " + CurrentPQ.univercity_Course_name + ", " + CurrentPQ.univercity_Course_type + ", " + CurrentPQ.grade + ", " + ", " + CurrentPQ.gpa + ", " + CurrentPQ.duration + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = PQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        }
                                    }
                                }
                                c++;
                            }
                            wb.SaveAs("C:\\H2SO4\\HRExcelReports\\HRReport.xlsx");
                            Marshal.ReleaseComObject(app);
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("Report Generated Successfully");
                        }
                        else if (ExNonex && !ReportTypeExNonex)
                        {
                            DirectoryPath = @"C:\\H2SO4\\HRExcelReports\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            char c = 'A';
                            int i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                ws.Range[c + "1"].Value = ColumnListItem;
                                c++;
                            }
                            c = 'A';
                            i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                if (ColumnListItem.Equals("EPF No"))
                                {
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_no != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_no;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("EPF Name"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.epf_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Gender"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.gender != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.gender;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Title"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.title_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.title_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("NIC"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.nic != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.nic;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Race"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.race_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.race_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Active/Inactive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.isActive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Active";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "In-Active";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Executive/Nonexecutive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.isExecutive != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isExecutive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Executive";
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Non-Executive";
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Religion"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.religen_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.religen_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("District"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.city != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.city;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Permanent Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Current Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line3;
                                        }

                                        else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Mobile"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.mobile_number1 != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.mobile_number1;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Civil Status"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.civil_status != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.civil_status;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Birthday"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Branch"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.companyBranch_Name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.companyBranch_Name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Department"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.department_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.department_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Section"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.section_name != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.section_name;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Designation"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.designation != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.designation;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Grade"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.grade != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.grade;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Basic Salary"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.basic_salary != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.basic_salary;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Join Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.join_date;
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Resign Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.resign_date != null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.resign_date;
                                        }
                                        else if (CurrentEmployeeBasicDetail.birthday != null && CurrentEmployeeBasicDetail.resign_date == null)
                                        {
                                            ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday.Value.AddYears(60);
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Birthday Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Service"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                    {
                                        if (CurrentEmployeeBasicDetail.join_date != null)
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive == true)
                                            {
                                                ws.Range[c + i.ToString()].Value = (DateTime.Now.Date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                            }
                                            else
                                            {
                                                if (CurrentEmployeeBasicDetail.resign_date != null)
                                                {
                                                    ws.Range[c + i.ToString()].Value = ((DateTime)CurrentEmployeeBasicDetail.resign_date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days";
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Resigned Date Not Entered";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ws.Range[c + i.ToString()].Value = "Join Date Not Entered";
                                        }
                                        i++;
                                    }
                                }
                                if (ColumnListItem.Equals("Academic Qualifications"))
                                {
                                    if (EmployeeAQDetails != null && EmployeeAQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                        {
                                            List<EmployeeAQDetailsView> AQList = EmployeeAQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (AQList != null)
                                            {
                                                string AQLine = "";
                                                foreach (var CurrentAQ in AQList)
                                                {
                                                    AQLine += CurrentAQ.school_name + ", " + CurrentAQ.school_qualifiaction_type + ", " + CurrentAQ.subject + ", " + CurrentAQ.school_grade_type + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = AQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Professional Qualification"))
                                {
                                    if (EmployeePQDetails != null && EmployeePQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails.Where(z => z.isExecutive == false))
                                        {
                                            List<EmployeePQDetailsView> PQList = EmployeePQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                            if (PQList != null)
                                            {
                                                string PQLine = "";
                                                foreach (var CurrentPQ in PQList)
                                                {
                                                    PQLine += CurrentPQ.univercity_name + ", " + CurrentPQ.univercity_Course_name + ", " + CurrentPQ.univercity_Course_type + ", " + CurrentPQ.grade + ", " + ", " + CurrentPQ.gpa + ", " + CurrentPQ.duration + "/ ";
                                                }
                                                ws.Range[c + i.ToString()].Value = PQLine;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++;
                                        }
                                    }
                                }
                                c++;
                            }
                            wb.SaveAs("C:\\H2SO4\\HRExcelReports\\HRReport.xlsx");
                            Marshal.ReleaseComObject(app);
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("Report Generated Successfully");
                        }
                        else if (ResignInfo)
                        {
                            DirectoryPath = @"C:\\H2SO4\\HRExcelReports\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            char c = 'A';
                            int i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                ws.Range[c + "1"].Value = ColumnListItem;
                                c++;
                            }
                            c = 'A';
                            i = 2;
                            foreach (var ColumnListItem in ColumnList)
                            {
                                if (ColumnListItem.Equals("EPF No"))
                                {
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.epf_no != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_no;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("EPF Name"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.epf_name != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.epf_name;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Gender"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.gender != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.gender;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Title"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.title_name != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.title_name;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("NIC"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.nic != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.nic;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Race"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.race_name != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.race_name;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Active/Inactive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.isActive != null)
                                            {
                                                if (CurrentEmployeeBasicDetail.isActive == true)
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Active";
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "In-Active";
                                                }
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Executive/Nonexecutive"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.isExecutive != null)
                                            {
                                                if (CurrentEmployeeBasicDetail.isExecutive == true)
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Executive";
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Non-Executive";
                                                }
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Religion"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.religen_name != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.religen_name;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("District"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.city != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.city;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Permanent Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                            }

                                            else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line2;
                                            }

                                            else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                            }

                                            else if (CurrentEmployeeBasicDetail.permant_addr_line1 != null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line1;
                                            }

                                            else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2 + ", " + CurrentEmployeeBasicDetail.permant_addr_line3;
                                            }

                                            else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 != null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line2;
                                            }

                                            else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.permant_addr_line3;
                                            }

                                            else if (CurrentEmployeeBasicDetail.permant_addr_line1 == null && CurrentEmployeeBasicDetail.permant_addr_line2 == null && CurrentEmployeeBasicDetail.permant_addr_line3 == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Current Address"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                            }

                                            else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line2;
                                            }

                                            else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                            }

                                            else if (CurrentEmployeeBasicDetail.current_addr_line1 != null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line1;
                                            }

                                            else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2 + ", " + CurrentEmployeeBasicDetail.current_addr_line3;
                                            }

                                            else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 != null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line2;
                                            }

                                            else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.current_addr_line3;
                                            }

                                            else if (CurrentEmployeeBasicDetail.current_addr_line1 == null && CurrentEmployeeBasicDetail.current_addr_line2 == null && CurrentEmployeeBasicDetail.current_addr_line3 == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Mobile"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.mobile_number1 != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.mobile_number1;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Civil Status"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.civil_status != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.civil_status;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Birthday"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.birthday != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Branch"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.companyBranch_Name != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.companyBranch_Name;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Department"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.department_name != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.department_name;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Section"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.section_name != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.section_name;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Designation"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.designation != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.designation;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Grade"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.grade != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.grade;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Basic Salary"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.basic_salary != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.basic_salary;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Join Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.join_date != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.join_date;
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Resign Date"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.resign_date != null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.resign_date;
                                            }
                                            else if (CurrentEmployeeBasicDetail.birthday != null && CurrentEmployeeBasicDetail.resign_date == null)
                                            {
                                                ws.Range[c + i.ToString()].Value = CurrentEmployeeBasicDetail.birthday.Value.AddYears(60);
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Birthday Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Service"))
                                {
                                    i = 2;
                                    foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                    {
                                        if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                        {
                                            if (CurrentEmployeeBasicDetail.join_date != null)
                                            {
                                                if (CurrentEmployeeBasicDetail.isActive == true)
                                                {
                                                    ws.Range[c + i.ToString()].Value = (DateTime.Now.Date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days"; 
                                                }
                                                else
                                                {
                                                    if (CurrentEmployeeBasicDetail.resign_date != null)
                                                    {
                                                        ws.Range[c + i.ToString()].Value = ((DateTime)CurrentEmployeeBasicDetail.resign_date - (DateTime)CurrentEmployeeBasicDetail.join_date).TotalDays + " Days"; 
                                                    }
                                                    else
                                                    {
                                                        ws.Range[c + i.ToString()].Value = "Resigned Date Not Entered";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ws.Range[c + i.ToString()].Value = "Join Date Not Entered";
                                            }
                                            i++; 
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Academic Qualifications"))
                                {
                                    if (EmployeeAQDetails != null && EmployeeAQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                        {
                                            if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                            {
                                                List<EmployeeAQDetailsView> AQList = EmployeeAQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                                if (AQList != null)
                                                {
                                                    string AQLine = "";
                                                    foreach (var CurrentAQ in AQList)
                                                    {
                                                        AQLine += CurrentAQ.school_name + ", " + CurrentAQ.school_qualifiaction_type + ", " + CurrentAQ.subject + ", " + CurrentAQ.school_grade_type + "/ ";
                                                    }
                                                    ws.Range[c + i.ToString()].Value = AQLine;
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Not Entered";
                                                }
                                                i++; 
                                            }
                                        }
                                    }
                                }
                                if (ColumnListItem.Equals("Professional Qualification"))
                                {
                                    if (EmployeePQDetails != null && EmployeePQDetails.Count() > 0)
                                    {
                                        i = 2;
                                        foreach (var CurrentEmployeeBasicDetail in EmployeeBasicDetails)
                                        {
                                            if (CurrentEmployeeBasicDetail.birthday != null && FromDate <= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60) && ToDate >= CurrentEmployeeBasicDetail.birthday.Value.AddYears(60))
                                            {
                                                List<EmployeePQDetailsView> PQList = EmployeePQDetails.Where(z => z.employee_id == CurrentEmployeeBasicDetail.employee_id).ToList();
                                                if (PQList != null)
                                                {
                                                    string PQLine = "";
                                                    foreach (var CurrentPQ in PQList)
                                                    {
                                                        PQLine += CurrentPQ.univercity_name + ", " + CurrentPQ.univercity_Course_name + ", " + CurrentPQ.univercity_Course_type + ", " + CurrentPQ.grade + ", " + ", " + CurrentPQ.gpa + ", " + CurrentPQ.duration + "/ ";
                                                    }
                                                    ws.Range[c + i.ToString()].Value = PQLine;
                                                }
                                                else
                                                {
                                                    ws.Range[c + i.ToString()].Value = "Not Entered";
                                                }
                                                i++; 
                                            }
                                        }
                                    }
                                }
                                c++;
                            }
                            wb.SaveAs("C:\\H2SO4\\HRExcelReports\\HRReport.xlsx");
                            Marshal.ReleaseComObject(app);
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("Report Generated Successfully");
                        } 
                    }
                    else
                    {
                        clsMessages.setMessage("Error in Report Generating");
                    }
                }
                else
                {
                    clsMessages.setMessage("Please Select Atleast One Column");
                }
            }
            catch (Exception ex)
            {
                BusyBox.CloseBusy();
                clsMessages.setMessage(ex.Message);
            }
        }

        private bool Validate()
        {
            if (!EPFNo && !EPFName && !Gender && !Title && !NIC && !Race && !ReportColumnActiveInactive && !ReportColumnExNonex && !Religion && !District && !PAddress && !CAddress && !Mobile && !CivilStatus && !Birthday && !Branch && !Department && !Section && !Grade && !BasicSalary && !JoinDate && !ResignDate && !Service && !AQ && !PQ)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void CreatSubFolder()
        {
            if (Directory.Exists(DirectoryPath) == false)
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }
        #endregion
    }
}
