using AttendanceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceOnlineService.HelperClasses
{
    public class AttendanceMigrateEmployee
    {
        #region Member Data

        trns_EmployeeAttendanceSumarry empAttendSummary;

        #region Lists

        List<dtl_EmployeeRule> assignedRuleList = new List<dtl_EmployeeRule>();
        List<trns_ProcessedEmployeeAttendance> processedAttendanceList = new List<trns_ProcessedEmployeeAttendance>();
        List<trns_EmployeePeriodQunatity> capturedRuleQuantityList = new List<trns_EmployeePeriodQunatity>();

        List<dtl_EmployeeRule> updatingBonusDeductionRules = new List<dtl_EmployeeRule>();

        #endregion

        #endregion

        #region Properties

        public trns_EmployeeAttendanceSumarry EmployeeAttendSummary
        {
            set { empAttendSummary = value; }
        }

        public List<dtl_EmployeeRule> AssignedRuleList
        {
            get { return assignedRuleList; }
            set { assignedRuleList = value; }
        }

        public List<trns_ProcessedEmployeeAttendance> ProcessedAttendanceList
        {
            get { return processedAttendanceList; }
            set { processedAttendanceList = value; }
        }

        public List<trns_EmployeePeriodQunatity> CapturedRuleQuantityList
        {
            get { return capturedRuleQuantityList; }
        }

        public List<dtl_EmployeeRule> UpdatingBonusDeductionRules
        {
            get { return updatingBonusDeductionRules; }
        }

        #region Leave and No-pay

        #region Morning Leave Count

        double morningFullLeaveCount;
        public double MorningFullLeaveCount
        {
            get { return morningFullLeaveCount; }
        }

        double morningHalfdayLeaveCount;
        public double MorningHalfdayLeaveCount
        {
            get { return morningHalfdayLeaveCount; }
        }

        double morningShortLeaveCount;
        public double MorningShortLeaveCount
        {
            get { return morningShortLeaveCount; }
        }

        #endregion

        #region Evening Leave Count

        double eveningFullLeaveCount;
        public double EveningFullLeaveCount
        {
            get { return eveningFullLeaveCount; }
        }

        double eveningHalfDayLeaveCount;
        public double EveningHalfDayLeaveCount
        {
            get { return eveningHalfDayLeaveCount; }
        }

        double eveningShortLeaveCount;
        public double EveningShortLeaveCount
        {
            get { return eveningShortLeaveCount; }
        }

        #endregion

        #region Morning No-Pay Count

        double morningFullLeaveNopayCount;
        public double MorningFullLeaveNopayCount
        {
            get { return morningFullLeaveNopayCount; }
        }

        double morningHalfdayLeaveNopayCount;
        public double MorningHalfdayLeaveNopayCount
        {
            get { return morningHalfdayLeaveNopayCount; }
        }

        double morningShortLeaveNopayCount;
        public double MorningShortLeaveNopayCount
        {
            get { return morningShortLeaveNopayCount; }
        }

        #endregion

        #region Evening No-Pay Count

        double eveningFulldayLeaveNopayCount;
        public double EveningFulldayLeaveNopayCount
        {
            get { return eveningFulldayLeaveNopayCount; }
        }

        double eveningHalfdayLeaveNopayCount;
        public double EveningHalfdayLeaveNopayCount
        {
            get { return eveningHalfdayLeaveNopayCount; }
        }

        double eveningShortLeaveNopayCount;
        public double EveningShortLeaveNopayCount
        {
            get { return eveningShortLeaveNopayCount; }
        }

        #endregion

        #region MCN Late In Count
        int countLateInDuration;
        public int CountLateInDuration
        {
            get { return countLateInDuration; }
        }

        double countLateIn;
        public double CountLateIn
        {
            get { return countLateIn; }
        }

        #endregion

        trns_EmployeePeriodQunatity nopayRule;
        public trns_EmployeePeriodQunatity NopayRule
        {
            get { return nopayRule; }
        }

        #region MCN

        trns_EmployeePeriodQunatity gracelate;
        public trns_EmployeePeriodQunatity Gracelate
        {
            get { return gracelate; }
        } 

        #endregion

        #endregion

        #region Late

       
        int morningLateInDuration;
        public int MorningLateInDuration
        {
            get { return morningLateInDuration; }
        }

        int eveningEarlyOutDuration;
        public int EveningEarlyOutDuration
        {
            get { return eveningEarlyOutDuration; }
        }

        trns_EmployeePeriodQunatity lateRule;
        public trns_EmployeePeriodQunatity LateRule
        {
            get { return lateRule; }
        }

        #endregion

        #region Over Time

        #region Pre OT Duration

        int preSingleOtDuration;
        public int PreSingleOtDuration
        {
            get { return preSingleOtDuration; }
        }

        int preDoubleOtDuration;
        public int PreDoubleOtDuration
        {
            get { return preDoubleOtDuration; }
        }

        int preTripleOtDuration;
        public int PreTripleOtDuration
        {
            get { return preTripleOtDuration; }
        }

        #endregion

        #region Post OT Duration

        int postSingleOtDuration;
        public int PostSingleOtDuration
        {
            get { return postSingleOtDuration; }
        }

        int postDoubleOtDuration;
        public int PostDoubleOtDuration
        {
            get { return postDoubleOtDuration; }
        }

        int postTripleOtDuration;
        public int PostTripleOtDuration
        {
            get { return postTripleOtDuration; }
        }

        #endregion

        trns_EmployeePeriodQunatity normalOtRule;
        public trns_EmployeePeriodQunatity NormalOtRule
        {
            get { return normalOtRule; }
        }

        trns_EmployeePeriodQunatity doubleOtRule;
        public trns_EmployeePeriodQunatity DoubleOtRule
        {
            get { return doubleOtRule; }
        }

        trns_EmployeePeriodQunatity tripleOtRule;
        public trns_EmployeePeriodQunatity TripleOtRule
        {
            get { return tripleOtRule; }
        }

        // h 2020-10-13
        private int otDays;
        public int OtDays
        {
            get { return otDays; }
        }

        #endregion

        #region Rule Status

        bool hasNopayRule;
        public bool HasNopayRule
        {
            get { return hasNopayRule; }
            set { hasNopayRule = value; }
        }

        bool hasNormalOtRule;
        public bool HasNormalOtRule
        {
            get { return hasNormalOtRule; }
            set { hasNormalOtRule = value; }
        }

        bool hasDoubleOtRule;
        public bool HasDoubleOtRule
        {
            get { return hasDoubleOtRule; }
            set { hasDoubleOtRule = value; }
        }

        bool hasTripleOtRule;
        public bool HasTripleOtRule
        {
            get { return hasTripleOtRule; }
            set { hasTripleOtRule = value; }
        }

        bool hasLateRule;
        public bool HasLateRule
        {
            get { return hasLateRule; }
            set { hasLateRule = value; }
        }

        #region MCN
        bool hasGracelateRule;
        public bool HasGracelateRule
        {
            get { return hasGracelateRule; }
            set { hasGracelateRule = value; }
        } 
        #endregion

        #endregion

        #endregion

        #region Data Migration

        public void SetEmployeeAttendanceRuleStatus()
        {
            if (assignedRuleList.Count > 0)
            {
                if (assignedRuleList.Count(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.NoPayShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.NoPayWagesBoards)) > 0)
                {
                    nopayRule = new trns_EmployeePeriodQunatity();
                    dtl_EmployeeRule currentRule = assignedRuleList.FirstOrDefault(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.NoPayShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.NoPayWagesBoards));
                    nopayRule.rule_id = currentRule.rule_id;
                    nopayRule.employee_id = currentRule.employee_id;
                    capturedRuleQuantityList.Add(nopayRule);
                    hasNopayRule = true;
                }

                if (assignedRuleList.Count(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.NormalOverTimeShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.NormalOverTimeWagesBoards)) > 0)
                {
                    normalOtRule = new trns_EmployeePeriodQunatity();
                    dtl_EmployeeRule currentRule = assignedRuleList.FirstOrDefault(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.NormalOverTimeShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.NormalOverTimeWagesBoards));
                    normalOtRule.rule_id = currentRule.rule_id;
                    normalOtRule.employee_id = currentRule.employee_id;
                    capturedRuleQuantityList.Add(normalOtRule);
                    hasNormalOtRule = true;
                }

                if (assignedRuleList.Count(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.DoubleOverTimeShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.DoubleOverTimeWagesBoards)) > 0)
                {
                    doubleOtRule = new trns_EmployeePeriodQunatity();
                    dtl_EmployeeRule currentRule = assignedRuleList.FirstOrDefault(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.DoubleOverTimeShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.DoubleOverTimeWagesBoards));
                    doubleOtRule.rule_id = currentRule.rule_id;
                    doubleOtRule.employee_id = currentRule.employee_id;
                    capturedRuleQuantityList.Add(doubleOtRule);
                    hasDoubleOtRule = true;
                }
                if (assignedRuleList.Count(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.TripleOTShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.TripleOTWagesBoards)) > 0)
                {
                    tripleOtRule = new trns_EmployeePeriodQunatity();
                    dtl_EmployeeRule currentRule = assignedRuleList.FirstOrDefault(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.TripleOTShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.TripleOTWagesBoards));
                    tripleOtRule.rule_id = currentRule.rule_id;
                    tripleOtRule.employee_id = currentRule.employee_id;
                    capturedRuleQuantityList.Add(tripleOtRule);
                    hasTripleOtRule = true;
                }
                if (assignedRuleList.Count(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.LateShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.LateWagesBoards)) > 0)
                {
                    lateRule = new trns_EmployeePeriodQunatity();
                    dtl_EmployeeRule currentRule = assignedRuleList.FirstOrDefault(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.LateShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.LateWagesBoards));
                    lateRule.rule_id = currentRule.rule_id;
                    lateRule.employee_id = currentRule.employee_id;
                    capturedRuleQuantityList.Add(lateRule);
                    hasLateRule = true;
                }

                //if (assignedRuleList.Count(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.LateShopAndOffice) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.LateWagesBoards)) > 0)
                //{
                //    gracelate = new trns_EmployeePeriodQunatity();
                //    dtl_EmployeeRule currentRule = assignedRuleList.FirstOrDefault(c => c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.GRACE_LATE_SHOP_AND_OFFICE) || c.rule_id == AttendanceRuleData.GetAttendanceRule(AttendanceRuleName.GRACE_LATE_WAGES_BOARD));
                //    gracelate.rule_id = currentRule.rule_id;
                //    gracelate.employee_id = currentRule.employee_id;
                //    capturedRuleQuantityList.Add(gracelate);
                //    hasGracelateRule = true;

                //}

                this.employeeAttendanceQuantityCalculation();
                this.setEmployeeAttendanceRuleQuantity();
                this.setEmployeeOtherRuleQuantity();
                //this.employeeAttendanceBonusCalculation();

            }
        }

        void employeeAttendanceQuantityCalculation()
        {
            foreach (trns_ProcessedEmployeeAttendance dayAttendance in processedAttendanceList)
            {
                if (hasNopayRule)
                {
                    // no-pay leaves obtained in morning
                    // morning full day, halfday and short leave no-pay leaves
                    morningFullLeaveNopayCount += dayAttendance.trns_ProcessedLeaveStatus.Count(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_FULL_LEAVE) && c.is_approved == false);
                    morningHalfdayLeaveNopayCount += dayAttendance.trns_ProcessedLeaveStatus.Count(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_HALFDAY_LEAVE) && c.is_approved == false);
                    morningShortLeaveNopayCount += dayAttendance.trns_ProcessedLeaveStatus.Count(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_SHORT_LEAVE) && c.is_approved == false);

                    // no-pay leaves obtained in evening
                    // evening full day, halfday and short leave no-pay leaves
                    eveningFulldayLeaveNopayCount += dayAttendance.trns_ProcessedLeaveStatus.Count(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_FULL_LEAVE) && c.is_approved == false);
                    eveningHalfdayLeaveNopayCount += dayAttendance.trns_ProcessedLeaveStatus.Count(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_HALFDAY_LEAVE) && c.is_approved == false);
                    eveningShortLeaveNopayCount += dayAttendance.trns_ProcessedLeaveStatus.Count(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.EVENING_SHORT_LEAVE) && c.is_approved == false);
                }

                // morning late in total quantity
                if (hasLateRule)
                {
                    morningLateInDuration += dayAttendance.late_in_duration.Value;
                    // evening early out total quantity
                    eveningEarlyOutDuration += dayAttendance.early_out_duration.Value;
                }

                if (hasNormalOtRule)
                {
                    // morning single overtime total quantity
                    preSingleOtDuration += dayAttendance.pre_single_ot_duration.Value;
                    // evening single overtime total quantity
                    postSingleOtDuration += dayAttendance.post_single_ot_duration.Value;
                }

                if (hasDoubleOtRule)
                {
                    // morning double overtime total quantity
                    preDoubleOtDuration += dayAttendance.pre_double_ot_duration.Value;
                    // evening double overtime total quantity
                    postDoubleOtDuration += dayAttendance.post_double_ot_duration.Value;
                }

                if (hasTripleOtRule)
                {
                    // morning triple overtime total quantity
                    preTripleOtDuration += dayAttendance.post_triple_ot_duration.Value;
                    // evening triple overtime total quantity
                    postTripleOtDuration += dayAttendance.post_triple_ot_duration.Value;
                }
               
                #region MCN Get Count in late in duration < 15 Minutes && approved Leave and HALFDAY or SHORT Leave
                if (hasGracelateRule)
                {
                    if ((dayAttendance.late_in_duration.Value <= new TimeSpan(0, 15, 0).TotalSeconds) && (dayAttendance.trns_ProcessedLeaveStatus.Count(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_HALFDAY_LEAVE) && c.is_approved == false) > 0 || dayAttendance.trns_ProcessedLeaveStatus.Count(c => c.leave_type_id == clsLeave.GetLeaveOption(leaveoption.MORNING_SHORT_LEAVE) && c.is_approved == false) > 0))
                    {
                        countLateInDuration++;
                    }
                }
                #endregion

            }
        }

        void setEmployeeAttendanceRuleQuantity()
        {
            // h 2020-10-14
            //if (hasNopayRule)
            //    nopayRule.quantity = (decimal)(morningFullLeaveNopayCount + (morningHalfdayLeaveNopayCount/2) + (morningShortLeaveNopayCount/4) + eveningFulldayLeaveNopayCount + (eveningHalfdayLeaveNopayCount/2) + (eveningShortLeaveNopayCount/4));
            if (hasNopayRule)
            {
                nopayRule.quantity = (decimal)(morningFullLeaveNopayCount + (morningHalfdayLeaveNopayCount / 2) + (morningShortLeaveNopayCount / 4) + eveningFulldayLeaveNopayCount + (eveningHalfdayLeaveNopayCount / 2) + (eveningShortLeaveNopayCount / 4));
            }
            #region MCN LeaveCount * Leave value ( morningShortLeaveNopayCount *  0.27)
            //if (hasNopayRule)
            //    nopayRule.quantity = (decimal)(
            //        morningFullLeaveNopayCount * clsLeave.GetLeaveTypeValue(leaveType.MORNING_FD_VALUE) +
            //        morningHalfdayLeaveNopayCount * clsLeave.GetLeaveTypeValue(leaveType.MORNING_HD_VALUE) +
            //        morningShortLeaveNopayCount * clsLeave.GetLeaveTypeValue(leaveType.MORNING_SL_VALUE) +
            //        eveningFulldayLeaveNopayCount * clsLeave.GetLeaveTypeValue(leaveType.EVENING_FD_VALUE) +
            //        eveningHalfdayLeaveNopayCount * clsLeave.GetLeaveTypeValue(leaveType.EVENING_HD_VALUE) +
            //        eveningShortLeaveNopayCount * clsLeave.GetLeaveTypeValue(leaveType.EVENING_SL_VALUE));            
            #endregion

            if (hasLateRule)
                lateRule.quantity = (decimal)(TimeSpan.FromSeconds(morningLateInDuration + eveningEarlyOutDuration)).TotalMinutes;
            if (hasNormalOtRule)
                normalOtRule.quantity = (decimal)(TimeSpan.FromSeconds(preSingleOtDuration + postSingleOtDuration)).TotalHours;
            if (hasDoubleOtRule)
                doubleOtRule.quantity = (decimal)(TimeSpan.FromSeconds(preDoubleOtDuration + postDoubleOtDuration)).TotalHours;
            if (hasTripleOtRule)
                tripleOtRule.quantity = (decimal)(TimeSpan.FromSeconds(preTripleOtDuration + postTripleOtDuration)).TotalHours;
            #region MCN
            if (hasGracelateRule)
            {
                countLateIn = countLateInDuration;
                if (((countLateIn / 3) - 1) <= 0)
                {
                    countLateInDuration = 0;
                }
                else if (((countLateIn / 3) % 1) == 0)
                {
                    countLateInDuration = (int)(countLateIn / 3) - 1;
                }
                else if ((countLateIn / 3) > 1)
                {
                    countLateInDuration = (int)(countLateIn / 3);
                }
                gracelate.quantity = (decimal)(countLateInDuration * clsLeave.GetLeaveTypeValue(leaveType.MORNING_HD_VALUE));
            } 
            #endregion
                
        }

        void setEmployeeOtherRuleQuantity()
        {
            List<dtl_EmployeeRule> remainingRules = assignedRuleList.Where(c => !capturedRuleQuantityList.Any(d => d.rule_id == c.rule_id)).ToList();
            if (remainingRules.Count > 0)
            {
                // process employee rules not related to any hard-coded rules in application
                foreach (dtl_EmployeeRule empRule in remainingRules)
                {
                    trns_EmployeePeriodQunatity processingRule = new trns_EmployeePeriodQunatity();
                    processingRule.rule_id = empRule.rule_id;
                    processingRule.employee_id = empRule.employee_id;
                    z_Datamigration_Configuration fixedRule = AttendanceRuleData.FIXED_DATA_MIGRATE_RULES.FirstOrDefault(c => c.rule_id == empRule.rule_id);
                    if (fixedRule == null)
                    {
                        processingRule.quantity = 0;
                    }
                    else
                    {
                        processingRule.quantity = fixedRule.default_qty;
                    }
                    
                    // m 2021-06-16
                    if(empRule.rule_id == new Guid("7DBA3F91-1878-488F-B766-797AF7F90C53")) //morning incentive
                    {
                        processingRule.quantity = processedAttendanceList.Count(c => c.is_mor_inc.GetValueOrDefault() == true);
                    }
                    if (empRule.rule_id == new Guid("E612AF8F-9E5F-439C-AB3D-AEA1EF330063")) //special late deduction
                    {
                        processingRule.quantity = processedAttendanceList.Count(c => c.is_late_deduc.GetValueOrDefault() == true);
                    }
                    if (empRule.rule_id == new Guid("F51AA2B2-D17B-449C-90C8-F2500CECC624")) //working inc for per day
                    {
                        processingRule.quantity = Convert.ToDecimal(empAttendSummary.working_days);
                    }
                    if (empRule.rule_id == new Guid("")) //working inc for propotioanal
                    {
                        processingRule.quantity = Convert.ToDecimal(empAttendSummary.working_days) / processedAttendanceList.Count(c => c.shift_detail_id != 0);
                    }

                    capturedRuleQuantityList.Add(processingRule);
                }
            }
        }

        void employeeAttendanceBonusCalculation()
        {
            if (capturedRuleQuantityList.Count > 0)
            {
                /* check for employee rules entitled for Attendance Bonus 
                   Attendance Bonus is configured with two parts
                 * 1. Additions - Attendance allowance for bonus 
                 * 2. Subtractions - Deductions due to specified conditions
                 * Bonus = Additions - Subtractions
                 
                 * So for one attendance allowance several deductions could be specified. But for a specific employee only
                 * one Allowance and one Deduction should be applied.
                 */

                var empAssignedBonusRuleList = AttendanceRuleData.ATTENDANCE_BONUS_RULES.Where(c => capturedRuleQuantityList.Any(d => d.rule_id == c.benifit_rule_id));

                if (empAssignedBonusRuleList.Count() > 0)
                {
                    // find out distinct attendance allowances entitled for employee and calculate attendance bonus for each rule
                    foreach (Guid benefitID in empAssignedBonusRuleList.Select(c => c.benifit_rule_id).Distinct())
                    {
                        // find all deduction conditions for current benefit rule
                        var empAttendanceBonusDeductionRuleList = AttendanceRuleData.ATTENDANCE_BONUS_RULES.Where(c => c.benifit_rule_id == benefitID && c.is_deduct == true).OrderBy(c => c.value);
                        if (empAttendSummary != null)
                        {
                            double emp_SL = Convert.ToInt32(empAttendSummary.morning_short_day_leave) * clsLeave.GetLeaveTypeValue(leaveType.MORNING_SL_VALUE) + Convert.ToInt32(empAttendSummary.evening_short_day_leave) * clsLeave.GetLeaveTypeValue(leaveType.EVENING_SL_VALUE);
                            double emp_HD = Convert.ToInt32(empAttendSummary.morning_halfday_leave_count) * clsLeave.GetLeaveTypeValue(leaveType.MORNING_HD_VALUE) + Convert.ToInt32(empAttendSummary.evening_halfday_leave_count) * clsLeave.GetLeaveTypeValue(leaveType.EVENING_HD_VALUE);
                            double emp_FD = Convert.ToInt32(empAttendSummary.leave_fulldays_count) * clsLeave.GetLeaveTypeValue(leaveType.MORNING_FD_VALUE);
                            double emp_SL_NOPAY = Convert.ToInt32(empAttendSummary.morning_short_day_nopay) * clsLeave.GetLeaveTypeValue(leaveType.MORNING_SL_VALUE) + Convert.ToInt32(empAttendSummary.evening_short_day_nopay) * clsLeave.GetLeaveTypeValue(leaveType.EVENING_SL_VALUE);
                            double emp_HD_NOPAY = Convert.ToInt32(empAttendSummary.morning_halfday_nopay_count) * clsLeave.GetLeaveTypeValue(leaveType.MORNING_HD_VALUE) + Convert.ToInt32(empAttendSummary.evening_halfday_nopay_count) * clsLeave.GetLeaveTypeValue(leaveType.EVENING_HD_VALUE);
                            double emp_FD_NOPAY = Convert.ToInt32(empAttendSummary.nopay_fulldays_count) * clsLeave.GetLeaveTypeValue(leaveType.MORNING_FD_VALUE);

                            decimal totalDeductionDays = (decimal)(emp_SL + emp_SL_NOPAY + emp_HD + emp_HD_NOPAY + emp_FD + emp_FD_NOPAY);

                            if (empAttendanceBonusDeductionRuleList.Any(c => c.value <= totalDeductionDays))
                            {
                                // Employee entitled for Bonus deduction
                                dtl_EmployeeRule bonusDeduction = new dtl_EmployeeRule();

                                dtl_AttendanceBonus maxDeduction = empAttendanceBonusDeductionRuleList.LastOrDefault();
                                if (maxDeduction.value < totalDeductionDays)
                                {
                                    // Employee entitled deduction days more than all specified deduction values
                                    // So consider Bonus deduction as largest value specified

                                    trns_EmployeePeriodQunatity empAttendBonusDeductQty = capturedRuleQuantityList.FirstOrDefault(c => c.rule_id == maxDeduction.deduction_rule_id);
                                    if (empAttendBonusDeductQty != null)
                                    {
                                        // update special amount of Attendance Bonus deduction for employee
                                        bonusDeduction.rule_id = empAttendBonusDeductQty.rule_id;
                                        bonusDeduction.employee_id = empAttendBonusDeductQty.employee_id;
                                        bonusDeduction.special_amount = maxDeduction.deduct_value.Value;
                                        empAttendBonusDeductQty.quantity = 1;
                                    }
                                }
                                else
                                {
                                    foreach (var deductRule in empAttendanceBonusDeductionRuleList)
                                    {
                                        if (totalDeductionDays <= deductRule.value)
                                        {
                                            // deduction found
                                            // Bonus deduction amount is considered as deduct_value amount of matching deduction rule
                                            trns_EmployeePeriodQunatity empAttendBonusDeductQty = capturedRuleQuantityList.FirstOrDefault(c => c.rule_id == deductRule.deduction_rule_id);
                                            if (empAttendBonusDeductQty != null)
                                            {
                                                // update special amount of Attendance Bonus deduction for employee
                                                bonusDeduction.rule_id = empAttendBonusDeductQty.rule_id;
                                                bonusDeduction.employee_id = empAttendBonusDeductQty.employee_id;
                                                bonusDeduction.special_amount = deductRule.deduct_value;
                                                empAttendBonusDeductQty.quantity = 1;
                                            }

                                            break;
                                        }
                                    }
                                }

                                this.updatingBonusDeductionRules.Add(bonusDeduction);
                            }

                        }
                    }
                }
            }
        }

        #endregion
    }
}