using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportData.NewUserControl
{
    public class NewSalaryReconciliationViewModel : ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();
        public List<z_Department> department = new List<z_Department>();
        public List<z_Section> section = new List<z_Section>();
        public List<EmployeeSumarryView> employee = new List<EmployeeSumarryView>();
        public List<z_Designation> designation = new List<z_Designation>();
        public NewSalaryReconciliationViewModel()
        {
            GetCompanyBranchList();
            GetPeriodList();
            GetDepartmentList();
            GetSectionList();
            GetGradeList();
            GetDesignationList();
            GetEmployeeList();
            GetPaymethodsList();
            FilterFomula = "";
            isExecutive = true;
        }
        private IEnumerable<z_Period> _Periods;
        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; this.OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;
        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; this.OnPropertyChanged("CurrentPeriod"); }
        }

        private IEnumerable<z_CompanyBranches> _CompanyBranches;
        public IEnumerable<z_CompanyBranches> CompanyBranche
        {
            get { return _CompanyBranches; }
            set { _CompanyBranches = value; this.OnPropertyChanged("CompanyBranche"); }
        }

        private z_CompanyBranches _CurrentCompanyBranch;
        public z_CompanyBranches CurrentCompanyBranch
        {
            get { return _CurrentCompanyBranch; }
            set { _CurrentCompanyBranch = value; }
        }

        private IEnumerable<z_Department> _Departments;
        public IEnumerable<z_Department> Departments
        {
            get { return _Departments; }
            set { _Departments = value; this.OnPropertyChanged("Departments"); }
        }

        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set { _CurrentDepartment = value; this.OnPropertyChanged("CurrentDepartment"); }
        }

        private IEnumerable<z_Section> _Section;
        public IEnumerable<z_Section> Section
        {
            get { return _Section; }
            set { _Section = value; this.OnPropertyChanged("Section"); }
        }

        private z_Section _CurrentSection;
        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value; this.OnPropertyChanged("CurrentSection"); }
        }

        private IEnumerable<z_Grade> _Grades;
        public IEnumerable<z_Grade> Grades
        {
            get { return _Grades; }
            set { _Grades = value; this.OnPropertyChanged("Grades"); }
        }
        private z_Grade _CurrentGrade;
        public z_Grade CurrentGrade
        {
            get { return _CurrentGrade; }
            set { _CurrentGrade = value; this.OnPropertyChanged("CurrentGrade"); }
        }

        private IEnumerable<z_Designation> _Designation;
        public IEnumerable<z_Designation> Designation
        {
            get { return _Designation; }
            set { _Designation = value; this.OnPropertyChanged("Designation"); }
        }

        private z_Designation _CurrentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return _CurrentDesignation; }
            set { _CurrentDesignation = value; this.OnPropertyChanged("CurrentDesignation"); }
        }

        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; this.OnPropertyChanged("Employees"); }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; this.OnPropertyChanged("CurrentEmployee"); }
        }

        private IEnumerable<z_ShiftCategory> _ShiftCatagory;
        public IEnumerable<z_ShiftCategory> ShiftCatagory
        {
            get { return _ShiftCatagory; }
            set { _ShiftCatagory = value; this.OnPropertyChanged("ShiftCatagory"); }
        }


        private z_ShiftCategory _CurrentShiftCatagory;
        public z_ShiftCategory CurrentShiftCatagory
        {
            get { return _CurrentShiftCatagory; }
            set { _CurrentShiftCatagory = value; this.OnPropertyChanged("CurrentShiftCatagory"); }
        }

        private IEnumerable<z_PaymentMethod> _PaymentMethod;
        public IEnumerable<z_PaymentMethod> PaymentMethod
        {
            get { return _PaymentMethod; }
            set { _PaymentMethod = value; OnPropertyChanged("PaymentMethod"); }
        }

        private z_PaymentMethod _CurrentPaymentMethod;
        public z_PaymentMethod CurrentPaymentMethod
        {
            get { return _CurrentPaymentMethod; }
            set { _CurrentPaymentMethod = value; OnPropertyChanged("CurrentPaymentMethod"); }
        }

        private string _FilterFomula;
        public string FilterFomula
        {
            get { return _FilterFomula; }
            set { _FilterFomula = value; this.OnPropertyChanged("FilterFomula"); }
        }

        public ICommand PrintBTN
        {
            get { return new RelayCommand(Print, PrintCanExecute); }
        }

        private bool _isExecutive;

        public bool isExecutive
        {
            get { return _isExecutive; }
            set { _isExecutive = value; OnPropertyChanged("isExecutive"); }
        }


        private void Print()
        {
            try
            {
                //EmployeePayrollSummaryDataSet DataSetSummary = new EmployeePayrollSummaryDataSet();
                //IEnumerable<GetEmployeeMonthlyPayrollSumarryReconsiliation_Result> Summary = serviceClient.GetEmployeePayrollForReconsilation(CurrentPeriod.period_id.ToString(), isExecutive);


                //if (Summary != null)
                //{
                //    if (CurrentCompanyBranch != null)
                //    {
                //        Summary = Summary.Where(z => z.branch_id == CurrentCompanyBranch.companyBranch_id);
                //    }
                //    if (CurrentDepartment != null)
                //    {
                //        Summary = Summary.Where(z => z.department_id == CurrentDepartment.department_id);
                //    }
                //    if (CurrentDesignation != null)
                //    {
                //        Summary = Summary.Where(z => z.designation_id == CurrentDesignation.designation_id);
                //    }
                //    if (CurrentSection != null)
                //    {
                //        Summary = Summary.Where(z => z.section_id == CurrentSection.section_id);
                //    }
                //    if (CurrentGrade != null)
                //    {
                //        Summary = Summary.Where(z => z.grade_id == CurrentGrade.grade_id);
                //    }
                //    if (CurrentEmployee != null)
                //    {
                //        Summary = Summary.Where(z => z.emp_id == CurrentEmployee.emp_id);
                //    }
                //    if(CurrentPaymentMethod !=null)
                //    {
                //        Summary = Summary.Where(z => z.payment_methord_id == CurrentPaymentMethod.paymet_method_id);
                //    }

                //        foreach (var item in Summary.ToList())
                //        {
                //            EmployeePayrollSummaryDataSet.GetEmployeeMonthlyPayrollSumarryReconsiliationRow AddRow = DataSetSummary.GetEmployeeMonthlyPayrollSumarryReconsiliation.NewGetEmployeeMonthlyPayrollSumarryReconsiliationRow();
                //            AddRow.employee_id = item.employee_id == null ? Guid.Empty : item.employee_id.Value;
                //            AddRow.emp_id = item.emp_id;
                //            AddRow.epf_no = item.epf_no == null ? "0000" : item.epf_no;
                //            AddRow.etf_no = item.etf_no == null ? "0000" : item.etf_no;
                //            AddRow.branch_id = item.branch_id.Value == null ? Guid.Empty : item.branch_id.Value;
                //            AddRow.companyBranch_Name = item.companyBranch_Name == null ? "System" : item.companyBranch_Name;

                //            AddRow.initials = item.initials == null ? "" : item.initials;
                //            AddRow.first_name = item.first_name == null ? "" : item.first_name;
                //            AddRow.second_name = item.second_name == null ? "" : item.second_name;
                //            AddRow.surname = item.surname == null ? "" : item.surname;
                //            AddRow.nic = item.nic == null ? "" : item.nic;
                //            AddRow.department_id = item.department_id.Value == null ? Guid.Empty : item.department_id.Value;
                //            AddRow.designation_id = item.designation_id.Value == null ? Guid.Empty : item.designation_id.Value;
                //            AddRow.section_id = item.section_id.Value == null ? Guid.Empty : item.section_id.Value;
                //            AddRow.grade_id = item.grade_id.Value == null ? Guid.Empty : item.grade_id.Value;


                //            AddRow.companyaddress_01 = item.companyaddress_01 == null ? "" : item.companyaddress_01;
                //            AddRow.compayaddress_02 = item.compayaddress_02 == null ? "" : item.compayaddress_02;
                //            AddRow.companyAddress_03 = item.companyAddress_03 == null ? "" : item.companyAddress_03;

                //           // AddRow.currentBasicsalary = item.basicSalary == null ? 0m : item.basicSalary.Value;
                //            AddRow.company_name_01 = item.company_name_01 == null ? "" : item.company_name_01;
                //            AddRow.comapany_name_02 = item.comapany_name_02 == null ? "" : item.comapany_name_02;
                //            AddRow.companyImage = item.companyImage == null ? "" : item.companyImage;

                //            AddRow.companyaddress_01 = item.companyaddress_01 == null ? "" : item.companyaddress_01;

                //            AddRow.compayaddress_02 = item.compayaddress_02 == null ? "" : item.compayaddress_02;

                //            AddRow.companyAddress_03 = item.companyAddress_03 == null ? "" : item.companyAddress_03;
                //            AddRow.telephone_01 = item.telephone_01 == null ? "" : item.telephone_01;
                //            AddRow.telephone_02 = item.telephone_02 == null ? "" : item.telephone_02;
                //            AddRow.fax = item.fax == null ? "" : item.fax;
                //            AddRow.compayemail = item.compayemail == null ? "" : item.compayemail;
                //            AddRow.web = item.web == null ? "" : item.web;
                //            AddRow.department_name = item.department_name == null ? "" : item.department_name;
                //            AddRow.designation = item.designation == null ? "" : item.designation;
                //            AddRow.section_name = item.section_name == null ? "" : item.section_name;
                //            AddRow.grade = item.grade == null ? "" : item.grade;
                //            AddRow.period_id = item.period_id == null ? Guid.Empty : item.period_id.Value;
                //            AddRow.period_name = item.period_name == null ? "" : item.period_name;
                //            AddRow.payment_methord_id = item.payment_methord_id == null ? Guid.Empty : item.payment_methord_id.Value;
                //            //AddRow.paydate = item.paydate == null ? DateTime.Now : DateTime.Parse(item.paydate.ToString());
                //            AddRow.rule_name = item.rule_name == null ? "" : item.rule_name;
                //            AddRow.unit_price = item.unit_price == null ? 0m : item.unit_price.Value;
                //            AddRow.total_amount = item.total_amount == null ? 0m : item.total_amount.Value;
                //            AddRow.catagory = item.catagory == null ? int.Parse("0")  :int.Parse(item.catagory.ToString());
                //            DataSetSummary.GetEmployeeMonthlyPayrollSumarryReconsiliation.AddGetEmployeeMonthlyPayrollSumarryReconsiliationRow(AddRow);

                //        }


                //    NewSalaryReconciliationReport rpt = new NewSalaryReconciliationReport();
                //    rpt.SetDataSource(DataSetSummary);
                //    ReportViewer r = new ReportViewer(rpt, false);
                //    r.Show();
                //}
                //else
                //{
                //    clsMessages.setMessage("No Transaction Data for this Period");
                //}
                string rpt_period_id = "";
                string rpt_companyBranch_id = "";
                string rpt_department_id = "";
                string rpt_section_id = "";
                string rpt_designation_id = "";
                string rpt_grade_id = "";
                string rpt_employee_id = "";
                string rpt_paymet_methord_id = "";

                if (CurrentPeriod != null)
                {
                    rpt_period_id = CurrentPeriod.period_id.ToString();
                }
                if (CurrentCompanyBranch != null)
                {
                    rpt_companyBranch_id = CurrentCompanyBranch.companyBranch_id.ToString();
                }
                if (CurrentDepartment != null)
                {
                    rpt_department_id = CurrentDepartment.department_id.ToString();
                }
                if (CurrentDesignation != null)
                {
                    rpt_designation_id = CurrentDesignation.designation_id.ToString();
                }
                if (CurrentSection != null)
                {
                    rpt_section_id = CurrentSection.section_id.ToString();
                }
                if (CurrentGrade != null)
                {
                    rpt_grade_id = CurrentGrade.grade_id.ToString();
                }
                if (CurrentEmployee != null)
                {
                    rpt_employee_id = CurrentEmployee.employee_id.ToString();
                }
                if (CurrentPaymentMethod != null)
                {
                    rpt_paymet_methord_id = CurrentPaymentMethod.paymet_method_id.ToString();
                }

                //IEnumerable<GetEmployeeMonthlyPayrollSumarryReconsiliation_Result> Summary = serviceClient.GetEmployeePayrollForReconsilation(rpt_period_id, rpt_companyBranch_id, rpt_department_id, rpt_designation_id, rpt_section_id, rpt_grade_id, rpt_employee_id, rpt_paymet_methord_id, isExecutive);

                //foreach (var item in Summary.ToList())
                //{
                //    EmployeePayrollSummaryDataSet.GetEmployeeMonthlyPayrollSumarryReconsiliationRow AddRow = DataSetSummary.GetEmployeeMonthlyPayrollSumarryReconsiliation.NewGetEmployeeMonthlyPayrollSumarryReconsiliationRow();
                //    AddRow.trns_id = item.trns_id;
                //    AddRow.employee_id = item.employee_id == null ? Guid.Empty : item.employee_id;
                //    AddRow.emp_id = item.emp_id;
                //    AddRow.epf_no = item.epf_no == null ? "0000" : item.epf_no;
                //    AddRow.etf_no = item.etf_no == null ? "0000" : item.etf_no;
                //    AddRow.branch_id = item.branch_id.Value == null ? Guid.Empty : item.branch_id.Value;
                //    AddRow.companyBranch_Name = item.companyBranch_Name == null ? "System" : item.companyBranch_Name;

                //    AddRow.initials = item.initials == null ? "" : item.initials;
                //    AddRow.first_name = item.first_name == null ? "" : item.first_name;
                //    AddRow.second_name = item.second_name == null ? "" : item.second_name;
                //    AddRow.surname = item.surname == null ? "" : item.surname;
                //    AddRow.nic = item.nic == null ? "" : item.nic;
                //    AddRow.department_id = item.department_id == null ? Guid.Empty : item.department_id;
                //    AddRow.designation_id = item.designation_id == null ? Guid.Empty : item.designation_id;
                //    AddRow.section_id = item.section_id.Value == null ? Guid.Empty : item.section_id.Value;
                //    AddRow.grade_id = item.grade_id.Value == null ? Guid.Empty : item.grade_id.Value;


                //    AddRow.companyaddress_01 = item.companyaddress_01 == null ? "" : item.companyaddress_01;
                //    AddRow.compayaddress_02 = item.compayaddress_02 == null ? "" : item.compayaddress_02;
                //    AddRow.companyAddress_03 = item.companyAddress_03 == null ? "" : item.companyAddress_03;

                //    // AddRow.currentBasicsalary = item.basicSalary == null ? 0m : item.basicSalary.Value;
                //    AddRow.company_name_01 = item.company_name_01 == null ? "" : item.company_name_01;
                //    AddRow.comapany_name_02 = item.comapany_name_02 == null ? "" : item.comapany_name_02;
                //    AddRow.companyImage = item.companyImage == null ? "" : item.companyImage;

                //    AddRow.companyaddress_01 = item.companyaddress_01 == null ? "" : item.companyaddress_01;

                //    AddRow.compayaddress_02 = item.compayaddress_02 == null ? "" : item.compayaddress_02;

                //    AddRow.companyAddress_03 = item.companyAddress_03 == null ? "" : item.companyAddress_03;
                //    AddRow.telephone_01 = item.telephone_01 == null ? "" : item.telephone_01;
                //    AddRow.telephone_02 = item.telephone_02 == null ? "" : item.telephone_02;
                //    AddRow.fax = item.fax == null ? "" : item.fax;
                //    AddRow.compayemail = item.compayemail == null ? "" : item.compayemail;
                //    AddRow.web = item.web == null ? "" : item.web;
                //    AddRow.department_name = item.department_name == null ? "" : item.department_name;
                //    AddRow.designation = item.designation == null ? "" : item.designation;
                //    AddRow.section_name = item.section_name == null ? "" : item.section_name;
                //    AddRow.grade = item.grade == null ? "" : item.grade;
                //    AddRow.period_id = item.period_id == null ? Guid.Empty : item.period_id;
                //    AddRow.period_name = item.period_name == null ? "" : item.period_name;
                //    AddRow.payment_methord_id = item.payment_methord_id == null ? Guid.Empty : item.payment_methord_id.Value;
                //    //AddRow.paydate = item.paydate == null ? DateTime.Now : DateTime.Parse(item.paydate.ToString());
                //    AddRow.rule_name = item.rule_name == null ? "" : item.rule_name;
                //    AddRow.unit_price = item.unit_price == null ? 0m : item.unit_price.Value;
                //    AddRow.total_amount = item.total_amount == null ? 0m : item.total_amount.Value;
                //    AddRow.catagory = item.catagory == null ? int.Parse("0") : int.Parse(item.catagory.ToString());
                //    DataSetSummary.GetEmployeeMonthlyPayrollSumarryReconsiliation.AddGetEmployeeMonthlyPayrollSumarryReconsiliationRow(AddRow);

                //}

                //if (CurrentPeriod != null)
                //{
                //    NewSalaryReconciliationReport rpt = new NewSalaryReconciliationReport();
                //    //rpt.SetDataSource(DataSetSummary);
                //    rpt.SetDataSource(IEnumerableToDataTable.ToDataTable(Summary.ToList()));
                //    ReportViewer r = new ReportViewer(rpt, false);
                //    r.Show();
                //}
                string[,] parameetors = new string[8, 2] { { "@timePeriodID", rpt_period_id }, { "@companyBranchid",rpt_companyBranch_id  },
                                        { "@dapartmentid", rpt_department_id },{"@designationid",rpt_designation_id},{"@sectionid", rpt_section_id},{"@gradeid", rpt_grade_id},{"@employeeid",rpt_employee_id},{"@paymentMethordId",rpt_paymet_methord_id} };//,{"@employeeType",isExecutive.ToString()}  };

                ReportViewer form = new ReportViewer();
                form.GetReportUsingParameetors(parameetors, "PayrollReport\\PayrollReportData", "NewSalaryReconciliationReport", FilterFomula, "");
                form.Show();

            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
        }


        private bool PrintCanExecute()
        {
            if (CurrentPeriod != null)
            {
                return true;
            }
            return false;
        }

        #region Period List

        public void GetPeriodList()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetPeriodsAsync();

        }

        #endregion

        #region Company Branch
        public void GetCompanyBranchList()
        {
            this.serviceClient.GetCompanyBranchesCompleted += (s, e) =>
            {
                CompanyBranche = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetCompanyBranchesAsync();

        }
        #endregion

        #region Department List
        public void GetDepartmentList()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                Departments = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetDepartmentsAsync();

        }
        #endregion

        #region Section List
        public void GetSectionList()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
            {
                Section = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetSectionsAsync();

        }
        #endregion

        #region Grade List
        public void GetGradeList()
        {
            this.serviceClient.GetGradeCompleted += (s, e) =>
            {
                Grades = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetGradeAsync();

        }
        #endregion

        #region Designation List
        public void GetDesignationList()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                Designation = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetDesignationsAsync();

        }
        #endregion

        #region Employee List
        public void GetEmployeeList()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                Employees = e.Result.Where(z => z.isdelete == false && z.isActive == true);
            };
            this.serviceClient.GetEmloyeeSearchAsync(clsSecurity.loggedUser.user_id);

        }
        #endregion

        #region Shift List
        public void GetEmployeeShift()
        {
            this.serviceClient.GetShiftcategoryCompleted += (s, e) =>
            {
                ShiftCatagory = e.Result.Where(z => z.isdelete == false && z.is_active == true);

            };
            this.serviceClient.GetShiftcategoryAsync();

        }
        #endregion

        #region Pay Method List

        private void GetPaymethodsList()
        {
            serviceClient.GetPaymentMethodsCompleted += (s, e) =>
            {
                PaymentMethod = e.Result;
            };
            serviceClient.GetPaymentMethodsAsync();
        }


        #endregion

    }
}