using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using SAPBusinessObjects.WPF.Viewer;
using System.Data;
using ERP.Reports.Documents.Attendance;
using ERP.ERPService;
using ERP.HelperClass;



namespace ERP
{
    /// <summary>
    /// Interaction logic for ReportViewer.xaml
    /// </summary>
    public partial class ReportViewer : Window
    {
        private ERPServiceClient serviceClient;
        //object report;
        public ReportViewer()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            serviceClient = new ERPServiceClient();
        }

        #region Reporting Method II

        object report;
        bool print;

        public ReportViewer(object report, bool print)
        {
            this.report = new object();
            this.report = report;
            this.print = print;
            InitializeComponent();
            CristalReportViewer1.Loaded += (s, e) => { if (print) this.reportPrint(); else this.reportLoad(); };
        }

        public void reportPrint()
        {
            CristalReportViewer1.ViewerCore.ReportSource = report;
            CristalReportViewer1.ViewerCore.PrintReport();
        }
        public void reportLoad()
        {
            CristalReportViewer1.ViewerCore.ReportSource = report;
        }
        #endregion


        //public ReportViewer(string[,] a, string foldername, string reportname)
        //{
        //    string staeredpat = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        //    string connectionString = ConfigurationManager.ConnectionStrings["ERPConnectionString"].ConnectionString;
        //    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
        //    ConnectionInfo crConnectionInfo = new ConnectionInfo();
        //    crConnectionInfo.ServerName = builder.DataSource;
        //    crConnectionInfo.DatabaseName = builder.InitialCatalog;
        //    crConnectionInfo.UserID = builder.UserID;
        //    crConnectionInfo.Password = builder.Password;

        //    ReportDocument reportDocument = new ReportDocument();

        //    reportDocument.Load(staeredpat + "\\Reports\\Documents\\Attendance\\NormalAttendaneReport.rpt");

        //for (int i = 0; i < a.Length / 2; i++)
        //{
        //    string s1 = a[i, 0];
        //    string s2 = a[i, 1];
        //    if (s2.ToString() == "")
        //    {
        //        reportDocument.SetParameterValue(s1, "");
        //    }
        //    else
        //    {
        //        reportDocument.SetParameterValue(s1, s2);
        //        reportDocument.SetParameterValue(
        //    }
        //}
        //     reportDocument.SetParameterValue("@timePeriodID","08d602ad-dc2b-4693-810e-0d8461c22b2f");
        //    Tables crTables = reportDocument.Database.Tables;

        //    TableLogOnInfo crTableLogonInfo = new TableLogOnInfo();

        //    foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
        //    {
        //        crTableLogonInfo = crTable.LogOnInfo;
        //        crTableLogonInfo.ConnectionInfo = crConnectionInfo;
        //        crTable.ApplyLogOnInfo(crTableLogonInfo);
        //    }
        //    reportDocument.Refresh();
        //    CristalReportViewer1.ViewerCore.ReportSource = reportDocument;


        //    NormalAttendaneReport attreport = new NormalAttendaneReport();
        //    #region Report Login
        //    ConnectionInfo connInfo = new ConnectionInfo();
        //    connInfo.ServerName = builder.DataSource.ToString();
        //    connInfo.DatabaseName = builder.InitialCatalog.ToString();
        //    connInfo.UserID = builder.UserID.ToString();
        //    connInfo.Password = builder.Password.ToString();


        //    TableLogOnInfo tableLogOnInfo = new TableLogOnInfo();
        //    tableLogOnInfo.ConnectionInfo = connInfo;

        //    foreach (CrystalDecisions.CrystalReports.Engine.Table table in attreport.Database.Tables)
        //    {
        //        table.ApplyLogOnInfo(tableLogOnInfo);
        //        table.LogOnInfo.ConnectionInfo.ServerName = connInfo.ServerName;
        //        table.LogOnInfo.ConnectionInfo.DatabaseName = connInfo.DatabaseName;
        //        table.LogOnInfo.ConnectionInfo.UserID = connInfo.UserID;
        //        table.LogOnInfo.ConnectionInfo.Password = connInfo.Password;

        //         Apply the schema name to the table's location
        //        table.Location = "dbo." + table.Location;
        //    }
        //    #endregion

        //    attreport.SetParameterValue("@timePeriodID", "");


        //    CristalReportViewer1.ViewerCore.ReportSource = attreport;
        //      report= attreport;
        //}

