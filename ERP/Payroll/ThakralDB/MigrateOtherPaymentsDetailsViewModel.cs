using CustomBusyBox;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ERP.Payroll.ThakralDB
{
    class MigrateOtherPaymentsDetailsViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        #endregion

        #region Properties
        private IEnumerable<z_BonusPeriod> _Periods;

        public IEnumerable<z_BonusPeriod> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_BonusPeriod _CurrentPeriod;

        public z_BonusPeriod CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }

        private IEnumerable<GetOtherPaymentsDataToMigrate_Result> _SalaryDetails;

        public IEnumerable<GetOtherPaymentsDataToMigrate_Result> SalaryDetails
        {
            get { return _SalaryDetails; }
            set { _SalaryDetails = value; OnPropertyChanged("SalaryDetails"); }
        }

        #endregion

        #region Constructor
        public MigrateOtherPaymentsDetailsViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefrshPeriods();
        }
        #endregion

        #region Refresh Method
        private void RefrshPeriods()
        {
            try
            {
                serviceClient.GetBonusPeriodCompleted += (s, e) =>
                    {
                        Periods = e.Result;
                    };
                serviceClient.GetBonusPeriodAsync();
            }
            catch (Exception)
            {
                clsMessages.setMessage("Periods Refresh Failed");
            }
        }
        #endregion

        #region Commands And Methods
        public ICommand btnSend
        {
            get
            {
                return new RelayCommand(Send);
            }
        }

        private void Send()
        {
            //try
            //{
            //    BusyBox.ShowBusy("Please Wait Until Data Migrate Completed...");
            //    SalaryDetails = serviceClient.GetEmployeeSalaryDetailsToThakralDB(CurrentPeriod.period_id);
            //    if(SalaryDetails != null)
            //    {
            //        SqlConnection con;
            //        SqlCommand com;
            //        string sql = null;
            //        int RowsAffected = 0;
            //        string ConnectionString = serviceClient.GetThakralDBConnection();
            //        con = new SqlConnection(ConnectionString);
            //        con.Open();
            //        foreach (var item in SalaryDetails.OrderBy(c => c.emp_id))
            //        {
            //            sql = "INSERT INTO trns_PayrollDetails (employee_id,emp_id,period_id,period_start,period_end,rule_name,rule_amount,rule_acc_id,cost_center_id,division_id,is_executive,save_user_id,save_datetime) values (@EmployeeID,@EmpID,@PeriodID,@PeriodStart,@PeriodEnd,@RuleName,@Amount,@AccID,@CCID,@DID,@Executive,@SaveUser,@SaveDate)";
            //            com = new SqlCommand(sql, con);

            //            com.Parameters.Add("@EmployeeID", System.Data.SqlDbType.UniqueIdentifier);
            //            com.Parameters.Add("@EmpID", System.Data.SqlDbType.NVarChar);
            //            com.Parameters.Add("@PeriodID", System.Data.SqlDbType.UniqueIdentifier);
            //            com.Parameters.Add("@PeriodStart", System.Data.SqlDbType.DateTime);
            //            com.Parameters.Add("@PeriodEnd", System.Data.SqlDbType.DateTime);
            //            com.Parameters.Add("@RuleName", System.Data.SqlDbType.NVarChar);
            //            com.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal);
            //            com.Parameters.Add("@AccID", System.Data.SqlDbType.Int);
            //            com.Parameters.Add("@CCID", System.Data.SqlDbType.Int);
            //            com.Parameters.Add("@DID", System.Data.SqlDbType.Int);
            //            com.Parameters.Add("@Executive", System.Data.SqlDbType.Bit);
            //            com.Parameters.Add("@SaveUser", System.Data.SqlDbType.UniqueIdentifier);
            //            com.Parameters.Add("@SaveDate", System.Data.SqlDbType.DateTime);

            //            com.Parameters["@EmployeeID"].Value = item.employee_id;
            //            com.Parameters["@EmpID"].Value = item.emp_id;
            //            com.Parameters["@PeriodID"].Value = item.period_id;
            //            com.Parameters["@PeriodStart"].Value = item.start_date;
            //            com.Parameters["@PeriodEnd"].Value = item.end_date;
            //            com.Parameters["@RuleName"].Value = item.rule_name;
            //            com.Parameters["@Amount"].Value = item.total_amount;
            //            com.Parameters["@AccID"].Value = 000;
            //            if (item.cost_center_id != null)
            //            {
            //                com.Parameters["@CCID"].Value = item.cost_center_id; 
            //            }
            //            else
            //            {
            //                com.Parameters["@CCID"].Value = 000;
            //            }
            //            if (item.division_id != null)
            //            {
            //                com.Parameters["@DID"].Value = item.division_id; 
            //            }
            //            else
            //            {
            //                com.Parameters["@DID"].Value = 000;
            //            }
            //            com.Parameters["@Executive"].Value = item.isExecutive;
            //            com.Parameters["@SaveUser"].Value = clsSecurity.loggedUser.user_id;
            //            com.Parameters["@SaveDate"].Value = System.DateTime.Now;
            //            RowsAffected = com.ExecuteNonQuery();
            //        }
            //        con.Close();
            //        BusyBox.CloseBusy();
            //        clsMessages.setMessage("Payroll Data Migrated Successfully. "+RowsAffected+" Rows Affected.");
            //    }
            //    else
            //    {
            //        BusyBox.CloseBusy();
            //        clsMessages.setMessage("No Data For The Selected Period");
            //    }
            //}
            //catch (Exception)
            //{
            //    BusyBox.CloseBusy();
            //    clsMessages.setMessage("Error In Payroll Data Migration");
            //}

            try
            {
                BusyBox.ShowBusy("Please Wait Until Data Migrate Completed...");
                SalaryDetails = serviceClient.GetEmployeeOtherPaymentsDetailsToThakralDB(CurrentPeriod.Bonus_Period_id).OrderBy(c => c.emp_id);
                if (SalaryDetails != null)
                {
                    SqlConnection con;
                    SqlCommand com;
                    string sql1 = null, sql2 = null, EmpId = null, RuleName = null;
                    int RowsAffected = 0, MAXRECORDNO = 0, RuleACCId = 0, CCId = 0, DId = 0;
                    Guid EmployeeId = new Guid();
                    Guid PeriodId = new Guid();
                    Guid SaveUserId = new Guid();
                    DateTime StartDate, EndDate, SaveDateTime;
                    decimal RuleAmount = 0;
                    string EmployeeType = "";
                    string ConnectionString = serviceClient.GetThakralDBConnection();
                    string DBName = serviceClient.GetThakralDBName();
                    con = new SqlConnection(ConnectionString);
                    con.Open();
                    sql1 = "DELETE from " + DBName;
                    //sql1 = "DELETE from trns_PayrollDetails";
                    com = new SqlCommand(sql1, con);
                    com.ExecuteNonQuery();
                    foreach (var item in SalaryDetails)
                    {
                        sql2 = "INSERT INTO " + DBName + " (trns_id,employee_id,emp_id,period_id,period_start,period_end,rule_name,rule_amount,rule_acc_id,cost_center_id,division_id,employee_type,save_user_id,save_datetime) values (@TrnsID,@EmployeeID,@EmpID,@PeriodID,@PeriodStart,@PeriodEnd,@RuleName,@Amount,@AccID,@CCID,@DID,@EmployeeType,@SaveUser,@SaveDate)";
                        //sql2 = "INSERT INTO trns_PayrollDetails (trns_id,employee_id,emp_id,period_id,period_start,period_end,rule_name,rule_amount,rule_acc_id,cost_center_id,division_id,employee_type,save_user_id,save_datetime) values (@TrnsID,@EmployeeID,@EmpID,@PeriodID,@PeriodStart,@PeriodEnd,@RuleName,@Amount,@AccID,@CCID,@DID,@EmployeeType,@SaveUser,@SaveDate)";
                        com = new SqlCommand(sql2, con);

                        com.Parameters.Add("@TrnsID", System.Data.SqlDbType.Int);
                        com.Parameters.Add("@EmployeeID", System.Data.SqlDbType.UniqueIdentifier);
                        com.Parameters.Add("@EmpID", System.Data.SqlDbType.NVarChar);
                        com.Parameters.Add("@PeriodID", System.Data.SqlDbType.UniqueIdentifier);
                        com.Parameters.Add("@PeriodStart", System.Data.SqlDbType.DateTime);
                        com.Parameters.Add("@PeriodEnd", System.Data.SqlDbType.DateTime);
                        com.Parameters.Add("@RuleName", System.Data.SqlDbType.NVarChar);
                        com.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal);
                        com.Parameters.Add("@AccID", System.Data.SqlDbType.Int);
                        com.Parameters.Add("@CCID", System.Data.SqlDbType.Int);
                        com.Parameters.Add("@DID", System.Data.SqlDbType.Int);
                        com.Parameters.Add("@EmployeeType", System.Data.SqlDbType.NVarChar);
                        com.Parameters.Add("@SaveUser", System.Data.SqlDbType.UniqueIdentifier);
                        com.Parameters.Add("@SaveDate", System.Data.SqlDbType.DateTime);

                        MAXRECORDNO = MAXRECORDNO + 1;
                        EmployeeId = (Guid)item.employee_id;
                        EmpId = item.emp_id;
                        PeriodId = new Guid("00000000-0000-0000-0000-000000000000");
                        StartDate = (DateTime)item.Bonus_Period_Start_Date;
                        EndDate = (DateTime)item.Bonus_Period_End_Date;
                        RuleName = item.other_payment_name;
                        if (item.PayedBonusAmount != null)
                        {
                            RuleAmount = (decimal)item.PayedBonusAmount; 
                        }
                        else
                        {
                            RuleAmount = 0;
                        }
                        RuleACCId = 000;
                        if (item.cost_center_id != null)
                        {
                            CCId = (int)item.cost_center_id;
                        }
                        else
                        {
                            CCId = 000;
                        }
                        if (item.division_id != null)
                        {
                            DId = (int)item.division_id;
                        }
                        else
                        {
                            DId = 000;
                        }
                        if ((bool)item.isExecutive)
                        {
                            EmployeeType = "Executive"; 
                        }
                        else
                        {
                            EmployeeType = "Non-Executive"; 
                        }
                        SaveUserId = (Guid)clsSecurity.loggedUser.user_id;
                        SaveDateTime = System.DateTime.Now;

                        com.Parameters["@TrnsID"].Value = MAXRECORDNO;
                        com.Parameters["@EmployeeID"].Value = EmployeeId;
                        com.Parameters["@EmpID"].Value = EmpId;
                        com.Parameters["@PeriodID"].Value = PeriodId;
                        com.Parameters["@PeriodStart"].Value = StartDate;
                        com.Parameters["@PeriodEnd"].Value = EndDate;
                        com.Parameters["@RuleName"].Value = RuleName;
                        com.Parameters["@Amount"].Value = RuleAmount;
                        com.Parameters["@AccID"].Value = RuleACCId;
                        com.Parameters["@CCID"].Value = CCId;
                        com.Parameters["@DID"].Value = DId;
                        com.Parameters["@EmployeeType"].Value = EmployeeType;
                        com.Parameters["@SaveUser"].Value = SaveUserId;
                        com.Parameters["@SaveDate"].Value = SaveDateTime;
                        com.ExecuteNonQuery();
                        RowsAffected = RowsAffected + 1;
                    }
                    con.Close();
                    BusyBox.CloseBusy();
                    clsMessages.setMessage("Payroll Data Migrated Successfully. " + RowsAffected + " Rows Affected.");
                }
                else
                {
                    BusyBox.CloseBusy();
                    clsMessages.setMessage("No Data For The Selected Period");
                }
            }
            catch (Exception)
            {
                BusyBox.CloseBusy();
                clsMessages.setMessage("Error In Payroll Data Migration");
            }
        }
        #endregion
    }
}
