using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportData.NewUserControl
{
    public class NewEmployeeTaxViewModel : ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<string> databasefield = new List<string>();
        public List<z_Department> department = new List<z_Department>();
        public List<z_Section> section = new List<z_Section>();
        public List<EmployeeSumarryView> employee = new List<EmployeeSumarryView>();
        public List<z_Designation> designation = new List<z_Designation>();

        public NewEmployeeTaxViewModel()
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
            GetEmployeeCompanyVariable();
            GetPaymethodsList();

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

        private IEnumerable<z_CompanyVariable> _CompanyVariables;
        public IEnumerable<z_CompanyVariable> CompanyVariables
        {
            get { return _CompanyVariables; }
            set { _CompanyVariables = value; this.OnPropertyChanged("CompanyVariables"); }
        }

        private z_CompanyVariable _CurrentCompanyVariable;
        public z_CompanyVariable CurrentCompanyVariable
        {
            get { return _CurrentCompanyVariable; }
            set { _CurrentCompanyVariable = value; this.OnPropertyChanged("CurrentCompanyVariable"); }
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
            //EmployeePayrollSummaryDataSet DataSetSummary = new EmployeePayrollSummaryDataSet();

            string rpt_period_id = "";
            string rpt_companyBranch_id = "";
            string rpt_department_id = "";
            string rpt_section_id = "";
            string rpt_designation_id = "";
            string rpt_grade_id = "";
            string rpt_employee_id = "";
            string rpt_shift_id = "";
            string rpt_paymet_methord_id = "";
            string rpt_company_rule_id = "";
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
            if (CurrentCompanyVariable != null)
            {
                rpt_company_rule_id = CurrentCompanyVariable.company_variableID.ToString();
            }
            if (CurrentPaymentMethod != null)
            {
                rpt_paymet_methord_id = CurrentPaymentMethod.paymet_method_id.ToString();
            }
            if (CurrentShiftCatagory != null)
            {
                rpt_shift_id = CurrentShiftCatagory.shift_id.ToString();
            }
            //try
            //{
            //    foreach (var item in Summary.ToList())
            //    {
            //        if(item.value !=0)
            //        {
            //        EmployeePayrollSummaryDataSet.EmployeeFundDetailRow AddRow = DataSetSummary.EmployeeFundDetail.NewEmployeeFundDetailRow();
            //        AddRow.emp_id = item.emp_id;
            //        AddRow.epf_no = item.epf_no == null ? "0000" : item.epf_no;
            //        AddRow.etf_no = item.etf_no == null ? "0000" : item.etf_no;

            //        AddRow.initials = item.initials == null ? "" : item.initials;
            //        AddRow.first_name = item.first_name == null ? "" : item.first_name;
            //        AddRow.second_name = item.second_name == null ? "" : item.second_name;
            //        AddRow.surname = item.surname == null ? "" : item.surname;
            //        AddRow.nic = item.nic == null ? "" : item.nic;
            //        AddRow.telephone = item.telephone == null ? "" : item.telephone;

            //        AddRow.branch_id = item.branch_id.Value == null ? Guid.Empty : item.branch_id.Value;
            //        AddRow.department_id = item.department_id == null ? Guid.Empty : item.department_id;
            //        AddRow.designation_id = item.designation_id == null ? Guid.Empty : item.designation_id;
            //        AddRow.section_id = item.section_id == null ? Guid.Empty : item.section_id.Value;
            //        AddRow.grade_id = item.grade_id == null ? Guid.Empty : item.grade_id.Value;
            //        AddRow.payment_methord_id = item.payment_methord_id == null ? Guid.Empty : item.payment_methord_id.Value;

            //        AddRow.company_id = item.company_id == null ? Guid.Empty : item.company_id;
            //        AddRow.companyBranch_id = item.companyBranch_id == null ? Guid.Empty : item.companyBranch_id;
            //        AddRow.companyBranch_Name = item.companyBranch_Name == null ? "System" : item.companyBranch_Name;
            //        AddRow.company_name_01 = item.company_name_01 == null ? "" : item.company_name_01;
            //        AddRow.comapany_name_02 = item.comapany_name_02 == null ? "" : item.comapany_name_02;
            //        AddRow.address_01 = item.address_01 == null ? "" : item.address_01;
            //        AddRow.address_02 = item.address_02 == null ? "" : item.address_02;
            //        AddRow.address_03 = item.address_03 == null ? "" : item.address_03;
            //        AddRow.image = item.image == null ? "" : item.image;
            //        AddRow.telephone_01 = item.telephone_01 == null ? "" : item.telephone_01;
            //        AddRow.telephone_02 = item.telephone_02 == null ? "" : item.telephone_02;
            //        AddRow.fax = item.fax == null ? "" : item.fax;
            //        AddRow.email = item.email == null ? "" : item.email;
            //        AddRow.web = item.web == null ? "" : item.web;
            //        AddRow.company_variableID = item.company_variableID == null ? Guid.Empty : item.company_variableID;
            //        AddRow.variable_Name = item.variable_Name == null ? "System" : item.variable_Name;
            //        AddRow.variable_Value = item.variable_Value == null ? 0m : item.variable_Value.Value;
            //        AddRow.value = item.value == null ? 0m : item.value.Value;
            //        AddRow.calculate_salary = item.calculate_salary == null ? 0m : item.calculate_salary.Value;
            //        AddRow.employee_id = item.employee_id == null ? Guid.Empty : item.employee_id;
            //        AddRow.period_id = item.period_id == null ? Guid.Empty : item.period_id;
            //        AddRow.period_name = item.period_name == null ? "" : item.period_name;
            //        AddRow.epf_name = item.epf_name == null ? "" : item.epf_name;

            //        DataSetSummary.EmployeeFundDetail.AddEmployeeFundDetailRow(AddRow);

            //        }
            //    }
            //}
            IEnumerable<EmployeeFundDetail_Result> Summary = serviceClient.GetEmployeeFundDetail(rpt_period_id, rpt_companyBranch_id, rpt_department_id, rpt_designation_id, rpt_section_id, rpt_grade_id, rpt_employee_id, rpt_paymet_methord_id, rpt_company_rule_id);

            if (CurrentPeriod != null && CurrentCompanyVariable != null)
            {
                NewEmployeeFundReport rpt = new NewEmployeeFundReport();
                //rpt.SetDataSource(DataSetSummary);
                rpt.SetDataSource(IEnumerableToDataTable.ToDataTable(Summary.ToList()));
                ReportViewer r = new ReportViewer(rpt, false);
                r.Show(); 
            }
        }

        private bool PrintCanExecute()
        {
            if (CurrentPeriod != null)
            {
                if (CurrentCompanyVariable != null)
                {
                    return true;

                }
                else
                {
                    return false;
                }
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
        public void GetEmployeeCompanyVariable()
        {
            this.serviceClient.GetCompanyVariablesCompleted += (s, e) =>
            {
                CompanyVariables = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetCompanyVariablesAsync();

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

        private void GetPaymethodsList()
        {
            serviceClient.GetPaymentMethodsCompleted += (s, e) =>
            {
                PaymentMethod = e.Result;
            };
            serviceClient.GetPaymentMethodsAsync();
        }

    }
}


