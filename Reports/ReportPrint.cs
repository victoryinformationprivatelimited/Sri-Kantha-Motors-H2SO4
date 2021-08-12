using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Reports
{
    class ReportPrint
    {
        dynamic report;
        public ReportPrint(dynamic report)
        {
            this.report = report;
            ReportLogonClass logon = new ReportLogonClass(report);
            report = logon.getLog();
        }
        public ReportPrint(string path)
        {
            ReportDocument Rep = new ReportDocument();
            string current = Directory.GetCurrentDirectory();
            string startPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            Rep.Load(startPath + path + ".rpt");
            ReportLogonClass logon = new ReportLogonClass(Rep);
            report = logon.getLog();
        }
        public void setParameterValue(string parameter, dynamic value)
        {
            report.SetParameterValue(parameter, value);
        }


        public void PrintReportWithReportViewer()
        {
            //report viewer + print
            ReportViewer Form = new ReportViewer(report, true);
            Form.Show();
        }

        public void PrintReport()
        {
            //print without report viewer
            report.PrintToPrinter(1, false, 1, 9999);
        }

        public void LoadToReportViewer()
        {
            ReportViewer Form = new ReportViewer(report, false);
            Form.Show();
        }

        public void PrintReport(string printerName)
        {
            report.PrintOptions.PrinterName = printerName;
            report.PrintToPrinter(1, false, 1, 9999);
        }
    }
}
