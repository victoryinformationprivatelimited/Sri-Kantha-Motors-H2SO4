using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows;
namespace ERP.Reports.Documents.Leave.LeaveUserControl
{
    class LeaveRegisterViewModel:ViewModelBase
    {
        #region ServiceClient

        ERPServiceClient Serviceclient;

        #endregion

        public LeaveRegisterViewModel()
        {
            Serviceclient = new ERPServiceClient();
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            RefreshEmployees();
            RefreshLeavePeriods();
            Detail = false;
            Summary = false;
            Nopay = false;
            DL = false;
        }

        #region Properties

        private IEnumerable<mas_Employee> _Employee;
        public IEnumerable<mas_Employee> Employee
        {
            get { return _Employee; }
            set { _Employee = value; OnPropertyChanged("Employee"); }
        }

        private mas_Employee _CurrentEmployee;
        public mas_Employee CurrentEmployee
        {
            get { return _CurrentEmployee; }
            set { _CurrentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }
        

        private IEnumerable<z_LeavePeriod> _LeavePeriod;
        public IEnumerable<z_LeavePeriod> LeavePeriod
        {
            get { return _LeavePeriod; }
            set { _LeavePeriod = value; OnPropertyChanged("LeavePeriod"); }
        }

        private z_LeavePeriod _CurrentLeavePeriod;

        public z_LeavePeriod CurrentLeavePeriod
        {
            get { return _CurrentLeavePeriod; }
            set { _CurrentLeavePeriod = value; OnPropertyChanged("CurrentLeavePeriod"); }
        }

        private bool _Entitlement;
        public bool Entitlement
        {
            get { return _Entitlement; }
            set { _Entitlement = value; OnPropertyChanged("Entitlement"); if (Entitlement) { Summary = false; Nopay = false; DL = false; StartDay = false;  } }
        }

        private bool _StartDay;

        public bool StartDay
        {
            get { return _StartDay; }
            set { _StartDay = value; OnPropertyChanged("StartDay"); }
        }
        

        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged("SelectDate"); }
        }

        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; OnPropertyChanged("EndDate"); }
        }

        private bool _Detail;
        public bool Detail
        {
            get { return _Detail; }
            set { _Detail = value; this.OnPropertyChanged("Detail"); if (Detail) { Summary = false; Nopay = false; DL = false; StartDay = true; } }
        }

        private bool _Summary;
        public bool Summary
        {
            get { return _Summary; }
            set { _Summary = value; this.OnPropertyChanged("Summary"); if (Summary) { Detail = false; Nopay = false; DL = false; StartDay = true; } }
        }

        private bool _Nopay;
        public bool Nopay
        {
            get { return _Nopay; }
            set { _Nopay = value; this.OnPropertyChanged("Nopay"); if (Nopay) { Detail = false; Summary = false; DL = false; StartDay = true; } }
        }

        private bool _DL;

        public bool DL
        {
            get { return _DL; }
            set { _DL = value; this.OnPropertyChanged("DL"); if (DL) { Detail = false; Summary = false; Nopay = false; StartDay = true; } }
        }
        

        #endregion

        #region Commands

        #region Print
        public ICommand PrintButton
        {
            get { return new RelayCommand(Print); }
        }

        private void Print()
        {
            try
            {

                if (Detail)
                {
                    ReportPrint print = new ReportPrint("\\Reports\\Documents\\Leave\\LeaveRegisterDetailReport");
                    print.setParameterValue("@StartDate", StartDate.Date);
                    print.setParameterValue("@EndDate", EndDate.Date);
                    if (CurrentEmployee != null)
                    {
                        print.setParameterValue("@EmpID", CurrentEmployee.employee_id.ToString());
                    }
                    else
                    {
                        //MessageBox.Show("Select a employee ID");
                        print.setParameterValue("@EmpID", "");
                    }
                    print.PrintReportWithReportViewer();
                }
                else if (Summary)
                {
                    ReportPrint print = new ReportPrint("\\Reports\\Documents\\Leave\\LeaveRegisterSummary");
                    print.setParameterValue("@StartDate", StartDate.Date);
                    print.setParameterValue("@EndDate", EndDate.Date);
                    if (CurrentEmployee != null)
                    {
                        print.setParameterValue("@EmpID", CurrentEmployee.employee_id.ToString());
                    }
                    else
                    {
                        print.setParameterValue("@EmpID", "");
                    }
                    print.PrintReportWithReportViewer();
                }

                else if(Nopay)
                {
                    ReportPrint print = new ReportPrint("\\Reports\\Documents\\Leave\\NopayDetailReport");
                    print.setParameterValue("@StartDate", StartDate.Date);
                    print.setParameterValue("@EndDate", EndDate.Date);
                    print.PrintReportWithReportViewer();

                }
                else if(DL)
                {
                    ReportPrint print = new ReportPrint("\\Reports\\Documents\\Leave\\LeaveRegisterSummary(DL)");
                    print.setParameterValue("@StartDate", StartDate.Date);
                    print.setParameterValue("@EndDate", EndDate.Date);
                    if (CurrentEmployee != null)
                    {
                        print.setParameterValue("@EmpID", CurrentEmployee.employee_id.ToString());
                    }
                    else
                    {
                        print.setParameterValue("@EmpID", "");
                    }
                    print.PrintReportWithReportViewer();
                }
                else if(Entitlement)
                {
                    if (CurrentLeavePeriod != null)
                    {
                        ReportPrint print = new ReportPrint("\\Reports\\Documents\\Leave\\CurrentLeaveEntitlementReport");
                        print.setParameterValue("@StartDate", EndDate.Date);
                        print.setParameterValue("@EndDate", CurrentLeavePeriod.to_date);
                        print.setParameterValue("@period",CurrentLeavePeriod.period_id.ToString());
                        if (CurrentEmployee != null)
                        {
                            print.setParameterValue("@EmpID", CurrentEmployee.employee_id.ToString());
                        }
                        else
                        {
                            print.setParameterValue("@EmpID", "");
                        }
                        print.PrintReportWithReportViewer();
                    }
                    else { MessageBox.Show("Select a Leave Period"); }

                }
                else 
                {
                    MessageBox.Show("Select a Report Type");
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Clear
        public ICommand ClearButton
        {
            get { return new RelayCommand(Clear); }
        }

        private void Clear()
        {
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;
            Detail = false;
            Summary = false;
            Nopay = false;
            DL = false;
        }

        #endregion

        #region Refresh Methods

        private void RefreshEmployees() 
        {
            try
            {
                this.Serviceclient.GetEmployeesCompleted += (s, e) => 
                {
                    Employee = e.Result;
                };
                this.Serviceclient.GetEmployeesAsync();
            }
            catch (Exception)
            {
                

            }
        }


        private void RefreshLeavePeriods() 
        {
            try
            {
                this.Serviceclient.GetLeavePeriodsCompleted += (s, e) => 
                {
                    LeavePeriod = e.Result;
                };

                this.Serviceclient.GetLeavePeriodsAsync(); 
            }
            catch (Exception)
            {

               
            }
        }

        #endregion

        #endregion
    }
}
