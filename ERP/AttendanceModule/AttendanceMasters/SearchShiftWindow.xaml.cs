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
    /// Interaction logic for SearchShiftWindow.xaml
    /// </summary>
    public partial class SearchShiftWindow : Window
    {
        public SearchShiftViewModel viewModel;
        public SearchShiftWindow(Window ownerWindow)
        {
            this.Owner = ownerWindow;
            InitializeComponent();
            viewModel = new SearchShiftViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void selectShftBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void searchShiftTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void searchShiftTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
