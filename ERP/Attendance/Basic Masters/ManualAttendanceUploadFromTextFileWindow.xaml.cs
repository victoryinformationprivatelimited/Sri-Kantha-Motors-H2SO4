using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace ERP.Attendance.Basic_Masters
{
    /// <summary>
    /// Interaction logic for ManualAttendanceUploadFromTextFileWindow.xaml
    /// </summary>
    public partial class ManualAttendanceUploadFromTextFileWindow : Window
    {
        #region Service Object
        private ERPServiceClient serviceClient;
        #endregion

        #region Fields
        String FilePath;
        //DataTable Textfile;
        string[] Textfile;
        Guid device_id, mode_id, verify_id;
        #endregion

        public ManualAttendanceUploadFromTextFileWindow()
        {
            InitializeComponent();
            try
            {
                savebtn.IsEnabled = false;
                serviceClient = new ERPServiceClient();
                if (!GetAppSettings())
                    browsebtn.IsEnabled = false;

            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString(), ex.Message); }
        }

        #region Browse
        private void browsebtn_Click(object sender, RoutedEventArgs e)
        {

            savebtn.IsEnabled = false;
            //Textfile = null;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".dat";
            dlg.Filter = "dat files(*.dat)|*.dat";
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                //Textfile = new string[];
                FilePath = dlg.FileName;
                filepath.Text = FilePath;
                Textfile = ReadTextFile(FilePath);
                if (Textfile != null)
                    savebtn.IsEnabled = true;
                else
                    savebtn.IsEnabled = false;
            }
        }

        private string[] ReadTextFile(string filename)
        {
            List<string> lines = new List<string>();
            try
            {
                using (System.IO.StreamReader st = System.IO.File.OpenText(filename))
                {
                    while (!st.EndOfStream)
                    {
                        lines.Add(st.ReadLine());
                    }
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Unable to Find the File", "Invalid File Name", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lines.ToArray();
        }

        #endregion

        #region Save
        private void savebtn_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private bool Save()
        {
            bool b = false, error = false;
            int x = 0;

            if (clsSecurity.GetSavePermission(317))
            {
                //return b;

                List<dtl_AttendanceData> listdtlAttendance = new List<dtl_AttendanceData>();
                dtl_AttendanceData Row;
                DateTime attendate = new DateTime();
                for (int i = 0; i < Textfile.Length; i++)
                {
                    try
                    {
                        Row = new dtl_AttendanceData();
                        Row.attendance_data_id = Guid.NewGuid();
                        Row.device_id = device_id;
                        Row.mode_id = mode_id;
                        Row.verify_id = verify_id;
                        string[] row = Textfile[i].Split('\t');
                        Row.emp_id = row[0].ToString().Trim();
                        attendate = DateTime.Parse(row[1].ToString().Trim());
                        Row.attend_datetime = attendate;
                        Row.attend_date = attendate;
                        Row.day = attendate.Day;
                        Row.month = attendate.Month;
                        Row.year = attendate.Year;
                        Row.second = attendate.Second;
                        Row.minute = attendate.Minute;
                        Row.hour = attendate.Hour;
                        Row.attend_time = attendate.TimeOfDay;//TimeSpan.Parse(attendate.ToString());
                        Row.is_manual = false;
                        Row.isdelete = false;
                        listdtlAttendance.Add(Row);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        error = true;
                        break;
                        //throw;
                    }
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
                            MessageBox.Show("File is Sucessfully Uploaded.(" + x + " records are saved)", "Save Sucessfully", MessageBoxButton.OK, MessageBoxImage.Information);
                            filepath.Text = string.Empty;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to Upload the File.(" + ex.Message + ")", "File Upload Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                return b;
            }
            else
            {
                clsMessages.setMessage("You don't have permission to Save this record(s)");
                return b;
            }
        }

        #endregion

        #region Close
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
        #endregion


        public bool GetAppSettings()
        {
            try
            {
                device_id = Guid.Parse(ConfigurationManager.AppSettings["device_id"]);
                mode_id = Guid.Parse(ConfigurationManager.AppSettings["mode_id"]);
                verify_id = Guid.Parse(ConfigurationManager.AppSettings["verify_id"]);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Find the device_id, mode_id, verify_id in App.config file.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }

        /*
       *          int x = connectionString.IndexOf('\t');
                  int y = connectionString.IndexOf('=');
                  int x1 = connectionString.IndexOf(';', x + 1);
                  int y1 = connectionString.IndexOf('=', y + 1);
                  int x2 = connectionString.IndexOf(';', x1 + 1);
                  int y2 = connectionString.IndexOf('=', y1 + 1);
                  int y3 = connectionString.IndexOf('=', y2 + 1);

                  Database = connectionString.Substring(y + 1, x - (y + 1));
                  Server = connectionString.Substring(y1 + 1, x1 - (y1 + 1));
                  User = connectionString.Substring(y2 + 1, x2 - (y2 + 1));
                  Password = connectionString.Substring(y3 + 1);
       */

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
