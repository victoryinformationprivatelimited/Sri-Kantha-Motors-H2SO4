/************************************************************************************************************
*   Author     : Akalanka Kasun                                                                                                
*   Date       : 2013-05-25                                                                                                
*   Purpose    : Employee Details View Model                                                                                                
*   Company    : Victory Information                                                                            
*   Module     : ERP System => Masters => Payroll                                                        
*                                                                                                                
************************************************************************************************************/

using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.MastersDetails
{
    class EmployeeDetailsViewModel : ViewModelBase
    {
        mas_Employee CurrentEmployee = new mas_Employee();

        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        #region Constructor
        public EmployeeDetailsViewModel(mas_Employee SelectedEmployee)
        {
            CurrentEmployee = SelectedEmployee;
            this.reafreshDepartments();
            this.reafreshDesignations();
            this.reafreshSections();
            this.reafreshGrades();
            this.reafreshDetailEmployees();
        }

        #endregion

        #region Properties

        private IEnumerable<dtl_Employee> _DtlEmployees;
        public IEnumerable<dtl_Employee> DtlEmployees
        {
            get
            {
                return this._DtlEmployees;
            }
            set
            {
                this._DtlEmployees = value;
                this.OnPropertyChanged("DtlEmployees");
                
            }
        }

        private dtl_Employee _CurrentDtlEmployee;
        public dtl_Employee CurrentDtlEmployee
        {
            get
            {
                return this._CurrentDtlEmployee;
            }
            set
            {
                this._CurrentDtlEmployee = value;
                this.OnPropertyChanged("CurrentDtlEmployee");
                
            }
        }

        private IEnumerable<z_Department> _Departments;
        public IEnumerable<z_Department> Departments
        {
            get
            {
                return this._Departments;
            }
            set
            {
                this._Departments = value;
                this.OnPropertyChanged("Departments");
            }
        }
        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get
            {
                return this._CurrentDepartment;
            }
            set
            {
                this._CurrentDepartment = value;
                this.OnPropertyChanged("CurrentDepartment");
                
            }
        }

        private IEnumerable<z_Designation> _Designation;
        public IEnumerable<z_Designation> Designation
        {
            get
            {
                return this._Designation;
            }
            set
            {
                this._Designation = value;
                this.OnPropertyChanged("Designation");
            }
        }

        private z_Designation _CurrentDesignation;
        public z_Designation CurrentDesignation
        {
            get
            {
                return this._CurrentDesignation;
            }
            set
            {
                this._CurrentDesignation = value;
                this.OnPropertyChanged("CurrentDesignation");
                
            }
        }

        private IEnumerable<z_Grade> _Grades;
        public IEnumerable<z_Grade> Grades
        {
            get
            {
                return this._Grades;
            }
            set
            {
                this._Grades = value;
                this.OnPropertyChanged("Grades");
            }
        }

        private z_Grade _CurrentGrade;
        public z_Grade CurrentGrade
        {
            get
            {
                return this._CurrentGrade;
            }
            set
            {
                this._CurrentGrade = value;
                this.OnPropertyChanged("CurrentGrade");
                
            }
        }

        private IEnumerable<z_Section> _Sections;
        public IEnumerable<z_Section> Sections
        {
            get
            {
                return this._Sections;
            }
            set
            {
                this._Sections = value;
                this.OnPropertyChanged("Sections");
            }
        }

        private z_Section _CurrentSection;
        public z_Section CurrentSection
        {
            get
            {
                return this._CurrentSection;
            }
            set
            {
                this._CurrentSection = value;
                this.OnPropertyChanged("CurrentSection");
                
            }
        } 
        #endregion

        #region New Method
        void New()
        {
            this.CurrentDtlEmployee = null;
            CurrentDtlEmployee = new dtl_Employee();
            CurrentDtlEmployee.employee_id = CurrentEmployee.employee_id;
            reafreshDetailEmployees();
        }

        #endregion

        #region New Button Class & Property
        bool newCanexecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanexecute);
            }
        } 
        #endregion

        #region Save Method
        void Save()
        {
            bool IsUpdate = false;
            dtl_Employee newDetailEmployee = new dtl_Employee();
            newDetailEmployee.employee_id = CurrentDtlEmployee.employee_id;
            newDetailEmployee.department_id = CurrentDtlEmployee.department_id;
            newDetailEmployee.designation_id = CurrentDtlEmployee.designation_id;
            newDetailEmployee.grade_id = CurrentDtlEmployee.grade_id;
            newDetailEmployee.section_id = CurrentDtlEmployee.section_id;
            newDetailEmployee.join_date = CurrentDtlEmployee.join_date;
            newDetailEmployee.isEPF_applicable = CurrentDtlEmployee.isEPF_applicable;
            newDetailEmployee.isETF_applicable = CurrentDtlEmployee.isETF_applicable;
            newDetailEmployee.isOT_applicable = CurrentDtlEmployee.isOT_applicable;
            newDetailEmployee.basic_salary = CurrentDtlEmployee.basic_salary;
            newDetailEmployee.prmernant_active_date = CurrentDtlEmployee.prmernant_active_date;

            IEnumerable<dtl_Employee> alldetailemployees = this.serviceClient.GetDetailEmployees(CurrentEmployee.employee_id);

            if (alldetailemployees != null)
            {
                foreach (dtl_Employee DetailEmployee in alldetailemployees)
                {
                    if (DetailEmployee.employee_id == CurrentDtlEmployee.employee_id)
                    {
                        IsUpdate = true;
                    }
                }
            }
            if (newDetailEmployee != null && newDetailEmployee.employee_id != null)
            {
                if (IsUpdate)
                {
                    this.serviceClient.UpdateDetailEmployee(newDetailEmployee);
                    MessageBox.Show("Record Update Successfully");
                }
                else
                {
                    this.serviceClient.SaveDetailEmployees(newDetailEmployee);
                    MessageBox.Show("Record Save Successfully");
                }
            }
            else
            {
                MessageBox.Show("Please Meantion the Basic Salary......");
            }
            reafreshDetailEmployees();
            reafreshDepartments();
            reafreshDesignations();
            reafreshGrades();
            reafreshSections();
        } 
        #endregion

        #region Save Button Class & Property
        bool saveCanExecute()
        {
            if (CurrentDtlEmployee != null)
            {
                if (CurrentDtlEmployee.isETF_applicable == null)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        } 
        #endregion

        #region User Define Methods

        private void reafreshDetailEmployees()
        {
            this.serviceClient.GetDetailEmployeesCompleted += (s, e) =>
                {
                    this.DtlEmployees = e.Result;
                    foreach (dtl_Employee item in DtlEmployees)
                    {
                        CurrentDtlEmployee = item;
                    }
                   
                };
            this.serviceClient.GetDetailEmployeesAsync(CurrentEmployee.employee_id);
        }
        private void reafreshDepartments()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
                {
                    this.Departments = e.Result;
                };
            this.serviceClient.GetDepartmentsAsync();
        }

        private void reafreshDesignations()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
                {
                    this.Designation = e.Result;
                };
            this.serviceClient.GetDesignationsAsync();
        }

        private void reafreshSections()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
                {
                    this.Sections = e.Result;
                };
            this.serviceClient.GetSectionsAsync();
        }

        private void reafreshGrades()
        {
            this.serviceClient.GetGradeCompleted += (s, e) =>
                {
                    this.Grades = e.Result;
                };
            this.serviceClient.GetGradeAsync();
        } 
        #endregion
    }
}
