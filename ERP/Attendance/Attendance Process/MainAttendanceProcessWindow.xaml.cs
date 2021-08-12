using ERP.Notification;
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

namespace ERP.Attendance.Attendance_Process
{
    /// <summary>
    /// Interaction logic for MainAttendanceProcessWindow.xaml
    /// </summary>
    public partial class MainAttendanceProcessWindow : Window
    {
        AttendanceProcessViewModel ViewModel;

        public MainAttendanceProcessWindow()
        {
            InitializeComponent();
            ViewModel = new AttendanceProcessViewModel();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
            addForm();
        }

        public void addForm()
        {
            AttendanceProcessUserControl process = new AttendanceProcessUserControl(ViewModel);
            AttendanceMDI.Children.Clear();
            AttendanceMDI.Children.Add(process);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            progressBar process = new progressBar(ViewModel);
            AttendanceMDI.Children.Clear();
            AttendanceMDI.Children.Add(process);
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
