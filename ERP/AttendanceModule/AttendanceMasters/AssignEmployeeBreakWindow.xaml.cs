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
    /// Interaction logic for AssignEmployeeBreakWindow.xaml
    /// </summary>
    public partial class AssignEmployeeBreakWindow : Window
    {
        DailyShiftAssignViewModel UIViewModel;
        public AssignEmployeeBreakWindow(DailyShiftAssignViewModel viewModel)
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            UIViewModel = viewModel;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void assign_shift_break_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void assign_shift_break_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void assign_break_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_shifts_btn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

    }
}
