using ERP.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Message_BOX
{
    public class MessageBOXviewModel : ViewModelBase
    {
        public MessageBOXviewModel()
        {
            CancelVisibilty = Visibility.Collapsed;
        }
        public MessageBOXviewModel(Visibility Cancel)
        {
            CancelVisibilty = Cancel;
        }

        private Visibility _MBVisible;
        public Visibility MBVisible
        {
            get
            {
                return this._MBVisible;
            }
            set
            {
                this._MBVisible = value;
                this.OnPropertyChanged("MBVisible");
            }
        }

        private Visibility _CancelVisibilty;
        public Visibility CancelVisibilty
        {
            get { return _CancelVisibilty; }
            set { _CancelVisibilty = value; OnPropertyChanged("CancelVisibilty"); }
        }


        private string _Content;
        public string Content
        {
            get { return _Content; }
            set { _Content = value; OnPropertyChanged("Content"); }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); }
        }

        private string result;

        public string MyProperty
        {
            get { return result; }
            set { result = value; }
        }

        private string _Result;
        public string Result
        {
            get { return _Result; }
            set { _Result = value; OnPropertyChanged("Result"); }
        }

        public ICommand OK
        {
            get { return new RelayCommand(messageBOXOK); }
        }

        public ICommand Cancel
        {
            get { return new RelayCommand(messageBOXCancel); }
        }

        void messageBOXCancel()
        {
            Result = null;
            Result = Resources.MessageBoxCancel;
            MBVisible = Visibility.Hidden;
        }

        void messageBOXOK()
        {
            Result = null;
            Result = Resources.MessageBoxOK;
            MBVisible = Visibility.Hidden;
        }
    }
}
