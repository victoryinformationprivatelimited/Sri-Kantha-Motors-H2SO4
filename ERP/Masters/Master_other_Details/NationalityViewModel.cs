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
    class NationalityViewModel : ViewModelBase
    {
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_Nationality> NationalitiesAll;

        #endregion

        #region constructor

        public NationalityViewModel()
        {
            serviceClient = new ERPServiceClient();
            NationalitiesAll = new List<z_Nationality>();
            New();
        }

        #endregion

        #region Refresh Methods

        private void RefreshNationalities()
        {
            serviceClient.GetNationalityCompleted += (s, e) =>
            {
                try
                {
                    Nationalities = e.Result;
                    if (Nationalities != null)
                        NationalitiesAll = Nationalities.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetNationalityAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_Nationality> _Nationalities;
        public IEnumerable<z_Nationality> Nationalities
        {
            get { return _Nationalities; }
            set { _Nationalities = value; OnPropertyChanged("Nationalities"); }
        }

        private z_Nationality _CurrentNationality;
        public z_Nationality CurrentNationality
        {
            get { return _CurrentNationality; }
            set { _CurrentNationality = value; OnPropertyChanged("CurrentNationality"); }
        }

        #endregion

        #region Methods and Commands

        public ICommand NewButton 
        {
            get { return new RelayCommand(New); }
        }
        private void New()
        {
            CurrentNationality = null;
            CurrentNationality = new z_Nationality();
            RefreshNationalities();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentNationality.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentNationality.save_datetime = DateTime.Now;
                _CurrentNationality.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentNationality.modified_datetime = DateTime.Now;

                if (serviceClient.SaveNationality(_CurrentNationality))
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
            if (_CurrentNationality == null || _CurrentNationality.nationality_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteNationality(_CurrentNationality))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentNationality == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentNationality.natinolity_name))
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
            Nationalities = null;
            Nationalities = NationalitiesAll.Where(e => e.natinolity_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
