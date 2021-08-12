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
    /// Interaction logic for ReportMaster.xaml
    /// </summary>
    public partial class ReportMaster : UserControl
    {
        private ERPServiceClient serviceClient = new ERPServiceClient();
        public string modleidfornather = "";
        public List<z_module> module = new List<z_module>();

        public ReportMaster()
        {
            this.InitializeComponent();
            refreshModule();
           
        }
        public void AddButton(string containt)
        {
            //Button btn = new Button();
            //btn.Width = 75;
            //btn.Height = 25;
            //btn.Padding = new Thickness(5, 5, 5, 5);
            //btn.Margin = new Thickness(15, -30, 0, 0);
            //btn.Name = containt;
            //btn.Content = containt;
            //ReportMasterBtnPanal.Children.Add(btn);
            //btn.Click += new RoutedEventHandler(button_click);
        }

        private void button_click(object sender, RoutedEventArgs e )
        {
            //Button clicked = (Button)sender;
            ////addValueToNextForm(clicked.Name);
            //switch (clicked.Name)
            //{
            //    case "Admin":
            //        MessageBox.Show("Admin Module");
            //        break;
            //    case "Payroll":

            //        Reports.ReportControllManager PayrollReportcontroll = new Reports.ReportControllManager();
            //        PayrollReportcontroll.ModuleID = modleidfornather;
            //        MdiMainReport.Children.Clear();
            //        MdiMainReport.Children.Add(PayrollReportcontroll);

            //        break;

            //}

            
           
        }

        //public void addValueToNextForm(string name)
        //{

        //    _data = "dsfdf";
        //    //foreach (z_module itemGuidlist in module)
        //    //{
        //    //    if (itemGuidlist.module_name == name)
        //    //    {
        //    //        moduleid.ModuleID= itemGuidlist.module_id.ToString();
        //    //    }
        //    //}

        //}

     

      

      

        public void refreshModule()
        {
            

            this.serviceClient.GetModulesCompleted += (s, e) =>
            {
                    foreach (var itemmodule in e.Result)
                    {
                        this.AddButton(itemmodule.module_name);
                        modleidfornather= itemmodule.module_id.ToString();
                    }

            };
            this.serviceClient.GetModulesAsync();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Report_ui_UserControlers.Report_Main_btn report = new Report_ui_UserControlers.Report_Main_btn(this);
            Mdi.Children.Clear();
            Mdi.Children.Add(report);
        }
     
        

    }
}
