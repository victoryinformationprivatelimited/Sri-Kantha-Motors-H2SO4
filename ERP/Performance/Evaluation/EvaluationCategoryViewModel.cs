using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows;
using System.Windows.Input;

namespace ERP.Performance.Evaluation
{
    class EvaluationCategoryViewModel:ViewModelBase 
    {
        #region ServicesReference

        private ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion

        #region Constructor
        public EvaluationCategoryViewModel()
        {
            this.CurrentEvaluationCategory = null;
            this.RefreshEvaluationCategories();
            this.New();

        }
        
        #endregion

        #region Properties

        private IEnumerable<z_EvaluationCategory> _EvaluationCategory;
        public IEnumerable<z_EvaluationCategory> EvaluationCategory
        {
            get
            {
                return this._EvaluationCategory;
            }
            set
            {
                this._EvaluationCategory = value;
                this.OnPropertyChanged("EvaluationCategory");
            }
        }

        private z_EvaluationCategory _CurrentEvaluationCategory;
        public z_EvaluationCategory CurrentEvaluationCategory
        {
            get
            {
                return this._CurrentEvaluationCategory; 
            }
            set
            {
                this._CurrentEvaluationCategory = value;
                this.OnPropertyChanged("CurrentEvaluationCategory");
            }
        }

        #endregion

        #region New Method
        void New()
        {

            this.CurrentEvaluationCategory = null;
            CurrentEvaluationCategory = new z_EvaluationCategory();
               
        
        }
        #endregion

        #region Save Method
        void Save()
        {
            bool IsUpdate = false;

            z_EvaluationCategory newEvaluationCategory = new z_EvaluationCategory();

            newEvaluationCategory.evaluation_cat_name = CurrentEvaluationCategory.evaluation_cat_name;
            newEvaluationCategory.evaluation_cat_description = CurrentEvaluationCategory.evaluation_cat_description;
            newEvaluationCategory.save_user_id = clsSecurity.loggedUser.user_id;
            newEvaluationCategory.save_datetime = System.DateTime.Now;
            newEvaluationCategory.modified_datetime = System.DateTime.Now;
            newEvaluationCategory.modified_user_id = clsSecurity.loggedUser.user_id;
            newEvaluationCategory.delete_datetime = System.DateTime.Now;
            newEvaluationCategory.delete_user_id = clsSecurity.loggedUser.user_id;
            newEvaluationCategory.is_delete = false;
            newEvaluationCategory.is_active = CurrentEvaluationCategory.is_active;

            IEnumerable<z_EvaluationCategory> allEvaluationCategories = this.serviceClient.GetEvaluationCategories();

            if (allEvaluationCategories != null)
            {
                foreach (var Evaluation in allEvaluationCategories)
                {
                    if (Evaluation.evaluation_cat_id == CurrentEvaluationCategory.evaluation_cat_id)
                    {
                        IsUpdate = true;
                    }
                }
            }
            if (newEvaluationCategory != null)
            {
                if (IsUpdate)
                {

                    newEvaluationCategory.evaluation_cat_id = CurrentEvaluationCategory.evaluation_cat_id;
                    newEvaluationCategory.modified_user_id = clsSecurity.loggedUser.user_id;
                    newEvaluationCategory.modified_datetime = System.DateTime.Now;

                    if (this.serviceClient.UpdateEvaluationCategory(newEvaluationCategory))
                    {
                        clsMessages.setMessage(Properties.Resources.UpdateSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.UpdateFail);
                    }


                }
                else
                {

                    newEvaluationCategory.save_user_id = clsSecurity.loggedUser.user_id;
                    newEvaluationCategory.save_datetime = System.DateTime.Now;

                    if (this.serviceClient.SaveEvaluationCategory(newEvaluationCategory))
                    {
                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.SaveFail);
                    }


                }
                RefreshEvaluationCategories();
            }
        }
            
        #endregion

        #region Delete Method
        void Delete()
        {
           
                MessageBoxResult rs = new MessageBoxResult();
                rs = MessageBox.Show("Do You Want To Delete This Record...?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (rs == MessageBoxResult.Yes)
                {
                    z_EvaluationCategory EvaluationCategory = new z_EvaluationCategory();

                    EvaluationCategory.evaluation_cat_id = CurrentEvaluationCategory.evaluation_cat_id;
                    EvaluationCategory.evaluation_cat_description = CurrentEvaluationCategory.evaluation_cat_description;
                    EvaluationCategory.evaluation_cat_name = CurrentEvaluationCategory.evaluation_cat_name;
                    EvaluationCategory.delete_user_id = clsSecurity.loggedUser.user_id;
                    EvaluationCategory.delete_datetime = System.DateTime.Now;

                    if (this.serviceClient.DeleteEvaluationCategory(EvaluationCategory))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        RefreshEvaluationCategories();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
                this.New();

        }
        #endregion

        #region Refresh Methods

        private void RefreshEvaluationCategories()
        {
            this.serviceClient.GetEvaluationCategoriesCompleted += (s, e) =>
            {
                this.EvaluationCategory = e.Result;
            };
            this.serviceClient.GetEvaluationCategoriesAsync();
        } 


        #endregion

        #region SaveButton Calss & Property
        bool saveCanExecute()
        {
            if (CurrentEvaluationCategory != null)
            {
                if (CurrentEvaluationCategory.evaluation_cat_name == null || CurrentEvaluationCategory.evaluation_cat_name == string.Empty)
                {
                    return false;
                }
                if (CurrentEvaluationCategory.evaluation_cat_description == null || CurrentEvaluationCategory.evaluation_cat_description == string.Empty)
                { 
                    return false; 
                }
                if (CurrentEvaluationCategory.is_active == null)
                {
                    return false;
                }
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

        #region DeleteButton Calss & Property
        bool DeleteCanExecute()
        {
            if (CurrentEvaluationCategory == null)
            {
                return false;
            }
            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, DeleteCanExecute);
            }
        }
        #endregion

        #region NewButton Class & Property
        bool NewCanExecute()
        {
            return true;
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, NewCanExecute);
            }
        }
        #endregion


    }
}
