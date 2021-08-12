using ERP.ERPService;
using ERP.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ZKTClassLibrary.HelperClass;
///using ZKTClassLibrary.HelperClass;

namespace ERP.Attendance
{
    class DataDownloadViewModel : ViewModelBase
    {
        bool DiviceConection = false;
        ZKTClassLibrary.ConnectClass conect = new ZKTClassLibrary.ConnectClass();

        #region Sevice Object

        private ERPServiceClient serviceClient = new ERPServiceClient();
        public List<DownloadDataType> TempDownloadData = new List<DownloadDataType>();
        public List<dtl_AttendanceData> SaveAttendanceList = new List<dtl_AttendanceData>();

        #endregion

        #region Constructor

        public DataDownloadViewModel()
        {

            this.reafreshDevicesType();
            this.reafreshAttendanceDeviceView();
            reafreshAttendanceInOutMode();
        }

        #endregion

        #region Properties

        private IEnumerable<z_AttendanceInOutMode> _InOutModes;
        public IEnumerable<z_AttendanceInOutMode> InOutModes
        {
            get { return _InOutModes; }
            set { _InOutModes = value; }
        }

        private IEnumerable<z_DeviceModel> _Device;
        public IEnumerable<z_DeviceModel> Device
        {
            get
            {
                return this._Device;
            }
            set
            {
                this._Device = value;
                this.OnPropertyChanged("Device");
            }
        }

        private z_DeviceModel _CurrentDevice;
        public z_DeviceModel CurrentDevice
        {
            get
            {
                return this._CurrentDevice;
            }
            set
            {
                this._CurrentDevice = value;
                this.OnPropertyChanged("CurrentDevice");
                if (CurrentDevice != null)
                {
                    AttendanceDivices = AttendanceDivices.Where(z => z.device_model_id == CurrentDevice.device_model_id);

                }

            }
        }

        private IEnumerable<AttendanceDeviceView> _AttendanceDivices;
        public IEnumerable<AttendanceDeviceView> AttendanceDivices
        {
            get { return _AttendanceDivices; }
            set { _AttendanceDivices = value; this.OnPropertyChanged("AttendanceDivices"); }
        }

        private AttendanceDeviceView _CurretAttendanceDivice;
        public AttendanceDeviceView CurretAttendanceDivice
        {
            get { return _CurretAttendanceDivice; }
            set
            {
                _CurretAttendanceDivice = value; this.OnPropertyChanged("CurretAttendanceDivice");
                if (CurretAttendanceDivice != null)
                {
                    conect.Disconnect();
                }
            }
        }

        IEnumerable<dtl_AttendanceData> downloadedUserData;
        public IEnumerable<dtl_AttendanceData> DownloadedUserData
        {
            get { return downloadedUserData; }
            set { downloadedUserData = value; OnPropertyChanged("DownloadedUserData"); }
        }

        #endregion

        #region Refresh Methods

        private void reafreshDevicesType()
        {
            this.serviceClient.GetDeviceModelesCompleted += (s, e) =>
            {
                this.Device = e.Result;
            };
            this.serviceClient.GetDeviceModelesAsync();
        }

        private void reafreshAttendanceInOutMode()
        {
            this.serviceClient.GetAttendanceInOutModeCompleted += (s, e) =>
            {
                this.InOutModes = e.Result;
            };
            this.serviceClient.GetAttendanceInOutModeAsync();
        }

        private void reafreshAttendanceDeviceView()
        {
            this.serviceClient.GetAttendanceDeviceViewCompleted += (s, e) =>
            {
                this.AttendanceDivices = e.Result;
            };
            this.serviceClient.GetAttendanceDeviceViewAsync();
        }

        #endregion

        #region Button Methods

        #region Connect Button

        public ICommand ConnectToDevice
        {
            get
            {
                return new RelayCommand(Connect, ConnectCanExecute);
            }
        }

        bool ConnectCanExecute()
        {
            if (CurretAttendanceDivice != null)
            {
                if (CurretAttendanceDivice.device_name == null)
                    return false;
                if (CurretAttendanceDivice.ip_address == null)
                    return false;
                if (CurretAttendanceDivice.port == null)
                    return false;
                if (CurretAttendanceDivice.device_id == null)
                    return false;
            }
            else
            {
                return false;
            }
            return true;
        }

