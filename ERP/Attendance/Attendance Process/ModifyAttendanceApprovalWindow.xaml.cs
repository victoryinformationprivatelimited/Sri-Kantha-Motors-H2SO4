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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ERP.Attendance.Attendance_Process
{
    /// <summary>
    /// Interaction logic for ModifyAttendanceApprovalWindow.xaml
    /// </summary>
    public partial class ModifyAttendanceApprovalWindow : Window
    {

        public ModifyAttendanceApprovalWindow(ModifyAttendanceViewmodel viewModel)
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                this.DataContext = viewModel;
            };
        }

        private void CloseBtn_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
