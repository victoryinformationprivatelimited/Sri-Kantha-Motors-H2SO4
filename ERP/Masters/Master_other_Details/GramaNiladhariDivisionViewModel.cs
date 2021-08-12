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
    class GramaNiladhariDivisionViewModel : ViewModelBase
    {
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_grama_niladhari_divition> GramaNiladhariDivisionAll;

        #endregion

        #region constructor

        public GramaNiladhariDivisionViewModel()
        {
            serviceClient = new ERPServiceClient();
            GramaNiladhariDivisionAll = new List<z_grama_niladhari_divition>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshGramaNiladhariDivision()
        {
            serviceClient.GetGramaNiladhariDivitionCompleted += (s, e) =>
            {
                try
                {
                    GramaNiladhariDivision = e.Result;
                    if (GramaNiladhariDivision != null)
                        GramaNiladhariDivisionAll = GramaNiladhariDivision.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetGramaNiladhariDivitionAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_grama_niladhari_divition> _GramaNiladhariDivision;
        public IEnumerable<z_grama_niladhari_divition> GramaNiladhariDivision
        {
            get { return _GramaNiladhariDivision; }
            set { _GramaNiladhariDivision = value; OnPropertyChanged("GramaNiladhariDivision"); }
        }

        private z_grama_niladhari_divition _CurrentGramaNiladhariDivision;
        public z_grama_niladhari_divition CurrentGramaNiladhariDivision
        {
            get { return _CurrentGramaNiladhariDivision; }
            set { _CurrentGramaNiladhariDivision = value; OnPropertyChanged("CurrentGramaNiladhariDivision"); }
        }

        #endregion

        #region Methods and Commands

        public ICommand NewButton 
        {
            get { return new RelayCommand(New); }
        }
        private void New()
        {
            CurrentGramaNiladhariDivision = null;
            CurrentGramaNiladhariDivision = new z_grama_niladhari_divition();
            RefreshGramaNiladhariDivision();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentGramaNiladhariDivision.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentGramaNiladhariDivision.save_datetime = DateTime.Now;
                _CurrentGramaNiladhariDivision.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentGramaNiladhariDivision.modified_datetime = DateTime.Now;

                if (serviceClient.SaveGramaNiladhariDivition(_CurrentGramaNiladhariDivision))
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
            if (_CurrentGramaNiladhariDivision == null || _CurrentGramaNiladhariDivision.grama_niladhari_divition_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteGramaNiladhariDivition(_CurrentGramaNiladhariDivision))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentGramaNiladhariDivision == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentGramaNiladhariDivision.grama_niladhari_divition_name))
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
            GramaNiladhariDivision = null;
            GramaNiladhariDivision = GramaNiladhariDivisionAll.Where(e => e.grama_niladhari_divition_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
