using ERP.BasicSearch;
using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Reports.Documents.Bonus_Report
{
    class BonusWithPayeeViewModel : ViewModelBase
    {
        #region Fields
        private ERPServiceClient serviceClient;
        
        #endregion

        #region Constrouctor
        public BonusWithPayeeViewModel()
        {
            serviceClient = new ERPServiceClient();
            New();
        }
        #endregion

        #region Properties

        private IEnumerable<z_BonusPeriod> _BonusPeriods;

        public IEnumerable<z_BonusPeriod> BonusPeriods
        {
            get { return _BonusPeriods; }
            set { _BonusPeriods = value; OnPropertyChanged("BonusPeriods"); }
        }

        private z_BonusPeriod _CurrentBonusPeriod;
        public z_BonusPeriod CurrentBonusPeriod
        {
            get { return _CurrentBonusPeriod; }
            set { _CurrentBonusPeriod = value; OnPropertyChanged("CurrentBonusPeriod");}
        }

        private EmployeeSearchView _CurrentEmployeesForDialogBox;
        public EmployeeSearchView CurrentEmployeesForDialogBox
        {
            get { return _CurrentEmployeesForDialogBox; }
            set { _CurrentEmployeesForDialogBox = value; OnPropertyChanged("CurrentEmployeesForDialogBox");}
        }

        private bool _WithPayee;

        public bool WithPayee
        {
            get { return _WithPayee; }
            set { _WithPayee = value; OnPropertyChanged("WithPayee"); }
        }

        private bool _WithoutPayee;

        public bool WithoutPayee
        {
            get { return _WithoutPayee; }
            set { _WithoutPayee = value; OnPropertyChanged("WithoutPayee"); }
        }
        private Visibility _Visible;
        public Visibility Visible
        {
            get { return _Visible; }
            set { _Visible = value; OnPropertyChanged("Visible"); }
        }
        private Visibility _VisibleLable;

        public Visibility VisibleLable
        {
            get { return _VisibleLable; }
            set { _VisibleLable = value; OnPropertyChanged("VisibleLable"); }
        }
        
        #endregion

        #region Refresh Methods

        private void RefreshBonusPeriod()
        {
            serviceClient.GetBonusPeriodCompleted += (s, e) =>
                {
                    BonusPeriods = e.Result;
                };
            serviceClient.GetBonusPeriodAsync();
        }
        
        #endregion

        #region Button Commands
        public ICommand SelectEmployeeButton
        {
            get { return new RelayCommand(searchEmp); }
        }
        public ICommand PrintButton
        {
            get { return new RelayCommand(print); }
        }

        public ICommand NewButton
        {
            get { return new RelayCommand(New); }
        }


        
        #endregion

        #region Methods
        private void searchEmp()
        {
            EmployeeSearchWindow searchControl = new EmployeeSearchWindow();

            bool? b = searchControl.ShowDialog();
            if (b != null)
            {
                CurrentEmployeesForDialogBox = searchControl.viewModel.CurrentEmployeeSearchView;
            }
            searchControl.Close();
        }
        private void print()
        {
            if (ValidatePrint())
            {
                string path = "";
                try
                {
                    if (WithPayee)
                    {

                        path = "\\Reports\\Documents\\BonusReport\\BonusReportWithPayee";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@employeeid", CurrentEmployeesForDialogBox == null ? string.Empty : CurrentEmployeesForDialogBox.employee_id.ToString());
                        print.setParameterValue("@BonustimePeriodID", CurrentBonusPeriod == null ? string.Empty : CurrentBonusPeriod.Bonus_Period_id.ToString());
                        print.LoadToReportViewer(); 
                    }
                    else if(WithoutPayee)
                    {
                        path = "\\Reports\\Documents\\BonusReport\\BonusReportWithoutPayee";
                        ReportPrint print = new ReportPrint(path);
                        print.setParameterValue("@employeeid", CurrentEmployeesForDialogBox == null ? string.Empty : CurrentEmployeesForDialogBox.employee_id.ToString());
                        print.setParameterValue("@BonustimePeriodID", CurrentBonusPeriod == null ? string.Empty : CurrentBonusPeriod.Bonus_Period_id.ToString());
                        print.LoadToReportViewer(); 
                    }
                }
                catch (Exception ex)
                {
                    clsMessages.setMessage("Report loading is failed: " + ex.Message);
                }
            } 
        }
        private bool ValidatePrint()
        {
            if (CurrentBonusPeriod == null)
            {
                clsMessages.setMessage("Please Select A Bonus Period..");
                return false;
            }
            else if (CurrentBonusPeriod.Bonus_Period_id == 0)
            {
                clsMessages.setMessage("Please Select A Bonus Period..");
                return false;
            }
            else
                return true;
        }

        private void New()
        {
            RefreshBonusPeriod();
            CurrentBonusPeriod = new z_BonusPeriod();
            Visible = Visibility.Visible;
            WithPayee = true;
            VisibleLable = Visibility.Hidden;
        }
 
        
        #endregion
    }
}
