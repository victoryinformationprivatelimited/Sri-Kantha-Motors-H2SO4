using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Attendance.Attendance_Process
{
    public class ModifyAttendanceViewmodel : ViewModelBase
    {
        #region  Srevice Reference

        ERPServiceClient serviceClient;

        #endregion

        #region Window

        ModifyAttendanceApprovalWindow ApprovalWindow;

        #endregion

        #region Constructor

        public ModifyAttendanceViewmodel()
        {
            serviceClient = new ERPServiceClient();
            ResfreshUsers();
            ResfreshEmployeeManagers();
            ResfreshModifiedAttendance();
            RefreshApprovedAttendance();
            EnableCurrentDate = false;
            EnableDateDown = false;
            EnableDateUp = false;
        }

        #endregion

        #region Lists

        public List<EmployeeAttendTimeView> ListAllData = new List<EmployeeAttendTimeView>();

        #endregion

        #region Refresh Methods

        private async Task<IEnumerable<EmployeeAttendTimeView>> referesh()
        {
            Task<IEnumerable<EmployeeAttendTimeView>> asyncTask = Task<IEnumerable<EmployeeAttendTimeView>>.Factory.FromAsync(serviceClient.BeginGetExistingAttendance, serviceClient.EndGetExistingAttendance,FromDate,Todate, null);
            return await asyncTask;
        }

        private void RefreshApprovedAttendance() 
        {
            serviceClient.GetEmployeeApprovedAttendanceViewCompleted += (s, e) =>
            {
                ApproveAttendance = e.Result;
            };
            serviceClient.GetEmployeeApprovedAttendanceViewAsync();
        }

        private void ResfreshUsers()
        {
            serviceClient.GetUserEmployeesCompleted += (s, e) =>
            {
                UserEmployee = e.Result;
            };
            serviceClient.GetUserEmployeesAsync();
        }

        private void ResfreshEmployeeManagers()
        {
            serviceClient.GetEmployeeManagerCompleted += (s, e) =>
            {
                EmployeeManager = e.Result;
            };
            serviceClient.GetEmployeeManagerAsync();
        }

        private async void RefreshEmployee()
        {
            if (FromDate == null || Todate == null)
                MessageBox.Show("Either 'From Date' or 'To Date' is not Selected", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                Guid ID;
                try
                {
                    ID = (Guid)UserEmployee.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;
                }
                catch (Exception)
                {

                    ID = Guid.Empty;
                }

                if (ID != Guid.Empty)
                {
                    FilteredEmployeeManager = EmployeeManager.Where(i => i.supervisor_employee_id == ID);
                }
                AllData = await referesh();
                
                if (AllData != null && AllData.Count() > 0)
                {
                    ListAllData.Clear();
                    foreach (dtl_EmployeeSupervisor empMgr in FilteredEmployeeManager.ToList())
                    {
                        ListAllData.AddRange(AllData.Where(c => c.employee_id == empMgr.employee_id));
                    }

                    if (CurrentDate == null || (CurrentDate < FromDate || CurrentDate > Todate))
                    CurrentDate = FromDate;
                    FilteredData = ListAllData.Where(c => c.attend_date == CurrentDate);
                    EnableDateSelection();
                }
                else 
                {
                    MessageBox.Show("No Data For The Selected Date Range", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void ResfreshModifiedAttendance()
        {
            serviceClient.GetModifedAttendanceCompleted += (s, e) =>
            {
                ModifiedAttendData = e.Result;
            };
            serviceClient.GetModifedAttendanceAsync();
        }



        #endregion

        #region Properties

        private IEnumerable<usr_UserEmployee> userEmployee;
        public IEnumerable<usr_UserEmployee> UserEmployee
        {
            get { return userEmployee; }
            set { userEmployee = value; OnPropertyChanged("UserEmployee"); }
        }


        private IEnumerable<EmployeeAttendTimeView> allData;
        public IEnumerable<EmployeeAttendTimeView> AllData
        {
            get { return allData; }
            set { allData = value; OnPropertyChanged("AllData"); }
        }
        
        private IEnumerable<EmployeeAttendTimeView> filteredData;
        public IEnumerable<EmployeeAttendTimeView> FilteredData
        {
            get { return filteredData; }
            set { filteredData = value; OnPropertyChanged("FilteredData"); }
        }

        private IEnumerable<EmployeeAttendTimeView> saveData;
        public IEnumerable<EmployeeAttendTimeView> SaveData
        {
            get { return saveData; }
            set { saveData = value; OnPropertyChanged("SaveData"); }
        }

        private EmployeeAttendTimeView currentEmployee;
        public EmployeeAttendTimeView CurrentEmployee
        {
            get { return currentEmployee; }
            set { currentEmployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private IEnumerable<dtl_EmployeeSupervisor> employeeManager;
        public IEnumerable<dtl_EmployeeSupervisor> EmployeeManager
        {
            get { return employeeManager; }
            set { employeeManager = value; OnPropertyChanged("EmployeeManager"); }
        }

        private IEnumerable<dtl_EmployeeSupervisor> filteredEmployeeManager;
        public IEnumerable<dtl_EmployeeSupervisor> FilteredEmployeeManager
        {
            get { return filteredEmployeeManager; }
            set { filteredEmployeeManager = value; OnPropertyChanged("FilteredEmployeeManager"); }
        }

        private IEnumerable<trns_EmployeeAttendTime> modifiedAttendData;
        public IEnumerable<trns_EmployeeAttendTime> ModifiedAttendData
        {
            get { return modifiedAttendData; }
            set { modifiedAttendData = value; OnPropertyChanged("ModifiedAttendData"); }
        }

        private IEnumerable<EmployeeAprrovedAttendanceView> approveAttendance;
        public IEnumerable<EmployeeAprrovedAttendanceView> ApproveAttendance
        {
            get { return approveAttendance; }
            set { approveAttendance = value; OnPropertyChanged("ApproveAttendance"); }
        }
        

        private List<EmployeeAprrovedAttendanceView> filteredApproveAttendance = new List<EmployeeAprrovedAttendanceView>();
        public List<EmployeeAprrovedAttendanceView> FilteredApproveAttendance
        {
            get { return filteredApproveAttendance; }
            set { filteredApproveAttendance = value; OnPropertyChanged("FilteredApproveAttendance");}
        }

        private EmployeeAprrovedAttendanceView currentapproveAttendance;
        public EmployeeAprrovedAttendanceView CurrentapproveAttendance
        {
            get { return currentapproveAttendance; }
            set { currentapproveAttendance = value; OnPropertyChanged("CurrentapproveAttendance"); if(CurrentapproveAttendance!=null) CheckApproval(); }
        }
        
        
        
        private DateTime? fromDate;
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { fromDate = value; OnPropertyChanged("FromDate"); }
        }

        private DateTime? todate;
        public DateTime? Todate
        {
            get { return todate; }
            set { todate = value; OnPropertyChanged("Todate"); }
        }

        private DateTime? currentDate;
        public DateTime? CurrentDate
        {
            get { return currentDate; }
            set { currentDate = value; OnPropertyChanged("CurrentDate"); if (AllData != null && FilteredData != null) DateSelectionChanged(); }
        }

        private bool enableDateDown;
        public bool EnableDateDown
        {
            get { return enableDateDown; }
            set { enableDateDown = value; OnPropertyChanged("EnableDateDown"); }
        }

        private bool enableDateUp;
        public bool EnableDateUp
        {
            get { return enableDateUp; }
            set { enableDateUp = value; OnPropertyChanged("EnableDateUp"); }
        }

        private bool enableCurrentDate;
        public bool EnableCurrentDate
        {
            get { return enableCurrentDate; }
            set { enableCurrentDate = value; OnPropertyChanged("EnableCurrentDate"); }
        }



        #endregion

        #region Button Commands

        public ICommand FillEmployee
        {
            get { return new RelayCommand(RefreshEmployee); }
        }

        public ICommand DateDown
        {
            get { return new RelayCommand(DecreaseDate); }
        }

        public ICommand DateUp
        {
            get { return new RelayCommand(IncreaseDate); }
        }

        public ICommand SaveEmployee
        {
            get { return new RelayCommand(SaveChanges,SaveCanExecute); }
        }

        public ICommand SaveApproval 
        {
            get { return new RelayCommand(SaveApprovedAttendance); }
        }

        #endregion

        #region Methods


        private void DecreaseDate()
        {
            if (CurrentDate.Value.AddDays(-1) < FromDate || CurrentDate > Todate)
                MessageBox.Show("Please Select a Date Within the Selected Range", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
                CurrentDate = CurrentDate.Value.AddDays(-1);
        }


        private void IncreaseDate()
        {
            if (CurrentDate.Value.AddDays(1) > Todate || CurrentDate < FromDate)
                MessageBox.Show("Please Select a Date Within the Selected Range", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
                CurrentDate = CurrentDate.Value.AddDays(1);

        }


        private void DateSelectionChanged()
        {
            if (CurrentDate != null)
            {
                if (CurrentDate < FromDate || CurrentDate > Todate) 
                {
                    MessageBox.Show("Please Select a Date Within the Selected Range", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    FilteredData = ListAllData.Where(c => c.attend_date == CurrentDate);
                }
                    
                else
                    FilteredData = ListAllData.Where(c => c.attend_date == CurrentDate);
            }

        }


        private void FilterManagers()
        {
            //FilteredEmployeeManager = EmployeeManager.Where(c => c.Employee_ID == empID);
            //if (FilteredEmployeeManager.Count() > 0)
            //{
            //    return FilteredEmployeeManager;
            //}
            //else
            //    return null;          
        }


        public void ShowApprovalWindow() 
        {
            if (ApproveAttendance != null)
            {
                FilteredApproveAttendance.Clear();
                if(CurrentEmployee!=null)
                FilteredApproveAttendance = ApproveAttendance.Where(c => c.AttendDate == CurrentEmployee.attend_date && c.Employee_ID == CurrentEmployee.employee_id ).ToList();
                if (FilteredApproveAttendance != null && FilteredApproveAttendance.Count() == 0)
                    MessageBox.Show("This employee doesn't have any Modified Attendance Data", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else 
                {
                    ApprovalWindow = new ModifyAttendanceApprovalWindow(this);
                    ApprovalWindow.ShowDialog();
                }
                
            }
        }


        private void EnableDateSelection()
        {
            EnableCurrentDate = true;
            EnableDateDown = true;
            EnableDateUp = true;
        }


        private void SaveChanges()
        {
            if (FilteredData.Count() > 0)
            {
                int itemSaveCount = 0;
                int itemUpdateCount = 0;
                SaveData = FilteredData.Where(c => c.modified_actual_ot_intime != null && c.modified_actual_ot_intime != TimeSpan.Zero && c.modified_actual_ot_intime != c.actual_ot_intime || c.modified_actual_ot_outtime != null && c.modified_actual_ot_outtime != TimeSpan.Zero && c.modified_actual_ot_outtime != c.actual_ot_outtime);
                if (SaveData.Count() == 0)
                    MessageBox.Show("Please Modify the Records And Try Again", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else 
                {
                    foreach (var item in SaveData)
                    {
                        if (ModifiedAttendData != null)
                        {
                            bool isUpdate = ModifiedAttendData.Where(c => c.employee_id == item.employee_id && c.period_id == item.period_id && c.attend_date == item.attend_date && c.in_time == item.in_time && c.out_time == item.out_time && c.roster_header_id == item.roster_header_id).Count() == 1;

                            if (!isUpdate)
                            {
                                trns_EmployeeAttendTime saveObj = new trns_EmployeeAttendTime();
                                saveObj.employee_id = item.employee_id;
                                saveObj.period_id = item.period_id;
                                saveObj.attend_date = item.attend_date;
                                saveObj.in_time = item.in_time;
                                saveObj.out_time = item.out_time;
                                saveObj.roster_header_id = item.roster_header_id;
                                saveObj.actual_ot_intime = (TimeSpan)item.actual_ot_intime;
                                saveObj.actual_ot_outtime = (TimeSpan)item.actual_ot_outtime;
                                saveObj.modified_actual_ot_intime = item.modified_actual_ot_intime;
                                saveObj.modified_actual_ot_outtime = item.modified_actual_ot_outtime;

                                FilteredEmployeeManager = EmployeeManager.Where(c => c.employee_id == item.employee_id);
                                if (FilteredEmployeeManager != null && FilteredEmployeeManager.Count() > 0)
                                {
                                    List<trns_EmployeeApprovedAttendance> savePermissions = new List<trns_EmployeeApprovedAttendance>();
                                    foreach (var mgr in FilteredEmployeeManager)
                                    {
                                        trns_EmployeeApprovedAttendance savePermissionsObj = new trns_EmployeeApprovedAttendance();
                                        savePermissionsObj.Employee_ID = item.employee_id;
                                        savePermissionsObj.Manager_ID = mgr.supervisor_employee_id;
                                        savePermissionsObj.AttendDate = item.attend_date;
                                        savePermissionsObj.Is_Approved = false;

                                        savePermissions.Add(savePermissionsObj);
                                    }

                                    saveObj.is_approved = false;

                                    if (serviceClient.SaveModifiedAttendance(saveObj, savePermissions.ToArray()) == true)
                                        itemSaveCount++;
                                    else
                                        MessageBox.Show("save failed");
                                }
                                else
                                {
                                    List<trns_EmployeeApprovedAttendance> savePermissions = new List<trns_EmployeeApprovedAttendance>();
                                    saveObj.is_approved = true;
                                    if (serviceClient.SaveModifiedAttendance(saveObj, savePermissions.ToArray()) == true)
                                        itemSaveCount++;
                                    else
                                        MessageBox.Show("save failed");
                                }
                            }

                            else
                            {
                                if (ModifiedAttendData.Where(c => c.employee_id == item.employee_id && c.period_id == item.period_id && c.attend_date == item.attend_date && c.in_time == item.in_time && c.out_time == item.out_time && c.roster_header_id == item.roster_header_id && (c.modified_actual_ot_intime != item.modified_actual_ot_intime || c.modified_actual_ot_outtime != item.modified_actual_ot_outtime)).Count() == 1)
                                {
                                    trns_EmployeeAttendTime saveObj = new trns_EmployeeAttendTime();
                                    saveObj.employee_id = item.employee_id;
                                    saveObj.period_id = item.period_id;
                                    saveObj.attend_date = item.attend_date;
                                    saveObj.in_time = item.in_time;
                                    saveObj.out_time = item.out_time;
                                    saveObj.roster_header_id = item.roster_header_id;
                                    saveObj.actual_ot_intime = (TimeSpan)item.actual_ot_intime;
                                    saveObj.actual_ot_outtime = (TimeSpan)item.actual_ot_outtime;
                                    saveObj.modified_actual_ot_intime = item.modified_actual_ot_intime;
                                    saveObj.modified_actual_ot_outtime = item.modified_actual_ot_outtime;
                                    saveObj.is_approved = item.is_approved;

                                    if (serviceClient.UpdateModifiedAttendance(saveObj) == true)
                                        itemUpdateCount++;
                                    else
                                        MessageBox.Show("Update failed");
                                }

                            }
                        }
                    }

                    RefreshEmployee();
                    ResfreshModifiedAttendance();
                    RefreshApprovedAttendance();

                    if (itemUpdateCount == 0 && itemSaveCount > 0)
                        MessageBox.Show(itemSaveCount.ToString() + " " + "New Records Saved Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    else if (itemSaveCount == 0 && itemUpdateCount > 0)
                        MessageBox.Show(itemUpdateCount.ToString() + " " + "Existing Records Updated Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    else if (itemSaveCount == 0 && itemUpdateCount == 0)
                        MessageBox.Show(itemUpdateCount.ToString() + " " + "No Changes Were Made", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show(itemSaveCount.ToString() + " " + "New Records Saved And" + "\n" + itemUpdateCount.ToString() + " " + "Existing Records Updated Successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    itemUpdateCount = 0;
                    itemSaveCount = 0;
                }
                
            }

        }


        private void SaveApprovedAttendance()
        {
            Guid ID;
            EmployeeAprrovedAttendanceView FilteredSaveApproval = new EmployeeAprrovedAttendanceView();

                try
                {
                    ID = (Guid)UserEmployee.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;
                }
                catch (Exception)
                {

                    ID = Guid.Empty;
                }

                if (ID != Guid.Empty)
                {
                    bool flag = false;

                    FilteredSaveApproval = FilteredApproveAttendance.FirstOrDefault(c => c.Manager_ID == ID);
                    if (serviceClient.UpdateModifiedApprovedAttendance(FilteredSaveApproval))
                    {
                        FilteredApproveAttendance.Clear();
                        RefreshApprovedAttendance();
                        FilteredApproveAttendance = ApproveAttendance.Where(c => c.AttendDate == CurrentEmployee.attend_date && c.Employee_ID == CurrentEmployee.employee_id).ToList();

                        trns_EmployeeAttendTime saveObj = new trns_EmployeeAttendTime();
                        saveObj.employee_id = currentEmployee.employee_id;
                        saveObj.period_id = currentEmployee.period_id;
                        saveObj.attend_date = currentEmployee.attend_date;
                        saveObj.in_time = currentEmployee.in_time;
                        saveObj.out_time = currentEmployee.out_time;
                        saveObj.roster_header_id = currentEmployee.roster_header_id;
                        saveObj.actual_ot_intime = (TimeSpan)currentEmployee.actual_ot_intime;
                        saveObj.actual_ot_outtime = (TimeSpan)currentEmployee.actual_ot_outtime;
                        saveObj.modified_actual_ot_intime = currentEmployee.modified_actual_ot_intime;
                        saveObj.modified_actual_ot_outtime = currentEmployee.modified_actual_ot_outtime;

                        if ((FilteredApproveAttendance.Count() == ApproveAttendance.Where(c => c.AttendDate == CurrentEmployee.attend_date && c.Employee_ID == CurrentEmployee.employee_id && c.Is_Approved == true).Count()) && ApproveAttendance.Where(c => c.AttendDate == CurrentEmployee.attend_date && c.Employee_ID == CurrentEmployee.employee_id && c.Is_Approved == true).Count() != 0)
                        {                    
                            if (CurrentEmployee.is_approved == false || CurrentEmployee.is_approved == null)
                            {
                                saveObj.is_approved = true;
                             if(serviceClient.UpdateModifiedAttendance(saveObj))
                                MessageBox.Show("Records Modified Successfully", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                             flag = true;
                            }
                        }
                        
                        else if (CurrentEmployee.is_approved == true)
                        {
                            saveObj.is_approved = false;
                            if(serviceClient.UpdateModifiedAttendance(saveObj))
                                MessageBox.Show("Records Modified Successfully", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            flag = true;
                        }

                        if(flag == false)
                        MessageBox.Show("Records Modified Successfully", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        ApprovalWindow.Close();
                        RefreshEmployee();
                        ResfreshModifiedAttendance();
                    }
                }
            
        }


        private void CheckApproval() 
        {
            Guid ID;
            try
            {
                ID = (Guid)UserEmployee.FirstOrDefault(c => c.user_id == clsSecurity.loggedUser.user_id).employee_id;
            }
            catch (Exception)
            {
                ID = Guid.Empty;
            }

            if (CurrentapproveAttendance.Manager_ID != ID ) 
            {
                MessageBox.Show("You don't have Permission for this Approval","",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                ApprovalWindow.Close();
                RefreshApprovedAttendance();
            }
               
        }


        private bool SaveCanExecute()
        {
            if (FilteredData != null && FilteredData.Count() > 0)
                return true;
            else
                return false;
        }


        #endregion
    }
}
