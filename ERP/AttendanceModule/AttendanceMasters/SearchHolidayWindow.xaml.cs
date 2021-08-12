
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
    /// Interaction logic for SearchHolidayWindow.xaml
    /// </summary>
    public partial class SearchHolidayWindow : Window
    {
        public SearchHolidayViewModel viewModel;

        public SearchHolidayWindow(AssignEmployeeHolidayWindow ownerWindow)
        {
            InitializeComponent();
            viewModel = new SearchHolidayViewModel();
            viewModel.ownerWindow = this;
            if (ownerWindow != null)
            {
                this.Owner = ownerWindow;
            }
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void searchHolidayTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void searchHolidayTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void searchHolidayBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
