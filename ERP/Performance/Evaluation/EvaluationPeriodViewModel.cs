using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using ERP.Properties;

namespace ERP.Performance.Evaluation
{
    class EvaluationPeriodViewModel:ViewModelBase
    {

        #region Service Client

            private ERPServiceClient serviceClient = new ERPServiceClient();  

        #endregion

        #region Constructor
            public EvaluationPeriodViewModel()
            {
                this.RefrishEvaluationPeriod();
                this.New();

            }
            
        #endregion

        #region Propeties

        private IEnumerable <z_EvaluationPeriod > _Periods;
        public IEnumerable <z_EvaluationPeriod > Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_EvaluationPeriod _CurrentPeriod;
        public z_EvaluationPeriod CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }


        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }
        
        #endregion

        #region Refrish Evaluation Period
        public void RefrishEvaluationPeriod()
        {
            this.serviceClient.GetEvaluationPeriodsCompleted  += (s, e) =>
            {
                this.Periods = e.Result.Where(a => a.is_delete == false);
            };
            this.serviceClient.GetEvaluationPeriodsAsync();
        }
        #endregion

        #region Button Command
        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(SaveUpdate, savCanExecute);
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, DeleteCanExecute);
            }
        }


        void New()
        {
            CurrentPeriod = null;
            CurrentPeriod = new z_EvaluationPeriod();
            CurrentPeriod.from_date = DateTime.Today.Date;
            CurrentPeriod.to_date = DateTime.Today.Date;
        }

        bool newCanExecute()
        {
            return true;
        }

        bool savCanExecute()
        {
            if (CurrentPeriod != null)
            {
                return this.CurrentPeriod != null;
            }
            return false;
        }

        bool DeleteCanExecute()
        {
            if (CurrentPeriod != null)
            {
                return this.CurrentPeriod != null;
            }
            return false;
        }
        #endregion

        #region Save Method

        public void SaveUpdate()
        {
            if (ValidateSaveUpdate()) 
            {
                _CurrentPeriod.evaluation_period_name = "From " + _CurrentPeriod.from_date.ToShortDateString() + " To " + _CurrentPeriod.to_date.ToShortDateString();
                _CurrentPeriod.save_datetime = DateTime.Now;
                _CurrentPeriod.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentPeriod.is_delete = false;

                if (serviceClient.SaveUpdateEvaluationPeriod(CurrentPeriod))
                    clsMessages.setMessage("Record Saved/Updated Successfully");
                else
                    clsMessages.setMessage("Record Save/Update Failed");

                RefrishEvaluationPeriod();
            }

        }

        private bool ValidateSaveUpdate()
        {
            if (_CurrentPeriod.from_date == null)
            {
                clsMessages.setMessage("Please select a period start date");
                return false;
            }
            else if (_CurrentPeriod.to_date == null)
            {
                clsMessages.setMessage("Please select a period end date");
                return false;
            }
            else if (_CurrentPeriod.from_date >= _CurrentPeriod.to_date)
            {
                clsMessages.setMessage("Please enter a valid period");
                return false;
            }
            else
                return true;
        } 
        #endregion

        #region Delete Method
        public void Delete()
        {

            clsMessages.setMessage("Are you sure that you want to delete this record?", Visibility.Visible);
            if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
            {
                CurrentPeriod.is_delete = true;
                CurrentPeriod.delete_datetime = DateTime.Now;
                CurrentPeriod.delete_user_id = clsSecurity.loggedUser.user_id;

                if (serviceClient.DeleteEvaluationPeriod(CurrentPeriod))
                {
                    clsMessages.setMessage("Record Deleted Successfully");
                }
                else
                {
                    clsMessages.setMessage("Record Delete Failed");
                }
                RefrishEvaluationPeriod();

            }

        } 
        #endregion
    }
}
