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
    class HealthTypeViewModel:ViewModelBase
    {


        #region Fileds

        ERPServiceClient serviceClient;
        List<z_Blood_Group_Type> BloodGroupTypeAll;

        #endregion

        #region constructor

        public HealthTypeViewModel()
        {
            serviceClient = new ERPServiceClient();
            BloodGroupTypeAll = new List<z_Blood_Group_Type>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshHealthType()
        {
            serviceClient.GetHealthTypeCompleted += (s, e) =>
            {
                try
                {
                    BloodGroupType = e.Result;
                    if (BloodGroupType != null)
                        BloodGroupTypeAll = BloodGroupType.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetHealthTypeAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_Blood_Group_Type> _BloodGroupType;
        public IEnumerable<z_Blood_Group_Type> BloodGroupType
        {
            get { return _BloodGroupType; }
            set { _BloodGroupType = value; OnPropertyChanged("BloodGroupType"); }
        }

        private z_Blood_Group_Type _CurrentBloodGroupType;
        public z_Blood_Group_Type CurrentBloodGroupType
        {
            get { return _CurrentBloodGroupType; }
            set { _CurrentBloodGroupType = value; OnPropertyChanged("CurrentBloodGroupType"); }
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
            CurrentBloodGroupType = null;
            CurrentBloodGroupType = new z_Blood_Group_Type();
            RefreshHealthType();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentBloodGroupType.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentBloodGroupType.save_datetime = DateTime.Now;
                _CurrentBloodGroupType.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentBloodGroupType.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateHealth(_CurrentBloodGroupType))
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
            if (_CurrentBloodGroupType == null || _CurrentBloodGroupType.bloodGroupType_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteHealth(_CurrentBloodGroupType))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentBloodGroupType == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentBloodGroupType.bloodGroupType))
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
            BloodGroupType = null;
            BloodGroupType = BloodGroupTypeAll.Where(e => e.bloodGroupType.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

        
        #endregion
    }
}
