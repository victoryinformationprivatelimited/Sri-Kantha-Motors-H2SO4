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
    class QualificationCategoryNameViewModel : ViewModelBase
    {

        #region Fileds

        ERPServiceClient serviceClient;
        List<z_UnivercityDigreeNames> AllUnivercityDigreeNames;

        #endregion

        #region constructor

        public QualificationCategoryNameViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllUnivercityDigreeNames = new List<z_UnivercityDigreeNames>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshGetUnivercityDegreeName()
        {
            serviceClient.GetUnivercityDegreeNameCompleted += (s, e) =>
            {
                try
                {
                    UnivercityDigreeNames = e.Result;
                    if (UnivercityDigreeNames != null)
                        AllUnivercityDigreeNames = UnivercityDigreeNames.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetUnivercityDegreeNameAsync();
        }

        private void RefreshGetUnivercityDegreeType()
        {
            serviceClient.GetUnivercityDegreeTypeCompleted += (s, e) =>
            {
                try
                {
                    UnivercityDigreeType = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetUnivercityDegreeTypeAsync();

        }

        #endregion

        #region Properties

        private IEnumerable<z_UnivercityDigreeType> _UnivercityDigreeType;
        public IEnumerable<z_UnivercityDigreeType> UnivercityDigreeType
        {
            get { return _UnivercityDigreeType; }
            set { _UnivercityDigreeType = value; OnPropertyChanged("UnivercityDigreeType"); }
        }


        private IEnumerable<z_UnivercityDigreeNames> _UnivercityDigreeNames;
        public IEnumerable<z_UnivercityDigreeNames> UnivercityDigreeNames
        {
            get { return _UnivercityDigreeNames; }
            set { _UnivercityDigreeNames = value; OnPropertyChanged("UnivercityDigreeNames"); }
        }

        private z_UnivercityDigreeNames _CurrentUnivercityDigreeNames;
        public z_UnivercityDigreeNames CurrentUnivercityDigreeNames
        {
            get { return _CurrentUnivercityDigreeNames; }
            set { _CurrentUnivercityDigreeNames = value; OnPropertyChanged("CurrentUnivercityDigreeNames");}
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
            CurrentUnivercityDigreeNames = null;
            CurrentUnivercityDigreeNames = new z_UnivercityDigreeNames();
            RefreshGetUnivercityDegreeType();
            RefreshGetUnivercityDegreeName();
        }

        public ICommand SaveBtn
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave())
            {
                _CurrentUnivercityDigreeNames.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentUnivercityDigreeNames.save_datetime = DateTime.Now;
                _CurrentUnivercityDigreeNames.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentUnivercityDigreeNames.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateUnivercityDegreeName(_CurrentUnivercityDigreeNames))
                    clsMessages.setMessage("Record Saved Successfully");
                else
                    clsMessages.setMessage("Record Save Failed");

                New();

            }
        }

        public ICommand DeleteBtn
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (_CurrentUnivercityDigreeNames == null || _CurrentUnivercityDigreeNames.univercity_Course_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteUnivercityDegreeName(_CurrentUnivercityDigreeNames))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentUnivercityDigreeNames == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentUnivercityDigreeNames.univercity_Course_name))
            {
                clsMessages.setMessage("Please enter a valid name!");
                return false;
            }

            else if (_CurrentUnivercityDigreeNames.univercity_Course_type_id == null || _CurrentUnivercityDigreeNames.univercity_Course_type_id == 0)
            {

                clsMessages.setMessage("Please Select an Institute name!");
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
            set { _Search = value; OnPropertyChanged("Search"); if (UnivercityDigreeNames != null) searchTextChanged(); }
        }
        private void searchTextChanged()
        {
            UnivercityDigreeNames = null;
            UnivercityDigreeNames = AllUnivercityDigreeNames.Where(e => e.univercity_Course_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion


        #endregion

    }
}
