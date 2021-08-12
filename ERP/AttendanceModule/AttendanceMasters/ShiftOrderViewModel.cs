using ERP.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class ShiftOrderViewModel:ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor

        public ShiftOrderViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            RefreshShifts();
        }

        #endregion

        #region List Members

        List<dtl_Shift_Master> shiftsList = new List<dtl_Shift_Master>();

        #endregion

        #region Properties

        IEnumerable<dtl_Shift_Master> shifts;
        public IEnumerable<dtl_Shift_Master> Shifts
        {
            get { if (shifts != null) shifts = shifts.OrderBy(c => c.dtl_ShiftOrder.order_number); return shifts; }
            set { shifts = value; OnPropertyChanged("Shifts"); }
        }

        dtl_Shift_Master currentShift;
        public dtl_Shift_Master CurrentShift
        {
            get { return currentShift; }
            set { currentShift = value; OnPropertyChanged("CurrentShift"); }
        }

        #endregion

        #region Refresh Methods

        void RefreshShifts()
        {
            attendServiceClient.GetShiftOrderDetailsCompleted += (s, e) =>
            {
                try
                {
                    Shifts = e.Result;
                    if (shifts != null)
                        shiftsList = shifts.ToList();
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Shift order details refresh is failed");
                }
                
            };
            attendServiceClient.GetShiftOrderDetailsAsync();
        }
        
        #endregion

        #region Button Methods

        #region Up Button

        public ICommand UpButton
        {
            get { return new RelayCommand(OneUp); }
        }

        private void OneUp()
        {
            if(currentShift != null)
            {
                dtl_Shift_Master selectedShift = shiftsList.FirstOrDefault(c => c.shift_detail_id == currentShift.shift_detail_id);
                dtl_Shift_Master nextUpShift = shiftsList.OrderBy(c => c.dtl_ShiftOrder.order_number).FirstOrDefault(c => c.dtl_ShiftOrder.order_number == selectedShift.dtl_ShiftOrder.order_number - 1);
                if(nextUpShift != null)
                {
                    selectedShift.dtl_ShiftOrder.order_number--;
                    nextUpShift.dtl_ShiftOrder.order_number++;
                    Shifts = shiftsList;
                }
            }
        }
        
        #endregion

        #region Down Button

        public ICommand DownButton
        {
            get { return new RelayCommand(OneDown); }
        }

        private void OneDown()
        {
            if (currentShift != null)
            {
                dtl_Shift_Master selectedShift = shiftsList.FirstOrDefault(c => c.shift_detail_id == currentShift.shift_detail_id);
                dtl_Shift_Master nextDownShift = shiftsList.OrderBy(c => c.dtl_ShiftOrder.order_number).FirstOrDefault(c => c.dtl_ShiftOrder.order_number == selectedShift.dtl_ShiftOrder.order_number + 1);
                if (nextDownShift != null)
                {
                    selectedShift.dtl_ShiftOrder.order_number++;
                    nextDownShift.dtl_ShiftOrder.order_number--;
                    Shifts = shiftsList;
                }
            }
        }

        #endregion

        #region Save Button

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        private void Save()
        {
            if(shiftsList.Count > 0)
            {
                if(attendServiceClient.UpdateShiftOrderDetails(shiftsList.ToArray()))
                {
                    clsMessages.setMessage("Shift Order is saved successfully");
                    this.RefreshShifts();
                }
                else
                {
                    clsMessages.setMessage("Shift Order save is failed");
                }
            }
        }

        #endregion

        #endregion
    }
}
