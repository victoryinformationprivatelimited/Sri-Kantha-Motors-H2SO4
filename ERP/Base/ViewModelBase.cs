using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace ERP
{
    public class ViewModelBase : INotifyPropertyChanged,IDataErrorInfo
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            try
            {
                PropertyChangedEventHandler propertyChanged = PropertyChanged;

                if (propertyChanged != null)
                    propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception)
            {

                clsMessages.setMessage("Property Changed Error !");
            }
        }

        protected object GetValue(NotifyingProperty notifyingProperty)
        {
            return notifyingProperty.Value;
        }

        protected void SetValue(NotifyingProperty notifyingProperty, object value, bool forceUpdate = false)
        {
            if (notifyingProperty.Value != value || forceUpdate)
            {
                notifyingProperty.Value = value;

                OnPropertyChanged(notifyingProperty.Name);
            }
        }

        #region Error Interface

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        string IDataErrorInfo.this[string PropertyName]
        {
            get
            {
                return getValidationError(PropertyName);
            }
        }

        public virtual string getValidationError(string PropertyName)
        {
            return "";
        }

        #endregion
    }
}
