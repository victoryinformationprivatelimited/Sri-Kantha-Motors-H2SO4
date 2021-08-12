using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Medical
{
    class MedicalPeriodMasterViewModel : ViewModelBase
    {
        #region Services Object

        private ERPServiceClient serviceClient = new ERPServiceClient();

        #endregion

        #region Constructor

        public MedicalPeriodMasterViewModel()
        {
            this.refreshMedicalPeriods();
            this.New();

        }

        #endregion

        #region Properties

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

        private z_MedicalPeriod _CurrentMedicalPeriod;
        public z_MedicalPeriod CurrentMedicalPeriod
        {
            get
            {
                return this._CurrentMedicalPeriod;
            }
            set
            {
                this._CurrentMedicalPeriod = value;
                this.OnPropertyChanged("CurrentMedicalPeriod");

                if (CurrentMedicalPeriod != null)
                {
                    if (CurrentMedicalPeriod.is_active != null)
                        this.CurrentPeriodIsActive = (bool)CurrentMedicalPeriod.is_active;
                }
                else
                {
                    this.CurrentPeriodIsActive = false;
                }
            }
        }

        private string _CurrentPeriodName;
        public string CurrentPeriodName
        {
            get
            {
                return this._CurrentPeriodName;
            }
            set
            {
                this._CurrentPeriodName = value;
                this.OnPropertyChanged("CurrentPeriodName");
            }
        }

        private bool _CurrentPeriodIsActive;
        public bool CurrentPeriodIsActive
        {
            get
            {
                return this._CurrentPeriodIsActive;
            }
            set
            {
                this._CurrentPeriodIsActive = value;
                this.OnPropertyChanged("CurrentPeriodIsActive");
            }
        }


        #endregion

        #region New Method

        void New()
        {
            //if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.MedicalReimbursementPeriod), clsSecurity.loggedUser.user_id))
            //{
            this.CurrentMedicalPeriod = null;
            this.CurrentPeriodIsActive = false;
            CurrentMedicalPeriod = new z_MedicalPeriod();
            CurrentMedicalPeriod.period_id = Guid.NewGuid();
            refreshMedicalPeriods();
            //}
            //else
            //{
            //    clsMessages.setMessage(Properties.Resources.NoPermissionForView);
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
            bool IsUpdate = false;

            z_MedicalPeriod newMedicalPeriod = new z_MedicalPeriod();
            newMedicalPeriod.period_id = CurrentMedicalPeriod.period_id;
            newMedicalPeriod.from_date = CurrentMedicalPeriod.from_date;

            string _from = Convert.ToDateTime(CurrentMedicalPeriod.from_date).ToShortDateString();
            string _to = Convert.ToDateTime(CurrentMedicalPeriod.to_date).ToShortDateString();
            CurrentPeriodName = _from + " to " + _to;
            newMedicalPeriod.period_name = CurrentPeriodName;

            newMedicalPeriod.to_date = CurrentMedicalPeriod.to_date;

            IEnumerable<z_MedicalPeriod> allPeriods = this.serviceClient.GetMedicalPeriods();

            if (allPeriods != null)
            {
                foreach (var Period in allPeriods)
                {
                    if (Period.period_id == CurrentMedicalPeriod.period_id)
                    {
                        IsUpdate = true;
                        break;
                    }
                }
            }

            newMedicalPeriod.is_active = (CurrentPeriodIsActive == null) ? false : CurrentPeriodIsActive;

            if (newMedicalPeriod != null && newMedicalPeriod.period_id != null)
            {
                if (IsUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(701))
                    {
                        if (this.serviceClient.UpdateMedicalPeriods(newMedicalPeriod))
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
                        clsMessages.setMessage("You Dont Have Permission to Update in this Form...");
                    }
                }
                else
                {
                    newMedicalPeriod.is_active = (CurrentPeriodIsActive == null) ? false : CurrentPeriodIsActive;
                    newMedicalPeriod.is_delete = false;

                    if (clsSecurity.GetSavePermission(701))
                    {
                        if (this.serviceClient.SaveMedicalPeriods(newMedicalPeriod))
                        {
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                        }
                        else
                        {
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("You dont have Permission to Save in this Form...");
                    }
                }

                refreshMedicalPeriods();
            }
        }

        #endregion

        #region SaveButton Class & Property

        bool saveCanExecute()
        {
            if (CurrentMedicalPeriod != null)
            {
                if (CurrentMedicalPeriod.from_date == null)
                    return false;
                if (CurrentMedicalPeriod.to_date == null)
                    return false;
                //if (CurrentMedicalPeriod.from_date < System.DateTime.Now)
                //    return false;
                //if (CurrentMedicalPeriod.from_date > CurrentMedicalPeriod.to_date)
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
            IEnumerable<z_MedicalPeriod> allPeriods = this.serviceClient.GetMedicalPeriods();

            if (allPeriods != null)
            {
                foreach (var Period in allPeriods)
                {
                    if (Period.period_id == CurrentMedicalPeriod.period_id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Delete Method

        void Delete()
        {
            if (clsSecurity.GetDeletePermission(701))
            {
                MessageBoxResult result = new MessageBoxResult();

                result = MessageBox.Show("Are you sure you want to delete this record?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (this.serviceClient.DeleteMedicalPeriods(CurrentMedicalPeriod))
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteSucess);
                        refreshMedicalPeriods();
                    }
                    else
                    {
                        clsMessages.setMessage(Properties.Resources.DeleteFail);
                    }
                }
                this.New();
            }
            else
            {
                clsMessages.setMessage("You Dont Have Permission to Delete In this Form...");
            }
        }

        #endregion

        #region Delete Button Calass & Property

        bool deleteCanExecute()
        {
            if (CurrentMedicalPeriod == null)
            {
                return false;
            }
            return true;
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecute);
            }
        }

        #endregion

        #region Refresh Methods

        private void refreshMedicalPeriods()
        {
            this.serviceClient.GetMedicalPeriodsCompleted += (s, e) =>
            {
                this.MedicalPeriods = e.Result.Where(o => o.is_delete == false);
            };
            this.serviceClient.GetMedicalPeriodsAsync();
        }

        #endregion Refresh Methods

    }
}
