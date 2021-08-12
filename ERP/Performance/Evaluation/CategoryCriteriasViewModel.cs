using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP.Performance;

namespace ERP.Performance.Evaluation
{
    class CategoryCriteriasViewModel : ViewModelBase
    {
        #region Fields

        ERPServiceClient serviceClient;
        List<dtl_EvaluationCriteria> ListAllCriteria;
        List<dtl_EvaluationCriteria> ListCriteria;

        #endregion

        #region Constructor

        public CategoryCriteriasViewModel()
        {
            serviceClient = new ERPServiceClient();
            ListAllCriteria = new List<dtl_EvaluationCriteria>();
            ListCriteria = new List<dtl_EvaluationCriteria>();
            SearchIndex = 0;
            New();
        }

        #endregion

        #region Properties

        private string _SearchText;
        public string SearchText 
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText"); }
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private IEnumerable<z_EvaluationCategory> _Categories;
        public IEnumerable<z_EvaluationCategory> Categories
        {
            get { return _Categories; }
            set { _Categories = value; OnPropertyChanged("Categories"); }
        }

        private z_EvaluationCategory _CurrentCategory;
        public z_EvaluationCategory CurrentCategory
        {
            get { return _CurrentCategory; }
            set { _CurrentCategory = value; OnPropertyChanged("CurrentCategory"); if (CurrentCategory != null && ListAllCriteria != null && ListAllCriteria.Count() > 0) { Criterias = null; Criterias = ListAllCriteria.Where(c => c.evaluation_cat_id == CurrentCategory.evaluation_cat_id); } }
        }

        private IEnumerable<dtl_EvaluationCriteria> _Criterias;
        public IEnumerable<dtl_EvaluationCriteria> Criterias
        {
            get { return _Criterias; }
            set { _Criterias = value; OnPropertyChanged("Criterias"); }
        }

        private dtl_EvaluationCriteria _CurrentCriteria;
        public dtl_EvaluationCriteria CurrentCriteria
        {
            get { return _CurrentCriteria; }
            set { _CurrentCriteria = value; OnPropertyChanged("CurrentCriteria");  }
        }

        private IEnumerable<z_EvaluationRateGoup> _RateGroups;
        public IEnumerable<z_EvaluationRateGoup> RateGroups
        {
            get { return _RateGroups; }
            set { _RateGroups = value; OnPropertyChanged("RateGroups"); }
        }
        
               
        #endregion

        #region RefreshMethods

        private void RefreshCriterias()
        {
            serviceClient.GetEvaluationCategoriesCompleted += (s, e) => 
            {
                try
                {
                    Categories = e.Result;
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshCriterias()\n\n" + ex.Message);
                }
            };
            serviceClient.GetEvaluationCategoriesAsync();
        }

