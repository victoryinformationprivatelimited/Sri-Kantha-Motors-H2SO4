using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKTClassLibrary.HelperClass;



namespace ZKTClassLibrary
{
    public class ConnectClass
    {
        public List<DownloadDataType> tempData = new List<DownloadDataType>();

        #region U300C Fields
        int iMachineNumber = 1;
        string sdwEnrollNumber = "";
        int idwVerifyMode = 0;
        int idwInOutMode = 0;
        int idwYear = 0;
        int idwMonth = 0;
        int idwDay = 0;
        int idwHour = 0;
        int idwMinute = 0;
        int idwSecond = 0;
        int idwWorkcode = 0;
        string month, day, hour, minute, second = "";
        #endregion

        #region MA300C Fields
        int dwMachineNumber = 0;
        int dwTMachineNumber = 0;
        int dwEnrollNumber = 0;
        int dwEMachineNumber = 0;
        int dwVerifyMode = 0;
        int dwInOutMode = 0;
        //int dwVerifyMode=0;
        //int dwInOutMode=0;
        int dwYear = 0;
        int dwMonth = 0;
        int dwDay = 0;
        int dwHour = 0;
        int dwMinute = 0;
        #endregion

        public zkemkeeper.CZKEM zkem = new zkemkeeper.CZKEM();

        public ConnectClass()
        {

            zkem = ConnectionObject.ZKObject;
          
        }

        public bool ConnectToZkDivice(string IpAddress,int portno)
        {

            if (zkem.Connect_Net(IpAddress, portno))
            {
                return true;
            }
            
            return false;
        }

        public bool U300CDataDownload(int deviceNo, Guid diviceId)
        {
            try
            {
                int i = 0;
                iMachineNumber = deviceNo;

                if (zkem.ReadGeneralLogData(iMachineNumber))
                {
                    while (zkem.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, idwWorkcode))
                    {
                        DownloadDataType datatype = new DownloadDataType();
                        datatype.Attendance_data_id = Guid.NewGuid();
                        datatype.Device_id = diviceId;
                        datatype.Emp_id = sdwEnrollNumber;
                        datatype.Year = RearrangeNumber(idwYear);
                        datatype.Month = RearrangeNumber(idwMonth);
                        datatype.Day = RearrangeNumber(idwDay);
                        datatype.Hour = RearrangeNumber(idwHour);
                        datatype.Minute = RearrangeNumber(idwMinute);
                        datatype.Second = RearrangeNumber(idwSecond);
                        datatype.InOutMode = idwInOutMode;
                        datatype.VeryfyMode = idwVerifyMode;
                        tempData.Add(datatype);
                        i++;
                    }

                    if (i > 0)  //Downlaod Successful
                    {
                        return true;
                    }
                }
            }

            catch (Exception)
            {
                return false;
            }
            finally
            {
                zkem.EnableDevice(deviceNo, true);


            }
            return false;
        }


        #region Download From MA00C

        public bool MA300(int deviceNo, Guid diviceId)
        {
            tempData.Clear();
            try
            {
                int i = 0;
                iMachineNumber = deviceNo;

                if (zkem.ReadGeneralLogData(iMachineNumber))
                {
                    while (zkem.GetAllGLogData(dwMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwInOutMode,ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute))
                    {
                        DownloadDataType datetype = new DownloadDataType();
                        datetype.Attendance_data_id = Guid.NewGuid();
                        datetype.Device_id = diviceId;
                        datetype.Emp_id = dwEnrollNumber.ToString();
                        datetype.Year = dwYear.ToString();
                        datetype.Month = dwMonth.ToString();
                        datetype.Day = dwDay.ToString();
                        datetype.Hour = dwHour.ToString();
                        datetype.Minute = dwMinute.ToString();
                        datetype.Second = "0";
                        datetype.VeryfyMode = idwVerifyMode;
                        datetype.InOutMode = idwInOutMode;
                        tempData.Add(datetype);
                        i++;
                    }

                    if (i > 0)  //Downlaod Successful
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                zkem.EnableDevice(deviceNo, true);
            }
            return false;
        }

        #endregion 

        
        public List<DownloadDataType> GetDataList()
        {
                return tempData;
        }

        public bool CleareData(int deviceNo)
        {
            if (zkem.ClearGLog(deviceNo))
            {
                if (zkem.RefreshData(deviceNo))
                {
                    return true; 
                }
                return true; 
            }
            return false;
           
        }

        public bool ClerTempList()
        {
            if (tempData != null)
            {
                return true;

            }
            else
            {
                tempData.Clear();
            }
            return true;
        }

        private string RearrangeNumber(int number)
        {
            string strNumber = "";
            if (number < 10)
            {
                strNumber = "0" + number.ToString();
            }
            else
            {
                strNumber = number.ToString();
            }
            return strNumber;
        }

        public bool Disconnect()
        {
            try
            {
                zkem.Disconnect();
                return true;
            }
            catch (Exception)
            { }
            return false;
        }
       
    }
}
