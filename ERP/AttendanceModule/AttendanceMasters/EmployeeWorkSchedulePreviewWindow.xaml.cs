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
    /// Interaction logic for EmployeeWorkSchedulePreviewWindow.xaml
    /// </summary>
    /// 
    
    public partial class EmployeeWorkSchedulePreviewWindow : Window
    {
        DailyShiftAssignViewModel currentViewModel;
        public DailyShiftAssignWindow parentWindow;
        public EmployeeWorkSchedulePreviewWindow(DailyShiftAssignViewModel viewModel)
        {
            InitializeComponent();
            if (viewModel != null)
            {
                this.Loaded += (s, e) => { DataContext = viewModel; };
                currentViewModel = viewModel;
                this.Owner = clsWindow.Mainform;
            }
                
        }

        private void previewDailyShiftTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
			this.DragMove();
        }

        private void previewDailyShiftTitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	this.DragMove();
        }

        private void previewDailyShiftCloseBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	this.Close();
        }

        private void delShiftAssignedBtn_Click(object sender, RoutedEventArgs e)
        {
            if (currentViewModel != null)
                currentViewModel.DeleteAssignedShift();
        }
    }
}