        private void RefreshCategories()
        {
            serviceClient.GetEvaluationCriteriaCompleted += (s, e) => 
            {
                try
                {
                    Criterias = e.Result;
                    if (Criterias != null && Criterias.Count() > 0) 
                    {
                        ListAllCriteria = Criterias.ToList();
                        Criterias = null;
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshCategories()\n\n" + ex.Message);
                }
            };
            serviceClient.GetEvaluationCriteriaAsync();
        }

        private void RefreshRateGroup() 
        {
            serviceClient.GetEvaluationRateGroupsCompleted += (s, e) => 
            {
                try
                {
                    RateGroups = e.Result; 
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("RefreshRateGroup()\n\n" + ex.Message);
                }
            };
            serviceClient.GetEvaluationRateGroupsAsync();
        }

        
        #endregion

        #region Commands & Methods
        private void New()
        {
            ListCriteria.Clear();
            ListAllCriteria.Clear();
            CurrentCriteria = null;
            CurrentCriteria = new dtl_EvaluationCriteria();
            RefreshCategories();
            RefreshCriterias();
            RefreshRateGroup();
        }

        public ICommand NewBtn 
        {
            get { return new RelayCommand(New); }
        }
        public ICommand AddCriteriabtn
        {
            get { return new RelayCommand(AddCriteria, AddCriteriaCE); }
        }
        private void AddCriteria()
        {
            if(ValidateAddCriteria())
            {
                //List<dtl_EvaluationCriteria> TempList = new List<dtl_EvaluationCriteria>();
                //TempList = ListAllCriteria.Where(c => c.evaluation_cat_id == CurrentCategory.evaluation_cat_id).ToList();

                CurrentCriteria.evaluation_cat_id = CurrentCategory.evaluation_cat_id;
                CurrentCriteria.save_user_id = clsSecurity.loggedUser.user_id;
                CurrentCriteria.is_active = true;

                ListAllCriteria.Add(CurrentCriteria);

                CurrentCriteria = null;
                CurrentCriteria = new dtl_EvaluationCriteria();
                Criterias = null;
                Criterias = ListAllCriteria.Where(c=> c.evaluation_cat_id == CurrentCategory.evaluation_cat_id);
            }
        }

        private bool ValidateAddCriteria()
        {
            if (string.IsNullOrEmpty(CurrentCriteria.evaluation_criteria_name))
            {
                clsMessages.setMessage("Criteria Cannot be Empty");
                return false;
            }
            else if (CurrentCriteria.evaluation_criteria_id != 0)
            {
                clsMessages.setMessage("This criteria is already in the list");
                return false;
            }
            else if (CurrentCriteria.evaluation_rate_group_id == 0)
            {
                clsMessages.setMessage("Please select a Rete Group for this criteria");
                return false;
            }
            else
                return true;
        }
        private bool AddCriteriaCE()
        {
            if (CurrentCategory == null || CurrentCriteria == null)
                return false;
            else
                return true;
        }
        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save,SaveCE);}
        }

        private bool SaveCE()
        {
            if (CurrentCategory != null)
                return true;
            else
                return false;
        }
        private void Save()
        {
            if (Criterias == null || Criterias.Count() == 0)
                clsMessages.setMessage("Please add criterias and try again");
            else if (Criterias.Count(c => string.IsNullOrEmpty(c.evaluation_criteria_name)) > 0)
            {
                clsMessages.setMessage("Empty criterias cannot be saved");
                CurrentCriteria = Criterias.FirstOrDefault(c => string.IsNullOrEmpty(c.evaluation_criteria_name));
            }
            else 
            {

                if(serviceClient.SaveUpdateEvaluationCrieteria(Criterias.ToArray()))
                    clsMessages.setMessage("Criterias Saved/Updated Successfully");
                else
                    clsMessages.setMessage("Criterias Save/Update failed");
                New();
            }
        }

        public ICommand DeleteBtn 
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (CurrentCriteria != null && CurrentCriteria.evaluation_criteria_id != 0)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            CurrentCriteria.delete_user_id = clsSecurity.loggedUser.user_id;
            CurrentCriteria.delete_datetime = DateTime.Now;

            if (serviceClient.DeleteEvaluationCriteria(CurrentCriteria))
                clsMessages.setMessage("Criteria Deleted Successfully");
            else
                clsMessages.setMessage("Criteria Delete Failed");
            New();
        }

        public ICommand RemoveItemBtn
        {
            get { return new RelayCommand(RemoveItem); }
        }
        private void RemoveItem()
        {
            if (CurrentCategory != null && CurrentCriteria != null && CurrentCriteria.evaluation_criteria_id == 0)
            {
                ListAllCriteria.Remove(CurrentCriteria);
                Criterias = null;
                Criterias = ListAllCriteria.Where(c => c.evaluation_cat_id == CurrentCategory.evaluation_cat_id);
                CurrentCriteria = null;
                CurrentCriteria = new dtl_EvaluationCriteria();
            }
        }

        public ICommand Resetbtn 
        {
            get { return new RelayCommand(Reset); }
        }

        private void Reset()
        {
            CurrentCriteria = null;
            CurrentCriteria = new dtl_EvaluationCriteria();
        }

        #endregion
    }
}
