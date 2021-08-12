using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.Globalization;


namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    class ETFEreturnViewModel : ViewModelBase
    {
        #region ServiceClient

        ERPServiceClient serviceClient;
        string StringType = null;

        #endregion

        #region Constructor

        public ETFEreturnViewModel()
        {
            serviceClient = new ERPServiceClient();
            From = System.DateTime.Now.Date; ;
            To = System.DateTime.Now.Date;
            ETF = true;
            EPF = false;
        }
        #endregion

        #region Default Values

        string path = @"c:\ETFeReturn";
        string fullPath = "";
        string fileName1 = "HMEMTXT.txt";
        string fileName2 = "DMEMTXT.txt";
        List<GET_ETF_MONTHS_DETAIL_Result> DMEMTXT = new List<GET_ETF_MONTHS_DETAIL_Result>();

        #endregion

        #region Properties

        private DateTime _From;
        public DateTime From
        {
            get { return _From; }
            set { _From = value; OnPropertyChanged("From"); }
        }

        private DateTime _To;
        public DateTime To
        {
            get { return _To; }
            set { _To = value; OnPropertyChanged("To"); }
        }

        private bool _ETF;
        public bool ETF
        {
            get { return _ETF; }
            set { _ETF = value; OnPropertyChanged("ETF"); }
        }

        private bool _EPF;

        public bool EPF
        {
            get { return _EPF; }
            set { _EPF = value; OnPropertyChanged("EPF"); }
        }


        #endregion

        #region Commands

        #region SubFolder

        void CreatSubFolder()
        {
            try
            {
                fullPath = this.path + @"\ETF_ERETURN";
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(Path.Combine(fullPath));
                }
            }
            catch (Exception)
            {

                throw null;
            }
        }

        #endregion

        #region CheckFilePath

        void CheckPath()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    this.Genarate();
                }
                else
                {
                    this.Genarate();
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());

            }

        }

        #endregion

        #region AnalyzeButton
        public ICommand AnalyzeButton
        {
            get { return new RelayCommand(Analyze); }
        }

        void Analyze()
        {
            try
            {
                string FromDate = From.ToString("yyyy-MM-dd");
                string ToDate = To.ToString("yyyy-MM-dd");
                int r;
                int q;
                if (ETF)
                {
                    StringType = "ETF";
                    r = serviceClient.GetETFEreturnData(FromDate, ToDate, StringType);
                    if (r == 1)
                    {
                        MessageBox.Show("Analyse Complete");
                        From = System.DateTime.Now.Date; ;
                        To = From;
                    }

                    if (r == 0)
                    {
                        MessageBox.Show("Analyse Error");
                    }
                }
                else if (EPF)
                {
                    StringType = "EPF";
                    q = serviceClient.GetEPFEreturnData(FromDate, ToDate, StringType);
                    if (q == 1)
                    {
                        MessageBox.Show("Analyse Complete");
                        From = System.DateTime.Now.Date; ;
                        To = From;
                    }

                    if (q == 0)
                    {
                        MessageBox.Show("Analyse Error");
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region GenerateButton

        public ICommand GenerateButton
        {
            get { return new RelayCommand(CheckPath); }
        }

        async void Genarate()
        {

            try
            {
                DMEMTXT = (await GetETFData_Refresh()).ToList();

                CreatSubFolder();
                if (DMEMTXT != null && DMEMTXT.Count != 0)
                {

                    string Employer = DMEMTXT.FirstOrDefault().EMPLOYER_NO;
                    Employer.Insert(1, " 00");
                    decimal totalContribution = Convert.ToDecimal(DMEMTXT.Sum(c => c.TOT_CONTRIBUTE));
                    totalContribution = Decimal.Round(totalContribution, 2, MidpointRounding.AwayFromZero);
                    string startDate = DMEMTXT.FirstOrDefault().YSTART + "" + ((int)DMEMTXT.FirstOrDefault().MSTART).ToString("00");
                    string endDtae = DMEMTXT.FirstOrDefault().YEND + "" + ((int)DMEMTXT.FirstOrDefault().MEND).ToString("00");
                    int TotalMembers = DMEMTXT.Count(c => c.ETF_NO != null);

                    #region DMEMTXT Process

                    List<string> selectedList = new List<string>();

                    foreach (var item in DMEMTXT)
                    {

                        string empNo = item.ETF_NO;
                        string initials = item.INITIAL;
                        initials = initials == null ? " " : initials.Replace(".", " ");
                        string lastName = item.FNAME;
                        lastName = lastName.Replace(".", " ");
                        string NIC = item.NIC;

                        decimal contribution = Convert.ToDecimal(item.TOT_CONTRIBUTE);
                        contribution = Decimal.Round(contribution, 2, MidpointRounding.AwayFromZero);

                        selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", "D", Employer.Insert(1, " 00").TrimEnd(' '), empNo.PadLeft(6, '0'), initials.PadRight(20), lastName.PadRight(30), NIC.PadLeft(10), startDate, endDtae, (contribution.ToString().Split('.')[0] + contribution.ToString().Split('.')[1]).PadLeft(9, '0')) + Environment.NewLine);

                    }
                    if (selectedList != null && selectedList.Count != 0)
                    {
                        MessageBox.Show("DMEMTXT Generated Successfully");
                        File.Delete(this.fullPath + @"\" + this.fileName2);
                        foreach (string item in selectedList)
                        {
                            File.AppendAllText(this.fullPath + @"\" + this.fileName2, item);
                        }

                    }
                    else
                    {
                        MessageBox.Show("DMEMTXT File is not Generated");
                    }


                    #endregion

                    #region HMEMTXT Process
                    string selectedStringHMEMTXT = string.Format("{0}{1}{2}{3}{4}{5}", "H", Employer.Insert(1, " 00").TrimEnd(' '), startDate, endDtae, TotalMembers.ToString("000000"), (totalContribution.ToString().Split('.')[0] + totalContribution.ToString().Split('.')[1]).PadLeft(14, '0'));


                    if (selectedStringHMEMTXT != null)
                    {
                        File.Delete(this.fullPath + @"\" + this.fileName1);
                        File.WriteAllText(this.fullPath + @"\" + this.fileName1, selectedStringHMEMTXT);
                        MessageBox.Show("HMEMTXT Generated Successfully");
                    }



                    #endregion
                }
                else
                {
                    MessageBox.Show("Please Analyze before generate the files!");
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Genarate Error !");
            }

        }

        #endregion

        #region PrintButton


        public ICommand PrintButton
        {
            get { return new RelayCommand(Print); }
        }

        private void Print()
        {
            try
            {
                if (ETF)
                {
                    ReportPrint print = new ReportPrint("\\Reports\\Documents\\PayrollReport\\ETFEreturnReport");
                    print.PrintReportWithReportViewer();
                }

                if (EPF)
                {
                    ReportPrint print = new ReportPrint("\\Reports\\Documents\\PayrollReport\\EPFEreturnReport");
                    print.PrintReportWithReportViewer();

                }
            }
            catch (Exception)
            {


            }

        }

        #endregion
        #endregion

        #region RefreshMethods

        private async Task<IEnumerable<GET_ETF_MONTHS_DETAIL_Result>> GetETFData_Refresh()
        {
            Task<IEnumerable<GET_ETF_MONTHS_DETAIL_Result>> asyncTask = Task<IEnumerable<GET_ETF_MONTHS_DETAIL_Result>>.Factory.FromAsync(serviceClient.BeginGET_ETF_DataRows, serviceClient.EndGET_ETF_DataRows, null);
            return await asyncTask;
        }

        #endregion
    }
}
