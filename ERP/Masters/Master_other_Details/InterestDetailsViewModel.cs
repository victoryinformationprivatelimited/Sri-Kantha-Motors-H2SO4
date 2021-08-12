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
    class InterestDetailsViewModel : ViewModelBase
    {

        #region Fileds

        ERPServiceClient serviceClient;
        List<z_InterestField> AllInterestField;

        #endregion

        #region constructor

        public InterestDetailsViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllInterestField = new List<z_InterestField>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshInterst()
        {
            serviceClient.GetInterestTypeCompleted += (s, e) =>
            {
                try
                {
                    InterestField = e.Result;
                    if (InterestField != null)
                        AllInterestField = InterestField.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetInterestTypeAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_InterestField> _InterestField;
        public IEnumerable<z_InterestField> InterestField
        {
            get { return _InterestField; }
            set { _InterestField = value; OnPropertyChanged("InterestField"); }
        }

        private z_InterestField _CurrentInterestField;
        public z_InterestField CurrentInterestField
        {
            get { return _CurrentInterestField; }
            set { _CurrentInterestField = value; OnPropertyChanged("CurrentInterestField"); }
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
            CurrentInterestField = null;
            CurrentInterestField = new z_InterestField();
            RefreshInterst();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentInterestField.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentInterestField.save_datetime = DateTime.Now;
                _CurrentInterestField.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentInterestField.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateInterest(_CurrentInterestField))
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
            if (_CurrentInterestField == null || _CurrentInterestField.interest_field_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteInterestField(_CurrentInterestField))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentInterestField == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentInterestField.interest_field_type))
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
            set { _Search = value; OnPropertyChanged("Search"); if (InterestField != null) searchTextChanged(); }
        }
        private void searchTextChanged()
        {
            InterestField = null;
            InterestField = AllInterestField.Where(e => e.interest_field_type.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion
        
        #endregion

    }
}
