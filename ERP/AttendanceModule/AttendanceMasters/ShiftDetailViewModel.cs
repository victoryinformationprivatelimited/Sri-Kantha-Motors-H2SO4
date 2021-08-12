using ERP.AttendanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.AttendanceModule.AttendanceMasters
{
    class ShiftDetailViewModel : ViewModelBase
    {
        #region Service Client

        AttendanceServiceClient attendServiceClient;

        #endregion

        #region Constructor

        public ShiftDetailViewModel()
        {
            attendServiceClient = new AttendanceServiceClient();
            this.RefreshShiftCategories();
            this.RefreshDayRanges();
            this.RefreshAllShiftDetails();
            SetValidatingProperties();
            this.New();
        }

        #endregion

        #region Class data members

        #region Basic shift details date values

        DateTime shiftInDayTime;
        DateTime shiftOutDayTime;
        DateTime shiftOnDayTime;
        DateTime shiftOffDayTime;

        #endregion

        #region Additional days list

        List<dtl_Shift_Additional_Day> addedDaysList = new List<dtl_Shift_Additional_Day>();
        List<dtl_Shift_Additional_Day> removedDaysList = new List<dtl_Shift_Additional_Day>();

        #endregion

        #region Shift breaks list

        List<dtl_Shift_Break_Details> addedBreakList = new List<dtl_Shift_Break_Details>();
        List<dtl_Shift_Break_Details> removedBreakList = new List<dtl_Shift_Break_Details>();

        #endregion

        //Shift Covering 2017-07-24
        #region Shift Covering list

        List<dtl_Shift_Covering_Details> addCoverList = new List<dtl_Shift_Covering_Details>();
        List<dtl_Shift_Covering_Details> removedCoverList = new List<dtl_Shift_Covering_Details>();

        #endregion

        #region shift properties for validation

        List<string> validatingPropertyList = new List<string>();
        string validatedProperty;

        #endregion

        #endregion

        #region Properties

        #region Main shift properties

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
                if (currentShiftCategory != null)
                {
                    ShiftDetailName = "";
                    this.RefreshShifts();
                }
            }
        }

        string shiftDetailName;
        public string ShiftDetailName
        {
            get { return shiftDetailName; }
            set { shiftDetailName = value; OnPropertyChanged("ShiftDetailName"); }
        }

        IEnumerable<z_ShiftDayRange> shiftDayRanges;
        public IEnumerable<z_ShiftDayRange> ShiftDayRanges
        {
            get { return shiftDayRanges; }
            set { shiftDayRanges = value; OnPropertyChanged("ShiftDayRanges"); }
        }

        z_ShiftDayRange shiftInDayRange;
        public z_ShiftDayRange ShiftInDayRange
        {
            get { return shiftInDayRange; }
            set
            {
                shiftInDayRange = value; OnPropertyChanged("ShiftInDayRange");
                if (shiftInDayRange != null)
                {
                    this.SetShiftDayTimes();
                }
            }
        }

        z_ShiftDayRange shiftOutDayRange;
        public z_ShiftDayRange ShiftOutDayRange
        {
            get { return shiftOutDayRange; }
            set
            {
                shiftOutDayRange = value; OnPropertyChanged("ShiftOutDayRange");
                if (shiftOutDayRange != null)
                {
                    this.SetShiftDayTimes();
                }
            }
        }

        z_ShiftDayRange shiftOnDayRange;
        public z_ShiftDayRange ShiftOnDayRange
        {
            get { return shiftOnDayRange; }
            set { shiftOnDayRange = value; OnPropertyChanged("ShiftOnDayRange"); }
        }

        z_ShiftDayRange shiftOffDayRange;
        public z_ShiftDayRange ShiftOffDayRange
        {
            get { return shiftOffDayRange; }
            set { shiftOffDayRange = value; OnPropertyChanged("ShiftOffDayRange"); }
        }

        IEnumerable<ShiftDetailAllView> allShiftDetails;
        public IEnumerable<ShiftDetailAllView> AllShiftDetails
        {
            get { return allShiftDetails; }
            set { allShiftDetails = value; OnPropertyChanged("AllShiftDetails"); }
        }

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
            set
            {
                currentShift = value; OnPropertyChanged("CurrentShift");
                if (currentShift != null)
                {
                    CurrentShiftDetail = null;
                    CurrentShiftDetail = allShiftDetails.FirstOrDefault(c => c.shift_detail_id == currentShift.shift_detail_id);
                }
            }
        }

        ShiftDetailAllView currentShiftDetail;
        public ShiftDetailAllView CurrentShiftDetail
        {
            get { return currentShiftDetail; }
            set
            {
                currentShiftDetail = value; OnPropertyChanged("CurrentShiftDetail");
                if (currentShiftDetail != null && currentShiftDetail.shift_detail_id != 0)
                {
                    // setting shift basic details to empty
                    ShiftInTime = null;
                    ShiftOutTime = null;
                    ShiftOnTimeDuration = null;
                    ShiftOffTimeDuration = null;
                    this.PopulateCurrentShiftDetails();
                    this.RefreshShiftCoverDetails();
                }

            }
        }

        bool isDailyShift;
        public bool IsDailyShift
        {
            get { return isDailyShift; }
            set { isDailyShift = value; OnPropertyChanged("IsDailyShift"); }
        }

        bool isRosterShift;
        public bool IsRosterShift
        {
            get { return isRosterShift; }
            set { isRosterShift = value; OnPropertyChanged("IsRosterShift"); }
        }

        bool isSplitShift;
        public bool IsSplitShift
        {
            get { return isSplitShift; }
            set { isSplitShift = value; OnPropertyChanged("IsSplitShift"); }
        }

        bool isEntitleLeiuLeave;
        public bool IsEntitleLeiuLeave
        {
            get { return isEntitleLeiuLeave; }
            set { isEntitleLeiuLeave = value; OnPropertyChanged("IsEntitleLeiuLeave"); }
        }

        bool _IsOfficeExecutive;
        public bool IsOfficeExecutive
        {
            get { return _IsOfficeExecutive; }
            set { _IsOfficeExecutive = value; OnPropertyChanged("IsOfficeExecutive"); }
        }

        bool _IsStoresExecutive;
        public bool IsStoresExecutive
        {
            get { return _IsStoresExecutive; }
            set { _IsStoresExecutive = value; OnPropertyChanged("IsStoresExecutive"); }
        }

        bool _IsOfficeNonExecutive;
        public bool IsOfficeNonExecutive
        {
            get { return _IsOfficeNonExecutive; }
            set { _IsOfficeNonExecutive = value; OnPropertyChanged("IsOfficeNonExecutive"); }
        }

        bool _IsStoresNonExecutive;
        public bool IsStoresNonExecutive
        {
            get { return _IsStoresNonExecutive; }
            set { _IsStoresNonExecutive = value; OnPropertyChanged("IsStoresNonExecutive"); }
        }

        bool isSecurity;
        public bool IsSecurity
        {
            get { return isSecurity; }
            set { isSecurity = value; OnPropertyChanged("IsSecurity"); }
        }

        bool isOTShift;
        public bool IsOTShift
        {
            get { return isOTShift; }
            set { isOTShift = value; OnPropertyChanged("IsOTShift"); }
        }

        bool isSingleOT;
        public bool IsSingleOT
        {
            get { return isSingleOT; }
            set { isSingleOT = value; OnPropertyChanged("IsSingleOT"); }
        }

        bool isMultipleOT;
        public bool IsMultipleOT
        {
            get { return isMultipleOT; }
            set { isMultipleOT = value; OnPropertyChanged("IsMultipleOT"); }
        }

        bool isHalfDay;
        public bool IsHalfDay
        {
            get { return isHalfDay; }
            set { isHalfDay = value; OnPropertyChanged("IsHalfDay"); }
        }

        private bool hasBreak;

        public bool HasBreak
        {
            get { return hasBreak; }
            set { hasBreak = value; OnPropertyChanged("HasBreak"); }
        }

        //m 2021-06-07
        bool _IsSpecShift;
        public bool IsSpecShift
        {
            get { return _IsSpecShift; }
            set { _IsSpecShift = value; OnPropertyChanged("IsSpecShift"); }
        }

        bool _IsMorIncentive;
        public bool IsMorIncentive
        {
            get { return _IsMorIncentive; }
            set { _IsMorIncentive = value; OnPropertyChanged("IsMorIncentive"); }
        }

        #region MCN Open Shift

        private bool isOpenShift;

        public bool IsOpenShift
        {
            get { return isOpenShift; }
            set { isOpenShift = value; OnPropertyChanged("IsOpenShift"); OpenShiftBoolConvert(); }
        }

        private bool isBoolConvert;

        public bool IsBoolConvert
        {
            get { return isBoolConvert; }
            set { isBoolConvert = value; OnPropertyChanged("IsBoolConvert"); }
        }

        private bool isOpenShiftLate;

        public bool IsOpenShiftLate
        {
            get { return isOpenShiftLate; }
            set { isOpenShiftLate = value; OnPropertyChanged("IsOpenShiftLate"); }
        }

        private bool isOpenShiftOT;

        public bool IsOpenShiftOT
        {
            get { return isOpenShiftOT; }
            set { isOpenShiftOT = value; OnPropertyChanged("IsOpenShiftOT"); }
        }

        #endregion

        #endregion

        #region Basic details properties

        #region Time values

        TimeSpan? shiftOnTimeDuration;
        public TimeSpan? ShiftOnTimeDuration
        {
            get { return shiftOnTimeDuration; }
            set
            {
                shiftOnTimeDuration = value; OnPropertyChanged("ShiftOnTimeDuration");
                if (shiftOnTimeDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.shift_on_time = (int)((TimeSpan)shiftOnTimeDuration).TotalMinutes;
                    this.SetShiftDayTimes();
                }

            }
        }

        TimeSpan? shiftOffTimeDuration;
        public TimeSpan? ShiftOffTimeDuration
        {
            get { return shiftOffTimeDuration; }
            set
            {
                shiftOffTimeDuration = value; OnPropertyChanged("ShiftOffTimeDuration");
                if (shiftOffTimeDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.shift_off_time = (int)((TimeSpan)shiftOffTimeDuration).TotalMinutes;
                    this.SetShiftDayTimes();
                }

            }
        }

        TimeSpan? shiftInTime;
        public TimeSpan? ShiftInTime
        {
            get { return shiftInTime; }
            set
            {
                shiftInTime = value; OnPropertyChanged("ShiftInTime");
                if (shiftInTime != null)
                {
                    SetShiftDayTimes();
                }
            }
        }

        TimeSpan? shiftOutTime;
        public TimeSpan? ShiftOutTime
        {
            get { return shiftOutTime; }
            set
            {
                shiftOutTime = value; OnPropertyChanged("ShiftOutTime");
                if (shiftOutTime != null)
                {
                    SetShiftDayTimes();
                }
            }
        }

        #endregion

        #region Day values

        int shiftOnDayValue;
        public int ShiftOnDayValue
        {
            get { return shiftOnDayValue; }
            set { shiftOnDayValue = value; OnPropertyChanged("ShiftOnDayValue"); }
        }

        int shiftOffDayValue;
        public int ShiftOffDayValue
        {
            get { return shiftOffDayValue; }
            set { shiftOffDayValue = value; OnPropertyChanged("ShiftOffDayValue"); }
        }

        int shiftInDayValue;
        public int ShiftInDayValue
        {
            get { return shiftInDayValue; }
            set { shiftInDayValue = value; OnPropertyChanged("ShiftInDayValue"); }
        }

        int shiftOutDayValue;
        public int ShiftOutDayValue
        {
            get { return shiftOutDayValue; }
            set { shiftOutDayValue = value; OnPropertyChanged("ShiftOutDayValue"); }
        }

        #endregion

        #endregion

        #region Pre-OT properties

        DateTime preNoOtTime;
        public DateTime PreNoOtTime
        {
            get { return preNoOtTime; }
            set
            {
                preNoOtTime = value; OnPropertyChanged("PreNoOtTime");
            }
        }

        DateTime preSingleOtTime;
        public DateTime PreSingleOtTime
        {
            get { return preSingleOtTime; }
            set
            {
                preSingleOtTime = value; OnPropertyChanged("PreSingleOtTime");
            }
        }

        DateTime preDoubleOtTime;
        public DateTime PreDoubleOtTime
        {
            get { return preDoubleOtTime; }
            set
            {
                preDoubleOtTime = value; OnPropertyChanged("PreDoubleOtTime");
            }
        }

        DateTime preTripleOtTime;
        public DateTime PreTripleOtTime
        {
            get { return preTripleOtTime; }
            set
            {
                preTripleOtTime = value; OnPropertyChanged("PreTripleOtTime");
            }
        }

        TimeSpan? preNoOtDuration;
        public TimeSpan? PreNoOtDuration
        {
            get { return preNoOtDuration; }
            set
            {
                preNoOtDuration = value; OnPropertyChanged("PreNoOtDuration");
                if (currentShiftDetail != null && preNoOtDuration != null)
                {
                    currentShiftDetail.pre_non_ot = (int)(((TimeSpan)preNoOtDuration).TotalMinutes);
                    this.SetShiftMorningTimes();
                }

            }
        }

        TimeSpan? preSingleOtDuration;
        public TimeSpan? PreSingleOtDuration
        {
            get { return preSingleOtDuration; }
            set
            {
                preSingleOtDuration = value; OnPropertyChanged("PreSingleOtDuration");
                if (currentShiftDetail != null && preSingleOtDuration != null)
                {
                    currentShiftDetail.pre_single_ot = (int)((TimeSpan)preSingleOtDuration).TotalMinutes;
                    this.SetShiftMorningTimes();
                }
            }
        }

        TimeSpan? preDoubleOtDuration;
        public TimeSpan? PreDoubleOtDuration
        {
            get { return preDoubleOtDuration; }
            set
            {
                preDoubleOtDuration = value; OnPropertyChanged("PreDoubleOtDuration");
                if (currentShiftDetail != null && preDoubleOtDuration != null)
                {
                    currentShiftDetail.pre_double_ot = (int)((TimeSpan)preDoubleOtDuration).TotalMinutes;
                    this.SetShiftMorningTimes();
                }
            }
        }

        TimeSpan? preTripleOtDuration;
        public TimeSpan? PreTripleOtDuration
        {
            get { return preTripleOtDuration; }
            set
            {
                preTripleOtDuration = value; OnPropertyChanged("PreTripleOtDuration");
                if (currentShiftDetail != null && preTripleOtDuration != null)
                {
                    currentShiftDetail.pre_triple_ot = (int)((TimeSpan)preTripleOtDuration).TotalMinutes;
                    this.SetShiftMorningTimes();
                }
            }
        }

        #region Pre-OT round up

        int preSingleOtRound;
        public int PreSingleOtRound
        {
            get { return preSingleOtRound; }
            set
            {
                preSingleOtRound = value; OnPropertyChanged("PreSingleOtRound");
                if (currentShiftDetail != null)
                    currentShiftDetail.pre_single_ot_roundup = preSingleOtRound;
            }
        }

        int preDoubleOtRound;
        public int PreDoubleOtRound
        {
            get { return preDoubleOtRound; }
            set
            {
                preDoubleOtRound = value; OnPropertyChanged("PreDoubleOtRound");
                if (currentShiftDetail != null)
                    currentShiftDetail.pre_double_ot_roundup = preDoubleOtRound;
            }
        }

        int preTripleOtRound;
        public int PreTripleOtRound
        {
            get { return preTripleOtRound; }
            set
            {
                preTripleOtRound = value; OnPropertyChanged("PreTripleOtRound");
                if (currentShiftDetail != null)
                    currentShiftDetail.pre_triple_ot_roundup = preTripleOtRound;
            }
        }

        #endregion

        #region Pre-OT compensation

        int preNonOtCompensate;

        public int PreNonOtCompensate
        {
            get { return preNonOtCompensate; }
            set
            {
                preNonOtCompensate = value; OnPropertyChanged("PreNonOtCompensate");
                if (currentShiftDetail != null)
                    currentShiftDetail.pre_non_ot_compensate = preNonOtCompensate;
            }
        }

        #endregion

        #region Pre-OT Checkboxes

        bool isPreNoOtChecked;
        public bool IsPReNoOtChecked
        {
            get { return isPreNoOtChecked; }
            set { isPreNoOtChecked = value; OnPropertyChanged("IsNoOtChecked"); }
        }

        bool isPreSingleOtChecked;
        public bool IsPreSingleOtChecked
        {
            get { return isPreSingleOtChecked; }
            set { isPreSingleOtChecked = value; OnPropertyChanged("IsSingleOtChecked"); }
        }

        bool isPreDoubleOtChecked;
        public bool IsPreDoubleOtChecked
        {
            get { return isPreDoubleOtChecked; }
            set { isPreDoubleOtChecked = value; OnPropertyChanged("IsDoubleOtChecked"); }
        }

        bool isPreTripleOtChecked;
        public bool IsPreTripleOtChecked
        {
            get { return isPreTripleOtChecked; }
            set { isPreTripleOtChecked = value; OnPropertyChanged("IsTripleOtChecked"); }
        }

        #endregion

        #endregion

        #region Post-OT properties

        DateTime postNoOtTime;
        public DateTime PostNoOtTime
        {
            get { return postNoOtTime; }
            set { postNoOtTime = value; OnPropertyChanged("PostNoOtTime"); }
        }

        TimeSpan? postNoOtDuration;
        public TimeSpan? PostNoOtDuration
        {
            get { return postNoOtDuration; }
            set
            {
                postNoOtDuration = value; OnPropertyChanged("PostNoOtDuration");
                if (currentShiftDetail != null && postNoOtDuration != null)
                {
                    currentShiftDetail.post_non_ot = (int)((TimeSpan)postNoOtDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }
            }
        }

        DateTime postSingleOtTime;
        public DateTime PostSingleOtTime
        {
            get { return postSingleOtTime; }
            set { postSingleOtTime = value; OnPropertyChanged("PostSingleOtTime"); }
        }

        TimeSpan? postSingleOtDuration;
        public TimeSpan? PostSingleOtDuration
        {
            get { return postSingleOtDuration; }
            set
            {
                postSingleOtDuration = value; OnPropertyChanged("PostSingleOtDuration");
                if (currentShiftDetail != null && postSingleOtDuration != null)
                {
                    currentShiftDetail.post_single_ot = (int)((TimeSpan)postSingleOtDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }
            }
        }

        DateTime postDoubleOtTime;
        public DateTime PostDoubleOtTime
        {
            get { return postDoubleOtTime; }
            set { postDoubleOtTime = value; OnPropertyChanged("PostDoubleOtTime"); }
        }

        TimeSpan? postDoubleOtDuration;
        public TimeSpan? PostDoubleOtDuration
        {
            get { return postDoubleOtDuration; }
            set
            {
                postDoubleOtDuration = value; OnPropertyChanged("PostDoubleOtDuration");
                if (currentShiftDetail != null && postDoubleOtDuration != null)
                {
                    currentShiftDetail.post_double_ot = (int)((TimeSpan)postDoubleOtDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }
            }
        }

        DateTime postTripleOtTime;
        public DateTime PostTripleOtTime
        {
            get { return postTripleOtTime; }
            set
            {
                postTripleOtTime = value; OnPropertyChanged("PostTripleOtTime");

            }
        }

        TimeSpan? postTripleOtDuration;
        public TimeSpan? PostTripleOtDuration
        {
            get { return postTripleOtDuration; }
            set
            {
                postTripleOtDuration = value; OnPropertyChanged("PostTripleOtDuration");
                if (currentShiftDetail != null && postTripleOtDuration != null)
                {
                    currentShiftDetail.post_triple_ot = (int)((TimeSpan)postTripleOtDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }
            }
        }

        #region Post-OT round up

        int postSingleOtRound;
        public int PostSingleOtRound
        {
            get { return postSingleOtRound; }
            set
            {
                postSingleOtRound = value; OnPropertyChanged("PostSingleOtRound");
                if (currentShiftDetail != null)
                    currentShiftDetail.post_single_ot_roundup = postSingleOtRound;
            }
        }

        int postDoubleOtRound;
        public int PostDoubleOtRound
        {
            get { return postDoubleOtRound; }
            set
            {
                postDoubleOtRound = value; OnPropertyChanged("PostDoubleOtRound");
                if (currentShiftDetail != null)
                    currentShiftDetail.post_double_ot_roundup = postDoubleOtRound;
            }
        }

        int postTripleOtRound;
        public int PostTripleOtRound
        {
            get { return postTripleOtRound; }
            set
            {
                postTripleOtRound = value; OnPropertyChanged("PostTripleOtRound");
                if (currentShiftDetail != null)
                    currentShiftDetail.post_triple_ot_roundup = postTripleOtRound;
            }
        }

        #endregion

        #region Post-OT compensation

        int postNonOtCompensate;
        public int PostNonOtCompensate
        {
            get { return postNonOtCompensate; }
            set
            {
                postNonOtCompensate = value; OnPropertyChanged("PostNonOtCompensate");
                if (currentShiftDetail != null)
                    currentShiftDetail.post_non_ot_compensate = postNonOtCompensate;
            }
        }

        #endregion

        #region Post-OT Checkboxes

        bool isPostNoOtChecked;
        public bool IsPostNoOtChecked
        {
            get { return isPostNoOtChecked; }
            set { isPostNoOtChecked = value; OnPropertyChanged("IsPostNoOtChecked"); }
        }

        bool isPostSingleOtChecked;
        public bool IsPostSingleOtChecked
        {
            get { return isPostSingleOtChecked; }
            set { isPostSingleOtChecked = value; OnPropertyChanged("IsPostSingleOtChecked"); }
        }

        bool isPostDoubleOtChecked;
        public bool IsPostDoubleOtChecked
        {
            get { return isPostDoubleOtChecked; }
            set { isPostDoubleOtChecked = value; OnPropertyChanged("IsPostDoubleOtChecked"); }
        }

        bool isPostTripleOtChecked;
        public bool IsPostTripleOtChecked
        {
            get { return isPostTripleOtChecked; }
            set { isPostTripleOtChecked = value; OnPropertyChanged("IsPostTripleOtChecked"); }
        }

        #endregion

        #endregion

        #region Late details properties

        TimeSpan? lateInTimeDuration;
        public TimeSpan? LateInTimeDuration
        {
            get { return lateInTimeDuration; }
            set
            {
                lateInTimeDuration = value; OnPropertyChanged("LateInTimeDuration");
                if (currentShiftDetail != null && lateInTimeDuration != null)
                {
                    currentShiftDetail.late_in = (int)((TimeSpan)lateInTimeDuration).TotalMinutes;
                    this.SetShiftMorningTimes();
                }
            }
        }

        DateTime lateInTime;
        public DateTime LateInTime
        {
            get { return lateInTime; }
            set { lateInTime = value; OnPropertyChanged("LateInTime"); }
        }

        TimeSpan? lateShotLeaveTimeDuration;
        public TimeSpan? LateShotLeaveTimeDuration
        {
            get { return lateShotLeaveTimeDuration; }
            set
            {
                lateShotLeaveTimeDuration = value; OnPropertyChanged("LateShotLeaveTimeDuration");
                if (currentShiftDetail != null && lateShotLeaveTimeDuration != null)
                {
                    currentShiftDetail.morning_short_leave = (int)((TimeSpan)lateShotLeaveTimeDuration).TotalMinutes;
                    this.SetShiftMorningTimes();
                }
            }
        }

        DateTime lateShortLeaveTime;
        public DateTime LateShortLeaveTime
        {
            get { return lateShortLeaveTime; }
            set { lateShortLeaveTime = value; OnPropertyChanged("LateShortLeaveTime"); }
        }

        TimeSpan? lateHalfDayTimeDuration;
        public TimeSpan? LateHalfDayTimeDuration
        {
            get { return lateHalfDayTimeDuration; }
            set
            {
                lateHalfDayTimeDuration = value; OnPropertyChanged("LateHalfDayTimeDuration");
                if (currentShiftDetail != null && lateHalfDayTimeDuration != null)
                {
                    currentShiftDetail.morning_halfday_leave = (int)((TimeSpan)lateHalfDayTimeDuration).TotalMinutes;
                    this.SetShiftMorningTimes();
                }
            }
        }

        DateTime lateHalfDayTime;
        public DateTime LateHalfDayTime
        {
            get { return lateHalfDayTime; }
            set { lateHalfDayTime = value; OnPropertyChanged("LateHalfDayTime"); }
        }

        TimeSpan? lateFullLeaveTimeDuration;
        public TimeSpan? LateFullLeaveTimeDuration
        {
            get { return lateFullLeaveTimeDuration; }
            set
            {
                lateFullLeaveTimeDuration = value; OnPropertyChanged("LateFullLeaveTimeDuration");
                if (currentShiftDetail != null && lateFullLeaveTimeDuration != null)
                    currentShiftDetail.fullday_leave = (int)((TimeSpan)lateFullLeaveTimeDuration).TotalMinutes;
            }
        }

        DateTime lateFullLeaveTime;
        public DateTime LateFullLeaveTime
        {
            get { return lateFullLeaveTime; }
            set { lateFullLeaveTime = value; OnPropertyChanged("LateFullLeaveTime"); }
        }

        TimeSpan? lateGraceEffectDuration;
        public TimeSpan? LateGraceEffectDuration
        {
            get { return lateGraceEffectDuration; }
            set
            {
                lateGraceEffectDuration = value; OnPropertyChanged("LateGraceEffectDuration");
                if (currentShiftDetail != null && lateGraceEffectDuration != null)
                {
                    currentShiftDetail.late_grace_effect_time = (int)((TimeSpan)lateGraceEffectDuration).TotalMinutes;
                    this.SetShiftMorningTimes();
                }
            }
        }

        DateTime lateGraceEffectTime;
        public DateTime LateGraceEffectTime
        {
            get { return lateGraceEffectTime; }
            set { lateGraceEffectTime = value; OnPropertyChanged("LateGraceEffectTime"); }
        }

        TimeSpan? lateGraceTimeDuration;
        public TimeSpan? LateGraceTimeDuration
        {
            get { return lateGraceTimeDuration; }
            set
            {
                lateGraceTimeDuration = value; OnPropertyChanged("LateGraceTimeDuration");
                if (currentShiftDetail != null && lateGraceTimeDuration != null)
                {
                    currentShiftDetail.late_grace_time = (int)((TimeSpan)lateGraceTimeDuration).TotalMinutes;
                    this.SetShiftMorningTimes();
                }
            }
        }

        DateTime lateGraceRemainTime;
        public DateTime LateGraceRemainTime
        {
            get { return lateGraceRemainTime; }
            set { lateGraceRemainTime = value; OnPropertyChanged("LateGraceRemainTime"); }
        }

        #endregion

        #region Early Details Properties

        TimeSpan? earlyOutTimeDuration;
        public TimeSpan? EarlyOutTimeDuration
        {
            get { return earlyOutTimeDuration; }
            set
            {
                earlyOutTimeDuration = value; OnPropertyChanged("EarlyOutTimeDuration");
                if (currentShiftDetail != null && earlyOutTimeDuration != null)
                {
                    currentShiftDetail.early_out = (int)((TimeSpan)earlyOutTimeDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }

            }
        }

        DateTime earlyOutTime;
        public DateTime EarlyOutTime
        {
            get { return earlyOutTime; }
            set { earlyOutTime = value; OnPropertyChanged("EarlyOutTime"); }
        }

        TimeSpan? earlyGraceTimeDuration;
        public TimeSpan? EarlyGraceTimeDuration
        {
            get { return earlyGraceTimeDuration; }
            set
            {
                earlyGraceTimeDuration = value; OnPropertyChanged("EarlyGraceTimeDuration");
                if (currentShiftDetail != null && earlyGraceTimeDuration != null)
                {
                    currentShiftDetail.early_grace_time = (int)((TimeSpan)earlyGraceTimeDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }
            }
        }

        DateTime earlyGraceRemainTime;
        public DateTime EarlyGraceRemainTime
        {
            get { return earlyGraceRemainTime; }
            set { earlyGraceRemainTime = value; OnPropertyChanged("EarlyGraceRemainTime"); }
        }

        TimeSpan? earlyShortLeaveTimeDuration;
        public TimeSpan? EarlyShortLeaveTimeDuration
        {
            get { return earlyShortLeaveTimeDuration; }
            set
            {
                earlyShortLeaveTimeDuration = value; OnPropertyChanged("EarlyShortLeaveTimeDuration");
                if (currentShiftDetail != null && earlyShortLeaveTimeDuration != null)
                {
                    currentShiftDetail.evening_short_leave = (int)((TimeSpan)earlyShortLeaveTimeDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }
            }
        }

        DateTime earlyShortLeaveTime;
        public DateTime EarlyShortLeaveTime
        {
            get { return earlyShortLeaveTime; }
            set { earlyShortLeaveTime = value; OnPropertyChanged("EarlyShortLeaveTime"); }
        }

        TimeSpan? earlyHalfDayTimeDuration;
        public TimeSpan? EarlyHalfDayTimeDuration
        {
            get { return earlyHalfDayTimeDuration; }
            set
            {
                earlyHalfDayTimeDuration = value; OnPropertyChanged("EarlyHalfDayTimeDuration");
                if (currentShiftDetail != null && earlyHalfDayTimeDuration != null)
                {
                    currentShiftDetail.evening_halfday_leave = (int)((TimeSpan)earlyHalfDayTimeDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }
            }
        }

        DateTime earlyHalfDayTime;
        public DateTime EarlyHalfDayTime
        {
            get { return earlyHalfDayTime; }
            set { earlyHalfDayTime = value; OnPropertyChanged("EarlyHalfDayTime"); }
        }

        TimeSpan? earlyFullLeaveTimeDuration;
        public TimeSpan? EarlyFullLeaveTimeDuration
        {
            get { return earlyFullLeaveTimeDuration; }
            set
            {
                earlyFullLeaveTimeDuration = value; OnPropertyChanged("EarlyFullLeaveTimeDuration");
            }
        }

        TimeSpan? earlyGraceEffectDuration;
        public TimeSpan? EarlyGraceEffectDuration
        {
            get { return earlyGraceEffectDuration; }
            set
            {
                earlyGraceEffectDuration = value; OnPropertyChanged("EarlyGraceEffectDuration");
                if (currentShiftDetail != null && earlyGraceEffectDuration != null)
                {
                    currentShiftDetail.early_grace_effect_time = (int)((TimeSpan)earlyGraceEffectDuration).TotalMinutes;
                    this.SetShiftEveningTimes();
                }
            }
        }

        DateTime earlyGraceEffectTime;
        public DateTime EarlyGraceEffectTime
        {
            get { return earlyGraceEffectTime; }
            set { earlyGraceEffectTime = value; OnPropertyChanged("EarlyGraceEffectTime"); }
        }

        #endregion

        #region Additional Days Properties

        int? additionalDayCount;
        public int? AdditionalDayCount
        {
            get { return additionalDayCount; }
            set
            {
                additionalDayCount = value; OnPropertyChanged("AdditionalDayCount");
            }
        }

        string additionalDayDescription;
        public string AdditionalDayDescription
        {
            get { return additionalDayDescription; }
            set { additionalDayDescription = value; OnPropertyChanged("AdditionalDayDescription"); }
        }

        TimeSpan? additionalDayFromTime;
        public TimeSpan? AdditionalDayFromTime
        {
            get { return additionalDayFromTime; }
            set { additionalDayFromTime = value; OnPropertyChanged("AdditionalDayFromTime"); }
        }

        TimeSpan? additionalDayToTime;
        public TimeSpan? AdditionalDayToTime
        {
            get { return additionalDayToTime; }
            set { additionalDayToTime = value; OnPropertyChanged("AdditionalDayToTime"); }
        }

        int? additionalDayFromDayValue;
        public int? AdditionalDayFromDayValue
        {
            get { return additionalDayFromDayValue; }
            set { additionalDayFromDayValue = value; OnPropertyChanged("AdditionalDayFromDayValue"); }
        }

        int? additionalDayToDayValue;
        public int? AdditionalDayToDayValue
        {
            get { return additionalDayToDayValue; }
            set { additionalDayToDayValue = value; OnPropertyChanged("AdditionalDayToDayValue"); }
        }

        TimeSpan? additionalDayWorkStartTime;
        public TimeSpan? AdditionalDayWorkStartTime
        {
            get { return additionalDayWorkStartTime; }
            set { additionalDayWorkStartTime = value; OnPropertyChanged("AdditionalDayWorkStartTime"); }
        }

        TimeSpan? additionalDayWorkEndTime;
        public TimeSpan? AdditionalDayWorkEndTime
        {
            get { return additionalDayWorkEndTime; }
            set { additionalDayWorkEndTime = value; OnPropertyChanged("AdditionalDayWorkEndTime"); }
        }

        int? additionalDayWorkStartDayValue;
        public int? AdditionalDayWorkStartDayValue
        {
            get { return additionalDayWorkStartDayValue; }
            set { additionalDayWorkStartDayValue = value; OnPropertyChanged("AdditionalDayWorkStartDayValue"); }
        }

        int? additionalDayWorkEndDayValue;
        public int? AdditionalDayWorkEndDayValue
        {
            get { return additionalDayWorkEndDayValue; }
            set { additionalDayWorkEndDayValue = value; OnPropertyChanged("AdditionalDayWorkEndDayValue"); }
        }

        TimeSpan? additionalDayWorkTimeDuration;
        public TimeSpan? AdditionalDayWorkTimeDuration
        {
            get { return additionalDayWorkTimeDuration; }
            set { additionalDayWorkTimeDuration = value; OnPropertyChanged("AdditionalDayWorkTimeDuration"); }
        }

        /*public Dictionary<AdditionalDayCheckType, string> CheckTypes
        {
            get
            {
                return new Dictionary<AdditionalDayCheckType, string>() 
                {
                    {AdditionalDayCheckType.In_Time, "In Time"},
                    {AdditionalDayCheckType.Out_Time, "Out Time"},
                    {AdditionalDayCheckType.Work_Time, "Work Time"}
                };
            }
        }

        private AdditionalDayCheckType? currentCheckType;
        public AdditionalDayCheckType? CurrentCheckType
        {
            get { return currentCheckType; }
            set
            {
                currentCheckType = value; OnPropertyChanged("CurrentCheckType");
                if (currentCheckType == AdditionalDayCheckType.Work_Time)
                {
                    IsCheckTypeWork = true;
                    IsCheckTypeNotWork = false;
                }

                else
                {
                    IsCheckTypeWork = false;
                    IsCheckTypeNotWork = true;
                }

            }
        }*/

        IEnumerable<dtl_Shift_Additional_Day> additionalDays;
        public IEnumerable<dtl_Shift_Additional_Day> AdditionalDays
        {
            get { return additionalDays; }
            set { additionalDays = value; OnPropertyChanged("AdditionalDays"); }
        }

        dtl_Shift_Additional_Day currentAdditionalDay;
        public dtl_Shift_Additional_Day CurrentAdditionalDay
        {
            get { return currentAdditionalDay; }
            set
            {
                currentAdditionalDay = value; OnPropertyChanged("CurrentAdditionalDay");
                if (currentAdditionalDay != null)
                {
                    this.PopulateCurrentAdditionalDay();
                }
            }
        }

        bool isCheckTypeWork;
        public bool IsCheckTypeWork
        {
            get { return isCheckTypeWork; }
            set
            {
                isCheckTypeWork = value; OnPropertyChanged("IsCheckTypeWork");
                if (isCheckTypeWork)
                {
                    this.ValidateCheckType();
                }
            }
        }

        bool isCheckTypeNotWork;
        public bool IsCheckTypeNotWork
        {
            get { return isCheckTypeNotWork; }
            set
            {
                isCheckTypeNotWork = value; OnPropertyChanged("IsCheckTypeNotWork");
                if (isCheckTypeNotWork)
                {
                    this.ValidateCheckType();
                }
            }
        }

        z_ShiftDayRange additionalDayFromDay;
        public z_ShiftDayRange AdditionalDayFromDay
        {
            get { return additionalDayFromDay; }
            set { additionalDayFromDay = value; OnPropertyChanged("AdditionalDayFromDay"); }
        }

        z_ShiftDayRange additionalDayToDay;
        public z_ShiftDayRange AdditionalDayToDay
        {
            get { return additionalDayToDay; }
            set { additionalDayToDay = value; OnPropertyChanged("AdditionalDayToDay"); }
        }

        z_ShiftDayRange additionalDayWorkStartDay;
        public z_ShiftDayRange AdditionalDayWorkStartDay
        {
            get { return additionalDayWorkStartDay; }
            set { additionalDayWorkStartDay = value; OnPropertyChanged("AdditionalDayWorkStartDay"); }
        }

        z_ShiftDayRange additionalDayWorkEndDay;
        public z_ShiftDayRange AdditionalDayWorkEndDay
        {
            get { return additionalDayWorkEndDay; }
            set { additionalDayWorkEndDay = value; OnPropertyChanged("AdditionalDayWorkEndDay"); }
        }

        #endregion

        #region Shift break properties

        TimeSpan? breakOnTime;
        public TimeSpan? BreakOnTime
        {
            get { return breakOnTime; }
            set { breakOnTime = value; OnPropertyChanged("BreakOnTime"); }
        }

        TimeSpan? breakOffTime;
        public TimeSpan? BreakOffTime
        {
            get { return breakOffTime; }
            set { breakOffTime = value; OnPropertyChanged("BreakOffTime"); }
        }

        TimeSpan? breakInTime;
        public TimeSpan? BreakInTime
        {
            get { return breakInTime; }
            set { breakInTime = value; OnPropertyChanged("BreakInTime"); }
        }

        TimeSpan? breakOutTime;
        public TimeSpan? BreakOutTime
        {
            get { return breakOutTime; }
            set { breakOutTime = value; OnPropertyChanged("BreakOutTime"); }
        }

        int? breakOnDayValue;
        public int? BreakOnDayValue
        {
            get { return breakOnDayValue; }
            set { breakOnDayValue = value; OnPropertyChanged("BreakOnDayValue"); }
        }

        int? breakOffDayValue;
        public int? BreakOffDayValue
        {
            get { return breakOffDayValue; }
            set { breakOffDayValue = value; OnPropertyChanged("BreakOffDayValue"); }
        }

        int? breakInDayValue;
        public int? BreakInDayValue
        {
            get { return breakInDayValue; }
            set { breakInDayValue = value; OnPropertyChanged("BreakInDayValue"); }
        }

        int? breakOutDayValue;
        public int? BreakOutDayValue
        {
            get { return breakOutDayValue; }
            set { breakOutDayValue = value; OnPropertyChanged("BreakOutDayValue"); }
        }

        string breakDescription;
        public string BreakDescription
        {
            get { return breakDescription; }
            set { breakDescription = value; OnPropertyChanged("BreakDescription"); }
        }

        IEnumerable<dtl_Shift_Break_Details> shiftBreaks;
        public IEnumerable<dtl_Shift_Break_Details> ShiftBreaks
        {
            get { return shiftBreaks; }
            set { shiftBreaks = value; OnPropertyChanged("ShiftBreaks"); }
        }

        dtl_Shift_Break_Details currentShiftBreak;
        public dtl_Shift_Break_Details CurrentShiftBreak
        {
            get { return currentShiftBreak; }
            set
            {
                currentShiftBreak = value; OnPropertyChanged("CurrentShiftBreak");
                if (currentShiftBreak != null)
                    this.PopulateCurrentShiftBreak();
            }
        }

        #endregion

        #region Error properties

        bool hasError;
        #endregion

        #region MCN updated 2017-07-24
        #region Shift Covering properties

        string coverDescription;
        public string CoverDescription
        {
            get { return coverDescription; }
            set { coverDescription = value; OnPropertyChanged("CoverDescription"); }
        }

        TimeSpan? coverOnTime;
        public TimeSpan? CoverOnTime
        {
            get { return coverOnTime; }
            set { coverOnTime = value; OnPropertyChanged("CoverOnTime"); }
        }

        TimeSpan? coverOffTime;
        public TimeSpan? CoverOffTime
        {
            get { return coverOffTime; }
            set { coverOffTime = value; OnPropertyChanged("CoverOffTime"); }
        }

        int? coverOnDayValue;
        public int? CoverOnDayValue
        {
            get { return coverOnDayValue; }
            set { coverOnDayValue = value; OnPropertyChanged("CoverOnDayValue"); }
        }

        int? coverOffDayValue;
        public int? CoverOffDayValue
        {
            get { return coverOffDayValue; }
            set { coverOffDayValue = value; OnPropertyChanged("CoverOffDayValue"); }
        }

        int? coverMaxLate;
        public int? CoverMaxLate
        {
            get { return coverMaxLate; }
            set { coverMaxLate = value; OnPropertyChanged("CoverMaxLate"); }
        }

        int? coverEffTime;
        public int? CoverEffTime
        {
            get { return coverEffTime; }
            set { coverEffTime = value; OnPropertyChanged("CoverEffTime"); }
        }

        IEnumerable<dtl_Shift_Covering_Details> shiftCover;
        public IEnumerable<dtl_Shift_Covering_Details> ShiftCover
        {
            get { return shiftCover; }
            set { shiftCover = value; OnPropertyChanged("ShiftCover"); }
        }

        dtl_Shift_Covering_Details currentShiftCover;
        public dtl_Shift_Covering_Details CurrentShiftCover
        {
            get { return currentShiftCover; }
            set
            {
                currentShiftCover = value; OnPropertyChanged("CurrentShiftCover");
                if (currentShiftCover != null)
                    this.PopulateCurrentShiftCover();
            }
        }

        #endregion

        #region Open Shift

        private TimeSpan? openShiftMinWordDuration;

        public TimeSpan? OpenShiftMinWordDuration
        {
            get { return openShiftMinWordDuration; }
            set
            {
                openShiftMinWordDuration = value; OnPropertyChanged("OpenShiftMinWordDuration");
                if (openShiftMinWordDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_min_word_time = (int)((TimeSpan)openShiftMinWordDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftNoOtDuration;

        public TimeSpan? OpenShiftNoOtDuration
        {
            get { return openShiftNoOtDuration; }
            set
            {
                openShiftNoOtDuration = value; OnPropertyChanged("OpenShiftNoOtDuration");
                if (openShiftNoOtDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_non_ot = (int)((TimeSpan)openShiftNoOtDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftSingleOtDuration;

        public TimeSpan? OpenShiftSingleOtDuration
        {
            get { return openShiftSingleOtDuration; }
            set
            {
                openShiftSingleOtDuration = value; OnPropertyChanged("OpenShiftSingleOtDuration");
                if (openShiftSingleOtDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_single_ot = (int)((TimeSpan)openShiftSingleOtDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftDoubleOtDuration;

        public TimeSpan? OpenShiftDoubleOtDuration
        {
            get { return openShiftDoubleOtDuration; }
            set
            {
                openShiftDoubleOtDuration = value; OnPropertyChanged("OpenShiftDoubleOtDuration");
                if (openShiftDoubleOtDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_double_ot = (int)((TimeSpan)openShiftDoubleOtDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftTripleOtDuration;

        public TimeSpan? OpenShiftTripleOtDuration
        {
            get { return openShiftTripleOtDuration; }
            set
            {
                openShiftTripleOtDuration = value; OnPropertyChanged("OpenShiftTripleOtDuration");
                if (openShiftTripleOtDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_triple_ot = (int)((TimeSpan)openShiftTripleOtDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftLateDuration;

        public TimeSpan? OpenShiftLateDuration
        {
            get { return openShiftLateDuration; }
            set
            {
                openShiftLateDuration = value; OnPropertyChanged("OpenShiftLateDuration");
                if (openShiftLateDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_late = (int)((TimeSpan)openShiftLateDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftShotLeaveTimeDuration;

        public TimeSpan? OpenShiftShotLeaveTimeDuration
        {
            get { return openShiftShotLeaveTimeDuration; }
            set
            {
                openShiftShotLeaveTimeDuration = value; OnPropertyChanged("OpenShiftShotLeaveTimeDuration");
                if (openShiftShotLeaveTimeDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_short_leave = (int)((TimeSpan)openShiftShotLeaveTimeDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftHalfDayTimeDuration;

        public TimeSpan? OpenShiftHalfDayTimeDuration
        {
            get { return openShiftHalfDayTimeDuration; }
            set
            {
                openShiftHalfDayTimeDuration = value; OnPropertyChanged("OpenShiftHalfDayTimeDuration");
                if (openShiftHalfDayTimeDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_halfday_leave = (int)((TimeSpan)openShiftHalfDayTimeDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftFullDayTimeDuration;

        public TimeSpan? OpenShiftFullDayTimeDuration
        {
            get { return openShiftFullDayTimeDuration; }
            set
            {
                openShiftFullDayTimeDuration = value; OnPropertyChanged("OpenShiftFullDayTimeDuration");
                if (openShiftFullDayTimeDuration != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_fullday_leave = (int)((TimeSpan)openShiftFullDayTimeDuration).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftMinWordDurationforWeek;

        public TimeSpan? OpenShiftMinWordDurationforWeek
        {
            get { return openShiftMinWordDurationforWeek; }
            set
            {
                openShiftMinWordDurationforWeek = value; OnPropertyChanged("OpenShiftMinWordDurationforWeek");
                if (openShiftMinWordDurationforWeek != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_min_word_time_week = (int)((TimeSpan)openShiftMinWordDurationforWeek).TotalMinutes;
                }
            }
        }

        private TimeSpan? openShiftMinWordDurationforMonth;

        public TimeSpan? OpenShiftMinWordDurationforMonth
        {
            get { return openShiftMinWordDurationforMonth; }
            set
            {
                openShiftMinWordDurationforMonth = value; OnPropertyChanged("OpenShiftMinWordDurationforMonth");
                if (openShiftMinWordDurationforMonth != null)
                {
                    if (currentShiftDetail != null)
                        currentShiftDetail.open_min_word_time_month = (int)((TimeSpan)openShiftMinWordDurationforMonth).TotalMinutes;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Refresh Methods

        void RefreshShiftCategories()
        {
            attendServiceClient.GetShiftCategoryDetailsCompleted += (s, e) =>
            {
                try
                {
                    ShiftCategories = e.Result;
                }
                catch (Exception)
                {

                    clsMessages.setMessage("Shift categories refresh is failed");
                }
            };
            attendServiceClient.GetShiftCategoryDetailsAsync();

        }

        void RefreshDayRanges()
        {
            attendServiceClient.GetDayRangeDetailsCompleted += (s, e) =>
            {
                try
                {
                    ShiftDayRanges = e.Result;
                }
                catch (Exception)
                {

                    clsMessages.setMessage("Day ranges refresh is failed");
                }
            };
            attendServiceClient.GetDayRangeDetailsAsync();

        }

        void RefreshShifts()
        {
            try
            {
                var shiftList = (from es in allShiftDetails.Where(c => c.shift_category_id == currentShiftCategory.shift_category_id).GroupBy(o => o.shift_detail_id).Select(x => x.First())
                                 select new dtl_Shift_Master { shift_detail_id = es.shift_detail_id, shift_detail_name = es.shift_detail_name, dtl_ShiftOrder = new dtl_ShiftOrder { order_number = es.order_number } });
                Shifts = null;
                Shifts = shiftList;
                this.ClearCurrentData();
            }
            catch (Exception)
            {
            }
        }

        void RefreshAllShiftDetails()
        {
            attendServiceClient.GetShiftAllDetailsCompleted += (s, e) =>
            {
                try
                {
                    AllShiftDetails = e.Result;
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Shifts details refresh is failed");
                }
            };
            attendServiceClient.GetShiftAllDetailsAsync();
        }

        void RefreshAdditionalDaysDetails()
        {
            attendServiceClient.GetAdditionalDaysByShiftCompleted += (s, e) =>
            {
                try
                {
                    AdditionalDays = e.Result;
                    if (additionalDays != null)
                    {
                        addedDaysList.Clear();
                        removedDaysList.Clear();
                        addedDaysList = additionalDays.ToList();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Additional days refresh is failed");
                }
            };

            attendServiceClient.GetAdditionalDaysByShiftAsync(currentShift.shift_detail_id);
        }

        void RefreshShiftBreakDetails()
        {
            attendServiceClient.GetShiftBreaksByShiftCompleted += (s, e) =>
            {
                try
                {
                    ShiftBreaks = e.Result;
                    if (shiftBreaks != null)
                    {
                        addedBreakList.Clear();
                        removedBreakList.Clear();
                        addedBreakList = shiftBreaks.ToList();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Shift break details refresh is failed");
                }

            };

            attendServiceClient.GetShiftBreaksByShiftAsync(currentShiftDetail.shift_detail_id);
        }

        //updated 2017-07-24
        void RefreshShiftCoverDetails()
        {
            attendServiceClient.GetShiftConveringCompleted += (s, e) =>
            {
                try
                {
                    ShiftCover = e.Result;
                    if (shiftCover != null)
                    {
                        addCoverList.Clear();
                        removedCoverList.Clear();
                        addCoverList = shiftCover.ToList();
                    }
                }
                catch (Exception)
                {
                    clsMessages.setMessage("Shift Convering details refresh is failed");
                }
            };
            attendServiceClient.GetShiftConveringAsync(CurrentShift.shift_detail_id);
        }

        #endregion

        #region Button Methods

        #region New Button

        void New()
        {
            CurrentShiftDetail = null;
            CurrentShiftDetail = new ShiftDetailAllView();
            this.ClearCurrentData();
            CurrentShiftCategory = null;
            CurrentShift = null;
            CurrentAdditionalDay = null;
            CurrentAdditionalDay = new dtl_Shift_Additional_Day();
            IsDailyShift = true;
            IsRosterShift = false;
            IsSplitShift = false;
            IsHalfDay = false;
            IsBoolConvert = true;
            IsOpenShift = false;
            IsOpenShiftOT = false;
            IsOpenShiftLate = false;
            CurrentShiftCover = null;
            IsSpecShift = false;
            IsMorIncentive = false;

            OpenShiftMinWordDuration = GetTimeFromMinutes(0);
            OpenShiftMinWordDurationforWeek = GetTimeFromMinutes(0);
            OpenShiftMinWordDurationforMonth = GetTimeFromMinutes(0);

            OpenShiftNoOtDuration = GetTimeFromMinutes(0);
            OpenShiftSingleOtDuration = GetTimeFromMinutes(0);
            OpenShiftDoubleOtDuration = GetTimeFromMinutes(0);
            OpenShiftTripleOtDuration = GetTimeFromMinutes(0);

            OpenShiftLateDuration = GetTimeFromMinutes(0);
            OpenShiftShotLeaveTimeDuration = GetTimeFromMinutes(0);
            OpenShiftHalfDayTimeDuration = GetTimeFromMinutes(0);
            OpenShiftFullDayTimeDuration = GetTimeFromMinutes(0);

            PreSingleOtRound = 0;
            PreDoubleOtRound = 0;
            PreTripleOtRound = 0;
            PostSingleOtRound = 0;
            PostDoubleOtRound = 0;
            PostTripleOtRound = 0;
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }

        #endregion

        #region Additional Day Add Button

        void AdditionalDayAdd()
        {
            dtl_Shift_Additional_Day newDay = this.SetAdditionalDayData();
            if (addedDaysList.Count(c => c.shift_detail_id == newDay.shift_detail_id && c.addition_day_id == newDay.addition_day_id && c.addition_day_id != 0) > 0)      // update of additional day
            {
                dtl_Shift_Additional_Day current = addedDaysList.FirstOrDefault(c => c.shift_detail_id == newDay.shift_detail_id && c.addition_day_id == newDay.addition_day_id);
                current.addition_day_count = newDay.addition_day_count;
                current.addition_day_description = newDay.addition_day_description;
                current.from_time = newDay.from_time;
                current.to_time = newDay.to_time;
                current.from_time_day_value = newDay.from_time_day_value;
                current.to_time_day_value = newDay.to_time_day_value;
                current.worktime_start = newDay.worktime_start;
                current.worktime_end = newDay.worktime_end;
                current.worktime_start_day_value = newDay.worktime_start_day_value;
                current.worktime_end_day_value = newDay.worktime_end_day_value;
            }
            else
            {
                dtl_Shift_Additional_Day addingDay = new dtl_Shift_Additional_Day();
                addingDay.shift_detail_id = newDay.shift_detail_id;
                addingDay.addition_day_count = newDay.addition_day_count;
                addingDay.addition_day_description = newDay.addition_day_description;
                addingDay.check_type = newDay.check_type;
                addingDay.from_time = newDay.from_time;
                addingDay.to_time = newDay.to_time;
                addingDay.from_time_day_value = newDay.from_time_day_value;
                addingDay.to_time_day_value = newDay.to_time_day_value;
                addingDay.worktime_start = newDay.worktime_start;
                addingDay.worktime_end = newDay.worktime_end;
                addingDay.worktime_start_day_value = newDay.worktime_start_day_value;
                addingDay.worktime_end_day_value = newDay.worktime_end_day_value;
                addedDaysList.Add(addingDay);
            }

            AdditionalDays = null;
            AdditionalDays = addedDaysList;
            this.ClearAdditionalDayData();
        }

        public ICommand AdditionalDayAddButton
        {
            get { return new RelayCommand(AdditionalDayAdd); }
        }

        #endregion

        #region Additional Day Remove Button

        void AdditionalDayRemove()
        {
            if (currentAdditionalDay != null)
            {
                if (addedDaysList.Count(c => c.shift_detail_id == currentAdditionalDay.shift_detail_id && c.addition_day_id == currentAdditionalDay.addition_day_id && c.addition_day_id != 0) > 0) // saved additional day. This could be deleted from database
                {
                    dtl_Shift_Additional_Day current = addedDaysList.FirstOrDefault(c => c.shift_detail_id == currentAdditionalDay.shift_detail_id && c.addition_day_id == currentAdditionalDay.addition_day_id);
                    current.delete_user_id = clsSecurity.loggedUser.user_id;
                    current.delete_datetime = DateTime.Now;
                    current.is_delete = true;
                    removedDaysList.Add(current);
                    addedDaysList.Remove(current);
                }
                else
                {
                    addedDaysList.Remove(currentAdditionalDay);
                }
            }

            AdditionalDays = null;
            AdditionalDays = addedDaysList;
            this.AdditionalDayNew();
        }

        public ICommand AdditionalDayRemoveButton
        {
            get { return new RelayCommand(AdditionalDayRemove); }
        }

        #endregion

        #region Additional Day New Button

        void AdditionalDayNew()
        {
            CurrentAdditionalDay = null;
            CurrentAdditionalDay = new dtl_Shift_Additional_Day();
        }

        public ICommand AdditionalDayNewButton
        {
            get { return new RelayCommand(AdditionalDayNew); }
        }

        #endregion

        #region Shift Break Add Button

        void BreakAdd()
        {
            dtl_Shift_Break_Details addingBreak = this.SetShiftBreakData();
            if (addedBreakList.Count(c => c.shift_detail_id == addingBreak.shift_detail_id && c.break_id == addingBreak.break_id && c.break_id != 0) > 0)          // updating existing shift break
            {
                dtl_Shift_Break_Details current = addedBreakList.FirstOrDefault(c => c.shift_detail_id == addingBreak.shift_detail_id && c.break_id == addingBreak.break_id && c.break_id != 0);
                current.break_description = addingBreak.break_description;
                current.break_on_time = addingBreak.break_on_time;
                current.break_off_time = addingBreak.break_off_time;
                current.break_on_day_value = addingBreak.break_on_day_value;
                current.break_off_day_value = addingBreak.break_off_day_value;
                current.break_in_time = addingBreak.break_in_time;
                current.break_out_time = addingBreak.break_out_time;
                current.break_in_day_value = addingBreak.break_in_day_value;
                current.break_out_day_value = addingBreak.break_out_day_value;
            }
            else
            {
                addedBreakList.Add(addingBreak);
            }

            ShiftBreaks = null;
            ShiftBreaks = addedBreakList;
            this.ClearCurrentShiftBreak();
        }

        public ICommand BreakAddButton
        {
            get { return new RelayCommand(BreakAdd); }
        }

        #endregion

        #region Shift Break Remove Button

        void BreakRemove()
        {
            if (currentShiftBreak != null)
            {
                if (addedBreakList.Count(c => c.shift_detail_id == currentShiftBreak.shift_detail_id && c.break_id == currentShiftBreak.break_id && c.break_id != 0) > 0)
                {
                    dtl_Shift_Break_Details current = addedBreakList.FirstOrDefault(c => c.shift_detail_id == currentShiftBreak.shift_detail_id && c.break_id == currentShiftBreak.break_id);
                    current.is_delete = true;
                    current.delete_datetime = DateTime.Now;
                    current.delete_user_id = clsSecurity.loggedUser.user_id;
                    addedBreakList.Remove(current);
                    removedBreakList.Add(current);
                }
                else
                {
                    addedBreakList.Remove(currentShiftBreak);
                }

                ShiftBreaks = null;
                ShiftBreaks = addedBreakList;
                this.BreakNew();
            }
        }

        public ICommand BreakRemoveButton
        {
            get { return new RelayCommand(BreakRemove); }
        }

        #endregion

        #region Shift Break New Button

        void BreakNew()
        {
            CurrentShiftBreak = null;
            CurrentShiftBreak = new dtl_Shift_Break_Details();
        }

        public ICommand BreakNewButton
        {
            get { return new RelayCommand(BreakNew); }
        }

        #endregion

        #region Save Button

        void Save()
        {
            this.RefreshShiftErrors(validatedProperty);
            if (!hasError)
            {
                if (IsValidSplitShift())
                {
                    if (currentShiftCategory != null && currentShiftDetail != null)
                    {
                        if (currentShiftDetail.shift_detail_id == 0)        // adding new shift
                        {
                            dtl_Shift_Master addingShift = new dtl_Shift_Master();
                            addingShift = this.SetCurrentShiftDetails();
                            addingShift.save_user_id = clsSecurity.loggedUser.user_id;
                            addingShift.save_datetime = DateTime.Now;
                            if (clsSecurity.GetSavePermission(302))
                            {
                                if (attendServiceClient.SaveShiftMasterDetails(addingShift))
                                {
                                    clsMessages.setMessage("Shift is saved successfully");
                                    this.RefreshAllShiftDetails();
                                    this.New();
                                }

                                else
                                {
                                    clsMessages.setMessage("Shift save is failed");
                                }
                            }
                            else
                                clsMessages.setMessage("You don't have permission to save this form");
                        }
                        else              // updating existing shift
                        {
                            dtl_Shift_Master updatingShift = new dtl_Shift_Master();
                            updatingShift = this.SetCurrentShiftDetails();
                            updatingShift.shift_detail_id = currentShiftDetail.shift_detail_id;
                            updatingShift.dtl_Shift_Detail_Basic.shift_detail_id = currentShiftDetail.shift_detail_id;
                            updatingShift.dtl_Shift_OT_Configuration_Details.shift_detail_id = currentShiftDetail.shift_detail_id;
                            updatingShift.dtl_Shift_Late_Configuration_Details.shift_detail_id = currentShiftDetail.shift_detail_id;
                            updatingShift.modified_user_id = clsSecurity.loggedUser.user_id;
                            updatingShift.modified_datetime = DateTime.Now;
                            //2017-07-24
                            if (updatingShift.dtl_Shift_Covering_Details != null)
                            {
                                updatingShift.dtl_Shift_Covering_Details.shift_detail_id = currentShiftDetail.shift_detail_id;
                            }
                            if (clsSecurity.GetUpdatePermission(302))
                            {
                                if (attendServiceClient.UpdateShiftMasterDetails(updatingShift))
                                {
                                    if (removedDaysList.Count > 0)
                                    {
                                        if (attendServiceClient.DeleteShiftAdditionalDays(removedDaysList.ToArray()))
                                            clsMessages.setMessage("Additional day delete is successful");
                                        else
                                            clsMessages.setMessage("Additional days delete is failed");

                                    }

                                    //2017-07-24
                                    if (removedCoverList.Count > 0)
                                    {
                                        if (attendServiceClient.DeleteShiftCoveringDetails(removedCoverList.ToArray()))
                                            clsMessages.setMessage("Shift Cover delete is successful");
                                        else
                                            clsMessages.setMessage("Shift Cover delete is failed");
                                    }

                                    if (removedBreakList.Count > 0)
                                    {
                                        if (attendServiceClient.DeleteShiftBreakDetails(removedBreakList.ToArray()))
                                            clsMessages.setMessage("Shift breaks delete is successful");
                                        else
                                            clsMessages.setMessage("Shift breaks delete is failed");
                                    }
                                    clsMessages.setMessage("Shift is updated successfully");
                                    this.RefreshAllShiftDetails();
                                }
                                else
                                {
                                    clsMessages.setMessage("Shift update is failed");
                                }
                            }
                            else
                                clsMessages.setMessage("You don't have permission to update this form");
                        }
                    }
                }
                else
                {
                    clsMessages.setMessage("Split shift can be contained only one shift-break");
                }
            }
            else
            {
                clsMessages.setMessage("Shift detail values may be incorrect: " + Error);
            }
        }

        public ICommand SaveButton
        {
            get { return new RelayCommand(Save); }
        }

        #endregion

        #region Delete Button

        public ICommand DeleteButton
        {
            get { return new RelayCommand(Delete, DeleteCE); }
        }

        private bool DeleteCE()
        {
            if (currentShiftDetail != null && currentShiftDetail.shift_detail_id != 0)
                return true;
            else
                return false;
        }

        private void Delete()
        {
            dtl_Shift_Master deleteObj = new dtl_Shift_Master();
            deleteObj.shift_detail_id = CurrentShiftDetail.shift_detail_id;
            deleteObj.is_delete = true;
            deleteObj.delete_user_id = clsSecurity.loggedUser.user_id;
            deleteObj.delete_datetime = DateTime.Now;

            if (clsSecurity.GetDeletePermission(302))
            {
                if (attendServiceClient.DeleteShiftMasterDetails(deleteObj))
                {
                    clsMessages.setMessage("Shift deleted successfully");
                    this.RefreshAllShiftDetails();
                    this.New();
                }

                else
                {
                    clsMessages.setMessage("Shift delete failed");
                }
            }
            else
                clsMessages.setMessage("You don't permission to delete this form");
        }

        //covering 2017-07-24
        #region Shift Cover Add Button

        void CoverAdd()
        {
            dtl_Shift_Covering_Details addingCover = this.SetShiftCoverData();
            if (addCoverList.Count(c => c.shift_detail_id == addingCover.shift_detail_id) > 0)          // updating existing shift cover
            {
                dtl_Shift_Covering_Details current = addCoverList.FirstOrDefault(c => c.shift_detail_id == addingCover.shift_detail_id);
                current.covering_Description = addingCover.covering_Description;
                current.covering_on_time = addingCover.covering_on_time;
                current.covering_off_time = addingCover.covering_off_time;
                current.covering_on_day_value = addingCover.covering_on_day_value;
                current.covering_off_day_value = addingCover.covering_off_day_value;
                current.covering_effect_time = addingCover.covering_effect_time;
                current.maximum_late_time = addingCover.maximum_late_time;

            }
            else
            {
                addCoverList.Add(addingCover);
            }

            ShiftCover = null;
            ShiftCover = addCoverList;
            this.ClearCurrentShiftCover();
        }

        public ICommand CoverAddButton
        {
            get { return new RelayCommand(CoverAdd); }
        }

        #endregion

        #region Shift Cover Remove Button

        void CoverRemove()
        {
            if (currentShiftCover != null)
            {
                if (addCoverList.Count(c => c.shift_detail_id == currentShiftCover.shift_detail_id) > 0)
                {
                    dtl_Shift_Covering_Details current = addCoverList.FirstOrDefault(c => c.shift_detail_id == currentShiftCover.shift_detail_id);
                    current.is_delete = true;
                    current.delete_datetime = DateTime.Now;
                    current.delete_user_id = clsSecurity.loggedUser.user_id;
                    addCoverList.Remove(current);
                    removedCoverList.Add(current);
                }
                else
                {
                    addCoverList.Remove(currentShiftCover);
                }

                ShiftCover = null;
                ShiftCover = addCoverList;
                this.CoverNew();
            }
        }

        public ICommand CoverRemoveButton
        {
            get { return new RelayCommand(CoverRemove); }
        }

        #endregion

        #region Shift Cover New Button

        void CoverNew()
        {
            CurrentShiftCover = null;
            CurrentShiftCover = new dtl_Shift_Covering_Details();
        }

        public ICommand CoverNewButton
        {
            get { return new RelayCommand(CoverNew); }
        }

        #endregion

        #endregion



        #endregion

        #region Data Setting Methods

        void PopulateCurrentShiftDetails()
        {
            ShiftInDayValue = (int)currentShiftDetail.shift_in_day_value;
            ShiftOutDayValue = (int)currentShiftDetail.shift_out_day_value;
            ShiftDetailName = currentShiftDetail.shift_detail_name;
            IsDailyShift = currentShiftDetail.is_daily_shift;
            IsRosterShift = currentShiftDetail.is_roster;
            IsSplitShift = (bool)currentShiftDetail.is_split_shift;
            IsEntitleLeiuLeave = (bool)currentShiftDetail.is_entitle_leiu_leave;
            IsOfficeExecutive = (bool)currentShiftDetail.is_executive;
            IsStoresExecutive = (bool)currentShiftDetail.is_stores_executive;
            IsOfficeNonExecutive = (bool)currentShiftDetail.is_nonexecutive;
            IsStoresNonExecutive = (bool)currentShiftDetail.is_stores_nonexecutive;
            IsSecurity = (bool)currentShiftDetail.is_security;
            IsOTShift = (bool)currentShiftDetail.is_ot_shift;
            IsSingleOT = (bool)currentShiftDetail.is_single;
            IsMultipleOT = (bool)currentShiftDetail.is_multiple;
            IsHalfDay = (bool)currentShiftDetail.is_halfday;
            IsSpecShift = (bool)currentShiftDetail.is_late_deduc;
            IsMorIncentive = (bool)currentShiftDetail.is_mor_inc;

            // Open shift details
            IsOpenShift = (bool)currentShiftDetail.is_open_shift;
            IsBoolConvert = IsOpenShift == true ? false : true;
            IsOpenShiftOT = (bool)currentShiftDetail.is_open_shift_ot;
            IsOpenShiftLate = (bool)currentShiftDetail.is_open_shift_late;

            // setting shift basic details
            ShiftInTime = (TimeSpan)currentShiftDetail.shift_in_time;
            ShiftOutTime = (TimeSpan)currentShiftDetail.shift_out_time;
            ShiftOnTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.shift_on_time);
            ShiftOffTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.shift_off_time);

            // Open shift details
            OpenShiftMinWordDuration = this.GetTimeFromMinutes(currentShiftDetail.open_min_word_time);
            OpenShiftMinWordDurationforWeek = this.GetTimeFromMinutes(currentShiftDetail.open_min_word_time_week);
            OpenShiftMinWordDurationforMonth = this.GetTimeFromMinutes(currentShiftDetail.open_min_word_time_month);

            // setting pre ot details
            PreNoOtDuration = this.GetTimeFromMinutes(currentShiftDetail.pre_non_ot);
            PreSingleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.pre_single_ot);
            PreDoubleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.pre_double_ot);
            PreTripleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.pre_triple_ot);

            PreSingleOtRound = (int)currentShiftDetail.pre_single_ot_roundup; // == null ? 0 : currentShiftDetail.pre_single_ot_roundup.Value;
            PreDoubleOtRound = (int)currentShiftDetail.pre_double_ot_roundup; // == null ? 0 : currentShiftDetail.pre_single_ot_roundup.Value;
            PreTripleOtRound = (int)currentShiftDetail.pre_triple_ot_roundup; // == null ? 0 : currentShiftDetail.pre_triple_ot_roundup.Value;

            PreNonOtCompensate = (int)currentShiftDetail.pre_non_ot_compensate;

            // Open shift details
            OpenShiftNoOtDuration = this.GetTimeFromMinutes(currentShiftDetail.open_non_ot);
            OpenShiftSingleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.open_single_ot);
            OpenShiftDoubleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.open_double_ot);
            OpenShiftTripleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.open_triple_ot);

            // setting post ot details
            PostNoOtDuration = this.GetTimeFromMinutes(currentShiftDetail.post_non_ot);
            PostSingleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.post_single_ot);
            PostDoubleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.post_double_ot);
            PostTripleOtDuration = this.GetTimeFromMinutes(currentShiftDetail.post_triple_ot);

            PostSingleOtRound = (int)currentShiftDetail.post_single_ot_roundup; // == null ? 0 : currentShiftDetail.post_single_ot_roundup.Value;
            PostDoubleOtRound = (int)currentShiftDetail.post_double_ot_roundup; // == null ? 0 : currentShiftDetail.post_double_ot_roundup.Value;
            PostTripleOtRound = (int)currentShiftDetail.post_triple_ot_roundup; // == null ? 0 : currentShiftDetail.post_triple_ot_roundup.Value;

            PostNonOtCompensate = (int)currentShiftDetail.post_non_ot_compensate;

            // setting late attend details
            LateInTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.late_in);
            LateShotLeaveTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.morning_short_leave);
            LateHalfDayTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.morning_halfday_leave);
            LateFullLeaveTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.fullday_leave);
            LateGraceTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.late_grace_time);
            LateGraceEffectDuration = this.GetTimeFromMinutes(currentShiftDetail.late_grace_effect_time);

            // Open shift details
            OpenShiftLateDuration = this.GetTimeFromMinutes(currentShiftDetail.open_late);
            OpenShiftShotLeaveTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.open_short_leave);
            OpenShiftHalfDayTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.open_halfday_leave);
            OpenShiftFullDayTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.open_fullday_leave);

            // setting early out details
            EarlyOutTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.early_out);
            EarlyShortLeaveTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.evening_short_leave);
            EarlyHalfDayTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.evening_halfday_leave);
            EarlyGraceTimeDuration = this.GetTimeFromMinutes(currentShiftDetail.early_grace_time);
            EarlyGraceEffectDuration = this.GetTimeFromMinutes(currentShiftDetail.early_grace_effect_time);

            this.RefreshAdditionalDaysDetails();
            this.RefreshShiftBreakDetails();
            this.RefreshShiftCoverDetails();
        }

        dtl_Shift_Master SetCurrentShiftDetails()
        {
            dtl_Shift_Master shiftMaster = new dtl_Shift_Master();
            shiftMaster.shift_detail_name = shiftDetailName;
            shiftMaster.shift_category_id = currentShiftCategory.shift_category_id;
            shiftMaster.is_daily_shift = isDailyShift;
            shiftMaster.is_roster = isRosterShift;
            shiftMaster.is_split_shift = isSplitShift;
            shiftMaster.is_entitle_leiu_leave = isEntitleLeiuLeave;
            shiftMaster.is_executive = _IsOfficeExecutive;
            shiftMaster.is_stores_executive = _IsStoresExecutive;
            shiftMaster.is_nonexecutive = _IsOfficeNonExecutive;
            shiftMaster.is_stores_nonexecutive = _IsStoresNonExecutive;
            shiftMaster.is_security = isSecurity;
            shiftMaster.is_ot_shift = isOTShift;
            shiftMaster.is_single = isSingleOT;
            shiftMaster.is_multiple = isMultipleOT;
            shiftMaster.is_halfday = isHalfDay;
            shiftMaster.is_late_deduc = _IsSpecShift;
            shiftMaster.is_mor_inc = _IsMorIncentive;
            // Open shift details
            shiftMaster.is_open_shift = isOpenShift;
            shiftMaster.is_open_shift_late = isOpenShiftLate;
            shiftMaster.is_open_shift_ot = isOpenShiftOT;

            dtl_Shift_Detail_Basic basicShiftData = new dtl_Shift_Detail_Basic();
            basicShiftData.shift_in_time = currentShiftDetail.shift_in_time;
            basicShiftData.shift_out_time = currentShiftDetail.shift_out_time;
            basicShiftData.shift_on_time = currentShiftDetail.shift_on_time;
            basicShiftData.shift_off_time = currentShiftDetail.shift_off_time;
            basicShiftData.shift_in_day_value = currentShiftDetail.shift_in_day_value;
            basicShiftData.shift_out_day_value = currentShiftDetail.shift_out_day_value;
            // Open shift details
            basicShiftData.open_min_word_time = currentShiftDetail.open_min_word_time;
            basicShiftData.open_min_word_time_week = currentShiftDetail.open_min_word_time_week;
            basicShiftData.open_min_word_time_month = currentShiftDetail.open_min_word_time_month;

            shiftMaster.dtl_Shift_Detail_Basic = basicShiftData;

            dtl_Shift_OT_Configuration_Details shiftOtData = new dtl_Shift_OT_Configuration_Details();
            shiftOtData.pre_non_ot = (int)currentShiftDetail.pre_non_ot;
            shiftOtData.pre_single_ot = (int)currentShiftDetail.pre_single_ot;
            shiftOtData.pre_double_ot = (int)currentShiftDetail.pre_double_ot;
            shiftOtData.pre_triple_ot = (int)currentShiftDetail.pre_triple_ot;
            shiftOtData.post_non_ot = (int)currentShiftDetail.post_non_ot;
            shiftOtData.post_single_ot = (int)currentShiftDetail.post_single_ot;
            shiftOtData.post_double_ot = (int)currentShiftDetail.post_double_ot;
            shiftOtData.post_triple_ot = (int)currentShiftDetail.post_triple_ot;
            shiftOtData.pre_single_ot_roundup = (int)currentShiftDetail.pre_single_ot_roundup;
            shiftOtData.pre_double_ot_roundup = (int)currentShiftDetail.pre_double_ot_roundup;
            shiftOtData.pre_triple_ot_roundup = (int)currentShiftDetail.pre_triple_ot_roundup;
            shiftOtData.post_single_ot_roundup = (int)currentShiftDetail.post_single_ot_roundup;
            shiftOtData.post_double_ot_roundup = (int)currentShiftDetail.post_double_ot_roundup;
            shiftOtData.post_triple_ot_roundup = (int)currentShiftDetail.post_triple_ot_roundup;

            // Open shift details
            shiftOtData.open_non_ot = currentShiftDetail.open_non_ot;
            shiftOtData.open_single_ot = currentShiftDetail.open_single_ot;
            shiftOtData.open_double_ot = currentShiftDetail.open_double_ot;
            shiftOtData.open_triple_ot = currentShiftDetail.open_triple_ot;

            //fix issue
            if (currentShiftDetail.pre_non_ot_compensate == null)
                shiftOtData.pre_non_ot_compensate = 0;
            else
                shiftOtData.pre_non_ot_compensate = (int)currentShiftDetail.pre_non_ot_compensate;

            if (currentShiftDetail.post_non_ot_compensate == null)
                shiftOtData.post_non_ot_compensate = 0;
            else
                shiftOtData.post_non_ot_compensate = (int)currentShiftDetail.post_non_ot_compensate;

            shiftMaster.dtl_Shift_OT_Configuration_Details = shiftOtData;

            dtl_Shift_Late_Configuration_Details shiftLateData = new dtl_Shift_Late_Configuration_Details();
            shiftLateData.late_in = currentShiftDetail.late_in;
            shiftLateData.morning_short_leave = currentShiftDetail.morning_short_leave;
            shiftLateData.morning_halfday_leave = currentShiftDetail.morning_halfday_leave;
            shiftLateData.early_out = currentShiftDetail.early_out;
            shiftLateData.evening_short_leave = currentShiftDetail.evening_short_leave;
            shiftLateData.evening_halfday_leave = currentShiftDetail.evening_halfday_leave;
            shiftLateData.late_grace_time = currentShiftDetail.late_grace_time;
            shiftLateData.late_grace_effect_time = currentShiftDetail.late_grace_effect_time;
            shiftLateData.early_grace_time = currentShiftDetail.early_grace_time;
            shiftLateData.early_grace_effect_time = currentShiftDetail.early_grace_effect_time;
            // Open shift details
            shiftLateData.open_late = currentShiftDetail.open_late;
            shiftLateData.open_short_leave = currentShiftDetail.open_short_leave;
            shiftLateData.open_halfday_leave = currentShiftDetail.open_halfday_leave;
            shiftLateData.open_fullday_leave = currentShiftDetail.open_fullday_leave;

            shiftMaster.dtl_Shift_Late_Configuration_Details = shiftLateData;
            shiftMaster.dtl_Shift_Additional_Day = addedDaysList.ToArray();
            shiftMaster.dtl_Shift_Break_Details = addedBreakList.ToArray();

            #region 2017-07-24

            shiftMaster.dtl_Shift_Covering_Details = addCoverList.FirstOrDefault();

            if (addCoverList.Any())
            {
                if (CurrentShiftCover == null)
                {
                    currentShiftCover = shiftMaster.dtl_Shift_Covering_Details;
                }
                dtl_Shift_Covering_Details shiftCover = new dtl_Shift_Covering_Details();
                shiftCover.covering_Description = currentShiftCover.covering_Description;
                shiftCover.covering_effect_time = currentShiftCover.covering_effect_time.GetValueOrDefault();
                shiftCover.maximum_late_time = currentShiftCover.maximum_late_time.GetValueOrDefault();
                shiftCover.covering_off_day_value = currentShiftCover.covering_off_day_value.GetValueOrDefault();
                shiftCover.covering_off_time = currentShiftCover.covering_off_time.GetValueOrDefault();
                shiftCover.covering_on_day_value = currentShiftCover.covering_on_day_value.GetValueOrDefault();
                shiftCover.covering_on_time = currentShiftCover.covering_on_time.GetValueOrDefault();

                shiftMaster.dtl_Shift_Covering_Details = shiftCover;
            }
            #endregion

            return shiftMaster;
        }

        #endregion

        #region Time Setting Methods

        void SetShiftDayTimes()
        {
            if (shiftInTime != null && shiftInDayRange != null)
            {
                TimeSpan inDay = TimeSpan.FromDays((int)shiftInDayRange.day_value);
                this.shiftInDayTime = DateTime.Today;
                this.shiftInDayTime = this.shiftInDayTime.Add(inDay + (TimeSpan)shiftInTime);
                if (currentShiftDetail != null)
                {
                    currentShiftDetail.shift_in_time = shiftInDayTime.TimeOfDay;
                    currentShiftDetail.shift_in_day_value = shiftInDayRange.day_value;
                }

                if (shiftOnTimeDuration != null)
                {
                    shiftOnDayTime = this.shiftInDayTime.Subtract((TimeSpan)shiftOnTimeDuration);
                }
            }
            if (shiftOutTime != null && shiftOutDayRange != null)
            {
                TimeSpan outDay = TimeSpan.FromDays((int)shiftOutDayRange.day_value);
                this.shiftOutDayTime = DateTime.Today;
                this.shiftOutDayTime = this.shiftOutDayTime.Add(outDay + (TimeSpan)shiftOutTime);
                if (currentShiftDetail != null)
                {
                    currentShiftDetail.shift_out_time = shiftOutDayTime.TimeOfDay;
                    currentShiftDetail.shift_out_day_value = shiftOutDayRange.day_value;
                }
                if (shiftOffTimeDuration != null)
                {
                    shiftOffDayTime = this.shiftOutDayTime.Add((TimeSpan)shiftOffTimeDuration);
                }
            }
            if (shiftInTime != null && shiftOutTime != null)
                IsValidBasicShiftTime();
        }

        void SetShiftMorningTimes()
        {
            if (shiftInDayTime != null)
            {
                if (preNoOtDuration != null)
                {
                    PreNoOtTime = shiftInDayTime.Subtract((TimeSpan)preNoOtDuration);
                    if (preSingleOtDuration != null)
                    {
                        PreSingleOtTime = preNoOtTime.Subtract((TimeSpan)preSingleOtDuration);
                        if (preDoubleOtDuration != null)
                        {
                            PreDoubleOtTime = preSingleOtTime.Subtract((TimeSpan)preDoubleOtDuration);
                            if (preTripleOtDuration != null)
                            {
                                PreTripleOtTime = preDoubleOtTime.Subtract((TimeSpan)preTripleOtDuration);
                            }
                        }
                    }
                }

                if (lateGraceEffectDuration != null)
                {
                    LateGraceEffectTime = shiftInDayTime.Add((TimeSpan)lateGraceEffectDuration);
                    if (lateGraceTimeDuration != null)
                    {
                        LateGraceRemainTime = lateGraceEffectTime.Add((TimeSpan)lateGraceTimeDuration);
                        if (lateInTimeDuration != null)
                        {
                            LateInTime = lateGraceRemainTime.Add((TimeSpan)lateInTimeDuration);
                            if (lateShotLeaveTimeDuration != null)
                            {
                                LateShortLeaveTime = lateInTime.Add((TimeSpan)lateShotLeaveTimeDuration);
                                if (lateHalfDayTimeDuration != null)
                                {
                                    LateHalfDayTime = lateShortLeaveTime.Add((TimeSpan)lateHalfDayTimeDuration);
                                }
                            }
                        }
                    }
                }
            }
        }

        void SetShiftEveningTimes()
        {
            if (shiftOutDayTime != null)
            {
                if (postNoOtDuration != null)
                {
                    PostNoOtTime = shiftOutDayTime.Add((TimeSpan)postNoOtDuration);
                    if (postSingleOtDuration != null)
                    {
                        PostSingleOtTime = postNoOtTime.Add((TimeSpan)postSingleOtDuration);
                        if (postDoubleOtDuration != null)
                        {
                            PostDoubleOtTime = postSingleOtTime.Add((TimeSpan)postDoubleOtDuration);
                            if (postTripleOtDuration != null)
                            {
                                PostTripleOtTime = postDoubleOtTime.Add((TimeSpan)postTripleOtDuration);
                            }
                        }
                    }
                }

                if (earlyGraceEffectDuration != null)
                {
                    EarlyGraceEffectTime = shiftOutDayTime.Subtract((TimeSpan)earlyGraceEffectDuration);
                    if (earlyGraceTimeDuration != null)
                    {
                        EarlyGraceRemainTime = earlyGraceEffectTime.Subtract((TimeSpan)earlyGraceTimeDuration);
                        if (earlyOutTimeDuration != null)
                        {
                            EarlyOutTime = earlyGraceRemainTime.Subtract((TimeSpan)earlyOutTimeDuration);
                            if (earlyShortLeaveTimeDuration != null)
                            {
                                EarlyShortLeaveTime = earlyOutTime.Subtract((TimeSpan)earlyShortLeaveTimeDuration);
                                if (earlyHalfDayTimeDuration != null)
                                {
                                    EarlyHalfDayTime = earlyShortLeaveTime.Subtract((TimeSpan)earlyHalfDayTimeDuration);
                                }
                            }
                        }
                    }
                }
            }
        }

        TimeSpan GetTimeFromMinutes(int? mins)
        {
            TimeSpan duration = new TimeSpan();
            if (mins >= 0)
            {
                duration = TimeSpan.FromMinutes((double)mins);
            }
            else
            {
                duration = TimeSpan.FromMinutes(0.00);
            }
            return duration;
        }

        #endregion

        #region Time Validation Methods

        #region Basic Shift Time Validation

        bool IsValidBasicShiftTime()
        {
            if ((shiftOutDayTime - shiftInDayTime).TotalMinutes <= 0)
            {
                clsMessages.setMessage("shift basic times could be invalid");
                return false;
            }
            return true;
        }

        bool IsValidPreOverTime()
        {
            TimeSpan totalPreOtDuration = (preNoOtDuration == null ? new TimeSpan(0, 0, 0) : (TimeSpan)preNoOtDuration) + (preSingleOtDuration == null ? new TimeSpan(0) : (TimeSpan)preSingleOtDuration) + (preDoubleOtDuration == null ? new TimeSpan(0) : (TimeSpan)preDoubleOtDuration) + (preTripleOtDuration == null ? new TimeSpan(0) : (TimeSpan)preTripleOtDuration);
            if (shiftInDayTime.Subtract(totalPreOtDuration) < shiftOnDayTime)
            {
                //clsMessages.setMessage("Pre OT times could be invalid");
                return false;
            }

            return true;
        }

        bool IsValidPostOverTime()
        {
            TimeSpan totalPostOtDuration = (postNoOtDuration == null ? new TimeSpan(0) : (TimeSpan)postNoOtDuration) + (postSingleOtDuration == null ? new TimeSpan(0) : (TimeSpan)postSingleOtDuration) + (postDoubleOtDuration == null ? new TimeSpan(0) : (TimeSpan)postDoubleOtDuration) + (postTripleOtDuration == null ? new TimeSpan(0) : (TimeSpan)postTripleOtDuration);
            if (shiftOutDayTime.Add(totalPostOtDuration) > shiftOffDayTime)
            {
                //clsMessages.setMessage("Post OT times could be invalid");
                return false;
            }
            return true;
        }

        bool IsValidLateAttendTime()
        {
            return true;
        }

        #endregion

        #region Split Shift Validation

        bool IsValidSplitShift()
        {
            if (isSplitShift)
            {
                if (addedBreakList.Count == 1)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        #endregion

        #endregion

        #region Data Clear Methods

        void ClearCurrentData()
        {
            // setting shift basic details
            ShiftInTime = null;
            ShiftOutTime = null;
            ShiftOnTimeDuration = null;
            ShiftOffTimeDuration = null;

            // setting pre ot details
            PreNoOtDuration = null;
            PreSingleOtDuration = null;
            PreDoubleOtDuration = null;
            PreTripleOtDuration = null;

            // setting post ot details
            PostNoOtDuration = null;
            PostSingleOtDuration = null;
            PostDoubleOtDuration = null;
            PostTripleOtDuration = null;

            // setting late attend details
            LateInTimeDuration = null;
            LateShotLeaveTimeDuration = null;
            LateHalfDayTimeDuration = null;
            LateFullLeaveTimeDuration = null;
            LateGraceTimeDuration = null;
            LateGraceEffectDuration = null;

            // setting early out details
            EarlyOutTimeDuration = null;
            EarlyShortLeaveTimeDuration = null;
            EarlyHalfDayTimeDuration = null;
            EarlyGraceTimeDuration = null;
            EarlyGraceEffectDuration = null;

            ShiftInDayValue = 0;
            ShiftOutDayValue = 0;
            ShiftDetailName = null;
            IsSplitShift = false;
        }

        #endregion

        #region Error Handling Methods

        public override string getValidationError(string PropertyName)
        {
            string error = null;
            hasError = false;
            switch (PropertyName)
            {
                case "ShiftInTime":
                    error = shiftInTime == null ? "require to be filled" : null; validatedProperty = PropertyName; // though this property is validated there could be some other properties to be validated.
                    break;
                case "ShiftOutTime":
                    error = shiftOutTime == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "ShiftOnTimeDuration":
                    error = shiftOnTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "ShiftOffTimeDuration":
                    error = shiftOffTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "PreNoOtDuration":
                    error = preNoOtDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    if (!IsValidPreOverTime())
                        error = "Pre No OT duration could be incorrect";
                    break;
                case "PreSingleOtDuration":
                    error = preSingleOtDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    if (!IsValidPreOverTime())
                        error = "Pre Single OT duration could be incorrect";
                    break;
                case "PreDoubleOtDuration":
                    error = preDoubleOtDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    if (!IsValidPreOverTime())
                        error = "Pre Double OT duration could be incorrect";
                    break;
                case "PreTripleOtDuration":
                    error = preTripleOtDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    if (!IsValidPreOverTime())
                        error = "Pre Triple OT duration could be incorrect";
                    break;
                case "PostNoOtDuration":
                    error = postNoOtDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    if (!IsValidPostOverTime())
                        error = "Post No OT duration could be incorrect";
                    break;
                case "PostSingleOtDuration":
                    error = postSingleOtDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    if (!IsValidPostOverTime())
                        error = "Post Single OT duration could be incorrect";
                    break;
                case "PostDoubleOtDuration":
                    error = postDoubleOtDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    if (!IsValidPostOverTime())
                        error = "Post Double OT duration could be incorrect";
                    break;
                case "PostTripleOtDuration":
                    error = postTripleOtDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    if (!IsValidPostOverTime())
                        error = "Post Triple OT duration could be incorrect";
                    break;
                case "LateGraceEffectDuration":
                    error = lateGraceEffectDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "LateGraceTimeDuration":
                    error = lateGraceTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "LateInTimeDuration":
                    error = lateInTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "LateShotLeaveTimeDuration":
                    error = lateShotLeaveTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "LateHalfDayTimeDuration":
                    error = lateHalfDayTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "EarlyGraceEffectDuration":
                    error = earlyGraceEffectDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "EarlyGraceTimeDuration":
                    error = earlyGraceTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "EarlyOutTimeDuration":
                    error = earlyOutTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "EarlyShortLeaveTimeDuration":
                    error = earlyShortLeaveTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "EarlyHalfDayTimeDuration":
                    error = earlyHalfDayTimeDuration == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                case "ShiftDetailName":
                    error = shiftDetailName == null ? "require to be filled" : null; validatedProperty = PropertyName;
                    break;
                //case "IsSplitShift":
                //    if (!IsValidSplitShift()) error = "only one shift-break could be assigned to a split-shift"; else error =null; validatedProperty = PropertyName;
                //    break;
            }
            if (error != null)
                hasError = true;
            return error;
        }

        #endregion

        #region Additional Days Calculations

        void PopulateCurrentAdditionalDay()
        {
            AdditionalDayCount = currentAdditionalDay.addition_day_count;
            AdditionalDayFromTime = currentAdditionalDay.from_time;
            AdditionalDayToTime = currentAdditionalDay.to_time;
            AdditionalDayFromDayValue = currentAdditionalDay.from_time_day_value;
            AdditionalDayToDayValue = currentAdditionalDay.to_time_day_value;
            AdditionalDayWorkStartTime = currentAdditionalDay.worktime_start;
            AdditionalDayWorkEndTime = currentAdditionalDay.worktime_end;
            AdditionalDayWorkStartDayValue = currentAdditionalDay.worktime_start_day_value;
            AdditionalDayWorkEndDayValue = currentAdditionalDay.worktime_end_day_value;
            //CurrentCheckType = currentAdditionalDay.check_type;
            AdditionalDayDescription = currentAdditionalDay.addition_day_description;
        }

        void ClearAdditionalDayData()
        {
            CurrentAdditionalDay = null;
            CurrentAdditionalDay = new dtl_Shift_Additional_Day();
            AdditionalDayCount = null;
            AdditionalDayFromTime = null;
            AdditionalDayToTime = null;
            AdditionalDayFromDayValue = null;
            AdditionalDayToDayValue = null;
            AdditionalDayWorkStartTime = null;
            AdditionalDayWorkEndTime = null;
            AdditionalDayWorkStartDayValue = null;
            AdditionalDayWorkEndDayValue = null;
            additionalDayWorkTimeDuration = null;
            //CurrentCheckType = null;
            AdditionalDayDescription = null;
        }

        void ValidateCheckType()
        {
            if (isCheckTypeWork)
            {
                AdditionalDayFromTime = null;
                AdditionalDayToTime = null;
                AdditionalDayFromDayValue = null;
                AdditionalDayToDayValue = null;
            }
            if (isCheckTypeNotWork)
            {
                AdditionalDayWorkStartTime = null;
                AdditionalDayWorkEndTime = null;
                AdditionalDayWorkStartDayValue = null;
                AdditionalDayWorkEndDayValue = null;
            }
        }

        dtl_Shift_Additional_Day SetAdditionalDayData()
        {
            dtl_Shift_Additional_Day day = new dtl_Shift_Additional_Day();
            if (currentAdditionalDay != null && currentAdditionalDay.addition_day_id != 0)
                day.addition_day_id = currentAdditionalDay.addition_day_id;
            if (currentShiftDetail != null && currentShiftDetail.shift_detail_id != 0)
                day.shift_detail_id = currentShiftDetail.shift_detail_id;
            day.addition_day_count = additionalDayCount;
            day.addition_day_description = additionalDayDescription;
            //day.check_type = currentCheckType;
            day.from_time = additionalDayFromTime;
            day.to_time = additionalDayToTime;
            day.from_time_day_value = additionalDayFromDayValue;
            day.to_time_day_value = additionalDayToDayValue;
            day.worktime_start = additionalDayWorkStartTime;
            day.worktime_end = additionalDayWorkEndTime;
            day.worktime_start_day_value = additionalDayWorkStartDayValue;
            day.worktime_end_day_value = additionalDayWorkEndDayValue;
            if (additionalDayWorkTimeDuration != null)
                day.worktime_duration = (decimal)((TimeSpan)additionalDayWorkTimeDuration).TotalMinutes;
            else
                day.worktime_duration = null;

            return day;
        }

        #endregion

        #region Shift Break Calculations

        void PopulateCurrentShiftBreak()
        {
            BreakInTime = currentShiftBreak.break_in_time;
            BreakOutTime = currentShiftBreak.break_out_time;
            BreakOnTime = currentShiftBreak.break_on_time;
            BreakOffTime = currentShiftBreak.break_off_time;
            BreakInDayValue = currentShiftBreak.break_in_day_value;
            BreakOutDayValue = currentShiftBreak.break_out_day_value;
            BreakOnDayValue = currentShiftBreak.break_on_day_value;
            BreakOffDayValue = currentShiftBreak.break_off_day_value;
            BreakDescription = currentShiftBreak.break_description;

        }

        void ClearCurrentShiftBreak()
        {
            CurrentShiftBreak = null;
            CurrentShiftBreak = new dtl_Shift_Break_Details();
            BreakInTime = null;
            BreakOutTime = null;
            BreakOnTime = null;
            BreakOffTime = null;
            BreakInDayValue = null;
            BreakOutDayValue = null;
            BreakOnDayValue = null;
            BreakOffDayValue = null;
            BreakDescription = null;
        }

        dtl_Shift_Break_Details SetShiftBreakData()
        {
            dtl_Shift_Break_Details shiftBreak = new dtl_Shift_Break_Details();
            if (currentShiftDetail != null && currentShiftDetail.shift_detail_id != 0)
                shiftBreak.shift_detail_id = currentShiftDetail.shift_detail_id;
            if (currentShiftBreak != null && currentShiftBreak.break_id != 0)
                shiftBreak.break_id = currentShiftBreak.break_id;
            shiftBreak.break_in_time = breakInTime;
            shiftBreak.break_out_time = breakOutTime;
            shiftBreak.break_on_time = breakOnTime;
            shiftBreak.break_off_time = breakOffTime;
            shiftBreak.break_in_day_value = breakOffDayValue;
            shiftBreak.break_out_day_value = breakOutDayValue;
            shiftBreak.break_on_day_value = breakOnDayValue;
            shiftBreak.break_off_day_value = breakOffDayValue;
            shiftBreak.break_description = breakDescription;

            return shiftBreak;
        }

        #endregion

        //updated 2017-07-24
        #region Shift Cover Calculations

        dtl_Shift_Covering_Details SetShiftCoverData()
        {
            dtl_Shift_Covering_Details shiftCover = new dtl_Shift_Covering_Details();
            if (currentShiftDetail != null && currentShiftDetail.shift_detail_id != 0)
                shiftCover.shift_detail_id = currentShiftDetail.shift_detail_id;

            shiftCover.covering_Description = coverDescription;
            shiftCover.covering_effect_time = coverEffTime;
            shiftCover.maximum_late_time = coverMaxLate;
            shiftCover.covering_on_time = coverOnTime;
            shiftCover.covering_off_time = coverOffTime;
            shiftCover.covering_on_day_value = coverOnDayValue;
            shiftCover.covering_off_day_value = CoverOffDayValue;

            return shiftCover;
        }

        void ClearCurrentShiftCover()
        {
            //CurrentShiftCover = null;
            //CurrentShiftCover = new dtl_Shift_Covering_Details();

            CoverOnTime = null;
            CoverOffTime = null;
            CoverOnDayValue = null;
            CoverOffDayValue = null;
            CoverDescription = null;
            CoverEffTime = null;
            CoverMaxLate = null;
        }

        void PopulateCurrentShiftCover()
        {
            CoverOnTime = currentShiftCover.covering_on_time;
            CoverOffTime = currentShiftCover.covering_off_time;
            CoverOnDayValue = currentShiftCover.covering_on_day_value;
            CoverOffDayValue = currentShiftCover.covering_off_day_value;
            CoverDescription = currentShiftCover.covering_Description;
            CoverMaxLate = currentShiftCover.maximum_late_time;
            CoverEffTime = currentShiftCover.covering_effect_time;

        }

        #endregion

        #region Refresh Errors

        //void RefreshPreOTErrors(string propertyName)
        //{
        //    List<string> properties = new List<string>();
        //    properties.Add("PreNoOtDuration");
        //    properties.Add("PreSingleOtDuration");
        //    properties.Add("PreDoubleOtDuration");
        //    properties.Add("PreTripleOtDuration");

        //    properties.Remove(propertyName);
        //    properties.ForEach(c => OnPropertyChanged(c));
        //}

        //void RefreshPostOTErrors(string propertyName)
        //{
        //    List<string> properties = new List<string>();
        //    properties.Add("PostNoOtDuration");
        //    properties.Add("PostSingleOtDuration");
        //    properties.Add("PostDoubleOtDuration");
        //    properties.Add("PostTripleOtDuration");

        //    properties.Remove(propertyName);
        //    properties.ForEach(c => OnPropertyChanged(c));
        //}

        void RefreshShiftErrors(string validatedProperty)
        {
            foreach (string propName in validatingPropertyList.Where(c => c != validatedProperty))
            {
                if (!hasError)
                    OnPropertyChanged(propName);
                else
                    break;
            }
        }

        void SetValidatingProperties()
        {
            // adding all properties to be validated when saving a new shift or updating a existing one
            validatingPropertyList.Add("ShiftDetailName");
            validatingPropertyList.Add("ShiftInTime");
            validatingPropertyList.Add("ShiftOutTime");
            validatingPropertyList.Add("ShiftOnTimeDuration");
            validatingPropertyList.Add("ShiftOffTimeDuration");
            validatingPropertyList.Add("PreNoOtDuration");
            validatingPropertyList.Add("PreSingleOtDuration");
            validatingPropertyList.Add("PreSingleOtDuration");
            validatingPropertyList.Add("PreDoubleOtDuration");
            validatingPropertyList.Add("PreTripleOtDuration");
            validatingPropertyList.Add("PostNoOtDuration");
            validatingPropertyList.Add("PostSingleOtDuration");
            validatingPropertyList.Add("PostDoubleOtDuration");
            validatingPropertyList.Add("PostTripleOtDuration");
            validatingPropertyList.Add("LateGraceEffectDuration");
            validatingPropertyList.Add("LateGraceTimeDuration");
            validatingPropertyList.Add("LateInTimeDuration");
            validatingPropertyList.Add("LateShotLeaveTimeDuration");
            validatingPropertyList.Add("LateHalfDayTimeDuration");
            validatingPropertyList.Add("EarlyGraceEffectDuration");
            validatingPropertyList.Add("EarlyGraceTimeDuration");
            validatingPropertyList.Add("EarlyOutTimeDuration");
            validatingPropertyList.Add("EarlyShortLeaveTimeDuration");
            validatingPropertyList.Add("EarlyHalfDayTimeDuration");
        }

        #endregion

        #region Open Shift
        void OpenShiftBoolConvert()
        {

            IsBoolConvert = IsOpenShift == true ? false : true;
            if (IsBoolConvert == false)
            {
                if (ShiftInTime == null && (currentShiftDetail != null && currentShiftDetail.shift_detail_id == 0))
                    ShiftInTime = GetTimeFromMinutes(0);
                if (ShiftOutTime == null && (currentShiftDetail != null && currentShiftDetail.shift_detail_id == 0))
                    ShiftOutTime = GetTimeFromMinutes(720);
                ShiftOnTimeDuration = GetTimeFromMinutes(0);
                ShiftOffTimeDuration = GetTimeFromMinutes(0);

                LateGraceEffectDuration = GetTimeFromMinutes(0);
                LateGraceTimeDuration = GetTimeFromMinutes(0);
                LateInTimeDuration = GetTimeFromMinutes(0);
                LateShotLeaveTimeDuration = GetTimeFromMinutes(0);
                LateHalfDayTimeDuration = GetTimeFromMinutes(0);
                LateFullLeaveTimeDuration = GetTimeFromMinutes(0);

                EarlyGraceEffectDuration = GetTimeFromMinutes(0);
                EarlyGraceTimeDuration = GetTimeFromMinutes(0);
                EarlyOutTimeDuration = GetTimeFromMinutes(0);
                EarlyShortLeaveTimeDuration = GetTimeFromMinutes(0);
                EarlyHalfDayTimeDuration = GetTimeFromMinutes(0);

                PreNoOtDuration = GetTimeFromMinutes(0);
                PreSingleOtDuration = GetTimeFromMinutes(0);
                PreDoubleOtDuration = GetTimeFromMinutes(0);
                PreTripleOtDuration = GetTimeFromMinutes(0);

                PreSingleOtRound = 0;
                PreDoubleOtRound = 0;
                PreTripleOtRound = 0;
                PreNonOtCompensate = 0;

                PostNoOtDuration = GetTimeFromMinutes(0);
                PostSingleOtDuration = GetTimeFromMinutes(0);
                PostDoubleOtDuration = GetTimeFromMinutes(0);
                PostTripleOtDuration = GetTimeFromMinutes(0);

                PostSingleOtRound = 0;
                PostDoubleOtRound = 0;
                PostTripleOtRound = 0;
                PostNonOtCompensate = 0;
            }
            else
            {
                //ShiftInTime = null;
                //ShiftOutTime = null;
                //ShiftOnTimeDuration = null;
                //ShiftOffTimeDuration = null;

                //LateGraceEffectDuration = null;
                //LateGraceTimeDuration = null;
                //LateInTimeDuration = null;
                //LateShotLeaveTimeDuration = null;
                //LateHalfDayTimeDuration = null;
                //LateFullLeaveTimeDuration = null;

                //EarlyGraceEffectDuration = null;
                //EarlyGraceTimeDuration = null;
                //EarlyOutTimeDuration = null;
                //EarlyShortLeaveTimeDuration = null;
                //EarlyHalfDayTimeDuration = null;

                //PreNoOtDuration = null;
                //PreSingleOtDuration = null;
                //PreDoubleOtDuration = null;
                //PreTripleOtDuration = null;

                //PreSingleOtRound = 0;
                //PreDoubleOtRound = 0;
                //PreTripleOtRound = 0;
                //PreNonOtCompensate = 0;

                //PostNoOtDuration = null;
                //PostSingleOtDuration = null;
                //PostDoubleOtDuration = null;
                //PostTripleOtDuration = null;

                //PostSingleOtRound = 0;
                //PostDoubleOtRound = 0;
                //PostTripleOtRound = 0;
                //PostNonOtCompensate = 0;

            }
        }
        #endregion
    }
}
