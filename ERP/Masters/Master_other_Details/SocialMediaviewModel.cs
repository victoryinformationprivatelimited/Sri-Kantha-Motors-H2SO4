using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Collections;
using System.Windows.Input;

namespace ERP.Masters.Master_other_Details
{
    class SocialMediaviewModel:ViewModelBase
    {

        #region Service Object

        private ERPServiceClient serviceclient = new ERPServiceClient();
        List<z_SocialMedia> AllSocialMedia;

        #endregion

        #region Constructor

        public SocialMediaviewModel()
        {
            serviceclient = new ERPServiceClient();
            AllSocialMedia = new List<z_SocialMedia>();
            refresh();
            New();
        }

      
        #endregion

        #region Properties

        private IEnumerable<z_SocialMedia> _SocialMedia;
        public IEnumerable<z_SocialMedia> SocialMedia
        {
            get { return _SocialMedia; }
            set { _SocialMedia = value; OnPropertyChanged("SocialMedia"); }
        }

        private z_SocialMedia _CurrentSocialMedia;
        public z_SocialMedia CurrentSocialMedia
        {
            get { return _CurrentSocialMedia; }
            set { _CurrentSocialMedia = value; OnPropertyChanged("CurrentSocialMedia");}
        }

        //string socialMediaName;
        //public string SocialMediaName
        //{
        //    get { return socialMediaName; }
        //    set { socialMediaName = value; OnPropertyChanged("SocialMediaName"); }
        //}

        #endregion

        #region Button Command

        #region New Button
        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New);
            }
        }

        private void New()
        {
            CurrentSocialMedia = null;
            CurrentSocialMedia = new z_SocialMedia();
            refresh();
        } 

        #endregion

        #region Save Button
        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }
        private void Save()
        {

            if (_CurrentSocialMedia != null)
            {
              
                z_SocialMedia socialmedia = new z_SocialMedia();
                if (_CurrentSocialMedia.social_media_id == 0)  
                {
                    socialmedia.social_media_name = _CurrentSocialMedia.social_media_name;
                    socialmedia.save_datetime = System.DateTime.Now;
                    socialmedia.save_user_id = clsSecurity.loggedUser.user_id;
                    socialmedia.isdelete = false;
                }
                else
                {
                    
                    socialmedia.social_media_id = _CurrentSocialMedia.social_media_id;
                    socialmedia.social_media_name = _CurrentSocialMedia.social_media_name;
                    socialmedia.modified_user_id = clsSecurity.loggedUser.user_id;
                    socialmedia.modified_datetime = DateTime.Now;
                }

                if (serviceclient.SaveUpdateSocialMedia(socialmedia))
                {
                    clsMessages.setMessage("Record Has Been Saved...");
                }
                else
                {


                }
                refresh();

            }
            
        } 

        #endregion
        public ICommand DeleteBtn
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (_CurrentSocialMedia == null || _CurrentSocialMedia.social_media_id == 0)
                return false;
            else
                return true;
        }
        private void Delete()
        {

            if (serviceclient.DeleteSocialMedia(_CurrentSocialMedia))
                clsMessages.setMessage("Record Deleted Successfully");
            else
                clsMessages.setMessage("Record Delete Failed");

            refresh();
        }

        #endregion

        #region Refresh Method

        private void refresh()
        {

            this.serviceclient.GetSocialMediaCompleted += (s, e) =>
                {
                    SocialMedia = e.Result;
                    if (SocialMedia != null)
                        AllSocialMedia = SocialMedia.ToList();
                   
                };
            this.serviceclient.GetSocialMediaAsync();
            
        }

        #endregion

        #region Search TextBox

        private string _Search;

        public string Search
        {
            get { return _Search; }
            set { _Search = value; OnPropertyChanged("Search"); if (SocialMedia != null) searchTextChanged(); }
        }
        private void searchTextChanged()
        {
            SocialMedia = null;
            SocialMedia = AllSocialMedia.Where(e => e.social_media_name.ToUpper().Contains(Search.ToUpper())).ToList();
        }

        #endregion

      
    }
}
