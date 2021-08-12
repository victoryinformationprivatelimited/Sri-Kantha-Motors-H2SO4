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
    /// Interaction logic for ShiftWeek.xaml
    /// </summary>
    public partial class ShiftWeek : Window
    {
        ShiftWeekViewModel viewModel;
        
        public ShiftWeek()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();
            viewModel = new ShiftWeekViewModel();
            viewModel.shiftWeekUI = this;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void shiftWeekTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void shiftWeekTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void shiftWeekCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
