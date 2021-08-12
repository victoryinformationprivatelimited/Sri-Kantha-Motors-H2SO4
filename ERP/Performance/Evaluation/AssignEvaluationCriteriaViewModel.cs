using ERP.BasicSearch;
using ERP.ERPService;
using ERP.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Performance.Evaluation
{
    class AssignEvaluationCriteriaViewModel : ViewModelBase
    {
        #region Service Client

        ERPServiceClient serviceClient;

        #endregion

        #region Constructor

        public AssignEvaluationCriteriaViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListEvaluationCriterias = new List<dtl_EvaluationCriteria>();
            ListSelectedEmployeeEvaluations = new List<EmployeeEvaluationsView>();
            ListSelectedEmployeeEvaluationDetails = new List<EmployeeEvaluationDetailsView>();

            RefreshEvaluationCategories();
            RefreshEvaluationPeriods();
            RefreshEvaluationCriterias();
            RefreshEmployeeSearch();
            ResfeshEmployees();
            RefreshSupervisor();
        }

        #endregion

        #region Lists

        List<dtl_EvaluationCriteria> ListEvaluationCriterias;
        List<EmployeeEvaluationsView> ListSelectedEmployeeEvaluations;
        List<EmployeeEvaluationDetailsView> ListSelectedEmployeeEvaluationDetails;

        #endregion

        #region Properties

        private IEnumerable<EmployeeEvaluationDetailsView> _SelectedEmployeeEvaluationDetails;
        public IEnumerable<EmployeeEvaluationDetailsView> SelectedEmployeeEvaluationDetails
        {
            get { return _SelectedEmployeeEvaluationDetails; }
            set { _SelectedEmployeeEvaluationDetails = value; OnPropertyChanged("SelectedEmployeeEvaluationDetails"); }
        }

        private IList _CurrentSelectedEmployeeEvaluationDetails = new ArrayList();
        public IList CurrentSelectedEmployeeEvaluationDetails
        {
            get { return _CurrentSelectedEmployeeEvaluationDetails; }
            set { _CurrentSelectedEmployeeEvaluationDetails = value; OnPropertyChanged("CurrentSelectedEmployeeEvaluationDetails"); }
        }


        private IEnumerable<EmployeeEvaluationsView> _SelectedEmployeeEvaluations;
        public IEnumerable<EmployeeEvaluationsView> SelectedEmployeeEvaluations
        {
            get { return _SelectedEmployeeEvaluations; }
            set { _SelectedEmployeeEvaluations = value; OnPropertyChanged("SelectedEmployeeEvaluations");}
        }

        private IList _CurrentSelectedEmployeeEvaluations = new ArrayList();
        public IList CurrentSelectedEmployeeEvaluations
        {
            get { return _CurrentSelectedEmployeeEvaluations; }
            set { _CurrentSelectedEmployeeEvaluations = value; OnPropertyChanged("CurrentSelectedEmployeeEvaluations"); if (ListSelectedEmployeeEvaluationDetails != null && ListSelectedEmployeeEvaluationDetails.Count > 0) FilterEmployeeEvaluationDetails(); }
        }

        private IEnumerable<EmployeeSupervisorsView> _Supervisors;
        public IEnumerable<EmployeeSupervisorsView> Supervisors
        {
            get { return _Supervisors; }
            set { _Supervisors = value; OnPropertyChanged("Supervisors"); }
        }

        private EmployeeSupervisorsView _CurrentSupervisor;
        public EmployeeSupervisorsView CurrentSupervisor
        {
            get { return _CurrentSupervisor; }
            set { _CurrentSupervisor = value; OnPropertyChanged("CurrentSupervisor"); }
        }

        private IEnumerable<EmployeeSearchView> _EmployeeSearch;
        public IEnumerable<EmployeeSearchView> EmployeeSearch
        {
            get { return _EmployeeSearch; }
            set { _EmployeeSearch = value; OnPropertyChanged("EmployeeSearch"); }
        }

        private IEnumerable<EmployeeSupervisorsByUserView> _Employees;
        public IEnumerable<EmployeeSupervisorsByUserView> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private IEnumerable<z_EvaluationCategory> _EvaluationCatrgories;
        public IEnumerable<z_EvaluationCategory> EvaluationCatrgories
        {
            get { return _EvaluationCatrgories; }
            set { _EvaluationCatrgories = value; OnPropertyChanged("EvaluationCatrgories"); }
        }

        private z_EvaluationCategory _CurrentEvaluationCategory;
        public z_EvaluationCategory CurrentEvaluationCategory
        {
            get { return _CurrentEvaluationCategory; }
            set { _CurrentEvaluationCategory = value; OnPropertyChanged("CurrentEvaluationCategory"); if (CurrentEvaluationCategory != null) FilterCriterias(); }
        }

        private IEnumerable<z_EvaluationPeriod> _EvaluationPeriods;
        public IEnumerable<z_EvaluationPeriod> EvaluationPeriods
        {
            get { return _EvaluationPeriods; }
            set { _EvaluationPeriods = value; OnPropertyChanged("EvaluationPeriods"); }
        }

        private z_EvaluationPeriod _CurrentEvaluationPeriod;
        public z_EvaluationPeriod CurrentEvaluationPeriod
        {
            get { return _CurrentEvaluationPeriod; }
            set { _CurrentEvaluationPeriod = value; OnPropertyChanged("CurrentEvaluationPeriod"); }
        }

        private IEnumerable<dtl_EvaluationCriteria> _EvaluationCriterias;
        public IEnumerable<dtl_EvaluationCriteria> EvaluationCriterias
        {
            get { return _EvaluationCriterias; }
            set { _EvaluationCriterias = value; OnPropertyChanged("EvaluationCriterias"); }
        }


        private IList _CurrentEvaluationCriteria = new ArrayList();
        public IList CurrentEvaluationCriteria
        {
            get { return _CurrentEvaluationCriteria; }
            set { _CurrentEvaluationCriteria = value; OnPropertyChanged("CurrentEvaluationCriteria"); }
        }

        private IEnumerable<AssignedEvaluationsView> _AssignedEvaluations;
        public IEnumerable<AssignedEvaluationsView> AssignedEvaluations
        {
            get { return _AssignedEvaluations; }
            set { _AssignedEvaluations = value; OnPropertyChanged("AssignedEvaluations"); }
        }


        private AssignedEvaluationsView _CurrentAssignedEvaluation;
        public AssignedEvaluationsView CurrentAssignedEvaluation
        {
            get { return _CurrentAssignedEvaluation; }
            set { _CurrentAssignedEvaluation = value; OnPropertyChanged("CurrentAssignedEvaluation"); if (CurrentAssignedEvaluation != null) FillSavedEvaluationDeatils(); }
        }

        #endregion

        #region Refresh Methods

        private void RefreshEvaluationCriterias()
        {
            serviceClient.GetEvaluationCriteriaCompleted += (s, e) =>
            {
                try
                {
                    ListEvaluationCriterias.Clear();
                    EvaluationCriterias = e.Result;
                    if (EvaluationCriterias != null && EvaluationCriterias.Count() > 0)
                        ListEvaluationCriterias = EvaluationCriterias.ToList();
                    EvaluationCriterias = null;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEvaluationCriteriaAsync();
        }

        private void RefreshEvaluationPeriods()
        {
            serviceClient.GetEvaluationPeriodsCompleted += (s, e) =>
            {
                try
                {
                    EvaluationPeriods = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEvaluationPeriodsAsync();
        }

        private void RefreshEvaluationCategories()
        {
            serviceClient.GetEvaluationCategoriesCompleted += (s, e) =>
            {
                try
                {
                    EvaluationCatrgories = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetEvaluationCategoriesAsync();
        }

        private void ResfeshEmployees()
        {
            serviceClient.GetEmployeesBySupervisorUserCompleted += (s, e) =>
            {
                try
                {
                    Employees = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("ResfeshEmployees() \n\n" + ex.Message);
                }
            };
            serviceClient.GetEmployeesBySupervisorUserAsync(clsSecurity.loggedUser.user_id, new Guid("69EE43C8-5A81-4BFC-8C58-0924F5812F6E"));
        }

        private void RefreshEmployeeSearch()
        {
            serviceClient.GetEmloyeeSearchCompleted += (s, e) =>
            {
                try
                {
                    EmployeeSearch = e.Result.OrderBy(c => c.emp_id);
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshEmployeeSearch() \n\n" + ex.Message);
                }

            };
            serviceClient.GetEmloyeeSearchAsync();
        }

        private void RefreshSupervisor()
        {
            serviceClient.GetAllEmployeeSupervisorCompleted += (s, e) =>
            {
                try
                {
                    Supervisors = e.Result;

                    if (Supervisors != null && Supervisors.Count() > 0)
                    {
                        if (clsSecurity.loggedEmployee != null)
                        {
                            CurrentSupervisor = Supervisors.FirstOrDefault(c => c.supervisor_employee_id == clsSecurity.loggedEmployee.employee_id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshSupervisor() \n\n" + ex.Message);
                }

            };
            serviceClient.GetAllEmployeeSupervisorAsync(clsSecurity.loggedUser.user_id);
        }

        private void RefreshAssignedEvaluations()
        {
            serviceClient.GetFilterdAssignedEvaluationsCompleted += (s, e) =>
            {
                try
                {
                    AssignedEvaluations = e.Result;
                    if (AssignedEvaluations == null || AssignedEvaluations.Count() == 0)
                        clsMessages.setMessage("Current supervisor doesn't have any evaluations under the selected categories");
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetFilterdAssignedEvaluationsAsync(CurrentEvaluationCategory.evaluation_cat_id, CurrentEvaluationPeriod.evaluation_period_id, (Guid)clsSecurity.loggedEmployee.employee_id);
        }

        private void RefreshEmployeeEvaluations()
        {
            serviceClient.GetFilteredEmployeeEvaluationsCompleted += (s, e) =>
            {
                try
                {
                    ListSelectedEmployeeEvaluations.Clear();
                    IEnumerable<EmployeeEvaluationsView> Temp = e.Result;
                    if (Temp != null)
                        ListSelectedEmployeeEvaluations = Temp.ToList();
                    Temp = null;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetFilteredEmployeeEvaluationsAsync(CurrentEvaluationCategory.evaluation_cat_id, CurrentEvaluationPeriod.evaluation_period_id, (Guid)clsSecurity.loggedEmployee.employee_id);
        }

        private void RefreshEmployeeEvaluationDetails()
        {
            serviceClient.GetFilteredEmployeeEvaluationDetailsCompleted += (s, e) =>
            {
                try
                {
                    ListSelectedEmployeeEvaluationDetails.Clear();
                    IEnumerable<EmployeeEvaluationDetailsView> Temp = e.Result;
                    if (Temp != null)
                        ListSelectedEmployeeEvaluationDetails = Temp.ToList();
                    Temp = null;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetFilteredEmployeeEvaluationDetailsAsync(CurrentEvaluationCategory.evaluation_cat_id, CurrentEvaluationPeriod.evaluation_period_id, (Guid)clsSecurity.loggedEmployee.employee_id);
        }

        #endregion

        #region Button Methods

        #region Select Employees

        public ICommand BtnSelectEmployees
        {
            get { return new RelayCommand(AddEmployees, AddEmployeesCE); }
        }

        private bool AddEmployeesCE()
        {
            if (CurrentEvaluationCategory == null || CurrentEvaluationPeriod == null)
                return false;
            else
                return true;
        }

        private void AddEmployees()
        {
            if (EmployeeSearch != null && EmployeeSearch.Count() > 0 && Employees != null && Employees.Count() > 0)
            {
                EmployeeSearch = EmployeeSearch.Where(c => Employees.Count(d => c.employee_id == d.employee_id) > 0);
                EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow(EmployeeSearch);
                window.ShowDialog();
                if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                {
                    foreach (var employee in window.viewModel.selectEmployeeList)
                    {
                        if (ListSelectedEmployeeEvaluations.Count(c => c.employee_id == employee.employee_id) == 0)
                        {
                            EmployeeEvaluationsView evaluation = new EmployeeEvaluationsView();
                            evaluation.evaluation_id = 0;
                            evaluation.evaluation_cat_id = CurrentEvaluationCategory.evaluation_cat_id;
                            evaluation.evaluation_period_id = CurrentEvaluationPeriod.evaluation_period_id;
                            evaluation.employee_id = employee.employee_id;
                            evaluation.emp_id = employee.emp_id;
                            evaluation.first_name = employee.first_name;
                            evaluation.is_active = true;
                            evaluation.is_delete = false;
                            ListSelectedEmployeeEvaluations.Add(evaluation);
                        }
                    }

                    SelectedEmployeeEvaluations = null;
                    SelectedEmployeeEvaluations = ListSelectedEmployeeEvaluations;
                }
            }
            else if (CurrentSupervisor == null)
                clsMessages.setMessage("Current user is not a Supervisor");
            else
                clsMessages.setMessage("There's no Employees under this Supervisor");
        }

        #endregion

        #region New

        public ICommand BtnNew
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            SelectedEmployeeEvaluations = null;
            ListSelectedEmployeeEvaluations.Clear();
            SelectedEmployeeEvaluationDetails = null;
            ListSelectedEmployeeEvaluationDetails.Clear();
            AssignedEvaluations = null;
        }

        #endregion

        #region Select Criterias

        public ICommand BtnSelectedCriterias
        {
            get { return new RelayCommand(SelectCriterias, SelectCriteriasCE); }
        }

        private bool SelectCriteriasCE()
        {
            if (CurrentEvaluationCriteria != null && CurrentEvaluationCriteria.Count > 0 && CurrentSelectedEmployeeEvaluations != null && CurrentSelectedEmployeeEvaluations.Count > 0)
                return true;
            else
                return false;
        }

        private void SelectCriterias()
        {
            foreach (EmployeeEvaluationsView employee in CurrentSelectedEmployeeEvaluations)
            {
                foreach (dtl_EvaluationCriteria criteria in CurrentEvaluationCriteria)
                {
                    if (ListSelectedEmployeeEvaluationDetails.Count(c => c.evaluation_criteria_id == criteria.evaluation_criteria_id && c.employee_id == employee.employee_id) == 0)
                    {
                        EmployeeEvaluationDetailsView CriteriaObj = new EmployeeEvaluationDetailsView();
                        CriteriaObj.evaluation_id = 0;
                        CriteriaObj.employee_id = employee.employee_id;
                        CriteriaObj.evaluation_cat_id = CurrentEvaluationCategory.evaluation_cat_id;
                        CriteriaObj.evaluation_period_id = CurrentEvaluationPeriod.evaluation_period_id;
                        CriteriaObj.evaluation_criteria_id = criteria.evaluation_criteria_id;
                        CriteriaObj.evaluation_criteria_name = criteria.evaluation_criteria_name;
                        CriteriaObj.is_active = true;
                        CriteriaObj.is_delete = false;
                        ListSelectedEmployeeEvaluationDetails.Add(CriteriaObj);
                    }
                }
            }
            SelectedEmployeeEvaluationDetails = null;
            SelectedEmployeeEvaluationDetails = ListSelectedEmployeeEvaluationDetails.Where(c => c.employee_id == ((EmployeeEvaluationsView)CurrentSelectedEmployeeEvaluations[0]).employee_id);
        }

        #endregion

        #region Save

        public ICommand BtnSave
        {
            get { return new RelayCommand(Save, SaveCE); }
        }

        private bool SaveCE()
        {
            if (CurrentEvaluationCriteria == null || CurrentEvaluationPeriod == null)
                return false;
            else
                return true;
        }

        private void Save()
        {
            if (ValidateSaveUpdate())
            {
                List<mas_Evaluation> MasSaveObjList = new List<mas_Evaluation>();

                foreach (var Employee in ListSelectedEmployeeEvaluations)
                {
                    mas_Evaluation MasSaveObj = new mas_Evaluation();
                    MasSaveObj.evaluation_id = Employee.evaluation_id;
                    MasSaveObj.evaluation_cat_id = Employee.evaluation_cat_id;
                    MasSaveObj.evaluation_period_id = Employee.evaluation_period_id;
                    MasSaveObj.employee_id = Employee.employee_id;
                    MasSaveObj.evaluation_supervisor = (Guid)clsSecurity.loggedEmployee.employee_id;
                    MasSaveObj.is_employee_finished = Employee.is_employee_finished;
                    MasSaveObj.is_manager_finished = Employee.is_manager_finished;
                    MasSaveObj.is_active = Employee.is_active;
                    MasSaveObj.is_delete = Employee.is_delete;
                    MasSaveObj.save_user_id = clsSecurity.loggedUser.user_id;
                    MasSaveObj.save_datetime = DateTime.Now;

                    List<dtl_Evaluation> DtlSaveObjList = new List<dtl_Evaluation>();

                    foreach (var Criteria in ListSelectedEmployeeEvaluationDetails.Where(c => c.employee_id == Employee.employee_id))
                    {
                        dtl_Evaluation dtlSaveObj = new dtl_Evaluation();
                        dtlSaveObj.evaluation_id = Employee.evaluation_id;
                        dtlSaveObj.evaluation_criteria_id = (int)Criteria.evaluation_criteria_id;
                        dtlSaveObj.employee_rate = Criteria.employee_rate;
                        dtlSaveObj.employee_response = Criteria.employee_response;
                        dtlSaveObj.manager_rate = Criteria.manager_rate;
                        dtlSaveObj.manager_response = Criteria.manager_response;
                        dtlSaveObj.is_active = Criteria.is_active;
                        dtlSaveObj.is_delete = Criteria.is_delete;
                        dtlSaveObj.save_user_id = clsSecurity.loggedUser.user_id;
                        dtlSaveObj.save_datetime = DateTime.Now;
                        DtlSaveObjList.Add(dtlSaveObj);
                    }

                    MasSaveObj.dtl_Evaluation = DtlSaveObjList.ToArray();
                    MasSaveObjList.Add(MasSaveObj);
                }

                if (serviceClient.SaveUpdateAssignEmployeeEvaluations(MasSaveObjList.ToArray()))
                {
                    New();
                    RefreshEmployeeEvaluationDetails();
                    RefreshEmployeeEvaluations();
                    RefreshAssignedEvaluations();
                    clsMessages.setMessage("Evaluation Assigned Successfully!");
                }
                else
                {
                    clsMessages.setMessage("Evaluation Assignment failed!");
                }

            }
        }

        #endregion

        #region Get Evaluations

        public ICommand BtnGetEvaluations
        {
            get { return new RelayCommand(GetEvaluations, GetEvaluationsCE); }
        }

        private bool GetEvaluationsCE()
        {
            if (CurrentEvaluationCriteria == null || CurrentEvaluationPeriod == null)
                return false;
            else
                return true;
        }

        private void GetEvaluations()
        {
            if (CurrentSupervisor != null)
            {
                New();
                RefreshEmployeeEvaluationDetails();
                RefreshEmployeeEvaluations();
                RefreshAssignedEvaluations();
            }
            else
                clsMessages.setMessage("Current user is not a supervisor");
        }

        #endregion

        #region Delete Activate Employees

        public ICommand BtnActInactEmployee 
        {
            get { return new RelayCommand(ActInactEmployees); }
        }

        private void ActInactEmployees()
        {
            if (CurrentSelectedEmployeeEvaluations != null && CurrentSelectedEmployeeEvaluations.Count > 0) 
            {
                int FailCount = 0;

                foreach (EmployeeEvaluationsView employee  in CurrentSelectedEmployeeEvaluations)
                {
                    if (employee.is_employee_finished == false && employee.is_manager_finished == false)
                    {
                        if (employee.evaluation_id == 0)
                        {
                            employee.is_active = employee.is_delete == true ? false : (employee.is_active == true ? false : true);
                        }
                        else
                        {
                            employee.is_active = employee.is_delete == true ? false : (employee.is_active == true ? false : true);
                        }

                        if (employee.is_active == false && (ListSelectedEmployeeEvaluationDetails != null && ListSelectedEmployeeEvaluationDetails.Count(c => c.employee_id == employee.employee_id && c.evaluation_id == employee.evaluation_id) > 0))
                        {
                            foreach (EmployeeEvaluationDetailsView criteria in ListSelectedEmployeeEvaluationDetails.Where(c => c.employee_id == employee.employee_id && c.evaluation_id == employee.evaluation_id))
                            {
                                criteria.is_active = employee.is_delete == true ? false : (criteria.is_active == true ? false : false);
                                criteria.is_delete = employee.is_delete == true ? true : false;
                            }
                        }

                        else if (employee.is_active == true && (ListSelectedEmployeeEvaluationDetails != null && ListSelectedEmployeeEvaluationDetails.Count(c => c.employee_id == employee.employee_id && c.evaluation_id == employee.evaluation_id && c.is_delete == true) > 0))
                        {
                            foreach (EmployeeEvaluationDetailsView criteria in ListSelectedEmployeeEvaluationDetails.Where(c => c.employee_id == employee.employee_id && c.evaluation_id == employee.evaluation_id && c.is_delete == true))
                            {
                                criteria.is_active = false ;
                                criteria.is_delete = false ;
                            }
                        }
                    }
                    else
                        FailCount++;
                }

                if (FailCount > 0)
                    clsMessages.setMessage("Some evaluations was not modified since they have been finished");
            }
        }


        public ICommand BtnDelUnDelEmployee 
        {
            get { return new RelayCommand(DelUnDelEmployee); }
        }

        private void DelUnDelEmployee()
        {
            int FailCount = 0;

            if (CurrentSelectedEmployeeEvaluations != null && CurrentSelectedEmployeeEvaluations.Count > 0)
            {
                foreach (EmployeeEvaluationsView employee in CurrentSelectedEmployeeEvaluations)
                {
                    if (employee.is_employee_finished == false && employee.is_manager_finished == false)
                    {
                        if (employee.evaluation_id == 0)
                        {
                            employee.is_delete = employee.is_delete == false ? true : false;
                            employee.is_active = employee.is_delete == true ? false : (employee.is_active == false ? true : true);
                        }
                        else
                        {
                            employee.is_delete = employee.is_delete == false ? true : false;
                            employee.is_active = employee.is_delete == true ? false : (employee.is_active == false ? false : true);
                        }

                        if (employee.is_delete == true && (ListSelectedEmployeeEvaluationDetails != null && ListSelectedEmployeeEvaluationDetails.Count(c => c.employee_id == employee.employee_id && c.evaluation_id == employee.evaluation_id) > 0))
                        {
                            foreach (EmployeeEvaluationDetailsView criteria in ListSelectedEmployeeEvaluationDetails.Where(c => c.employee_id == employee.employee_id && c.evaluation_id == employee.evaluation_id))
                            {
                                criteria.is_delete = true;
                                criteria.is_active = false;
                            }
                        }

                        else if (employee.is_delete == false && (ListSelectedEmployeeEvaluationDetails != null && ListSelectedEmployeeEvaluationDetails.Count(c => c.employee_id == employee.employee_id && c.evaluation_id == employee.evaluation_id && c.is_delete == true) > 0))
                        {
                            foreach (EmployeeEvaluationDetailsView criteria in ListSelectedEmployeeEvaluationDetails.Where(c => c.employee_id == employee.employee_id && c.evaluation_id == employee.evaluation_id && c.is_delete == true))
                            {
                                criteria.is_delete = false;
                                criteria.is_active = false;
                            }
                        }
                    }
                    else
                        FailCount++;
                } 
            }

            if (FailCount > 0)
                clsMessages.setMessage("Some evaluations was not modified since they have been finished");
        }
        
        #endregion

        #region Delete Activate Criterias

        public ICommand BtnActInactCriteria 
        {
            get { return new RelayCommand(ActInactCriteria); }
        }

        private void ActInactCriteria()
        {
            if (CurrentSelectedEmployeeEvaluationDetails != null && CurrentSelectedEmployeeEvaluationDetails.Count > 0) 
            {
                EmployeeEvaluationsView employee = (CurrentSelectedEmployeeEvaluations[0] as EmployeeEvaluationsView);

                foreach (EmployeeEvaluationDetailsView criteria in CurrentSelectedEmployeeEvaluationDetails)
                {
                    if (employee.is_employee_finished == false && employee.is_manager_finished == false) 
                    {
                        if (employee.is_active != false && employee.is_delete != true) 
                        {
                            criteria.is_active = criteria.is_delete == true ? false : (criteria.is_active == true ? false : true);
                        }
                    }
                }
            }
        }

        public ICommand BtnDelUnDelCriteria 
        {
            get { return new RelayCommand(DelUnDelCriteria); }
        }

        private void DelUnDelCriteria()
        {
            if (CurrentSelectedEmployeeEvaluationDetails != null && CurrentSelectedEmployeeEvaluationDetails.Count > 0)
            {
                EmployeeEvaluationsView employee = (CurrentSelectedEmployeeEvaluations[0] as EmployeeEvaluationsView);

                foreach (EmployeeEvaluationDetailsView criteria in CurrentSelectedEmployeeEvaluationDetails)
                {
                    if (employee.is_employee_finished == false && employee.is_manager_finished == false)
                    {
                        if (employee.is_active != false && employee.is_delete != true)
                        {
                            criteria.is_delete = employee.is_delete == true ? true : (criteria.is_delete == false ? true : false);
                            criteria.is_active = criteria.is_delete == true ? false : false;
                        }
                    }
                }
            }
        }
        
        #endregion

        #region Delete Evaluation

        public ICommand BtnDelete 
        {
            get { return new RelayCommand(DeleteEvaluation, DeleteEvaluationCE); }
        }

        private bool DeleteEvaluationCE()
        {
            if (AssignedEvaluations == null || AssignedEvaluations.Count() == 0 || CurrentAssignedEvaluation == null)
                return false;
            else
                return true;
        }

        private void DeleteEvaluation()
        {
            clsMessages.setMessage("All the employee evaluations under this evaluation will be deleted \n do you want to proceed?", Visibility.Visible);
            if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
            {
                if (ValidateDeleteEvaluations())
                {
                    List<mas_Evaluation> MasDelObjList = new List<mas_Evaluation>();

                    foreach (var Employee in ListSelectedEmployeeEvaluations.Where(c => c.evaluation_id != 0))
                    {
                        mas_Evaluation MasDelObj = new mas_Evaluation();
                        MasDelObj.employee_id = Employee.employee_id;
                        MasDelObj.evaluation_id = Employee.evaluation_id;
                        MasDelObj.is_active = false;
                        MasDelObj.is_delete = true;
                        MasDelObj.delete_user_id = clsSecurity.loggedUser.user_id;
                        MasDelObj.delete_datetime = DateTime.Now;

                        List<dtl_Evaluation> DtlSaveObjList = new List<dtl_Evaluation>();

                        foreach (var Criteria in ListSelectedEmployeeEvaluationDetails.Where(c => c.employee_id == MasDelObj.employee_id))
                        {
                            dtl_Evaluation dtlSaveObj = new dtl_Evaluation();
                            dtlSaveObj.evaluation_id = MasDelObj.evaluation_id;
                            dtlSaveObj.evaluation_criteria_id = Criteria.evaluation_criteria_id;
                            DtlSaveObjList.Add(dtlSaveObj);
                        }

                        MasDelObj.dtl_Evaluation = DtlSaveObjList.ToArray();
                        MasDelObjList.Add(MasDelObj);
                    }

                    if (serviceClient.DeleteAssignedEvaluations(MasDelObjList.ToArray()))
                    {
                        clsMessages.setMessage("Evaluation Deleted Successfully!");
                        New();
                        RefreshEmployeeEvaluationDetails();
                        RefreshEmployeeEvaluations();
                        RefreshAssignedEvaluations();
                    }
                    else
                    {
                        clsMessages.setMessage("Evaluation Delete failed!");
                    }
                } 
            }
        }

        #endregion

        #endregion

        #region Validation Methods

        private bool ValidateDeleteEvaluations()
        {
            if (ListSelectedEmployeeEvaluations == null || ListSelectedEmployeeEvaluations.Count(c => c.evaluation_id != 0) == 0)
            {
                clsMessages.setMessage("No saved employees found under this Evaluation");
                return false;
            }
            else if (ListSelectedEmployeeEvaluationDetails != null && ListSelectedEmployeeEvaluationDetails.Count(c => c.evaluation_id != 0) == 0)
            {
                clsMessages.setMessage("No saved criterias found under this Evaluation");
                return false;
            }
            else
                return true;
        }

        private bool ValidateSaveUpdate()
        {
            if (CurrentSupervisor == null)
            {
                clsMessages.setMessage("Only a supervisor can perform this task");
                return false;
            }
            if (ListSelectedEmployeeEvaluations == null || ListSelectedEmployeeEvaluations.Count == 0)
            {
                clsMessages.setMessage("Please select employees and try again");
                return false;
            }

            else if (ListSelectedEmployeeEvaluationDetails == null || ListSelectedEmployeeEvaluationDetails.Count == 0)
            {
                clsMessages.setMessage("Please select criterias and try again");
                return false;
            }

            else if (ListSelectedEmployeeEvaluations != null && ListSelectedEmployeeEvaluations.Count != 0)
            {
                bool status = true;

                foreach (var item in ListSelectedEmployeeEvaluations)
                {
                    if (ListSelectedEmployeeEvaluationDetails.Count(c => c.employee_id == item.employee_id) == 0)
                    {
                        clsMessages.setMessage("Employees without any criterias exist, Please check and try again");
                        status = false;
                        break;
                    }
                }

                return status;
            }
            else
                return true;
        }

        #endregion

        #region FilterMethods

        private void FilterCriterias()
        {
            EvaluationCriterias = null;
            EvaluationCriterias = ListEvaluationCriterias.Where(c => c.evaluation_cat_id == CurrentEvaluationCategory.evaluation_cat_id);
            New();
        }

        private void FilterEmployeeEvaluationDetails()
        {

            if (CurrentSelectedEmployeeEvaluations != null && CurrentSelectedEmployeeEvaluations.Count > 0)
            {
                SelectedEmployeeEvaluationDetails = null;
                SelectedEmployeeEvaluationDetails = ListSelectedEmployeeEvaluationDetails.Where(c => c.employee_id == ((EmployeeEvaluationsView)CurrentSelectedEmployeeEvaluations[0]).employee_id);
            }
        }

        private void FillSavedEvaluationDeatils()
        {
            SelectedEmployeeEvaluations = null;
            SelectedEmployeeEvaluationDetails = null;
            EvaluationCriterias = null;
            EvaluationCriterias = ListEvaluationCriterias.Where(c => c.evaluation_cat_id == CurrentAssignedEvaluation.evaluation_cat_id);
            SelectedEmployeeEvaluations = ListSelectedEmployeeEvaluations;
        }


        #endregion

    }
}
