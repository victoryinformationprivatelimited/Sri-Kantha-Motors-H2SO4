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
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using ERP.ERPService;

namespace ERP.Notification
{
    /// <summary>
    /// Interaction logic for ViewNotificationDetails.xaml
    /// </summary>
    public partial class ViewNotificationDetails : Window
    {
        ViewNotificationDetailsViewModel ViewModel;
        ERPServiceClient serviceClient;

        public ViewNotificationDetails()
        {
            InitializeComponent();
            ViewModel = new ViewNotificationDetailsViewModel();
            serviceClient = new ERPServiceClient();

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DataGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            ViewModel.UpdateRun();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
