/*Class Information******************************************************************************************
 *   Author     :  CHAMARA HERATH                                                                           *                    
 *   Date       :  14-11-2013                                                                               *             
 *   Purpose    :  For Get Company Details For Report Module.                                               *                                    
 *   Company    :  Victory Information                                                                      *     
 *   Module     :  ERP System => Report => ReportBase                                                       * 
 *   Tips       :  Inherit this class for Entity Report class.Then directly access company                  *
 *                 Propertise for report parameters.                                                        *
 ************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using CrystalDecisions.Shared;


namespace ERP.Reports.Documents
{
    public class ReportBase : ViewModelBase
    {
        ERPServiceClient objserviceClient = new ERPServiceClient();
        public ReportDocument objReportDocument = new ReportDocument();       

        private string sReportFiler = null;
        public string sDateTimeFormat = "yyyy-MM-dd";

        #region Constructor
        public ReportBase(string sFilePath,string sFiltering,string sTitle)
        {
            refreshCompany();

            if (sFilePath != null)
            {
                sReportPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName+"\\"+sFilePath+".rpt";
            }

            if (sFiltering != null)
            {
                sReportFiler = sFiltering;
            }

            if (sTitle != null)
            {
                sReportTitle = sTitle;
            }

            LoadReport();
            
        }

        public ReportBase()
        {
            refreshCompany();
        }
        #endregion

        #region Properties
        private z_Company _Comapay;
        public z_Company Comapay
        {
            get { return _Comapay; }
            set { _Comapay = value; }
        }

        private string _sReportPath;
        public string sReportPath
        {
            get { return _sReportPath; }
            set { _sReportPath = value; OnPropertyChanged("sReportPath"); if (sReportPath != null) 
            { 
                LoadReport(); } }
        }

        private string _sReportFilter;
        public string sReportFilter
        {
            get { return _sReportFilter; }
            set { _sReportFilter = value; OnPropertyChanged("sReportFilter"); }
        }

        private string _FilterFileds;
        public string FilterFileds
        {
            get { return _FilterFileds; }
            set { _FilterFileds = value; OnPropertyChanged("FilterFileds"); }
        }

        private string _sReportTitle;
        public string sReportTitle
        {
            get { return _sReportTitle; }
            set { _sReportTitle = value; OnPropertyChanged("sReportTitle"); }
        }

        private DateTime _SelectedFromDate;
        public DateTime SelectedFromDate
        {
            get { return _SelectedFromDate; }
            set { _SelectedFromDate = value; OnPropertyChanged("SelectedFromDate"); }
        }

        private DateTime _SelectedToDate;
        public DateTime SelectedToDate
        {
            get { return _SelectedToDate; }
            set { _SelectedToDate = value; OnPropertyChanged("SelectedToDate"); }
        }

        #endregion

        #region Refresh Methods
        /// <summary>
        /// GetCompany Detatils from Online Service.
        /// </summary>
        private void refreshCompany()
        {
           this.Comapay = this.objserviceClient.GetCompanies().FirstOrDefault();          
        }

        #endregion
        
        #region User Defined Method

        public void Print()
        {
            try
            {
                objReportDocument.ReportOptions.ConvertNullFieldToDefault = true;

                EntityReportViwer objEntityReportViewr = new EntityReportViwer();
                objEntityReportViewr.setReport(objReportDocument);
                objEntityReportViewr.Show();
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
        }

        public void setFileds()
        {
            try
            {
                objReportDocument.SetParameterValue("CompanyAddress", (this.Comapay.company_name_01 != null) ? this.Comapay.company_name_01 : " - " + " " + ((this.Comapay.comapany_name_02 != null) ? this.Comapay.comapany_name_02 : " - "));
                objReportDocument.SetParameterValue("address01",(this.Comapay.address_01 != null) ? this.Comapay.address_01 : " - ");                    
                objReportDocument.SetParameterValue("address02",(this.Comapay.address_02 != null)? this.Comapay.address_02 : " - ");
                objReportDocument.SetParameterValue("address03", (this.Comapay.address_03 != null) ? this.Comapay.address_03 : " - ");
                objReportDocument.SetParameterValue("Telephone",(this.Comapay.telephone_01 != null) ? this.Comapay.telephone_01 : " - ");
                objReportDocument.SetParameterValue("Fax",(this.Comapay.fax != null) ? this.Comapay.fax : " - ");
                objReportDocument.SetParameterValue("Email", (this.Comapay.email != null) ? this.Comapay.email : " - ");
                objReportDocument.SetParameterValue("Web",(this.Comapay.web != null) ? this.Comapay.web : " - ");
                objReportDocument.SetParameterValue("Prepared By", clsSecurity.loggedUser.user_name);
                objReportDocument.SetParameterValue("FilterBy", FilterFileds);
                objReportDocument.SetParameterValue("ReportTitle", this.sReportTitle);
                objReportDocument.SetParameterValue("FromDate", this.SelectedFromDate.ToString(clsConfig.DateTimeFormat));
                objReportDocument.SetParameterValue("Todate", this.SelectedToDate.ToString(clsConfig.DateTimeFormat));
            }
            catch (NullReferenceException)
            {
                //refreshCompany();
                //setFileds();
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message);
            }
        }

        public void getConnections()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ERPConnectionString"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            crConnectionInfo.ServerName = builder.DataSource;
            crConnectionInfo.DatabaseName = builder.InitialCatalog;
            crConnectionInfo.UserID = builder.UserID;
            crConnectionInfo.Password = builder.Password;

            Tables crTables = objReportDocument.Database.Tables;

            TableLogOnInfo crTableLogonInfo = new TableLogOnInfo();

            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
            {
                crTableLogonInfo = crTable.LogOnInfo;
                crTableLogonInfo.ConnectionInfo = crConnectionInfo;
                crTable.ApplyLogOnInfo(crTableLogonInfo);
            }
        }

        public void LoadReport()
        {
            objReportDocument.Load(sReportPath);
        }
        #endregion
    }
}
