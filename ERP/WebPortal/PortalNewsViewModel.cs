using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
using ERP.BasicSearch;
using ERP.Properties;
using Microsoft.Win32;

namespace ERP.WebPortal
{
    class PortalNewsViewModel : ViewModelBase
    {
         #region Fields

        ERPServiceClient serviceClient;
        OpenFileDialog FileBrowser;

        #endregion

        #region Constructor

        public PortalNewsViewModel()
        {
            serviceClient = new ERPServiceClient();
            FileBrowser = new OpenFileDialog();
            FileBrowser.Multiselect = false;
            FileBrowser.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            New();
            SearchIndex = 0;
        }

        #endregion

        #region Properties

        private IEnumerable<z_News> _News;
        public IEnumerable<z_News> News
        {
            get { return _News; }
            set { _News = value; OnPropertyChanged("News"); }
        }

        private z_News _CurrentNews;
        public z_News CurrentNews
        {
            get { return _CurrentNews; }
            set { _CurrentNews = value; OnPropertyChanged("CurrentNews"); }
        }
        

        private string _SearchText;
        public string SearchText 
        {
            get { return _SearchText; }
            set { _SearchText = value; OnPropertyChanged("SearchText");}
        }

        private int _SearchIndex;
        public int SearchIndex
        {
            get { return _SearchIndex; }
            set { _SearchIndex = value; OnPropertyChanged("SearchIndex"); }
        }
               
        #endregion

        #region RefreshMethods


        private void RefreshNews()
        {
            serviceClient.GetNewsCompleted += (s, e) => 
            {
                News = e.Result;
            };
            serviceClient.GetNewsAsync();
        }
        
        #endregion

        #region Commands & Methods

        public ICommand NewBtn
        {
            get { return new RelayCommand(New); }
        }

        private void New()
        {
            CurrentNews = null;
            CurrentNews = new z_News();
            CurrentNews.News_ID = Guid.NewGuid();
            CurrentNews.Is_Visible = true;
            RefreshNews();
        }

        public ICommand SaveBtn
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if (clsSecurity.GetSavePermission(1103) && clsSecurity.GetUpdatePermission(1103))
            {
                if (CurrentNews == null)
                    clsMessages.setMessage("Please Resfresh and try again.");
                else if (string.IsNullOrEmpty(CurrentNews.Heading))
                    clsMessages.setMessage("Please enter a Heading for this News.");
                else if (string.IsNullOrEmpty(CurrentNews.Image))
                    clsMessages.setMessage("Please Select an Image for this News.");
                else
                {
                    CurrentNews.SaveUser_ID = clsSecurity.loggedUser.user_id;

                    if (serviceClient.SaveNews(CurrentNews))
                        clsMessages.setMessage("News saved/updated successfully");
                    else
                        clsMessages.setMessage("News save/update failed");
                    New();
                }
            }
            else
                clsMessages.setMessage("You Don't Have Permission to Save or Update in this Form...");
        }

        public ICommand DeleteBtn
        {
            get { return new RelayCommand(Delete, DeleteCanexecute); }
        }

        private bool DeleteCanexecute()
        {
            if (CurrentNews != null)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            if (clsSecurity.GetDeletePermission(1103))
            {
                if (News != null && News.Count(c => c.News_ID == CurrentNews.News_ID) > 0)
                {
                    clsMessages.setMessage("Are you sure that you want to Delete this News?", Visibility.Visible);
                    if (clsMessages.Messagebox.viewModel.Result == Resources.MessageBoxOK)
                    {
                        if (serviceClient.DeleteNews(CurrentNews))
                            clsMessages.setMessage("News Deleted Successfully");
                        else
                            clsMessages.setMessage("News Delete Failed");
                        New();
                    }
                }
            }
            else
                clsMessages.setMessage("You Don't Have Permission to Delete in this Form...");
        }

        public ICommand AddimageBtn 
        {
            get { return new RelayCommand(AddImage, AddimageCanexecute); }
        }

        private void AddImage()
        {
            FileBrowser.ShowDialog();
            CurrentNews.Image = FileBrowser.FileName;
        }

        private bool AddimageCanexecute()
        {
            if (CurrentNews != null)
                return true;
            else
                return false;
        }

        #endregion
    }
}
