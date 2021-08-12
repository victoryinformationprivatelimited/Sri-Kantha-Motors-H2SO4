using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Windows;
using System.Data.SqlClient;
using ERP.ERPService;

namespace ERP.Reports
{
    class ReportLogonClass
    {
        dynamic report;
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public ReportLogonClass(dynamic report)
        {
            //  this.report = new dynamic();
            //  report = new object();

            this.report = report;
        }

        public dynamic getLog()
        {
            string connectionString="";
            try
            {
                try
                {
                connectionString = serviceClient.GetConnection();
                }
                catch (Exception ex)
                {
                     clsMessages.setMessage("Report Error");
                MessageBox.Show("Service Error" + ex.Message.ToString(), "Error !", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                ConnectionInfo connInfo = new ConnectionInfo();
                connInfo.ServerName = builder.DataSource;
                connInfo.DatabaseName = builder.InitialCatalog;
                connInfo.UserID = builder.UserID;
                connInfo.Password = builder.Password;

                TableLogOnInfo tableLogOnInfo = new TableLogOnInfo();
                tableLogOnInfo.ConnectionInfo = connInfo;

                foreach (Table table in report.Database.Tables)
                {
                    table.ApplyLogOnInfo(tableLogOnInfo);
                    table.LogOnInfo.ConnectionInfo.ServerName = connInfo.ServerName;
                    table.LogOnInfo.ConnectionInfo.DatabaseName = connInfo.DatabaseName;
                    table.LogOnInfo.ConnectionInfo.UserID = connInfo.UserID;
                    table.LogOnInfo.ConnectionInfo.Password = connInfo.Password;

                    // Apply the schema name to the table's location
                    table.Location = "dbo." + table.Location;
                }
                report.Refresh();
            }
            catch (Exception ex)
            {
                clsMessages.setMessage("Report Error");
                MessageBox.Show("Check Connection" + ex.Message.ToString(), "Error !", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            return report;
        }

    }
}
