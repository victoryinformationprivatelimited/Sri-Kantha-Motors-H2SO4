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

namespace ERP.Attendance.Attendance_Data_Migration
{
    /// <summary>
    /// Interaction logic for AttendanceDataMigrationWindow.xaml
    /// </summary>
    public partial class AttendanceDataMigrationWindow : Window
    {
        AttendanceDataMigrationViewModel ViewModel;

        public AttendanceDataMigrationWindow()
        {
            InitializeComponent();
            ViewModel = new AttendanceDataMigrationViewModel();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel; 
            };
        }

        private void Attendence_Data_Migration_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

    }
}
