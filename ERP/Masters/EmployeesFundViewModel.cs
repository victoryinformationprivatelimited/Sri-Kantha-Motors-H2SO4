using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP
{
    class EmployeesFundViewModel : INotifyPropertyChanged
    {
        #region Service Object
        private ERPServiceClient serviceClient = new ERPServiceClient();
        #endregion

        public EmployeesFundViewModel()
        {

        }

        #region Event Properties
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private IEnumerable<z_EmployeesFund> _EPF;
        public IEnumerable<z_EmployeesFund> EPF
        {
            get
            {
                return this._EPF;
            }
            set
            {
                this._EPF = value;
                this.OnPropertyChanged("EPF");
            }
        }
        private z_EmployeesFund _CurrentEPF;
        public z_EmployeesFund CurrentEPF
        {
            get
            {
                return this._CurrentEPF;
            }
            set
            {
                this._CurrentEPF = value;
                this.OnPropertyChanged("CurrentEPF");
            }
        }

        private IEnumerable<z_EmployeesFund> _ETF;
        public IEnumerable<z_EmployeesFund> ETF
        {
            get
            {
                return this._ETF;
            }
            set
            {
                this._ETF = value;
                this.OnPropertyChanged("ETF");
            }
        }

        private z_EmployeesFund _CurrentETF;
        public z_EmployeesFund CurrentETF
        {
            get
            {
                return this._CurrentETF;
            }
            set
            {
                this._CurrentETF = value;
                this.OnPropertyChanged("CurrentETF");
            }
        }

        private double _scaleSize;
        public double ScaleSize
        {
            get { return _scaleSize; }
            set { _scaleSize = value; OnPropertyChanged("ScaleSize"); }
        }


        public void scale()
        {
            ScaleSize = 1;
            double h = System.Windows.SystemParameters.PrimaryScreenHeight;
            double w = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (h * w == 1366 * 768)
                ScaleSize = 0.90;
            if (w * h == 1280 * 768)
                ScaleSize = 0.90;
            if (w * h == 1024 * 768)
                ScaleSize = 1.5;
        }
    }


}
