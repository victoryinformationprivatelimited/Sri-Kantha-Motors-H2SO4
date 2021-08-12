using ERP.HelperClass;
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

namespace ERP.WebPortal
{
    /// <summary>
    /// Interaction logic for PortalEventsWindow.xaml
    /// </summary>
    public partial class PortalEventsWindow : Window
    {
        PortalEventsViewModel ViewModel;

        public PortalEventsWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            ViewModel = new PortalEventsViewModel();

            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }


        private void emp_event_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_event_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void emp_event_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
