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
    class LifeInsuranceViewModel : ViewModelBase
    {
        
        #region Fileds

        ERPServiceClient serviceClient;

        #endregion

        #region constructor

        public LifeInsuranceViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshLifeInsurance()
        {
            serviceClient.GetLifeInsuranceCompleted += (s, e) =>
            {
                try
                {
                    Life_Insurance = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetLifeInsuranceAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_Life_Insurance> _Life_Insurance;
        public IEnumerable<z_Life_Insurance> Life_Insurance
        {
            get { return _Life_Insurance; }
            set { _Life_Insurance = value; OnPropertyChanged("Life_Insurance"); }
        }

        private z_Life_Insurance _CurrentLife_Insurance;
        public z_Life_Insurance CurrentLife_Insurance
        {
            get { return _CurrentLife_Insurance; }
            set { _CurrentLife_Insurance = value; OnPropertyChanged("CurrentLife_Insurance"); }
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
            CurrentLife_Insurance = null;
            CurrentLife_Insurance = new z_Life_Insurance();
            RefreshLifeInsurance();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentLife_Insurance.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentLife_Insurance.save_datetime = DateTime.Now;
                _CurrentLife_Insurance.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentLife_Insurance.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateInsurance(_CurrentLife_Insurance))
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
            if (_CurrentLife_Insurance == null || _CurrentLife_Insurance.insurance_covers_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteInsurance(_CurrentLife_Insurance))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentLife_Insurance == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentLife_Insurance.insurance_covers_type))
            {
                clsMessages.setMessage("Please enter a valid name!");
                return false;
            }

            else
                return true;
        }

        
        #endregion

    }
}
