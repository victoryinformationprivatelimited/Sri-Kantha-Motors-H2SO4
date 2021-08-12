using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using AttendanceData;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Configuration;
using AttendanceOnlineService.HelperClasses;
using System.IO;
using System.Security.AccessControl;
using System.Data.Objects.DataClasses;


namespace AttendanceOnlineService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceContract]
    public class AttendanceService
    {
        #region Attendance Module

        private bool validateSaveUpdate(int result)
        {
            bool isSave = false;
            if (result > 0)
            {
                isSave = true;
            }
            else
            {
                isSave = false;
            }
            return isSave;
        }

        #region Table Operations

        #region dtl_EmployeeRule operations

        bool UpdateAttendanceBonusDeductionAmount(List<dtl_EmployeeRule> updatedList)
        {
            using (var context = new AttendanceEntities())
            {
                foreach (var item in updatedList)
                {
                    var current = context.dtl_EmployeeRule.FirstOrDefault(c => c.rule_id == item.rule_id && c.employee_id == item.employee_id);
                    if (current != null)
                    {
                        current.special_amount = item.special_amount;
                        current.modified_datetime = item.modified_datetime;
                        current.modified_user_id = item.modified_user_id;
                    }
                }

                if (validateSaveUpdate(context.SaveChanges()))
                    return true;
            }

            return false;
        }

        #endregion

        #region dtl_AttendanceGroup operations

        [OperationContract]
        public bool SaveAttendanceGroupEmployees(List<dtl_AttendanceGroup> groupList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (dtl_AttendanceGroup item in groupList)
                    {
                        var current = context.dtl_AttendanceGroup.FirstOrDefault(c => c.employee_id == item.employee_id);
                        if (current == null)
                        {
                            dtl_AttendanceGroup savedGroup = new dtl_AttendanceGroup();
                            savedGroup.attendance_group_id = item.attendance_group_id;
                            savedGroup.employee_id = item.employee_id;
                            savedGroup.saved_datetime = item.saved_datetime;
                            savedGroup.saved_user_id = item.saved_user_id;
                            savedGroup.is_active = item.is_active;
                            savedGroup.is_delete = item.is_delete;
                            context.dtl_AttendanceGroup.AddObject(savedGroup);
                        }
                        else
                        {
                            current.attendance_group_id = item.attendance_group_id;
                            current.is_active = item.is_active;
                            current.is_delete = item.is_delete;
                            current.modified_datetime = item.modified_datetime;
                            current.modified_user_id = item.modified_user_id;
                        }
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region dtl_Shift_Master operations

        [OperationContract]
        public bool SaveShiftMasterDetails(dtl_Shift_Master addedShift)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    int nextOrderNo = getNextShiftOrderNumber();

                    if (nextOrderNo != 0)
                    {
                        addedShift.dtl_ShiftOrder = new dtl_ShiftOrder { order_number = nextOrderNo };
                        context.dtl_Shift_Master.AddObject(addedShift);
                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool UpdateShiftMasterDetails(dtl_Shift_Master updatedShift)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    dtl_Shift_Master current = context.dtl_Shift_Master.FirstOrDefault(c => c.shift_detail_id == updatedShift.shift_detail_id);
                    if (current != null)
                    {
                        current.shift_detail_name = updatedShift.shift_detail_name;
                        current.shift_category_id = updatedShift.shift_category_id;
                        current.is_daily_shift = updatedShift.is_daily_shift;
                        current.is_roster = updatedShift.is_roster;
                        current.is_split_shift = updatedShift.is_split_shift;
                        current.is_entitle_leiu_leave = updatedShift.is_entitle_leiu_leave;
                        current.is_executive = updatedShift.is_executive;
                        current.is_stores_executive = updatedShift.is_stores_executive;
                        current.is_nonexecutive = updatedShift.is_nonexecutive;
                        current.is_stores_nonexecutive = updatedShift.is_stores_nonexecutive;
                        current.is_security = updatedShift.is_security;
                        current.is_ot_shift = updatedShift.is_ot_shift;
                        current.is_single = updatedShift.is_single;
                        current.is_multiple = updatedShift.is_multiple;
                        current.is_halfday = updatedShift.is_halfday;
                        current.is_late_deduc = updatedShift.is_late_deduc;
                        current.is_mor_inc = updatedShift.is_mor_inc;
                        current.modified_datetime = updatedShift.modified_datetime;
                        current.modified_user_id = updatedShift.modified_user_id;
                        dtl_Shift_Detail_Basic updatingBasicShift = updatedShift.dtl_Shift_Detail_Basic;
                        #region 2017-07-24
                        dtl_Shift_Covering_Details updatingCoverDetails = updatedShift.dtl_Shift_Covering_Details;
                        #endregion
                        dtl_Shift_OT_Configuration_Details updatingOtShift = updatedShift.dtl_Shift_OT_Configuration_Details;
                        dtl_Shift_Late_Configuration_Details updatingLateShift = updatedShift.dtl_Shift_Late_Configuration_Details;
                        List<dtl_Shift_Additional_Day> additionalDays = updatedShift.dtl_Shift_Additional_Day.ToList();
                        List<dtl_Shift_Break_Details> shiftBreaks = updatedShift.dtl_Shift_Break_Details.ToList();

                        if (UpdateBasicShiftDetails(updatingBasicShift))
                        {
                            if (UpdateShiftOtDetails(updatingOtShift))
                            {
                                if (UpdateLateShiftDetails(updatingLateShift))
                                {
                                    if (updatingCoverDetails == null ? true : SaveShiftCoveringDetails(updatingCoverDetails))
                                    {
                                        if (additionalDays.Count > 0)
                                        {
                                            if (!SaveShiftAdditionalDays(additionalDays))
                                                return false;
                                        }
                                        if (shiftBreaks.Count > 0)
                                        {
                                            if (!SaveShiftBreakDetails(shiftBreaks))
                                                return false;
                                        }


                                        if (validateSaveUpdate(context.SaveChanges()))
                                            return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool DeleteShiftMasterDetails(dtl_Shift_Master deleteShift)
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    dtl_Shift_Master current = context.dtl_Shift_Master.FirstOrDefault(c => c.shift_detail_id == deleteShift.shift_detail_id);
                    current.is_delete = deleteShift.is_delete;
                    current.delete_datetime = deleteShift.delete_datetime;
                    current.delete_user_id = deleteShift.delete_user_id;

                    return validateSaveUpdate(context.SaveChanges());
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }

        [OperationContract]
        public IEnumerable<dtl_Shift_Master> GetShiftWithCategories()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_Shift_Master.Include("z_Shift_Category").ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public IEnumerable<dtl_Shift_Master> GetShiftNames()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_Shift_Master.Where(c => c.is_delete == false).Select(c => new { SID = c.shift_detail_id, SCAT = c.shift_category_id, SNAME = c.shift_detail_name }).ToList().Select(c => new dtl_Shift_Master { shift_category_id = c.SCAT, shift_detail_id = c.SID, shift_detail_name = c.SNAME });
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region dtl_Shift_Detail_Basic operations

        bool UpdateBasicShiftDetails(dtl_Shift_Detail_Basic updatedBasicShift)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    dtl_Shift_Detail_Basic current = context.dtl_Shift_Detail_Basic.FirstOrDefault(c => c.shift_detail_id == updatedBasicShift.shift_detail_id);
                    if (current != null)
                    {
                        current.shift_in_day_value = updatedBasicShift.shift_in_day_value;
                        current.shift_in_time = updatedBasicShift.shift_in_time;
                        current.shift_out_day_value = updatedBasicShift.shift_out_day_value;
                        current.shift_out_time = updatedBasicShift.shift_out_time;
                        current.shift_on_time = updatedBasicShift.shift_on_time;
                        current.shift_off_time = updatedBasicShift.shift_off_time;

                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region  dtl_Shift_OT_Configuration_Details operations

        bool UpdateShiftOtDetails(dtl_Shift_OT_Configuration_Details updatedOtShift)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    dtl_Shift_OT_Configuration_Details current = context.dtl_Shift_OT_Configuration_Details.FirstOrDefault(c => c.shift_detail_id == updatedOtShift.shift_detail_id);
                    if (current != null)
                    {
                        current.pre_non_ot = updatedOtShift.pre_non_ot;
                        current.pre_single_ot = updatedOtShift.pre_single_ot;
                        current.pre_double_ot = updatedOtShift.pre_double_ot;
                        current.pre_triple_ot = updatedOtShift.pre_triple_ot;
                        current.post_non_ot = updatedOtShift.post_non_ot;
                        current.post_single_ot = updatedOtShift.post_single_ot;
                        current.post_double_ot = updatedOtShift.post_double_ot;
                        current.post_triple_ot = updatedOtShift.post_triple_ot;
                        current.pre_single_ot_roundup = updatedOtShift.pre_single_ot_roundup;
                        current.pre_double_ot_roundup = updatedOtShift.pre_double_ot_roundup;
                        current.pre_triple_ot_roundup = updatedOtShift.pre_triple_ot_roundup;
                        current.post_single_ot_roundup = updatedOtShift.post_single_ot_roundup;
                        current.post_double_ot_roundup = updatedOtShift.post_double_ot_roundup;
                        current.post_triple_ot_roundup = updatedOtShift.post_triple_ot_roundup;
                        current.pre_non_ot_compensate = updatedOtShift.pre_non_ot_compensate;
                        current.post_non_ot_compensate = updatedOtShift.post_non_ot_compensate;

                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region dtl_Shift_Late_Configuration_Details operations

        bool UpdateLateShiftDetails(dtl_Shift_Late_Configuration_Details updatedLateShift)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    dtl_Shift_Late_Configuration_Details current = context.dtl_Shift_Late_Configuration_Details.FirstOrDefault(c => c.shift_detail_id == updatedLateShift.shift_detail_id);
                    if (current != null)
                    {
                        current.late_in = updatedLateShift.late_in;
                        current.late_grace_time = updatedLateShift.late_grace_time;
                        current.late_grace_effect_time = updatedLateShift.late_grace_effect_time;
                        current.morning_short_leave = updatedLateShift.morning_short_leave;
                        current.morning_halfday_leave = updatedLateShift.morning_halfday_leave;
                        current.fullday_leave = updatedLateShift.fullday_leave;
                        current.early_out = updatedLateShift.early_out;
                        current.early_grace_time = updatedLateShift.early_grace_time;
                        current.early_grace_effect_time = updatedLateShift.early_grace_effect_time;
                        current.evening_short_leave = updatedLateShift.evening_short_leave;
                        current.evening_halfday_leave = updatedLateShift.evening_halfday_leave;


                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region dtl_Shift_Additional_Day operations

        [OperationContract]
        public IEnumerable<dtl_Shift_Additional_Day> GetAdditionalDaysByShift(int shiftDetailID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_Shift_Additional_Day.Where(c => c.shift_detail_id == shiftDetailID && c.is_delete == false).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        bool SaveShiftAdditionalDays(List<dtl_Shift_Additional_Day> addedDays)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (dtl_Shift_Additional_Day day in addedDays)
                    {
                        dtl_Shift_Additional_Day current = context.dtl_Shift_Additional_Day.FirstOrDefault(c => c.shift_detail_id == day.shift_detail_id && c.addition_day_id == day.addition_day_id);
                        if (current == null)
                        {
                            context.dtl_Shift_Additional_Day.AddObject(day);
                        }
                        else
                        {
                            current.addition_day_count = day.addition_day_count;
                            current.from_time = day.from_time;
                            current.from_time_day_value = day.from_time_day_value;
                            current.to_time = day.to_time;
                            current.to_time_day_value = day.to_time_day_value;
                            current.worktime_start = day.worktime_start;
                            current.worktime_start_day_value = day.worktime_start_day_value;
                            current.worktime_end = day.worktime_end;
                            current.worktime_end_day_value = day.worktime_end_day_value;
                            current.check_type = day.check_type;
                        }
                    }
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        bool DeleteShiftAdditionalDays(List<dtl_Shift_Additional_Day> deletedDays)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (dtl_Shift_Additional_Day day in deletedDays)
                    {
                        dtl_Shift_Additional_Day current = context.dtl_Shift_Additional_Day.FirstOrDefault(c => c.shift_detail_id == day.shift_detail_id && c.addition_day_id == day.addition_day_id);
                        if (current != null)
                        {
                            current.is_delete = day.is_delete;
                            current.delete_datetime = day.delete_datetime;
                            current.delete_user_id = day.delete_user_id;
                        }
                    }
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region dtl_Shift_Break_Details operations

        [OperationContract]
        public IEnumerable<dtl_Shift_Break_Details> GetShiftBreaksByShift(int shiftID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_Shift_Break_Details.Where(c => c.is_delete == false && c.shift_detail_id == shiftID).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        bool SaveShiftBreakDetails(List<dtl_Shift_Break_Details> addedBreaks)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (dtl_Shift_Break_Details addingBreak in addedBreaks)
                    {
                        dtl_Shift_Break_Details current = context.dtl_Shift_Break_Details.FirstOrDefault(c => c.shift_detail_id == addingBreak.shift_detail_id && c.break_id == addingBreak.break_id);
                        if (current != null)
                        {
                            current.break_description = addingBreak.break_description;
                            current.break_in_time = addingBreak.break_in_time;
                            current.break_out_time = addingBreak.break_out_time;
                            current.break_in_day_value = addingBreak.break_in_day_value;
                            current.break_out_day_value = addingBreak.break_out_day_value;
                            current.break_on_time = addingBreak.break_on_time;
                            current.break_off_time = addingBreak.break_off_time;
                            current.break_on_day_value = addingBreak.break_on_day_value;
                            current.break_off_day_value = addingBreak.break_off_day_value;
                        }
                        else
                        {
                            dtl_Shift_Break_Details addedBreak = new dtl_Shift_Break_Details();
                            addedBreak.shift_detail_id = addingBreak.shift_detail_id;
                            addedBreak.break_description = addingBreak.break_description;
                            addedBreak.break_in_time = addingBreak.break_in_time;
                            addedBreak.break_out_time = addingBreak.break_out_time;
                            addedBreak.break_in_day_value = addingBreak.break_in_day_value;
                            addedBreak.break_out_day_value = addingBreak.break_out_day_value;
                            addedBreak.break_on_time = addingBreak.break_on_time;
                            addedBreak.break_off_time = addingBreak.break_off_time;
                            addedBreak.break_on_day_value = addingBreak.break_on_day_value;
                            addedBreak.break_off_day_value = addingBreak.break_off_day_value;

                            context.dtl_Shift_Break_Details.AddObject(addedBreak);
                        }
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        bool DeleteShiftBreakDetails(List<dtl_Shift_Break_Details> deletedBreaks)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (dtl_Shift_Break_Details deletingBreak in deletedBreaks)
                    {
                        dtl_Shift_Break_Details current = context.dtl_Shift_Break_Details.FirstOrDefault(c => c.shift_detail_id == deletingBreak.shift_detail_id && c.break_id == deletingBreak.break_id);
                        if (current != null)
                        {
                            current.is_delete = deletingBreak.is_delete;
                            current.delete_datetime = deletingBreak.delete_datetime;
                            current.delete_user_id = deletingBreak.delete_user_id;
                        }

                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        //#region MCN

        //#region dtl_Shift_Covering_Details operation
        //[OperationContract]
        //public IEnumerable<dtl_Shift_Covering_Details> GetShiftConvering(int shiftID)
        //{
        //    try
        //    {
        //        using (var context = new AttendanceEntities())
        //        {
        //            var result = context.dtl_Shift_Covering_Details.Where(c => c.is_delete == false && c.shift_detail_id == shiftID).ToList();
        //            result.ForEach(c => context.Detach(c));
        //            return result;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return null;
        //}

        //[OperationContract]
        //bool SaveShiftCoveringDetails(List<dtl_Shift_Covering_Details> addedCovering)
        //{
        //    try
        //    {
        //        using (var context = new AttendanceEntities())
        //        {
        //            foreach (dtl_Shift_Covering_Details addingCove in addedCovering)
        //            {
        //                dtl_Shift_Covering_Details current = context.dtl_Shift_Covering_Details.FirstOrDefault(c => c.shift_detail_id == addingCove.shift_detail_id);
        //                if (current != null)
        //                {
        //                    current.covering_Description = addingCove.covering_Description;
        //                    current.covering_on_time = addingCove.covering_on_time;
        //                    current.covering_off_time = addingCove.covering_off_time;
        //                    current.covering_on_day_value = addingCove.covering_on_day_value;
        //                    current.covering_off_day_value = addingCove.covering_off_day_value;
        //                    current.maximum_late_time = addingCove.maximum_late_time;
        //                    current.covering_effect_time = addingCove.covering_effect_time;
        //                }
        //                else
        //                {
        //                    dtl_Shift_Covering_Details addingCoves = new dtl_Shift_Covering_Details();
        //                    addingCoves.shift_detail_id = addingCove.shift_detail_id;
        //                    addingCoves.covering_Description = addingCove.covering_Description;
        //                    addingCoves.covering_on_time = addingCove.covering_on_time;
        //                    addingCoves.covering_off_time = addingCove.covering_off_time;
        //                    addingCoves.covering_on_day_value = addingCove.covering_on_day_value;
        //                    addingCoves.covering_off_day_value = addingCove.covering_off_day_value;
        //                    addingCoves.maximum_late_time = addingCove.maximum_late_time;
        //                    addingCoves.covering_effect_time = addingCove.covering_effect_time;

        //                    context.dtl_Shift_Covering_Details.AddObject(addingCoves);
        //                }
        //            }

        //            if (validateSaveUpdate(context.SaveChanges()))
        //                return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return false;
        //}

        //[OperationContract]
        //bool DeleteShiftCoveringDetails(List<dtl_Shift_Covering_Details> deletedCovering)
        //{
        //    try
        //    {
        //        using (var context = new AttendanceEntities())
        //        {
        //            foreach (dtl_Shift_Covering_Details deletingCov in deletedCovering)
        //            {
        //                dtl_Shift_Covering_Details current = context.dtl_Shift_Covering_Details.FirstOrDefault(c => c.shift_detail_id == deletingCov.shift_detail_id);
        //                if (current != null)
        //                {
        //                    current.is_delete = deletingCov.is_delete;
        //                    current.delete_datetime = deletingCov.delete_datetime;
        //                    current.delete_user_id = deletingCov.delete_user_id;
        //                }

        //            }

        //            if (validateSaveUpdate(context.SaveChanges()))
        //                return true;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return false;
        //}

        //#endregion

        //#endregion

        #region MCN

        #region dtl_Shift_Covering_Details operation
        [OperationContract]
        public IEnumerable<dtl_Shift_Covering_Details> GetShiftConvering(int shiftID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_Shift_Covering_Details.Where(c => (c.is_delete == false || c.is_delete == null) && c.shift_detail_id == shiftID).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        bool SaveShiftCoveringDetails(dtl_Shift_Covering_Details addedCovering)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {

                    dtl_Shift_Covering_Details current = context.dtl_Shift_Covering_Details.FirstOrDefault(c => c.shift_detail_id == addedCovering.shift_detail_id);
                    if (current != null)
                    {
                        current.covering_Description = addedCovering.covering_Description;
                        current.covering_on_time = addedCovering.covering_on_time;
                        current.covering_off_time = addedCovering.covering_off_time;
                        current.covering_on_day_value = addedCovering.covering_on_day_value;
                        current.covering_off_day_value = addedCovering.covering_off_day_value;
                        current.maximum_late_time = addedCovering.maximum_late_time;
                        current.covering_effect_time = addedCovering.covering_effect_time;
                        current.is_delete = addedCovering.is_delete == null ? false : addedCovering.is_delete;
                    }
                    else
                    {
                        dtl_Shift_Covering_Details addingCoves = new dtl_Shift_Covering_Details();
                        addingCoves.shift_detail_id = addedCovering.shift_detail_id;
                        addingCoves.covering_Description = addedCovering.covering_Description;
                        addingCoves.covering_on_time = addedCovering.covering_on_time;
                        addingCoves.covering_off_time = addedCovering.covering_off_time;
                        addingCoves.covering_on_day_value = addedCovering.covering_on_day_value;
                        addingCoves.covering_off_day_value = addedCovering.covering_off_day_value;
                        addingCoves.maximum_late_time = addedCovering.maximum_late_time;
                        addingCoves.covering_effect_time = addedCovering.covering_effect_time;
                        addingCoves.is_delete = addedCovering.is_delete == null ? false : addedCovering.is_delete;

                        context.dtl_Shift_Covering_Details.AddObject(addingCoves);
                    }


                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        bool DeleteShiftCoveringDetails(List<dtl_Shift_Covering_Details> deletedCovering)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (dtl_Shift_Covering_Details deletingCov in deletedCovering)
                    {
                        dtl_Shift_Covering_Details current = context.dtl_Shift_Covering_Details.FirstOrDefault(c => c.shift_detail_id == deletingCov.shift_detail_id);
                        if (current != null)
                        {
                            current.is_delete = deletingCov.is_delete;
                            current.delete_datetime = deletingCov.delete_datetime;
                            current.delete_user_id = deletingCov.delete_user_id;
                        }

                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #endregion

        #region dtl_ShiftOrder operations

        [OperationContract]
        IEnumerable<dtl_Shift_Master> GetShiftOrderDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_Shift_Master.Where(c => c.is_delete == false).Select(
                                    c => new { SID = c.shift_detail_id, SNAME = c.shift_detail_name, ORDER_NO = c.dtl_ShiftOrder.order_number }
                        ).ToList().Select(c => new dtl_Shift_Master { shift_detail_id = c.SID, shift_detail_name = c.SNAME, dtl_ShiftOrder = new dtl_ShiftOrder { order_number = c.ORDER_NO } });
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return new List<dtl_Shift_Master>();
        }

        [OperationContract]
        bool UpdateShiftOrderDetails(List<dtl_Shift_Master> orderedShiftsList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (dtl_Shift_Master shift in orderedShiftsList)
                    {
                        var current = context.dtl_Shift_Master.FirstOrDefault(c => c.shift_detail_id == shift.shift_detail_id);
                        current.dtl_ShiftOrder.order_number = shift.dtl_ShiftOrder.order_number;
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        int getNextShiftOrderNumber()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var lastShiftOrder = context.dtl_ShiftOrder.OrderByDescending(c => c.order_number).FirstOrDefault();
                    if (lastShiftOrder != null)
                        return (int)lastShiftOrder.order_number + 1;
                    else
                        return 1;
                }
            }
            catch (Exception)
            {
            }
            return 0;
        }

        #endregion

        #region z_Shift_Category operations

        [OperationContract]
        public IEnumerable<z_Shift_Category> GetShiftCategoryDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_Shift_Category.Where(c => c.is_delete == false).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public bool SaveShiftCategory(z_Shift_Category addedCategory)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    context.z_Shift_Category.AddObject(addedCategory);
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return true;
        }

        [OperationContract]
        public bool UpdateShiftCategory(z_Shift_Category updatedCategory)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    z_Shift_Category current = context.z_Shift_Category.FirstOrDefault(c => c.shift_category_id == updatedCategory.shift_category_id);
                    if (current != null)
                    {
                        current.shift_category_name = updatedCategory.shift_category_name;
                        current.description = updatedCategory.description;
                        current.modified_user_id = updatedCategory.modified_user_id;
                        current.modified_datetime = updatedCategory.modified_datetime;
                        current.is_active = updatedCategory.is_active;
                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool DeleteShiftCategory(z_Shift_Category deletedCategory)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    z_Shift_Category current = context.z_Shift_Category.FirstOrDefault(c => c.shift_category_id == deletedCategory.shift_category_id);
                    if (current != null)
                    {
                        current.is_delete = deletedCategory.is_delete;
                        current.delete_datetime = deletedCategory.delete_datetime;
                        current.delete_user_id = deletedCategory.delete_user_id;
                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region z_ShiftWeek operations

        [OperationContract]
        public IEnumerable<z_ShiftWeek> GetShiftWeeks()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_ShiftWeek.ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public bool SaveShiftWeeksDetails(z_ShiftWeek addedShiftWeek)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    context.z_ShiftWeek.AddObject(addedShiftWeek);
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool UpdateShiftWeekDetails(z_ShiftWeek updatedShiftWeek)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    z_ShiftWeek current = context.z_ShiftWeek.FirstOrDefault(c => c.week_id == updatedShiftWeek.week_id);
                    if (current != null)
                    {
                        current.week_name = updatedShiftWeek.week_name;
                        current.week_description = updatedShiftWeek.week_description;
                        if (current.dtl_ShiftWeek.Count > 0)
                        {
                            foreach (dtl_ShiftWeek dayWeek in updatedShiftWeek.dtl_ShiftWeek)
                            {
                                dtl_ShiftWeek currentShift = context.dtl_ShiftWeek.FirstOrDefault(c => c.trans_id == dayWeek.trans_id);
                                if (currentShift != null)        // update existing day of week shift
                                {
                                    currentShift.shift_detail_id = dayWeek.shift_detail_id;
                                }
                                else                           // adding new day of week shift
                                {
                                    dtl_ShiftWeek addedDay = new dtl_ShiftWeek();
                                    addedDay.day_id = dayWeek.day_id;
                                    addedDay.week_id = current.week_id;
                                    addedDay.shift_detail_id = dayWeek.shift_detail_id;
                                    current.dtl_ShiftWeek.Add(addedDay);
                                }
                            }
                        }

                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;

                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool DeleteShiftWeekDetails(z_ShiftWeek deletedShiftWeek, bool isWeekRemove)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    z_ShiftWeek currentShiftWeek = context.z_ShiftWeek.FirstOrDefault(c => c.week_id == deletedShiftWeek.week_id);
                    if (currentShiftWeek != null)
                    {
                        foreach (dtl_ShiftWeek weekDay in deletedShiftWeek.dtl_ShiftWeek)
                        {
                            dtl_ShiftWeek currentWeekDay = currentShiftWeek.dtl_ShiftWeek.FirstOrDefault(c => c.trans_id == weekDay.trans_id);
                            context.dtl_ShiftWeek.DeleteObject(currentWeekDay);
                        }
                        if (isWeekRemove)
                            context.z_ShiftWeek.DeleteObject(currentShiftWeek);

                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {

            }

            return false;
        }

        #endregion

        #region z_ShiftDayRange operations

        [OperationContract]
        public IEnumerable<z_ShiftDayRange> GetDayRangeDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_ShiftDayRange.ToList();
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region z_ShiftType operations

        [OperationContract]
        IEnumerable<z_ShiftType> GetShiftTypes()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_ShiftType.ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return new List<z_ShiftType>();
        }

        #endregion

        #region z_AttendanceGroup operations

        [OperationContract]
        public IEnumerable<z_AttendanceGroup> GetAttendanceGroups()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_AttendanceGroup.Where(c => c.is_delete == false).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public bool SaveAttendanceGroup(z_AttendanceGroup updatingGroup)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    z_AttendanceGroup current = context.z_AttendanceGroup.FirstOrDefault(c => c.attendance_group_id == updatingGroup.attendance_group_id);
                    if (current == null)
                        context.z_AttendanceGroup.AddObject(updatingGroup);
                    else
                    {
                        current.attendance_group_name = updatingGroup.attendance_group_name;
                        current.is_roster_group = updatingGroup.is_roster_group;
                        current.is_shift_group = updatingGroup.is_shift_group;
                        current.is_active = updatingGroup.is_active;
                        current.modified_user_id = updatingGroup.modified_user_id;
                        current.modified_datetime = updatingGroup.modified_datetime;
                    }
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool DeleteAttendanceGroup(z_AttendanceGroup deletingGroup)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    z_AttendanceGroup current = context.z_AttendanceGroup.FirstOrDefault(c => c.attendance_group_id == deletingGroup.attendance_group_id);
                    if (current != null)
                    {
                        current.is_delete = deletingGroup.is_delete;
                        current.delete_datetime = deletingGroup.delete_datetime;
                        current.delete_user_id = deletingGroup.delete_user_id;
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region z_HolidayType operations

        [OperationContract]
        public IEnumerable<z_HolidayType> GetHolidayTypesBasicDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_HolidayType.Where(c => c.is_delete == false).Select(c => new { holID = c.holiday_type_id, holType = c.holiday_type, IsActive = c.is_active }).ToList().Select(c => new z_HolidayType { holiday_type_id = c.holID, holiday_type = c.holType, is_active = c.IsActive });
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public bool SaveHolidayType(z_HolidayType addedHolidayType)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    if (addedHolidayType.holiday_type_id > 0)
                    {
                        // update existing holiday type
                        z_HolidayType current = context.z_HolidayType.FirstOrDefault(c => c.holiday_type_id == addedHolidayType.holiday_type_id);
                        current.holiday_type = addedHolidayType.holiday_type;
                        current.is_active = addedHolidayType.is_active;
                        current.modified_datetime = addedHolidayType.modified_datetime;
                        current.modified_user_id = addedHolidayType.modified_user_id;
                    }
                    else
                    {
                        // adding new holiday type
                        context.z_HolidayType.AddObject(addedHolidayType);
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool DeleteHolidayType(z_HolidayType deletedHolidayType)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var current = context.z_HolidayType.FirstOrDefault(c => c.holiday_type_id == deletedHolidayType.holiday_type_id);
                    current.is_delete = deletedHolidayType.is_delete;
                    current.delete_datetime = deletedHolidayType.delete_datetime;
                    current.delete_user_id = deletedHolidayType.delete_user_id;

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region z_HolidayData operations

        [OperationContract]
        public IEnumerable<z_HolidayData> GetHolidayBasicDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    //context.ContextOptions.ProxyCreationEnabled = false;
                    //var result = context.z_HolidayData.Include("trns_HolidayData").Where(c => c.is_delete == false).ToList();
                    var result = context.z_HolidayData.Where(c => c.is_delete == false).ToList();
                    result.ForEach(c => context.Detach(c));
                    foreach (var item in result)
                    {
                        var current = result.FirstOrDefault(c => c.holiday_id == item.holiday_id);
                        var holidayTypes = context.trns_HolidayData.Where(c => c.holiday_id == current.holiday_id);
                        foreach (var holType in holidayTypes)
                            current.trns_HolidayData.Add(holType);
                    }
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public bool SaveHolidayData(z_HolidayData addedHoliday)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    context.z_HolidayData.AddObject(addedHoliday);
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        bool SaveBulkHolidayData(List<z_HolidayData> addedHolidayList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (z_HolidayData holiday in addedHolidayList)
                    {
                        context.z_HolidayData.AddObject(holiday);
                    }
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool UpdateHolidayData(z_HolidayData updatedHoliday)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var current = context.z_HolidayData.FirstOrDefault(c => c.holiday_id == updatedHoliday.holiday_id);
                    current.holiday_name = updatedHoliday.holiday_name;
                    current.holiday_start = updatedHoliday.holiday_start;
                    current.holiday_end = updatedHoliday.holiday_end;
                    current.is_active = updatedHoliday.is_active;
                    current.modified_datetime = updatedHoliday.modified_datetime;
                    current.modified_user_id = updatedHoliday.modified_user_id;

                    if (current.trns_HolidayData != null)
                    {
                        var addedHolidayTypes = updatedHoliday.trns_HolidayData.Where(c => !current.trns_HolidayData.Any(d => d.holiday_type_id == c.holiday_type_id)).ToList();
                        var deletedHolidayTypes = current.trns_HolidayData.Where(c => !updatedHoliday.trns_HolidayData.Any(d => d.holiday_type_id == c.holiday_type_id)).ToList();
                        if (deletedHolidayTypes.Count() > 0)
                        {
                            List<trns_HolidayData> allHolidayTrnsList = current.trns_HolidayData.ToList();
                            foreach (var item in deletedHolidayTypes)
                            {
                                trns_HolidayData currentDelItem = current.trns_HolidayData.FirstOrDefault(c => c.holiday_trans_id == item.holiday_trans_id);
                                context.trns_HolidayData.DeleteObject(currentDelItem);
                            }
                        }

                        if (addedHolidayTypes.Count > 0)
                        {
                            foreach (var item in addedHolidayTypes)
                            {
                                current.trns_HolidayData.Add(item);
                            }
                        }
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool DeleteHolidayData(z_HolidayData deletedHoliday)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    z_HolidayData current = context.z_HolidayData.FirstOrDefault(c => c.holiday_id == deletedHoliday.holiday_id);
                    current.is_delete = deletedHoliday.is_delete;
                    current.delete_datetime = deletedHoliday.delete_datetime;
                    current.delete_user_id = deletedHoliday.delete_user_id;

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        [OperationContract]
        public IEnumerable<string> GetHolidayYears()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_HolidayData.Select(c => new { HolDate = c.holiday_start }).ToList().Select(c => c.HolDate.Value.Year.ToString()).Distinct();
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public IEnumerable<z_HolidayData> GetHolidaysBasicDetailsByYear(int year)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_HolidayData.Where(c => c.is_active == true && c.is_delete == false).Select(c => new { holID = c.holiday_id, holName = c.holiday_name, holStart = c.holiday_start, holEnd = c.holiday_end, IsActive = c.is_active }).ToList().Where(c => c.holStart.Value.Year == year).Select(c => new z_HolidayData { holiday_id = c.holID, holiday_name = c.holName, holiday_start = c.holStart, holiday_end = c.holEnd, is_active = c.IsActive });
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region trns_EmployeeDailyShiftDetails operations

        //[OperationContract]
        //public bool AssignShiftDetails(DateTime fromDate, DateTime toDate, List<WeekDayShift> weekDays, List<EmployeeSearchView> selectedEmployeeList)
        //{
        //    try
        //    {
        //        bool isErrors = false;
        //        List<trns_EmployeeDailyShiftDetails> addingList = new List<trns_EmployeeDailyShiftDetails>();
        //        List<trns_EmployeeDailyShiftDetails> dailyShiftList = new List<trns_EmployeeDailyShiftDetails>();
        //        //if (addingList.Count > 0)
        //        //    addingList.Clear();

        //        if (fromDate != null && toDate != null)
        //        {
        //            DateTime startDate = ((DateTime)fromDate).Date;
        //            DateTime endDate = ((DateTime)toDate).Date;
        //            DateTime current = startDate.Date;
        //            while (current.Date <= endDate.Date)
        //            {
        //                trns_EmployeeDailyShiftDetails addingShift = new trns_EmployeeDailyShiftDetails();
        //                addingShift.date = current;
        //                addingList.Add(addingShift);
        //                current = current.Date.AddDays(1);
        //            }

        //            DateTime selectedDate;
        //            if (addingList.Count > 0)
        //            {
        //                foreach (trns_EmployeeDailyShiftDetails selectDay in addingList)
        //                {
        //                    selectedDate = (DateTime)selectDay.date;
        //                    WeekDayShift assignedShift = weekDays.FirstOrDefault(c => c.day == selectedDate.DayOfWeek);
        //                    if (assignedShift.shift.shift_detail_id != 0)
        //                    {
        //                        addingList.FirstOrDefault(c => c.date == selectedDate.Date).shift_detail_id = assignedShift.shift.shift_detail_id;
        //                    }
        //                }

        //                if (selectedEmployeeList.Count > 0)
        //                {
        //                    foreach (EmployeeSearchView currentEmp in selectedEmployeeList)
        //                    {
        //                        foreach (trns_EmployeeDailyShiftDetails selectShift in addingList.Where(c => c.shift_detail_id != null))
        //                        {
        //                            trns_EmployeeDailyShiftDetails item = new trns_EmployeeDailyShiftDetails();
        //                            item.employee_id = currentEmp.employee_id;
        //                            item.shift_detail_id = selectShift.shift_detail_id;
        //                            item.date = selectShift.date;
        //                            dailyShiftList.Add(item);
        //                        }
        //                        if (dailyShiftList.Count > 0)
        //                        {
        //                            if (!SaveEmployeeDailyShiftDetails(dailyShiftList))
        //                                isErrors = true;
        //                            dailyShiftList.Clear();
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        if (isErrors)
        //            return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;

        //}

        [OperationContract]
        public bool SaveEmployeeDailyShiftDetails(List<trns_EmployeeDailyShiftDetails> addedList, bool isRemoveIfExists)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (trns_EmployeeDailyShiftDetails dailyShift in addedList)
                    {
                        trns_EmployeeDailyShiftDetails current = context.trns_EmployeeDailyShiftDetails.FirstOrDefault(c => c.employee_id == dailyShift.employee_id && c.shift_detail_id == dailyShift.shift_detail_id && c.date == dailyShift.date);
                        if (current == null)
                        {
                            context.trns_EmployeeDailyShiftDetails.AddObject(dailyShift);
                        }
                        else
                        {
                            current.shift_detail_id = dailyShift.shift_detail_id;
                            current.attendance_group_id = dailyShift.attendance_group_id;
                            current.trns_EmployeeShiftBreakStatus.is_free_break = dailyShift.trns_EmployeeShiftBreakStatus.is_free_break;
                            current.trns_EmployeeShiftBreakStatus.is_shift_break = dailyShift.trns_EmployeeShiftBreakStatus.is_shift_break;
                            current.trns_EmployeeShiftBreakStatus.is_no_break = dailyShift.trns_EmployeeShiftBreakStatus.is_no_break;
                        }
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public IEnumerable<trns_EmployeeDailyShiftDetails> GetEmployeeDailyShiftDetailsWithOT(List<Guid> employeeList, List<DateTime> filterDates)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.trns_EmployeeDailyShiftDetails.Where(c => employeeList.Contains(c.employee_id) && filterDates.Contains(c.date)).ToList();
                    result.ForEach(c => context.Detach(c));
                    foreach (var item in result)
                    {
                        var current = result.FirstOrDefault(c => c.trns_id == item.trns_id);
                        var maxOtData = context.trns_EmployeeMaxOTDetails.Where(c => c.shift_assigned_id == current.trns_id);
                        foreach (var maxOt in maxOtData)
                            current.trns_EmployeeMaxOTDetails = new trns_EmployeeMaxOTDetails { shift_assigned_id = maxOt.shift_assigned_id, morning_ot_limit = maxOt.morning_ot_limit, evening_ot_limit = maxOt.evening_ot_limit };
                    }
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        bool DeleteAssignedShift(trns_EmployeeDailyShiftDetails deletedShift)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var current = context.trns_EmployeeDailyShiftDetails.FirstOrDefault(c => c.trns_id == deletedShift.trns_id);
                    if (current != null)
                        context.trns_EmployeeDailyShiftDetails.DeleteObject(current);

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        #endregion

        #region trns_EmployeeShiftBreakStatus operations

        [OperationContract]
        bool UpdateShiftBreakOptions(List<trns_EmployeeShiftBreakStatus> updatedList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (trns_EmployeeShiftBreakStatus updBreak in updatedList)
                    {
                        var current = context.trns_EmployeeShiftBreakStatus.FirstOrDefault(c => c.trns_id == updBreak.trns_id);
                        current.is_free_break = updBreak.is_free_break;
                        current.is_no_break = updBreak.is_no_break;
                        current.is_shift_break = current.is_shift_break;
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region trns_ProcessedEmployeeAttendance operations

        bool SaveEmployeeAttendanceDetails(List<trns_ProcessedEmployeeAttendance> attendDataList)
        {
            try
            {
                bool isOnceSaveFailed = false;

                using (var context = new AttendanceEntities())
                {
                    var employeeIDList = attendDataList.Select(c => new { EmpID = c.employee_id }).Distinct();
                    foreach (var emp in employeeIDList)
                    {
                        bool isAttendanceExists = false;
                        DateTime empAttendStart = (DateTime)attendDataList.Where(c => c.employee_id == emp.EmpID).OrderBy(c => c.attend_date).FirstOrDefault().attend_date;
                        DateTime empAttendEnd = (DateTime)attendDataList.Where(c => c.employee_id == emp.EmpID).OrderByDescending(c => c.attend_date).FirstOrDefault().attend_date;

                        // Get previous attendance process data for current employee if exists
                        List<trns_ProcessedEmployeeAttendance> processedAttendanceList = context.trns_ProcessedEmployeeAttendance.Where(c => c.employee_id == emp.EmpID && c.attend_date >= empAttendStart.Date && c.attend_date <= empAttendEnd.Date).ToList();
                        if (processedAttendanceList != null && processedAttendanceList.Count > 0)
                        {
                            // delete previous attendance process data for current employee for selected processing time period
                            processedAttendanceList.ForEach(c => context.trns_ProcessedEmployeeAttendance.DeleteObject(c));
                            isAttendanceExists = true;
                        }

                        List<trns_ProcessedEmployeeAttendance> currentEmployeeAttendance = attendDataList.Where(c => c.employee_id == emp.EmpID).ToList();
                        if (isAttendanceExists)
                        {
                            if (validateSaveUpdate(context.SaveChanges()))
                            {
                                // delete previous process data first and then add new processed data for current employee to database
                                currentEmployeeAttendance.ForEach(c => context.trns_ProcessedEmployeeAttendance.AddObject(c));
                                if (!validateSaveUpdate(context.SaveChanges()))
                                    isOnceSaveFailed = true;    // save failed for current employee, but process is continues for remaining employees
                            }
                            else
                            {
                                isOnceSaveFailed = true;    // save failed for current employee, but process is continues for remaining employees
                            }
                        }
                        else
                        {
                            // error capture code for debugging purposes
                            var errorList = attendDataList.Where(c => c.attend_date == null || c.ot_in_time == null || c.ot_out_time == null || c.in_time == null || c.out_time == null);

                            // No previous attendance process data for current employee and so add new processed attendance data to database
                            currentEmployeeAttendance.ForEach(c => context.trns_ProcessedEmployeeAttendance.AddObject(c));
                            if (!validateSaveUpdate(context.SaveChanges()))
                                isOnceSaveFailed = true;    // save failed for current employee, but process is continues for remaining employees
                        }
                    }

                    if (isOnceSaveFailed)
                        return false;       // saving data failed for at least one employee but also remaining employees' processed data might be saved successfully.
                    else
                        return true;        // saving data for all employees is successfully completed

                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        #endregion

        #region trns_EmployeeHoliday operations

        [OperationContract]
        public IEnumerable<mas_Employee> GetEmployeeHolidayDetails(int grpID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    List<Guid> resignedEmpList = GetResignedEmployees();
                    var result = context.mas_Employee.Where(c => !resignedEmpList.Contains(c.employee_id) && c.dtl_AttendanceGroup.attendance_group_id == grpID && c.isdelete == false).Select(c =>
                         new
                         {
                             grpID = c.dtl_AttendanceGroup.attendance_group_id,
                             EmpID = c.employee_id,
                             fName = c.first_name,
                             sName = c.second_name,
                             empNo = c.emp_id,
                             isActive = c.dtl_AttendanceGroup.is_active

                         }).ToList().Select(c => new mas_Employee { employee_id = c.EmpID, emp_id = c.empNo, first_name = c.fName, second_name = c.sName, isdelete = c.isActive, dtl_AttendanceGroup = new dtl_AttendanceGroup { attendance_group_id = grpID } }).ToList();

                    foreach (var item in result)
                    {
                        var current = result.FirstOrDefault(c => c.employee_id == item.employee_id);
                        var holidays = context.z_HolidayData.Where(c => c.mas_Employee.Any(d => d.employee_id == current.employee_id)).ToList();
                        holidays.ForEach(c => context.Detach(c));
                        foreach (var holiday in holidays)
                        {
                            current.z_HolidayData.Add(holiday);
                        }

                    }
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public IEnumerable<mas_Employee> GetEmployeeHolidayDetailsByEmployee(List<Guid> empIDList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.mas_Employee.Where(c => empIDList.Contains(c.employee_id) && c.isdelete == false).Select(c =>
                        new
                        {
                            grpID = c.dtl_AttendanceGroup.attendance_group_id,
                            EmpID = c.employee_id,
                            fName = c.first_name,
                            sName = c.second_name,
                            empNo = c.emp_id,
                            isActive = c.isdelete
                        }).ToList().Select(c => new mas_Employee { employee_id = c.EmpID, emp_id = c.empNo, first_name = c.fName, second_name = c.sName, isdelete = c.isActive, dtl_AttendanceGroup = new dtl_AttendanceGroup { attendance_group_id = c.grpID } }).ToList();

                    foreach (var item in result)
                    {
                        var current = result.FirstOrDefault(c => c.employee_id == item.employee_id);
                        var holidays = context.z_HolidayData.Where(c => c.mas_Employee.Any(d => d.employee_id == current.employee_id)).ToList();
                        holidays.ForEach(c => context.Detach(c));
                        foreach (var holiday in holidays)
                        {
                            current.z_HolidayData.Add(holiday);
                        }
                    }

                    return result;
                }
            }
            catch (Exception)
            {
            }
            return new List<mas_Employee>();
        }

        [OperationContract]
        public bool AssignEmployeeHolidayDetails(IEnumerable<mas_Employee> assignedList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (var assignEmp in assignedList)
                    {
                        var currentEmp = context.mas_Employee.FirstOrDefault(c => c.employee_id == assignEmp.employee_id);
                        foreach (var assignHoliday in assignEmp.z_HolidayData)
                        {
                            var currentHoliday = context.z_HolidayData.FirstOrDefault(c => c.holiday_id == assignHoliday.holiday_id);
                            currentEmp.z_HolidayData.Add(currentHoliday);
                        }
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        public bool RemoveEmployeeHoliday(IEnumerable<mas_Employee> removeList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (var removeEmp in removeList)
                    {
                        var currentEmp = context.mas_Employee.FirstOrDefault(c => c.employee_id == removeEmp.employee_id);
                        foreach (var removeHoliday in removeEmp.z_HolidayData)
                        {
                            z_HolidayData currentHoliday = context.z_HolidayData.FirstOrDefault(c => c.holiday_id == removeHoliday.holiday_id);
                            currentEmp.z_HolidayData.Remove(currentHoliday);
                        }
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #region New Code For Delete Employee Holiday

        [OperationContract]
        public bool RemoveIndividualEmployeeHoliday(Guid EmployeeID, int HolidayID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var currentEmp = context.mas_Employee.FirstOrDefault(c => c.employee_id == EmployeeID);
                    foreach (var asignHoliday in currentEmp.z_HolidayData.Where(c => c.holiday_id == HolidayID).ToList())
                    {
                        var SelectedHoliday = context.z_HolidayData.FirstOrDefault(c => c.holiday_id == asignHoliday.holiday_id);
                        currentEmp.z_HolidayData.Remove(SelectedHoliday);
                    }
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region trns_EmployeeMaxOTDetails operations

        [OperationContract]
        bool SaveEmployeeMaxOtDetails(List<trns_EmployeeMaxOTDetails> addedList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (trns_EmployeeMaxOTDetails item in addedList)
                    {
                        var current = context.trns_EmployeeMaxOTDetails.FirstOrDefault(c => c.shift_assigned_id == item.shift_assigned_id);
                        if (current == null)     // new max ot save
                            context.trns_EmployeeMaxOTDetails.AddObject(item);
                        else
                        {
                            current.morning_ot_limit = item.morning_ot_limit;
                            current.evening_ot_limit = item.evening_ot_limit;
                        }
                    }
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        bool DeleteEmployeeMaxOtDetails(List<trns_EmployeeMaxOTDetails> deletedList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (var item in deletedList)
                    {
                        var current = context.trns_EmployeeMaxOTDetails.FirstOrDefault(c => c.shift_assigned_id == item.shift_assigned_id);
                        context.trns_EmployeeMaxOTDetails.DeleteObject(current);
                    }
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region trns_EmployeePeriodQunatity operations

        bool SaveEmployeePeriodQuantityDetailsFull(List<trns_EmployeePeriodQunatity> addedPeriodQtyList, List<dtl_EmployeeRule> bonusDeductionRuleList, Guid periodID, bool Partial, bool Full)
        {
            try
            {
                bool isOnceSaveFailed = false;
                string st = null;
                if (Partial == true)
                    st = "P";
                else if (Full == true)
                    st = "F";
                using (var context = new AttendanceEntities())
                {
                    List<Guid> empIDList = addedPeriodQtyList.Select(c => c.employee_id).Distinct().ToList();
                    foreach (Guid empID in empIDList)
                    {
                        bool isRuleQuantityExists = false;
                        List<Guid> empAddingRules = addedPeriodQtyList.Where(c => c.employee_id == empID).Select(c => c.rule_id).ToList();
                        List<trns_EmployeePeriodQunatity> currentEmployeeProcessedRules = context.trns_EmployeePeriodQunatity.Where(c => c.period_id == periodID && c.employee_id == empID && empAddingRules.Contains(c.rule_id) && c.status == st).ToList();
                        if (currentEmployeeProcessedRules != null && currentEmployeeProcessedRules.Count > 0)
                        {
                            currentEmployeeProcessedRules.ForEach(c => context.trns_EmployeePeriodQunatity.DeleteObject(c));
                            isRuleQuantityExists = true;
                        }

                        List<trns_EmployeePeriodQunatity> currentEmployeeAddingRules = addedPeriodQtyList.Where(c => c.employee_id == empID).ToList();
                        if (isRuleQuantityExists)
                        {
                            if (validateSaveUpdate(context.SaveChanges()))
                            {
                                currentEmployeeAddingRules.ForEach(c => context.trns_EmployeePeriodQunatity.AddObject(c));
                                if (validateSaveUpdate(context.SaveChanges()))
                                {
                                    List<dtl_EmployeeRule> bonusDeductUpdateList = bonusDeductionRuleList.Where(c => c.employee_id == empID).ToList();
                                    if (bonusDeductUpdateList.Count > 0)
                                    {
                                        bool isRuleUpdated = UpdateAttendanceBonusDeductionAmount(bonusDeductUpdateList);
                                        if (!isRuleUpdated)
                                            isOnceSaveFailed = true;
                                    }
                                }
                                else
                                {
                                    isOnceSaveFailed = true;
                                }
                            }
                            else
                            {
                                isOnceSaveFailed = true;
                            }
                        }
                        else
                        {
                            currentEmployeeAddingRules.ForEach(c => context.trns_EmployeePeriodQunatity.AddObject(c));
                            if (validateSaveUpdate(context.SaveChanges()))
                            {
                                List<dtl_EmployeeRule> bonusDeductUpdateList = bonusDeductionRuleList.Where(c => c.employee_id == empID).ToList();
                                if (bonusDeductUpdateList.Count > 0)
                                {
                                    bool isRuleUpdated = UpdateAttendanceBonusDeductionAmount(bonusDeductUpdateList);
                                    if (!isRuleUpdated)
                                        isOnceSaveFailed = true;
                                }
                            }
                            else
                            {
                                isOnceSaveFailed = true;
                            }
                        }
                    }

                    if (isOnceSaveFailed)
                        return false;

                    return true;

                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region trns_EmployeeAttendanceSumarry

        bool SaveEmployeeAttendanceSummaryDetails(List<trns_EmployeeAttendanceSumarry> addedAttendSummaryList)
        {
            try
            {
                bool isOnceSaveFailed = false;
                List<trns_EmployeeAttendanceSumarry> existingAttendSummaryList = new List<trns_EmployeeAttendanceSumarry>();
                using (var context = new AttendanceEntities())
                {
                    foreach (var item in addedAttendSummaryList)
                    {
                        var current = context.trns_EmployeeAttendanceSumarry.FirstOrDefault(c => c.employee_id == item.employee_id && c.period_id == item.period_id);
                        if (current != null)
                            existingAttendSummaryList.Add(current);
                    }

                    if (existingAttendSummaryList.Count > 0)
                    {
                        // existing attendance summary found
                        existingAttendSummaryList.ForEach(c => context.trns_EmployeeAttendanceSumarry.DeleteObject(c));
                        if (validateSaveUpdate(context.SaveChanges()))
                        {
                            // add newly processed attendance summary
                            addedAttendSummaryList.ForEach(c => context.trns_EmployeeAttendanceSumarry.AddObject(c));
                            if (!validateSaveUpdate(context.SaveChanges()))
                            {
                                isOnceSaveFailed = true;
                            }
                        }
                        else
                        {
                            isOnceSaveFailed = true;
                        }
                    }
                    else
                    {
                        // No existing attendance summary found
                        // add newly processed attendance summary
                        addedAttendSummaryList.ForEach(c => context.trns_EmployeeAttendanceSumarry.AddObject(c));
                        if (!validateSaveUpdate(context.SaveChanges()))
                        {
                            isOnceSaveFailed = true;
                        }
                    }

                    if (isOnceSaveFailed)
                        return false;
                    else
                        return true;

                }
            }
            catch (Exception)
            {

            }

            return false;
        }

        #endregion

        #region trns_ShiftType operations

        //[OperationContract]
        //IEnumerable<dtl_Shift_Master> GetShiftsWithType()
        //{
        //    try
        //    {
        //        using (var context = new AttendanceEntities())
        //        {
        //            var result = context.dtl_Shift_Master.Include("trns_ShiftType").Include("trns_ShiftType.z_ShiftType").Where(c => c.is_delete == false).Select(
        //                c => new
        //                {
        //                    SHIFT_ID = c.shift_detail_id,
        //                    SHIFT_CAT_ID = c.z_Shift_Category.shift_category_id,
        //                    SHIFT_CAT_NAME = c.z_Shift_Category.shift_category_name,
        //                    SHIFT_NAME = c.shift_detail_name,
        //                    SHIFT_ORDER = c.dtl_ShiftOrder.order_number
        //                }
        //                ).ToList().Select(
        //                        c => new dtl_Shift_Master
        //                        {
        //                            shift_detail_id = c.SHIFT_ID,
        //                            shift_detail_name = c.SHIFT_NAME,
        //                            z_Shift_Category = new z_Shift_Category { shift_category_id = c.SHIFT_CAT_ID, shift_category_name = c.SHIFT_CAT_NAME },
        //                            dtl_ShiftOrder = new dtl_ShiftOrder { order_number = c.SHIFT_ORDER }
        //                        }
        //                ).ToList();

        //            if (result != null && result.Count() > 0)
        //            {
        //                foreach (dtl_Shift_Master item in result)
        //                {
        //                    var currentShift = result.FirstOrDefault(c => c.shift_detail_id == item.shift_detail_id);
        //                    var currentShiftType = context.trns_ShiftType.FirstOrDefault(c => c.shift_detail_id == item.shift_detail_id);
        //                    if (currentShiftType != null)
        //                    {

        //                        currentShift.trns_ShiftType = new trns_ShiftType();
        //                        currentShift.trns_ShiftType.shift_type_id = currentShiftType.shift_type_id;
        //                        currentShiftType.shift_detail_id = currentShiftType.shift_detail_id;
        //                        currentShift.trns_ShiftType.z_ShiftType = new z_ShiftType();
        //                        currentShift.trns_ShiftType.z_ShiftType.shift_type_id = currentShiftType.z_ShiftType.shift_type_id;
        //                        currentShift.trns_ShiftType.z_ShiftType.shift_type_name = currentShiftType.z_ShiftType.shift_type_name;
        //                    }
        //                    else
        //                    {
        //                        currentShift.trns_ShiftType = new trns_ShiftType();
        //                        currentShift.trns_ShiftType.z_ShiftType = new z_ShiftType();
        //                    }
        //                }
        //            }
        //            return result;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return new List<dtl_Shift_Master>();
        //}

        [OperationContract]
        bool UpdateShiftType(List<dtl_Shift_Master> updatedShiftList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (dtl_Shift_Master shiftItem in updatedShiftList)
                    {
                        var current = context.dtl_Shift_Master.FirstOrDefault(c => c.shift_detail_id == shiftItem.shift_detail_id);
                        if (current != null)
                        {
                            if (current.trns_ShiftType == null)
                                current.trns_ShiftType = new trns_ShiftType();
                            current.trns_ShiftType.shift_type_id = shiftItem.trns_ShiftType.shift_type_id;
                        }
                    }
                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        [OperationContract]
        bool DeleteShiftType(List<trns_ShiftType> deletedTypeList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (trns_ShiftType shiftType in deletedTypeList)
                    {
                        var current = context.trns_ShiftType.FirstOrDefault(c => c.shift_detail_id == shiftType.shift_detail_id);
                        if (current != null)
                            context.trns_ShiftType.DeleteObject(current);
                    }

                    if (validateSaveUpdate(context.SaveChanges()))
                        return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        #endregion

        #region Resigned-Employee Information

        List<Guid> GetResignedEmployees()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    List<Guid> employeeIDList = context.dtl_Employee.Where(c => c.isActive == false || c.isdelete == true).Select(c => c.employee_id).ToList();
                    return employeeIDList;
                }
            }
            catch (Exception)
            {
            }
            return new List<Guid>();
        }

        #endregion

        #endregion

        #region View Operations

        #region AttendanceGroupDetailsView view operations

        [OperationContract]
        public IEnumerable<AttendanceGroupDetailsView> GetAttendanceGroupsDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.AttendanceGroupDetailsViews.ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region AttendanceGroupWithEmployee view operations

        [OperationContract]
        public IEnumerable<AttendanceGroupWithEmployee> GetAttendanceGroupWithEmployeesDetails(int groupID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    List<Guid> resignedEmpList = GetResignedEmployees();
                    var result = context.AttendanceGroupWithEmployees.Where(c => !resignedEmpList.Contains(c.employee_id) && c.attendance_group_id == groupID && c.is_active == true).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public IEnumerable<AttendanceGroupWithEmployee> GetAttendanceGroupEmployeeIDs(int groupID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.AttendanceGroupWithEmployees.Where(c => c.attendance_group_id == groupID).Select(c => new { EmpID = c.employee_id, GrpName = c.attendance_group_name }).ToList().Select(c => new AttendanceGroupWithEmployee { employee_id = c.EmpID, attendance_group_name = c.GrpName });
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public IEnumerable<AttendanceGroupWithEmployee> GetAssignedGroupEmployees(List<Guid> empList, int grpID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.AttendanceGroupWithEmployees.Where(c => c.attendance_group_id != grpID && empList.Contains(c.employee_id)).Select(c => new { EmpID = c.employee_id, GrpName = c.attendance_group_name }).ToList().Select(c => new AttendanceGroupWithEmployee { employee_id = c.EmpID, attendance_group_name = c.GrpName });
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region EmployeeShiftCalendarDetailView view operations

        [OperationContract]
        public IEnumerable<EmployeeShiftCalendarDetailView> GetEmployeeShiftCalendarDetails(DateTime startDate, DateTime endDate, Guid? employeeID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.EmployeeShiftCalendarDetailViews.Where(c => c.employee_id == employeeID && c.date >= startDate.Date && c.date <= endDate).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region AttendEmployeeSummaryView view operations

        [OperationContract]
        public IEnumerable<AttendEmployeeSummaryView> GetAttendanceEmployeeSummaryDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    List<Guid> resignedEmpList = GetResignedEmployees();
                    var result = context.AttendEmployeeSummaryViews.Where(c => !resignedEmpList.Contains(c.employee_id) && c.isActive == true && c.isdelete == false).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        #endregion

        #region AttendancePeriodView view operations

        [OperationContract]
        public IEnumerable<AttendancePeriodView> GetAttendancePeriodDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.AttendancePeriodViews.ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion

        #region ShiftMasterDetailsView view operations

        [OperationContract]
        public IEnumerable<ShiftMasterDetailsView> GetShiftMasterDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.ShiftMasterDetailsViews.ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region ShiftDetailAllView operations

        [OperationContract]
        public IEnumerable<ShiftDetailAllView> GetShiftAllDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.ShiftDetailAllViews.Where(c => c.is_delete == false).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region ShiftWeekWithDaysView operations

        [OperationContract]
        public IEnumerable<ShiftWeekWithDaysView> GetShiftWeekWithDaysDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.ShiftWeekWithDaysViews.ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        [OperationContract]
        public IEnumerable<ShiftWeekWithDaysView> GetShiftWeekWithDaysByWeek(int weekID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.ShiftWeekWithDaysViews.Where(c => c.week_id == weekID).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region EmployeeAssignedShiftsView view operations

        [OperationContract]
        public IEnumerable<EmployeeAssignedShiftsView> GetEmployeeAssignedShiftDetails(List<Guid> empIDList, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.EmployeeAssignedShiftsViews.Where(c => empIDList.Contains(c.employee_id) && c.date >= startDate.Date && c.date <= endDate.Date).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #endregion

        #region Employee Attendance Process

        #region Employee Attendance Data Members


        #endregion

        #region Start of process Main Method

        // Attendance process starts here 
        [OperationContract]
        public bool BeginAttendanceProcess(List<AttendanceEmployeeDataView> employeeList, DateTime startDate, DateTime endDate, z_Period timePeriod, bool checkleiuleave)
        {
            /* parameter 1 - employee ID of type uniqueidentifier.
             * parameter 2 - first date of processing time period.
             * parameter 3 - last date of processing time period.
             */

            /*
             *  Each employee's attendance records are stored for each day.
             *  So attendance process to begin with selecting each employee and continue with remaining process.
             */
            StreamWriter shiftDataFile = null;
            try
            {
                #region Lists

                bool SelectedAttendance = false;
                List<dtl_AttendanceData> currentAttendanceDataList;
                List<dtl_EmployeeAttendanceData> currentSelectedAttendanceDataList;
                List<ShiftDetailAllView> currentShiftDataList;
                List<trns_EmployeeDailyShiftDetails> currentAttendanceCalendarDataList;
                List<dtl_AttendanceData> selectedEmployeeAttendanceDataList = new List<dtl_AttendanceData>();
                List<AttendEmployee> attendEmployeeList = new List<AttendEmployee>();
                List<trns_LeavePool> employeeLeaveList = new List<trns_LeavePool>();
                List<ShiftConfiguration> createdShiftConfigList = new List<ShiftConfiguration>();
                List<z_HolidayData> employeeHolidayList = new List<z_HolidayData>();
                List<z_LeaveType> leaveTypeList = new List<z_LeaveType>();
                List<dtl_Shift_Break_Details> shiftBreaks = new List<dtl_Shift_Break_Details>();
                List<dtl_Shift_Covering_Details> shiftLateCoveringList = new List<dtl_Shift_Covering_Details>();

                #endregion

                #region Variables

                //AttendEmployee attendEmployee;

                #endregion

                // selecting attendance data for selected date range
                List<string> empIDList = employeeList.Select(c => c.fingerprint_device_ID.ToString()).ToList();
                List<Guid> employeeIDs = employeeList.Select(c => c.employee_id).ToList();
                currentAttendanceDataList = GetCurrentAttendanceDetails(startDate.AddDays(-3), endDate.AddDays(3), empIDList);
                currentSelectedAttendanceDataList = GetCurrentSelectedAttendanceDetails(startDate, endDate, employeeIDs);
                // Instantiate Random Thumb-In/Thumb-Out Generating object
                Random rnd = new Random();

                if (currentAttendanceDataList != null || currentSelectedAttendanceDataList != null)
                {
                    DateTime sdate = startDate;  // (DateTime)timePeriod.start_date;
                    DateTime edate = endDate.AddDays(2);   // (DateTime)timePeriod.end_date;
                    currentShiftDataList = GetCurrentShiftDetails();
                    currentAttendanceCalendarDataList = GetEmployeeAttendanceCalendarData(sdate, edate, employeeIDs);
                    shiftBreaks = GetShiftBreakDetails(currentShiftDataList.Select(c => c.shift_detail_id).ToList());
                    employeeLeaveList = GetEmployeeApprovedLeaveDetails(sdate, edate, employeeIDs);
                    employeeHolidayList = GetCurrentHolidayDetails(sdate, edate);
                    leaveTypeList = GetLeaveTypeDetails();
                    shiftLateCoveringList = GetShiftLateCoveringDetails(currentShiftDataList.Select(c => c.shift_detail_id).ToList());

                    //h 2019-06-24
                    List<EmployeeAttendanceData> empLateSLDetailList = new List<EmployeeAttendanceData>();

                    for (int i = 1; edate.Date >= sdate; i++)
                    {

                        foreach (AttendanceEmployeeDataView selectedEmployee in employeeList)
                        {
                            List<trns_EmployeeDailyShiftDetails> employeeCalendarDetails = currentAttendanceCalendarDataList.Where(c => c.employee_id == selectedEmployee.employee_id && c.date == sdate.Date).ToList();
                            if (employeeCalendarDetails.Count > 0)
                            {
                                foreach (trns_EmployeeDailyShiftDetails empCalendarShift in employeeCalendarDetails)
                                {
                                    AttendEmployee attendEmployee = new AttendEmployee();
                                    attendEmployee.EmployeeID = selectedEmployee.employee_id;
                                    attendEmployee.EmpID = selectedEmployee.fingerprint_device_ID.ToString();
                                    attendEmployee.FirstName = selectedEmployee.first_name;
                                    attendEmployee.SecondName = selectedEmployee.second_name;

                                    ShiftConfiguration workingShift = new ShiftConfiguration();
                                    workingShift.EmployeeShift = currentShiftDataList.FirstOrDefault(c => c.shift_detail_id == empCalendarShift.shift_detail_id);
                                    workingShift.ShiftBreaksList = shiftBreaks.Where(c => c.shift_detail_id == empCalendarShift.shift_detail_id).ToList();
                                    workingShift.ShiftLateCoverDetail = shiftLateCoveringList.FirstOrDefault(c => c.shift_detail_id == empCalendarShift.shift_detail_id);
                                    workingShift.CurrentDate = empCalendarShift.date;
                                    attendEmployee.WorkShift = workingShift;
                                    if (currentSelectedAttendanceDataList.Count > 0)
                                    {
                                        attendEmployee.EmployeeSelectedAttendance = currentSelectedAttendanceDataList.Where(c => c.date == empCalendarShift.date && c.employee_id == selectedEmployee.employee_id).ToList();
                                        if (attendEmployee.EmployeeSelectedAttendance.Count() > 0 && (attendEmployee.EmployeeSelectedAttendance.FirstOrDefault().in_time != null || attendEmployee.EmployeeSelectedAttendance.FirstOrDefault() != null))
                                        {
                                            SelectedAttendance = true;
                                        }
                                        else
                                        {
                                            attendEmployee.EmployeeAttendance = currentAttendanceDataList.Where(c => c.attend_datetime >= attendEmployee.WorkShift.ShiftOn && c.attend_datetime <= attendEmployee.WorkShift.ShiftOff && c.emp_id == selectedEmployee.emp_id).OrderBy(c => c.attend_datetime).ToList();
                                            SelectedAttendance = false;
                                        }
                                    }
                                    else
                                    {
                                        attendEmployee.EmployeeAttendance = currentAttendanceDataList.Where(c => c.attend_datetime >= attendEmployee.WorkShift.ShiftOn && c.attend_datetime <= attendEmployee.WorkShift.ShiftOff && c.emp_id == selectedEmployee.emp_id).OrderBy(c => c.attend_datetime).ToList();
                                        SelectedAttendance = false;
                                    }
                                    attendEmployee.AvailableLeaveTypes = leaveTypeList;
                                    attendEmployee.IsShiftAssigned = true;
                                    attendEmployee.HasShiftBreak = empCalendarShift.trns_EmployeeShiftBreakStatus.is_shift_break;
                                    attendEmployee.HasFreeBreak = empCalendarShift.trns_EmployeeShiftBreakStatus.is_free_break;
                                    attendEmployee.HasNoBreak = empCalendarShift.trns_EmployeeShiftBreakStatus.is_no_break;

                                    // Approved leaves of employee for the current assigned shift
                                    attendEmployee.ApproveLeave = employeeLeaveList.Where(c => c.emp_id == attendEmployee.EmployeeID && c.shift_detail_id == attendEmployee.WorkShift.EmployeeShift.shift_detail_id && c.leave_date.Value.Date == sdate.Date).ToList();

                                    // employee's assigned holidays though he has been assigned a shift. Holidays are filtered out according to Shift On and Shift Off
                                    attendEmployee.AssignedHolidays = employeeHolidayList.Where(c => c.mas_Employee.Any(d => d.employee_id == selectedEmployee.employee_id) && c.holiday_start <= attendEmployee.WorkShift.ShiftOn && c.holiday_end >= attendEmployee.WorkShift.ShiftOff).ToList();
                                    attendEmployee.SetEmployeeAttendanceStatus(empCalendarShift, SelectedAttendance);       // Set basic attended employee status

                                    // Get Max-OT assigned for employee
                                    attendEmployee.MaxOt = empCalendarShift.trns_EmployeeMaxOTDetails;
                                    attendEmployee.RndTimeGenerator = rnd;

                                    //h 2019-06-24
                                    EmployeeAttendanceData empLateSLDetail = empLateSLDetailList.FirstOrDefault(c => c.Employee_id == selectedEmployee.employee_id);
                                    if (empLateSLDetail != null)
                                    {
                                        attendEmployee.ShortLeaveCount = empLateSLDetail.SLCount;
                                        attendEmployee.LateCount = empLateSLDetail.LateCount;
                                    }
                                    // Actual work done by employee process begins
                                    attendEmployee.BeginWorkCalculation();

                                    // Holidays assigned for employee calculation begins
                                    attendEmployee.BeginHolidayCalculation(sdate);

                                    // Actual leaves obtained by employee calculation begins
                                    attendEmployee.BeginLeaveCalculation();
                                    attendEmployee.SetAllAttendanceStatus(empCalendarShift);
                                    attendEmployeeList.Add(attendEmployee);

                                    // Actual break-times obtained by employee calculation begins -- break-time duration could be affected on overtime, total work time
                                    attendEmployee.BeginEmployeeBreakCalculation();

                                    //h 2019-06-24
                                    //attendEmployee.LateSLCalculation();
                                    if (empLateSLDetail == null)
                                    {
                                        EmployeeAttendanceData newEmp = new EmployeeAttendanceData(selectedEmployee.employee_id);
                                        newEmp.LateCount = attendEmployee.LateCount;
                                        newEmp.SLCount = attendEmployee.ShortLeaveCount;
                                        empLateSLDetailList.Add(newEmp);
                                    }
                                    else
                                    {
                                        empLateSLDetail.LateCount = attendEmployee.LateCount;
                                        empLateSLDetail.SLCount = attendEmployee.ShortLeaveCount;
                                        //empLateSLDetailList.Add(empLateSLDetail);
                                    }
                                }
                            }
                            else      // Employee has not been assigned a shift for current processing date (holiday/Sunday/Error)
                            {
                                /*
                                 * if user imply that reason as holiday
                                 * 
                                 */
                                AttendEmployee attendEmployee = new AttendEmployee();
                                attendEmployee.EmployeeID = selectedEmployee.employee_id;
                                attendEmployee.EmpID = selectedEmployee.fingerprint_device_ID.ToString();
                                attendEmployee.FirstName = selectedEmployee.first_name;
                                attendEmployee.SecondName = selectedEmployee.second_name;
                                attendEmployee.IsShiftAssigned = false;
                                attendEmployee.AssignedHolidays = employeeHolidayList.Where(c => c.mas_Employee.Any(d => d.employee_id == selectedEmployee.employee_id) && c.holiday_start.Value.Date <= sdate.Date && c.holiday_end.Value.Date >= sdate.Date).ToList();
                                attendEmployee.BeginHolidayCalculation(sdate);
                                attendEmployeeList.Add(attendEmployee);
                            }
                        }

                        sdate = startDate.AddDays(i).Date;
                    }

                    //probation leave generate
                    if (employeeList.Count() > 0)
                    {
                        this.GenerateProbationLeave(employeeList, timePeriod);

                    }

                    if (attendEmployeeList.Count > 0)
                    {
                        List<trns_ProcessedEmployeeAttendance> attendanceList = SetAttendanceData(attendEmployeeList, timePeriod.period_id, checkleiuleave);
                        if (attendanceList.Count > 0)
                        {
                            if (SaveEmployeeAttendanceDetails(attendanceList))
                            {
                                if (BeginAttendanceSummaryProcess(attendEmployeeList.Select(c => c.EmployeeID).Distinct().ToList(), timePeriod))
                                {
                                    return true;
                                }
                                //return true;
                            }
                            else
                            {
                                // part of employees could be saved successfully. So attendance summary should be performed for them.
                                bool isSummarySuccess = BeginAttendanceSummaryProcess(attendEmployeeList.Select(c => c.EmployeeID).Distinct().ToList(), timePeriod);
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (shiftDataFile != null)
                {
                    shiftDataFile.Close();
                }
            }
            return false;
        }

        #endregion


        #region Data Retrieval Methods

        #region Attendance Data

        List<dtl_AttendanceData> GetCurrentAttendanceDetails(DateTime startDate, DateTime endDate, List<string> selectedEmpList)
        {
            /* 
             * parameter 1 - start date of the date range in request
             * parameter 2 - end date of the date range in request
             */

            /* 
             * Employee attendance data are stored in system friendly format after downloading from device.
             * These records are retrieved for given date range on all employees from attendance process.
             */

            try
            {
                using (var context = new AttendanceEntities())
                {
                    //var result = context.dtl_AttendanceData.Where(c => c.attend_date >= startDate.Date && c.attend_date <= endDate.Date && selectedEmpList.Contains(c.emp_id)).ToList();
                    var result = (context.dtl_AttendanceData.Where(c => c.attend_date >= startDate.Date && c.attend_date <= endDate.Date && selectedEmpList.Contains(c.emp_id) && (c.isdelete == false || c.isdelete == null)).GroupBy(x => new { x.attend_datetime, x.attend_date, x.emp_id }).Select(y => y.FirstOrDefault())).ToList(); ;
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        List<dtl_EmployeeAttendanceData> GetCurrentSelectedAttendanceDetails(DateTime startDate, DateTime endDate, List<Guid> selectedEmpList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_EmployeeAttendanceData.Where(c => c.date >= startDate.Date && c.date <= endDate.Date && selectedEmpList.Contains((Guid)c.employee_id)).ToList();
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region Holiday Data

        List<z_HolidayData> GetCurrentHolidayDetails(DateTime startDate, DateTime endDate)
        {
            /* 
             * parameter 1 - start date of the date range in request
             * parameter 2 - end date of the date range in request
             */

            /* Holidays are assigned to employees. Holidays are stored with start date/time and end date/time.
             * All holidays within given date range are retrieved.
             */

            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_HolidayData.Include("mas_Employee").Where(c => c.holiday_start >= startDate.Date && c.holiday_end <= endDate.Date && c.is_active == true && c.is_delete == false).ToList();
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region Late Covering Data

        List<dtl_Shift_Covering_Details> GetShiftLateCoveringDetails(List<int> shiftIDs)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_Shift_Covering_Details.Where(c => (c.is_delete == false || c.is_delete == null) && shiftIDs.Contains(c.shift_detail_id)).ToList();
                    return result;
                }
            }
            catch (Exception)
            {
            }

            return new List<dtl_Shift_Covering_Details>();
        }

        #endregion

        #region Shfit/Roster Data

        List<ShiftDetailAllView> GetCurrentShiftDetails()
        {
            /*
             * User configured shifts are stored in several database tables.
             * All shifts information are retrived.
             */

            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.ShiftDetailAllViews.ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        List<trns_EmployeeDailyShiftDetails> GetEmployeeAttendanceCalendarData(DateTime startDate, DateTime endDate, List<Guid> selectedEmps)
        {
            /*
             * Employees have been assigned shifts/rosters for particular time period.
             * These informations are retrieved for all employees
             * 
             */
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.trns_EmployeeDailyShiftDetails.Include("trns_EmployeeMaxOTDetails").Include("trns_EmployeeShiftBreakStatus").Where(c => c.date >= startDate.Date && c.date <= endDate.Date && selectedEmps.Contains((Guid)c.employee_id)).ToList();
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        #endregion

        #region Shift Break Data

        List<dtl_Shift_Break_Details> GetShiftBreakDetails(List<int> shiftIDs)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_Shift_Break_Details.Where(c => shiftIDs.Contains(c.shift_detail_id)).ToList();
                    return result;
                }
            }
            catch (Exception)
            {

            }
            return new List<dtl_Shift_Break_Details>();
        }
        #endregion

        #region Leave Data

        List<trns_LeavePool> GetEmployeeApprovedLeaveDetails(DateTime startDate, DateTime endDate, List<Guid> selectedEmpList)
        {
            /*
             *  Employee has apply for leaves and get approved by authorized personnel. These leaves can be filtered out by
             *  Leave Date from the table
             */

            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.trns_LeavePool.Include("z_LeaveType").Include("z_LeaveCategory").Where(c => c.is_approved == true && c.leave_date >= startDate.Date && c.leave_date <= endDate.Date && selectedEmpList.Contains((Guid)c.emp_id)).ToList();
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        List<z_LeaveType> GetLeaveTypeDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.z_LeaveType.ToList();
                    return result;
                }
            }
            catch (Exception)
            {
            }
            return new List<z_LeaveType>();
        }

        #endregion

        #region Kasun Leiu Executive Data

        [OperationContract]
        public IEnumerable<ExecutiveOptionView> GetExecutiveDetails()
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    var result = context.ExecutiveOptionViews.Where(c => c.is_expire == false && c.is_leiu_leave == false && c.is_pay == false).ToList();
                    result.ForEach(e => context.Detach(e));
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [OperationContract]
        public IEnumerable<AttendEmployeeSummaryView> GetExecutiveDetailsFirst()
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    var result = context.AttendEmployeeSummaryViews.Where(c => context.ExecutiveOptionViews.Count(d => c.employee_id == d.employee_id && d.is_expire == false && d.is_leiu_leave == false && d.is_pay == false) > 0).ToList();
                    result.ForEach(e => context.Detach(e));
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [OperationContract]
        public IEnumerable<EmployeeLeiuLeaveView> GetEmployeeLeiuLeaveDetails()
        {
            using (var contex = new AttendanceEntities())
            {
                try
                {
                    var result = contex.EmployeeLeiuLeaveViews.Where(c => c.is_use_status == false && c.is_expire == false && c.is_pay == false && c.is_leiu_leave == true).ToList();
                    result.ForEach(e => contex.Detach(e));
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [OperationContract]
        public bool UpdateExecutiveDetails(List<ExecutiveOptionView> UpdateObj, bool pay, DateTime DT, Guid UID)
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    foreach (var item in UpdateObj)
                    {
                        trns_ProcessedEmployeeLeiuleaveStatus result = context.trns_ProcessedEmployeeLeiuleaveStatus.FirstOrDefault(c => c.trns_attend_id == item.trns_attend_id);
                        if (pay == true)
                        {
                            result.is_pay = item.is_pay;
                            AddPermanentPay(item, DT, UID);
                        }
                        else
                        {
                            result.is_leiu_leave = item.is_leiu_leave;
                            AddPermanentLeiuLeave(item, DT, UID);
                        }
                    }
                    return validateSaveUpdate(context.SaveChanges());
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool AddPermanentPay(ExecutiveOptionView LeiuObj, DateTime DTN, Guid UIDN)
        {
            using (var context = new AttendanceEntities())
            {
                List<trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_> pay = new List<trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_>();
                trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_ addpay = new trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_();
                try
                {
                    var result = context.trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_.FirstOrDefault(c => c.employee_id == LeiuObj.employee_id && c.attend_date == LeiuObj.attend_date);
                    if (result == null)
                    {
                        addpay.employee_id = LeiuObj.employee_id;
                        addpay.attend_date = LeiuObj.attend_date;
                        addpay.expire_date = LeiuObj.expire_date;
                        addpay.amount = LeiuObj.amount;
                        addpay.is_use_status = false;
                        addpay.is_expire = false;
                        addpay.is_leiu_leave = false;
                        addpay.is_pay = true;
                        addpay.is_paid = false;
                        addpay.pay_period_id = LeiuObj.pay_period_id;
                        addpay.save_user_id = UIDN;
                        addpay.save_datetime = DTN;
                        pay.Add(addpay);
                    }
                    else
                    {
                        result.is_pay = true;
                        result.is_leiu_leave = false;
                        result.save_user_id = UIDN;
                        result.save_datetime = DTN;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                if (pay != null)
                    pay.ForEach(c => context.trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_.AddObject(c));
                return validateSaveUpdate(context.SaveChanges());
            }
        }

        public bool AddPermanentLeiuLeave(ExecutiveOptionView LeiuObj, DateTime DTN, Guid UIDN)
        {
            using (var context = new AttendanceEntities())
            {
                List<trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_> leiuleave = new List<trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_>();
                trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_ addleiuleave = new trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_();
                try
                {
                    var result = context.trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_.FirstOrDefault(c => c.employee_id == LeiuObj.employee_id && c.attend_date == LeiuObj.attend_date);
                    if (result == null)
                    {
                        addleiuleave.employee_id = LeiuObj.employee_id;
                        addleiuleave.attend_date = LeiuObj.attend_date;
                        addleiuleave.expire_date = LeiuObj.expire_date;
                        addleiuleave.amount = LeiuObj.amount;
                        addleiuleave.is_use_status = false;
                        addleiuleave.is_expire = false;
                        addleiuleave.is_leiu_leave = true;
                        addleiuleave.is_pay = false;
                        addleiuleave.is_paid = false;
                        addleiuleave.pay_period_id = LeiuObj.pay_period_id;
                        addleiuleave.save_user_id = UIDN;
                        addleiuleave.save_datetime = DTN;
                        leiuleave.Add(addleiuleave);
                    }
                    else
                    {
                        result.is_leiu_leave = true;
                        result.is_pay = false;
                        result.save_user_id = UIDN;
                        result.save_datetime = DTN;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                if (leiuleave != null)
                    leiuleave.ForEach(c => context.trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_.AddObject(c));
                return validateSaveUpdate(context.SaveChanges());
            }
        }

        [OperationContract]
        public bool UpdateEmployeeLeiuLeaveDetails(List<EmployeeLeiuLeaveView> UpdateObj)
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    foreach (var item in UpdateObj)
                    {
                        trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_ result = context.trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_.FirstOrDefault(c => c.leiu_pay_id == item.leiu_pay_id);
                        result.is_use_status = true;
                    }
                    return validateSaveUpdate(context.SaveChanges());
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        [OperationContract]
        public IEnumerable<PendingLeiuLeavesView> GetApprovedLeiuLeavePoolDataByDate(DateTime fromDate, DateTime toDate, Guid user_id)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.PendingLeiuLeavesViews.Where(c => c.is_approved == true && c.approved_user_id == user_id && c.leave_date >= fromDate && c.leave_date <= toDate && c.leave_category_id == new Guid("9B615C80-32D7-4951-BABC-04AD7193BC32")).ToList();
                    result.ForEach(d => context.Detach(d));
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [OperationContract]
        public bool ModifyApprovedLeiuLeaves(IEnumerable<PendingLeiuLeavesView> LeaveList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (var leave in LeaveList)
                    {
                        trns_LeavePool LeavepoolObj = context.trns_LeavePool.FirstOrDefault(c => c.pool_id == leave.pool_id && c.emp_id == leave.Employee_id);
                        LeavepoolObj.is_approved = false;
                        LeavepoolObj.is_rejected = true;
                        //var EmployeeLeiuLeaveObj = context.trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_.Where(c => c.employee_id == leave.Employee_id && c.attend_date == leave.used_leiu_leave_date);
                        //foreach (var leiuleave in EmployeeLeiuLeaveObj)
                        //{
                        //    leiuleave.is_use_status = false; 
                        //}
                    }

                    return validateSaveUpdate(context.SaveChanges());
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        [OperationContract]
        public IEnumerable<EmployeeLeiuLeaveView> GetExecutivePaymentDetails()
        {
            using (var contex = new AttendanceEntities())
            {
                try
                {
                    var result = contex.EmployeeLeiuLeaveViews.Where(c => c.is_pay == true && c.is_paid == false).ToList();
                    result.ForEach(e => contex.Detach(e));
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [OperationContract]
        public bool SaveLeiuLeavePayments(List<trns_LeiuLeavePayment> paymentlist)
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    foreach (var payment in paymentlist)
                    {
                        context.trns_LeiuLeavePayment.AddObject(payment);
                        trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_ res = context.trns_ProcessedEmployeeLeiuLeaveStatus_Permanent_.FirstOrDefault(c => c.employee_id == payment.employee_id && c.attend_date == payment.attend_date);
                        res.is_paid = true;
                    }
                    return validateSaveUpdate(context.SaveChanges());
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion

        #region Max OT Data

        List<trns_EmployeeMaxOTDetails> GetEmployeeMaxOT(List<trns_EmployeeDailyShiftDetails> assignedShiftList)
        {
            List<trns_EmployeeMaxOTDetails> otList = new List<trns_EmployeeMaxOTDetails>();
            try
            {
                using (var context = new AttendanceEntities())
                {

                }
            }
            catch (Exception)
            {
            }
            return otList;
        }

        #endregion

        #region Get Last IDs

        int GetNextProcessedAttendanceID()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var lastRecord = context.trns_ProcessedEmployeeAttendance.OrderByDescending(c => c.trns_attend_id).FirstOrDefault();
                    if (lastRecord != null)
                        return lastRecord.trns_attend_id;
                    else
                        return 1;
                }
            }
            catch (Exception)
            {

            }
            return -1;
        }

        #endregion

        #endregion

        #region Data Setting Methods

        List<trns_ProcessedEmployeeAttendance> SetAttendanceData(List<AttendEmployee> employeeList, Guid periodID, bool checkll)
        {
            try
            {
                List<trns_ProcessedEmployeeAttendance> addingList = new List<trns_ProcessedEmployeeAttendance>();
                foreach (AttendEmployee currentEmp in employeeList)
                {
                    if (currentEmp.IsAttendanceOk || currentEmp.IsAttendanceAbsent || currentEmp.IsHoliday || currentEmp.IsAttendanceInvalid)
                    {
                        trns_ProcessedEmployeeAttendance addedAttendance = new trns_ProcessedEmployeeAttendance();
                        addedAttendance.employee_id = currentEmp.EmployeeID;
                        addedAttendance.shift_detail_id = currentEmp.WorkShift.EmployeeShift.shift_detail_id;
                        addedAttendance.attend_date = (DateTime)currentEmp.WorkShift.CurrentDate;
                        addedAttendance.in_time = currentEmp.ThumbIn;
                        addedAttendance.out_time = currentEmp.ThumbOut;
                        addedAttendance.ot_in_time = currentEmp.WorkOT.WorkOtStart;
                        addedAttendance.ot_out_time = currentEmp.WorkOT.WorkOtEnd;
                        addedAttendance.grace_in_duration = currentEmp.WorkGraceInDuration;
                        addedAttendance.grace_out_duration = currentEmp.WorkGraceOutDuration;
                        addedAttendance.ealry_in_duration = currentEmp.WorkEarlyInDuration;
                        addedAttendance.late_in_duration = currentEmp.WorkLateInDuration;
                        addedAttendance.early_out_duration = currentEmp.WorkEarlyOutDuration;
                        addedAttendance.late_out_duration = currentEmp.WorkLateOutDuration;
                        addedAttendance.work_late_in_duration = currentEmp.TotalLateWorkDurationFromShiftIn;
                        addedAttendance.work_early_out_duration = currentEmp.TotalEarlyOutWorkDurationFromShiftOut;
                        addedAttendance.is_early_in = currentEmp.IsEarlyIn;
                        addedAttendance.is_late_in = currentEmp.IsLateIn;
                        addedAttendance.is_early_out = currentEmp.IsEarlyOut;
                        addedAttendance.is_late_out = currentEmp.IsLateOut;
                        addedAttendance.is_grace_in = currentEmp.IsGraceIn;
                        addedAttendance.is_grace_out = currentEmp.IsGraceOut;
                        addedAttendance.pre_single_ot_duration = currentEmp.WorkOT.WorkPreSingleOtDuration;
                        addedAttendance.pre_double_ot_duration = currentEmp.WorkOT.WorkPreDoubleOtDuration;
                        addedAttendance.pre_triple_ot_duration = currentEmp.WorkOT.WorkPreTripleOtDuration;
                        addedAttendance.post_single_ot_duration = currentEmp.WorkOT.WorkPostSingleOtDuration;
                        addedAttendance.post_double_ot_duration = currentEmp.WorkOT.WorkPostDoubleOtDuration;
                        addedAttendance.post_triple_ot_duration = currentEmp.WorkOT.WorkPostTripleOtDuration;
                        addedAttendance.total_work_duration = currentEmp.TotalWorkDuration;
                        addedAttendance.total_minimum_work_duration = currentEmp.TotalMinimumWorkDuration;
                        addedAttendance.actual_total_work_duration = currentEmp.ActualTotalWorkDuration;
                        // m 2021-06-16
                        addedAttendance.is_late_deduc = currentEmp.SpecialLateDeduction;
                        addedAttendance.is_mor_inc = currentEmp.MorningIncentive;

                        // logging employee obtained leaves to database
                        List<trns_ProcessedLeaveStatus> employeeLeaves = SetEmployeeLeaveData(currentEmp.CapturedLeaves.ToList());
                        if (employeeLeaves != null)
                            employeeLeaves.ForEach(c => addedAttendance.trns_ProcessedLeaveStatus.Add(c));

                        // logging employee's attendance statuses to database
                        if (currentEmp.AttendStatusList.Count > 0)
                            currentEmp.AttendStatusList.ForEach(c => addedAttendance.trns_ProcessedAttendanceStatus.Add(c));

                        // logging employee obtained holidays to database
                        List<trns_ProcessedEmployeeHolidayStatus> employeeHolidays = SetEmployeeHolidayData(currentEmp.CapturedHolidays);
                        if (employeeHolidays != null)
                            employeeHolidays.ForEach(c => addedAttendance.trns_ProcessedEmployeeHolidayStatus.Add(c));

                        // logging employee obtained break-times to database
                        if (currentEmp.CapturedBreaks.Count > 0)
                        {
                            List<trns_ProcessedEmployeeBreakStatus> employeeBreaks = SetEmployeeBreakData(currentEmp.CapturedBreaks);
                            if (employeeBreaks != null)
                                employeeBreaks.ForEach(c => addedAttendance.trns_ProcessedEmployeeBreakStatus.Add(c));
                        }

                        // logging employee's extra-ot to database
                        if (currentEmp.MaxOt != null)
                        {
                            addedAttendance.trns_ProcessedEmployeeMaxOT = new trns_ProcessedEmployeeMaxOT();

                            if (currentEmp.HasMorningMaxOt)
                                addedAttendance.trns_ProcessedEmployeeMaxOT.actual_in_time = currentEmp.ActualIn;
                            else
                                addedAttendance.trns_ProcessedEmployeeMaxOT.actual_in_time = currentEmp.ThumbIn;

                            if (currentEmp.HasEveningMaxOt)
                                addedAttendance.trns_ProcessedEmployeeMaxOT.actual_out_time = currentEmp.ActualOut;
                            else
                                addedAttendance.trns_ProcessedEmployeeMaxOT.actual_out_time = currentEmp.ThumbOut;

                            addedAttendance.trns_ProcessedEmployeeMaxOT.pre_extra_single_ot = currentEmp.WorkOT.PreSingleExtraOtDuration;
                            addedAttendance.trns_ProcessedEmployeeMaxOT.pre_extra_double_ot = currentEmp.WorkOT.PreDoubleExtraOtDuration;
                            addedAttendance.trns_ProcessedEmployeeMaxOT.pre_extra_triple_ot = currentEmp.WorkOT.PreTripleExtraOtDuration;
                            addedAttendance.trns_ProcessedEmployeeMaxOT.post_extra_single_ot = currentEmp.WorkOT.PostSingleExtraOtDuration;
                            addedAttendance.trns_ProcessedEmployeeMaxOT.post_extra_double_ot = currentEmp.WorkOT.PostDoubleExtraOtDuration;
                            addedAttendance.trns_ProcessedEmployeeMaxOT.post_extra_triple_ot = currentEmp.WorkOT.PostTripleExtraOtDuration;
                        }

                        #region MCN/Kasun Leiu leave

                        if (checkll == true)
                        {
                            List<trns_ProcessedEmployeeLeiuleaveStatus> employeeLeiuleave = new List<trns_ProcessedEmployeeLeiuleaveStatus>();
                            trns_ProcessedEmployeeLeiuleaveStatus addLeiuleave = new trns_ProcessedEmployeeLeiuleaveStatus();
                            //Saturday
                            if ((currentEmp.WorkShift.EmployeeShift.is_entitle_leiu_leave == true && currentEmp.WorkShift.EmployeeShift.is_executive == true) && addedAttendance.attend_date.Value.DayOfWeek == DayOfWeek.Saturday && TimeSpan.FromSeconds((double)addedAttendance.actual_total_work_duration).TotalMinutes >= 235)
                            {
                                addLeiuleave.employee_id = currentEmp.EmployeeID;
                                addLeiuleave.attend_date = (DateTime)currentEmp.WorkShift.CurrentDate;
                                addLeiuleave.expire_date = addLeiuleave.attend_date.Value.AddMonths(6);
                                addLeiuleave.amount = (decimal)0.5;
                                addLeiuleave.is_use_status = false;
                                addLeiuleave.is_expire = false;
                                addLeiuleave.is_leiu_leave = false;
                                addLeiuleave.is_pay = false;
                                addLeiuleave.is_paid = false;
                                addLeiuleave.pay_period_id = periodID;
                                employeeLeiuleave.Add(addLeiuleave);
                            }
                            else if ((currentEmp.WorkShift.EmployeeShift.is_entitle_leiu_leave == true && currentEmp.WorkShift.EmployeeShift.is_nonexecutive == true) && addedAttendance.attend_date.Value.DayOfWeek == DayOfWeek.Saturday && TimeSpan.FromSeconds((double)addedAttendance.actual_total_work_duration).TotalMinutes >= 1320)
                            {
                                addLeiuleave.employee_id = currentEmp.EmployeeID;
                                addLeiuleave.attend_date = (DateTime)currentEmp.WorkShift.CurrentDate;
                                addLeiuleave.expire_date = addLeiuleave.attend_date.Value.AddMonths(6);
                                addLeiuleave.amount = (decimal)1.5;
                                addLeiuleave.is_use_status = false;
                                addLeiuleave.is_expire = false;
                                addLeiuleave.is_leiu_leave = false;
                                addLeiuleave.is_pay = false;
                                addLeiuleave.is_paid = false;
                                addLeiuleave.pay_period_id = periodID;
                                employeeLeiuleave.Add(addLeiuleave);
                            }
                            else if ((currentEmp.WorkShift.EmployeeShift.is_entitle_leiu_leave == true && currentEmp.WorkShift.EmployeeShift.is_nonexecutive == true) && addedAttendance.attend_date.Value.DayOfWeek == DayOfWeek.Saturday && TimeSpan.FromSeconds((double)addedAttendance.actual_total_work_duration).TotalMinutes >= 205)
                            {
                                addLeiuleave.employee_id = currentEmp.EmployeeID;
                                addLeiuleave.attend_date = (DateTime)currentEmp.WorkShift.CurrentDate;
                                addLeiuleave.expire_date = addLeiuleave.attend_date.Value.AddMonths(6);
                                addLeiuleave.amount = (decimal)0.5;
                                addLeiuleave.is_use_status = false;
                                addLeiuleave.is_expire = false;
                                addLeiuleave.is_leiu_leave = false;
                                addLeiuleave.is_pay = false;
                                addLeiuleave.is_paid = false;
                                addLeiuleave.pay_period_id = periodID;
                                employeeLeiuleave.Add(addLeiuleave);
                            }

                            //Sunday
                            if ((currentEmp.WorkShift.EmployeeShift.is_entitle_leiu_leave == true && currentEmp.WorkShift.EmployeeShift.is_executive == true) && addedAttendance.attend_date.Value.DayOfWeek == DayOfWeek.Sunday && TimeSpan.FromSeconds((double)addedAttendance.actual_total_work_duration).TotalMinutes >= 415)
                            {
                                addLeiuleave.employee_id = currentEmp.EmployeeID;
                                addLeiuleave.attend_date = (DateTime)currentEmp.WorkShift.CurrentDate;
                                addLeiuleave.expire_date = addLeiuleave.attend_date.Value.AddMonths(6);
                                addLeiuleave.amount = (decimal)1.0;
                                addLeiuleave.is_use_status = false;
                                addLeiuleave.is_expire = false;
                                addLeiuleave.is_leiu_leave = false;
                                addLeiuleave.is_pay = false;
                                addLeiuleave.is_paid = false;
                                addLeiuleave.pay_period_id = periodID;
                                employeeLeiuleave.Add(addLeiuleave);
                            }
                            else if ((currentEmp.WorkShift.EmployeeShift.is_entitle_leiu_leave == true && currentEmp.WorkShift.EmployeeShift.is_nonexecutive == true) && addedAttendance.attend_date.Value.DayOfWeek == DayOfWeek.Sunday && TimeSpan.FromSeconds((double)addedAttendance.actual_total_work_duration).TotalMinutes >= 475)
                            {
                                addLeiuleave.employee_id = currentEmp.EmployeeID;
                                addLeiuleave.attend_date = (DateTime)currentEmp.WorkShift.CurrentDate;
                                addLeiuleave.expire_date = addLeiuleave.attend_date.Value.AddMonths(6);
                                addLeiuleave.amount = (decimal)1.0;
                                addLeiuleave.is_use_status = false;
                                addLeiuleave.is_expire = false;
                                addLeiuleave.is_leiu_leave = false;
                                addLeiuleave.is_pay = false;
                                addLeiuleave.is_paid = false;
                                addLeiuleave.pay_period_id = periodID;
                                employeeLeiuleave.Add(addLeiuleave);
                            }

                            //Security
                            if ((currentEmp.WorkShift.EmployeeShift.is_entitle_leiu_leave == true && currentEmp.WorkShift.EmployeeShift.is_security == true) && currentEmp.CapturedHolidays.Count == 1 && TimeSpan.FromSeconds((double)addedAttendance.actual_total_work_duration).TotalMinutes >= 475)
                            {
                                addLeiuleave.employee_id = currentEmp.EmployeeID;
                                addLeiuleave.attend_date = (DateTime)currentEmp.WorkShift.CurrentDate;
                                addLeiuleave.expire_date = addLeiuleave.attend_date.Value.AddMonths(6);
                                addLeiuleave.amount = (decimal)1.0;
                                addLeiuleave.is_use_status = false;
                                addLeiuleave.is_expire = false;
                                addLeiuleave.is_leiu_leave = false;
                                addLeiuleave.is_pay = false;
                                addLeiuleave.is_paid = false;
                                addLeiuleave.pay_period_id = periodID;
                                employeeLeiuleave.Add(addLeiuleave);
                            }
                            if (employeeLeiuleave != null)
                                employeeLeiuleave.ForEach(c => addedAttendance.trns_ProcessedEmployeeLeiuleaveStatus.Add(c));
                        }

                        #endregion

                        addingList.Add(addedAttendance);
                    }
                }

                return addingList.OrderBy(c => c.attend_date).ThenBy(c => c.employee_id).ToList();
            }
            catch (Exception)
            {

            }
            return null;
        }

        List<trns_ProcessedLeaveStatus> SetEmployeeLeaveData(List<EmployeeLeave> capturedLeaveList)
        {
            try
            {
                if (capturedLeaveList.Count > 0)
                {
                    int nextID = 1;
                    List<trns_ProcessedLeaveStatus> addingLeaveList = new List<trns_ProcessedLeaveStatus>();
                    foreach (EmployeeLeave empLeave in capturedLeaveList)
                    {
                        if (empLeave.HasCoveringLeaves)
                        {
                            foreach (trns_LeavePool coverLeave in empLeave.CoveringLeaves)
                            {
                                trns_ProcessedLeaveStatus addingLeave = new trns_ProcessedLeaveStatus();
                                addingLeave.trns_leave_id = nextID;
                                addingLeave.pool_id = coverLeave.pool_id;
                                addingLeave.leave_type_id = (Guid)coverLeave.leave_type_id;
                                addingLeave.is_approved = empLeave.IsAuthorized;
                                addingLeave.leave_status = true;
                                addingLeave.is_official_leave = coverLeave.z_LeaveCategory.is_official.Value;
                                addingLeaveList.Add(addingLeave);
                                nextID++;
                            }
                        }
                        else
                        {
                            trns_ProcessedLeaveStatus addingLeave = new trns_ProcessedLeaveStatus();
                            addingLeave.trns_leave_id = nextID;
                            if (empLeave.IsAuthorized)
                                addingLeave.pool_id = empLeave.ApplyLeave.pool_id;
                            addingLeave.leave_type_id = clsLeave.GetLeaveOption(empLeave.ObtainedLeaveType);
                            addingLeave.is_approved = empLeave.IsAuthorized;
                            addingLeave.leave_status = true;
                            addingLeave.is_official_leave = empLeave.IsOfficialLeave;
                            addingLeaveList.Add(addingLeave);
                            nextID++;
                        }
                    }
                    return addingLeaveList;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        List<trns_ProcessedEmployeeHolidayStatus> SetEmployeeHolidayData(List<EmployeeHoliday> capturedHolidayList)
        {
            try
            {
                if (capturedHolidayList.Count > 0)
                {
                    List<trns_ProcessedEmployeeHolidayStatus> addingHolidayList = new List<trns_ProcessedEmployeeHolidayStatus>();
                    foreach (EmployeeHoliday empHoliday in capturedHolidayList)
                    {
                        trns_ProcessedEmployeeHolidayStatus addingHoliday = new trns_ProcessedEmployeeHolidayStatus();
                        addingHoliday.holiday_id = empHoliday.AssignedHoliday.holiday_id;
                        addingHoliday.holiday_status = true;

                        addingHolidayList.Add(addingHoliday);
                    }
                    return addingHolidayList;
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        List<trns_ProcessedEmployeeBreakStatus> SetEmployeeBreakData(List<EmployeeBreak> capturedBreakList)
        {
            try
            {
                List<trns_ProcessedEmployeeBreakStatus> addingBreakList = new List<trns_ProcessedEmployeeBreakStatus>();
                int nextID = 1;
                foreach (EmployeeBreak empBreak in capturedBreakList)
                {
                    trns_ProcessedEmployeeBreakStatus addingBreak = new trns_ProcessedEmployeeBreakStatus();
                    addingBreak.trns_break_id = nextID;
                    addingBreak.break_in = empBreak.BreakIn;
                    addingBreak.break_out = empBreak.BreakOut;
                    addingBreak.break_duration = empBreak.BreakDuration;
                    addingBreak.is_free_break = empBreak.IsFreeBreak;
                    addingBreak.is_shift_break = empBreak.IsShiftBreak;
                    addingBreak.is_break_invalid = empBreak.IsBreakInvalid;
                    if (empBreak.EmpAssignedBreak != null)
                        addingBreak.break_id = empBreak.EmpAssignedBreak.ShiftBreakID;
                    addingBreakList.Add(addingBreak);

                    nextID++;
                }
                return addingBreakList;
            }
            catch (Exception)
            {
            }
            return null;
        }


        #endregion

        #endregion

        #region Employee Attendance Summary Process

        #region Start of process Main Method

        // Begin calculating attendance summary for given period
        bool BeginAttendanceSummaryProcess(List<Guid> empIDList, z_Period summaryPeriod)
        {
            try
            {
                List<AttendanceSummaryEmployee> empAttendanceSummaryList = new List<AttendanceSummaryEmployee>();
                using (var context = new AttendanceEntities())
                {
                    DateTime periodStartDate = summaryPeriod.start_date.Value;
                    DateTime periodEndDate = summaryPeriod.end_date.Value;

                    // Get already processed attendance for given employees for the given period's date range
                    IEnumerable<trns_ProcessedEmployeeAttendance> empProcessedAttendance = context.trns_ProcessedEmployeeAttendance.Where(c => empIDList.Contains(c.employee_id.Value) && c.attend_date >= periodStartDate.Date && c.attend_date <= periodEndDate.Date);
                    if (empProcessedAttendance != null)
                    {
                        foreach (Guid empID in empProcessedAttendance.Select(c => c.employee_id).Distinct())
                        {
                            AttendanceSummaryEmployee currentEmpSummary = new AttendanceSummaryEmployee();
                            currentEmpSummary.EmployeeID = empID;
                            currentEmpSummary.SummaryPeriod = summaryPeriod.period_id;
                            currentEmpSummary.ProcessedAttendance = empProcessedAttendance.Where(c => c.employee_id == empID).ToList();
                            currentEmpSummary.EmpAttendanceStatusList = empProcessedAttendance.Where(c => c.employee_id == empID).Where(c => c.trns_ProcessedAttendanceStatus.Count > 0).SelectMany(c => c.trns_ProcessedAttendanceStatus).ToList();
                            currentEmpSummary.EmpLeaveStatusList = empProcessedAttendance.Where(c => c.employee_id == empID).Where(c => c.trns_ProcessedLeaveStatus.Count > 0).SelectMany(c => c.trns_ProcessedLeaveStatus).ToList();
                            currentEmpSummary.EmpMaxOtList = empProcessedAttendance.Where(c => c.employee_id == empID).Where(c => c.trns_ProcessedEmployeeMaxOT != null).Select(c => c.trns_ProcessedEmployeeMaxOT).ToList();

                            currentEmpSummary.CalculateEmployeeAttendanceSummary();
                            empAttendanceSummaryList.Add(currentEmpSummary);

                        }

                        if (empAttendanceSummaryList.Count > 0)
                        {
                            List<trns_EmployeeAttendanceSumarry> addingSummaryList = SetEmployeeAttendanceSummaryDetails(empAttendanceSummaryList);
                            if (addingSummaryList.Count > 0)
                            {
                                if (SaveEmployeeAttendanceSummaryDetails(addingSummaryList))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        #endregion

        #region Data Setting Methods

        List<trns_EmployeeAttendanceSumarry> SetEmployeeAttendanceSummaryDetails(List<AttendanceSummaryEmployee> summaryEmpList)
        {
            try
            {
                List<trns_EmployeeAttendanceSumarry> processedAttendanceSummaryList = new List<trns_EmployeeAttendanceSumarry>();
                foreach (var summaryEmp in summaryEmpList)
                {
                    trns_EmployeeAttendanceSumarry addingSummary = new trns_EmployeeAttendanceSumarry();
                    addingSummary.employee_id = summaryEmp.EmployeeID;
                    addingSummary.period_id = summaryEmp.SummaryPeriod;
                    addingSummary.working_days = summaryEmp.NetWorkDayCount.ToString();
                    addingSummary.absent_days = summaryEmp.AbsentDayCount.ToString();
                    addingSummary.invalid_days = summaryEmp.InvalidDayCount.ToString();
                    addingSummary.morning_short_day_leave = summaryEmp.MorningShortLeaveCount.ToString();
                    addingSummary.morning_short_day_nopay = summaryEmp.MorningShortLeaveNopayCount.ToString();
                    addingSummary.morning_halfday_leave_count = summaryEmp.MorningHalfDayLeaveCount.ToString();
                    addingSummary.morning_halfday_nopay_count = summaryEmp.MorningHalfDayNopayCount.ToString();
                    addingSummary.evening_short_day_leave = summaryEmp.EveningShortLeaveCount.ToString();
                    addingSummary.evening_short_day_nopay = summaryEmp.EveningShortLeaveNopayCount.ToString();
                    addingSummary.evening_halfday_leave_count = summaryEmp.EveningHalfdayLeaveCount.ToString();
                    addingSummary.evening_halfday_nopay_count = summaryEmp.EveningHalfDayNopayCount.ToString();
                    addingSummary.leave_fulldays_count = (summaryEmp.MorningFullDayLeaveCount + summaryEmp.EveningFullDayLeaveCount).ToString();
                    addingSummary.nopay_fulldays_count = (summaryEmp.MorningFullDayNopayCount + summaryEmp.EveningFullDayNopayCount).ToString();

                    addingSummary.late_in_time = ((int)summaryEmp.LateInTime.TotalHours).ToString("00") + ":" + summaryEmp.LateInTime.Minutes.ToString("00");
                    addingSummary.late_out_time = ((int)summaryEmp.LateOutTime.TotalHours).ToString("00") + ":" + summaryEmp.LateOutTime.Minutes.ToString("00");
                    addingSummary.early_in_time = ((int)summaryEmp.EarlyInTime.TotalHours).ToString("00") + ":" + summaryEmp.EarlyInTime.Minutes.ToString("00");
                    addingSummary.early_out_time = ((int)summaryEmp.EarlyOutTime.TotalHours).ToString("00") + ":" + summaryEmp.EarlyOutTime.Minutes.ToString("00");

                    addingSummary.actual_ot_intime = ((int)summaryEmp.MorningAllOtTime.TotalHours).ToString("00") + ":" + summaryEmp.MorningAllOtTime.Minutes.ToString("00");
                    addingSummary.actual_ot_outtime = ((int)summaryEmp.EveningAllOtTime.TotalHours).ToString("00") + ":" + summaryEmp.EveningAllOtTime.Minutes.ToString("00");
                    addingSummary.extra_ot_hours = ((int)summaryEmp.ExtraAllOtTime.TotalHours).ToString("00") + ":" + summaryEmp.ExtraAllOtTime.Minutes.ToString("00");

                    addingSummary.single_ot_total = ((int)summaryEmp.SingleOtTime.TotalHours).ToString("000") + ":" + summaryEmp.SingleOtTime.Minutes.ToString("00") + ":" + summaryEmp.SingleOtTime.Seconds.ToString("00");
                    addingSummary.double_ot_total = ((int)summaryEmp.DoubleOtTime.TotalHours).ToString("000") + ":" + summaryEmp.DoubleOtTime.Minutes.ToString("00") + ":" + summaryEmp.DoubleOtTime.Seconds.ToString("00");
                    addingSummary.triple_ot_total = ((int)summaryEmp.TripleOtTime.TotalHours).ToString("000") + ":" + summaryEmp.TripleOtTime.Minutes.ToString("00") + ":" + summaryEmp.TripleOtTime.Seconds.ToString("00");

                    addingSummary.extra_single_ot_total = ((int)summaryEmp.ExtraSingleOtTime.TotalHours).ToString("00") + ":" + summaryEmp.ExtraSingleOtTime.Minutes.ToString("00");
                    addingSummary.extra_double_ot_total = ((int)summaryEmp.ExtraDoubleOtTime.TotalHours).ToString("00") + ":" + summaryEmp.ExtraDoubleOtTime.Minutes.ToString("00");
                    addingSummary.extra_triple_ot_total = ((int)summaryEmp.ExtraTripleOtTime.TotalHours).ToString("00") + ":" + summaryEmp.ExtraTripleOtTime.Minutes.ToString("00");

                    addingSummary.poya_work_time = ((int)summaryEmp.PoyaWorkTime.TotalHours).ToString("00") + ":" + summaryEmp.PoyaWorkTime.Minutes.ToString("00");
                    addingSummary.holiday_work_time = ((int)summaryEmp.HolidayWorkTime.TotalHours).ToString("00") + ":" + summaryEmp.HolidayWorkTime.Minutes.ToString("00");
                    addingSummary.freeday_work_time = ((int)summaryEmp.FreeDayWorkTime.TotalHours).ToString("00") + ":" + summaryEmp.FreeDayWorkTime.Minutes.ToString("00");
                    addingSummary.mercantile_work_time = ((int)summaryEmp.MercantileWorkTime.TotalHours).ToString("00") + ":" + summaryEmp.MercantileWorkTime.Minutes.ToString("00");

                    addingSummary.authorized_nopay_fulldays_count = summaryEmp.MorningFullDayAuthorizedCount.ToString();
                    addingSummary.morning_halfday_authorized_count = summaryEmp.MorningHalfDayAuthorizedCount.ToString();
                    addingSummary.evening_halfday_authorize_count = summaryEmp.EveningHalfDayAuthorizedCount.ToString();
                    addingSummary.aditional_day_count = summaryEmp.AdditionalDayCount.ToString();
                    addingSummary.free_days = summaryEmp.FreeDayCount.ToString();
                    addingSummary.freeday_workcount = summaryEmp.FreeDayWorkCount.ToString();
                    addingSummary.mercantile_workcount = summaryEmp.MercantileWorkCount.ToString();

                    processedAttendanceSummaryList.Add(addingSummary);
                }

                return processedAttendanceSummaryList;
            }
            catch (Exception)
            {

            }

            return new List<trns_EmployeeAttendanceSumarry>();
        }

        #endregion

        #endregion

        #region Employee Attendance Data Migration

        #region Start of Process Main Method

        [OperationContract]
        bool BeginAttendanceDataMigration(List<AttendanceEmployeeDataView> employeeList, DateTime startDate, DateTime endDate, z_Period timePeriod, Guid saveUserID, bool partial, bool full)
        {
            try
            {
                #region Lists

                List<dtl_EmployeeRule> currentRuleList = new List<dtl_EmployeeRule>();
                List<trns_ProcessedEmployeeAttendance> currentProcessedAttendanceList = new List<trns_ProcessedEmployeeAttendance>();
                List<trns_EmployeeAttendanceSumarry> currentProcessedAttendanceSummaryList = new List<trns_EmployeeAttendanceSumarry>();

                #endregion

                List<Guid> selectedEmpList = employeeList.Select(c => c.employee_id).ToList();
                // Get current rule details
                if (partial == true)
                {
                    currentRuleList = GetEmployeeAssignedRulesForPartial(selectedEmpList);
                }
                else if (full == true)
                {
                    currentRuleList = GetEmployeeAssignedRulesForFull(selectedEmpList);
                }
                // Get current processed Attendance details
                currentProcessedAttendanceList = GetEmployeeProcessedAttendance(selectedEmpList, timePeriod.start_date.Value.Date, timePeriod.end_date.Value.Date);

                // Get current processed Attendance summary details
                currentProcessedAttendanceSummaryList = GetEmployeeProcessedAttendanceSummary(timePeriod.period_id);

                // Get fixed data migration rules
                AttendanceRuleData.FIXED_DATA_MIGRATE_RULES = GetFixedRuleConfigurationDetails();

                // Get Attendance Bonus rules
                AttendanceRuleData.ATTENDANCE_BONUS_RULES = GetAttendanceBonusRuleDetails();

                if (currentProcessedAttendanceList.Count > 0 && currentRuleList.Count > 0)
                {
                    List<AttendanceMigrateEmployee> migrateEmpList = new List<AttendanceMigrateEmployee>();
                    foreach (AttendanceEmployeeDataView selectedEmp in employeeList)
                    {
                        if (currentProcessedAttendanceList.Count(c => c.employee_id == selectedEmp.employee_id) > 0)
                        {
                            if (currentRuleList.Count(c => c.employee_id == selectedEmp.employee_id) > 0)
                            {
                                AttendanceMigrateEmployee migrateEmployee = new AttendanceMigrateEmployee();
                                migrateEmployee.ProcessedAttendanceList = currentProcessedAttendanceList.Where(c => c.employee_id == selectedEmp.employee_id).ToList();
                                migrateEmployee.EmployeeAttendSummary = currentProcessedAttendanceSummaryList.FirstOrDefault(c => c.employee_id == selectedEmp.employee_id);
                                migrateEmployee.AssignedRuleList = currentRuleList.Where(c => c.employee_id == selectedEmp.employee_id).ToList();
                                migrateEmployee.SetEmployeeAttendanceRuleStatus();
                                migrateEmpList.Add(migrateEmployee);
                            }
                        }
                    }

                    if (migrateEmpList.Count > 0)
                    {
                        List<trns_EmployeePeriodQunatity> addingQtyList = SetRuleQuantity(migrateEmpList, timePeriod.period_id, saveUserID, partial, full);
                        List<dtl_EmployeeRule> bonusDeductRuleList = SetEmployeeRuleSpecialAmount(migrateEmpList, saveUserID);
                        if (addingQtyList.Count > 0)
                        {
                            if (SaveEmployeePeriodQuantityDetailsFull(addingQtyList, bonusDeductRuleList, timePeriod.period_id, partial, full))
                                return true;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        #endregion

        #region Data Retrieval Methods

        #region Employee Rules

        List<dtl_EmployeeRule> GetEmployeeAssignedRulesForPartial(List<Guid> empList)
        {
            List<dtl_EmployeeRule> employeeRules = new List<dtl_EmployeeRule>();
            try
            {
                using (var context = new AttendanceEntities())
                {
                    employeeRules = context.dtl_EmployeeRule.Where(c => empList.Contains(c.employee_id) && c.isactive == true && c.status == "P").ToList();
                    employeeRules.ForEach(c => context.Detach(c));
                    return employeeRules;
                }
            }
            catch (Exception)
            {
            }

            return employeeRules;
        }

        List<dtl_EmployeeRule> GetEmployeeAssignedRulesForFull(List<Guid> empList)
        {
            List<dtl_EmployeeRule> employeeRules = new List<dtl_EmployeeRule>();
            try
            {
                using (var context = new AttendanceEntities())
                {
                    employeeRules = context.dtl_EmployeeRule.Where(c => empList.Contains(c.employee_id) && c.isactive == true).ToList();
                    employeeRules.ForEach(c => context.Detach(c));
                    return employeeRules;
                }
            }
            catch (Exception)
            {
            }

            return employeeRules;
        }

        #endregion

        #region Processed Attendance

        List<trns_ProcessedEmployeeAttendance> GetEmployeeProcessedAttendance(List<Guid> empList, DateTime periodStart, DateTime periodEnd)
        {
            List<trns_ProcessedEmployeeAttendance> attendanceList = new List<trns_ProcessedEmployeeAttendance>();
            try
            {
                using (var context = new AttendanceEntities())
                {
                    attendanceList = context.trns_ProcessedEmployeeAttendance.Include("trns_ProcessedLeaveStatus").Where(c => c.attend_date >= periodStart.Date && c.attend_date <= periodEnd.Date && empList.Contains(c.employee_id.Value)).ToList();
                    return attendanceList;
                }
            }
            catch (Exception)
            {
            }
            return attendanceList;
        }

        #endregion

        #region Processed Attendance Summary

        List<trns_EmployeeAttendanceSumarry> GetEmployeeProcessedAttendanceSummary(Guid periodID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.trns_EmployeeAttendanceSumarry.Where(c => c.period_id == periodID).ToList();
                    return result;
                }
            }
            catch (Exception)
            {

            }
            return new List<trns_EmployeeAttendanceSumarry>();
        }

        #endregion

        #region Fixed Data Migration Rules

        List<z_Datamigration_Configuration> GetFixedRuleConfigurationDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    List<z_Datamigration_Configuration> fixedRuleList = context.z_Datamigration_Configuration.Where(c => c.is_active == true).ToList();
                    return fixedRuleList;
                }
            }
            catch (Exception)
            {

            }

            return new List<z_Datamigration_Configuration>();
        }

        #endregion

        #region Attendance Bonus Rules

        List<dtl_AttendanceBonus> GetAttendanceBonusRuleDetails()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_AttendanceBonus.Where(c => c.is_active == true).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {

            }

            return new List<dtl_AttendanceBonus>();
        }

        #endregion

        #endregion

        #region Data Setting Methods

        List<trns_EmployeePeriodQunatity> SetRuleQuantity(List<AttendanceMigrateEmployee> empList, Guid periodID, Guid saveUserID, bool Partial, bool Full)
        {
            List<trns_EmployeePeriodQunatity> employeeQuantityList = new List<trns_EmployeePeriodQunatity>();
            foreach (AttendanceMigrateEmployee migratedEmp in empList)
            {
                foreach (trns_EmployeePeriodQunatity empRule in migratedEmp.CapturedRuleQuantityList)
                {
                    trns_EmployeePeriodQunatity empQty = new trns_EmployeePeriodQunatity();
                    empQty.rule_id = empRule.rule_id;
                    empQty.employee_id = empRule.employee_id;
                    empQty.period_id = periodID;
                    empQty.quantity = empRule.quantity;
                    empQty.save_datetime = DateTime.Now;
                    empQty.save_user_id = saveUserID;
                    empQty.isdelete = false;
                    empQty.is_proceed = false;
                    if (Partial == true)
                        empQty.status = "P";
                    else if (Full == true)
                        empQty.status = "F";
                    employeeQuantityList.Add(empQty);
                }
            }
            return employeeQuantityList;
        }

        List<dtl_EmployeeRule> SetEmployeeRuleSpecialAmount(List<AttendanceMigrateEmployee> empList, Guid modifyUserID)
        {
            List<dtl_EmployeeRule> empUpdatingRules = new List<dtl_EmployeeRule>();
            foreach (AttendanceMigrateEmployee migratedEmp in empList)
            {
                foreach (dtl_EmployeeRule empRule in migratedEmp.UpdatingBonusDeductionRules)
                {
                    dtl_EmployeeRule bonusRule = new dtl_EmployeeRule();
                    bonusRule.rule_id = empRule.rule_id;
                    bonusRule.employee_id = empRule.employee_id;
                    bonusRule.special_amount = empRule.special_amount;
                    bonusRule.modified_user_id = modifyUserID;
                    bonusRule.modified_datetime = DateTime.Now;

                    empUpdatingRules.Add(bonusRule);
                }
            }

            return empUpdatingRules;
        }

        #endregion

        #endregion

        #endregion

        #region Leave Module

        #region Praveen

        #region Leave Shifts

        [OperationContract]
        public IEnumerable<EmployeeLeaveShiftDetailsView> GetShiftDetailsByDate(Guid Employee_ID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.EmployeeLeaveShiftDetailsViews.Where(c => c.employee_id == Employee_ID && c.date >= FromDate.Date && c.date <= ToDate.Date).ToList();
                    result.ForEach(d => context.Detach(d));
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [OperationContract]
        public IEnumerable<EmployeeLeaveShiftDetailsNewView> GetShiftDetailsByDateNew(Guid Employee_ID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.EmployeeLeaveShiftDetailsNewViews.Where(c => c.employee_id == Employee_ID && c.date >= FromDate.Date && c.date <= ToDate.Date).OrderBy(c => c.date).ToList();
                    result.ForEach(d => context.Detach(d));
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Automate Attendance Data Upload

        [OperationContract]
        bool UploadAttendanceToDatabase(List<dtl_AttendanceData> empAttendanceList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    if (empAttendanceList.Count > 0)
                    {
                        foreach (dtl_AttendanceData item in empAttendanceList)
                        {
                            context.dtl_AttendanceData.AddObject(item);
                        }

                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region Automate Attendance Data Upload from API

        [OperationContract]
        bool AddAttendanceFromAPI(List<dtl_AttendanceData> empAttendanceList)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    if (empAttendanceList.Count > 0)
                    {
                        foreach (dtl_AttendanceData item in empAttendanceList)
                        {
                            context.dtl_AttendanceData.AddObject(item);
                        }

                        if (validateSaveUpdate(context.SaveChanges()))
                            return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        #endregion

        #region get Last API Download Date

        [OperationContract]
        DateTime? getLastDataDownloadDate() {

            try
            {
                using (var context = new AttendanceEntities())
                {
                    var dataSet = context.dtl_AttendanceData.Where(c => c.device_id == new Guid("00000000-0000-0000-0000-000000000099")).OrderBy(c => c.attend_date).ToList();
                    return dataSet.LastOrDefault().save_datetime;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion

        #region SMS
        [OperationContract]
        IEnumerable<SMS_IN> GetSMSAttendance()
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.SMS_IN.Where(c => c.is_delete == null).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        [OperationContract]
        private bool UpdateSMSAttendance(List<SMS_IN> SMSData)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    foreach (var item in SMSData)
                    {
                        SMS_IN CurrentData = context.SMS_IN.FirstOrDefault(c => c.ID == item.ID);
                        CurrentData.is_delete = true;
                        context.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Drivers Attendance
        [OperationContract]
        IEnumerable<EmployeeShiftView> GetEmployeeShifts(DateTime FromDate, DateTime ToDate, Guid EmployeeID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.EmployeeShiftViews.Where(c => c.date >= FromDate && c.date <= ToDate && c.employee_id == EmployeeID).ToList();
                    result.ForEach(c => context.Detach(c));
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [OperationContract]
        IEnumerable<dtl_AttendanceData> GetEmployeeAttendanceData(DateTime FromDate, DateTime ToDate, string EmpID)
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    var result = context.dtl_AttendanceData.Where(c => c.attend_date >= FromDate && c.attend_date <= ToDate && c.emp_id == EmpID).ToList();
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [OperationContract]
        public bool SaveEmployeeAttendanceData(List<EmployeeShiftView> SaveObj, Guid UserID)
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    foreach (var item in SaveObj)
                    {
                        dtl_EmployeeAttendanceData Exist = context.dtl_EmployeeAttendanceData.FirstOrDefault(c => c.employee_id == item.employee_id && c.date == item.date && c.shift_id == item.shift_detail_id);
                        if (Exist == null)
                        {
                            dtl_EmployeeAttendanceData temp = new dtl_EmployeeAttendanceData();
                            temp.employee_id = item.employee_id;
                            temp.in_time = item.in_time;
                            temp.out_time = item.out_time;
                            temp.date = item.date;
                            temp.shift_id = item.shift_detail_id;
                            temp.save_user_id = UserID;
                            temp.save_datetime = DateTime.Now;
                            context.dtl_EmployeeAttendanceData.AddObject(temp);
                        }
                        else
                        {
                            Exist.in_time = item.in_time;
                            Exist.out_time = item.out_time;
                            Exist.modified_user_id = UserID;
                            Exist.modified_datetime = DateTime.Now;
                        }
                    }
                    return validateSaveUpdate(context.SaveChanges());
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion

        #region Leave Automate Operation

        public bool GenerateProbationLeave(List<AttendanceEmployeeDataView> employeeList, z_Period timePeriod)
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    #region old
                    //foreach (var item in employeeList)
                    //{
                    //    dtl_EmployeeLeave current = context.dtl_EmployeeLeave.FirstOrDefault(c => c.emp_id == item.employee_id && c.is_automate == true && c.is_probation == true);
                    //    if (current != null)
                    //    {
                    //        trns_EmployeeAutoLeaveGenerate ExistsAutoLeave = context.trns_EmployeeAutoLeaveGenerate.FirstOrDefault(x => x.employee_id == item.employee_id && x.leave_detail_id == current.leave_detail_id && x.leave_year == timePeriod.start_date.Value.Year && x.leave_month == timePeriod.start_date.Value.Month && x.is_probation == true);
                    //        // m 2020-08-04
                    //        trns_EmployeeAutoLeaveGenerate FirstDate = context.trns_EmployeeAutoLeaveGenerate.OrderBy(c => c.period_start_date).FirstOrDefault(c => c.employee_id == item.employee_id && c.leave_detail_id == current.leave_detail_id && c.is_probation == true);
                    //        if (ExistsAutoLeave == null)
                    //        {
                    //            // m 2020-08-04
                    //            //trns_EmployeeAutoLeaveGenerate temp = new trns_EmployeeAutoLeaveGenerate();
                    //            //temp.employee_id = item.employee_id;
                    //            //temp.processed_date = DateTime.Now.Date;
                    //            //temp.leave_year = timePeriod.start_date.Value.Year;
                    //            //temp.leave_month = timePeriod.start_date.Value.Month;
                    //            //temp.leave_detail_id = current.leave_detail_id;
                    //            //temp.is_probation = true;
                    //            //temp.leave_amount = Convert.ToDecimal(0.5);
                    //            //context.trns_EmployeeAutoLeaveGenerate.AddObject(temp);

                    //            //current.remaining_days += Convert.ToDecimal(0.5);
                    //            trns_EmployeeAutoLeaveGenerate temp = new trns_EmployeeAutoLeaveGenerate();
                    //            if (FirstDate == null)
                    //            {

                    //                    temp.employee_id = item.employee_id;
                    //                    temp.processed_date = DateTime.Now.Date;
                    //                    temp.period_start_date = timePeriod.start_date.Value.Date;
                    //                    temp.leave_year = timePeriod.start_date.Value.Year;
                    //                    temp.leave_month = timePeriod.start_date.Value.Month;
                    //                    temp.leave_detail_id = current.leave_detail_id;
                    //                    temp.is_probation = true;
                    //                    temp.leave_amount = Convert.ToDecimal(0.5);
                    //                    context.trns_EmployeeAutoLeaveGenerate.AddObject(temp);

                    //                    current.remaining_days += Convert.ToDecimal(0.5);  


                    //            }
                    //            else if (timePeriod.start_date.Value.Date > FirstDate.period_start_date)
                    //            {
                    //                temp.employee_id = item.employee_id;
                    //                temp.processed_date = DateTime.Now.Date;
                    //                temp.period_start_date = timePeriod.start_date.Value.Date;
                    //                temp.leave_year = timePeriod.start_date.Value.Year;
                    //                temp.leave_month = timePeriod.start_date.Value.Month;
                    //                temp.leave_detail_id = current.leave_detail_id;
                    //                temp.is_probation = true;
                    //                temp.leave_amount = Convert.ToDecimal(0.5);
                    //                context.trns_EmployeeAutoLeaveGenerate.AddObject(temp);

                    //                current.remaining_days += Convert.ToDecimal(0.5);
                    //            }
                    //        }                                     
                    //    }
                    //} 
                    #endregion

                    foreach (var item in employeeList)
                    {
                        dtl_EmployeeLeave current = context.dtl_EmployeeLeave.FirstOrDefault(c => c.emp_id == item.employee_id && c.is_automate == true && c.is_probation == true);
                        if (current != null)
                        {
                            if (item.leave_end_date != null && item.prmernant_active_date >= DateTime.Now.Date)
                            {
                                trns_EmployeeAutoLeaveGenerate ExistsAutoLeave = context.trns_EmployeeAutoLeaveGenerate.FirstOrDefault(x => x.employee_id == item.employee_id && x.leave_detail_id == current.leave_detail_id && x.leave_year == timePeriod.start_date.Value.Year && x.leave_month == timePeriod.start_date.Value.Month && x.is_probation == true);
                                trns_EmployeeAutoLeaveGenerate FirstDate = context.trns_EmployeeAutoLeaveGenerate.OrderBy(c => c.period_start_date).FirstOrDefault(c => c.employee_id == item.employee_id && c.leave_detail_id == current.leave_detail_id && c.is_probation == true);
                                //DateTime? FirstDate = context.trns_EmployeeAutoLeaveGenerate.First(c => c.employee_id == item.employee_id && c.leave_detail_id == current.leave_detail_id && c.is_probation == true).period_start_date;
                                if (ExistsAutoLeave == null)
                                {
                                    trns_EmployeeAutoLeaveGenerate temp = new trns_EmployeeAutoLeaveGenerate();
                                    if (FirstDate == null)
                                    {
                                        temp.employee_id = item.employee_id;
                                        temp.processed_date = DateTime.Now.Date;
                                        temp.period_start_date = timePeriod.start_date.Value.Date;
                                        temp.leave_year = timePeriod.start_date.Value.Year;
                                        temp.leave_month = timePeriod.start_date.Value.Month;
                                        temp.leave_detail_id = current.leave_detail_id;
                                        temp.is_probation = true;
                                        temp.leave_amount = Convert.ToDecimal(0.5);
                                        context.trns_EmployeeAutoLeaveGenerate.AddObject(temp);

                                        current.remaining_days += Convert.ToDecimal(0.5);
                                    }
                                    else if (timePeriod.start_date.Value.Date > FirstDate.period_start_date)
                                    {
                                        temp.employee_id = item.employee_id;
                                        temp.processed_date = DateTime.Now.Date;
                                        temp.period_start_date = timePeriod.start_date.Value.Date;
                                        temp.leave_year = timePeriod.start_date.Value.Year;
                                        temp.leave_month = timePeriod.start_date.Value.Month;
                                        temp.leave_detail_id = current.leave_detail_id;
                                        temp.is_probation = true;
                                        temp.leave_amount = Convert.ToDecimal(0.5);
                                        context.trns_EmployeeAutoLeaveGenerate.AddObject(temp);

                                        current.remaining_days += Convert.ToDecimal(0.5);
                                    }
                                }
                            }
                            else
                            {
                                current.remaining_days = 0;
                            }
                        }
                    }

                    return validateSaveUpdate(context.SaveChanges());
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion

        #region Workdays for payroll
        [OperationContract]
        decimal GetEmployeeAttendanceSummary(Guid employee, Guid period)    //haritha
        {
            try
            {
                using (var context = new AttendanceEntities())
                {
                    return Convert.ToDecimal(context.trns_EmployeeAttendanceSumarry.Where(c => c.employee_id == employee && c.period_id == period).Select(d => d.working_days).FirstOrDefault());
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        // h 2020-08-12
        #region Attendance Process Delete Method

        [OperationContract]
        public bool DeleteAttendanceProcessEmployeeAndAttendDayWise(List<Guid> EmpList, DateTime SDate, DateTime EDate)
        {
            using (var context = new AttendanceEntities())
            {
                try
                {
                    foreach (var empid in EmpList)
                    {
                        context.SP_DeleteAttendanceProcessEmployeeAndPeriodWise(empid, SDate, EDate);
                    }
                    context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }

        #endregion
    }
}