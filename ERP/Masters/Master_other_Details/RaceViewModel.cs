using ERP.AdditionalWindows;
using ERP.Attendance.Master_Details;
using ERP.ERPService;
using ERP.Masters;
using ERP.Masters.Master_other_Details;
using ERP.Medical;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ERP.Masters.Master_other_Details
{
    class RaceViewModel : ViewModelBase
    {
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_race> RaceAll;

        #endregion

        #region constructor

        public RaceViewModel()
        {
            serviceClient = new ERPServiceClient();
            RaceAll = new List<z_race>();
            New();
        }

        #endregion

        #region Refresh Methods

        private void RefreshRace()
        {
            serviceClient.GetRaceCompleted += (s, e) =>
            {
                try
                {
                    Race = e.Result;
                    if (Race != null)
                        RaceAll = Race.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetRaceAsync();
        }
        
        #endregion

        #region Properties

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

        #endregion

        #region Methods and Commands

        public ICommand NewButton 
        {
            get { return new RelayCommand(New); }
        }
        private void New()
        {
            CurrentRace = null;
            CurrentRace = new z_race();
            RefreshRace();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentRace.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentRace.save_datetime = DateTime.Now;
                _CurrentRace.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentRace.modified_datetime = DateTime.Now;

                if (serviceClient.SaveRace(_CurrentRace))
                    clsMessages.setMessage("Record Saved Successfully");
                else
                    clsMessages.setMessage("Record Save Failed");

                New();

            }
        }

        public ICommand DeleteBtn 
        {
            get { return new RelayCommand(Delete,DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (_CurrentRace == null || _CurrentRace.race_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteRace(_CurrentRace))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentRace == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentRace.race_name))
            {
                clsMessages.setMessage("Please enter a valid name!");
                return false;
            }

            else
                return true;
        }

        #region Search TextBox

        private string _Search;
        public string Search
        {
            get
            {
                return _Search;
            }
            set
            {
                this._Search = value;
                this.OnPropertyChanged("Search");
                if (_Search != null )                
                    searchTextChanged();
            }
        }

        private void searchTextChanged()
        {
            Race = null;
            Race = RaceAll.Where(e => e.race_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
