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
    class SchoolQualificationCategoryTypeViewModel : ViewModelBase
    {
          #region Fileds

        ERPServiceClient serviceClient;

        #endregion

        #region constructor

        public SchoolQualificationCategoryTypeViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshSchoolQualificationCategoryType()
        {
            serviceClient.GetSchoolQualificationTypeCompleted += (s, e) =>
            {
                try
                {
                    SchoolQualificationType = e.Result;
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetSchoolQualificationTypeAsync();
        }
        
        #endregion

        #region Properties

        private IEnumerable<z_SchoolQualificationType> _SchoolQualificationType;
        public IEnumerable<z_SchoolQualificationType> SchoolQualificationType
        {
            get { return _SchoolQualificationType; }
            set { _SchoolQualificationType = value; OnPropertyChanged("SchoolQualificationType"); }
        }

        private z_SchoolQualificationType _CurrentSchoolQualificationType;
        public z_SchoolQualificationType CurrentSchoolQualificationType
        {
            get { return _CurrentSchoolQualificationType; }
            set { _CurrentSchoolQualificationType = value; OnPropertyChanged("CurrentSchoolQualificationType"); }
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
            CurrentSchoolQualificationType = null;
            CurrentSchoolQualificationType = new z_SchoolQualificationType();
            RefreshSchoolQualificationCategoryType();
        }

        public ICommand SaveBtn 
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave()) 
            {
                _CurrentSchoolQualificationType.save_user_id = clsSecurity.loggedUser.user_id;
                _CurrentSchoolQualificationType.save_datetime = DateTime.Now;
                _CurrentSchoolQualificationType.modified_user_id = clsSecurity.loggedUser.user_id;
                _CurrentSchoolQualificationType.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateSchoolQualificationType(_CurrentSchoolQualificationType))
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
            if (_CurrentSchoolQualificationType == null || _CurrentSchoolQualificationType.school_qualifiaction_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteSchoolQualification(_CurrentSchoolQualificationType))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_CurrentSchoolQualificationType == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_CurrentSchoolQualificationType.school_qualifiaction_type))
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