        private void Connect()
        {
            if (clsSecurity.GetSavePermission(314))
            {
                try
                {
                    if (conect.ConnectToZkDivice(CurretAttendanceDivice.ip_address, int.Parse(CurretAttendanceDivice.port.ToString())))
                    {
                        DiviceConection = true;
                        MessageBox.Show("Device is  connected...now you can download data");
                    }
                    else
                    {
                        MessageBox.Show("Can't Connect to the Device");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't Connect to the Device" + " " + ex.Message);

                }
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Connect to the device");
            }

        }

        #endregion

        #region Download Button

        public ICommand DataDownload
        {
            get
            {
                return new RelayCommand(DownloadData, DownloadDataCanExecute);
            }
        }

        public ICommand ClearData
        {
            get
            {
                return new RelayCommand(ClearDeviceData, DownloadDataCanExecute);
            }
        }



        private bool DownloadDataCanExecute()
        {
            if (DiviceConection == true)
            {
                return true;
            }
            else
                return false;
        }

        private void DownloadData()
        {
            //if (clsSecurity.GetPermissionForSave(clsConfig.GetViewModelId(Viewmodels.DataDownload), clsSecurity.loggedUser.user_id))
            //{
                if (CurrentDevice.model_name == "U-300-C")
                {
                    try
                    {
                        if (conect.U300CDataDownload(int.Parse(CurretAttendanceDivice.device_number2.ToString()), CurretAttendanceDivice.device_id))
                        {
                            TempDownloadData = conect.GetDataList();
                            SaveDownloadedData();
                            conect.ClerTempList();
                        }
                        else
                        {
                            MessageBox.Show("Data download Error");
                        }
                    }
                    catch (Exception exx)
                    {

                        MessageBox.Show("Data download Error" + " " + exx.Message);
                    }
                }

                if (CurrentDevice.model_name == "MA-300")
                {
                    try
                    {
                        if (conect.MA300(int.Parse(CurretAttendanceDivice.device_number2.ToString()), CurretAttendanceDivice.device_id))
                        {
                            TempDownloadData = conect.GetDataList();
                            SaveDownloadedData();
                            conect.ClerTempList();
                        }
                        else
                        {
                            MessageBox.Show("Data download Error");
                        }
                    }
                    catch (Exception exx)
                    {

                        MessageBox.Show("Data download Error" + " " + exx.Message);
                    }
                }
            //}
            //else
            //{
            //    clsMessages.setMessage("No Permission For Connect to the Device");
            //}
        }

        #endregion

        #region Clear Button

        public ICommand ClearButton
        {
            get { return new RelayCommand(Clear); }
        }

        private void Clear()
        {
            DownloadedUserData = null;
            if (SaveAttendanceList.Count > 0)
            {
                SaveAttendanceList.Clear();
            }
        }

        #endregion

        #endregion

        public bool SaveUserDownloadedData()
        {
            return true;
        }

        public void SaveDownloadedData()
        {
            if (TempDownloadData.Count > 0)
            {
                CreateSaveDataList();
            }
            else
            {
                MessageBox.Show("No Data in Attendance Device");
            }

        }

        private void ClearDeviceData()
        {
            MessageBox.Show("Do you want to clear device data?", "Really?", MessageBoxButtons.YesNo);
            conect.CleareData(int.Parse(CurretAttendanceDivice.device_number2.ToString()));
            MessageBox.Show("Device data deleted successfully ");
        }

        public void SaveDataToDataBase()
        {
            if (SaveAttendanceList.Count > 0)
            {
                if (clsSecurity.GetSavePermission(314))
                {
                    bool can_save = false;
                    List<dtl_AttendanceData> listattendance = new List<dtl_AttendanceData>();
                    try
                    {
                        //status.Content = "Saving Data....";
                        if (serviceClient.StartSaveAttendanceData())
                        {
                            int x = 0;
                            foreach (var item in SaveAttendanceList)
                            {
                                listattendance.Add(item);
                                x++;
                                if (x >= 50)
                                {
                                    if (serviceClient.AddSaveAttendanceItem(listattendance.ToArray()))
                                    {
                                        x = 0;
                                        can_save = true;
                                        listattendance.Clear();
                                    }
                                    else
                                    {
                                        can_save = false;
                                        break;
                                    }
                                }
                            }
                            if (x != 0 && listattendance.Count != 0)
                                can_save = serviceClient.AddSaveAttendanceItem(listattendance.ToArray());
                            if (can_save)
                            {
                                if (serviceClient.CommitSaveAttendanceData())
                                {
                                    // Due to TimeAttendance Software also using same data then clearing of data not allowed
                                    // conect.CleareData(int.Parse(CurretAttendanceDivice.device_number2.ToString()));
                                    MessageBox.Show("Data are downloaded successfully");
                                    //status.Content = "Data Download Completed";
                                    // Download.IsEnabled = false;
                                    //device.Disconnect();
                                }
                                else
                                {
                                    MessageBox.Show("Data download is failed");
                                    // status.Content = "Save Fail";
                                }
                            }
                            else
                                MessageBox.Show("Unable to upload all data to server");

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("System Error " + ex.Message);
                        //status.Content = "Save Fail";
                    }
                    finally
                    {
                        try
                        {
                            DiviceConection = false;
                            conect.Disconnect();
                        }
                        catch (Exception)
                        { }
                    }

                    // return false; 
                }
                else
                    clsMessages.setMessage("You don't have permission to Update this record(s)");
            }
            else
            {
                MessageBox.Show("No Data in Save Data List");
            }
        }

        #region Data Setting Methods

        void ViewDownloadedData()
        {
            if (SaveAttendanceList.Count > 0)
            {
                DownloadedUserData = null;
                DownloadedUserData = SaveAttendanceList;
            }
        }

        public void CreateSaveDataList()
        {
            foreach (var item in TempDownloadData)
            {
                dtl_AttendanceData TempObject = new dtl_AttendanceData();
                TempObject.attendance_data_id = item.Attendance_data_id;
                TempObject.device_id = item.Device_id;
                TempObject.emp_id = item.Emp_id;
                TempObject.year = int.Parse(item.Year);
                TempObject.day = int.Parse(item.Day);
                TempObject.month = int.Parse(item.Month);
                TempObject.hour = int.Parse(item.Hour);
                TempObject.minute = int.Parse(item.Minute);
                TempObject.second = int.Parse(item.Second);
                TempObject.attend_datetime = new DateTime(int.Parse(item.Year), int.Parse(item.Month), int.Parse(item.Day), int.Parse(item.Hour), int.Parse(item.Minute), int.Parse(item.Second));
                TempObject.attend_date = new DateTime(TempObject.year, TempObject.month, TempObject.day);
                TempObject.attend_time = new TimeSpan(TempObject.hour, TempObject.minute, TempObject.second);
                TempObject.isdelete = false;
                TempObject.is_manual = false;
                if (InOutModes != null)
                {
                    foreach (var mode_item in InOutModes.ToList())
                    {
                        if (mode_item.value == item.InOutMode)
                        {
                            TempObject.mode_id = mode_item.mode_id;
                        }
                    }
                }
                else
                {
                    TempObject.mode_id = Guid.Parse("00000000-0000-0000-0000-000000000010");
                }
                TempObject.verify_id = Guid.Empty;
                TempObject.save_user_id = clsSecurity.loggedUser.user_id;
                TempObject.save_datetime = System.DateTime.Now;
                TempObject.isdelete = false;
                //TempObject.modified_user_id = clsSecurity.loggedUser.user_id;
                //TempObject.modified_datetime = System.DateTime.Now;
                //TempObject.delete_user_id = clsSecurity.loggedUser.user_id;
                //TempObject.delete_datetime = System.DateTime.Now;
                SaveAttendanceList.Add(TempObject);
            }

            this.ViewDownloadedData();
            SaveDataToDataBase();
        }

        #endregion

    }
}
