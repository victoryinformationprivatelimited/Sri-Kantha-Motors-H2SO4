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

namespace ERP.Security
{
    /// <summary>
    /// Interaction logic for UserPermissionWindow.xaml
    /// </summary>
    public partial class UserPermissionWindow : Window
    {
        UserPermissionViewModel ViewModel;
        public UserPermissionWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            ViewModel = new UserPermissionViewModel();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
