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

namespace ERP.AttendanceModule.AttendanceMasters
{
    /// <summary>
    /// Interaction logic for HolidayTypeWindow.xaml
    /// </summary>
    public partial class HolidayTypeWindow : Window
    {
        HolidayTypeViewModel viewModel;
        public HolidayTypeWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new HolidayTypeViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void holidayTypeCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void holidayTypeTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void holidayTypeTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
