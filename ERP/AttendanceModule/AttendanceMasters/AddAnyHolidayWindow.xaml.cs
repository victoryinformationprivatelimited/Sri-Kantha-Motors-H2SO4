using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP.AttendanceModule.AttendanceMasters
{
    /// <summary>
    /// Interaction logic for AddAnyHolidayWindow.xaml
    /// </summary>
    public partial class AddAnyHolidayWindow : Window
    {
        AddAnyHolidayViewModel viewModel;
        public AddAnyHolidayWindow(Window parentWindow)
        {
            InitializeComponent();
            viewModel = new AddAnyHolidayViewModel();
            viewModel.ownerWindow = this;
            this.Owner = parentWindow;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void bulkHolidayTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void bulkHolidayTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void bulkHolidayCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
