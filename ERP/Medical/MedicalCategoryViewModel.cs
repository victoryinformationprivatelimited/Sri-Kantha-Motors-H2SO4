using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Medical
{
    class MedicalCategoryViewModel :ViewModelBase
    {
        #region Services Object

        private ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion

        #region Constructor

        public MedicalCategoryViewModel()
        {
            //if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.MedicalReimbursementCategories), clsSecurity.loggedUser.user_id))
            //{
                refreshMedicalPeriods();
                New();
            //}
            //else
            //{
            //    clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            //}

        }

        #endregion Constructor

        #region Properties
       
        private IEnumerable<z_MedicalPeriod> _MedicalPeriods;
        public IEnumerable<z_MedicalPeriod> MedicalPeriods
        {
            get { return _MedicalPeriods; }
            set { _MedicalPeriods = value; OnPropertyChanged("MedicalPeriods");  }
        }

        private z_MedicalPeriod _CurrentMedicalPeriod;
        public z_MedicalPeriod CurrentMedicalPeriod
        {
            get { return _CurrentMedicalPeriod; }
            set { _CurrentMedicalPeriod = value; OnPropertyChanged("CurrentMedicalPeriod"); refreshMedicalCategoriesInPeriod(); }
        }

        private IEnumerable<z_MedicalCategory> _MedicalCategoriesInPeriod;
        public IEnumerable<z_MedicalCategory> MedicalCategoriesInPeriod
        {
            get { return _MedicalCategoriesInPeriod; }
            set { _MedicalCategoriesInPeriod = value; OnPropertyChanged("MedicalCategoriesInPeriod"); }
        }

        private z_MedicalCategory _CurrentMedicalCategory;
        public z_MedicalCategory CurrentMedicalCategory
        {
            get { return _CurrentMedicalCategory; }
            set { _CurrentMedicalCategory = value; OnPropertyChanged("CurrentMedicalCategory"); }
        }

       
        #endregion Properties

        #region New Method

        void New()
        {
            //if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.MedicalReimbursementCategories), clsSecurity.loggedUser.user_id))
            //{
                this.CurrentMedicalCategory = null;
                CurrentMedicalCategory = new z_MedicalCategory();
               

            //}
        }

        #endregion

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

        #region Save Method

        void Save()
        {
            //if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.MedicalReimbursementCategories), clsSecurity.loggedUser.user_id))
            //{
                //bool IsUpdate = false;


            if ((CurrentMedicalCategory.total_amount > 0) && !(CurrentMedicalCategory.total_amount == null))
            {
                z_MedicalCategory newMedicalCategory = new z_MedicalCategory();

                newMedicalCategory.period_id = CurrentMedicalPeriod.period_id;
                newMedicalCategory.cat_id = CurrentMedicalCategory.cat_id;
                newMedicalCategory.cat_desc = CurrentMedicalCategory.cat_desc;
                newMedicalCategory.total_amount = CurrentMedicalCategory.total_amount;

                newMedicalCategory.is_active = true;//CurrentMedicalCategory.is_active;

                //IEnumerable<z_MedicalCategory> allcats = this.serviceClient.GetMedicalCategories();

                //if (allcats != null)
                //{
                //    foreach (var value in allcats)
                //    {
                //        if (value.cat_id == CurrentMedicalCategory.cat_id)
                //        {
                //            IsUpdate = true;
                //            break;
                //        }
                //    }
                //}

                if (newMedicalCategory != null && newMedicalCategory.cat_id != null && newMedicalCategory.period_id != null)
                {
                    if (isUpdate())
                    {
                        newMedicalCategory.modified_datetime = System.DateTime.Now;
                        if (clsSecurity.GetUpdatePermission(702))
                        {

                            if (this.serviceClient.UpdateMedicalCategory(newMedicalCategory))
                            {
                                //MessageBox.Show("Record Updated Successfully");
                                clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            }
                            else
                            {
                                //MessageBox.Show("Record Update Failed");
                                clsMessages.setMessage(Properties.Resources.UpdateFail);
                            }
                        }
                        else
                            clsMessages.setMessage("You Don't Have Permission  to Update in this Form...");
                    }
                    else
                    {
                        newMedicalCategory.is_delete = false;

                        if (clsSecurity.GetSavePermission(702))
                        {
                            if (this.serviceClient.SaveMedicalCategory(newMedicalCategory))
                            {
                                clsMessages.setMessage(Properties.Resources.SaveSucess);

                                CurrentMedicalCategory = null;
                            }
                            else
                            {
                                clsMessages.setMessage(Properties.Resources.SaveFail);
                            }
                        }
                        else
                            clsMessages.setMessage("You don't have permission to Save in this Form...");
                    }

                    refreshMedicalCategoriesInPeriod();
                }
                //}
            }
        }

        #endregion Save Method

        #region SaveButton Class & Property

        bool saveCanExecute()
        {
            if (CurrentMedicalCategory != null)
            {
                if (CurrentMedicalCategory.cat_desc == null)
                    return false;
                if (CurrentMedicalCategory.cat_id == Guid.Empty || CurrentMedicalCategory.cat_id == null)
                    CurrentMedicalCategory.cat_id = Guid.NewGuid();
               
                //if (CurrentMedicalPeriod.from_date > System.DateTime.Now)
                //    return false;                  
                //if (isUpdate())
                //{
                //    if (CurrentMedicalPeriod.from_date <= System.DateTime.Now)
                //    {
                //        return false;
                //    }
                //}
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

        private bool isUpdate()
        {
            IEnumerable<z_MedicalCategory> allcats = this.serviceClient.GetMedicalCategories();

            if (allcats != null)
            {
                foreach (var value in allcats)
                {
                    if (value.cat_id == CurrentMedicalCategory.cat_id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Refresh Methods

        private void refreshMedicalPeriods()
        {
            this.serviceClient.GetMedicalPeriodsCompleted += (s, e) =>
            {
                this.MedicalPeriods = e.Result.Where(p => p.is_delete == false && p.is_active == true).OrderBy(p => p.from_date).ToList();
            };
            this.serviceClient.GetMedicalPeriodsAsync();


        }

        private void refreshMedicalCategoriesInPeriod()
        {
            if(CurrentMedicalPeriod != null){

                this.serviceClient.GetMedicalCategoriesByPeriodIdCompleted += (s, e) =>
                {
                    this.MedicalCategoriesInPeriod = e.Result.OrderBy(p => p.total_amount);
                };

                this.serviceClient.GetMedicalCategoriesByPeriodIdAsync(CurrentMedicalPeriod.period_id);
            
            }
        }

        #endregion Refresh Methods
    }



}
