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

namespace ERP.Reports
{
    /// <summary>
    /// Interaction logic for ReportControllManager.xaml
    /// </summary>
    public partial class ReportControllManager : UserControl
    {
        public List<rpt_catagory> cat = new List<rpt_catagory>();
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public int b = 0;
        public int a = 0;
        public int c = 0;
        public ReportControllManager()
        {
            this.InitializeComponent();
            refreshModule();
            //basicReportContainer.Visibility = Visibility.Hidden;
            //AdvanceReportContainer.Visibility = Visibility.Hidden;
            //DetailReportContainer.Visibility = Visibility.Hidden;
          
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            ((Grid)this.Parent).Children.Remove(this);
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            Report_ui_UserControlers.Report_HR_btn RHR = new Report_ui_UserControlers.Report_HR_btn(this);
            Mdiwrappanel.Children.Clear();
            Mdiwrappanel.Children.Add(RHR);

        }

        #region propaties
        private string moduleID;

        public string ModuleID
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        private string cataID;

        public string CataID
        {
            get { return cataID; }
            set { cataID = value; }
        }
        #endregion

        #region Add Button Class
        public void AddButton(string containt, string name)
        {
            Button type_btn = new Button();
            type_btn.Width = 100;
            type_btn.Height = 25;
            //btn.Padding = new Thickness(5, 5, 5, 5);
            type_btn.Margin = new Thickness(15, 10, 0, 0);
            type_btn.Name = name;
            type_btn.Content = containt;
            //ReportType.Children.Add(type_btn);

            type_btn.Click += new RoutedEventHandler(ReportType_Click);
        }
        #endregion

        #region Report Type button click
        private void ReportType_Click(object sender, RoutedEventArgs e)
        {

            Button clicked = (Button)sender;
            switch (clicked.Name)
            {
                case "BasicReport":
                    Documents.BasicReports.BasicReportControll  basicrpt= new Documents.BasicReports.BasicReportControll();
                    string gd = getcatid(clicked.Content.ToString());
                    Guid repcat = Guid.Parse(gd);
                    basicrpt.refreshReportBasic(repcat, Guid.Parse(ModuleID));
                    MDIRptCatagory.Children.Clear();
                    MDIRptCatagory.Children.Add(basicrpt);
                    break;

                case "AdvanceReport":
                    Documents.Payroll.PayrollReportControll rptcontroll = new Documents.Payroll.PayrollReportControll();
                    string gda = getcatid(clicked.Content.ToString());
                    Guid repcata = Guid.Parse(gda);
                    rptcontroll.refreshReportDetails(repcata, Guid.Parse(ModuleID));
                    MDIRptCatagory.Children.Clear();
                    MDIRptCatagory.Children.Add(rptcontroll);  
                    break;

                case "DetailReport":

                    // Documents.Detail_Report.DetailReportControll detailControll = new Documents.Detail_Report.DetailReportControll();
                    //string gdd = getcatid(clicked.Content.ToString());
                    //Guid repcatd = Guid.Parse(gdd);
                    //detailControll.refreshReportAdvance(repcatd, Guid.Parse(ModuleID));
                    //MDIRptCatagory.Children.Clear();
                    //MDIRptCatagory.Children.Add(detailControll);


                    break;

            }
        }
        #endregion

        #region refresh catagory
        public void refreshModule()
        {
            this.serviceClient.GetrptCatagoryCompleted += (s, e) =>
            {
                foreach (var itemcatagory in e.Result)
                {
                    string nam = itemcatagory.rpt_type_name;
                    nam = nam.Replace(" ", "");
                    AddButton(itemcatagory.rpt_type_name, nam);
                    cat.Add(itemcatagory);
                }

            };
            this.serviceClient.GetrptCatagoryAsync();

        }
        #endregion

        #region getcurrent catagory id
        public string getcatid(string catagory)
        {
            foreach (rpt_catagory itemcat in cat)
            {
                if (itemcat.rpt_type_name == catagory)
                {
                    return itemcat.rpt_catagory_id.ToString();
                }

            }
            return "";
        }
        #endregion












        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ReportViewer rep = new ReportViewer();
            rep.Show();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Report_ui_UserControlers.Report_HR_btn RHR = new Report_ui_UserControlers.Report_HR_btn(this);
            Mdiwrappanel.Children.Clear();
            Mdiwrappanel.Children.Add(RHR);

        }

        //private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        //{
        //    Report_ui_UserControlers.Report_HR_btn RHR = new Report_ui_UserControlers.Report_HR_btn(this);
        //    Mdiwrappanel.Children.Clear();
        //    Mdiwrappanel.Children.Add(RHR);
        //}
    }
}
