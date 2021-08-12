using ERP.ERPService;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Utility
{
    public class EmployeeDataMigrationViewModel:ViewModelBase
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        List<mas_Employee> emp = new List<mas_Employee>();

        public EmployeeDataMigrationViewModel()
        {
            reafreshGrade();
            reafreshSection();
            reafreshDesignation();
            reafreshDepartment();
            reafreshCivel();
            reafreshTowns();
            reafreshCity();
            reafreshGenders();
           
        }
        private IEnumerable<z_Religen> _Religen;
        public IEnumerable<z_Religen> Religen
        {
            get { return _Religen; }
            set { _Religen = value; this.OnPropertyChanged("Religen"); }
        }

        private z_Religen _CurrentReligen;
        public z_Religen CurrentReligen
        {
            get { return _CurrentReligen; }
            set { _CurrentReligen = value; this.OnPropertyChanged("CurrentReligen"); }
        }

        #region Genders list
        private IEnumerable<z_Gender> _Genders;
        public IEnumerable<z_Gender> Genders
        {
            get { return _Genders; }
            set { _Genders = value; this.OnPropertyChanged("Genders"); }
        }
        #endregion

        #region Current Gender
        private z_Gender _CurrentGender;
        public z_Gender CurrentGender
        {
            get { return _CurrentGender; }
            set { _CurrentGender = value; this.OnPropertyChanged("CurrentGender"); }

        }
        #endregion

        #region Citys List
        private IEnumerable<z_City> _Citys;
        public IEnumerable<z_City> Citys
        {
            get { return _Citys; }
            set { _Citys = value; this.OnPropertyChanged("Citys"); }
        }
        #endregion

        #region Current City
        private z_City _CurrentCity;
        public z_City CurrentCity
        {
            get { return _CurrentCity; }
            set
            {
                _CurrentCity = value;
                this.OnPropertyChanged("CurrentCity");
               
            }
        }
        #endregion

        #region Towns List
        private IEnumerable<z_Town> _Towns;
        public IEnumerable<z_Town> Towns
        {
            get { return _Towns; }
            set { _Towns = value; this.OnPropertyChanged("Towns"); }
        }
        #endregion

        #region Current Town
        private z_Town _CurrentTown;
        public z_Town CurrentTown
        {
            get { return _CurrentTown; }
            set { _CurrentTown = value; this.OnPropertyChanged("CurrentTown"); }
        }
        #endregion

        #region Civel State List
        private IEnumerable<z_CivilState> _SivelStates;
        public IEnumerable<z_CivilState> SivelStates
        {
            get { return _SivelStates; }
            set { _SivelStates = value; this.OnPropertyChanged("SivelStates"); }
        }
        #endregion

        #region Current Sivel State
        private z_CivilState _CurrentSivelState;
        public z_CivilState CurrentSivelState
        {
            get { return _CurrentSivelState; }
            set { _CurrentSivelState = value; this.OnPropertyChanged("CurrentSivelState"); }
        }
        #endregion

        #region Departments List
        private IEnumerable<z_Department> _Departments;
        public IEnumerable<z_Department> Departments
        {
            get { return _Departments; }
            set { _Departments = value; this.OnPropertyChanged("Departments"); }
        }
        #endregion

        #region Current Department
        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set
            {
                _CurrentDepartment = value;
                this.OnPropertyChanged("CurrentDepartment");
                // reafreshSection();
            }
        }
        #endregion

        #region Designations List
        private IEnumerable<z_Designation> _Designations;
        public IEnumerable<z_Designation> Designation
        {
            get { return _Designations; }
            set { _Designations = value; this.OnPropertyChanged("Designation"); }
        }
        #endregion

        #region Current Designation
        private z_Designation _CurrentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return _CurrentDesignation; }
            set { _CurrentDesignation = value; this.OnPropertyChanged("CurrentDesignation"); }
        }
        #endregion

        #region Sections List
        private IEnumerable<z_Section> _Sections;
        public IEnumerable<z_Section> Sections
        {
            get { return _Sections; }
            set { _Sections = value; this.OnPropertyChanged("Sections"); }
        }
        #endregion

        #region Current Section
        private z_Section _CurrentSection;
        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value; this.OnPropertyChanged("CurrentSection"); }
        }
        #endregion

        #region Grades List
        private IEnumerable<z_Grade> _Grades;
        public IEnumerable<z_Grade> Grades
        {
            get { return _Grades; }
            set { _Grades = value; this.OnPropertyChanged("Grades"); }
        }
        #endregion

        #region Current Grade
        private z_Grade _CurrentGrade;
        public z_Grade CurrentGrade
        {
            get { return _CurrentGrade; }
            set { _CurrentGrade = value; this.OnPropertyChanged("CurrentGrade"); }
        }
        #endregion 

        #region Get Genders From Service
        private void reafreshGenders()
        {
            this.serviceClient.getGendersCompleted += (s, e) =>
            {
                this.Genders = e.Result;
            };
            this.serviceClient.getGendersAsync();
        }
        #endregion

        #region Get Citys From Service
        private void reafreshCity()
        {
            this.serviceClient.GetCitiesCompleted += (s, e) =>
            {
                this.Citys = e.Result.Where(c => c.isdelete == false);
            };
            this.serviceClient.GetCitiesAsync();
        }
        #endregion

        #region Get Towns From Service
        private void reafreshTowns()
        {
            this.serviceClient.GetTownDetailsCompleted += (s, e) =>
            {
                if (CurrentCity != null)
                {
                    this.Towns = e.Result.Where(z => z.city_id.Equals(CurrentCity.city_id) && z.isdelete == false);
                }
                else
                {
                    this.Towns = e.Result;
                }
            };
            this.serviceClient.GetTownDetailsAsync();
        }
        #endregion

        #region Get Civel State From Service
        private void reafreshCivel()
        {
            this.serviceClient.GetCivilStatusCompleted += (s, e) =>
            {
                this.SivelStates = e.Result;
            };
            this.serviceClient.GetCivilStatusAsync();
        }
        #endregion

        #region Get Department From Service
        private void reafreshDepartment()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Departments = e.Result.Where(c => c.isdelete == false);
            };
            this.serviceClient.GetDepartmentsAsync();
        }
        #endregion

        #region Get Designation From Service
        private void reafreshDesignation()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                this.Designation = e.Result.Where(c => c.isdelete == false);
            };
            this.serviceClient.GetDesignationsAsync();
        }
        #endregion

        #region Get Section From Service
        private void reafreshSection()
        {
            this.serviceClient.GetSectionsCompleted += (s, e) =>
            {
                if (CurrentDepartment != null)
                {
                    this.Sections = e.Result.Where(a => a.department_id.Equals(CurrentDepartment.department_id) && a.isdelete == false);
                    // this.Sections = e.Result;
                }
                else
                {
                    this.Sections = e.Result;
                }
            };
            this.serviceClient.GetSectionsAsync();
        }
        #endregion

        #region Get Grade From Service
        private void reafreshGrade()
        {
            this.serviceClient.GetGradeCompleted += (s, e) =>
            {
                this.Grades = e.Result.Where(c => c.isdelete == false);
            };
            this.serviceClient.GetGradeAsync();
        }
        #endregion 

        private string _Path;
        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged("Path"); }
        }


        public ICommand BtnBrowse
        {
            get { return new RelayCommand(btnbrowse, btnBrowseCanExecute); }
        }

        public ICommand BtnStart
        {
            get { return new RelayCommand(Start, StartCanExecute); }
        }

        private bool StartCanExecute()
        {
            return true;
        }

        private void Start()
        {
            addValues();
        }

        private bool btnBrowseCanExecute()
        {
            return true;
        }

        private void btnbrowse()
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".csv";
            fd.Filter = "Excel files (*.xlsx)|*.xlsx";

            Nullable<bool> result = fd.ShowDialog();

            if (result == true)
            {
                Path = fd.FileName;
            }

        }
        private void addValues()
        {
           
                try
                {
                    DataTable dt = ReadExcelFile();

                    foreach (DataRow dr in dt.Rows)
                    {
                        Guid up_employee_id = Guid.NewGuid();
                        mas_Employee newEmp = new mas_Employee();
                        newEmp.employee_id = up_employee_id;
                        newEmp.emp_id = dr["Employee No"].ToString();
                        newEmp.etf_no = dr["ETF NO"].ToString();
                        newEmp.epf_no = dr["ETF NO"].ToString();
                        newEmp.initials =dr["Initials"].ToString();
                        newEmp.first_name = dr["First_name"].ToString();
                        newEmp.second_name =dr["Second_name"].ToString();
                        newEmp.surname = dr["Title"].ToString();
                        newEmp.address_01 = dr["Address_01"].ToString();
                        newEmp.address_02 = dr["Address_02"].ToString();
                        newEmp.address_03 = dr["Address_03"].ToString();
                        newEmp.emg_contact = dr["Emg_contact"].ToString();
                        newEmp.image = "";
                        newEmp.town_id = Guid.Empty;
                        newEmp.city_id = Guid.Empty;
                        newEmp.gender_id = Guid.Empty;
                        newEmp.birthday = DateTime.Parse(dr["Birthday"].ToString().Trim()); 
                        newEmp.nic = dr["Nic_no"].ToString();
                        newEmp.civil_status_id = Guid.Empty;
                        newEmp.telephone = dr["Telephone"].ToString();
                        newEmp.mobile = dr["Mobile"].ToString();
                        newEmp.email = dr["Email"].ToString();
                        newEmp.religen_id = Guid.Empty;
                        newEmp.civil_status_id = Guid.Empty;
                        newEmp.isdelete = false;

                        dtl_Employee newDaatel = new dtl_Employee();
                        newDaatel.employee_id = up_employee_id;
                        newDaatel.department_id = Guid.Empty;
                        newDaatel.designation_id = Guid.Empty;
                        newDaatel.grade_id = Guid.Empty;
                        newDaatel.section_id = Guid.Empty;

                        newDaatel.join_date = DateTime.Parse(dr["Join_date"].ToString().Trim()); 
                        newDaatel.basic_salary = decimal.Parse(dr["Basic_salary"].ToString());
                        newDaatel.prmernant_active_date = DateTime.Parse(dr["Confirmation_date"].ToString().Trim());
                        newDaatel.isETF_applicable = true;
                        newDaatel.isOT_applicable = true;
                        newDaatel.isEPF_applicable = true;
                        newDaatel.isActive = true;
                        newDaatel.payment_methord_id = Guid.Empty;
                        newDaatel.branch_id = Guid.Empty;
                        newDaatel.delete_datetime = System.DateTime.Now;
                        newDaatel.delete_user_id = clsSecurity.loggedUser.user_id;
                        newDaatel.isdelete = false;
                        newDaatel.mas_Employee = newEmp;
                        newEmp.dtl_Employee = newDaatel;

                        emp.Add(newEmp);
                    }
                    if (emp.Count > 0)
                    {
                        try
                        {
                            if (serviceClient.SaveEmployeeFromExcel(emp.ToArray()))
                            {
                                MessageBox.Show("Employee Upload Sussfully");
                            }
                            else
                            {
                                MessageBox.Show("Employee Upload Fail");
                            }
                        }
                        catch (Exception exx)
                        {
                            
                             MessageBox.Show(exx.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Employee In Excel Sheet");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            
        }
        private DataTable ReadExcelFile()
        {
            string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties=Excel 12.0;";
            string query = @"Select * From [Sheet1$]";
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(connStr);

            conn.Open();
            System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(query, conn);
            System.Data.OleDb.OleDbDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            conn.Close();

            return dt;

        }      
    }
}
