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
using ERP.Payroll.RI_Allowance;
using ERP.production;
using ERP.Reports.Documents.AllowanceReports.AllowanceUserControls;
//using ERP.production.Calibrate;

namespace ERP.UI_UserControlers
{
    /// <summary>
    /// Interaction logic for Productionbtn.xaml
    /// </summary>
    public partial class Productionbtn : UserControl
    {
        //AddGemWindow addGemWindow;
        //AddPreformTaskWindow addPreformTaskWindow;
        //GemGroupWindow addSubLotWindow;
        //GemLotWindow addLotWindow;
        //PreformTaskReturnWindow addPreformTaskReturn;
        //AddCalibrateTaskWindow addCalibrateTaskWindow;
        //CalibrateTaskReturnWindow calibrateTaskReturnWindow;
        //AddDesignTaskWindow addDesignTaskWindow;
        //DesignTaskReturnWindow designTaskReturnWindow;

        WrapPanel MDIWrip = new WrapPanel();
        public Productionbtn(ProductionMainUserControl us)
        {
            InitializeComponent();
            MDIWrip = us.Mdi;
        }

        private void GemTypeButton_Checked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addGemWindow = new AddGemWindow();
            //addGemWindow.Show();
        }

        private void GemLotBtn_Checked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addLotWindow = new GemLotWindow();
            //addLotWindow.Show();
        }

        private void AddAllowanceBtn_Checked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            AddAllowanceBtn.IsChecked = true;
            EmployeeAllowanceUserControl userControl = new EmployeeAllowanceUserControl();
            MDIWrip.Children.Add(userControl);
        }

        private void processAllowancBtn_Checked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            processAllowancBtn.IsChecked = true;
            AllowanceTransactionUserControl userControl = new AllowanceTransactionUserControl();
            MDIWrip.Children.Add(userControl);
        }

        private void btn_AllowanceReport_Checked(object sender, RoutedEventArgs e)
        {
            MDIWrip.Children.Clear();
            btn_AllowanceReport.IsChecked = true;
            AllowanceBasicUserControl userControl = new AllowanceBasicUserControl();
            MDIWrip.Children.Add(userControl);
        }

        private void SubLotBtn_Checked(object sender, RoutedEventArgs e)
        {
        //    this.ClearForm();
        //    addSubLotWindow = new GemGroupWindow();
        //    addSubLotWindow.Show();
        }

        private void PreformTaskBtn_Checked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addPreformTaskWindow = new AddPreformTaskWindow();
            //addPreformTaskWindow.Show();
        }

        private void PreformReturnBtn_Checked(object sender, RoutedEventArgs e)
        {
            //PreformReturnBtn.IsChecked = false;
            //this.ClearForm();
            //addPreformTaskReturn = new PreformTaskReturnWindow();
            //addPreformTaskReturn.Show();
        }

        private void testButton_Checked(object sender, RoutedEventArgs e)
        {
        //    MDIWrip.Children.Clear();
        //    testButton.IsChecked = true;
        //    UserControl1 userControl = new UserControl1();
        //    MDIWrip.Children.Add(userControl);
        }

        private void ClearForm()
        {
            try
            {
            //    if (addGemWindow != null)
            //        addGemWindow.Close();
            //    if (addPreformTaskWindow != null)
            //        addPreformTaskWindow.Close();
            //    if (addSubLotWindow != null)
            //        addSubLotWindow.Close();
            //    if (addLotWindow != null)
            //        addLotWindow.Close();
            //    if (addPreformTaskReturn != null)
            //        addPreformTaskReturn.Close();
            //    if (addCalibrateTaskWindow != null)
            //        addCalibrateTaskWindow.Close();
            //    if (calibrateTaskReturnWindow != null)
            //        calibrateTaskReturnWindow.Close();
            //    if (addDesignTaskWindow != null)
            //        addDesignTaskWindow.Close();
            //    if (designTaskReturnWindow != null)
            //        designTaskReturnWindow.Close();
            }
            catch (Exception)
            {
            }
        }

        private void GemLotBtn_Unchecked(object sender, RoutedEventArgs e)
        {

            //this.ClearForm();
            //addLotWindow = new GemLotWindow();
            //addLotWindow.Show();

        }

        private void SubLotBtn_Unchecked(object sender, RoutedEventArgs e)
        {

            //this.ClearForm();
            //addSubLotWindow = new GemGroupWindow();
            //addSubLotWindow.Show();

        }

        private void PreformTaskBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addPreformTaskWindow = new AddPreformTaskWindow();
            //addPreformTaskWindow.Show();
        }

        private void GemTypeButton_Unchecked(object sender, RoutedEventArgs e)
        {
        //    this.ClearForm();
        //    addGemWindow = new AddGemWindow();
        //    addGemWindow.Show();
        }

        private void PreformReturnBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addPreformTaskReturn = new PreformTaskReturnWindow();
            //addPreformTaskReturn.Show();

        }

        private void CalibrateTaskBtn_Checked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addCalibrateTaskWindow = new AddCalibrateTaskWindow();
            //addCalibrateTaskWindow.Show();
        }

        private void CalibrateTaskBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addCalibrateTaskWindow = new AddCalibrateTaskWindow();
            //addCalibrateTaskWindow.Show();
        }

        private void calibrateTaskReturnBtn_Checked(object sender, RoutedEventArgs e)
        {
        //    this.ClearForm();
        //    calibrateTaskReturnWindow = new CalibrateTaskReturnWindow();
        //    calibrateTaskReturnWindow.Show();
        }

        private void calibrateTaskReturnBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //calibrateTaskReturnWindow = new CalibrateTaskReturnWindow();
            //calibrateTaskReturnWindow.Show();
        }

        private void designTaskChkbtn_Checked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addDesignTaskWindow = new AddDesignTaskWindow();
            //addDesignTaskWindow.Show();
        }

        private void designTaskChkbtn_Unchecked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //addDesignTaskWindow = new AddDesignTaskWindow();
            //addDesignTaskWindow.Show();
        }

        private void designTaskReturnChkBtn_Checked(object sender, RoutedEventArgs e)
        {
            //this.ClearForm();
            //designTaskReturnWindow = new DesignTaskReturnWindow();
            //designTaskReturnWindow.Show();
        }
    }
}
