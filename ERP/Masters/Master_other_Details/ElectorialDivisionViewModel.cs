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
    class ElectorialDivisionViewModel : ViewModelBase
    {
                
        #region Fileds

        ERPServiceClient serviceClient;
        List<z_electorial_division> ElectorialDivisionAll;

        #endregion

        #region constructor

        public ElectorialDivisionViewModel()
        {
            serviceClient = new ERPServiceClient();
            ElectorialDivisionAll = new List<z_electorial_division>();
            New();
        }

        #endregion

        #region Refresh Methods

        private void RefreshElectorialDivisions()
        {
            serviceClient.GetElectorialDivisionCompleted += (s, e) =>
            {
                try
                {
                    ElectorialDivision = e.Result;
                    if (ElectorialDivision != null)
                        ElectorialDivisionAll = ElectorialDivision.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetElectorialDivisionAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_electorial_division> _ElectorialDivision;
        public IEnumerable<z_electorial_division> ElectorialDivision
        {
            get { return _ElectorialDivision; }
            set { _ElectorialDivision = value; OnPropertyChanged("ElectorialDivision"); }
        }

        private z_electorial_division _CurrentElectorialDivision;
        public z_electorial_division CurrentElectorialDivision
        {
            get { return _CurrentElectorialDivision; }
            set { _CurrentElectorialDivision = value; OnPropertyChanged("CurrentElectorialDivision"); }
        }

        #endregion

        #region Methods and Commands

        public ICommand NewButton 
        {
            get { return new RelayCommand(New); }
        }
        private void New()
        {
            CurrentElectorialDivision = null;
            CurrentElectorialDivision = new z_electorial_division();
            RefreshElectorialDivisions();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentElectorialDivision.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentElectorialDivision.save_datetime = DateTime.Now;
                _CurrentElectorialDivision.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentElectorialDivision.modified_datetime = DateTime.Now;

                if (serviceClient.SaveElectorialDivision(_CurrentElectorialDivision))
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
            if (_CurrentElectorialDivision == null || _CurrentElectorialDivision.electorial_division_id == 0)
                return false;
            else
                return true;
        }

        private void Delete()
        {

            if (serviceClient.DeleteElectorialDivision(_CurrentElectorialDivision))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentElectorialDivision == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentElectorialDivision.electorial_division_name))
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
            ElectorialDivision = null;
            ElectorialDivision = ElectorialDivisionAll.Where(e => e.electorial_division_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
