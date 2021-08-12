using ERP.Dashboard;
using ERP.HelperClass;
using ERP.Notification;
using ERP.Sales;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace ERP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel = new MainWindowViewModel();
        public MainWindow()
        {
            clsWindow.Mainform = this;
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };

            InitializeComponent();

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,1);
            dispatcherTimer.Start();
        }
        PoSWindow pwindow;
        InventoryWindow iwindow;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //UserMaster um = new UserMaster();
            //um.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString("F");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            formClose();
            MasterDetailUserControl MDC = new MasterDetailUserControl();
            MDIParent.Children.Clear();
            MDIParent.Children.Add(MDC);
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Image icon = new Image();
            BitmapImage bmImage = new BitmapImage();
            bmImage.BeginInit();
            bmImage.UriSource = new Uri("C:\\Users\\victoryinformation\\Downloads\\error.png", UriKind.Absolute);
            bmImage.EndInit();
            
            image.Source = bmImage;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
           
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            formClose();
            MasterUserControl usermasUcon = new MasterUserControl();
            MDIParent.Children.Clear();
            MDIParent.Children.Add(usermasUcon);
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            //PayrollUserControl payRoll = new PayrollUserControl();
            //MDIParent.Children.Clear();
            //MDIParent.Children.Add(payRoll);
            MDIParent.Children.Clear();
            formClose();
            iwindow = new InventoryWindow();
            iwindow.Show();
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            formClose();
            Reports.ReportMaster re = new Reports.ReportMaster();
            MDIParent.Children.Clear();
            MDIParent.Children.Add(re);
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
               }

        private void Hr_module_Btn_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            HRUserControl hr = new HRUserControl();
            MDIParent.Children.Clear();
            MDIParent.Children.Add(hr);
        }

        private void admin_Button_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            AdministratorUserControl adminisUsc = new AdministratorUserControl();
            MDIParent.Children.Clear();
            MDIParent.Children.Add(adminisUsc);
        }

        private void Sales_Button_Click(object sender, RoutedEventArgs e)
        {
            //SalesUserControl Sales = new SalesUserControl();
            //MDIParent.Children.Clear();
            //MDIParent.Children.Add(Sales);
            MDIParent.Children.Clear();
            formClose();
            pwindow = new PoSWindow();
            pwindow.Show();
            
        }

        private void Production_Click(object sender, RoutedEventArgs e)
        {
            formClose();
            production.ProductionMainUserControl pr = new production.ProductionMainUserControl();
            MDIParent.Children.Clear();
            MDIParent.Children.Add(pr);
        }

        void formClose() 
        {
            try
            {
                if (iwindow != null)
                    iwindow.Close();
                if (pwindow != null)
                    pwindow.Close();
            }
            catch (Exception)
            {

            }
        }

        private void checkBox1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewNotificationDetails window = new ViewNotificationDetails();
            window.Show();
        }

        private void checkBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            formClose();
            MDIParent.Children.Clear();
            MDIParent.Children.Add(new DashboardMainUI());
        }



    }
}
