using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP.ERPService;

using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Globalization;

namespace ERP.Attendance.Basic_Masters
{
    /// <summary>
    /// Interaction logic for ManualAttendanceUploadUserControl.xaml
    /// </summary>
    public partial class ManualAttendanceUploadUserControl : UserControl
    {
        //ManualAttendanceUploadViewModel viewModel;

        #region Service Object
        private ERPServiceClient serviceClient;
        #endregion

        #region Fields
        String FilePath;
        DataTable CSVFile;
        Guid device_id, mode_id, verify_id;
        #endregion

        #region Constructor
        public ManualAttendanceUploadUserControl()
        {
            InitializeComponent();
            try
            {
                savebtn.IsEnabled = false;
                serviceClient = new ERPServiceClient();
                if (!GetAppSettings())
                    browsebtn.IsEnabled = false;
                //viewModel = new ManualAttendanceUploadViewModel();
                //this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString(),ex.Message); }
        }
        #endregion

        #region Events
        private void browsebtn_Click(object sender, RoutedEventArgs e)
        {
            if (clsSecurity.GetPermissionForView(clsConfig.GetViewModelId(Viewmodels.AttendanceManualUpload), clsSecurity.loggedUser.user_id))
            {
                savebtn.IsEnabled = false;
                CSVFile = null;
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".csv";
                dlg.Filter = "csv files(*.csv)|*.csv"; ;
                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    FilePath = dlg.FileName;
                    filepath.Text = FilePath;
                    CSVFile = new DataTable();
                    CSVFile = ReadCSVFile(FilePath);
                    if (CSVFile != null)
                        savebtn.IsEnabled = true;
                    //String x = "";
                    /*try
                    {
                        for (int i = 0; i < CSVFile.Rows.Count; i++)
                        {
                            x += CSVFile.Rows[i][1] + " / ";
                        }
                        //tx2.Text = x;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }*/
                }
            }
            else
            {
                clsMessages.setMessage(Properties.Resources.NoPermissionForView);
            }
        }

        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            if (CSVFile != null)
            {
                Save();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
        #endregion

        #region Methods
        private DataTable ReadCSVFile(string filepath)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(filepath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn tablecolumn = new DataColumn(column);
                        tablecolumn.AllowDBNull = true;
                        csvData.Columns.Add(tablecolumn);
                    }

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                                fieldData[i] = null;
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),ex.Message);
            }
            return csvData;
        }

        private bool Save()
        {
            bool b = false, error = false;
            int x = 0;
            if (CSVFile != null)
            {
                List<dtl_AttendanceData> listdtlAttendance = new List<dtl_AttendanceData>();
                dtl_AttendanceData firstRow, secondRow;
                DateTime attime = new DateTime();

                try
                {
                    for (int i = 0; i < CSVFile.Rows.Count; i++)
                    {
                        firstRow = new dtl_AttendanceData();
                        secondRow = new dtl_AttendanceData();

                        firstRow.attendance_data_id = Guid.NewGuid();
                        secondRow.attendance_data_id = Guid.NewGuid();

                        firstRow.isdelete = secondRow.isdelete = false;
                        firstRow.emp_id = secondRow.emp_id = CSVFile.Rows[i][1].ToString();

                        try
                        {
                            //DateTime thisDate = new DateTime(CSVFile.Rows[i][0]);
                            //attime = DateTime.Parse( CSVFile.Rows[i][0].ToString());
                             attime = DateTime.ParseExact(CSVFile.Rows[i][0].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                            //attime.GetDateTimeFormats();
                            firstRow.year = secondRow.year = attime.Year;
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Unable to process the data. Please Change your System datetime.(" + ex.Message+")", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                            error = true;
                            break;
                        }
                        firstRow.month = secondRow.month = attime.Month;
                        firstRow.day = secondRow.day = attime.Day;
                        firstRow.attend_date = secondRow.attend_date = attime;

                        firstRow.attend_datetime = DateTime.Parse(attime.ToShortDateString() + " " + CSVFile.Rows[i][4].ToString());
                        secondRow.attend_datetime = DateTime.Parse(attime.ToShortDateString() + " " + CSVFile.Rows[i][5].ToString());

                        firstRow.attend_time = TimeSpan.Parse(CSVFile.Rows[i][4].ToString());
                        secondRow.attend_time = TimeSpan.Parse(CSVFile.Rows[i][5].ToString());

                        firstRow.device_id = secondRow.device_id = device_id;
                        firstRow.mode_id = secondRow.mode_id = mode_id;
                        firstRow.verify_id = secondRow.verify_id = verify_id;

                        listdtlAttendance.Add(firstRow);
                        listdtlAttendance.Add(secondRow);
                        x++;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to process the data. Please Change your System datetime.(" + ex.Message+")", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (!error)
                {
                    if (MessageBoxResult.OK == MessageBox.Show("Are you sure, you want to upload data? ", "", MessageBoxButton.OKCancel, MessageBoxImage.Question))
                    {
                        try
                        {
                            int y = 0, count = 0;
                            List<dtl_AttendanceData> saveList = new List<dtl_AttendanceData>();
                            foreach (var item in listdtlAttendance)
                            {
                                saveList.Add(item);
                                y++;
                                if (y >= 250)
                                {
                                    count = serviceClient.SaveManualAttendanceUpload(saveList.ToArray());
                                    y = 0;
                                    saveList.Clear();
                                }
                            }
                            if (y < 250 && saveList.Count != 0)
                                count = serviceClient.SaveManualAttendanceUpload(saveList.ToArray());
                            //if (count == 1)
                                MessageBox.Show("File is Successfully Uploaded.("+x+" records are saved)", "Save Sucessfully", MessageBoxButton.OK, MessageBoxImage.Information);
                            //else
                                //MessageBox.Show("Unable to complete the Process.", "Unable to complete the Process", MessageBoxButton.OK, MessageBoxImage.Error);
                            //count = serviceClient.SaveManualAttendanceUpload(listdtlAttendance.ToArray());
                            //if (x * 2 == count)
                            //    MessageBox.Show("File is Sucessfully Uploaded.", "Save Sucessfully", MessageBoxButton.OK, MessageBoxImage.Information);
                            //else
                            //    MessageBox.Show(x + " Out of " + count / 2 + " records Sucessfully Uploaded.", "Unable to complete the Process", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to Upload the File.("+ex.Message+")", "File Upload Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            return b;
        }

        public bool GetAppSettings()
        {
            try
            {
                device_id =Guid.Parse(ConfigurationManager.AppSettings["device_id"]);
                mode_id = Guid.Parse(ConfigurationManager.AppSettings["mode_id"]);
                verify_id= Guid.Parse(ConfigurationManager.AppSettings["verify_id"]);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Find the device_id, mode_id, verify_id in App.config file.",ex.Message,MessageBoxButton.OK,MessageBoxImage.Error);
            }
            return false;
        }
        #endregion
    }
}