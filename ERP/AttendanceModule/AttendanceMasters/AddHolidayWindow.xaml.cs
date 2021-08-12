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
    /// Interaction logic for AddHolidayWindow.xaml
    /// </summary>
    public partial class AddHolidayWindow : Window
    {
        AddHolidayViewModel viewModel;
        public AddHolidayWindow()
        {
            InitializeComponent();
            viewModel = new AddHolidayViewModel();
            this.Owner = clsWindow.Mainform;
            viewModel.viewModelUI = this;
            this.Loaded += (s, e) => { DataContext = viewModel; };
            
        }

        private void holidayTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void holidayTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void holidayCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
