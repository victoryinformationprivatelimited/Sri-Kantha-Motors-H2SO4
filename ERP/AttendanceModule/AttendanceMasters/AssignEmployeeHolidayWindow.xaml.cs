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
    /// Interaction logic for AssignEmployeeHolidayWindow.xaml
    /// </summary>
    public partial class AssignEmployeeHolidayWindow : Window
    {
        AssignEmployeeHolidayViewModel viewModel;
        public AssignEmployeeHolidayWindow()
        {
            InitializeComponent();
            viewModel = new AssignEmployeeHolidayViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
            this.Owner = clsWindow.Mainform;
            viewModel.ownerWindow = this;
        }

        private void holidayAssignedTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void holidayAssignedTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void holidayAssignedCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.RemoveSelectedGroup();            
        }

        private void assignHolidayTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	this.DragMove();
        }
    }
}
