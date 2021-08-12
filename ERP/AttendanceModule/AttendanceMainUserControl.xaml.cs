using ERP.UI_UserControlers;
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

namespace ERP.AttendanceModule
{
    /// <summary>
    /// Interaction logic for AttendanceMainUserControl.xaml
    /// </summary>
    public partial class AttendanceMainUserControl : UserControl
    {
        public AttendanceMainUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AttendanceButtons AttendanceBtn = new AttendanceButtons();
            Mdiwrappanel.Children.Add(AttendanceBtn);
        }
    }
}
