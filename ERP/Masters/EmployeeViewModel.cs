using ERP.AdditionalWindows;
using ERP.Attendance.Master_Details;
using ERP.ERPService;
using ERP.Masters;
using ERP.Masters.Master_other_Details;
using ERP.Medical;
using ERP.Properties;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ERP.Base;
using CustomBusyBox;

namespace ERP
{
    public class EmployeeViewModel : ViewModelBase, IDataErrorInfo
    {
        #region Service Client

        private ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion Service Client

        #region Fields

        List<EmployeeUniversityView> ListEmployeeUniversities = new List<EmployeeUniversityView>();
        List<EmployeeExtraCurricularView> ListEmployeeExtraCurricularActivites = new List<EmployeeExtraCurricularView>();
        public List<EmployeeSocialView> ListEmployeeSocial = new List<EmployeeSocialView>();
        public List<EmployeeWorkExperienceDetailsView> ListEmployeeWorkExperienceDetails = new List<EmployeeWorkExperienceDetailsView>();
        public List<EmployeeSkillView> ListEmployeeSkillView = new List<EmployeeSkillView>();
        public List<EmployeeInterstDetailsView> ListEmployeeInterstDetailsView = new List<EmployeeInterstDetailsView>();
        public List<EmployeeAcadamicView> ListEmployeeAcadamicView = new List<EmployeeAcadamicView>();
        public List<dtl_EmployeeFamilyDetails> Listdtl_EmployeeFamilyDetails = new List<dtl_EmployeeFamilyDetails>();
        public List<EmployeeAwardsDetail> ListEmployeeAwardsDetailsView = new List<EmployeeAwardsDetail>();
        public List<EmployeeOtherOfficialDetail> ListEmployeeOtherOfficialDetailsView = new List<EmployeeOtherOfficialDetail>();
        public List<EmployeeDetailsContact> ListEmployeeDetailsContactView = new List<EmployeeDetailsContact>();
        public List<EmployeeOtherBasicdetail> ListEmployeeOtherBasicdetail = new List<EmployeeOtherBasicdetail>();
        public List<EmployeeBloodGroupandHealthView> ListEmployeeBloodGroupandHealthView = new List<EmployeeBloodGroupandHealthView>();
        public EmployeeSumarryView CurrentEmployeeHistory = new EmployeeSumarryView();
        public List<his_Employee> EmployeeHistoryList = new List<his_Employee>();
        public List<z_SchoolQualificationSubject> ListSchoolQualificationSubject = new List<z_SchoolQualificationSubject>();
        public List<z_UnivercityDigreeType> ListUnivercityDigreeType = new List<z_UnivercityDigreeType>();
        public List<z_UnivercityDigreeNames> ListUnivercityDigreeNames = new List<z_UnivercityDigreeNames>();

        decimal? empOldSalary;

        System.Configuration.Configuration config;

        #region user related data temporary lists

        List<EmployeeAwardsDetail> empAddedAwardsList = new List<EmployeeAwardsDetail>();
        #endregion

        bool isCurrentEmpSelection = false;

        #endregion

        #region Constructor

        private Task T;

        public EmployeeViewModel()
        {
            scale();
            DispatcherTimer tick = new DispatcherTimer();
            int count = 0;
            tick.Interval = new TimeSpan(0, 0, 0, 0, 300);
            tick.Start();
            tick.Tick += (s, e) => { count++; if (count == 2) { loader.Start(); tick.Stop(); } };
            loader = new DispatcherTimer();
            loader.Tick += dis_Tick;
            loader.Interval = new TimeSpan(0, 0, 0, 0, 5);

            isBusy = true;
            busyGridVisibility = Visibility.Visible;
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            T = new Task(() =>
            {

                //path = "C:\\H2SO4\\LocalEmployee\\";
                if (ConfigurationManager.AppSettings["ImagePath"].ToString() != "" && ConfigurationManager.AppSettings["PortalImagePath"].ToString() != "")
                {
                    path = ConfigurationManager.AppSettings["ImagePath"].ToString();
                    portalpath = ConfigurationManager.AppSettings["PortalImagePath"].ToString();
                }
                else
                {
                    path = "C:\\H2SO4\\LocalEmployee\\";
                    portalpath = ConfigurationManager.AppSettings["PortalImagePath"].ToString();
                }

                refreshEmployees();

                SelectionItems.Add("Employee No");
                SelectionItems.Add("First Name");
                SelectionItems.Add("Last Name");
                SelectionItems.Add("Section");
                SelectionItems.Add("Department");
                SelectionItems.Add("Designation");
                SelectionItems.Add("Grade");

                EPFList.Add("Extg.");
                EPFList.Add("New");
                EPFList.Add("Vacated");

                refresh();
                refreshExtendedDetails();
                refreshEpfStatus();
            });

        }

        #endregion Constructor

        DispatcherTimer loader;

        string path;
        string portalpath;

        private void dis_Tick(object sender, EventArgs e)
        {
            Progress++;

            if (Progress == 20)
            {
                T.Start();
                loader.Stop();
                loader.Start();
            }
            if (Progress == 50)
            {
                loader.Stop();
                loader.Start();
            }
            if (Progress == 80)
            {
                T.ContinueWith(antecedent => after(), TaskScheduler.FromCurrentSynchronizationContext());
                loader.Stop();
                loader.Start();
            }
            if (Progress == 100)
            {
                loader.Stop();
                Progress = 0;
            }
        }

        void after()
        {
            if (T.IsCompleted)
            {
                isBusy = false;
                this.New();
                busyGridVisibility = Visibility.Collapsed;
            }
        }

