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

namespace ERP.Reports.Documents.BasicReports
{
    /// <summary>
    /// Interaction logic for BasicReportControll.xaml
    /// </summary>
    public partial class BasicReportControll : UserControl

    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public BasicReportControll()
        {
            InitializeComponent();
        }

        #region Refrish basic button Data

        public void refreshReportBasic(Guid cat ,Guid module)
        {
            this.serviceClient.GetReportDataForReportViewerCompleted += (s, e) =>
            {
                foreach (var subItems in e.Result.Where(c =>c.isNormal==true))
                {
                    string subname = subItems.rpt_name;
                    AddSubBasicReportButton(subItems.rpt_name, subname);
                }
            };
            this.serviceClient.GetReportDataForReportViewerAsync(cat, module);
        }
        #endregion

        #region Basic Report Button
        public void AddSubBasicReportButton(string containt, string name)
        {
            //name.Replace(" ", "");
            Button type_btn = new Button();
            type_btn.Width = 100;
            type_btn.Height = 25;
            //btn.Padding = new Thickness(5, 5, 5, 5);
            type_btn.Margin = new Thickness(15, 10, 0, 0);
            type_btn.Name = name.Replace(" ", "");
            type_btn.Content = containt;
            basicwrappanel.Children.Add(type_btn);
            type_btn.Click += new RoutedEventHandler(SubBasicReportType_Click);
        }

        private void SubBasicReportType_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            switch (clicked.Name)
            {
                case "Grade":
                    ReportViewer rptviewer = new ReportViewer();
                    rptviewer.ReportLoad("BasicReports", "GradeReport", "","");
                    rptviewer.Show();
                    break;

                case "Section":
                    ReportViewer rptviewersec = new ReportViewer();
                    rptviewersec.ReportLoad("BasicReports", "SectionReport", "","");
                    rptviewersec.Show();
                    break;

                case "Bank":
                    ReportViewer rptviewerbank = new ReportViewer();
                    rptviewerbank.ReportLoad("BasicReports", "Bank", "","");
                    rptviewerbank.Show();
                    break;

                case "BasicUnits":
                    ReportViewer rptviewerbasicunits = new ReportViewer();
                    rptviewerbasicunits.ReportLoad("BasicReports", "BasicUnit", "","");
                    rptviewerbasicunits.Show();
                    break;

                case "City":
                    ReportViewer rptviewerbranch = new ReportViewer();
                    rptviewerbranch.ReportLoad("BasicReports", "City", "","");
                    rptviewerbranch.Show();
                    break;

                case "Designation":
                    ReportViewer rptviewerdesi = new ReportViewer();
                    rptviewerdesi.ReportLoad("BasicReports", "DesignationReport", "","");
                    rptviewerdesi.Show();
                    break;

                case "Department":
                    ReportViewer rptviewerdept = new ReportViewer();
                    rptviewerdept.ReportLoad("BasicReports", "EmployeeDepartmentReport", "","");
                    rptviewerdept.Show();
                    break;

                case "Town":
                    ReportViewer rptviewertown = new ReportViewer();
                    rptviewertown.ReportLoad("BasicReports", "Town", "","");
                    rptviewertown.Show();
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
