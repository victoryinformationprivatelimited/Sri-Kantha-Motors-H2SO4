using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ERP.ERPService;
namespace ERP
{
    /// <summary>
    /// Interaction logic for ERPSplash.xaml
    /// </summary>
    public partial class ERPSplash : Window
    {
        DispatcherTimer dis = new DispatcherTimer();
        ERPServiceClient serviceClient;// = new ERPServiceClient();
        ERPSplashViewModel viewModel;
        public ERPSplash()
        {
            InitializeComponent();
            serviceClient = new ERPServiceClient();
            viewModel = new ERPSplashViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
            
        }

        private void Window_Activated_1(object sender, EventArgs e)
        {
            main_progress.Minimum = 0;
            main_progress.Maximum = 100;
            dis.Tick += dis_Tick;
            dis.Interval = new TimeSpan(0, 0, 0,0,1);
            dis.Start();
            lblStatus.Text = "Checking modules...";
        
        }

        void dis_Tick(object sender, EventArgs e)
        {
            main_progress.Value++;            
            if (main_progress.Value == 40)
            {
                bool canProceed = false;
                lblStatus.Text = "Getting ready with online service...";
                dis.Stop();
                this.serviceClient.GetServiceReceponceCompleted += (s, es) =>
                {
                    try
                    {
                        canProceed = es.Result;
                        if (canProceed)
                        {
                            dis.Start();

                        }
                    }
                    catch (Exception)
                    {
                        //MessageBox.Show(ex.Message);
                        //MessageBox.Show(ex.InnerException.Message);
                        //clsMessages.setMessage("Unable to connect to the data service. Please check your internet connection...");
                        MessageBox.Show("Unable to connect to the data service. Please check your internet connection...");
                        Application.Current.Shutdown();
                        
                    }
                };
                this.serviceClient.GetServiceReceponceAsync();
            }
            if (main_progress.Value > 40)
            {
                lblStatus.Text = "Checking security and user accessibility...";
                
            }
            if (main_progress.Value > 80)
            {
                lblStatus.Text = "System is ready to use...";
                try
                {
               
                }
                catch (Exception)
                {
                }
                
            }
            if (main_progress.Value == 100)
            {
                lblStatus.Text = "System is ready to use...";
                dis.Stop();
             
                this.Visibility = Visibility.Hidden;
                ERPLogin login = new ERPLogin();
                login.Show();              
            }
        }

        
    }
}
