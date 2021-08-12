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
    class InstituteNameViewModel : ViewModelBase
    {
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_UnivercityName> AllUniversites;

        #endregion

        #region constructor

        public InstituteNameViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllUniversites = new List<z_UnivercityName>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshUniversites()
        {
            serviceClient.GetUnivercityNameCompleted += (s, e) =>
            {
                try
                {
                    Universites = e.Result;
                    if (Universites != null)
                        AllUniversites = Universites.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetUnivercityNameAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_UnivercityName> _Universites;
        public IEnumerable<z_UnivercityName> Universites
        {
            get { return _Universites; }
            set { _Universites = value; OnPropertyChanged("Universites"); }
        }

        private z_UnivercityName _CurrentUniversity;
        public z_UnivercityName CurrentUniversity
        {
            get { return _CurrentUniversity; }
            set { _CurrentUniversity = value; OnPropertyChanged("CurrentUniversity"); }
        }

        #endregion

        #region Methods and Commands

        public ICommand NewButton 
        {
            get { return new RelayCommand(New); }
        }
        private void New()
        {
           // _Universites = null;
            CurrentUniversity = null;
            CurrentUniversity = new z_UnivercityName();
            RefreshUniversites();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentUniversity.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentUniversity.save_datetime = DateTime.Now;
                _CurrentUniversity.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentUniversity.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateUnivercityName(_CurrentUniversity))
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
            if (_CurrentUniversity == null || _CurrentUniversity.univercity_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteUnivercityName(_CurrentUniversity))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentUniversity == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentUniversity.univercity_name))
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
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (Universites != null) searchTextChanged(); }
        }
        private void searchTextChanged()
        {
            Universites = null;
            Universites = AllUniversites.Where(e => e.univercity_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion


        #endregion
    }
}
