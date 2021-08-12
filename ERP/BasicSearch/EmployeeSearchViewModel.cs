using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Collections;

namespace ERP.BasicSearch
{
    public class EmployeeSearchViewModel : ViewModelBase
    {
        #region Service Object

        private ERPServiceClient serviceClient;

        #endregion

        #region Constructor

        public EmployeeSearchViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployee();
        }

        public EmployeeSearchViewModel(bool a)
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployee();
        }

        public EmployeeSearchViewModel(Guid PaymentID)
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployee(PaymentID);
        }

        public EmployeeSearchViewModel(List<Guid> empIDList)
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployee(empIDList);
        }


        public EmployeeSearchViewModel(IEnumerable<EmployeeSearchView> SelectedEmployees)
        {
            serviceClient = new ERPServiceClient();
            RefreshEmployees(SelectedEmployees);
        }

        #endregion

        #region List

        List<EmployeeSearchView> listEmployee = new List<EmployeeSearchView>();
        public List<EmployeeSearchView> selectEmployeeList = new List<EmployeeSearchView>();
        List<EmployeeSearchView> TempCompanyHierarchy = new List<EmployeeSearchView>();
        List<EmployeeSearchView> currentSearchedList = new List<EmployeeSearchView>();

        #region IList

        private IList _selectedModels = new ArrayList();

        #endregion

        #endregion

        #region Properties

        public IList TestSelected
        {
            get { return _selectedModels; }
            set
            {
                _selectedModels = value;
                OnPropertyChanged("TestSelected");
            }
        }

        private IList _selectedOtherGrid;
        public IList SelectedOtherGrid
        {
            get { return _selectedOtherGrid; }
            set { _selectedOtherGrid = value; OnPropertyChanged("SelectedOtherGrid"); }
        }

        private IEnumerable<EmployeeSearchView> _SelectedList;
        public IEnumerable<EmployeeSearchView> SelectedList
        {
            get { return _SelectedList; }
            set { _SelectedList = value; OnPropertyChanged("SelectedList"); }
        }

        private int _searchIndex;
        public int SearchIndex
        {
            get { return _searchIndex; }
            set { _searchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private IEnumerable<EmployeeSearchView> _employeeSearchView;
        public IEnumerable<EmployeeSearchView> EmployeeSearchViews
        {
            get { return _employeeSearchView; }
            set { _employeeSearchView = value; OnPropertyChanged("EmployeeSearchViews"); }
        }

        private EmployeeSearchView _currentEmployeeSearchView;
        public EmployeeSearchView CurrentEmployeeSearchView
        {
            get { return _currentEmployeeSearchView; }
            set { _currentEmployeeSearchView = value; OnPropertyChanged("CurrentEmployeeSearchView"); }
        }

        private IEnumerable<z_CompanyBranches> _Branches;
        public IEnumerable<z_CompanyBranches> Branches
        {
            get { return _Branches; }
            set { _Branches = value; OnPropertyChanged("Branches"); }
        }

        private z_CompanyBranches _CurrentBranche;
        public z_CompanyBranches CurrentBranch
        {
            get { return _CurrentBranche; }
            set
            {
                _CurrentBranche = value; OnPropertyChanged("CurrentBranch");
                SearchByBranch();
            }
        }

        private IEnumerable<z_Department> _Department;
        public IEnumerable<z_Department> Department
        {
            get { return _Department; }
            set { _Department = value; OnPropertyChanged("Department"); }
        }

        private z_Department _CurrentDepartment;
        public z_Department CurrentDepartment
        {
            get { return _CurrentDepartment; }
            set { _CurrentDepartment = value; OnPropertyChanged("CurrentDepartment"); SearchByDepartment(); }
        }

        private IEnumerable<z_Section> _Section;
        public IEnumerable<z_Section> Section
        {
            get { return _Section; }
            set { _Section = value; OnPropertyChanged("Section"); }
        }

        private z_Section _CurrentSection;
        public z_Section CurrentSection
        {
            get { return _CurrentSection; }
            set { _CurrentSection = value; OnPropertyChanged("CurrentSection"); SearchBySection(); }
        }

        private IEnumerable<z_Designation> _Designation;
        public IEnumerable<z_Designation> Designation
        {
            get { return _Designation; }
            set { _Designation = value; OnPropertyChanged("Designation"); }
        }

        private z_Designation _CurrentDesignation;
        public z_Designation CurrentDesignation
        {
            get { return _CurrentDesignation; }
            set { _CurrentDesignation = value; OnPropertyChanged("CurrentDesignation"); SearchByDesignation(); }
        }

        private IEnumerable<z_Grade> _Grades;
        public IEnumerable<z_Grade> Grades
        {
            get { return _Grades; }
            set { _Grades = value; OnPropertyChanged("Grades"); }
        }

        private z_Grade _CurrentGrade;
        public z_Grade CurrentGrade
        {
            get { return _CurrentGrade; }
            set { _CurrentGrade = value; OnPropertyChanged("CurrentGrade"); SearchByGrade(); }
        }

        private IEnumerable<z_City> _City;
        public IEnumerable<z_City> City
        {
            get { return _City; }
            set { _City = value; OnPropertyChanged("City"); }
        }

        private z_City _CurrentCity;
        public z_City CurrentCity
        {
            get { return _CurrentCity; }
            set { _CurrentCity = value; OnPropertyChanged("CurrentCity"); SearchByCity(); }
        }

        private IEnumerable<z_Town> _Town;
        public IEnumerable<z_Town> Town
        {
            get { return _Town; }
            set { _Town = value; OnPropertyChanged("Town"); }
        }

        private z_Town _CurrentTown;
        public z_Town CurrentTown
        {
            get { return _CurrentTown; }
            set { _CurrentTown = value; OnPropertyChanged("CurrentTown"); SearchByTown(); }
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
            set { _CurrentPaymentMethod = value; OnPropertyChanged("CurrentPaymentMethod"); SearchByPaymentMethod(); }
        }

        private IEnumerable<z_Religen> _Religion;
        public IEnumerable<z_Religen> Religion
        {
            get { return _Religion; }
            set { _Religion = value; OnPropertyChanged("Religion"); }
        }

        private z_Religen _CurrentReligion;
        public z_Religen CurrentReligion
        {
            get { return _CurrentReligion; }
            set { _CurrentReligion = value; OnPropertyChanged("CurrentReligion"); SearcByRelgion(); }
        }

        private IEnumerable<z_Gender> _Gender;
        public IEnumerable<z_Gender> Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        private z_Gender _CurrentGender;
        public z_Gender CurrentGender
        {
            get { return _CurrentGender; }
            set { _CurrentGender = value; OnPropertyChanged("CurrentGender"); SearchByGender(); }
        }

        #endregion

        #region Search Method & Property

        private string search;
        public string Search
        {
            get { return search; }
            set
            {
                search = value; OnPropertyChanged("Search");
                SearchTextChanged();
            }
        }

        private void SearchTextChanged()
        {
            EmployeeSearchViews = null;
            List<EmployeeSearchView> Temp = new List<EmployeeSearchView>();

            try
            {
                if (SearchIndex == 0)
                    Temp = listEmployee.Where(e => e.first_name != null && e.first_name.ToUpper().Contains(Search.ToString().ToUpper())).OrderBy(c => c.first_name).ToList();
                else if (SearchIndex == 1)
                    Temp = listEmployee.Where(e => e.surname != null && e.surname.ToUpper().Contains(Search.ToString().ToUpper())).OrderBy(c => c.surname).ToList();
                else if (SearchIndex == 2)
                    Temp = listEmployee.Where(e => e.second_name != null && e.second_name.ToUpper().Contains(Search.ToString().ToUpper())).OrderBy(c => c.second_name).ToList();
                else if (SearchIndex == 3)
                {
                    try
                    {
                        int x = int.Parse(Search);
                        Temp = listEmployee.Where(e => e.emp_id != null && e.emp_id.ToString().Contains(Search)).OrderBy(i => i.emp_id).ToList();
                    }
                    catch (Exception)
                    { }
                }
                else if (SearchIndex == 4)
                    Temp = listEmployee.Where(e => e.mobile != null && e.mobile.ToString().Contains(Search.ToString())).ToList();
                else if (SearchIndex == 5)
                    Temp = listEmployee.Where(i => i.companyBranch_Name != null && i.companyBranch_Name.ToString().ToUpper().Contains(Search.ToString().ToUpper())).ToList();
                else if (SearchIndex == 6)
                    Temp = listEmployee.Where(i => i.department_name != null && i.department_name.ToString().ToUpper().Contains(Search.ToString().ToUpper())).ToList();
                else if (SearchIndex == 7)
                    Temp = listEmployee.Where(i => i.section_id != null && i.section_id.ToString().ToUpper().Contains(Search.ToString().ToUpper())).ToList();
                else if (SearchIndex == 8)
                    Temp = listEmployee.Where(i => i.designation != null && i.designation.ToString().ToUpper().Contains(Search.ToString().ToUpper())).ToList();
                else
                    Temp = listEmployee.Where(i => i.grade != null && i.grade.ToString().ToUpper().Contains(Search.ToString().ToUpper())).ToList();
                EmployeeSearchViews = Temp;
            }
            catch (Exception)
            {
                EmployeeSearchViews = listEmployee;
            }
        }

        private void SearchByBranch()
        {
            if (CurrentBranch != null)
            {
                EmployeeSearchViews = listEmployee.Where(c => c.companyBranch_id != null && c.companyBranch_id == CurrentBranch.companyBranch_id);
                this.UpdateCurrentSearchedEmployees();
                CurrentSection = null;
                CurrentDepartment = null;
                Department = Department.Where(c => c.branch_id != null &&  c.branch_id == CurrentBranch.companyBranch_id);
                RefreshMore();
            }
        }

        private void SearchByDepartment()
        {

            if (CurrentDepartment != null)
            {
                EmployeeSearchViews = listEmployee.Where(c => c.department_id != null && c.department_id == CurrentDepartment.department_id);
                this.UpdateCurrentSearchedEmployees();
                Section = Section.Where(c => c.department_id != null && c.department_id == CurrentDepartment.department_id);
                CurrentSection = null;
                RefreshMore();

            }
        }

        private void SearchBySection()
        {
            if (CurrentSection != null)
            {
                EmployeeSearchViews = listEmployee.Where(c => c.section_id != null && c.section_id == CurrentSection.section_id);
                this.UpdateCurrentSearchedEmployees();
            }
                
            RefreshMore();
        }

        private void SearchByDesignation()
        {
            if (CurrentDesignation != null)
            {
                EmployeeSearchViews = EmployeeSearchViews.Where(c => c.designation_id != null && c.designation_id == CurrentDesignation.designation_id);
                this.UpdateCurrentSearchedEmployees();
            }

        }

        private void SearchByGrade()
        {
            if (CurrentGrade != null)
            {
                EmployeeSearchViews = EmployeeSearchViews.Where(c => c.grade_id != null && c.grade_id == CurrentGrade.grade_id);
                this.UpdateCurrentSearchedEmployees();
            }
        }

        private void SearchByCity()
        {
            if (CurrentCity != null)
            {
                EmployeeSearchViews = EmployeeSearchViews.Where(c => c.city_id != null && c.city_id == CurrentCity.city_id);
                this.UpdateCurrentSearchedEmployees();
            }
        }

        private void SearchByTown()
        {
            if (CurrentTown != null)
            {
                EmployeeSearchViews = EmployeeSearchViews.Where(c =>  c.town_id  != null && c.town_id == CurrentTown.city_id);
                this.UpdateCurrentSearchedEmployees();
            }
        }

        private void SearchByPaymentMethod()
        {
            if (CurrentPaymentMethod != null)
            {
                EmployeeSearchViews = EmployeeSearchViews.Where(c => c.paymet_method_id != null && c.paymet_method_id == CurrentPaymentMethod.paymet_method_id);
                this.UpdateCurrentSearchedEmployees();
            }
        }

        private void SearcByRelgion()
        {
            if (CurrentReligion != null)
            {
                EmployeeSearchViews = EmployeeSearchViews.Where(c => c.religen_id != null && c.religen_id == CurrentReligion.religen_id);
                this.UpdateCurrentSearchedEmployees();
            }
        }

        private void SearchByGender()
        {
            if (CurrentGender != null)
            {
                EmployeeSearchViews = EmployeeSearchViews.Where(c => c.religen_id != null && c.religen_id == CurrentReligion.religen_id);
                this.UpdateCurrentSearchedEmployees();
            }
        }

        #endregion

        #region Refresh methods

        void Refresh()
        {
            RefreshDepartment();
            RefreshSection();
            RefershBranch();
            RefreshGrade();
            RefreshDesignation();
            RefreshCity();
            RefreshTown();
            RefreshPaymentMethod();
            RefreshReligion();
        }

        void RefershBranch()
        {
            try
            {
                var ListBranch = (from es in listEmployee.Where(c => c.companyBranch_id != null).GroupBy(o => o.companyBranch_id).Select(x => x.First())
                                  select new z_CompanyBranches { companyBranch_id = (Guid)es.companyBranch_id, companyBranch_Name = es.companyBranch_Name });
                Branches = null;
                Branches = ListBranch.ToList();
            }
            catch (Exception)
            {
            }
        }

        void RefreshGrade()
        {
            try
            {
                var ListGrade = (from es in listEmployee.Where(c => c.grade_id != null).GroupBy(o => o.grade_id).Select(x => x.First())
                                 select new z_Grade { grade_id = (Guid)es.grade_id, grade = es.grade });
                Grades = null;
                Grades = ListGrade.ToList();
            }
            catch (Exception)
            {
            }
        }

        void RefreshDesignation()
        {
            try
            {
                var ListDesignation = (from es in listEmployee.Where(c => c.designation_id != null).GroupBy(o => o.designation_id).Select(x => x.First())
                                       select new z_Designation { designation_id = (Guid)es.designation_id, designation = es.designation });
                Designation = null;
                Designation = ListDesignation.ToList();
            }
            catch (Exception)
            {
            }
        }

        void RefreshCity()
        {
            try
            {
                var ListCity = (from es in listEmployee.Where(c => c.city_id != null).GroupBy(o => o.city_id).Select(x => x.First())
                                select new z_City { city_id = (Guid)es.city_id, city = es.city });
                City = null;
                City = ListCity.ToList();
            }
            catch (Exception)
            {

            }
        }

        void RefreshTown()
        {
            try
            {
                var ListTown = (from es in listEmployee.Where(c => c.town_id != null).GroupBy(o => o.town_id).Select(x => x.First())
                                select new z_Town { town_id = (Guid)es.town_id, city_id = es.city_id, town_name = es.town_name });
                Town = null;
                Town = ListTown.ToList();
            }
            catch (Exception)
            {
            }
        }

        void RefreshPaymentMethod()
        {
            try
            {
                var ListPayment = (from es in listEmployee.Where(c => c.paymet_method_id != null).GroupBy(o => o.paymet_method_id).Select(x => x.First())
                                   select new z_PaymentMethod { paymet_method_id = (Guid)es.paymet_method_id, payment_method = es.payment_method });
                PaymentMethod = null;
                PaymentMethod = ListPayment.ToList();
            }
            catch (Exception)
            {
            }
        }

        void RefreshReligion()
        {
            try
            {
                var ListReligion = (from es in listEmployee.Where(c => c.religen_id != null).GroupBy(o => o.religen_id).Select(x => x.First())
                                    select new z_Religen { religen_id = (Guid)es.religen_id, religen_name = es.religen_name });
                Religion = null;
                Religion = ListReligion.ToList();
            }
            catch (Exception)
            {
            }
        }

        void RefreshSection()
        {
            try
            {
                var ListSection = (from es in listEmployee.Where(c => c.section_id != null).GroupBy(o => o.section_id).Select(x => x.First())
                                   select new z_Section { section_id = (Guid)es.section_id, section_name = es.section_name, department_id = es.department_id });
                Section = null;
                Section = ListSection.ToList();
            }
            catch (Exception)
            {
            }
        }

        void RefreshDepartment()
        {
            try
            {
                var ListDepartments = (from es in listEmployee.Where(c => c.designation_id != null).GroupBy(o => o.department_id).Select(x => x.First())
                                       select new z_Department { department_id = (Guid)es.department_id, department_name = es.department_name, branch_id = es.companyBranch_id });
                Department = null;
                Department = ListDepartments.ToList();
            }
            catch (Exception)
            {
            }
        }

        void RefreshEmployee(Guid PaymentID)
        {
            try
            {
                listEmployee.Clear();
                serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
                {
                    EmployeeSearchViews = e.Result.Where(c => c.paymet_method_id == PaymentID).OrderBy(c => c.emp_id);
                    if (EmployeeSearchViews != null)
                    {
                        listEmployee = EmployeeSearchViews.ToList();
                        this.UpdateCurrentSearchedEmployees();
                        Refresh();
                    }
                };
                serviceClient.GetEmloyeeSearchAsync();

            }
            catch (Exception ex)
            { System.Windows.MessageBox.Show(ex.Message.ToString()); }
        }

        void RefreshEmployee()
        {
            try
            {
                listEmployee.Clear();
                serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
                    {
                        EmployeeSearchViews = e.Result.OrderBy(c => c.emp_id);
                        if (EmployeeSearchViews != null)
                        {
                            listEmployee = EmployeeSearchViews.ToList();
                            this.UpdateCurrentSearchedEmployees();
                            Refresh();
                        }
                    };
                serviceClient.GetEmloyeeSearchAsync();

            }
            catch (Exception ex)
            { System.Windows.MessageBox.Show(ex.Message.ToString()); }
        }

        void RefreshEmployee(List<Guid> selectedEmps)
        {
            try
            {
                listEmployee.Clear();
                serviceClient.GetFilteredEmployeeSearchCompleted += (s, e) =>
                {
                    EmployeeSearchViews = e.Result.OrderBy(c => c.emp_id);
                    if (EmployeeSearchViews != null)
                    {
                        listEmployee = EmployeeSearchViews.ToList();
                        this.UpdateCurrentSearchedEmployees();
                        Refresh();
                    }
                };
                serviceClient.GetFilteredEmployeeSearchAsync(selectedEmps.ToArray());

            }
            catch (Exception)
            {
                clsMessages.setMessage("Employee details refresh is failed");
            }
        }

        private void RefreshEmployees(IEnumerable<EmployeeSearchView> SelectedEmployees)
        {
            try
            {
                if (SelectedEmployees != null && SelectedEmployees.Count() > 0)
                {
                    EmployeeSearchViews = SelectedEmployees;
                    listEmployee = SelectedEmployees.ToList();
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Refresh More

        void RefreshMore() 
        {
            RefreshDesignation();
            RefreshPaymentMethod();
            RefreshReligion();
            RefreshGrade();
            RefreshReligion();
            RefreshCity();
            RefreshTown();
        }

        #endregion

        #region Refresh Method

        public ICommand RefreshButton
        {
            get { return new RelayCommand(RefreshMethod); }
        }

        void RefreshMethod()
        {
            EmployeeSearchViews = null;
            EmployeeSearchViews = listEmployee;
            Refresh();
        }

        #endregion

        #region Button Methods

        #region Add Button

        public ICommand AddButton
        {
            get { return new RelayCommand(Add); }
        }

        void Add()
        {
            try
            {
                foreach (var item in TestSelected)
                {
                    selectEmployeeList.Add((EmployeeSearchView)item);
                    listEmployee.Remove((EmployeeSearchView)item);
                }

                EmployeeSearchViews = null;
                SelectedList = null;
                SelectedList = selectEmployeeList.OrderBy(c => c.emp_id);
                EmployeeSearchViews = currentSearchedList.Where(c=> !selectEmployeeList.Any(d=>d.employee_id == c.employee_id)).OrderBy(c => c.emp_id);

            }
            catch (Exception)
            {
                clsMessages.setMessage("Please Select");
            }
        }

        #endregion

        #region Remove Button

        public ICommand RemoveButton
        {
            get { return new RelayCommand(Remove); }
        }

        void Remove()
        {
            try
            {
                foreach (var item in SelectedOtherGrid)
                {
                    selectEmployeeList.Remove((EmployeeSearchView)item);
                    listEmployee.Add((EmployeeSearchView)item);
                }
                EmployeeSearchViews = null;
                SelectedList = null;
                SelectedList = selectEmployeeList.OrderBy(c => c.emp_id);
                EmployeeSearchViews = currentSearchedList.Where(c=>!selectEmployeeList.Any(d=>d.employee_id == c.employee_id)).OrderBy(c => c.emp_id);
            }
            catch (Exception)
            {
                clsMessages.setMessage("Please Select");
            }

        }

        #endregion

        #endregion

        #region Current Search Changes

        void UpdateCurrentSearchedEmployees()
        {
            if (EmployeeSearchViews != null)
            {
                currentSearchedList = EmployeeSearchViews.ToList(); 
            }
        }

        #endregion

        #region Close

        private Visibility close;
        public Visibility Close
        {
            get { return close; }
            set { close = value; OnPropertyChanged("Close"); }
        }

        public void getClose()
        {
            Close = Visibility.Hidden;
        }

        public ICommand CloseButton
        {
            get { return new RelayCommand(getClose); }
        }

        #endregion
    }
}