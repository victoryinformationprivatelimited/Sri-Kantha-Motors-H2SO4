using ERP.ERPService;
using ERP.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Medical
{
    class MedicalDetailsViewModel : ViewModelBase
    {
        #region Service Object

        private ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion Service Object

        #region Constructor

        public MedicalDetailsViewModel()
        {
            RefreshHealthType();
            //refreshBloodTypes();
            refreshEmployeeBloodType();
            refreshMedicalPeriods();
            refreshEmployees();
            refreshEmployeeAllocations();
            Editable = true;
            this.New();
        }

        public MedicalDetailsViewModel(EmployeeSumarryView CurrentEmployee)
        {
            ConstructedEmployeeSumarry = CurrentEmployee;
            //refreshBloodTypes();
            RefreshHealthType();
            refreshEmployeeBloodType();
            refreshMedicalPeriods();
            refreshEmployees();
            refreshEmployeeAllocations();
            Editable = false;
            
            this.New();
            this.CurrentEmployee = CurrentEmployee;
        }

        private bool _Editable;
        public bool Editable
        {
            get { return _Editable; }
            set { _Editable = value; OnPropertyChanged("Editable"); }
        }

        #endregion Constructor

        #region Properties
        private EmployeeSumarryView ConstructedEmployeeSumarry;


        private IEnumerable<EmployeeSumarryView> _Employees;
        public IEnumerable<EmployeeSumarryView> Employees
        {
            get { return _Employees; }
            set
            {
                _Employees = value;
                OnPropertyChanged("Employees");

                if (Employees != null && Editable)
                {
                    this.CurrentEmployee = Employees.FirstOrDefault();
                }
                if (!Editable)
                {
                    this.CurrentEmployee = ConstructedEmployeeSumarry;
                }
            }
        }

        private EmployeeSumarryView _CurrentEmployee;
        public EmployeeSumarryView CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set
            {
                _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee");
                refreshAllocationsGrid();
            }
        }

        //private IEnumerable<z_blood_type> _BloodTypes;
        //public IEnumerable<z_blood_type> BloodTypes
        //{
        //    get
        //    {
        //        return this._BloodTypes;
        //    }
        //    set
        //    {
        //        this._BloodTypes = value;
        //        this.OnPropertyChanged("BloodTypes");

        //        if (BloodTypes != null)
        //        {
        //            this.SelectedBloodType = BloodTypes.FirstOrDefault();
        //        }
        //    }
        //}

        //private z_blood_type _SelectedBloodType;
        //public z_blood_type SelectedBloodType
        //{
        //    get
        //    {
        //        return this._SelectedBloodType;
        //    }
        //    set
        //    {
        //        this._SelectedBloodType = value;
        //        this.OnPropertyChanged("SelectedBloodType");
        //    }
        //}

        private IEnumerable<dtl_blood_type> _EmployeeBloodTypes;
        public IEnumerable<dtl_blood_type> EmployeeBloodTypes
        {
            get
            {
                return this._EmployeeBloodTypes;
            }
            set
            {
                this._EmployeeBloodTypes = value;
                this.OnPropertyChanged("EmployeeBloodTypes");
            }
        }

        private dtl_blood_type _CurrentEmployeeBloodType;
        public dtl_blood_type CurrentEmployeeBloodType
        {
            get
            {
                return this._CurrentEmployeeBloodType;
            }
            set
            {
                this._CurrentEmployeeBloodType = value;
                this.OnPropertyChanged("CurrentEmployeeBloodType");

                if (CurrentEmployeeBloodType != null)
                {
                    CurrentBloodGroupType = BloodGroupType.Where(o => o.bloodGroupType_id == CurrentEmployeeBloodType.blood_id).FirstOrDefault();
                    BloodDescription = CurrentEmployeeBloodType.description;
                }
            }
        }

        private string _BloodDescription;
        public string BloodDescription
        {
            get
            {
                return this._BloodDescription;
            }
            set
            {
                this._BloodDescription = value;
                this.OnPropertyChanged("BloodDescription");
            }
        }

        private IEnumerable<z_MedicalPeriod> _MedicalPeriods;
        public IEnumerable<z_MedicalPeriod> MedicalPeriods
        {
            get
            {
                return this._MedicalPeriods;
            }
            set
            {
                this._MedicalPeriods = value;
                this.OnPropertyChanged("MedicalPeriods");
            }
        }

        
        private z_MedicalPeriod _SelectedMedicalPeriod;
        public z_MedicalPeriod SelectedMedicalPeriod
        {
            get
            {
                return this._SelectedMedicalPeriod;
            }
            set
            {
                this._SelectedMedicalPeriod = value;
                this.OnPropertyChanged("SelectedMedicalPeriod");
                refreshMedicalCategories();
            }
        }

        private IEnumerable<z_MedicalCategory> _MedicalCategories;
        public IEnumerable<z_MedicalCategory> MedicalCategories
        {
            get
            {
                return this._MedicalCategories;
            }
            set
            {
                this._MedicalCategories = value;
                this.OnPropertyChanged("MedicalCategories");
            }
        }

        private z_MedicalCategory _SelectedMedicalCategory;
        public z_MedicalCategory SelectedMedicalCategory
        {
            get
            {
                return this._SelectedMedicalCategory;
            }
            set
            {
                this._SelectedMedicalCategory = value;
                this.OnPropertyChanged("SelectedMedicalCategory");
            }
        }

        private IEnumerable<EmployeeMedicalAllocationView> _CategoryAllocations;
        public IEnumerable<EmployeeMedicalAllocationView> CategoryAllocations
        {
            get
            {
                return this._CategoryAllocations;
            }
            set
            {
                this._CategoryAllocations = value;
                this.OnPropertyChanged("CategoryAllocations");
            }
        }

        private IEnumerable<EmployeeMedicalAllocationView> _SelectedEmployeesCategoryAllocations;
        public IEnumerable<EmployeeMedicalAllocationView> SelectedEmployeesCategoryAllocations
        {
            get
            {
                return this._SelectedEmployeesCategoryAllocations;
            }
            set
            {
                this._SelectedEmployeesCategoryAllocations = value;
                this.OnPropertyChanged("SelectedEmployeesCategoryAllocations");
            }
        }

        private dtl_MedicalCategoryAllocation _SelectedCategoryAllocation;
        public dtl_MedicalCategoryAllocation SelectedCategoryAllocation
        {
            get
            {
                return this._SelectedCategoryAllocation;
            }
            set
            {
                this._SelectedCategoryAllocation = value;
                this.OnPropertyChanged("SelectedCategoryAllocation");
            }
        }

        private bool _AllocEdit;
        public bool AllocEdit
        {
            get
            {
                return this._AllocEdit;
            }
            set
            {
                this._AllocEdit = value;
                this.OnPropertyChanged("AllocEdit");
            }
        }

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

        #endregion Properties

        #region NewButton Class & Property

        bool newCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        #endregion

        #region New Method

        private void New()
        {
            AllocEdit = false;            
            CurrentBloodGroupType = null;
        }

        #endregion New Method

        #region Save Method

        void Save()
        {
            bool isUpdateBlood = false;
            bool isUpdateAlloc = false;

            dtl_blood_type newBloodDtl = new dtl_blood_type();
            newBloodDtl.employee_id = CurrentEmployee.employee_id;
            newBloodDtl.blood_id = CurrentBloodGroupType.bloodGroupType_id;
            newBloodDtl.description = BloodDescription;

            IEnumerable<dtl_blood_type> allBlood = this.serviceClient.GetBlood();

            if (allBlood != null)
            {
                foreach (var bloodDt in allBlood)
                {
                    if (CurrentEmployee.employee_id == bloodDt.employee_id)
                    {
                        isUpdateBlood = true;
                        break;
                    }
                }
            }

            if (newBloodDtl != null)
            {
                if (isUpdateBlood)
                {
                    //if ()//clsSecurity.GetPermissionForUpdate(clsConfig.GetViewModelId(Viewmodels.MedicalReimbursementPeriod), clsSecurity.loggedUser.user_id))
                    //{
                    if (this.serviceClient.bloodUpdate(newBloodDtl))
                    {
                        clsMessages.setMessage(Properties.Resources.UpdateSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.UpdateFail);
                    }
                    //}
                    //else
                    //{
                    //    clsMessages.setMessage(Properties.Resources.NoPermissionForUpdate);
                    //}
                }
                else
                {
                    //newMedicalPeriod.is_active = (CurrentMedicalPeriod.is_active == null) ? false : CurrentMedicalPeriod.is_active;
                    //newMedicalPeriod.is_delete = false;

                    //if (true)//clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.MedicalReimbursementPeriod), clsSecurity.loggedUser.user_id))
                    //{
                    if (this.serviceClient.bloodSave(newBloodDtl))
                    {
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.SaveFail);
                    }
                    //}
                    //else
                    //{
                    //    clsMessages.setMessage(Properties.Resources.NoPermissionForSave);
                    //}
                }

                if (MessageBoxResult.Yes == MessageBox.Show("Are you sure you want to modify the Medical Reimbursement details.", "ERP Says.", MessageBoxButton.YesNo))
                {
                    AllocEdit = true;

                    if (CurrentEmployee != null && SelectedMedicalPeriod != null && SelectedMedicalCategory != null)
                    {
                        dtl_MedicalCategoryAllocation newAllocDtl = new dtl_MedicalCategoryAllocation();
                        newAllocDtl.employee_id = CurrentEmployee.employee_id;
                        newAllocDtl.period_id = SelectedMedicalPeriod.period_id;
                        newAllocDtl.cat_id = SelectedMedicalCategory.cat_id;
                        newAllocDtl.is_active = true;
                        newAllocDtl.is_delete = false;

                        IEnumerable<dtl_MedicalCategoryAllocation> allAllocation = this.serviceClient.GetEmployeeMedicalAllocations().
                            Where(o => o.employee_id == CurrentEmployee.employee_id).AsEnumerable();

                        if (allAllocation != null)
                        {
                            foreach (var alloc in allAllocation)
                            {
                                if (CurrentEmployee.employee_id == newAllocDtl.employee_id &&
                                    alloc.period_id == newAllocDtl.period_id)
                                {
                                    isUpdateAlloc = true;
                                    break;
                                }
                            }
                        }

                        if (newAllocDtl != null)
                        {
                            if (isUpdateAlloc)
                            {
                                newAllocDtl.modified_datetime = System.DateTime.Now;
                                newAllocDtl.modified_user_id = clsSecurity.loggedUser.user_id;

                                bool isValid = false;

                                if (SelectedMedicalPeriod.from_date < DateTime.Now && DateTime.Now < SelectedMedicalPeriod.to_date)
                                {
                                    isValid = false;
                                }

                                if (SelectedMedicalPeriod.to_date < DateTime.Now || isValid)
                                {
                                    //allAllocation.Where(o => o.cat_id == newAllocDtl.cat_id).FirstOrDefault();

                                    if (clsSecurity.GetUpdatePermission(703))
                                    {
                                        if (this.serviceClient.UpdateMedicalCategoryAllocation(newAllocDtl))
                                        {
                                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                                        }
                                        else
                                        {
                                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                                        }
                                    }
                                    else
                                        clsMessages.setMessage("You Dont HAve Permission to Update in this Form...");
                                }
                                else
                                {
                                    clsMessages.setMessageWithCaption("Cannot proceed the operation. Try Again");
                                }

                            }
                            else
                            {
                                if (SelectedMedicalPeriod.to_date > DateTime.Now)
                                {
                                    allAllocation = this.serviceClient.GetEmployeeMedicalAllocations().
                                        Where(o => o.employee_id == CurrentEmployee.employee_id && o.period_id == newAllocDtl.period_id).AsEnumerable();

                                    if (allAllocation.Count() == 0)
                                    {
                                        if (clsSecurity.GetSavePermission(703))
                                        {
                                            if (this.serviceClient.SaveMedicalCategoryAllocation(newAllocDtl))
                                            {
                                                clsMessages.setMessage(Properties.Resources.SaveSucess);
                                                RefreshHealthType();
                                                refreshEmployeeBloodType();
                                                refreshMedicalPeriods();
                                                refreshEmployees();
                                                refreshEmployeeAllocations();
                                                Editable = true;
                                                this.New();
                                            }
                                            else
                                            {
                                                clsMessages.setMessage(Properties.Resources.SaveFail);
                                            }
                                            
                                        }
                                        else
                                            clsMessages.setMessage("You Don't have Permission to Save in this Form...");
                                    }
                                    else
                                    {
                                        clsMessages.setMessageWithCaption("Allocation already exists");
                                    }
                                }
                                else
                                {
                                    clsMessages.setMessageWithCaption("Cannot proceed the operation. Try Again");
                                }
                            }
                        }
                    }//
                    //refreshMedicalPeriods();
                   
                }
            }
        }

        #endregion

        #region SaveButton Class & Property

        bool saveCanExecute()
        {
            if (CurrentEmployee == null)
                return false;
            if (CurrentBloodGroupType == null)
                return false;
            if (AllocEdit)
            {
                if (SelectedCategoryAllocation == null)
                {
                    AllocEdit = false;
                    return false;
                }
                if (SelectedCategoryAllocation == null)
                {
                    AllocEdit = false;
                    return false;
                }
                if (SelectedMedicalPeriod == null)
                {
                    AllocEdit = false;
                    return false;
                }
                if (SelectedMedicalCategory == null)
                {
                    AllocEdit = false;
                    return false;
                }
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

        #region Refresh Methods

        private void refreshEmployees()
        {
            this.serviceClient.GetAllEmployeeDetailCompleted += (s, e) =>
            {
                this.Employees = e.Result;

            };
            this.serviceClient.GetAllEmployeeDetailAsync();
        }

        //private void refreshBloodTypes()
        //{
        //    this.BloodTypes = this.serviceClient.GetBloodTypes();
        //}

        private void refreshEmployeeBloodType()
        {
            //this.serviceClient.GetBloodCompleted += (s, e) =>
            //{
            this.EmployeeBloodTypes = serviceClient.GetBlood();//e.Result;
            //};
            //this.serviceClient.GetBloodAsync();
        }

        private void refreshBloodDetails()
        {
            if (CurrentEmployee != null && BloodGroupType != null)
            {
                this.CurrentEmployeeBloodType = EmployeeBloodTypes.Where(o => o.employee_id == CurrentEmployee.employee_id).FirstOrDefault();

                if (CurrentEmployeeBloodType != null)
                {
                    CurrentBloodGroupType = BloodGroupType.Where(o => o.bloodGroupType_id == CurrentEmployeeBloodType.blood_id).FirstOrDefault();
                }
                else
                {
                    this.CurrentEmployeeBloodType = new dtl_blood_type();
                }
            }
            else
            {
                this.CurrentEmployeeBloodType = new dtl_blood_type();
            }

        }

        private void refreshMedicalPeriods()
        {
            this.serviceClient.GetMedicalPeriodsCompleted += (s, e) =>
            {
                this.MedicalPeriods = e.Result.Where(p => p.is_delete == false && p.is_active == true).OrderBy(p => p.from_date).ToList();
            };
            this.serviceClient.GetMedicalPeriodsAsync();


        }

        private void refreshMedicalCategories()
        {
            if (SelectedMedicalPeriod != null)
            {
                MedicalCategories = serviceClient.GetMedicalCategoriesByPeriodId(SelectedMedicalPeriod.period_id);
            }
        }

        private void refreshEmployeeAllocations()
        {
            this.CategoryAllocations = serviceClient.GetMedicalCategoryAllocationViewAll();
        }

        private void refreshAllocationsGrid()
        {
            if (CurrentEmployee != null && CategoryAllocations != null)
            {
                SelectedEmployeesCategoryAllocations = CategoryAllocations.Where(o => o.employee_id == CurrentEmployee.employee_id).ToArray();

                DateTime date = System.DateTime.Now;

                if (SelectedEmployeesCategoryAllocations.Count() > 0)
                {

                    foreach (var alloc in SelectedEmployeesCategoryAllocations)
                    {
                        if (date >= alloc.from_date && date <= alloc.to_date)
                        {
                            if (MedicalPeriods != null)
                            {
                                SelectedMedicalPeriod = MedicalPeriods.Where(o => o.period_id == alloc.period_id).FirstOrDefault();
                                SelectedMedicalCategory = MedicalCategories.Where(o => o.cat_id == alloc.cat_id).FirstOrDefault();
                                break;
                            }
                        }

                    }
                }//

                else
                {
                    SelectedMedicalPeriod = null;
                    SelectedMedicalCategory = null;

                    //MessageBox.Show("No active allocation for selected employee");
                }


            }

            refreshBloodDetails();
        }

        #endregion Refresh Methods

    }
}