        public void getreport(Object obj, string FilterFormular, string reportname)
        {

            try
            {
                string staeredpat = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

                ReportDocument report = new ReportDocument();

                report.Load(staeredpat + "\\Reports\\Documents\\NewReportFilters\\PayrollSumarryReport.rpt");

                report.RecordSelectionFormula = FilterFormular;
                CristalReportViewer1.ViewerCore.ReportSource = report;
                report.SetDataSource(obj);
                report.Refresh();
            }
            catch (Exception)
            {

                MessageBox.Show("can't open The report");
            }

        }
        private void CrystalReportsViewer_Loaded_1(object sender, RoutedEventArgs e)
        {
            //CristalReportViewer1.ViewerCore.ReportSource = report;
        }

        public void ReportLoad(string foldername, string reportname, string FilterFormular, string filterBy)
        {
            string connectionString="";

            try
            {
                connectionString = serviceClient.GetConnection();
            }
            catch (Exception)
            {
               connectionString=ConfigurationManager.ConnectionStrings["ERPConnectionString"].ConnectionString;
                //clsMessages.setMessage("Report Error");
                //MessageBox.Show("Service Error" + ex.Message.ToString(), "Error !", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            crConnectionInfo.ServerName = builder.DataSource;
            crConnectionInfo.DatabaseName = builder.InitialCatalog;
            crConnectionInfo.UserID = builder.UserID;
            crConnectionInfo.Password = builder.Password;

            string staeredpat = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            ReportDocument cryRpt = new ReportDocument();
            //cryRpt.ParameterFields["@FilterBy"].CurrentValues.Clear();
            //cryRpt.ParameterFields["@FilterBy"].DefaultValues.Clear();
            //cryRpt.ParameterFields["@FilterBy"].CurrentValues.Add("hh");
            cryRpt.Load(staeredpat + "\\Reports\\Documents\\" + foldername + "\\" + reportname + ".rpt");
            cryRpt.RecordSelectionFormula = FilterFormular;

            Tables crTables = cryRpt.Database.Tables;

            TableLogOnInfo crTableLogonInfo = new TableLogOnInfo();

            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            {
                crTableLogonInfo = crTable.LogOnInfo;
                crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogonInfo);
            }
            cryRpt.Refresh();
            try
            {
                ((CrystalDecisions.CrystalReports.Engine.TextObject)cryRpt.ReportDefinition.ReportObjects["Text13"]).Text = filterBy;
            }
            catch (Exception)
            {

            }
            CristalReportViewer1.ViewerCore.ReportSource = cryRpt;

        }

        public void GetReportUsingParameetors(string[,] a, string foldername, string reportname, string FilterFormular, string filterBy)
        {
            string connectionString = "";

            try
            {
                connectionString = serviceClient.GetConnection();
            }
            catch (Exception)
            {
                connectionString = ConfigurationManager.ConnectionStrings["ERPConnectionString"].ConnectionString;
            }
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            crConnectionInfo.ServerName = builder.DataSource;
            crConnectionInfo.DatabaseName = builder.InitialCatalog;
            crConnectionInfo.UserID = builder.UserID;
            crConnectionInfo.Password = builder.Password;

            string staeredpat = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            ReportDocument cryRpt = new ReportDocument();


            try
            {
                cryRpt.Load(staeredpat + "\\Reports\\Documents\\" + foldername + "\\" + reportname + ".rpt");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            cryRpt.RecordSelectionFormula = FilterFormular;

            Tables crTables = cryRpt.Database.Tables;

            TableLogOnInfo crTableLogonInfo = new TableLogOnInfo();

            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            {
                crTableLogonInfo = crTable.LogOnInfo;
                crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogonInfo);
            }
            cryRpt.Refresh();

            for (int i = 0; i < a.Length / 2; i++)
            {
                string s1 = a[i, 0];
                string s2 = a[i, 1];
                if (s2.ToString() == "")
                {
                    cryRpt.SetParameterValue(s1, "");
                }
                else
                {
                    cryRpt.SetParameterValue(s1, s2);
                }
            }
            //cryRpt.SetParameterValue("@timePeriodID", "");
            //cryRpt.SetParameterValue("@companyBranchid", "");
            //cryRpt.SetParameterValue("@dapartmentid", "");
            //cryRpt.SetParameterValue("@designationid", "");
            //cryRpt.SetParameterValue("@sectionid", "");
            //cryRpt.SetParameterValue("@gradeid", "");
            //cryRpt.SetParameterValue("@employeeid", "");
            //cryRpt.SetParameterValue("@shiftid", "");

            try
            {
                ((CrystalDecisions.CrystalReports.Engine.TextObject)cryRpt.ReportDefinition.ReportObjects["Text13"]).Text = filterBy;
            }
            catch (Exception)
            {

            }

            CristalReportViewer1.ViewerCore.ReportSource = cryRpt;

        }

    }
}
