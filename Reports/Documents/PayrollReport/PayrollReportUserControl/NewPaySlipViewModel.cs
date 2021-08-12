using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    public class NewPaySlipViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();
        public List<z_Department> department = new List<z_Department>();
        public List<z_Section> section = new List<z_Section>();
        public List<EmployeeSumarryView> employee = new List<EmployeeSumarryView>();
        public List<z_Designation> designation = new List<z_Designation>();

        public NewPaySlipViewModel()
        {
            GetCompanyBranchList();
            GetPeriodList();
            GetDepartmentList();
            GetSectionList();
            GetGradeList();
            GetDesignationList();
            GetEmployeeList();
            GetEmployeeShift();
            GetPaymethodsList();
            FilterFomula = "";
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

        private void Print()
        {
            string rpt_period_id = "";
            string rpt_companyBranch_id = "";
            string rpt_department_id = "";
            string rpt_section_id = "";
            string rpt_designation_id = "";
            string rpt_grade_id = "";
            string rpt_employee_id = "";
            string rpt_shift_id = "";
            string rpt_paymet_methord_id = "";

            if (CurrentCompanyBranch != null)
            {
                rpt_companyBranch_id = CurrentCompanyBranch.bank_branch_id.ToString();
            }
            if (CurrentPeriod != null)
            {
                rpt_period_id = CurrentPeriod.period_id.ToString();
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
          
            if (CurrentPaymentMethod != null)
            {
                rpt_paymet_methord_id = CurrentPaymentMethod.paymet_method_id.ToString();
            }

            if (CurrentEmployee != null)
            {
                rpt_employee_id = CurrentEmployee.employee_id.ToString();
            }
            if (CurrentShiftCatagory != null)
            {
                rpt_shift_id = CurrentShiftCatagory.shift_id.ToString();
            }

            EmployeePayrollSummaryDataSet DataSetSummary = new EmployeePayrollSummaryDataSet();
            IEnumerable<GetEmployeeMonthlyPayrollSumarry_Result> Summary = serviceClient.GetEmployeePayrollSummarry_RPT(rpt_period_id, rpt_companyBranch_id, rpt_department_id, rpt_designation_id, rpt_section_id, rpt_grade_id, rpt_employee_id, rpt_paymet_methord_id);

            try
            {
                if (Summary != null)
                    foreach (var item in Summary.ToList())
                    {
                        EmployeePayrollSummaryDataSet.GetEmployeeMonthlyPayrollSumarryRow AddRow = DataSetSummary.GetEmployeeMonthlyPayrollSumarry.NewGetEmployeeMonthlyPayrollSumarryRow();
                        AddRow.employee_id = item.employee_id.Value;
                        AddRow.emp_id = item.emp_id;
                        AddRow.epf_no = item.epf_no;
                        AddRow.etf_no = item.etf_no;
                        AddRow.branch_id = item.branch_id.Value;
                        AddRow.companyBranch_Name = item.companyBranch_Name;
                        AddRow.initials = item.initials == null ? "" : item.initials;
                        AddRow.first_name = item.first_name == null ? "" : item.first_name;
                        AddRow.second_name = item.second_name == null ? "" : item.second_name;
                        AddRow.surname = item.surname == null ? "" : item.surname;
                        AddRow.nic = item.nic;
                        AddRow.isdelete = item.isdelete.Value;
                        AddRow.department_id = item.department_id.Value;
                        AddRow.designation_id = item.designation_id.Value;
                        AddRow.section_id = item.section_id.Value;
                        AddRow.grade_id = item.grade_id.Value;
                        AddRow.company_name_01 = item.company_name_01;
                        AddRow.comapany_name_02 = item.comapany_name_02;
                        AddRow.companyImage = item.companyImage;
                        AddRow.companyaddress_01 = item.companyaddress_01;
                        AddRow.compayaddress_02 = item.compayaddress_02;
                        AddRow.companyAddress_03 = item.companyAddress_03;
                        AddRow.telephone_01 = item.telephone_01;
                        AddRow.telephone_02 = item.telephone_02;
                        AddRow.fax = item.fax;
                        AddRow.compayemail = item.compayemail;
                        AddRow.web = item.web;
                        AddRow.department_name = item.department_name;
                        AddRow.designation = item.designation;
                        AddRow.section_name = item.section_name;
                        AddRow.grade = item.grade;
                        AddRow.isActive = item.isActive.Value;
                        AddRow.period_id = item.period_id.Value;
                        AddRow.rule_name = item.rule_name;
                        AddRow.unit_price = item.unit_price.Value;
                        AddRow.units = item.units.Value;
                        AddRow.total_amount = item.total_amount.Value;
                        AddRow.catagory = int.Parse(item.catagory.ToString());
                        AddRow.period_name = item.period_name;
                        AddRow.basic_salary = item.basic_salary.Value;
                        AddRow.total_benifit = item.total_benifit.Value;
                        AddRow.total_deduction = item.total_deduction.Value;
                        AddRow.grossSalary = item.grossSalary.Value;
                        AddRow.total_salary = item.total_salary.Value;
                        //AddRow.paydate = item.paydate.Value;
                        AddRow.isPaied = item.isPaied.Value;

                        DataSetSummary.GetEmployeeMonthlyPayrollSumarry.AddGetEmployeeMonthlyPayrollSumarryRow(AddRow);

                    }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Data Set Error");
            }

            PayslipNewMag rpt = new PayslipNewMag();
            rpt.SetDataSource(DataSetSummary);
            ReportViewer r = new ReportViewer(rpt, false);
            r.Show();
            //string[,] parameetors = new string[7, 2] { { "@timePeriodID", rpt_period_id }, { "@companyBranchid",rpt_companyBranch_id  },
            //                            { "@dapartmentid", rpt_department_id },{"@designationid",rpt_designation_id},{"@sectionid", rpt_section_id},{"@gradeid", rpt_grade_id},{"@employeeid",rpt_employee_id} };

            //ReportViewer form = new ReportViewer();
            //form.GetReportUsingParameetors(parameetors, "PayrollReport", "PaySlipNew", FilterFomula, "");
            //form.Show();

            //PaySlipNew xx = new PaySlipNew();
            
        }

        private bool PrintCanExecute()
        {
            if (CurrentPeriod != null)
            {
                return true;

            }
            return false;
        }

        public void GetPeriodList()
        {
            this.serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetPeriodsAsync();

        }

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
            this.serviceClient.GetAllEmployeeDetailAsync();

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
