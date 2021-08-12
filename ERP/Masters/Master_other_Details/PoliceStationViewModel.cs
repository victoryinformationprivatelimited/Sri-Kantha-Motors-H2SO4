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
    class PoliceStationViewModel : ViewModelBase
    {
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_nearest_police_station> PoliceStationAll;

        #endregion

        #region constructor

        public PoliceStationViewModel()
        {
            serviceClient = new ERPServiceClient();
            PoliceStationAll = new List<z_nearest_police_station>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshPoliceStations()
        {
            serviceClient.GetNearestPoliceStationCompleted += (s, e) =>
            {
                try
                {
                    PoliceStation = e.Result;
                    if (PoliceStation != null)
                        PoliceStationAll = PoliceStation.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetNearestPoliceStationAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_nearest_police_station> _PoliceStation;
        public IEnumerable<z_nearest_police_station> PoliceStation
        {
            get { return _PoliceStation; }
            set { _PoliceStation = value; OnPropertyChanged("PoliceStation"); }
        }

        private z_nearest_police_station _CurrentPoliceStation;
        public z_nearest_police_station CurrentPoliceStation
        {
            get { return _CurrentPoliceStation; }
            set { _CurrentPoliceStation = value; OnPropertyChanged("CurrentPoliceStation"); }
        }

        #endregion

        #region Methods and Commands

        public ICommand NewButton 
        {
            get { return new RelayCommand(New); }
        }
        private void New()
        {
            CurrentPoliceStation = null;
            CurrentPoliceStation = new z_nearest_police_station();
            RefreshPoliceStations();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentPoliceStation.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentPoliceStation.save_datetime = DateTime.Now;
                _CurrentPoliceStation.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentPoliceStation.modified_datetime = DateTime.Now;

                if (serviceClient.SaveNearestPoliceStation(_CurrentPoliceStation))
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
            if (_CurrentPoliceStation == null || _CurrentPoliceStation.nearest_police_station_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteNearestPoliceStation(_CurrentPoliceStation))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentPoliceStation == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentPoliceStation.nearest_police_station))
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
            PoliceStation = null;
            PoliceStation = PoliceStationAll.Where(e => e.nearest_police_station.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
