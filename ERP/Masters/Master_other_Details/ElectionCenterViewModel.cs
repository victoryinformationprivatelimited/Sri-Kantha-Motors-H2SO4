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
    class ElectionCenterViewModel : ViewModelBase
    {
        
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_election_center> ElectionCenterAll;

        #endregion

        #region constructor

        public ElectionCenterViewModel()
        {
            serviceClient = new ERPServiceClient();
            ElectionCenterAll = new List<z_election_center>();
            New();
        }

        #endregion

        #region Refresh Methods

        private void RefreshElectionCenters()
        {
            serviceClient.GetElectionCenterCompleted += (s, e) =>
            {
                try
                {
                    ElectionCenter = e.Result;
                    if (ElectionCenter != null)
                        ElectionCenterAll = ElectionCenter.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetElectionCenterAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_election_center> _ElectionCenter;
        public IEnumerable<z_election_center> ElectionCenter
        {
            get { return _ElectionCenter; }
            set { _ElectionCenter = value; OnPropertyChanged("ElectionCenter"); }
        }

        private z_election_center _CurrentElectionCenter;
        public z_election_center CurrentElectionCenter
        {
            get { return _CurrentElectionCenter; }
            set { _CurrentElectionCenter = value; OnPropertyChanged("CurrentElectionCenter"); }
        }

        #endregion

        #region Methods and Commands

        public ICommand NewButton 
        {
            get { return new RelayCommand(New); }
        }
        private void New()
        {
            CurrentElectionCenter = null;
            CurrentElectionCenter = new z_election_center();
            RefreshElectionCenters();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentElectionCenter.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentElectionCenter.save_datetime = DateTime.Now;
                _CurrentElectionCenter.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentElectionCenter.modified_datetime = DateTime.Now;

                if (serviceClient.SaveElectionCenter(_CurrentElectionCenter))
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
            if (_CurrentElectionCenter == null || _CurrentElectionCenter.election_center_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteElectionCenter(_CurrentElectionCenter))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentElectionCenter == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentElectionCenter.election_center))
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
            ElectionCenter = null;
            ElectionCenter = ElectionCenterAll.Where(e => e.election_center.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