        bool bloodSave()
        {
            if (current_blood_type != null)
            {
                dtl_blood_type newBlood = new dtl_blood_type();
                newBlood.employee_id = CurrentEmployee.employee_id;
                newBlood.blood_id = current_blood_type.blood_id;
                newBlood.description = "";

                if (employee_blood_type.Where(c => c.employee_id == CurrentEmployee.employee_id && c.blood_id == newBlood.blood_id) != null && employee_blood_type.Where(c => c.employee_id == CurrentEmployee.employee_id && c.blood_id == newBlood.blood_id).Count() > 0)
                    if (serviceClient.bloodUpdate(newBlood))
                        return true;
                    else
                        return false;
                else
                    if (serviceClient.bloodSave(newBlood))
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        #region Properties

        private IEnumerable<his_Employee> _EffectiveDates;
        public IEnumerable<his_Employee> EffectiveDates
        {
            get { return _EffectiveDates; }
            set { _EffectiveDates = value; OnPropertyChanged("EffectiveDates"); }
        }


        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set { _progress = value; OnPropertyChanged("Progress"); }
        }

        private IEnumerable<z_blood_type> _blood_type;
        public IEnumerable<z_blood_type> blood_type
        {
            get { return _blood_type; }
            set { _blood_type = value; OnPropertyChanged("blood_type"); }
        }

        private IEnumerable<dtl_blood_type> _employee_blood_type;
        public IEnumerable<dtl_blood_type> employee_blood_type
        {
            get { return _employee_blood_type; }
            set { _employee_blood_type = value; OnPropertyChanged("employee_blood_type"); }
        }

        private z_blood_type _current_blood_type;
        public z_blood_type current_blood_type
        {
            get { return _current_blood_type; }
            set { _current_blood_type = value; OnPropertyChanged("current_blood_type"); }
        }

        private dtl_blood_type _current_employee_blood_type;
        public dtl_blood_type current_employee_blood_type
        {
            get { return _current_employee_blood_type; }
            set { _current_employee_blood_type = value; OnPropertyChanged("current_employee_blood_type"); }
        }

        private string portalImageId;
        public string PortalImageId
        {
            get { return portalImageId; }
            set { portalImageId = value; }
        }

        private bool isDelete;
        public bool IsDelete
        {
            get { return isDelete; }
            set { isDelete = value; }
        }

        private IEnumerable<EmployeeSumarryView> _AllEmployees;
        public IEnumerable<EmployeeSumarryView> AllEmployees
        {
            get { return _AllEmployees; }
            set
            {
                _AllEmployees = value;
                this.OnPropertyChanged("AllEmployees");
            }
        }

        private IEnumerable<EmployeeSumarryView> _AllEmployeesSorted;
        public IEnumerable<EmployeeSumarryView> AllEmployeesSorted
        {
            get { return _AllEmployeesSorted; }
            set
            {
                _AllEmployeesSorted = value;
                this.OnPropertyChanged("AllEmployeesSorted");
            }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                _CurrentEmployee = value;
                this.OnPropertyChanged("CurrentEmployee");

                if (_CurrentEmployee != null)
                {
                    CurrentEmployeeHistory = null;
                    CurrentEmployeeHistory = new EmployeeSumarryView();
                    CurrentEmployeeHistory.basic_salary = CurrentEmployee.basic_salary == null ? 0 : CurrentEmployee.basic_salary;
                    CurrentEmployeeHistory.city_id = CurrentEmployee.city_id;
                    CurrentEmployeeHistory.city = CurrentEmployee.city;
                    CurrentEmployeeHistory.civil_status_id = CurrentEmployee.civil_status_id;
                    CurrentEmployeeHistory.civil_status = CurrentEmployee.civil_status;
                    CurrentEmployeeHistory.department_id = CurrentEmployee.department_id;
                    CurrentEmployeeHistory.department_name = CurrentEmployee.department_name;
                    CurrentEmployeeHistory.designation_id = CurrentEmployee.designation_id;
                    CurrentEmployeeHistory.designation = CurrentEmployee.designation;
                    CurrentEmployeeHistory.grade_id = CurrentEmployee.grade_id;
                    CurrentEmployeeHistory.grade = CurrentEmployee.grade;
                    CurrentEmployeeHistory.join_date = CurrentEmployee.join_date;
                    CurrentEmployeeHistory.payment_methord_id = CurrentEmployee.payment_methord_id;
                    CurrentEmployeeHistory.payment_method = CurrentEmployee.payment_method;
                    CurrentEmployeeHistory.section_id = CurrentEmployee.section_id;
                    CurrentEmployeeHistory.section_name = CurrentEmployee.section_name;
                    CurrentEmployeeHistory.town_id = CurrentEmployee.town_id;
                    CurrentEmployeeHistory.town_name = CurrentEmployee.town_name;
                    CurrentEmployeeHistory.branch_id = CurrentEmployee.branch_id;
                    CurrentEmployeeHistory.companyBranch_Name = CurrentEmployee.companyBranch_Name;
                    CurrentEmployeeHistory.email = CurrentEmployee.email;
                    CurrentEmployeeHistory.nic = CurrentEmployee.nic;
                    CurrentEmployeeHistory.address_01 = CurrentEmployee.address_01;
                    CurrentEmployeeHistory.address_02 = CurrentEmployee.address_02;
                    CurrentEmployeeHistory.address_03 = CurrentEmployee.address_03;
                    CurrentEmployeeHistory.telephone = CurrentEmployee.telephone;
                    CurrentEmployeeHistory.first_name = CurrentEmployee.first_name;
                    CurrentEmployeeHistory.second_name = CurrentEmployee.second_name;
                    CurrentEmployeeHistory.surname = CurrentEmployee.surname;
                    CurrentEmployeeHistory.birthday = CurrentEmployee.birthday;
                    CurrentEmployeeHistory.resign_date = CurrentEmployee.resign_date;
                    CurrentEmployeeHistory.initials = CurrentEmployee.initials;
                    CurrentEmployeeHistory.title_id = CurrentEmployee.title_id;
                    CurrentEmployeeHistory.epf_no = CurrentEmployee.epf_no;
                    CurrentEmployeeHistory.etf_no = CurrentEmployee.etf_no;
                    CurrentEmployeeHistory.contract_end_date = CurrentEmployee.contract_end_date;
                    CurrentEmployeeHistory.contract_start_date = CurrentEmployee.contract_start_date;
                    CurrentEmployeeHistory.leave_end_date = CurrentEmployee.leave_end_date;

                }

                if (CurrentEmployee != null && CurrentEmployee.employee_id != Guid.Empty && CurrentEmployee.isdelete == false)
                {
                    #region Image Save
                    EmployeeImagePath = null;
                    Image = null;
                    ImagePath = null;
                    if (CurrentEmployee.image != null)
                    {
                        this.EmployeeImagePath = CurrentEmployee.image;
                        this.ImagePath = CurrentEmployee.image;
                    }
                    List<dtl_blood_type> newBlood = new List<dtl_blood_type>();// employee_blood_type.Where(c => c.employee_id == CurrentEmployee.employee_id).ToList();
                    if (newBlood != null && newBlood.Count > 0)
                    {
                        current_employee_blood_type = newBlood.FirstOrDefault();
                    }
                    #endregion

                    #region Member

                    if (EPFList != null)
                        try
                        {
                            CurrentEPFList = null;
                            dtl_member_status m1;
                            m1 = MemberStaus.Where(c => c.employee_id == CurrentEmployee.employee_id).FirstOrDefault();
                            if (m1 != null)
                            {
                                if (m1.member_status.Equals("E"))
                                    CurrentEPFList = "Extg.";
                                else if (m1.member_status.Equals("N"))
                                    CurrentEPFList = "New";
                                else if (m1.member_status.Equals("V"))
                                    CurrentEPFList = "Vacated";
                            }
                        }
                        catch (Exception)
                        {
                            CurrentEPFList = null;
                        }

                    #endregion

                    this.Name = CurrentEmployee.first_name + " " + CurrentEmployee.surname;//CurrentEmployee.initials + " " + CurrentEmployee.first_name + " " + CurrentEmployee.second_name;

                    EmployeeNo = CurrentEmployee.emp_id;
                    FirstName = CurrentEmployee.first_name;
                    Surname = CurrentEmployee.surname;
                    NIC = CurrentEmployee.nic;
                    Birthday = CurrentEmployee.birthday;
                    Address = CurrentEmployee.address_01;
                    Mobile = CurrentEmployee.telephone;
                    empOldSalary = CurrentEmployee.basic_salary;
                    FileName = null;
                    FilePath = null;
                    loadGrid();
                }

                else
                {
                    Name = null;
                    EmployeeNo = null;
                    FirstName = null;
                    Surname = null;
                    NIC = null;
                    Birthday = null;
                    Address = null;
                    Mobile = null;
                    ImagePath = null;
                }

                #region EmployeeUniversities

                if (CurrentEmployee != null && ListEmployeeUniversities != null && ListEmployeeUniversities.Count > 0)
                {
                    EmployeeUniversities = null;
                    EmployeeUniversities = ListEmployeeUniversities.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeExtraCurricular

                if (CurrentEmployee != null && ListEmployeeExtraCurricularActivites != null && ListEmployeeExtraCurricularActivites.Count > 0)
                {
                    EmployeeExtraCurricularActivites = null;
                    EmployeeExtraCurricularActivites = ListEmployeeExtraCurricularActivites.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeSocialMedia

                if (CurrentEmployee != null && ListEmployeeSocial != null && ListEmployeeSocial.Count > 0)
                {
                    EmployeeSocialView = null;
                    EmployeeSocialView = ListEmployeeSocial.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeSkillType

                if (CurrentEmployee != null && ListEmployeeSkillView != null && ListEmployeeSkillView.Count > 0)
                {
                    EmployeeSkillView = null;
                    EmployeeSkillView = ListEmployeeSkillView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

                #region Employee Interst Details

                if (CurrentEmployee != null && ListEmployeeInterstDetailsView != null && ListEmployeeInterstDetailsView.Count > 0)
                {
                    EmployeeInterstDetailsView = null;
                    EmployeeInterstDetailsView = ListEmployeeInterstDetailsView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeAcadamic Details

                if (CurrentEmployee != null && ListEmployeeAcadamicView != null && ListEmployeeAcadamicView.Count > 0)
                {
                    EmployeeAcadamicView = null;
                    EmployeeAcadamicView = ListEmployeeAcadamicView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeWorkEx

                if (CurrentEmployee != null && ListEmployeeWorkExperienceDetails != null && ListEmployeeWorkExperienceDetails.Count > 0)
                {
                    EmployeeWorkExperienceDetailsView = null;
                    EmployeeWorkExperienceDetailsView = ListEmployeeWorkExperienceDetails.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeFamilyDeatails

                if (CurrentEmployee != null && Listdtl_EmployeeFamilyDetails != null && Listdtl_EmployeeFamilyDetails.Count > 0)
                {
                    EmployeeFamilyDetails = null;
                    EmployeeFamilyDetails = Listdtl_EmployeeFamilyDetails.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeAwordsDetails

                if (CurrentEmployee != null && ListEmployeeAwardsDetailsView != null && ListEmployeeAwardsDetailsView.Count > 0)
                {
                    EmployeeAwardsDetailsView = null;
                    EmployeeAwardsDetailsView = ListEmployeeAwardsDetailsView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                }



                #endregion

                #region EmployeeBloodType

                if (CurrentEmployee != null && ListEmployeeBloodGroupandHealthView != null && ListEmployeeBloodGroupandHealthView.Count > 0)
                {
                    EmployeeBloodGroupandHealthView = null;
                    EmployeeBloodGroupandHealthView = ListEmployeeBloodGroupandHealthView.Where(c => c.employee_id == CurrentEmployee.employee_id);

                }

                #endregion

                #region EmployeeOtherOfficialDetails

                if (CurrentEmployee != null && ListEmployeeOtherOfficialDetailsView != null && ListEmployeeOtherOfficialDetailsView.Count > 0)
                {
                    CurrentEmployeeOtherOfficialDetailsView = null;
                    CurrentEmployeeOtherOfficialDetailsView = ListEmployeeOtherOfficialDetailsView.Count(c => c.employee_id == CurrentEmployee.employee_id) == 0 ? new EmployeeOtherOfficialDetail() : ListEmployeeOtherOfficialDetailsView.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeOtherContactDetails

                if (CurrentEmployee != null && ListEmployeeDetailsContactView != null && ListEmployeeDetailsContactView.Count > 0)
                {
                    CurrentEmployeeDetailsContact = null;
                    CurrentEmployeeDetailsContact = ListEmployeeDetailsContactView.Count(c => c.employee_id == CurrentEmployee.employee_id) == 0 ? new EmployeeDetailsContact() : ListEmployeeDetailsContactView.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id);
                }

                #endregion

                #region EmployeeOtherBasicDetails

                if (_CurrentEmployee != null && ListEmployeeOtherBasicdetail != null && ListEmployeeOtherBasicdetail.Count > 0)
                {
                    CurrentEmployeeOtherBasicdetail = null;
                    CurrentEmployeeOtherBasicdetail = ListEmployeeOtherBasicdetail.Count(c => c.employee_id == _CurrentEmployee.employee_id) == 0 ? new EmployeeOtherBasicdetail() : ListEmployeeOtherBasicdetail.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id);
                }
                #endregion

                #region Employee Additional Details

                if (CurrentEmployee != null && Additionaldtl != null && Additionaldtl.Count() > 0)
                {
                    CurrentAdditionaldtl = null;
                    CurrentAdditionaldtl = Additionaldtl.Count(c => c.employee_id == CurrentEmployee.employee_id) == 0 ? new dtl_employee_Additional_details() : Additionaldtl.FirstOrDefault(c => c.employee_id == CurrentEmployee.employee_id);
                }

                #endregion

            }
        }

        private String _Name;
        public String Name
        {
            get { return _Name; }
            set { _Name = value; this.OnPropertyChanged("Name"); }
        }

        private IEnumerable<z_Title> _title;
        public IEnumerable<z_Title> Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        private z_Title _currentTitle;
        public z_Title CurrentTitle
        {
            get { return _currentTitle; }
            set { _currentTitle = value; OnPropertyChanged("CurrentTitle"); }
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

        #region Cities List
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
                if (CurrentCity != null)
                {
                    Towns = null;

                    if (ListTown.Count > 0)
                    {
                        Towns = ListTown.Where(c => c.city_id == CurrentCity.city_id);
                    }
                }
                else
                {
                    Towns = null;
                }
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

        #region Current Civil Status
        private z_CivilState _CurrentCivilStatus;
        public z_CivilState CurrentCivilStatus
        {
            get { return _CurrentCivilStatus; }
            set { _CurrentCivilStatus = value; this.OnPropertyChanged("CurrentCivilStatus"); }
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

        #region Detail Employee View List
        private IEnumerable<EmployeeSumarryView> _DetailEmployees;
        public IEnumerable<EmployeeSumarryView> DetailEmployees
        {
            get { return _DetailEmployees; }
            set
            {
                _DetailEmployees = value; this.OnPropertyChanged("DetailEmployees");


            }
        }
        #endregion

        private IEnumerable<z_PaymentMethod> _paymentmethord;
        public IEnumerable<z_PaymentMethod> paymentmethord
        {
            get { return _paymentmethord; }
            set { _paymentmethord = value; this.OnPropertyChanged("paymentmethord"); }
        }

        private z_PaymentMethod _CurrentPaymetMethord;
        public z_PaymentMethod CurrentPaymetMethord
        {
            get { return _CurrentPaymetMethord; }
            set { _CurrentPaymetMethord = value; }
        }

        private IEnumerable<z_CompanyBranches> _CompanyBraches;
        public IEnumerable<z_CompanyBranches> CompanyBraches
        {
            get { return _CompanyBraches; }
            set { _CompanyBraches = value; this.OnPropertyChanged("CompanyBraches"); }
        }

        private z_CompanyBranches _CurretCompanyBeanch;
        public z_CompanyBranches CurretCompanyBeanch
        {
            get { return _CurretCompanyBeanch; }
            set { _CurretCompanyBeanch = value; this.OnPropertyChanged("CurretCompanyBeanch"); }
        }

        private IEnumerable<z_CompanyBranches> _CompanyBranch;
        public IEnumerable<z_CompanyBranches> CompanyBranch
        {
            get { return _CompanyBranch; }
            set { _CompanyBranch = value; this.OnPropertyChanged("CompanyBranch"); }
        }

        private z_CompanyBranches _CurretCompanyBranch;
        public z_CompanyBranches CurretCompanyBranch
        {
            get { return _CurretCompanyBranch; }
            set { _CurretCompanyBranch = value; this.OnPropertyChanged("CurretCompanyBranch"); }
        }


        #endregion Properties

        #region Supun Property Refresh Methods

        #region SupunChamara Refresh

        #region ExtraCurriculamActivity

        private void RefreshExtraCurriculamActivity()
        {
            serviceClient.GetExtraCurricularActivitiesCompleted += (s, e) =>
            {
                try
                {
                    ExtraCurricularActivities = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetExtraCurricularActivitiesAsync();
        }



        private IEnumerable<z_ExtraCurricularActivities> _ExtraCurricularActivities;
        public IEnumerable<z_ExtraCurricularActivities> ExtraCurricularActivities
        {
            get { return _ExtraCurricularActivities; }
            set { _ExtraCurricularActivities = value; OnPropertyChanged("ExtraCurricularActivities"); }
        }

        private z_ExtraCurricularActivities _CurrentExtraCurricularActivities;

        public z_ExtraCurricularActivities CurrentExtraCurricularActivities
        {
            get { return _CurrentExtraCurricularActivities; }
            set { _CurrentExtraCurricularActivities = value; OnPropertyChanged("CurrentExtraCurricularActivities"); }
        }

        #endregion

        #region Health type

        private IEnumerable<z_Blood_Group_Type> _BloodGroupType;
        public IEnumerable<z_Blood_Group_Type> BloodGroupType
        {
            get { return _BloodGroupType; }
            set { _BloodGroupType = value; OnPropertyChanged("BloodGroupType"); }
        }

        private z_Blood_Group_Type _CurrentBloodGroupType;
        public z_Blood_Group_Type CurrentBloodGroupType
        {
            get { return _CurrentBloodGroupType; }
            set { _CurrentBloodGroupType = value; OnPropertyChanged("CurrentBloodGroupType"); }
        }

        private void RefreshHealthType()
        {
            serviceClient.GetHealthTypeCompleted += (s, e) =>
            {
                try
                {
                    BloodGroupType = e.Result;
                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetHealthTypeAsync();
        }

        #endregion

        #region InstituteGrade

        private IEnumerable<z_UniversityGrade> _UniversityGrade;

        public IEnumerable<z_UniversityGrade> UniversityGrade
        {
            get { return _UniversityGrade; }
            set { _UniversityGrade = value; OnPropertyChanged("UniversityGrade"); }
        }

        private z_UniversityGrade _CurrnetUniversityGrade;
        public z_UniversityGrade CurrnetUniversityGrade
        {
            get { return _CurrnetUniversityGrade; }
            set { _CurrnetUniversityGrade = value; OnPropertyChanged("CurrnetUniversityGrade"); }
        }


        private void refereshInstituteGrade()
        {
            serviceClient.GetUnivercityGradeTypeCompleted += (s, e) =>
            {
                try
                {
                    UniversityGrade = e.Result;
                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetUnivercityGradeTypeAsync();
        }



        #endregion

        #region InstituteName

        private IEnumerable<z_UnivercityName> _UnivercityName;

        public IEnumerable<z_UnivercityName> UnivercityName
        {
            get { return _UnivercityName; }
            set { _UnivercityName = value; OnPropertyChanged("UnivercityName"); }
        }

        private z_UnivercityName _CurrentUnivercityName;
        public z_UnivercityName CurrentUnivercityName
        {
            get { return _CurrentUnivercityName; }
            set { _CurrentUnivercityName = value; OnPropertyChanged("CurrentUnivercityName"); if (ListUnivercityDigreeType != null && ListUnivercityDigreeType.Count() > 0 && CurrentUnivercityName != null) { UnivercityDigreeType = null; UnivercityDigreeType = ListUnivercityDigreeType.Where(c => c.univercity_id == CurrentUnivercityName.univercity_id); } }
        }

        private void RefreshUniName()
        {
            serviceClient.GetUnivercityNameCompleted += (s, e) =>
            {
                try
                {
                    UnivercityName = e.Result;
                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetUnivercityNameAsync();
        }
        #endregion

        #region InterstDetails

        private IEnumerable<z_InterestField> _InterestField;

        public IEnumerable<z_InterestField> InterestField
        {
            get { return _InterestField; }
            set { _InterestField = value; OnPropertyChanged("InterestField"); }
        }

        private z_InterestField _CurrentInterestField;

        public z_InterestField CurrentInterestField
        {
            get { return _CurrentInterestField; }
            set { _CurrentInterestField = value; OnPropertyChanged("CurrentInterestField"); }
        }


        private void RefreshInterestField()
        {
            serviceClient.GetInterestTypeCompleted += (s, e) =>
            {
                try
                {
                    InterestField = e.Result;
                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetInterestTypeAsync();
        }
        #endregion

        #region LifeInsurance

        private IEnumerable<z_Life_Insurance> _Life_Insurance;

        public IEnumerable<z_Life_Insurance> Life_Insurance
        {
            get { return _Life_Insurance; }
            set { _Life_Insurance = value; OnPropertyChanged("Life_Insurance"); }
        }

        private z_Life_Insurance _CurrentLife_Insurance;
        public z_Life_Insurance CurrentLife_Insurance
        {
            get { return _CurrentLife_Insurance; }
            set { _CurrentLife_Insurance = value; OnPropertyChanged("CurrentLife_Insurance"); }
        }


        private void RefreshLifeInsurance()
        {
            serviceClient.GetLifeInsuranceCompleted += (s, e) =>
            {
                try
                {
                    Life_Insurance = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetLifeInsuranceAsync();
        }

        #endregion

        #region ProfessionalQualification

        private IEnumerable<z_UnivercityDigreeType> _UnivercityDigreeType;
        public IEnumerable<z_UnivercityDigreeType> UnivercityDigreeType
        {
            get { return _UnivercityDigreeType; }
            set { _UnivercityDigreeType = value; OnPropertyChanged("UnivercityDigreeType"); }
        }

        private z_UnivercityDigreeType _CurrentUnivercityDigreeType;
        public z_UnivercityDigreeType CurrentUnivercityDigreeType
        {
            get { return _CurrentUnivercityDigreeType; }
            set { _CurrentUnivercityDigreeType = value; OnPropertyChanged("CurrentUnivercityDigreeType"); if (ListUnivercityDigreeNames != null && ListUnivercityDigreeNames.Count() > 0 && CurrentUnivercityDigreeType != null) { UnivercityDigreeNames = null; UnivercityDigreeNames = ListUnivercityDigreeNames.Where(c => c.univercity_Course_type_id == CurrentUnivercityDigreeType.univercity_Course_type_id); } }
        }


        private void RefreshProfessionalQualification()
        {
            serviceClient.GetUnivercityDegreeTypeCompleted += (s, e) =>
            {
                try
                {
                    ListUnivercityDigreeType.Clear();
                    UnivercityDigreeType = e.Result;
                    if (UnivercityDigreeType != null && UnivercityDigreeType.Count() > 0)
                        ListUnivercityDigreeType = UnivercityDigreeType.ToList();
                    UnivercityDigreeType = null;
                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetUnivercityDegreeTypeAsync();
        }

        #endregion

        #region QualificvationCategory

        private IEnumerable<z_UnivercityDigreeNames> _UnivercityDigreeNames;

        public IEnumerable<z_UnivercityDigreeNames> UnivercityDigreeNames
        {
            get { return _UnivercityDigreeNames; }
            set { _UnivercityDigreeNames = value; OnPropertyChanged("UnivercityDigreeNames"); }
        }

        private z_UnivercityDigreeNames _CurrentUnivercityDigreeNames;
        public z_UnivercityDigreeNames CurrentUnivercityDigreeNames
        {
            get { return _CurrentUnivercityDigreeNames; }
            set { _CurrentUnivercityDigreeNames = value; OnPropertyChanged("CurrentUnivercityDigreeNames"); }
        }

        private void RefreshQualificvationCategory()
        {
            serviceClient.GetUnivercityDegreeNameCompleted += (s, e) =>
            {
                try
                {
                    UnivercityDigreeNames = e.Result;
                    if (UnivercityDigreeNames != null && UnivercityDigreeNames.Count() > 0)
                        ListUnivercityDigreeNames = UnivercityDigreeNames.ToList();
                    UnivercityDigreeNames = null;
                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetUnivercityDegreeNameAsync();
        }
        #endregion

        #region SchoolGrade

        private IEnumerable<z_SchoolGrade> _SchoolGrade;
        public IEnumerable<z_SchoolGrade> SchoolGrade
        {
            get { return _SchoolGrade; }
            set { _SchoolGrade = value; OnPropertyChanged("SchoolGrade"); }
        }

        private z_SchoolGrade _CurrentSchoolGrade;

        public z_SchoolGrade CurrentSchoolGrade
        {
            get { return _CurrentSchoolGrade; }
            set { _CurrentSchoolGrade = value; OnPropertyChanged("CurrentSchoolGrade"); }
        }

        private void RefreshSchoolGrade()
        {
            serviceClient.GetSchoolGradesCompleted += (s, e) =>
            {
                try
                {
                    SchoolGrade = e.Result;

                }
                catch (Exception)
                {


                }
            };

            serviceClient.GetSchoolGradesAsync();
        }
        #endregion

        #region SchoolQualificationCategory

        private IEnumerable<z_SchoolQualificationType> _SchoolQualificationType;
        public IEnumerable<z_SchoolQualificationType> SchoolQualificationType
        {
            get { return _SchoolQualificationType; }
            set { _SchoolQualificationType = value; OnPropertyChanged("SchoolQualificationType"); }
        }

        private z_SchoolQualificationType _CurrentSchoolQualificationType;

        public z_SchoolQualificationType CurrentSchoolQualificationType
        {
            get { return _CurrentSchoolQualificationType; }
            set { _CurrentSchoolQualificationType = value; OnPropertyChanged("CurrentSchoolQualificationType"); if (ListSchoolQualificationSubject != null && ListSchoolQualificationSubject.Count > 0 && CurrentSchoolQualificationType != null) { SchoolQualificationSubject = null; SchoolQualificationSubject = ListSchoolQualificationSubject.Where(c => c.school_qualifiaction_id == CurrentSchoolQualificationType.school_qualifiaction_id); } }
        }

        private void RefreshSchoolQualificationCategory()
        {
            serviceClient.GetSchoolQualificationTypeCompleted += (s, e) =>
            {
                try
                {
                    SchoolQualificationType = e.Result;

                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetSchoolQualificationTypeAsync();
        }
        #endregion

        #region SchoolSubjectName

        private IEnumerable<z_SchoolQualificationSubject> _SchoolQualificationSubject;

        public IEnumerable<z_SchoolQualificationSubject> SchoolQualificationSubject
        {
            get { return _SchoolQualificationSubject; }
            set { _SchoolQualificationSubject = value; OnPropertyChanged("SchoolQualificationSubject"); }
        }

        private z_SchoolQualificationSubject _CurrentSchoolQualificationSubject;
        public z_SchoolQualificationSubject CurrentSchoolQualificationSubject
        {
            get { return _CurrentSchoolQualificationSubject; }
            set { _CurrentSchoolQualificationSubject = value; OnPropertyChanged("CurrentSchoolQualificationSubject"); }
        }

        private void RefreshSchoolSubjectName()
        {
            serviceClient.GetSchoolQualificationSubjectCompleted += (s, e) =>
            {
                try
                {
                    ListSchoolQualificationSubject.Clear();

                    SchoolQualificationSubject = e.Result;
                    if (SchoolQualificationSubject != null && SchoolQualificationSubject.Count() > 0)
                        ListSchoolQualificationSubject = SchoolQualificationSubject.ToList();
                    SchoolQualificationSubject = null;
                }
                catch (Exception)
                {


                }

            };
            serviceClient.GetSchoolQualificationSubjectAsync();
        }

        #endregion

        #region SkillType

        private IEnumerable<z_SkillType> _SkillType;

        public IEnumerable<z_SkillType> SkillType
        {
            get { return _SkillType; }
            set { _SkillType = value; OnPropertyChanged("SkillType"); }
        }

        private z_SkillType _CurrentSkillType;

        public z_SkillType CurrentSkillType
        {
            get { return _CurrentSkillType; }
            set { _CurrentSkillType = value; OnPropertyChanged("CurrentSkillType"); }
        }

        private void RefreshSkillType()
        {
            serviceClient.GetSkillTypeCompleted += (s, e) =>
            {
                try
                {
                    SkillType = e.Result;
                }
                catch (Exception)
                {


                }

            };
            serviceClient.GetSkillTypeAsync();
        }

        #endregion

        #region SocialMedia

        private IEnumerable<z_SocialMedia> _SocialMedia;

        public IEnumerable<z_SocialMedia> SocialMedia
        {
            get { return _SocialMedia; }
            set { _SocialMedia = value; OnPropertyChanged("SocialMedia"); }
        }

        private z_SocialMedia _CurrentSocialMedia;

        public z_SocialMedia CurrentSocialMedia
        {
            get { return _CurrentSocialMedia; }
            set { _CurrentSocialMedia = value; OnPropertyChanged("CurrentSocialMedia"); }
        }

        private void RefreshSocialMedia()
        {
            serviceClient.GetSocialMediaCompleted += (s, e) =>
            {
                SocialMedia = e.Result;
            };

            serviceClient.GetSocialMediaAsync();
        }

        #endregion

        #region Additional Details

        #region Race

        private IEnumerable<z_race> _Race;
        public IEnumerable<z_race> Race
        {
            get { return _Race; }
            set { _Race = value; OnPropertyChanged("Race"); }
        }

        private z_race _CurrentRace;
        public z_race CurrentRace
        {
            get { return _CurrentRace; }
            set { _CurrentRace = value; OnPropertyChanged("CurrentRace"); }
        }

        private void RefreshRace()
        {
            serviceClient.GetRaceCompleted += (s, e) =>
            {
                try
                {
                    Race = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetRaceAsync();
        }

        #endregion

        #region Nationality

        private IEnumerable<z_Nationality> _Nationality;
        public IEnumerable<z_Nationality> Nationality
        {
            get { return _Nationality; }
            set { _Nationality = value; OnPropertyChanged("Nationality"); }
        }

        private z_Nationality _CurrentNationality;
        public z_Nationality CurrentNationality
        {
            get { return _CurrentNationality; }
            set { _CurrentNationality = value; OnPropertyChanged("CurrentNationality"); }
        }


        private void RefreshNationality()
        {
            serviceClient.GetNationalityCompleted += (s, e) =>
            {
                try
                {
                    Nationality = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetNationalityAsync();
        }

        #endregion

        #region ElectorialDivision

        private IEnumerable<z_electorial_division> _ElectorialDivision;
        public IEnumerable<z_electorial_division> ElectorialDivision
        {
            get { return _ElectorialDivision; }
            set { _ElectorialDivision = value; OnPropertyChanged("ElectorialDivision"); }
        }

        private z_electorial_division _CurrentElectorialDivision;
        public z_electorial_division CurrentElectorialDivision
        {
            get { return _CurrentElectorialDivision; }
            set { _CurrentElectorialDivision = value; OnPropertyChanged("CurrentElectorialDivision"); }
        }

        private void RefreshElectorialDivision()
        {
            serviceClient.GetElectorialDivisionCompleted += (s, e) =>
            {
                try
                {
                    ElectorialDivision = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetElectorialDivisionAsync();
        }

        #endregion

        #region Election Center

        private IEnumerable<z_election_center> _ElectionCenter;
        public IEnumerable<z_election_center> ElectionCenter
        {
            get { return _ElectionCenter; }
            set { _ElectionCenter = value; OnPropertyChanged("ElectionCenter"); }
        }

        private z_election_center _CurrentElectionCenter;
        public z_election_center CurrentElectionCenter
        {
            get { return _CurrentElectionCenter; }
            set { _CurrentElectionCenter = value; OnPropertyChanged("CurrentElectionCenter"); }
        }


        private void RefreshElectionCenter()
        {
            serviceClient.GetElectionCenterCompleted += (s, e) =>
            {
                try
                {
                    ElectionCenter = e.Result;
                }
                catch (Exception)
                {


                }
            };
            serviceClient.GetElectionCenterAsync();
        }


        #endregion

        #region Grama Niladhari

        private IEnumerable<z_grama_niladhari_divition> _GramaNiladhari;
        public IEnumerable<z_grama_niladhari_divition> GramaNiladhari
        {
            get { return _GramaNiladhari; }
            set { _GramaNiladhari = value; OnPropertyChanged("GramaNiladhari"); }
        }

        private z_grama_niladhari_divition _CurrentGramaNiladhari;
        public z_grama_niladhari_divition CurrentGramaNiladhari
        {
            get { return _CurrentGramaNiladhari; }
            set { _CurrentGramaNiladhari = value; OnPropertyChanged("CurrentGramaNiladhari"); }
        }


        private void RefreshGramaNiladhari()
        {
            serviceClient.GetGramaNiladhariDivitionCompleted += (s, e) =>
            {
                try
                {
                    GramaNiladhari = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetGramaNiladhariDivitionAsync();
        }

        #endregion

        #region Nearest Police Station

        private IEnumerable<z_nearest_police_station> _NearestPoliceStation;
        public IEnumerable<z_nearest_police_station> NearestPoliceStation
        {
            get { return _NearestPoliceStation; }
            set { _NearestPoliceStation = value; OnPropertyChanged("NearestPoliceStation"); }
        }

        private z_nearest_police_station _CurrentNearestPoliceStation;
        public z_nearest_police_station CurrentNearestPoliceStation
        {
            get { return _CurrentNearestPoliceStation; }
            set { _CurrentNearestPoliceStation = value; OnPropertyChanged("CurrentNearestPoliceStation"); }
        }


        private void RefreshNearestPoliceStation()
        {
            serviceClient.GetNearestPoliceStationCompleted += (s, e) =>
            {
                try
                {
                    NearestPoliceStation = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetNearestPoliceStationAsync();
        }

        private void RefreshCostCenter()
        {
            serviceClient.GetCostCenterCompleted += (s, e) =>
            {
                try
                {
                    CostCenter = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetCostCenterAsync();
        }

        private void RefreshDivision()
        {
            serviceClient.GetDivisionCompleted += (s, e) =>
            {
                try
                {
                    Division = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetDivisionAsync();
        }

        private void RefreshSalaryScales()
        {
            serviceClient.GetSalaryScalesCompleted += (s, e) =>
            {
                try
                {
                    SalaryScales = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetSalaryScalesAsync();
        }
        #endregion

        #region dtl_additionalDetails


        private IEnumerable<dtl_employee_Additional_details> _Additionaldtl;
        public IEnumerable<dtl_employee_Additional_details> Additionaldtl
        {
            get { return _Additionaldtl; }
            set { _Additionaldtl = value; OnPropertyChanged("Additionaldtl"); }
        }


        private dtl_employee_Additional_details _CurrentAdditionaldtl;
        public dtl_employee_Additional_details CurrentAdditionaldtl
        {
            get { return _CurrentAdditionaldtl; }
            set { _CurrentAdditionaldtl = value; OnPropertyChanged("CurrentAdditionaldtl"); }
        }


        private void RefreshAdditionalDetails()
        {
            serviceClient.GetEmployeeAdditionalDetailsCompleted += (s, e) =>
            {
                try
                {
                    Additionaldtl = e.Result;
                    if (CurrentEmployee != null && Additionaldtl != null && Additionaldtl.Count() > 0)
                    {
                        CurrentAdditionaldtl = null;
                        CurrentAdditionaldtl = Additionaldtl.Count(c => c.employee_id == CurrentEmployee.employee_id) == 0 ? new dtl_employee_Additional_details() : Additionaldtl.FirstOrDefault(c => c.employee_id == CurrentEmployee.employee_id);
                    }
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEmployeeAdditionalDetailsAsync();
        }


        #endregion

        #region Cost Center
        private IEnumerable<z_CostCenter> _CostCenter;

        public IEnumerable<z_CostCenter> CostCenter
        {
            get { return _CostCenter; }
            set { _CostCenter = value; OnPropertyChanged("CostCenter"); }
        }

        private z_CostCenter _CurrentCostCenter;

        public z_CostCenter CurrentCostCenter
        {
            get { return _CurrentCostCenter; }
            set { _CurrentCostCenter = value; OnPropertyChanged("CurrentCostCenter"); }
        }

        private IEnumerable<z_Division> _Division;

        public IEnumerable<z_Division> Division
        {
            get { return _Division; }
            set { _Division = value; OnPropertyChanged("Division"); }
        }

        private z_Division _CurrentDivision;

        public z_Division CurrentDivision
        {
            get { return _CurrentDivision; }
            set { _CurrentDivision = value; OnPropertyChanged("CurrentDivision"); }
        }

        private IEnumerable<z_SalaryScales> _SalaryScales;

        public IEnumerable<z_SalaryScales> SalaryScales
        {
            get { return _SalaryScales; }
            set { _SalaryScales = value; OnPropertyChanged("SalaryScales"); }
        }

        private z_SalaryScales _CurrentSalaryScale;

        public z_SalaryScales CurrentSalaryScale
        {
            get { return _CurrentSalaryScale; }
            set { _CurrentSalaryScale = value; OnPropertyChanged("CurrentSalaryScale"); }
        }

        #endregion

        #endregion

        #region DtldataBase

        #region dtl_ProfessionalQualification
        private void RefreshdtlProfessionalQualifications()
        {
            serviceClient.Get_ProfessionalQualificationViewCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeUniversities.Clear();
                    EmployeeUniversities = e.Result;
                    if (EmployeeUniversities != null)
                    {
                        ListEmployeeUniversities = EmployeeUniversities.ToList();
                        if (_CurrentEmployee != null)
                            EmployeeUniversities = ListEmployeeUniversities.Where(c => c.employee_id == _CurrentEmployee.employee_id);

                    }

                }
                catch (Exception)
                {


                }
            };
            serviceClient.Get_ProfessionalQualificationViewAsync();
        }

        private IEnumerable<EmployeeUniversityView> _EmployeeUniversities;
        public IEnumerable<EmployeeUniversityView> EmployeeUniversities
        {
            get { return _EmployeeUniversities; }
            set { _EmployeeUniversities = value; OnPropertyChanged("EmployeeUniversities"); }
        }

        private EmployeeUniversityView _CurrentEmployeeUniversities;
        public EmployeeUniversityView CurrentEmployeeUniversities
        {
            get { return _CurrentEmployeeUniversities; }
            set { _CurrentEmployeeUniversities = value; OnPropertyChanged("CurrentEmployeeUniversities"); }
        }


        #endregion

        #region dtl_ExtraCurricularActivity

        private void RefreshExtraCurricularActivities()
        {
            serviceClient.Get_ExtraCurricularViewCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeExtraCurricularActivites.Clear();
                    EmployeeExtraCurricularActivites = e.Result;
                    if (EmployeeExtraCurricularActivites != null)
                    {
                        ListEmployeeExtraCurricularActivites = EmployeeExtraCurricularActivites.ToList();
                        if (_CurrentEmployee != null)
                            EmployeeExtraCurricularActivites = ListEmployeeExtraCurricularActivites.Where(c => c.employee_id == _CurrentEmployee.employee_id);
                    }



                }
                catch (Exception)
                {

                }
            };
            serviceClient.Get_ExtraCurricularViewAsync();
        }

        private IEnumerable<EmployeeExtraCurricularView> _EmployeeExtraCurricularActivites;
        public IEnumerable<EmployeeExtraCurricularView> EmployeeExtraCurricularActivites
        {
            get { return _EmployeeExtraCurricularActivites; }
            set { _EmployeeExtraCurricularActivites = value; OnPropertyChanged("EmployeeExtraCurricularActivites"); }
        }

        private EmployeeExtraCurricularView _CurrentEmployeeExtraCurricularActivites;
        public EmployeeExtraCurricularView CurrentEmployeeExtraCurricularActivites
        {
            get { return _CurrentEmployeeExtraCurricularActivites; }
            set { _CurrentEmployeeExtraCurricularActivites = value; OnPropertyChanged("CurrentEmployeeExtraCurricularActivites"); }
        }


        #endregion

        #region SocailMedia



        private IEnumerable<EmployeeSocialView> _EmployeeSocialView;

        public IEnumerable<EmployeeSocialView> EmployeeSocialView
        {
            get { return _EmployeeSocialView; }
            set { _EmployeeSocialView = value; OnPropertyChanged("EmployeeSocialView"); }
        }

        private EmployeeSocialView _CurrentEmployeeSocialView;

        public EmployeeSocialView CurrentEmployeeSocialView
        {
            get { return _CurrentEmployeeSocialView; }
            set { _CurrentEmployeeSocialView = value; OnPropertyChanged("CurrentEmployeeSocialView"); }
        }

        private void RefreshEmployeeSocialMediaDetails()
        {
            serviceClient.Get_SocialViewCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeSocial.Clear();
                    EmployeeSocialView = e.Result;
                    if (EmployeeSocialView != null)
                    {
                        ListEmployeeSocial = EmployeeSocialView.ToList();
                        if (_CurrentEmployee != null)
                            EmployeeSocialView = ListEmployeeSocial.Where(c => c.employee_id == _CurrentEmployee.employee_id);
                    }


                }
                catch (Exception)
                {


                }
            };
            serviceClient.Get_SocialViewAsync();
        }

        #endregion

        #region WorkExperince

        private IEnumerable<EmployeeWorkExperienceDetailsView> _EmployeeWorkExperienceDetailsView;

        public IEnumerable<EmployeeWorkExperienceDetailsView> EmployeeWorkExperienceDetailsView
        {
            get { return _EmployeeWorkExperienceDetailsView; }
            set { _EmployeeWorkExperienceDetailsView = value; OnPropertyChanged("EmployeeWorkExperienceDetailsView"); }
        }

        private EmployeeWorkExperienceDetailsView _CurrentEmployeeWorkExperienceDetailsView;

        public EmployeeWorkExperienceDetailsView CurrentEmployeeWorkExperienceDetailsView
        {
            get { return _CurrentEmployeeWorkExperienceDetailsView; }
            set { _CurrentEmployeeWorkExperienceDetailsView = value; OnPropertyChanged("CurrentEmployeeWorkExperienceDetailsView"); }
        }
        private void RefreshEmployeeWorkexperienceDetails()
        {
            serviceClient.Get_ExperienceDetailsViewCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeWorkExperienceDetails.Clear();
                    EmployeeWorkExperienceDetailsView = e.Result;
                    if (EmployeeWorkExperienceDetailsView != null)
                    {
                        ListEmployeeWorkExperienceDetails = EmployeeWorkExperienceDetailsView.ToList();
                        if (_CurrentEmployee != null)
                            EmployeeWorkExperienceDetailsView = ListEmployeeWorkExperienceDetails.Where(c => c.employee_id == _CurrentEmployee.employee_id);
                    }




                }
                catch (Exception)
                {


                }
            };
            serviceClient.Get_ExperienceDetailsViewAsync();
        }

        #endregion

        #region skillType

        private IEnumerable<EmployeeSkillView> _EmployeeSkillView;

        public IEnumerable<EmployeeSkillView> EmployeeSkillView
        {
            get { return _EmployeeSkillView; }
            set { _EmployeeSkillView = value; OnPropertyChanged("EmployeeSkillView"); }
        }

        private EmployeeSkillView _CurrentEmployeeSkillView;

        public EmployeeSkillView CurrentEmployeeSkillView
        {
            get { return _CurrentEmployeeSkillView; }
            set { _CurrentEmployeeSkillView = value; OnPropertyChanged("CurrentEmployeeSkillView"); }
        }

        private void RefreshEmployeeSkillType()
        {
            serviceClient.Get_SkillViewCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeSkillView.Clear();
                    EmployeeSkillView = e.Result;
                    if (EmployeeSkillView != null)
                    {
                        ListEmployeeSkillView = EmployeeSkillView.ToList();
                        if (_CurrentEmployee != null)
                            EmployeeSkillView = ListEmployeeSkillView.Where(c => c.employee_id == _CurrentEmployee.employee_id);
                    }



                }
                catch (Exception)
                {


                }
            };
            serviceClient.Get_SkillViewAsync();
        }


        #endregion

        #region EmployeeInterestDetails

        private IEnumerable<EmployeeInterstDetailsView> _EmployeeInterstDetailsView;

        public IEnumerable<EmployeeInterstDetailsView> EmployeeInterstDetailsView
        {
            get { return _EmployeeInterstDetailsView; }
            set { _EmployeeInterstDetailsView = value; OnPropertyChanged("EmployeeInterstDetailsView"); }
        }

        private EmployeeInterstDetailsView _CurrentEmployeeInterstDetailsView;

        public EmployeeInterstDetailsView CurrentEmployeeInterstDetailsView
        {
            get { return _CurrentEmployeeInterstDetailsView; }
            set { _CurrentEmployeeInterstDetailsView = value; OnPropertyChanged("CurrentEmployeeInterstDetailsView"); }
        }

        private void RefreshEmployeeInterestDetails()
        {
            serviceClient.Get_InterstDetailsViewCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeInterstDetailsView.Clear();
                    EmployeeInterstDetailsView = e.Result;
                    if (EmployeeInterstDetailsView != null)
                    {
                        ListEmployeeInterstDetailsView = EmployeeInterstDetailsView.ToList();
                        if (_CurrentEmployee != null)
                            EmployeeInterstDetailsView = ListEmployeeInterstDetailsView.Where(c => c.employee_id == _CurrentEmployee.employee_id);
                    }




                }
                catch (Exception)
                {


                }
            };
            serviceClient.Get_InterstDetailsViewAsync();
        }
        #endregion

        #region Acadamic Qualification

        private IEnumerable<EmployeeAcadamicView> _EmployeeAcadamicView;
        public IEnumerable<EmployeeAcadamicView> EmployeeAcadamicView
        {
            get { return _EmployeeAcadamicView; }
            set { _EmployeeAcadamicView = value; OnPropertyChanged("EmployeeAcadamicView"); }
        }

        private EmployeeAcadamicView _CurrentEmployeeAcadamicView;

        public EmployeeAcadamicView CurrentEmployeeAcadamicView
        {
            get { return _CurrentEmployeeAcadamicView; }
            set { _CurrentEmployeeAcadamicView = value; OnPropertyChanged("CurrentEmployeeAcadamicView"); }
        }

        private void RefreshEmployeeAcadamic()
        {
            serviceClient.Get_AcadamicViewCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeAcadamicView.Clear();
                    EmployeeAcadamicView = e.Result;
                    if (EmployeeAcadamicView != null)
                    {
                        if (_CurrentEmployee != null)
                        {
                            ListEmployeeAcadamicView = EmployeeAcadamicView.ToList();
                            EmployeeAcadamicView = ListEmployeeAcadamicView.Where(c => c.employee_id == _CurrentEmployee.employee_id);
                        }
                        else
                        {
                            ListEmployeeAcadamicView = EmployeeAcadamicView.ToList();
                            EmployeeAcadamicView = null;
                        }

                    }



                }
                catch (Exception)
                {


                }

            };
            serviceClient.Get_AcadamicViewAsync();
        }

        #endregion

        #region dtl_EmployeeFamilyDetails

        private IEnumerable<dtl_EmployeeFamilyDetails> _EmployeeFamilyDetails;

        public IEnumerable<dtl_EmployeeFamilyDetails> EmployeeFamilyDetails
        {
            get { return _EmployeeFamilyDetails; }
            set { _EmployeeFamilyDetails = value; OnPropertyChanged("EmployeeFamilyDetails"); }
        }

        private dtl_EmployeeFamilyDetails _CurrentEmployeeFamilyDetails;

        public dtl_EmployeeFamilyDetails CurrentEmployeeFamilyDetails
        {
            get { return _CurrentEmployeeFamilyDetails; }
            set { _CurrentEmployeeFamilyDetails = value; OnPropertyChanged("CurrentEmployeeFamilyDetails"); }
        }

        private void RefreshFamilyDetails()
        {
            serviceClient.Get_FamilyDetailsCompleted += (s, e) =>
            {
                try
                {
                    Listdtl_EmployeeFamilyDetails.Clear();
                    EmployeeFamilyDetails = e.Result;
                    if (EmployeeFamilyDetails != null)
                    {
                        if (_CurrentEmployee != null)
                        {
                            Listdtl_EmployeeFamilyDetails = EmployeeFamilyDetails.ToList();
                            EmployeeFamilyDetails = Listdtl_EmployeeFamilyDetails.Where(c => c.employee_id == _CurrentEmployee.employee_id);
                        }

                    }

                }
                catch (Exception)
                {


                }
            };
            serviceClient.Get_FamilyDetailsAsync();
        }


        #endregion

        #region dtl_EmployeeAwordsDetails

        private IEnumerable<EmployeeAwardsDetail> _EmployeeAwardsDetailsView;

        public IEnumerable<EmployeeAwardsDetail> EmployeeAwardsDetailsView
        {
            get { return _EmployeeAwardsDetailsView; }
            set { _EmployeeAwardsDetailsView = value; OnPropertyChanged("EmployeeAwardsDetailsView"); }
        }

        private EmployeeAwardsDetail _CurrentEmployeeAwardsDetailsView;

        public EmployeeAwardsDetail CurrentEmployeeAwardsDetailsView
        {
            get { return _CurrentEmployeeAwardsDetailsView; }
            set { _CurrentEmployeeAwardsDetailsView = value; OnPropertyChanged("CurrentEmployeeAwardsDetailsView"); }
        }

        private void RefreshAwardsDetails()
        {
            serviceClient.Get_AwardsDetailsViewCompleted += (s, e) =>
            {
                try
                {
                    ListEmployeeAwardsDetailsView.Clear();
                    EmployeeAwardsDetailsView = e.Result;
                    if (EmployeeAwardsDetailsView != null)
                    {
                        if (_CurrentEmployee != null)
                        {
                            ListEmployeeAwardsDetailsView = EmployeeAwardsDetailsView.ToList();
                            EmployeeAwardsDetailsView = ListEmployeeAwardsDetailsView.Where(c => c.employee_id == _CurrentEmployee.employee_id);

                        }

                    }

                }
                catch (Exception)
                {

                }
            };
            serviceClient.Get_AwardsDetailsViewAsync();

        }
        #endregion

        #region Employee Other Officaial Details

        private IEnumerable<EmployeeOtherOfficialDetail> _EmployeeOtherOfficialDetailsView;
        public IEnumerable<EmployeeOtherOfficialDetail> EmployeeOtherOfficialDetailsView
        {
            get { return _EmployeeOtherOfficialDetailsView; }
            set { _EmployeeOtherOfficialDetailsView = value; OnPropertyChanged("EmployeeOtherOfficialDetailsView"); }
        }

        private EmployeeOtherOfficialDetail _CurrentEmployeeOtherOfficialDetailsView;
        public EmployeeOtherOfficialDetail CurrentEmployeeOtherOfficialDetailsView
        {
            get { return _CurrentEmployeeOtherOfficialDetailsView; }
            set { _CurrentEmployeeOtherOfficialDetailsView = value; OnPropertyChanged("CurrentEmployeeOtherOfficialDetailsView"); }
        }

        private void RefreshOtherOfficialDetails()
        {
            //serviceClient.Get_OfficialDetailsViewCompleted += (s, e) =>
            //{
            try
            {
                ListEmployeeOtherOfficialDetailsView.Clear();
                EmployeeOtherOfficialDetailsView = serviceClient.Get_OfficialDetailsView();
                if (EmployeeOtherOfficialDetailsView != null)
                {
                    ListEmployeeOtherOfficialDetailsView = EmployeeOtherOfficialDetailsView.ToList();
                }
                EmployeeOtherOfficialDetailsView = null;
            }
            catch (Exception)
            {

            }
            //};
            //serviceClient.Get_OfficialDetailsViewAsync();

        }

        #endregion

        #region Employee Other Contact Details

        private IEnumerable<EmployeeDetailsContact> _EmployeeDetailsContact;

        public IEnumerable<EmployeeDetailsContact> EmployeeDetailsContact
        {
            get { return _EmployeeDetailsContact; }
            set { _EmployeeDetailsContact = value; OnPropertyChanged("EmployeeDetailsContact"); }
        }

        private EmployeeDetailsContact _CurrentEmployeeDetailsContact;

        public EmployeeDetailsContact CurrentEmployeeDetailsContact
        {
            get { return _CurrentEmployeeDetailsContact; }
            set { _CurrentEmployeeDetailsContact = value; OnPropertyChanged("CurrentEmployeeDetailsContact"); }
        }

        private void RefreshEmployeeDetailsContact()
        {
            //serviceClient.Get_ContactDetailsViewCompleted += (s, e) =>
            //{
            ListEmployeeDetailsContactView.Clear();
            EmployeeDetailsContact = serviceClient.Get_ContactDetailsView();
            if (EmployeeDetailsContact != null)
            {
                ListEmployeeDetailsContactView = EmployeeDetailsContact.ToList();
            }
            EmployeeDetailsContact = null;
            //};
            //serviceClient.Get_ContactDetailsViewAsync();
        }
        #endregion

        #region EmployeeBasic Details

        private IEnumerable<EmployeeOtherBasicdetail> _EmployeeOtherBasicdetail;
        public IEnumerable<EmployeeOtherBasicdetail> EmployeeOtherBasicdetail
        {
            get { return _EmployeeOtherBasicdetail; }
            set { _EmployeeOtherBasicdetail = value; OnPropertyChanged("EmployeeOtherBasicdetail"); }
        }

        private EmployeeOtherBasicdetail _CurrentEmployeeOtherBasicdetail;
        public EmployeeOtherBasicdetail CurrentEmployeeOtherBasicdetail
        {
            get { return _CurrentEmployeeOtherBasicdetail; }
            set { _CurrentEmployeeOtherBasicdetail = value; OnPropertyChanged("CurrentEmployeeOtherBasicdetail"); }
        }

        private void RefreshEmployeeBasicDetails()
        {
            serviceClient.Get_EmployeeOtherBasicdetailViewCompleted += (s, e) =>
            {
                ListEmployeeOtherBasicdetail.Clear();
                EmployeeOtherBasicdetail = e.Result;
                if (EmployeeOtherBasicdetail != null)
                {
                    ListEmployeeOtherBasicdetail = EmployeeOtherBasicdetail.ToList();
                }
                EmployeeOtherBasicdetail = null;
            };
            serviceClient.Get_EmployeeOtherBasicdetailViewAsync();
        }
        #endregion

        #region EMPLOYEEHealth

        private IEnumerable<EmployeeBloodGroupandHealthView> _EmployeeBloodGroupandHealthView;

        public IEnumerable<EmployeeBloodGroupandHealthView> EmployeeBloodGroupandHealthView
        {
            get { return _EmployeeBloodGroupandHealthView; }
            set { _EmployeeBloodGroupandHealthView = value; OnPropertyChanged("EmployeeBloodGroupandHealthView"); }
        }

        private EmployeeBloodGroupandHealthView _CurrentEmployeeBloodGroupandHealthView;

        public EmployeeBloodGroupandHealthView CurrentEmployeeBloodGroupandHealthView
        {
            get { return _CurrentEmployeeBloodGroupandHealthView; }
            set { _CurrentEmployeeBloodGroupandHealthView = value; OnPropertyChanged("CurrentEmployeeBloodGroupandHealthView"); }
        }

        private void RefreshBloodGroupandHealth()
        {
            serviceClient.Get_BloodGroupandHealthViewCompleted += (s, e) =>
            {

                ListEmployeeBloodGroupandHealthView.Clear();
                EmployeeBloodGroupandHealthView = e.Result;
                if (EmployeeBloodGroupandHealthView != null)
                {
                    ListEmployeeBloodGroupandHealthView = EmployeeBloodGroupandHealthView.ToList();
                    if (_CurrentEmployee != null)
                        EmployeeBloodGroupandHealthView = ListEmployeeBloodGroupandHealthView.Where(c => c.employee_id == _CurrentEmployee.employee_id);
                }
            };
            serviceClient.Get_BloodGroupandHealthViewAsync();
        }
        #endregion

        #endregion

        #endregion

        #region Emp Prop

        #region emp Basic Details Prop

        private string _Passportnumber;

        public string Passportnumber
        {
            get { return _Passportnumber; }
            set { _Passportnumber = value; OnPropertyChanged("Passportnumber"); }
        }

        private DateTime _PassportexpirDate;
        public DateTime PassportexpirDate
        {
            get { return _PassportexpirDate; }
            set { _PassportexpirDate = value; OnPropertyChanged("PassportexpirDate"); }
        }

        private string _DrivinglicenseNo;

        public string DrivinglicenseNo
        {
            get { return _DrivinglicenseNo; }
            set { _DrivinglicenseNo = value; OnPropertyChanged("DrivinglicenseNo"); }
        }

        #endregion

        #region emp Offical Details Prop

        private int _FingerprintDeviceID;

        public int FingerprintDeviceID
        {
            get { return _FingerprintDeviceID; }
            set { _FingerprintDeviceID = value; OnPropertyChanged("FingerprintDeviceID"); }
        }

        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; OnPropertyChanged("DisplayName"); }
        }

        private string _AccountName;

        public string AccountName
        {
            get { return _AccountName; }
            set { _AccountName = value; OnPropertyChanged("DisplayName"); }
        }


        private string _EPFandETFname;

        public string EPFandETFname
        {
            get { return _EPFandETFname; }
            set { _EPFandETFname = value; OnPropertyChanged("EPFandETFname"); }
        }



        #endregion

        #region Contact Details

        private string _PermanentAddr1;

        public string PermanentAddr1
        {
            get { return _PermanentAddr1; }
            set { _PermanentAddr1 = value; OnPropertyChanged("PermanentAddr1"); }
        }

        private string _PermanentAddr2;

        public string PermanentAddr2
        {
            get { return _PermanentAddr2; }
            set { _PermanentAddr2 = value; OnPropertyChanged("PermanentAddr2"); }
        }

        private string _PermanentAddr3;

        public string PermanentAddr3
        {
            get { return _PermanentAddr3; }
            set { _PermanentAddr3 = value; OnPropertyChanged("PermanentAddr3"); }
        }

        private string _MobilNumber1;

        public string MobilNumber1
        {
            get { return _MobilNumber1; }
            set { _MobilNumber1 = value; OnPropertyChanged("MobilNumber1"); }
        }

        private string _MobilNumber2;

        public string MobilNumber2
        {
            get { return _MobilNumber2; }
            set { _MobilNumber2 = value; OnPropertyChanged("MobilNumber2"); }
        }


        private string _PermentTPnumber;

        public string PermentTPnumber
        {
            get { return _PermentTPnumber; }
            set { _PermentTPnumber = value; OnPropertyChanged("PermentTPnumber"); }
        }


        private string _Officemobile;

        public string Officemobile
        {
            get { return _Officemobile; }
            set { _Officemobile = value; OnPropertyChanged("Officemobile"); }
        }

        private string _OfficeTPnumber;

        public string OfficeTPnumber
        {
            get { return _OfficeTPnumber; }
            set { _OfficeTPnumber = value; OnPropertyChanged("OfficeTPnumber"); }
        }


        private string _PersanlEmail;

        public string PersanlEmail
        {
            get { return _PersanlEmail; }
            set { _PersanlEmail = value; OnPropertyChanged("PersanlEmail"); }
        }

        private string _EmagencyName;

        public string EmagencyName
        {
            get { return _EmagencyName; }
            set { _EmagencyName = value; OnPropertyChanged("EmagencyName"); }
        }


        private string _EmagencyName2;

        public string EmagencyName2
        {
            get { return _EmagencyName2; }
            set { _EmagencyName2 = value; OnPropertyChanged("EmagencyName2"); }
        }


        private string _EmagencyTPnumber;

        public string EmagencyTPnumber
        {
            get { return _EmagencyTPnumber; }
            set { _EmagencyTPnumber = value; OnPropertyChanged("EmagencyTPnumber"); }
        }


        #endregion

        #region Employee Helth And Safety Details

        private string _Diagnosis;

        public string Diagnosis
        {
            get { return _Diagnosis; }
            set { _Diagnosis = value; OnPropertyChanged("Diagnosis"); }
        }

        private string _InsurancePolicyNumber;

        public string InsurancePolicyNumber
        {
            get { return _InsurancePolicyNumber; }
            set { _InsurancePolicyNumber = value; OnPropertyChanged("InsurancePolicyNumber"); }
        }

        #endregion



        #endregion

        #region Supun Commands Methods

        #region Professional Qualification

        #region new
        public ICommand NewProfessionalQualificationBtn
        {
            get { return new RelayCommand(NewProfessionalQualification); }
        }

        private void NewProfessionalQualification()
        {
            CurrentUnivercityDigreeNames = null;
            CurrentUnivercityDigreeType = null;
            CurrentUnivercityName = null;
            CurrnetUniversityGrade = null;
            CurrentEmployeeUniversities = null;
            CurrentEmployeeUniversities = new EmployeeUniversityView();
        }

        #endregion

        #region Add

        public ICommand AddProfessionalQualBtn
        {
            get { return new RelayCommand(AddProfessionalQualifications, AddProfessionalQualificationsCE); }
        }

        private bool AddProfessionalQualificationsCE()
        {
            if (CurrentEmployeeUniversities != null && CurrentUnivercityName != null && CurrentUnivercityDigreeType != null && CurrentUnivercityDigreeNames != null && CurrnetUniversityGrade != null)
                return true;
            else
                return false;
        }

        private void AddProfessionalQualifications()
        {
            try
            {
                if (_CurrentEmployee != null)
                {
                    bool isProfessionalUpdate = false;
                    EmployeeUniversityView addingEmpProfesional = new EmployeeUniversityView();

                    if (_CurrentEmployeeUniversities.professional_qualification_id == 0)
                    {
                        addingEmpProfesional.professional_qualification_id = 0;
                    }
                    else
                    {
                        addingEmpProfesional = ListEmployeeUniversities.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.professional_qualification_id == _CurrentEmployeeUniversities.professional_qualification_id);
                        isProfessionalUpdate = true;
                    }

                    if (!isProfessionalUpdate)
                    {
                        addingEmpProfesional.employee_id = CurrentEmployee.employee_id;
                        addingEmpProfesional.univercity_id = CurrentUnivercityName.univercity_id;
                        addingEmpProfesional.univercity_name = CurrentUnivercityName.univercity_name;
                        addingEmpProfesional.univercity_Course_type_id = CurrentUnivercityDigreeType.univercity_Course_type_id;
                        addingEmpProfesional.univercity_Course_type = CurrentUnivercityDigreeType.univercity_Course_type;
                        addingEmpProfesional.univercity_Course_id = CurrentUnivercityDigreeNames.univercity_Course_id;
                        addingEmpProfesional.univercity_Course_name = CurrentUnivercityDigreeNames.univercity_Course_name;
                        addingEmpProfesional.uni_grade_id = CurrnetUniversityGrade.uni_grade_id;
                        addingEmpProfesional.grade = CurrnetUniversityGrade.grade;
                        addingEmpProfesional.duration = CurrentEmployeeUniversities.duration;
                        addingEmpProfesional.gpa = CurrentEmployeeUniversities.gpa;
                        addingEmpProfesional.isActive = true;
                        addingEmpProfesional.isdelete = false;

                        ListEmployeeUniversities.Add(addingEmpProfesional);
                    }

                    EmployeeUniversities = null;
                    EmployeeUniversities = ListEmployeeUniversities.Where(c => c.employee_id == CurrentEmployee.employee_id);
                    CurrentEmployeeUniversities = null;
                    CurrentEmployeeUniversities = new EmployeeUniversityView();
                }

                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }

            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }

        }

        #endregion

        #region Delete

        public ICommand deletebtn
        {
            get { return new RelayCommand(deletePro); }
        }

        private void deletePro()
        {
            try
            {
                if (_CurrentEmployeeUniversities != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeUniversities != null && ListEmployeeUniversities.Count > 0)
                        {
                            if (CurrentEmployeeUniversities.professional_qualification_id == 0)
                            {
                                ListEmployeeUniversities.Remove(CurrentEmployeeUniversities);
                            }
                            else
                            {
                                dtl_Employee_ProfessionalQualifications DelObj = new dtl_Employee_ProfessionalQualifications();
                                DelObj.professional_qualification_id = CurrentEmployeeUniversities.professional_qualification_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Univercity(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshdtlProfessionalQualifications();
                                }
                                else
                                    clsMessages.setMessage("Record delete failed");

                            }
                        }

                        EmployeeUniversities = null;
                        EmployeeUniversities = ListEmployeeUniversities.Where(c => c.employee_id == CurrentEmployee.employee_id);
                        CurrentEmployeeUniversities = null;
                        CurrentEmployeeUniversities = new EmployeeUniversityView();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #endregion

        #region Extra Curricular

        #region New
        public ICommand NewExtracurricularActBtn
        {
            get { return new RelayCommand(NewExtraCurricularActivity); }
        }

        private void NewExtraCurricularActivity()
        {
            CurrentExtraCurricularActivities = null;
            CurrentEmployeeExtraCurricularActivites = null;
            CurrentEmployeeExtraCurricularActivites = new EmployeeExtraCurricularView();
        }

        #endregion

        #region ADD

        public ICommand AddExtracurricularActBtn
        {
            get { return new RelayCommand(AddExtracurricularActivity, AddExtracurricularActivityCE); }
        }

        private bool AddExtracurricularActivityCE()
        {
            if (CurrentEmployeeExtraCurricularActivites != null && CurrentExtraCurricularActivities != null)
                return true;
            else
                return false;
        }

        private void AddExtracurricularActivity()
        {

            try
            {
                if (_CurrentEmployee != null)
                {
                    bool isActivityUpdate = false;
                    EmployeeExtraCurricularView addingExtra = new EmployeeExtraCurricularView();
                    if (_CurrentEmployeeExtraCurricularActivites.emp_extra_curricular_id == 0)
                    {
                        addingExtra.emp_extra_curricular_id = 0;
                    }
                    else
                    {
                        addingExtra = ListEmployeeExtraCurricularActivites.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.emp_extra_curricular_id == _CurrentEmployeeExtraCurricularActivites.emp_extra_curricular_id);
                        isActivityUpdate = true;
                    }

                    if (!isActivityUpdate)
                    {
                        addingExtra.employee_id = CurrentEmployee.employee_id;
                        addingExtra.activities_category_id = CurrentExtraCurricularActivities.activities_category_id;
                        addingExtra.activities_category_type = CurrentExtraCurricularActivities.activities_category_type;
                        addingExtra.activities_category_name = CurrentEmployeeExtraCurricularActivites.activities_category_name;
                        addingExtra.isActive = true;
                        addingExtra.isdelete = false;

                        ListEmployeeExtraCurricularActivites.Add(addingExtra);
                    }

                    EmployeeExtraCurricularActivites = null;
                    EmployeeExtraCurricularActivites = ListEmployeeExtraCurricularActivites.Where(c => c.employee_id == CurrentEmployee.employee_id);
                    CurrentEmployeeExtraCurricularActivites = null;
                    CurrentEmployeeExtraCurricularActivites = new EmployeeExtraCurricularView();
                }

                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }
        }

        #endregion

        #region REMOVE

        public ICommand RemoveExtracurricularActBtn
        {
            get { return new RelayCommand(removeExtra); }
        }

        private void removeExtra()
        {
            try
            {
                if (_CurrentEmployeeExtraCurricularActivites != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeExtraCurricularActivites != null && ListEmployeeExtraCurricularActivites.Count > 0)
                        {
                            if (CurrentEmployeeExtraCurricularActivites.emp_extra_curricular_id == 0)
                            {
                                ListEmployeeExtraCurricularActivites.Remove(CurrentEmployeeExtraCurricularActivites);
                            }
                            else
                            {
                                dtl_EmployeeExtraCurricularActivities DelObj = new dtl_EmployeeExtraCurricularActivities();
                                DelObj.emp_extra_curricular_id = CurrentEmployeeExtraCurricularActivites.emp_extra_curricular_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_ExtraCurricular(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshExtraCurricularActivities();
                                }
                                else
                                    clsMessages.setMessage("Record delete failed");

                            }
                        }

                        EmployeeExtraCurricularActivites = null;
                        EmployeeExtraCurricularActivites = ListEmployeeExtraCurricularActivites.Where(c => c.employee_id == CurrentEmployee.employee_id);
                        CurrentEmployeeExtraCurricularActivites = null;
                        CurrentEmployeeExtraCurricularActivites = new EmployeeExtraCurricularView();


                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        #endregion

        #endregion

        #region Social Media

        #region ADD
        public ICommand AddSocial_media_btn
        {
            get { return new RelayCommand(saveSocialMedia, saveSocialMediaCE); }
        }

        private bool saveSocialMediaCE()
        {
            if (CurrentEmployeeSocialView != null && CurrentSocialMedia != null)
                return true;
            else
                return false;
        }

        private void saveSocialMedia()
        {
            try
            {
                if (_CurrentEmployee != null)
                {
                    bool isSocialMediaUpdate = false;
                    EmployeeSocialView addingEmpSocial = new EmployeeSocialView();
                    if (_CurrentEmployeeSocialView.emp_social_media_id == 0)
                    {
                        addingEmpSocial.emp_social_media_id = 0;
                    }
                    else
                    {
                        addingEmpSocial = ListEmployeeSocial.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.emp_social_media_id == _CurrentEmployeeSocialView.emp_social_media_id);
                        isSocialMediaUpdate = true;
                    }

                    if (!isSocialMediaUpdate)
                    {
                        addingEmpSocial.employee_id = CurrentEmployee.employee_id;
                        addingEmpSocial.social_media_id = CurrentSocialMedia.social_media_id;
                        addingEmpSocial.social_media_name = CurrentSocialMedia.social_media_name;
                        addingEmpSocial.social_media_links = CurrentEmployeeSocialView.social_media_links;
                        addingEmpSocial.isActive = true;
                        addingEmpSocial.isdelete = false;

                        ListEmployeeSocial.Add(addingEmpSocial);
                    }

                    EmployeeSocialView = null;
                    EmployeeSocialView = ListEmployeeSocial.Where(c => c.employee_id == CurrentEmployee.employee_id);

                    CurrentEmployeeSocialView = null;
                    CurrentEmployeeSocialView = new EmployeeSocialView();

                }
                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }

        }

        #endregion

        #region NEW
        public ICommand NewSocial_media_btn
        {
            get { return new RelayCommand(newSocialMedia); }
        }

        private void newSocialMedia()
        {
            CurrentSocialMedia = null;
            CurrentEmployeeSocialView = null;
            CurrentEmployeeSocialView = new EmployeeSocialView();
        }
        #endregion

        #region REMOVE

        public ICommand Remove_social_btn
        {
            get { return new RelayCommand(deleteSocial); }
        }

        private void deleteSocial()
        {
            try
            {
                if (_CurrentEmployeeSocialView != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeSocial != null && ListEmployeeSocial.Count > 0)
                        {
                            if (CurrentEmployeeSocialView.emp_social_media_id == 0)
                            {
                                ListEmployeeSocial.Remove(CurrentEmployeeSocialView);
                            }
                            else
                            {
                                dtl_EmployeeSocialMediaDetails DelObj = new dtl_EmployeeSocialMediaDetails();
                                DelObj.emp_social_media_id = CurrentEmployeeSocialView.emp_social_media_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Social(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshEmployeeSocialMediaDetails();
                                }
                                else
                                {
                                    clsMessages.setMessage("Record delete failed");
                                }
                            }
                        }

                        EmployeeSocialView = null;
                        EmployeeSocialView = ListEmployeeSocial.Where(c => c.employee_id == CurrentEmployee.employee_id);

                        CurrentEmployeeSocialView = null;
                        CurrentEmployeeSocialView = new EmployeeSocialView();


                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Skill Type

        #region Addbtn
        public ICommand AddSkillBtn
        {
            get { return new RelayCommand(SaveSkillType, SaveSkillTypeCE); }
        }

        private bool SaveSkillTypeCE()
        {
            if (CurrentEmployeeSkillView != null && CurrentSkillType != null)
                return true;
            else
                return false;
        }

        private void SaveSkillType()
        {
            try
            {
                if (_CurrentEmployee != null)
                {
                    bool isSkillUpdate = false;
                    EmployeeSkillView addingEmpSkill = new EmployeeSkillView();
                    if (_CurrentEmployeeSkillView.emp_skill_id == 0)
                    {
                        addingEmpSkill.emp_skill_id = 0;
                    }
                    else
                    {
                        addingEmpSkill = ListEmployeeSkillView.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.emp_skill_id == _CurrentEmployeeSkillView.emp_skill_id);
                        isSkillUpdate = true;
                    }

                    if (!isSkillUpdate)
                    {
                        addingEmpSkill.employee_id = CurrentEmployee.employee_id;
                        addingEmpSkill.skill_type_id = CurrentSkillType.skill_type_id;
                        addingEmpSkill.skill_type = CurrentSkillType.skill_type;
                        addingEmpSkill.isActive = true;
                        addingEmpSkill.isdelete = false;

                        ListEmployeeSkillView.Add(addingEmpSkill);
                    }

                    EmployeeSkillView = null;
                    EmployeeSkillView = ListEmployeeSkillView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                    CurrentEmployeeSkillView = null;
                    CurrentEmployeeSkillView = new EmployeeSkillView();

                }

                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }
        }

        #endregion

        #region newbtn

        public ICommand NewSkill_btn
        {
            get { return new RelayCommand(NewSkill); }
        }

        private void NewSkill()
        {
            CurrentSkillType = null;
            CurrentEmployeeSkillView = null;
            CurrentEmployeeSkillView = new EmployeeSkillView();
        }


        #endregion

        #region REMOVE

        public ICommand RemoveSkillBtn
        {
            get { return new RelayCommand(removeSklii); }
        }

        private void removeSklii()
        {
            try
            {
                if (_CurrentEmployeeSkillView != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeSkillView != null && ListEmployeeSkillView.Count > 0)
                        {
                            if (_CurrentEmployeeSkillView.emp_skill_id == 0)
                            {
                                ListEmployeeSkillView.Remove(CurrentEmployeeSkillView);
                            }
                            else
                            {
                                dtl_EmployeeSkillType DelObj = new dtl_EmployeeSkillType();
                                DelObj.emp_skill_id = CurrentEmployeeSkillView.emp_skill_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Skill(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshEmployeeSkillType();
                                }
                                else
                                    clsMessages.setMessage("Record delete failed");

                            }
                        }

                        EmployeeSkillView = null;
                        EmployeeSkillView = ListEmployeeSkillView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                        CurrentEmployeeSkillView = null;
                        CurrentEmployeeSkillView = new EmployeeSkillView();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        #endregion

        #region Interst Details

        #region Add Interst

        public ICommand AddInterst_btn
        {
            get { return new RelayCommand(SaveInterst, SaveInterstCE); }
        }

        private bool SaveInterstCE()
        {
            if (CurrentEmployeeInterstDetailsView != null && CurrentInterestField != null)
                return true;
            else
                return false;
        }

        private void SaveInterst()
        {
            try
            {
                if (CurrentEmployee != null)
                {
                    bool isUpdateInterst = false;
                    EmployeeInterstDetailsView AddInterst = new EmployeeInterstDetailsView();

                    if (_CurrentEmployeeInterstDetailsView.emp_interest_id == 0)
                    {
                        AddInterst.emp_interest_id = 0;
                    }
                    else
                    {
                        AddInterst = ListEmployeeInterstDetailsView.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.emp_interest_id == _CurrentEmployeeInterstDetailsView.emp_interest_id);
                        isUpdateInterst = true;
                    }

                    if (!isUpdateInterst)
                    {
                        AddInterst.employee_id = CurrentEmployee.employee_id;
                        AddInterst.interest_field_id = CurrentInterestField.interest_field_id;
                        AddInterst.interest_field_type = CurrentInterestField.interest_field_type;
                        AddInterst.isActive = true;
                        AddInterst.isdelete = false;

                        ListEmployeeInterstDetailsView.Add(AddInterst);
                    }

                    EmployeeInterstDetailsView = null;
                    EmployeeInterstDetailsView = ListEmployeeInterstDetailsView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                    CurrentEmployeeInterstDetailsView = null;
                    CurrentEmployeeInterstDetailsView = new EmployeeInterstDetailsView();
                }
                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }
        }

        #endregion

        #region New
        public ICommand NewInterst_btn
        {
            get
            {
                return new RelayCommand(NEWInterst);
            }
        }

        private void NEWInterst()
        {
            CurrentInterestField = null;
            CurrentEmployeeInterstDetailsView = null;
            CurrentEmployeeInterstDetailsView = new EmployeeInterstDetailsView();
        }




        #endregion

        #region REMOVE

        public ICommand RemoveBtn
        {
            get { return new RelayCommand(removeInterst); }
        }

        private void removeInterst()
        {

            try
            {
                if (_CurrentEmployeeInterstDetailsView != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeInterstDetailsView != null && ListEmployeeInterstDetailsView.Count > 0)
                        {
                            if (CurrentEmployeeInterstDetailsView.emp_interest_id == 0)
                            {
                                ListEmployeeInterstDetailsView.Remove(CurrentEmployeeInterstDetailsView);
                            }
                            else
                            {
                                dtl_EmployeeInterestDetails DelObj = new dtl_EmployeeInterestDetails();
                                DelObj.emp_interest_id = CurrentEmployeeInterstDetailsView.emp_interest_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Interst(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshEmployeeInterestDetails();
                                }
                                else
                                    clsMessages.setMessage("Record delete failed");

                            }
                        }

                        EmployeeInterstDetailsView = null;
                        EmployeeInterstDetailsView = ListEmployeeInterstDetailsView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                        CurrentEmployeeInterstDetailsView = null;
                        CurrentEmployeeInterstDetailsView = new EmployeeInterstDetailsView();


                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #endregion

        #region Acadamic Details

        #region Add

        public ICommand AddAcadamicBtn
        {
            get { return new RelayCommand(ADDAcadamic, ADDAcadamicCE); }
        }

        private bool ADDAcadamicCE()
        {
            if (CurrentEmployeeAcadamicView != null && CurrentSchoolGrade != null && CurrentSchoolQualificationType != null && CurrentSchoolQualificationSubject != null && !string.IsNullOrEmpty(CurrentEmployeeAcadamicView.school_name))
                return true;
            else
                return false;
        }

        private void ADDAcadamic()
        {
            try
            {
                if (_CurrentEmployee != null)
                {
                    bool isAcademicUpdate = false;
                    EmployeeAcadamicView addingAcademic = new EmployeeAcadamicView();
                    if (_CurrentEmployeeAcadamicView.academic_qualification_id == 0)
                    {
                        addingAcademic.academic_qualification_id = 0;
                    }
                    else
                    {
                        addingAcademic = ListEmployeeAcadamicView.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.academic_qualification_id == _CurrentEmployeeAcadamicView.academic_qualification_id);
                        isAcademicUpdate = true;
                    }

                    if (!isAcademicUpdate)
                    {
                        addingAcademic.employee_id = _CurrentEmployee.employee_id;
                        addingAcademic.school_name = _CurrentEmployeeAcadamicView.school_name;
                        addingAcademic.school_qualifiaction_type = _CurrentSchoolQualificationType.school_qualifiaction_type;
                        addingAcademic.school_grade_type = _CurrentSchoolGrade.school_grade_type;
                        addingAcademic.subject = _CurrentSchoolQualificationSubject.subject;
                        addingAcademic.school_grade_id = _CurrentSchoolGrade.school_grade_id;
                        addingAcademic.schoolsubject_id = _CurrentSchoolQualificationSubject.schoolsubject_id;
                        addingAcademic.school_qualifiaction_id = _CurrentSchoolQualificationType.school_qualifiaction_id;
                        addingAcademic.isActive = true;
                        addingAcademic.isdelete = false;

                        ListEmployeeAcadamicView.Add(addingAcademic);
                    }

                    EmployeeAcadamicView = null;
                    EmployeeAcadamicView = ListEmployeeAcadamicView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                    CurrentEmployeeAcadamicView.schoolsubject_id = null;
                    CurrentEmployeeAcadamicView.school_grade_id = null;
                    //CurrentEmployeeAcadamicView = null;
                    //CurrentEmployeeAcadamicView = new EmployeeAcadamicView();
                }
                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }
        }

        #endregion

        #region NEW

        public ICommand NewAcadamicBtn
        {
            get { return new RelayCommand(NewAcadamic); }
        }

        private void NewAcadamic()
        {
            CurrentSchoolGrade = null;
            CurrentSchoolQualificationSubject = null;
            CurrentSchoolQualificationType = null;
            CurrentEmployeeAcadamicView = null;
            CurrentEmployeeAcadamicView = new EmployeeAcadamicView();

        }

        #endregion

        #region Remove

        public ICommand RemoveAcadamicBtn
        {
            get { return new RelayCommand(deleteAcadamic); }
        }

        private void deleteAcadamic()
        {
            try
            {
                if (_CurrentEmployeeAcadamicView != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeAcadamicView != null && ListEmployeeAcadamicView.Count > 0)
                        {
                            if (CurrentEmployeeAcadamicView.academic_qualification_id == 0)
                            {
                                ListEmployeeAcadamicView.Remove(CurrentEmployeeAcadamicView);
                            }
                            else
                            {
                                dtl_EmployeeAcadamicQualification DelObj = new dtl_EmployeeAcadamicQualification();
                                DelObj.academic_qualification_id = CurrentEmployeeAcadamicView.academic_qualification_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Acadamic(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshEmployeeAcadamic();
                                }
                                else
                                    clsMessages.setMessage("Record delete failed");

                            }
                        }

                        EmployeeAcadamicView = null;
                        EmployeeAcadamicView = ListEmployeeAcadamicView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                        CurrentEmployeeAcadamicView = null;
                        CurrentEmployeeAcadamicView = new EmployeeAcadamicView();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #endregion

        #region WorkEx

        #region Add

        public ICommand WorkEx_Add_btn
        {
            get { return new RelayCommand(AddWork, AddWorkCE); }
        }

        private bool AddWorkCE()
        {
            if (CurrentEmployeeWorkExperienceDetailsView != null && !string.IsNullOrEmpty(CurrentEmployeeWorkExperienceDetailsView.previous_company_name))
                return true;
            else
                return false;
        }

        private void AddWork()
        {
            try
            {
                if (CurrentEmployee != null)
                {
                    bool isUpdateWorkExperience = false;
                    EmployeeWorkExperienceDetailsView addWork = new EmployeeWorkExperienceDetailsView();

                    if (CurrentEmployeeWorkExperienceDetailsView.work_experience_id == 0)
                    {
                        addWork.work_experience_id = 0;
                    }
                    else
                    {
                        addWork = ListEmployeeWorkExperienceDetails.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.work_experience_id == CurrentEmployeeWorkExperienceDetailsView.work_experience_id);
                        isUpdateWorkExperience = true;
                    }


                    if (!isUpdateWorkExperience)
                    {
                        addWork.employee_id = CurrentEmployee.employee_id;
                        addWork.designation = CurrentEmployeeWorkExperienceDetailsView.designation;
                        addWork.joinDate = CurrentEmployeeWorkExperienceDetailsView.joinDate;
                        addWork.previous_company_name = CurrentEmployeeWorkExperienceDetailsView.previous_company_name;
                        addWork.resign_reason = CurrentEmployeeWorkExperienceDetailsView.resign_reason;
                        addWork.resignDate = CurrentEmployeeWorkExperienceDetailsView.resignDate;
                        addWork.workDuration = CurrentEmployeeWorkExperienceDetailsView.workDuration;
                        addWork.isActive = true;
                        addWork.isdelete = false;

                        ListEmployeeWorkExperienceDetails.Add(addWork);
                    }

                    EmployeeWorkExperienceDetailsView = null;
                    EmployeeWorkExperienceDetailsView = ListEmployeeWorkExperienceDetails.Where(c => c.employee_id == CurrentEmployee.employee_id);
                    CurrentEmployeeWorkExperienceDetailsView = null;
                    CurrentEmployeeWorkExperienceDetailsView = new EmployeeWorkExperienceDetailsView();

                }
                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }
        }

        #endregion

        #region New

        public ICommand Work_New_cBtn
        {
            get { return new RelayCommand(newbtn); }
        }

        private void newbtn()
        {
            CurrentEmployeeWorkExperienceDetailsView = null;
            CurrentEmployeeWorkExperienceDetailsView = new EmployeeWorkExperienceDetailsView();
        }

        #endregion

        #region Remove

        public ICommand RemoveWorkEX_btn
        {
            get { return new RelayCommand(deleteWorkEX); }
        }

        private void deleteWorkEX()
        {
            try
            {
                if (_CurrentEmployeeWorkExperienceDetailsView != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeWorkExperienceDetails != null && ListEmployeeWorkExperienceDetails.Count > 0)
                        {
                            if (CurrentEmployeeWorkExperienceDetailsView.work_experience_id == 0)
                            {
                                ListEmployeeWorkExperienceDetails.Remove(CurrentEmployeeWorkExperienceDetailsView);
                            }
                            else
                            {
                                dtl_WorkExperienceDetails DelObj = new dtl_WorkExperienceDetails();
                                DelObj.work_experience_id = CurrentEmployeeWorkExperienceDetailsView.work_experience_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Experience(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshEmployeeWorkexperienceDetails();
                                }
                                else
                                    clsMessages.setMessage("Record delete failed");

                            }
                        }

                        EmployeeWorkExperienceDetailsView = null;
                        EmployeeWorkExperienceDetailsView = ListEmployeeWorkExperienceDetails.Where(c => c.employee_id == CurrentEmployee.employee_id);
                        CurrentEmployeeWorkExperienceDetailsView = null;
                        CurrentEmployeeWorkExperienceDetailsView = new EmployeeWorkExperienceDetailsView();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #endregion

        #region Family Details

        #region ADD
        public ICommand FamilyAddbtn
        {
            get { return new RelayCommand(ADDfamily, ADDfamilyCE); }
        }

        private bool ADDfamilyCE()
        {
            if (CurrentEmployeeFamilyDetails != null && !string.IsNullOrEmpty(CurrentEmployeeFamilyDetails.first_name))
                return true;
            else
                return false;
        }

        private void ADDfamily()
        {
            try
            {
                if (_CurrentEmployee != null)
                {
                    bool isFamilyUpdate = false;
                    dtl_EmployeeFamilyDetails addingFamily = new dtl_EmployeeFamilyDetails();
                    if (_CurrentEmployeeFamilyDetails.family_member_id == 0)
                    {
                        addingFamily.family_member_id = 0;
                    }
                    else
                    {
                        addingFamily = Listdtl_EmployeeFamilyDetails.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.family_member_id == _CurrentEmployeeFamilyDetails.family_member_id);
                        isFamilyUpdate = true;
                    }


                    if (!isFamilyUpdate)
                    {
                        addingFamily.employee_id = _CurrentEmployee.employee_id;
                        addingFamily.first_name = _CurrentEmployeeFamilyDetails.first_name;
                        addingFamily.second_name = _CurrentEmployeeFamilyDetails.second_name;
                        addingFamily.relationship = _CurrentEmployeeFamilyDetails.relationship;
                        addingFamily.is_depend = _CurrentEmployeeFamilyDetails.is_depend;
                        addingFamily.birthday = _CurrentEmployeeFamilyDetails.birthday;
                        addingFamily.work_place = _CurrentEmployeeFamilyDetails.work_place;
                        addingFamily.employee_state = _CurrentEmployeeFamilyDetails.employee_state;
                        addingFamily.isActive = true;
                        addingFamily.isdelete = false;

                        Listdtl_EmployeeFamilyDetails.Add(addingFamily);
                    }

                    EmployeeFamilyDetails = null;
                    EmployeeFamilyDetails = Listdtl_EmployeeFamilyDetails.Where(c => c.employee_id == CurrentEmployee.employee_id);
                    CurrentEmployeeFamilyDetails = null;
                    CurrentEmployeeFamilyDetails = new dtl_EmployeeFamilyDetails();
                }
                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }
        }
        #endregion

        #region New

        public ICommand EmpFamilynewbtn
        {
            get { return new RelayCommand(NEWFAMILY); }
        }

        private void NEWFAMILY()
        {
            CurrentEmployeeFamilyDetails = null;
            CurrentEmployeeFamilyDetails = new dtl_EmployeeFamilyDetails();
        }


        #endregion

        #region Remove

        public ICommand FamilyRemovebtn
        {
            get { return new RelayCommand(deleteFamily); }
        }

        private void deleteFamily()
        {
            try
            {
                if (_CurrentEmployeeFamilyDetails != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (Listdtl_EmployeeFamilyDetails != null && Listdtl_EmployeeFamilyDetails.Count > 0)
                        {
                            if (CurrentEmployeeFamilyDetails.family_member_id == 0)
                            {
                                Listdtl_EmployeeFamilyDetails.Remove(CurrentEmployeeFamilyDetails);
                            }
                            else
                            {
                                dtl_EmployeeFamilyDetails DelObj = new dtl_EmployeeFamilyDetails();
                                DelObj.family_member_id = CurrentEmployeeFamilyDetails.family_member_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Family(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshFamilyDetails();

                                }
                                else
                                    clsMessages.setMessage("Record delete failed");

                            }
                        }

                        EmployeeFamilyDetails = null;
                        EmployeeFamilyDetails = Listdtl_EmployeeFamilyDetails.Where(c => c.employee_id == CurrentEmployee.employee_id);
                        CurrentEmployeeFamilyDetails = null;
                        CurrentEmployeeFamilyDetails = new dtl_EmployeeFamilyDetails();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #endregion

        #region Awords Details

        #region NEW

        public ICommand EmpAword_btn
        {
            get { return new RelayCommand(addAword); }
        }

        private void addAword()
        {
            CurrentEmployeeAwardsDetailsView = null;
            CurrentEmployeeAwardsDetailsView = new EmployeeAwardsDetail();

        }
        #endregion

        #region ADD

        public ICommand EmpAdd_btn
        {
            get { return new RelayCommand(AddBtn, AddBtnCE); }
        }

        private bool AddBtnCE()
        {
            if (CurrentEmployeeAwardsDetailsView != null && !string.IsNullOrEmpty(_CurrentEmployeeAwardsDetailsView.award_name))
                return true;
            else
                return false;
        }

        private void AddBtn()
        {
            try
            {
                if (_CurrentEmployee != null)
                {
                    bool isUpdateAward = false;
                    EmployeeAwardsDetail addingAward = new EmployeeAwardsDetail();
                    if (_CurrentEmployeeAwardsDetailsView.award_id == 0)
                    {
                        addingAward.award_id = 0;
                    }
                    else
                    {
                        addingAward = ListEmployeeAwardsDetailsView.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.award_id == _CurrentEmployeeAwardsDetailsView.award_id);
                        addingAward.award_id = _CurrentEmployeeAwardsDetailsView.award_id;
                        isUpdateAward = true;
                    }

                    if (!isUpdateAward)
                    {
                        addingAward.employee_id = CurrentEmployee.employee_id;
                        addingAward.award_id = _CurrentEmployeeAwardsDetailsView.award_id;
                        addingAward.award_description = _CurrentEmployeeAwardsDetailsView.award_description;
                        addingAward.award_name = _CurrentEmployeeAwardsDetailsView.award_name;
                        addingAward.awards_announcement_date = _CurrentEmployeeAwardsDetailsView.awards_announcement_date;
                        addingAward.isActive = true;
                        addingAward.isdelete = false;

                        ListEmployeeAwardsDetailsView.Add(addingAward);
                    }

                    EmployeeAwardsDetailsView = null;
                    EmployeeAwardsDetailsView = ListEmployeeAwardsDetailsView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                    CurrentEmployeeAwardsDetailsView = null;
                    CurrentEmployeeAwardsDetailsView = new EmployeeAwardsDetail();

                }
                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }
        }

        #endregion

        #region Remove

        public ICommand EmpRemove_btn
        {
            get { return new RelayCommand(daleteAword); }
        }

        private void daleteAword()
        {
            try
            {
                if (_CurrentEmployeeAwardsDetailsView != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeAwardsDetailsView != null && ListEmployeeAwardsDetailsView.Count > 0)
                        {
                            if (CurrentEmployeeAwardsDetailsView.award_id == 0)
                            {
                                ListEmployeeAwardsDetailsView.Remove(CurrentEmployeeAwardsDetailsView);
                            }
                            else
                            {
                                dtl_EmployeeAwardsDetails DelObj = new dtl_EmployeeAwardsDetails();
                                DelObj.award_id = CurrentEmployeeAwardsDetailsView.award_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Awards(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshAwardsDetails();
                                }
                                else
                                    clsMessages.setMessage("Record delete failed");
                            }
                        }

                        EmployeeAwardsDetailsView = null;
                        EmployeeAwardsDetailsView = ListEmployeeAwardsDetailsView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                        CurrentEmployeeAwardsDetailsView = null;
                        CurrentEmployeeAwardsDetailsView = new EmployeeAwardsDetail();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Health Type

        #region NEW

        public ICommand NewHealthBtn
        {
            get { return new RelayCommand(newHealth); }
        }

        private void newHealth()
        {
            CurrentBloodGroupType = null;
            CurrentEmployeeBloodGroupandHealthView = null;
            CurrentEmployeeBloodGroupandHealthView = new EmployeeBloodGroupandHealthView();
        }
        #endregion

        #region ADD

        public ICommand AddHealthTypeBtn
        {
            get { return new RelayCommand(addHealthtype, addHealthtypeCE); }
        }

        private bool addHealthtypeCE()
        {
            if (CurrentEmployeeBloodGroupandHealthView != null && CurrentBloodGroupType != null)
                return true;
            else
                return false;
        }

        private void addHealthtype()
        {
            try
            {
                if (_CurrentEmployee != null)
                {
                    bool isUpdateBloodType = false;
                    EmployeeBloodGroupandHealthView addEmpBlood = new EmployeeBloodGroupandHealthView();

                    if (_CurrentEmployeeBloodGroupandHealthView.emp_blood_type_id == 0)
                    {
                        addEmpBlood.emp_blood_type_id = 0;
                    }
                    else
                    {
                        addEmpBlood = ListEmployeeBloodGroupandHealthView.FirstOrDefault(c => c.employee_id == _CurrentEmployee.employee_id && c.emp_blood_type_id == _CurrentEmployeeBloodGroupandHealthView.emp_blood_type_id);
                        isUpdateBloodType = true;
                    }

                    if (!isUpdateBloodType)
                    {

                        addEmpBlood.employee_id = CurrentEmployee.employee_id;
                        addEmpBlood.bloodGroupType_id = CurrentBloodGroupType.bloodGroupType_id;
                        addEmpBlood.bloodGroupType = CurrentBloodGroupType.bloodGroupType;
                        addEmpBlood.diagnosis = CurrentEmployeeBloodGroupandHealthView.diagnosis;
                        addEmpBlood.isActive = true;
                        addEmpBlood.isdelete = false;

                        if (ListEmployeeBloodGroupandHealthView.Where(c => c.employee_id == CurrentEmployee.employee_id).Count() > 0)
                        {
                            if (ListEmployeeBloodGroupandHealthView.FirstOrDefault(c => c.employee_id == CurrentEmployee.employee_id).bloodGroupType_id != CurrentBloodGroupType.bloodGroupType_id)
                                clsMessages.setMessage("An employee cannot have multiple blood types!");
                            else
                                ListEmployeeBloodGroupandHealthView.Add(addEmpBlood);
                        }

                        else
                            ListEmployeeBloodGroupandHealthView.Add(addEmpBlood);
                    }

                    EmployeeBloodGroupandHealthView = null;
                    EmployeeBloodGroupandHealthView = ListEmployeeBloodGroupandHealthView.Where(c => c.employee_id == CurrentEmployee.employee_id);

                    CurrentEmployeeBloodGroupandHealthView = null;
                    CurrentEmployeeBloodGroupandHealthView = new EmployeeBloodGroupandHealthView();

                }

                else
                {
                    clsMessages.setMessage("Pelase select an employee, click 'New' and try again!");
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Pelase refresh and try again!");
            }
        }

        #endregion

        #region REMOVE
        public ICommand RemoveHealthBtn
        {
            get { return new RelayCommand(deleteBlood); }
        }

        private void deleteBlood()
        {
            try
            {
                if (_CurrentEmployeeSocialView != null)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this Record?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (ListEmployeeBloodGroupandHealthView != null && ListEmployeeBloodGroupandHealthView.Count > 0)
                        {
                            if (CurrentEmployeeBloodGroupandHealthView.emp_blood_type_id == 0)
                            {
                                ListEmployeeBloodGroupandHealthView.Remove(CurrentEmployeeBloodGroupandHealthView);
                                EmployeeBloodGroupandHealthView = null;
                                EmployeeBloodGroupandHealthView = ListEmployeeBloodGroupandHealthView.Where(c => c.employee_id == CurrentEmployee.employee_id);
                            }
                            else
                            {
                                dtl_Employee_Blood_and_Health DelObj = new dtl_Employee_Blood_and_Health();
                                DelObj.emp_blood_type_id = CurrentEmployeeBloodGroupandHealthView.emp_blood_type_id;
                                DelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                                DelObj.delete_datetime = DateTime.Now;
                                DelObj.isActive = false;
                                DelObj.isdelete = true;

                                if (serviceClient.Delete_Blood(DelObj))
                                {
                                    clsMessages.setMessage("Record deleted successfully");
                                    RefreshBloodGroupandHealth();
                                }
                                else
                                    clsMessages.setMessage("Record delete failed");

                            }
                        }

                        CurrentEmployeeBloodGroupandHealthView = null;
                        CurrentEmployeeBloodGroupandHealthView = new EmployeeBloodGroupandHealthView();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        #endregion

        #endregion

        #endregion

        #region Get Data From Database Using WCF Web Service

        #region Employee Details from Service
        private void refreshEmployees()
        {
            Guid emp_id = Guid.Empty;
            if (isCurrentEmpSelection)
                emp_id = _CurrentEmployee.employee_id;
            //this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            //{
            this.AllEmployees = serviceClient.GetAllEmployeeDetail().Where(c => c.isdelete == false).OrderBy(d => d.emp_id);
            if (AllEmployees != null)
            {
                this.AllEmployeesSorted = AllEmployees;
                foreach (var x in AllEmployeesSorted) {
                    x.image = Directory.GetCurrentDirectory() + "\\" + x.image;
                }
                if (isCurrentEmpSelection)
                {
                    CurrentEmployee = this._AllEmployeesSorted.FirstOrDefault(c => c.employee_id == emp_id);
                }
                else
                    New();

            }

            //};
            //this.serviceClient.GetAllEmployeeDetailAsync(clsSecurity.loggedUser.user_id);
        }

        #endregion Employee Details from Service

        #region Get Genders From Service

        private void refreshGenders()
        {
            try
            {
                this.serviceClient.getGendersCompleted += (s, e) =>
                {
                    this.Genders = e.Result;
                };
                this.serviceClient.getGendersAsync();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Get Cities From Service

        private void refreshCity()
        {
            this.serviceClient.GetCitiesCompleted += (s, e) =>
            {
                this.Citys = e.Result;
                if (Citys != null)
                    Citys = Citys.OrderBy(c => c.city).Where(c => c.isdelete == false);

            };
            this.serviceClient.GetCitiesAsync();
        }

        #endregion

        #region Get Towns From Service

        List<z_Town> ListTown = new List<z_Town>();

        void RefershTowns()
        {
            this.serviceClient.GetTownsCompleted += (s, e) =>
            {
                Towns = e.Result;
                if (Towns != null)
                {
                    Towns = Towns.OrderBy(c => c.town_name);
                    ListTown = Towns.ToList();

                    Towns = null;

                    if (CurrentCity != null)
                    {
                        Towns = ListTown.Where(c => c.city_id == CurrentCity.city_id);
                    }
                }
            };
            this.serviceClient.GetTownsAsync();
        }

        #endregion

        #region Get Civil Status From Service

        private void refreshCivil()
        {
            this.serviceClient.GetCivilStatusCompleted += (s, e) =>
            {
                this.SivelStates = e.Result;
            };
            this.serviceClient.GetCivilStatusAsync();
        }

        #endregion

        #region Get Department From Service

        private void refreshDepartment()
        {
            this.serviceClient.GetDepartmentsCompleted += (s, e) =>
            {
                this.Departments = e.Result.Where(c => c.isdelete == false);
                if (Departments != null && Departments.Count() > 0)
                    Departments = Departments.OrderBy(c => c.department_name);
            };
            this.serviceClient.GetDepartmentsAsync();
        }

        #endregion

        #region Get Designation From Service

        private void refreshDesignation()
        {
            this.serviceClient.GetDesignationsCompleted += (s, e) =>
            {
                this.Designation = e.Result.Where(c => c.isdelete == false);
                if (Designation != null && Designation.Count() > 0)
                    Designation = Designation.OrderBy(c => c.designation);
            };
            this.serviceClient.GetDesignationsAsync();
        }

        #endregion

        #region Get Section From Service

        private void refreshSection()
        {
            try
            {
                //List<z_Section> tempSections = this.serviceClient.GetSections().ToList();

                //if (CurrentDepartment != null)
                //{
                //    this.Sections = tempSections.Where(a => a.department_id.Equals(CurrentDepartment.department_id) && a.isdelete == false);
                //    // this.Sections = e.Result;
                //}
                //else
                //{
                //    this.Sections = tempSections;
                //}
                this.serviceClient.GetSectionsCompleted += (s, e) =>
                {
                    if (CurrentDepartment != null)
                    {
                        this.Sections = e.Result.Where(a => a.department_id.Equals(CurrentDepartment.department_id) && a.isdelete == false);
                        if (Sections != null && Sections.Count() > 0)
                            Sections = Sections.OrderBy(c => c.section_name);
                    }
                    else
                    {
                        this.Sections = e.Result.Where(c => c.isdelete == false);
                        if (Sections != null && Sections.Count() > 0)
                            Sections = Sections.OrderBy(c => c.section_name);
                    }
                };
                this.serviceClient.GetSectionsAsync();
            }
            catch (Exception)
            {
            }

        }

        #endregion

        #region Get Grade From Service
        private void refreshGrade()
        {
            try
            {
                this.serviceClient.GetGradeCompleted += (s, e) =>
                {
                    this.Grades = e.Result.Where(c => c.isdelete == false);
                    if (Grades != null && Grades.Count() > 0)
                        Grades = Grades.OrderBy(c => c.grade);
                };
                this.serviceClient.GetGradeAsync();
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Titles from Service
        private void refreshTitle()
        {

            this.serviceClient.GetTitleCompleted += (s, e) =>
            {
                this.Title = e.Result;
            };
            this.serviceClient.GetTitleAsync();

        }
        #endregion Titles from Service

        #region Religions from Service
        private void refreshReligion()
        {
            this.serviceClient.GetReligenCompleted += (s, e) =>
            {
                this.Religen = e.Result.Where(c => c.isdelete == false);
            };
            this.serviceClient.GetReligenAsync();
        }
        #endregion Religions from Service

        #region Company Branches from Service
        private void refreshCompanyBranch()
        {
            this.serviceClient.GetCompanyBranchesCompleted += (s, e) =>
            {
                this.CompanyBranch = e.Result.Where(z => z.isdelete == false);
            };
            this.serviceClient.GetCompanyBranchesAsync();
        }
        #endregion Company Branches from Service

        #region Payment Methods from Service
        private void refreshPaymentMethod()
        {
            this.serviceClient.GetPaymentMethodsCompleted += (s, e) =>
            {
                this.paymentmethord = e.Result;
            };
            this.serviceClient.GetPaymentMethodsAsync();
        }
        #endregion Payment Methods from Service

        #region Blood
        void refreshBlood()
        {
            serviceClient.GetBloodTypesCompleted += (s, e) =>
            {
                blood_type = e.Result;
            };
            serviceClient.GetBloodTypesAsync();
        }

        void refreshBloodDetail()
        {
            serviceClient.GetBloodCompleted += (s, e) =>
            {
                employee_blood_type = e.Result;
            };
            serviceClient.GetBloodAsync();
        }
        #endregion

        #region Epf Status from Service
        void refreshEpfStatus()
        {
            serviceClient.GetMemberStatusCompleted += (s, e) =>
            {
                MemberStaus = e.Result;
            };
            serviceClient.GetMemberStatusAsync();
        }
        #endregion Epf Status from Service


        void refresh()
        {
            refreshSection();
            refreshGenders();
            refreshTitle();
            refreshCity();
            refreshDepartment();
            refreshDesignation();
            refreshGrade();
            refreshCivil();
            RefershTowns();
            refreshReligion();
            refreshPaymentMethod();
            refreshCompanyBranch();
            refreshBlood();
            refreshBloodDetail();
            RefreshRace();
            RefreshNationality();
            RefreshElectorialDivision();
            RefreshElectionCenter();
            RefreshGramaNiladhari();
            RefreshNearestPoliceStation();
            RefreshCostCenter();
            RefreshDivision();
            RefreshSalaryScales();
        }

        void refreshExtendedDetails()
        {
            RefreshExtraCurriculamActivity();
            RefreshHealthType();
            refereshInstituteGrade();
            RefreshUniName();
            RefreshInterestField();
            RefreshLifeInsurance();
            RefreshProfessionalQualification();
            RefreshQualificvationCategory();
            RefreshSchoolGrade();
            RefreshSchoolQualificationCategory();
            RefreshSchoolSubjectName();
            RefreshSkillType();
            RefreshSocialMedia();
            RefreshdtlProfessionalQualifications();
            RefreshExtraCurricularActivities();
            RefreshEmployeeSocialMediaDetails();
            RefreshEmployeeWorkexperienceDetails();
            RefreshEmployeeSkillType();
            RefreshEmployeeInterestDetails();
            RefreshEmployeeAcadamic();
            RefreshFamilyDetails();
            RefreshOtherOfficialDetails();
            RefreshAwardsDetails();
            RefreshEmployeeDetailsContact();
            RefreshEmployeeBasicDetails();
            RefreshBloodGroupandHealth();
            RefreshAdditionalDetails();
        }

        #endregion

        #region Button Command

        #region New Button Command
        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }
        #endregion

        #region New Button Can Execute
        private bool newCanExecute()
        {
            return true;

        }
        #endregion

        #region Save Button Command
        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }

        #endregion

        #region Save Button Can Execute
        private bool saveCanExecute()
        {

            if (CurrentEmployee != null)
            {
                if (this.CurrentEmployee.first_name == null || this.CurrentEmployee.first_name.ToString() == String.Empty)
                    return false;
                if (this.CurrentEmployee.emp_id == null)
                    return false;
                if (this.CurrentEmployee.employee_id == null)
                    return false;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region sync Button Command
        public ICommand PortalImageSync
        {
            get
            {
                return new RelayCommand(sync, syncCanExecute);
            }
        }
        #endregion

        #region Sync Button Can Exicute

        private bool syncCanExecute()
        {
            return true;
        }

        #endregion

        #region Delete Button Command
        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(delete, deleteCanExecute);
            }
        }


        #endregion

        #region Delete Button Can Exicute

        private bool deleteCanExecute()
        {
            if (CurrentEmployee != null)
            {
                if (this.CurrentEmployee.employee_id != null)
                {
                    return this.CurrentEmployee.employee_id != null;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region User Defined Methods

        #region New Method

        public void New()
        {

            Name = null;
            ImagePath = null;
            CurrentEmployee = null;
            CurrentEmployeeUniversities = null;

            CurrentEmployeeExtraCurricularActivites = null;

            CurrentEmployeeSocialView = null;

            CurrentEmployeeWorkExperienceDetailsView = null;

            CurrentEmployeeSkillView = null;

            CurrentEmployeeInterstDetailsView = null;

            CurrentEmployeeAcadamicView = null;

            CurrentEmployeeFamilyDetails = null;

            CurrentEmployeeOtherOfficialDetailsView = null;

            CurrentEmployeeAwardsDetailsView = null;

            CurrentEmployeeDetailsContact = null;

            CurrentEmployeeOtherBasicdetail = null;

            CurrentEmployeeBloodGroupandHealthView = null;

            CurrentAdditionaldtl = null;


            CurrentEmployee = new EmployeeSumarryView();
            CurrentEmployee.employee_id = Guid.NewGuid();
            CurrentEmployee.isActive = true;
            CurrentEmployee.isdelete = false;
            CurrentEmployee.isExecutive = false;
            CurrentEmployeeUniversities = new EmployeeUniversityView();
            CurrentEmployeeExtraCurricularActivites = new EmployeeExtraCurricularView();
            CurrentEmployeeSocialView = new EmployeeSocialView();
            CurrentEmployeeWorkExperienceDetailsView = new EmployeeWorkExperienceDetailsView();
            CurrentEmployeeSkillView = new EmployeeSkillView();
            CurrentEmployeeInterstDetailsView = new EmployeeInterstDetailsView();
            CurrentEmployeeAcadamicView = new EmployeeAcadamicView();
            CurrentEmployeeFamilyDetails = new dtl_EmployeeFamilyDetails();
            CurrentEmployeeOtherOfficialDetailsView = new EmployeeOtherOfficialDetail();
            CurrentEmployeeAwardsDetailsView = new EmployeeAwardsDetail();
            CurrentEmployeeDetailsContact = new EmployeeDetailsContact();
            CurrentEmployeeOtherBasicdetail = new EmployeeOtherBasicdetail();
            CurrentEmployeeBloodGroupandHealthView = new EmployeeBloodGroupandHealthView();
            CurrentAdditionaldtl = new dtl_employee_Additional_details();

            CurrentEmployee.city_id = Guid.Empty;
            CurrentEmployee.town_id = Guid.Empty;
            CurrentEmployee.department_id = Guid.Empty;
            CurrentEmployee.designation_id = Guid.Empty;
            CurrentEmployee.section_id = Guid.Empty;
            CurrentEmployee.grade_id = Guid.Empty;
            CurrentEmployee.gender_id = Guid.Empty;
            CurrentEmployee.civil_status_id = Guid.Empty;
            CurrentEmployee.branch_id = Guid.Empty;
            CurrentEmployee.payment_methord_id = Guid.Empty;
            CurrentEmployee.religen_id = Guid.Empty;

            if (config.AppSettings.Settings["idAutoGenOn"].Value == "true")
            {
                int nextNumber = Convert.ToInt32(serviceClient.GetNewEmployee_emp_id()) + 1;
                EmployeeNo = nextNumber.ToString();
            }
        }

        #endregion

        #region Save Method

        public void Save()
        {

            if (IsDelete != true)
            {
                if (Image != null)
                {
                    if (CurrentEmployee.portalImage != null)
                    {
                        GoogleDriveModel.GetInstance.delete(CurrentEmployee.portalImage);
                    }
                    SaveImage();
                    if (CurrentEmployee != null)
                    {
                        PortalImageId = GoogleDriveModel.GetInstance.upload(path + CurrentEmployee.employee_id + ".Jpeg", CurrentEmployee.employee_id.ToString(), GoogleDriveModel.GetInstance.getFolderId());
                    }
                }
            }

            bool isUpdate = false;
            bool isNewSalary = false;
            decimal oldsalary = 0;
            DateTime? ContractStartDate = null;
            DateTime? ContractEndDate = null;

            mas_Employee newEmp = new mas_Employee();
            newEmp.employee_id = CurrentEmployee.employee_id;
            newEmp.emp_id = CurrentEmployee.emp_id;
            newEmp.etf_no = CurrentEmployee.etf_no;
            newEmp.epf_no = CurrentEmployee.epf_no;
            newEmp.initials = CurrentEmployee.initials;
            newEmp.first_name = CurrentEmployee.first_name;
            newEmp.second_name = CurrentEmployee.second_name;
            newEmp.surname = CurrentEmployee.surname;
            newEmp.address_01 = CurrentEmployee.address_01;
            newEmp.address_02 = CurrentEmployee.address_02;
            newEmp.address_03 = CurrentEmployee.address_03;
            newEmp.emg_contact = CurrentEmployee.emg_contact;
            newEmp.title_id = CurrentEmployee.title_id;
            newEmp.office_email = CurrentEmployee.office_email;
            newEmp.office_mobile = CurrentEmployee.office_mobile;

            newEmp.image = path + CurrentEmployee.employee_id + ".Jpeg";
            newEmp.image = EmployeeImagePath;
            newEmp.portalImage = PortalImageId;
            newEmp.town_id = CurrentEmployee.town_id == null ? Guid.Empty : CurrentEmployee.town_id;
            newEmp.city_id = CurrentEmployee.city_id == null ? Guid.Empty : CurrentEmployee.city_id;
            newEmp.gender_id = CurrentEmployee.gender_id == null ? Guid.Empty : CurrentEmployee.gender_id;
            newEmp.birthday = CurrentEmployee.birthday;
            newEmp.nic = CurrentEmployee.nic;
            newEmp.civil_status_id = CurrentEmployee.civil_status_id == null ? Guid.Empty : CurrentEmployee.civil_status_id;
            newEmp.telephone = CurrentEmployee.telephone;
            newEmp.mobile = CurrentEmployee.mobile;
            newEmp.email = CurrentEmployee.email;
            newEmp.religen_id = CurrentEmployee.religen_id;
            newEmp.isdelete = CurrentEmployee.isdelete;

            if (CurrentEmployee.contract_start_date != null)
                ContractStartDate = CurrentEmployee.contract_start_date;

            if (CurrentEmployee.contract_end_date != null)
                ContractEndDate = CurrentEmployee.contract_end_date;

            dtl_Employee newDaatel = new dtl_Employee();
            newDaatel.employee_id = CurrentEmployee.employee_id;
            newDaatel.department_id = CurrentEmployee.department_id == null ? Guid.Empty : (Guid)CurrentEmployee.department_id;
            newDaatel.designation_id = CurrentEmployee.designation_id == null ? Guid.Empty : (Guid)CurrentEmployee.designation_id;
            newDaatel.grade_id = CurrentEmployee.grade_id == null ? Guid.Empty : CurrentEmployee.grade_id;
            newDaatel.section_id = CurrentEmployee.section_id == null ? Guid.Empty : CurrentEmployee.section_id;
            newDaatel.join_date = CurrentEmployee.join_date;
            newDaatel.basic_salary = CurrentEmployee.basic_salary == null ? 0 : CurrentEmployee.basic_salary;
            newDaatel.prmernant_active_date = CurrentEmployee.prmernant_active_date;
            newDaatel.isETF_applicable = CurrentEmployee.isETF_applicable == null ? false : CurrentEmployee.isETF_applicable;
            newDaatel.isOT_applicable = CurrentEmployee.isOT_applicable == null ? false : CurrentEmployee.isOT_applicable;
            newDaatel.isEPF_applicable = CurrentEmployee.isEPF_applicable == null ? false : CurrentEmployee.isEPF_applicable;
            newDaatel.payment_methord_id = CurrentEmployee.payment_methord_id == null ? Guid.Empty : CurrentEmployee.payment_methord_id;
            newDaatel.branch_id = CurrentEmployee.branch_id == null ? Guid.Empty : CurrentEmployee.branch_id;
            newDaatel.resign_date = CurrentEmployee.resign_date;
            newDaatel.auto_ot = CurrentEmployee.auto_ot == null ? false : CurrentEmployee.auto_ot;
            newDaatel.save_datetime = System.DateTime.Now;
            newDaatel.save_user_id = clsSecurity.loggedUser.user_id;
            newDaatel.isActive = CurrentEmployee.isActive;
            newDaatel.isdelete = CurrentEmployee.isdelete;
            newDaatel.isExecutive = CurrentEmployee.isExecutive;
            newDaatel.leave_end_date = CurrentEmployee.leave_end_date;
            //newDaatel.cost_center_id = CurrentCostCenter.cost_center_id;
            //newDaatel.division_id = CurrentDivision.division_id;
            // newDaatel.tea_amount = CurrentEmployee.tea_amount;
            // newDaatel.dust_amount = CurrentEmployee.dust_amount;
            newDaatel.salary_scale_id = CurrentEmployee.salary_scale_id;
            newDaatel.mas_Employee = newEmp;
            newEmp.dtl_Employee = newDaatel;


            int addingEmpStatus = serviceClient.IsEmployeeExists(newEmp);

            if (addingEmpStatus == 1)
            {
                // updating existing employee
                isUpdate = true;
            }

            bool isAct = true;
            if (newDaatel != null)
            {
                #region EmpUni

                List<dtl_Employee_ProfessionalQualifications> SaveUniList = new List<dtl_Employee_ProfessionalQualifications>();

                if (ListEmployeeUniversities != null && ListEmployeeUniversities.Count > 0)
                {
                    foreach (var item in ListEmployeeUniversities.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_Employee_ProfessionalQualifications saveobj = new dtl_Employee_ProfessionalQualifications();

                        try
                        {
                            saveobj.professional_qualification_id = item.professional_qualification_id;
                            saveobj.employee_id = item.employee_id;
                            saveobj.univercity_Course_id = (int)item.univercity_Course_id;
                            saveobj.gradeclass_id = (int)item.uni_grade_id;
                            saveobj.gpa = item.gpa;
                            saveobj.duration = item.duration;
                            saveobj.isActive = CurrentEmployee.isActive;
                            saveobj.isdelete = CurrentEmployee.isdelete;
                            saveobj.save_user_id = clsSecurity.loggedUser.user_id;
                            saveobj.save_datetime = DateTime.Now;
                            saveobj.modified_datetime = DateTime.Now;
                            saveobj.modified_user_id = clsSecurity.loggedUser.user_id;

                            SaveUniList.Add(saveobj);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (SaveUniList != null && SaveUniList.Count > 0)
                        newEmp.dtl_Employee_ProfessionalQualifications = SaveUniList.ToArray();

                }

                #endregion

                #region EmpExtraCurricular

                List<dtl_EmployeeExtraCurricularActivities> SaveExtraCurricularList = new List<dtl_EmployeeExtraCurricularActivities>();

                if (ListEmployeeExtraCurricularActivites != null && ListEmployeeExtraCurricularActivites.Count > 0)
                {
                    foreach (var item in ListEmployeeExtraCurricularActivites.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_EmployeeExtraCurricularActivities saveObj = new dtl_EmployeeExtraCurricularActivities();

                        try
                        {
                            saveObj.emp_extra_curricular_id = item.emp_extra_curricular_id;
                            saveObj.activities_category_id = (int)item.activities_category_id;
                            saveObj.activities_category_name = item.activities_category_name;
                            saveObj.employee_id = item.employee_id;
                            saveObj.isActive = CurrentEmployee.isActive;
                            saveObj.isdelete = CurrentEmployee.isdelete;
                            saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            saveObj.save_datetime = DateTime.Now;
                            saveObj.modified_datetime = DateTime.Now;
                            saveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                            SaveExtraCurricularList.Add(saveObj);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (SaveExtraCurricularList != null && SaveExtraCurricularList.Count > 0)
                        newEmp.dtl_EmployeeExtraCurricularActivities = SaveExtraCurricularList.ToArray();
                }

                #endregion

                #region EmpSocialMedia

                List<dtl_EmployeeSocialMediaDetails> saveSocialMediaDetails = new List<dtl_EmployeeSocialMediaDetails>();
                if (ListEmployeeSocial != null && ListEmployeeSocial.Count > 0)
                {
                    foreach (var item in ListEmployeeSocial.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_EmployeeSocialMediaDetails saveObj = new dtl_EmployeeSocialMediaDetails();

                        try
                        {
                            saveObj.emp_social_media_id = item.emp_social_media_id;
                            saveObj.social_media_id = (int)item.social_media_id;
                            saveObj.social_media_links = item.social_media_links;
                            saveObj.employee_id = item.employee_id;
                            saveObj.isActive = CurrentEmployee.isActive;
                            saveObj.isdelete = CurrentEmployee.isdelete;
                            saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            saveObj.save_datetime = DateTime.Now;
                            saveObj.modified_datetime = DateTime.Now;
                            saveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                            saveSocialMediaDetails.Add(saveObj);
                        }
                        catch (Exception)
                        {

                        }

                    }

                    if (saveSocialMediaDetails != null && saveSocialMediaDetails.Count > 0)
                    {
                        newEmp.dtl_EmployeeSocialMediaDetails = saveSocialMediaDetails.ToArray();
                    }
                }


                #endregion

                #region WorkExperianceDetails

                List<dtl_WorkExperienceDetails> savedtl_EmployeeWorkexperienceDetails = new List<dtl_WorkExperienceDetails>();

                if (ListEmployeeWorkExperienceDetails != null && ListEmployeeWorkExperienceDetails.Count > 0)
                {
                    foreach (var item in ListEmployeeWorkExperienceDetails.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_WorkExperienceDetails SaveObj = new dtl_WorkExperienceDetails();

                        try
                        {
                            SaveObj.employee_id = item.employee_id;
                            SaveObj.work_experience_id = item.work_experience_id;
                            SaveObj.joinDate = item.joinDate;
                            SaveObj.resignDate = item.resignDate;
                            SaveObj.designation = item.designation;
                            SaveObj.workDuration = item.workDuration;
                            SaveObj.resign_reason = item.resign_reason;
                            SaveObj.previous_company_name = item.previous_company_name;
                            SaveObj.isActive = CurrentEmployee.isActive;
                            SaveObj.isdelete = CurrentEmployee.isdelete;
                            SaveObj.save_datetime = DateTime.Now;
                            SaveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            SaveObj.modified_datetime = DateTime.Now;
                            SaveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                            savedtl_EmployeeWorkexperienceDetails.Add(SaveObj);
                        }
                        catch (Exception)
                        {

                        }

                    }

                    if (savedtl_EmployeeWorkexperienceDetails != null && savedtl_EmployeeWorkexperienceDetails.Count > 0)
                    {
                        newEmp.dtl_WorkExperienceDetails = savedtl_EmployeeWorkexperienceDetails.ToArray();
                    }
                }

                #endregion

                #region EmpSkill

                List<dtl_EmployeeSkillType> savedtl_EmployeeSkillType = new List<dtl_EmployeeSkillType>();
                if (ListEmployeeSkillView != null && ListEmployeeSkillView.Count > 0)
                {
                    foreach (var item in ListEmployeeSkillView.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_EmployeeSkillType SaveObj = new dtl_EmployeeSkillType();

                        try
                        {
                            SaveObj.employee_id = item.employee_id;
                            SaveObj.emp_skill_id = item.emp_skill_id;
                            SaveObj.skill_type_id = (int)item.skill_type_id;
                            SaveObj.description = item.skill_type;
                            SaveObj.isActive = CurrentEmployee.isActive;
                            SaveObj.isdelete = CurrentEmployee.isdelete;
                            SaveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            SaveObj.save_datetime = DateTime.Now;
                            SaveObj.modified_user_id = clsSecurity.loggedUser.user_id;
                            SaveObj.modified_datetime = DateTime.Now;

                            savedtl_EmployeeSkillType.Add(SaveObj);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (savedtl_EmployeeSkillType != null && savedtl_EmployeeSkillType.Count > 0)
                    {
                        newEmp.dtl_EmployeeSkillType = savedtl_EmployeeSkillType.ToArray();
                    }

                }

                #endregion

                #region InterstDetails

                List<dtl_EmployeeInterestDetails> savedtl_EmployeeInterestDetails = new List<dtl_EmployeeInterestDetails>();
                if (ListEmployeeInterstDetailsView != null && ListEmployeeInterstDetailsView.Count > 0)
                {
                    foreach (var item in ListEmployeeInterstDetailsView.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_EmployeeInterestDetails SaveObj = new dtl_EmployeeInterestDetails();

                        try
                        {
                            SaveObj.employee_id = item.employee_id;
                            SaveObj.emp_interest_id = item.emp_interest_id;
                            SaveObj.interest_field_id = (int)item.interest_field_id;
                            SaveObj.description = item.description;
                            SaveObj.isActive = CurrentEmployee.isActive;
                            SaveObj.isdelete = CurrentEmployee.isdelete;
                            SaveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            SaveObj.save_datetime = DateTime.Now;
                            SaveObj.modified_user_id = clsSecurity.loggedUser.user_id;
                            SaveObj.modified_datetime = DateTime.Now;

                            savedtl_EmployeeInterestDetails.Add(SaveObj);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    if (savedtl_EmployeeInterestDetails != null && savedtl_EmployeeInterestDetails.Count > 0)
                    {
                        newEmp.dtl_EmployeeInterestDetails = savedtl_EmployeeInterestDetails.ToArray();
                    }
                }

                #endregion

                #region AcadamicDetails

                List<dtl_EmployeeAcadamicQualification> savedtl_EmployeeAcadamicQualification = new List<dtl_EmployeeAcadamicQualification>();

                if (ListEmployeeAcadamicView != null && ListEmployeeAcadamicView.Count > 0)
                {
                    foreach (var item in ListEmployeeAcadamicView.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_EmployeeAcadamicQualification saveObj = new dtl_EmployeeAcadamicQualification();

                        try
                        {
                            saveObj.employee_id = item.employee_id;
                            saveObj.academic_qualification_id = item.academic_qualification_id;
                            //saveObj.attempt = item.attempt;
                            saveObj.school_grade_id = (int)item.school_grade_id;
                            saveObj.school_name = item.school_name;
                            saveObj.schoolsubject_id = (int)item.schoolsubject_id;
                            //saveObj.year = item.year;
                            saveObj.isActive = CurrentEmployee.isActive;
                            saveObj.isdelete = CurrentEmployee.isdelete;
                            saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            saveObj.save_datetime = DateTime.Now;
                            saveObj.modified_user_id = clsSecurity.loggedUser.user_id;
                            saveObj.modified_datetime = DateTime.Now;

                            savedtl_EmployeeAcadamicQualification.Add(saveObj);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (savedtl_EmployeeAcadamicQualification != null && savedtl_EmployeeAcadamicQualification.Count > 0)
                    {
                        newEmp.dtl_EmployeeAcadamicQualification = savedtl_EmployeeAcadamicQualification.ToArray();
                    }
                }
                #endregion

                #region EmpFamilyDetails

                List<dtl_EmployeeFamilyDetails> savedtl_EmployeeFamilyDetails = new List<dtl_EmployeeFamilyDetails>();
                if (Listdtl_EmployeeFamilyDetails != null && Listdtl_EmployeeFamilyDetails.Count > 0)
                {
                    foreach (var item in Listdtl_EmployeeFamilyDetails.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_EmployeeFamilyDetails saveObj = new dtl_EmployeeFamilyDetails();

                        try
                        {
                            saveObj.family_member_id = item.family_member_id;
                            saveObj.employee_id = item.employee_id;
                            saveObj.first_name = item.first_name;
                            saveObj.second_name = item.second_name;
                            saveObj.birthday = item.birthday;
                            saveObj.employee_state = item.employee_state;
                            saveObj.is_depend = item.is_depend;
                            saveObj.work_place = item.work_place;
                            saveObj.relationship = item.relationship;
                            saveObj.isActive = CurrentEmployee.isActive;
                            saveObj.isdelete = CurrentEmployee.isdelete;
                            saveObj.save_datetime = DateTime.Now;
                            saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            saveObj.modified_datetime = DateTime.Now;
                            saveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                            savedtl_EmployeeFamilyDetails.Add(saveObj);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    if (savedtl_EmployeeFamilyDetails != null && savedtl_EmployeeFamilyDetails.Count > 0)
                    {
                        newEmp.dtl_EmployeeFamilyDetails = savedtl_EmployeeFamilyDetails.ToArray();
                    }
                }

                #endregion

                #region EmpAwordsDetails

                List<dtl_EmployeeAwardsDetails> saveEmployeeAwardsDetails = new List<dtl_EmployeeAwardsDetails>();
                if (ListEmployeeAwardsDetailsView != null && ListEmployeeAwardsDetailsView.Count > 0)
                {
                    foreach (var item in ListEmployeeAwardsDetailsView.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_EmployeeAwardsDetails saveObj = new dtl_EmployeeAwardsDetails();

                        try
                        {
                            saveObj.employee_id = item.employee_id;
                            saveObj.award_id = item.award_id;
                            saveObj.award_name = item.award_name;
                            saveObj.award_description = item.award_description;
                            saveObj.awards_announcement_date = item.awards_announcement_date;
                            saveObj.isActive = CurrentEmployee.isActive;
                            saveObj.isdelete = CurrentEmployee.isdelete;
                            saveObj.save_datetime = DateTime.Now;
                            saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            saveObj.modified_datetime = DateTime.Now;
                            saveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                            saveEmployeeAwardsDetails.Add(saveObj);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    if (saveEmployeeAwardsDetails != null && saveEmployeeAwardsDetails.Count > 0)
                    {
                        newEmp.dtl_EmployeeAwardsDetails = saveEmployeeAwardsDetails.ToArray();
                    }
                }

                #endregion

                #region EmployeeBloodType

                List<dtl_Employee_Blood_and_Health> saveEmployeeBloodandHealth = new List<dtl_Employee_Blood_and_Health>();
                if (ListEmployeeBloodGroupandHealthView != null && ListEmployeeBloodGroupandHealthView.Count > 0)
                {
                    foreach (var item in ListEmployeeBloodGroupandHealthView.Where(c => c.employee_id == CurrentEmployee.employee_id))
                    {
                        dtl_Employee_Blood_and_Health saveObj = new dtl_Employee_Blood_and_Health();

                        try
                        {
                            saveObj.employee_id = item.employee_id;
                            saveObj.emp_blood_type_id = item.emp_blood_type_id;
                            saveObj.bloodGroupType_id = (int)item.bloodGroupType_id;
                            saveObj.bloodGroupType = item.bloodGroupType;
                            saveObj.diagnosis = item.diagnosis;
                            saveObj.isActive = CurrentEmployee.isActive;
                            saveObj.isdelete = CurrentEmployee.isdelete;
                            saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            saveObj.save_datetime = DateTime.Now;
                            saveObj.modified_datetime = DateTime.Now;
                            saveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                            saveEmployeeBloodandHealth.Add(saveObj);

                        }
                        catch (Exception)
                        {

                        }
                    }

                    if (saveEmployeeBloodandHealth != null && saveEmployeeBloodandHealth.Count > 0)
                    {
                        newEmp.dtl_Employee_Blood_and_Health = saveEmployeeBloodandHealth.ToArray();
                    }

                }


                #endregion

                #region EmpOtherDetails


                if (_CurrentEmployee != null && _CurrentEmployeeOtherOfficialDetailsView != null)
                {
                    dtl_EmployeeOtherOfficialDetails saveObj = new dtl_EmployeeOtherOfficialDetails();

                    try
                    {
                        saveObj.employee_id = CurrentEmployee.employee_id;
                        saveObj.account_name = _CurrentEmployeeOtherOfficialDetailsView.account_name;
                        saveObj.display_name = _CurrentEmployeeOtherOfficialDetailsView.display_name;
                        saveObj.fingerprint_device_ID = _CurrentEmployeeOtherOfficialDetailsView.fingerprint_device_ID;
                        saveObj.etf_name = _CurrentEmployeeOtherOfficialDetailsView.etf_name;
                        saveObj.epf_name = _CurrentEmployeeOtherOfficialDetailsView.etf_name;
                        saveObj.isActive = CurrentEmployee.isActive;
                        saveObj.isdelete = CurrentEmployee.isdelete;
                        saveObj.save_datetime = DateTime.Now;
                        saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                        saveObj.modified_datetime = DateTime.Now;
                        saveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                        newEmp.dtl_EmployeeOtherOfficialDetails = saveObj;
                    }
                    catch (Exception)
                    {

                    }

                }


                #endregion

                #region EmpOtherContactDetails

                if (_CurrentEmployee != null && _CurrentEmployeeDetailsContact != null)
                {

                    dtl_Employee_ContactDetails saveObj = new dtl_Employee_ContactDetails();

                    try
                    {
                        saveObj.employee_id = CurrentEmployee.employee_id;
                        saveObj.permant_addr_line1 = _CurrentEmployeeDetailsContact.permant_addr_line1;
                        saveObj.permant_addr_line2 = _CurrentEmployeeDetailsContact.permant_addr_line2;
                        saveObj.permant_addr_line3 = _CurrentEmployeeDetailsContact.permant_addr_line3;
                        saveObj.mobile_number1 = _CurrentEmployeeDetailsContact.mobile_number1;
                        saveObj.mobile_number2 = _CurrentEmployeeDetailsContact.mobile_number2;
                        saveObj.perment_addr_tp = _CurrentEmployeeDetailsContact.perment_addr_tp;
                        saveObj.office_mobile = _CurrentEmployeeDetailsContact.office_mobile;
                        saveObj.office_tp = _CurrentEmployeeDetailsContact.office_tp;
                        saveObj.persanal_email = _CurrentEmployeeDetailsContact.persanal_email;
                        saveObj.emg_contact_name1 = _CurrentEmployeeDetailsContact.emg_contact_name1;
                        saveObj.emg_contact_number1 = _CurrentEmployeeDetailsContact.emg_contact_number1;
                        saveObj.emg_contact_name2 = _CurrentEmployeeDetailsContact.emg_contact_name2;
                        saveObj.emg_contact_number2 = _CurrentEmployeeDetailsContact.emg_contact_number2;
                        saveObj.office_email = _CurrentEmployeeDetailsContact.office_email;
                        saveObj.isActive = CurrentEmployee.isActive;
                        saveObj.isdelete = CurrentEmployee.isdelete;
                        saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                        saveObj.save_datetime = DateTime.Now;
                        saveObj.modified_datetime = DateTime.Now;
                        saveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                        newEmp.dtl_Employee_ContactDetails = saveObj;
                    }
                    catch (Exception)
                    {

                    }

                }


                #endregion

                #region EmpOtherBasicDetails

                if (ListEmployeeOtherBasicdetail != null)
                {
                    if (_CurrentEmployee != null && _CurrentEmployeeOtherBasicdetail != null)
                    {
                        dtl_EmployeeOtherBasicDetails saveObj = new dtl_EmployeeOtherBasicDetails();

                        try
                        {
                            saveObj.employee_id = CurrentEmployee.employee_id;
                            saveObj.driving_license_no = _CurrentEmployeeOtherBasicdetail.driving_license_no;
                            saveObj.license_expiry_date = _CurrentEmployeeOtherBasicdetail.license_expiry_date;
                            saveObj.Nationality = _CurrentEmployeeOtherBasicdetail.Nationality;
                            saveObj.passport_expiry_date = _CurrentEmployeeOtherBasicdetail.passport_expiry_date;
                            saveObj.passport_no = _CurrentEmployeeOtherBasicdetail.passport_no;
                            saveObj.Race = _CurrentEmployeeOtherBasicdetail.Race;
                            saveObj.isActive = CurrentEmployee.isActive;
                            saveObj.isdelete = CurrentEmployee.isdelete;
                            saveObj.save_user_id = clsSecurity.loggedUser.user_id;
                            saveObj.save_datetime = DateTime.Now;
                            saveObj.modified_datetime = DateTime.Now;
                            saveObj.modified_user_id = clsSecurity.loggedUser.user_id;

                            newEmp.dtl_EmployeeOtherBasicDetails = saveObj;
                        }
                        catch (Exception)
                        {

                        }

                    }
                }
                else
                {


                }

                #endregion

                #region Employee Additional Details

                if (CurrentEmployee != null && CurrentAdditionaldtl != null)
                {
                    dtl_employee_Additional_details tempAdddtl = new dtl_employee_Additional_details();
                    tempAdddtl.employee_id = CurrentEmployee.employee_id;
                    tempAdddtl.electorial_divition_id = CurrentAdditionaldtl.electorial_divition_id;
                    tempAdddtl.election_center_id = CurrentAdditionaldtl.election_center_id;
                    tempAdddtl.grama_niladhari_devision_id = CurrentAdditionaldtl.grama_niladhari_devision_id;
                    tempAdddtl.nearest_police_station_id = CurrentAdditionaldtl.nearest_police_station_id;
                    tempAdddtl.race_id = CurrentAdditionaldtl.race_id;
                    tempAdddtl.nationality_id = CurrentAdditionaldtl.nationality_id;
                    tempAdddtl.dis_between_resident_to_hometown = CurrentAdditionaldtl.dis_between_resident_to_hometown;
                    tempAdddtl.isactive = CurrentEmployee.isActive;
                    tempAdddtl.isdelete = CurrentEmployee.isdelete;
                    tempAdddtl.save_user_id = clsSecurity.loggedUser.user_id;
                    tempAdddtl.save_datetime = DateTime.Now;
                    tempAdddtl.modified_datetime = DateTime.Now;
                    tempAdddtl.modified_user_id = clsSecurity.loggedUser.user_id;


                    newEmp.dtl_employee_Additional_details = tempAdddtl;
                }

                #endregion

                if (isUpdate)
                {

                    if (_CurrentEmployee.basic_salary != null)
                    {
                        isNewSalary = true;
                        if (empOldSalary == null)
                        {
                            oldsalary = 0;
                            isAct = Act();
                        }
                        else if (empOldSalary != _CurrentEmployee.basic_salary)
                        {
                            isAct = Act();
                            oldsalary = (decimal)empOldSalary;
                        }
                        else
                        {
                            isNewSalary = false;
                        }
                    }

                    if (isNewSalary == true)
                    {
                        dtl_historyBasicSalary basicsalary = new dtl_historyBasicSalary();
                        basicsalary.employee_id = CurrentEmployee.employee_id;
                        basicsalary.old_salary = oldsalary;
                        basicsalary.new_salary = CurrentEmployee.basic_salary;
                        basicsalary.affective_date = System.DateTime.Now;
                        basicsalary.update_user_id = clsSecurity.loggedUser.user_id;
                        basicsalary.isactive = true;
                        bool a = serviceClient.UpdateEmployeeActive(basicsalary);
                        bool b = serviceClient.UpdateEmployeeBasicSalary(basicsalary);
                    }

                    newDaatel.modified_datetime = System.DateTime.Now;
                    newDaatel.modified_user_id = clsSecurity.loggedUser.user_id;

                    if (clsSecurity.GetUpdatePermission(209))
                    {
                        try
                        {
                            newEmp.modified_datetime = DateTime.Now;
                            newEmp.modified_user_id = clsSecurity.loggedUser.user_id;
                            EmployeeHistoryList.Clear();

                            #region History Details

                            if (CurrentEmployeeHistory != null)
                            {
                                if (newDaatel.basic_salary != null && CurrentEmployeeHistory.basic_salary != null && newDaatel.basic_salary != CurrentEmployeeHistory.basic_salary)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newDaatel.basic_salary.ToString();
                                    History.Old_Value = CurrentEmployeeHistory.basic_salary == 0 ? "Intial Basic Salary" : CurrentEmployeeHistory.basic_salary.ToString();
                                    History.Sub_Category_Id = 8;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.city_id != null && CurrentEmployeeHistory.city_id != null && CurrentEmployeeHistory.city_id != newEmp.city_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = CurrentCity.city;
                                    History.Old_Value = CurrentEmployeeHistory.city;
                                    History.Sub_Category_Id = 15;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.civil_status_id != null && CurrentEmployeeHistory.civil_status_id != null && CurrentEmployeeHistory.civil_status_id != newEmp.civil_status_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = CurrentCivilStatus.civil_status;
                                    History.Old_Value = CurrentEmployeeHistory.civil_status;
                                    History.Sub_Category_Id = 19;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newDaatel.department_id != null && CurrentEmployeeHistory.department_id != null && CurrentEmployeeHistory.department_id != newDaatel.department_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newDaatel.employee_id;
                                    History.New_Value = CurrentDepartment.department_name;
                                    History.Old_Value = CurrentEmployeeHistory.department_name;
                                    History.Sub_Category_Id = 5;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newDaatel.designation_id != null && CurrentEmployeeHistory.designation_id != null && CurrentEmployeeHistory.designation_id != newDaatel.designation_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newDaatel.employee_id;
                                    History.New_Value = CurrentDesignation.designation;
                                    History.Old_Value = CurrentEmployeeHistory.designation;
                                    History.Sub_Category_Id = 2;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newDaatel.grade_id != null && CurrentEmployeeHistory.grade_id != null && CurrentEmployeeHistory.grade_id != newDaatel.grade_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newDaatel.employee_id;
                                    History.New_Value = CurrentGrade.grade;
                                    History.Old_Value = CurrentEmployeeHistory.grade;
                                    History.Sub_Category_Id = 6;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newDaatel.join_date != null && CurrentEmployeeHistory.join_date != null && CurrentEmployeeHistory.join_date != newDaatel.join_date)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newDaatel.employee_id;
                                    History.New_Value = newDaatel.join_date.ToString();
                                    History.Old_Value = CurrentEmployeeHistory.join_date.ToString();
                                    History.Sub_Category_Id = 23;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newDaatel.payment_methord_id != null && CurrentEmployeeHistory.payment_methord_id != null && CurrentEmployeeHistory.payment_methord_id != newDaatel.payment_methord_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newDaatel.employee_id;
                                    if (paymentmethord != null)
                                    {
                                        History.New_Value = paymentmethord.FirstOrDefault(c => c.paymet_method_id == CurrentEmployee.payment_methord_id).payment_method;
                                        History.Old_Value = paymentmethord.FirstOrDefault(c => c.paymet_method_id == CurrentEmployeeHistory.payment_methord_id).payment_method;
                                    }
                                    History.Sub_Category_Id = 7;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newDaatel.section_id != null && CurrentEmployeeHistory.section_id != null && CurrentEmployeeHistory.section_id != newDaatel.section_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newDaatel.employee_id;
                                    History.New_Value = CurrentSection.section_name;
                                    History.Old_Value = CurrentEmployeeHistory.section_name;
                                    History.Sub_Category_Id = 4;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.town_id != null && CurrentEmployeeHistory.town_id != null && CurrentEmployeeHistory.town_id != newEmp.town_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = CurrentTown.town_name;
                                    History.Old_Value = CurrentEmployeeHistory.town_name;
                                    History.Sub_Category_Id = 16;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newDaatel.branch_id != null && CurrentEmployeeHistory.branch_id != null && CurrentEmployeeHistory.branch_id != newDaatel.branch_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newDaatel.employee_id;
                                    History.New_Value = CurretCompanyBranch.companyBranch_Name;
                                    History.Old_Value = CurrentEmployeeHistory.companyBranch_Name;
                                    History.Sub_Category_Id = 3;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.email != null && CurrentEmployeeHistory.email != null && CurrentEmployeeHistory.email != newEmp.email)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.email;
                                    History.Old_Value = CurrentEmployeeHistory.email;
                                    History.Sub_Category_Id = 22;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.nic != null && CurrentEmployeeHistory.nic != null && CurrentEmployeeHistory.nic != newEmp.nic)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.nic;
                                    History.Old_Value = CurrentEmployeeHistory.nic;
                                    History.Sub_Category_Id = 18;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.address_01 != null || newEmp.address_02 != null || newEmp.address_03 != null)
                                {
                                    his_Employee History = new his_Employee();
                                    bool Changed = false;

                                    if (newEmp.address_01 != null && CurrentEmployeeHistory.address_01 != null && newEmp.address_01 != CurrentEmployeeHistory.address_01)
                                    {
                                        History.Old_Value = CurrentEmployeeHistory.address_01;
                                        History.New_Value = newEmp.address_01;
                                        Changed = true;
                                    }

                                    if (newEmp.address_02 != null && CurrentEmployeeHistory.address_02 != null && newEmp.address_02 != CurrentEmployeeHistory.address_02)
                                    {
                                        History.Old_Value = History.Old_Value == null ? "" : History.Old_Value + " " + CurrentEmployeeHistory.address_02;
                                        History.New_Value = History.New_Value == null ? "" : History.New_Value + " " + newEmp.address_02;
                                        Changed = true;
                                    }

                                    if (newEmp.address_03 != null && CurrentEmployeeHistory.address_03 != null && newEmp.address_03 != CurrentEmployeeHistory.address_03)
                                    {
                                        History.Old_Value = History.Old_Value == null ? "" : History.Old_Value + " " + CurrentEmployeeHistory.address_03;
                                        History.New_Value = History.New_Value == null ? "" : History.New_Value + " " + newEmp.address_03;
                                        Changed = true;
                                    }

                                    if (Changed)
                                    {
                                        History.Sub_Category_Id = 1;
                                        History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                        History.SaveDate = DateTime.Now;
                                        EmployeeHistoryList.Add(History);
                                    }
                                }

                                if (newEmp.telephone != null && CurrentEmployeeHistory.telephone != null && newEmp.telephone != CurrentEmployeeHistory.telephone)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.telephone;
                                    History.Old_Value = CurrentEmployeeHistory.telephone;
                                    History.Sub_Category_Id = 20;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }
                                if (newEmp.mobile != null && CurrentEmployeeHistory.mobile != null && newEmp.mobile != CurrentEmployeeHistory.mobile)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.mobile;
                                    History.Old_Value = CurrentEmployeeHistory.mobile;
                                    History.Sub_Category_Id = 21;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.first_name != null && CurrentEmployeeHistory.first_name != null && newEmp.first_name != CurrentEmployeeHistory.first_name)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.first_name;
                                    History.Old_Value = CurrentEmployeeHistory.first_name;
                                    History.Sub_Category_Id = 11;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.second_name != null && CurrentEmployeeHistory.second_name != null && newEmp.second_name != CurrentEmployeeHistory.second_name)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.second_name;
                                    History.Old_Value = CurrentEmployeeHistory.second_name;
                                    History.Sub_Category_Id = 12;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.surname != null && CurrentEmployeeHistory.surname != null && newEmp.surname != CurrentEmployeeHistory.surname)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.surname;
                                    History.Old_Value = CurrentEmployeeHistory.surname;
                                    History.Sub_Category_Id = 13;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.birthday != null && CurrentEmployeeHistory.birthday != null && newEmp.birthday != CurrentEmployeeHistory.birthday)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.birthday.ToString();
                                    History.Old_Value = CurrentEmployeeHistory.birthday.ToString();
                                    History.Sub_Category_Id = 17;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newDaatel.resign_date != null && CurrentEmployeeHistory.resign_date != null && newDaatel.resign_date != CurrentEmployeeHistory.resign_date)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newDaatel.resign_date.ToString();
                                    History.Old_Value = CurrentEmployeeHistory.resign_date.ToString();
                                    History.Sub_Category_Id = 24;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.title_id != null && CurrentEmployeeHistory.title_id != null && newEmp.title_id != CurrentEmployeeHistory.title_id)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    if (Title != null && Title.Count() > 0)
                                    {
                                        History.New_Value = Title.FirstOrDefault(c => c.title_id == newEmp.title_id).title_name;
                                        History.Old_Value = Title.FirstOrDefault(c => c.title_id == CurrentEmployeeHistory.title_id).title_name;
                                    }
                                    History.Sub_Category_Id = 25;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.epf_no != null && CurrentEmployeeHistory.epf_no != null && newEmp.epf_no != CurrentEmployeeHistory.epf_no)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.epf_no.ToString();
                                    History.Old_Value = CurrentEmployeeHistory.epf_no.ToString();
                                    History.Sub_Category_Id = 9;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (newEmp.etf_no != null && CurrentEmployeeHistory.etf_no != null && newEmp.etf_no != CurrentEmployeeHistory.etf_no)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = newEmp.etf_no.ToString();
                                    History.Old_Value = CurrentEmployeeHistory.etf_no.ToString();
                                    History.Sub_Category_Id = 10;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (CurrentEmployeeHistory.contract_start_date != null && CurrentEmployee.contract_start_date != null && CurrentEmployeeHistory.contract_start_date != CurrentEmployee.contract_start_date)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = CurrentEmployee.contract_start_date.ToString();
                                    History.Old_Value = CurrentEmployeeHistory.contract_start_date.ToString();
                                    History.Sub_Category_Id = 24;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (CurrentEmployeeHistory.contract_end_date != null && CurrentEmployee.contract_end_date != null && CurrentEmployeeHistory.contract_end_date != CurrentEmployee.contract_end_date)
                                {
                                    his_Employee History = new his_Employee();
                                    History.Employee_Id = newEmp.employee_id;
                                    History.New_Value = CurrentEmployee.contract_end_date.ToString();
                                    History.Old_Value = CurrentEmployeeHistory.contract_end_date.ToString();
                                    History.Sub_Category_Id = 24;
                                    History.Save_User_Id = clsSecurity.loggedUser.user_id;
                                    History.SaveDate = DateTime.Now;
                                    EmployeeHistoryList.Add(History);
                                }

                                if (EmployeeHistoryList != null && EmployeeHistoryList.Count > 0)
                                {
                                    EffectiveDates = null;
                                    EffectiveDates = EmployeeHistoryList.Where(c => c.Sub_Category_Id == 1 || c.Sub_Category_Id == 2 || c.Sub_Category_Id == 3 || c.Sub_Category_Id == 4 || c.Sub_Category_Id == 5 || c.Sub_Category_Id == 6 || c.Sub_Category_Id == 7 || c.Sub_Category_Id == 8 || c.Sub_Category_Id == 15 || c.Sub_Category_Id == 16 || c.Sub_Category_Id == 19);

                                    //if (EffectiveDates != null && EffectiveDates.Count() > 0)
                                    //{
                                    //    EmployeeDetailsEffectiveDates EffectiveDays = new EmployeeDetailsEffectiveDates(this);
                                    //    EffectiveDays.ShowDialog();
                                    //}

                                    newEmp.his_Employee = EmployeeHistoryList.ToArray();
                                    EmployeeHistoryList.ForEach(c => c.EffectiveDate = DateTime.Now);
                                }

                            }

                            #endregion

                            if (isAct && serviceClient.UpdateEmployee(newEmp, ContractStartDate, ContractEndDate))// && SaveMemberStatus() && bloodSave())
                            {
                                if (serviceClient.UpdateAccountName(newEmp.employee_id, newEmp.dtl_EmployeeOtherOfficialDetails.account_name, (Guid)newEmp.modified_user_id, (DateTime)newEmp.modified_datetime))
                                {
                                    clsMessages.setMessage(Properties.Resources.UpdateSucess);
                                    isCurrentEmpSelection = true;
                                    refreshBloodDetail();
                                    refreshEmployees();
                                    refreshEpfStatus();
                                    refreshExtendedDetails();

                                    #region Mihiri_Report Print

                                    refreshConfirmationDetail();

                                    //MessageBox.Show(Convert.ToString(OldConfirmDetails.Count()));

                                    if (OldConfirmDetails != null && OldConfirmDetails.Where(c => c.employee_id == newEmp.employee_id).Count() > 0)
                                    {
                                        if (newDaatel.prmernant_active_date != null)
                                        {
                                            MessageBoxResult dr = MessageBox.Show("Permenent date changed.Do you want to print a letter for Employee?", "Employee Updated", MessageBoxButton.YesNo, MessageBoxImage.None);
                                            if (dr == MessageBoxResult.Yes)
                                            {
                                                // \Reports\Documents\HR_Report\ConfirmationDateExtendReport
                                                ReportPrint print = new ReportPrint("\\Reports\\Documents\\HR_Report\\ConfirmationDateExtendReport");
                                                print.setParameterValue("@id", newEmp.employee_id.ToString());
                                                print.PrintReportWithReportViewer();
                                            }
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    clsMessages.setMessage("Record save successfull except for Account name");
                                    isCurrentEmpSelection = true;
                                    refreshBloodDetail();
                                    refreshEmployees();
                                    refreshEpfStatus();
                                    refreshExtendedDetails();


                                    #region Mihiri_Report Print

                                    refreshConfirmationDetail();

                                    //MessageBox.Show(Convert.ToString(OldConfirmDetails.Count()));

                                    if (OldConfirmDetails != null && OldConfirmDetails.Where(c => c.employee_id == newEmp.employee_id).Count() > 0)
                                    {
                                        if (newDaatel.prmernant_active_date != null)
                                        {
                                            MessageBoxResult dr = MessageBox.Show("Permenent date changed.Do you want to print a letter for Employee?", "Employee Updated", MessageBoxButton.YesNo, MessageBoxImage.None);
                                            if (dr == MessageBoxResult.Yes)
                                            {
                                                // \Reports\Documents\HR_Report\ConfirmationDateExtendReport
                                                ReportPrint print = new ReportPrint("\\Reports\\Documents\\HR_Report\\ConfirmationDateExtendReport");
                                                print.setParameterValue("@id", newEmp.employee_id.ToString());
                                                print.PrintReportWithReportViewer();
                                            }
                                        }
                                    }

                                    #endregion
                                }

                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.UpdateFail);
                            }
                        }
                        catch (Exception)
                        {
                            clsMessages.setMessage("Operation was discarded due to network traffic, please save again...");
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                    }
                }
                else
                {
                    if (clsSecurity.GetSavePermission(209))
                    {

                        try
                        {
                            newDaatel.save_datetime = System.DateTime.Now;
                            newDaatel.save_user_id = clsSecurity.loggedUser.user_id;
                            newDaatel.modified_datetime = System.DateTime.Now;
                            newDaatel.modified_user_id = clsSecurity.loggedUser.user_id;
                            newDaatel.delete_datetime = System.DateTime.Now;
                            newDaatel.delete_user_id = clsSecurity.loggedUser.user_id;
                            EmployeeSumarryView existEmployee = AllEmployees.FirstOrDefault(c => c.emp_id.TrimStart('0') == newEmp.emp_id.TrimStart('0').TrimStart(' ').TrimEnd(' '));
                            if (existEmployee == null)
                            {
                                if (serviceClient.SaveEmployee(newEmp, ContractStartDate, ContractEndDate) && SaveMemberStatus())
                                {
                                    //reafreshMasEmployee();
                                    clsMessages.setMessage(Properties.Resources.SaveSucess);
                                    isCurrentEmpSelection = false;
                                    refreshBloodDetail();
                                    refreshEmployees();
                                    refreshEpfStatus();
                                    refreshExtendedDetails();
                                }
                                else
                                {
                                    clsMessages.setMessage(Properties.Resources.SaveFail);
                                }
                            }
                            else
                                clsMessages.setMessage("Employee Number Already Exist");
                        }
                        catch (Exception)
                        {
                            clsMessages.setMessage("Operation was discarded due to network traffic, please save again...");
                        }

                    }
                    else
                    {
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                    }
                }
            }
            else
            {
                MessageBox.Show("Required fields cannot be empty.");
            }
        }

        #endregion

        #region Delete Methord
        private void delete()
        {
            if (clsSecurity.GetDeletePermission(209))
            {
                dtl_Employee delEmp = new dtl_Employee();
                delEmp.employee_id = CurrentEmployee.employee_id;
                delEmp.delete_datetime = System.DateTime.Now;
                delEmp.delete_user_id = clsSecurity.loggedUser.user_id;
                MessageBoxResult Result = new MessageBoxResult();

                Result = MessageBox.Show("Do You Want to Delete This Record ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    if (serviceClient.DeleteEmployee(delEmp))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        refreshEmployees();
                        //reafreshEmployeeDetail();
                        //reafreshMasEmployee();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }
        #endregion

#region sync method
        private void sync()
        {
            if (clsSecurity.GetDeletePermission(209))
            {
                BusyBox.ShowBusy("Sync Process Started...");
                bool status = GoogleDriveModel.GetInstance.portalImageSync();
                BusyBox.CloseBusy();
                if (status)
                    clsMessages.setMessage("Image Sync is completed.");
                else
                    clsMessages.setMessage("Something went wrong.");
                refreshEmployees();
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

#endregion

        #endregion User Defined Methods

        #region Epf Status

        private List<string> _ePFList = new List<string>();
        public List<string> EPFList
        {
            get { return _ePFList; }
            set { _ePFList = value; OnPropertyChanged("EPFList"); }
        }

        private string _currentEPFList;
        public string CurrentEPFList
        {
            get { return _currentEPFList; }
            set { _currentEPFList = value; OnPropertyChanged("CurrentEPFList"); }
        }

        private IEnumerable<dtl_member_status> memberStaus;
        public IEnumerable<dtl_member_status> MemberStaus
        {
            get { return memberStaus; }
            set { memberStaus = value; OnPropertyChanged("MemberStaus"); }
        }

        private bool SaveMemberStatus()
        {
            if (CurrentEmployee != null && (String.IsNullOrEmpty(CurrentEPFList)) == false)
                try
                {
                    bool update = false;

                    dtl_member_status member = new dtl_member_status();
                    if (CurrentEmployee != null)
                        member.employee_id = CurrentEmployee.employee_id;
                    if (CurrentEPFList.Equals("Extg."))
                        member.member_status = "E";
                    else if (CurrentEPFList.Equals("New"))
                        member.member_status = "N";
                    else if (CurrentEPFList.Equals("Vacated"))
                        member.member_status = "V";
                    update = MemberStaus.Where(c => c.employee_id == CurrentEmployee.employee_id).Count() == 1;
                    if (update)
                        if (serviceClient.UpdateMemberStatus(member))
                            return true;
                        else
                            return false;
                    else
                        if (serviceClient.SaveMemberSatus(member))
                        return true;
                    else
                            if (serviceClient.UpdateMemberStatus(member))
                        return true;
                    else
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
            return true;

        }

        #endregion Epf Status

        #region Search Function

        private List<string> _SearchItem;
        public List<string> SearchItem
        {
            get { return _SearchItem; }
            set { _SearchItem = value; this.OnPropertyChanged("SearchItem"); }
        }

        private List<string> _SelectionItems = new List<string>();
        public List<string> SelectionItems
        {
            get { return this._SelectionItems; }
            set { this._SelectionItems = value; this.OnPropertyChanged("SelectionItems"); }
        }

        private string _SearchSelectedItem;
        public string SearchSelectedItem
        {
            get { return this._SearchSelectedItem; }
            set { this._SearchSelectedItem = value; OnPropertyChanged("SearchSelectedItem"); }
        }

        private string _Search;
        public string Search
        {
            get
            {
                return this._Search;
            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");

                if (this._Search == "")
                {
                    refreshSearch();
                }
                else
                {
                    searchTextChanged();
                }
            }
        }

        public List<string> SerchItem = new List<string>();

        public void searchTextChanged()
        {
            if (SearchSelectedItem == "Employee No")
            {
                AllEmployeesSorted = AllEmployees.Where(e => e.emp_id != null && e.emp_id.ToUpper().Contains(Search.ToUpper())).ToList();
            }
            if (SearchSelectedItem == "First Name")
            {
                AllEmployeesSorted = AllEmployees.Where(e => e.first_name != null && e.first_name.ToUpper().Contains(Search.ToUpper())).ToList();
            }
            if (SearchSelectedItem == "Last Name")
            {
                AllEmployeesSorted = AllEmployees.Where(e => e.second_name != null && e.second_name.ToUpper().Contains(Search.ToUpper())).ToList();
            }
        }

        public void refreshSearch()
        {
            this.AllEmployeesSorted = this.AllEmployees;
        }

        #endregion Search Function

        #region Busy Box

        private bool _isBusy;
        public bool isBusy
        {
            get { return _isBusy; }
            set { _isBusy = value; this.OnPropertyChanged("isBusy"); }
        }

        private Visibility _busyGridVisibility;
        public Visibility busyGridVisibility
        {
            get { return _busyGridVisibility; }
            set { _busyGridVisibility = value; this.OnPropertyChanged("busyGridVisibility"); }
        }



        #endregion Busy Box

        #region Additional UserContol Loader

        #region AddMethods

        void CityAdd()
        {
            CityWindow cityW = new CityWindow();
            cityW.ShowDialog();
            refreshCity();
        }

        void TownAdd()
        {
            TownMasterWindow TownMasterW = new TownMasterWindow();
            TownMasterW.ShowDialog();
            RefershTowns();
        }

        void DepartmentAdd()
        {
            DepartmentsWindow DepartmentsW = new DepartmentsWindow();
            DepartmentsW.ShowDialog();
            refreshDepartment();
        }

        void DesignationAdd()
        {
            DesignationWindow DesignationW = new DesignationWindow();
            DesignationW.ShowDialog();
            refreshDesignation();
        }

        void SectionAdd()
        {
            SectionsWindow SectionsW = new SectionsWindow();
            SectionsW.ShowDialog();
            refreshSection();
        }

        void GradeAdd()
        {
            EmployeeGradeWindow EmployeeGradeW = new EmployeeGradeWindow();
            EmployeeGradeW.ShowDialog();
            refreshGrade();
        }

        void SocialMediaADD()
        {
            SocialMedia Window = new SocialMedia();
            Window.ShowDialog();
            RefreshSocialMedia();
        }

        void ExtraCurricularADD()
        {
            ExtraCurriculamActivityWindow window = new ExtraCurriculamActivityWindow();
            window.ShowDialog();
            RefreshExtraCurriculamActivity();
        }

        void helthADD()
        {
            HealthTypeWindow window = new HealthTypeWindow();
            window.ShowDialog();
            RefreshHealthType();
        }

        void uniGradeADD()
        {
            InstituteGradeWindow window = new InstituteGradeWindow();
            window.ShowDialog();
            refereshInstituteGrade();
        }

        void InterstADD()
        {
            InterestDetailsWindow window = new InterestDetailsWindow();
            window.ShowDialog();
            RefreshInterestField();
        }

        void SkillADD()
        {
            SkillTypeWindow window = new SkillTypeWindow();
            window.ShowDialog();
            RefreshSkillType();
        }

        void LifeADD()
        {
            LifeInsuranceWindow window = new LifeInsuranceWindow();
            window.ShowDialog();
            RefreshLifeInsurance();
        }

        void UniCattypeADD()
        {
            ProfessionalQualificationCatagoryTypeWindow window = new ProfessionalQualificationCatagoryTypeWindow();
            CurrentUnivercityName = null;
            window.ShowDialog();
            RefreshProfessionalQualification();

        }

        void AcadamicQualificationcategoryADD()
        {
            SchoolQualificationCategoryTypeWindow window = new SchoolQualificationCategoryTypeWindow();
            window.ShowDialog();
            RefreshSchoolQualificationCategory();
        }

        void AcadamicGradeADD()
        {
            SchoolGradeWindow window = new SchoolGradeWindow();
            window.ShowDialog();
            RefreshSchoolGrade();
        }

        void AcadamicsubjectADD()
        {
            SchoolSubjectNameWindow window = new SchoolSubjectNameWindow();
            CurrentSchoolQualificationType = null;
            window.ShowDialog();
            RefreshSchoolSubjectName();


        }

        void DegreeNameADD()
        {
            QualificationCategoryNameWindow window = new QualificationCategoryNameWindow();
            CurrentUnivercityDigreeType = null;
            window.ShowDialog();
            RefreshQualificvationCategory();
        }


        void ElectionCenterAdd()
        {
            ElectionCenterWindow window = new ElectionCenterWindow();
            CurrentElectionCenter = null;
            window.ShowDialog();
            RefreshElectionCenter();
        }

        void ElectorialDivisionAdd()
        {
            ElectorialDivisionWindow window = new ElectorialDivisionWindow();
            CurrentElectorialDivision = null;
            window.ShowDialog();
            RefreshElectorialDivision();
        }

        void GramaniladhariDivisionsAdd()
        {
            GramaNiladhariDivisionWindow window = new GramaNiladhariDivisionWindow();
            CurrentGramaNiladhari = null;
            window.ShowDialog();
            RefreshGramaNiladhari();
        }


        void PoliceStationAdd()
        {
            PoliceStationWindow window = new PoliceStationWindow();
            CurrentNearestPoliceStation = null;
            window.ShowDialog();
            RefreshNearestPoliceStation();
        }


        void RaceAdd()
        {
            RaceWindow window = new RaceWindow();
            CurrentRace = null;
            window.ShowDialog();
            RefreshRace();
        }

        void NationalityAdd()
        {
            NationalityWindow window = new NationalityWindow();
            CurrentNationality = null;
            window.ShowDialog();
            RefreshNationality();
        }

        #endregion

        public ICommand CityAddButton
        {
            get
            {
                return new RelayCommand(CityAdd);
            }
        }

        public ICommand TownAddButton
        {
            get
            {
                return new RelayCommand(TownAdd);
            }
        }

        public ICommand DepartmentAddButton
        {
            get
            {
                return new RelayCommand(DepartmentAdd);
            }
        }

        public ICommand DesignationAddButton
        {
            get
            {
                return new RelayCommand(DesignationAdd);
            }
        }

        public ICommand SectionAddButton
        {
            get
            {
                return new RelayCommand(SectionAdd);
            }
        }


        #region Supun Add SocialMedia Button

        public ICommand SocialMediaAddButton
        {
            get
            {
                return new RelayCommand(SocialMediaADD);
            }
        }

        #endregion

        #region Add Institiute Name

        public ICommand InstitiuteAddButton
        {
            get
            {
                return new RelayCommand(InstitiuteADD);
            }
        }

        private void InstitiuteADD()
        {
            InstituteNameWindow Window = new InstituteNameWindow();
            Window.ShowDialog();
            RefreshUniName();
        }


        #endregion

        public ICommand ExtraCurricularAddButton
        {
            get { return new RelayCommand(ExtraCurricularADD); }
        }

        public ICommand HealthButton
        {
            get { return new RelayCommand(helthADD); }
        }

        public ICommand GradeAddButton
        {
            get { return new RelayCommand(GradeAdd); }
        }

        public ICommand UniGradeAddButton
        {
            get { return new RelayCommand(uniGradeADD); }
        }

        public ICommand InterstAddButton
        {
            get { return new RelayCommand(InterstADD); }
        }

        public ICommand SkillAddButton
        {
            get { return new RelayCommand(SkillADD); }
        }

        public ICommand LifeAddButton
        {
            get { return new RelayCommand(LifeADD); }
        }

        public ICommand UniCattypeAddButton
        {
            get { return new RelayCommand(UniCattypeADD); }
        }

        public ICommand AcadamicQualificationcategoryAddButton
        {
            get { return new RelayCommand(AcadamicQualificationcategoryADD); }
        }

        public ICommand AcadamicGradeAddButton
        {
            get { return new RelayCommand(AcadamicGradeADD); }
        }

        public ICommand AcadamicsubjectAddButton
        {
            get { return new RelayCommand(AcadamicsubjectADD); }
        }

        public ICommand DegreeNameAddButton
        {
            get { return new RelayCommand(DegreeNameADD); }
        }

        public ICommand ElectionCenterAddButton
        {
            get { return new RelayCommand(ElectionCenterAdd); }
        }

        public ICommand ElectorialDivisionAddButton
        {
            get { return new RelayCommand(ElectorialDivisionAdd); }
        }

        public ICommand GramaniladhariDivisionsAddButton
        {
            get { return new RelayCommand(GramaniladhariDivisionsAdd); }
        }

        public ICommand PoliceStationAddButton
        {
            get { return new RelayCommand(PoliceStationAdd); }
        }

        public ICommand RaceAddButton
        {
            get { return new RelayCommand(RaceAdd); }
        }

        public ICommand NationalityAddButton
        {
            get { return new RelayCommand(NationalityAdd); }
        }

        #endregion Additional UserContol Loader

        #region Error

        #region Validation Properties
        private string _EmployeeNo;
        public string EmployeeNo
        {
            get { return _EmployeeNo; }
            set
            {
                _EmployeeNo = value; OnPropertyChanged("EmployeeNo");
                if (CurrentEmployee != null)
                    CurrentEmployee.emp_id = EmployeeNo;
            }
        }

        private string _FirstName;
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value; OnPropertyChanged("FirstName");
                if (CurrentEmployee != null)
                    CurrentEmployee.first_name = FirstName;
            }
        }

        private string _Surname;
        public string Surname
        {
            get { return _Surname; }
            set
            {
                _Surname = value; OnPropertyChanged("Surname");
                if (CurrentEmployee != null)
                    CurrentEmployee.surname = Surname;
            }
        }

        private string _NIC;
        public string NIC
        {
            get { return _NIC; }
            set
            {
                _NIC = value; OnPropertyChanged("NIC");
                if (CurrentEmployee != null)
                    CurrentEmployee.nic = NIC;
            }
        }

        private DateTime? _Birthday;
        public DateTime? Birthday
        {
            get { return _Birthday; }
            set
            {
                _Birthday = value; OnPropertyChanged("Birthday");
                if (CurrentEmployee != null)
                {
                    CurrentEmployee.birthday = Birthday;
                }
                //CurrentEmployeeDetail.birthday = Birthday;
            }

        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set
            {
                _Address = value; OnPropertyChanged("Address");
                if (CurrentEmployee != null)
                    CurrentEmployee.address_01 = Address;

            }
        }

        private string _Mobile;
        public string Mobile
        {
            get { return _Mobile; }
            set
            {
                _Mobile = value; OnPropertyChanged("Mobile");
                if (CurrentEmployee != null)
                    CurrentEmployee.mobile = Mobile;

            }
        }
        #endregion

        #region Error Interface
        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        string IDataErrorInfo.this[string PropertyName]
        {
            get
            {
                return getValidationError(PropertyName);
            }
        }
        #endregion

        private string getValidationError(string PropertyName)
        {

            String error = null;

            switch (PropertyName)
            {
                case "EmployeeNo":
                    error = String.IsNullOrWhiteSpace(EmployeeNo) ? "Cannot Be Empty" : null;
                    break;
                case "FirstName":
                    error = String.IsNullOrWhiteSpace(FirstName) ? "Cannot Be Empty" : null;
                    break;
                case "Surname":
                    error = String.IsNullOrWhiteSpace(Surname) ? "Cannot Be Empty" : null;
                    break;
                case "NIC":
                    error = String.IsNullOrWhiteSpace(NIC) ? "Cannot Be Empty" : null;
                    break;
                case "CurrentTown":
                    error = CurrentTown == null ? "Cannot Be Empty" : null;
                    break;
                case "CurrentCivilStatus":
                    error = CurrentCivilStatus == null ? "Cannot Be Empty" : null;
                    break;
                //case "Birthday":
                //    error = CurrentEmployeeDetail.birthday == null ? "Cannot Be Empty" : null;
                //    break;
                case "Address":
                    error = String.IsNullOrWhiteSpace(Address) ? "Cannot Be Empty" : null;
                    break;
            }

            return error;
        }

        #endregion

        #region AutoLeave
        public ICommand LeaveButton
        {
            get { return new RelayCommand(LeaveLaunch); }
        }

        void LeaveLaunch()
        {
            Leave();
        }

        bool Leave()
        {
            bool isLeave = false;
            IEnumerable<EmployeeSumarryView> alldetailemployees = serviceClient.GetAllEmployeeDetail();
            foreach (EmployeeSumarryView emp in alldetailemployees)
            {
                if (emp.employee_id == CurrentEmployee.employee_id)
                {
                    isLeave = true;
                    break;
                }
            }

            if (isLeave)
            {

                EmployeeLeaveWindow window = new EmployeeLeaveWindow(CurrentEmployee.employee_id);
                window.ShowDialog();

                /*if (window.viewModel.SaveList == "" || window.viewModel.FailList != "")
                {
                    window.Close();
                    window = null;
                    return false;
                }
                else
                {
                    window.Close();
                    window = null;
                    return true;
                }*/

                window.Close();
                window = null;
                return true;
            }
            else
            {
                clsMessages.setMessage("Please select a saved employee");
                return false;
            }
        }

        #endregion

        #region Act
        public ICommand ActButton
        {
            get { return new RelayCommand(ActLaunch); }
        }

        void ActLaunch()
        {
            Act();
        }
        bool Act()
        {
            bool isAct = false;
            IEnumerable<EmployeeSumarryView> alldetailemployees = serviceClient.GetAllEmployeeDetail();
            foreach (EmployeeSumarryView emp in alldetailemployees)
            {
                if (emp.employee_id == CurrentEmployee.employee_id)
                {
                    isAct = true;
                    break;
                }
            }

            if (isAct)
            {
                if (CurrentEmployee.basic_salary != null && CurrentEmployee.basic_salary > 0)
                {
                    EmployeeActWindow window = new EmployeeActWindow(CurrentEmployee.employee_id, (decimal)CurrentEmployee.basic_salary);
                    window.ShowDialog();

                    if (window.viewModel.SaveList == "" || window.viewModel.FailList != "")
                    {
                        window.Close();
                        window = null;
                        return false;
                    }
                    else
                    {
                        window.Close();
                        window = null;
                        return true;
                    }

                }
                else
                {
                    clsMessages.setMessage("Basic Salary should be greater than 0");
                    return false;
                }
            }
            else
            {
                clsMessages.setMessage("Please select a saved employee");
                return false;
            }
        }
        #endregion

        #region Medical
        public ICommand MedicalButton
        {
            get { return new RelayCommand(Medical); }
        }
        void Medical()
        {
            bool MedCanLoad = false;
            IEnumerable<EmployeeSumarryView> alldetailemployees = serviceClient.GetAllEmployeeDetail();
            foreach (EmployeeSumarryView emp in alldetailemployees)
            {
                if (emp.employee_id == CurrentEmployee.employee_id)
                {
                    MedCanLoad = true;
                    break;
                }
            }
            if (MedCanLoad)
            {
                MedicalDetailsUserControl userControl = new MedicalDetailsUserControl(CurrentEmployee);
                UserControlWindow uC = new UserControlWindow(userControl);
                uC.Height = userControl.Height;
                uC.Width = userControl.Width;
                uC.ShowDialog();
                uC.Close();
                uC = null;
            }
            else
                clsMessages.setMessage("Please select a saved employee");
        }
        #endregion

        #region Scale
        private double _scaleSize;

        public double ScaleSize
        {
            get { return _scaleSize; }
            set { _scaleSize = value; OnPropertyChanged("ScaleSize"); }
        }

        public void scale()
        {
            ScaleSize = 1;
            double h = System.Windows.SystemParameters.PrimaryScreenHeight;
            double w = System.Windows.SystemParameters.PrimaryScreenWidth;
            //  double h= System.Windows.SystemParameters.PrimaryScreenHeight;
            if (h * w == 1366 * 768)
                ScaleSize = 0.90;
            // ScaleSize = 0.75;
            if (w * h == 1280 * 768)
                ScaleSize = 0.90;
            if (w * h == 1024 * 768)
                ScaleSize = 1.2;
        }
        #endregion

        #region Image

        private string _employeeImagePath;
        public string EmployeeImagePath
        {
            get { return _employeeImagePath; }
            set { _employeeImagePath = value; OnPropertyChanged("EmployeeImagePath"); }
        }

        private string _portalemployeeImagePath;
        public string PortalEmployeeImagePath
        {
            get { return _portalemployeeImagePath; }
            set { _portalemployeeImagePath = value; OnPropertyChanged("PortalEmployeeImagePath"); }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; OnPropertyChanged("ImagePath"); }
        }

        private string _portalimagePath;
        public string PortalImagePath
        {
            get { return _portalimagePath; }
            set { _portalimagePath = value; OnPropertyChanged("PortalImagePath"); }
        }

        private Image _Image;
        public Image Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged("Image"); }
        }
        public ICommand ImageDelete
        {
            get { return new RelayCommand(deleteImage); }
        }

        public ICommand ImageButton
        {
            get { return new RelayCommand(browseImage); }
        }

        void SaveImage()
        {
            string DirectoryPath = path + CurrentEmployee.employee_id + "\\";
            string PortalDirectoryPath = portalpath + "\\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (Directory.Exists(DirectoryPath) == false)
            {
                Directory.CreateDirectory(DirectoryPath);
                Image.Save(EmployeeImagePath, ImageFormat.Jpeg);
            }
            else
            {
                if (File.Exists(EmployeeImagePath))
                {
                    try
                    {
                        Image.Save(EmployeeImagePath, ImageFormat.Jpeg);
                    }
                    catch (Exception)
                    {
                        clsMessages.setMessage("Please Close the Application and Restart to Set the Image");
                    }
                }
                else
                {
                    Image.Save(EmployeeImagePath, ImageFormat.Jpeg);
                }
            }
            if (Directory.Exists(PortalDirectoryPath) == false)
            {
                Directory.CreateDirectory(PortalDirectoryPath);
                Image.Save(PortalEmployeeImagePath, ImageFormat.Jpeg);
            }
            else
            {
                if (File.Exists(PortalEmployeeImagePath))
                {
                    try
                    {
                        Image.Save(PortalEmployeeImagePath, ImageFormat.Jpeg);
                    }
                    catch (Exception)
                    {
                        clsMessages.setMessage("Please Close the Application and Restart to Set the Image");
                    }
                }
                else
                {
                    Image.Save(PortalEmployeeImagePath, ImageFormat.Jpeg);
                }
            }

            ImagePath = EmployeeImagePath;
            PortalImagePath = PortalEmployeeImagePath;
        }

        void deleteImage()
        {
            EmployeeImagePath = null;
            PortalEmployeeImagePath = null;
            ImagePath = null;
            PortalImagePath = null;
            PortalImageId = null;
            if (File.Exists(path + CurrentEmployee.employee_id))
            {
                File.Delete(path + CurrentEmployee.employee_id);
            }
            IsDelete = true;
            if (CurrentEmployee.employee_id.ToString() != null)
            {
                GoogleDriveModel.GetInstance.delete(CurrentEmployee.portalImage);
            }
            clsMessages.setMessage("Now Press Save button to Complete the Action");
        }

        void browseImage()
        {
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Image = new Bitmap(open.FileName);
                    ImagePath = open.FileName;
                    PortalImagePath = ImagePath;
                    //string imagename = open.SafeFileName;
                    ImageNameandPath();
                }
                catch (Exception)
                {
                }
            }
            open.Dispose();
        }

        void ImageNameandPath()
        {
            Guid FileName = new Guid();
            Guid PortalImageFileName = new Guid();
            FileName = Guid.NewGuid();
            PortalImageFileName = CurrentEmployee.employee_id;
            EmployeeImagePath = path + CurrentEmployee.employee_id + "\\" + FileName + ".Jpeg";
            PortalEmployeeImagePath = portalpath + PortalImageFileName + ".Jpeg";
        }

        #endregion

        #region Documents

        private List<string> _PathList;

        public List<string> PathList
        {
            get { return _PathList; }
            set { _PathList = value; OnPropertyChanged("PathList"); }
        }

        private IEnumerable<FileInfo> _FilePath;

        public IEnumerable<FileInfo> FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; OnPropertyChanged("FilePath"); }
        }

        private FileInfo _FileName;

        public FileInfo FileName
        {
            get { return _FileName; }
            set { _FileName = value; OnPropertyChanged("FileName"); }
        }

        public ICommand Browse
        {
            get { return new RelayCommand(openFile); }
        }

        public ICommand OpenADoc
        {
            get { return new RelayCommand(OpenADocument); }
        }

        void OpenADocument()
        {

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", FileName.FullName);
            }
            catch (Exception)
            {
                clsMessages.setMessage("No file selected");
            }
        }

        void loadGrid()
        {

            if (Directory.Exists(path + "Documents\\" + CurrentEmployee.employee_id + "\\") == true)
            {
                FilePath = new DirectoryInfo(path + "Documents\\" + CurrentEmployee.employee_id + "\\").GetFiles();
            }
        }

        void openFile()
        {
            if (CurrentEmployee != null && CurrentEmployee.employee_id != null)
            {
                string fileName = "";



                System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
                open.Filter = "pdf files (*.pdf) |*.pdf;";
                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {


                    string[] filename = (open.FileName).Split('\\');

                    foreach (var item in filename)
                    {
                        if (item.Contains(".pdf"))
                        {
                            fileName = item;
                            break;
                        }
                    }

                    if (Directory.Exists(path + "Documents\\" + CurrentEmployee.employee_id + "\\") == false)
                    {
                        Directory.CreateDirectory(path + "Documents\\" + CurrentEmployee.employee_id + "\\");

                        File.Copy(open.FileName, path + "Documents\\" + CurrentEmployee.employee_id + "\\" + fileName);
                    }
                    else
                    {
                        try
                        {
                            File.Copy(open.FileName, path + "Documents\\" + CurrentEmployee.employee_id + "\\" + fileName, true);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                loadGrid();
            }
        }

        #endregion

        #region Mihiri

        #region Extend Confirmation details

        #region Get data from service for confirmation

        private IEnumerable<OldConfirmationDate> _OldConfirmDetails;
        public IEnumerable<OldConfirmationDate> OldConfirmDetails
        {
            get { return _OldConfirmDetails; }
            set { _OldConfirmDetails = value; OnPropertyChanged("OldConfirmDetails"); }
        }

        #endregion

        #region Get Confirmation details

        private void refreshConfirmationDetail()
        {
            this.OldConfirmDetails = serviceClient.GetConfirmationDetails();
        }

        #endregion

        #endregion

        #endregion

    }
}