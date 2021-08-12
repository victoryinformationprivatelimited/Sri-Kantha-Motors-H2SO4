using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.AttendanceService;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    public class SearchShiftViewModel:ViewModelBase
    {
        #region Attendance Service Client

        AttendanceServiceClient serviceClient;

        #endregion

        #region Constructor

        public SearchShiftViewModel()
        {
            serviceClient = new AttendanceServiceClient();
            this.RefreshShifts();
        }

        #endregion

        #region List Members

        List<ShiftMasterDetailsView> allShiftsList = new List<ShiftMasterDetailsView>();

        #endregion

        #region Properties

        IEnumerable<ShiftMasterDetailsView> shiftDetails;
        public IEnumerable<ShiftMasterDetailsView> ShiftDetails
        {
            get { if (shiftDetails != null) shiftDetails = shiftDetails.OrderBy(c => c.order_number); return shiftDetails; }
            set { shiftDetails = value; OnPropertyChanged("ShiftDetails"); }
        }

        ShiftMasterDetailsView currentShiftDetail;
        public ShiftMasterDetailsView CurrentShiftDetail
        {
            get { return currentShiftDetail; }
            set { currentShiftDetail = value; OnPropertyChanged("CurrentShiftDetail"); }
        }

        IEnumerable<z_Shift_Category> shiftCategories;
        public IEnumerable<z_Shift_Category> ShiftCategories
        {
            get { return shiftCategories; }
            set { shiftCategories = value; OnPropertyChanged("ShiftCategories"); }
        }

        z_Shift_Category currentShiftCategory;
        public z_Shift_Category CurrentShiftCategory
        {
            get { return currentShiftCategory; }
            set 
            { 
                currentShiftCategory = value; OnPropertyChanged("CurrentShiftCategory");
                this.FilterShifts();
            }
        }


        bool isDailyShift;
        public bool IsDailyShift
        {
            get { return isDailyShift; }
            set 
            { 
                isDailyShift = value; OnPropertyChanged("IsDailyShift");
                this.FilterShifts();
            }
        }

        bool isRosterShift;
        public bool IsRosterShift
        {
            get { return isRosterShift; }
            set 
            { 
                isRosterShift = value; OnPropertyChanged("IsRosterShift");
                this.FilterShifts();
            }
        }

        #endregion

        #region Refresh Methdos

        void RefreshShifts()
        {
            serviceClient.GetShiftMasterDetailsCompleted += (s, e) =>
                {
                    ShiftDetails = e.Result;
                    if (shiftDetails != null)
                    {
                        allShiftsList = shiftDetails.ToList();
                        this.FilterCategories();
                    }
                        
                };

            serviceClient.GetShiftMasterDetailsAsync();
        }

        #endregion

        #region Filter Methods

        void FilterShifts()
        {
            ShiftDetails = null;
            ShiftDetails = allShiftsList;
            if (currentShiftCategory != null)
            {
                ShiftDetails = shiftDetails.Where(c => c.shift_category_id == currentShiftCategory.shift_category_id);
            }
            if(isDailyShift)
            {
                ShiftDetails = shiftDetails.Where(c => c.is_daily_shift == isDailyShift);
            }

            if(isRosterShift)
            {
                ShiftDetails = shiftDetails.Where(c => c.is_roster == isRosterShift);
            }
        }

        void FilterCategories()
        {
            if(allShiftsList.Count > 0)
            {
                ShiftCategories = allShiftsList.Where(c => c.shift_category_id != null).GroupBy(d => d.shift_category_id).Select(grp => grp.First()).Select(c => new z_Shift_Category { shift_category_id = (int)c.shift_category_id, shift_category_name = c.shift_category_name });
            }
        }

        #endregion

    }
}
