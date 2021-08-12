using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ERP.ERPService;

namespace ERP
{
    class PeriodViewModel : INotifyPropertyChanged
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();

        public PeriodViewModel()
        {

            refrishPeriod();
            this.New();

        }


        #region  Periods list  Properties
        private IEnumerable<z_Period> _Periods;

        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        #endregion

        #region Propety Change Event
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {


            var pargs = new PropertyChangedEventArgs(propertyName);
            if (this.PropertyChanged != null)
            {

                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Current Period Propertis
        private z_Period _CurrentPeriod;

        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }

            set
            {
                _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod");

            }
        }
        #endregion

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        private string _Search;

        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Search != null) FilterSearch(); }
        }

        // m 2021-07-08
        private bool _NoPayBaseEnable;

        public bool NoPayBaseEnable
        {
            get { return _NoPayBaseEnable; }
            set { _NoPayBaseEnable = value; OnPropertyChanged("NoPayBaseEnable"); }
        }

        private bool _LateBaseEnable;

        public bool LateBaseEnable
        {
            get { return _LateBaseEnable; }
            set { _LateBaseEnable = value; OnPropertyChanged("LateBaseEnable"); }
        }


        void New()
        {
            CurrentPeriod = null;
            CurrentPeriod = new z_Period();
            CurrentPeriod.period_id = Guid.NewGuid();
            // m 2021-07-08
            CurrentPeriod.nopay_base = 26;
            CurrentPeriod.late_base = 0;
            NoPayBaseEnable = LateBaseEnable = false;
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


        #region Refrish Period
        public void refrishPeriod()
        {
            this.serviceClient.GetAllPeriodCompleted += (s, e) =>
            {
                this.Periods = e.Result.Where(a => a.isdelete == false);
            };
            this.serviceClient.GetAllPeriodAsync();
        }
        #endregion


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
                return new RelayCommand(Save, savCanExecute);
            }
        }

        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, DeleteCanExecute);
            }
        }

        public void Save()
        {
            //bool isUpdate = false;
            if (ValidateSave())
            {
                bool isUpdate = false;

                z_Period newPeriod = new z_Period();
                newPeriod.period_id = CurrentPeriod.period_id;
                newPeriod.start_date = CurrentPeriod.start_date;
                newPeriod.end_date = CurrentPeriod.end_date;
                DateTime a = (DateTime)CurrentPeriod.start_date;
                DateTime b = (DateTime)CurrentPeriod.end_date;
                newPeriod.period_name = a.ToString("d MMM yyyy") + " to " + b.ToString("d MMM yyyy");
                newPeriod.save_datetime = System.DateTime.Now;
                newPeriod.save_user_id = Guid.Empty;
                newPeriod.modified_datetime = System.DateTime.Now;
                newPeriod.modified_user_id = Guid.Empty;
                newPeriod.delete_datetime = System.DateTime.Now;
                newPeriod.delete_user_id = Guid.Empty;
                newPeriod.is_proceed = CurrentPeriod.is_proceed == null ? false : CurrentPeriod.is_proceed; //haritha 2017-12-27
                // h 2020-10-09
                newPeriod.nopay_base = CurrentPeriod.nopay_base;
                newPeriod.late_base = CurrentPeriod.late_base;

                List<z_Period> allperiod = serviceClient.GetAllPeriod().ToList();

                foreach (z_Period per in allperiod)
                {
                    if (per.period_id == CurrentPeriod.period_id)
                    {
                        isUpdate = true;
                    }

                }
                if (CurrentPeriod != null && CurrentPeriod.period_id != null)
                {
                    if (newPeriod != null && CurrentPeriod.period_id != null)
                    {
                        bool isOldDatesChanged = false;  //haritha 2017-12-27
                        bool isValidDateFalse = true;

                        if (allperiod.Count != 0)
                        {
                            foreach (z_Period pe in allperiod)
                            {
                                if (pe.start_date <= CurrentPeriod.start_date && pe.start_date <= CurrentPeriod.end_date && pe.end_date <= CurrentPeriod.end_date && pe.end_date <= CurrentPeriod.end_date)
                                {
                                    //isValidDate = true;
                                }
                                else
                                {
                                    isValidDateFalse = false;
                                }
                                if (pe.start_date == CurrentPeriod.start_date && pe.end_date == CurrentPeriod.end_date && pe.period_id == CurrentPeriod.period_id && (CurrentPeriod.is_proceed == true || pe.is_proceed == true))
                                {
                                    isOldDatesChanged = true;
                                }//haritha 2017-12-27

                            }
                        }
                        else
                        {
                            //isValidDate = false;
                        }

                        if (isUpdate)
                        {


                            if (clsSecurity.GetUpdatePermission(316))
                            {
                                if (isValidDateFalse || isOldDatesChanged)   //haritha 2017-12-27
                                {
                                    newPeriod.modified_user_id = clsSecurity.loggedUser.user_id;
                                    newPeriod.modified_datetime = System.DateTime.Now;
                                    if (this.serviceClient.PeriodUpdate(newPeriod))
                                    {
                                        clsMessages.setMessage(Properties.Resources.UpdateSucess);
                                    }
                                }
                                else
                                {
                                    clsMessages.setMessage(Properties.Resources.UpdateFail);
                                }
                            }
                            else
                            {
                                clsMessages.setMessage("You don't have permission to Update this record(s)");
                            }
                        }
                        else
                        {
                            if (isValidDateFalse)
                            {
                                if (clsSecurity.GetSavePermission(316))
                                {
                                    newPeriod.save_user_id = clsSecurity.loggedUser.user_id;
                                    newPeriod.save_datetime = System.DateTime.Now;
                                    newPeriod.isdelete = false;

                                    if (this.serviceClient.SavePeriod(newPeriod))
                                    {
                                        clsMessages.setMessage(Properties.Resources.SaveSucess);
                                        CurrentPeriod = newPeriod;
                                    }
                                    else
                                    {
                                        clsMessages.setMessage(Properties.Resources.SaveFail);
                                    }
                                }

                                else
                                {
                                    clsMessages.setMessage("You don't have permission to Save this record(s)");

                                }
                            }
                        }
                    }
                    else
                    {
                        clsMessages.setMessage("Please Meantion the Department Name...!");
                    }
                    refrishPeriod();
                }
            }
        }

        private bool ValidateSave()
        {
            if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please Select Period Start Date...");
                return false;
            }
            else if (CurrentPeriod.start_date == null)
            {
                clsMessages.setMessage("Please Select Period Start Date...");
                return false;
            }
            else if (CurrentPeriod.end_date == null)
            {
                clsMessages.setMessage("Please Select Period End Date...");
                return false;
            }
            // h 2020-10-09 bases
            else if (CurrentPeriod.nopay_base == null || CurrentPeriod.nopay_base < 0)
            {
                clsMessages.setMessage("Please Enter No Pay Base");
                return false;
            }
            else if (CurrentPeriod.late_base == null || CurrentPeriod.late_base < 0)
            {
                clsMessages.setMessage("Please Enter Late Base");
                return false;
            }
            else
                return true;
        }
        public void Delete()
        {
            if (clsSecurity.GetDeletePermission(316))
            {
                if (ValidateDelete())
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to delete this record ?", "ERP", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        z_Period period = new z_Period();
                        period.period_id = CurrentPeriod.period_id;
                        period.period_name = CurrentPeriod.period_name;
                        period.start_date = CurrentPeriod.start_date;
                        period.end_date = CurrentPeriod.end_date;
                        period.delete_user_id = clsSecurity.loggedUser.user_id;
                        period.delete_datetime = System.DateTime.Now;

                        if (serviceClient.PeriodDelete(period))
                        {
                            clsMessages.setMessage("Record Delete sccusessful");
                        }
                        else
                        {
                            clsMessages.setMessage("Record Delete Fail");
                        }
                        refrishPeriod();

                    }
                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Delete this record(s)");
            }
        }

        private bool ValidateDelete()
        {
            if (CurrentPeriod == null)
            {
                clsMessages.setMessage("Please Select A Period Before Clicking The Delete Button...");
                return false;
            }
            else if (CurrentPeriod.period_id == Guid.Empty)
            {
                clsMessages.setMessage("Please Select A Period Before Clicking The Delete Button...");
                return false;
            }
            else if (CurrentPeriod.period_name == null || CurrentPeriod.period_name == string.Empty)
            {
                clsMessages.setMessage("Please Select A Period Before Clicking The Delete Button...");
                return false;
            }
            else
                return true;
        }
        private void FilterSearch()
        {
            Periods = Periods.Where(c => c.period_name != null && c.period_name.ToUpper().Contains(Search.ToUpper()));

        }


    }
}
