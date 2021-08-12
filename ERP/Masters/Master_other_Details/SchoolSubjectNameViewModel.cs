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
    class SchoolSubjectNameViewModel : ViewModelBase
    {

        #region Fileds

        ERPServiceClient serviceClient;
        List<z_SchoolQualificationSubject> AllSchoolQualificationSubject;

        #endregion

        #region constructor

        public SchoolSubjectNameViewModel()
        {
            serviceClient = new ERPServiceClient();
            AllSchoolQualificationSubject = new List<z_SchoolQualificationSubject>();
            New();
        }

        #endregion

        #region Refresh Methods
        private void RefreshSchoolSubjectName()
        {
            serviceClient.GetSchoolQualificationSubjectCompleted += (s, e) =>
            {
                try
                {
                    SchoolQualificationSubject = e.Result;
                    if (SchoolQualificationSubject != null)
                        AllSchoolQualificationSubject = SchoolQualificationSubject.ToList();
                }
                catch (Exception)
                {

                }
            };
            serviceClient.GetSchoolQualificationSubjectAsync();
        }

        private void RefreshSchoolQualificationType()
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


        private IEnumerable<z_SchoolQualificationSubject> _SchoolQualificationSubject;
        public IEnumerable<z_SchoolQualificationSubject> SchoolQualificationSubject
        {
            get { return _SchoolQualificationSubject; }
            set { _SchoolQualificationSubject = value; OnPropertyChanged("SchoolQualificationSubject"); }
        }


       
        private z_SchoolQualificationSubject _Current_SchoolQualificationSubject;
        public z_SchoolQualificationSubject Current_SchoolQualificationSubject
        {
            get { return _Current_SchoolQualificationSubject; }
            set { _Current_SchoolQualificationSubject = value; OnPropertyChanged("Current_SchoolQualificationSubject");}
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
            Current_SchoolQualificationSubject = null;
            Current_SchoolQualificationSubject = new z_SchoolQualificationSubject();
            RefreshSchoolQualificationType();
            RefreshSchoolSubjectName();
        }

        public ICommand SaveBtn
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (ValidateSave())
            {
                _Current_SchoolQualificationSubject.save_user_id = clsSecurity.loggedUser.user_id;
                _Current_SchoolQualificationSubject.save_datetime = DateTime.Now;
                _Current_SchoolQualificationSubject.modified_user_id = clsSecurity.loggedUser.user_id;
                _Current_SchoolQualificationSubject.modified_datetime = DateTime.Now;

                if (serviceClient.SaveUpdateSchoolQualificationSubject(_Current_SchoolQualificationSubject))
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
            if (_Current_SchoolQualificationSubject == null || _Current_SchoolQualificationSubject.schoolsubject_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceClient.DeleteSchoolSubject(_Current_SchoolQualificationSubject))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            New();
        }

        private bool ValidateSave()
        {
            if (_Current_SchoolQualificationSubject == null)
            {
                clsMessages.setMessage("Please Resfresh and Try Again!");
                return false;
            }

            else if (string.IsNullOrEmpty(_Current_SchoolQualificationSubject.subject))
            {
                clsMessages.setMessage("Please enter a valid name!");
                return false;
            }

            else if ( _Current_SchoolQualificationSubject.school_qualifiaction_id == 0)
            {

                clsMessages.setMessage("Please Select  QualificationType name!");
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
                if (_Search == "")
                {
                    RefreshSchoolSubjectName();
                }
                else
                {
                    searchTextChanged();
                }
            }
        }

        private void searchTextChanged()
        {
            SchoolQualificationSubject = null;
            SchoolQualificationSubject = AllSchoolQualificationSubject.Where(e => e.subject.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion


        #endregion

    }
}
