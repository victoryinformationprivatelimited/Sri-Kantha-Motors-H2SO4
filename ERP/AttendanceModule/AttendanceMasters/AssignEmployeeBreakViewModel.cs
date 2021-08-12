using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class AssignEmployeeBreakViewModel:ViewModelBase
    {
        #region Service Client
        
        #endregion

        #region Constructor
        
        #endregion

        #region Properties

        bool isPopUp;
        public bool IsPopUp
        {
            get { return isPopUp; }
            set { isPopUp = value; OnPropertyChanged("IsPopUp"); }
        }

        #endregion

        #region Refresh Methods

        #endregion

        #region Button Methods

        #region Select Date Button
        
        public ICommand SelectDateButton
        {
            get { return new RelayCommand(SelecteDate); }
        }

        private void SelecteDate()
        {
            IsPopUp = true;
        }

        #endregion

        #endregion


    }
}
