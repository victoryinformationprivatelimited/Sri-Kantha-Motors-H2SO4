using ERP.Attendance.Attendance_Process;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.Attendance.Attendance_Data_Migration
{
    /// <summary>
    /// Interaction logic for AttendanceDataUpload.xaml
    /// </summary>
    public partial class AttendanceDataUpload : UserControl
    {
        int Step = 1;
        AttendanceDataMigrationViewModel viewModel = new AttendanceDataMigrationViewModel();
        
        public AttendanceDataUpload()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
            AddForm();
        }
        void AddForm()
        {

            AttendanceDataMigrationUserControl migration = new AttendanceDataMigrationUserControl(this.viewModel);
            UploadMDI.Children.Add(migration);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            switch (Step)
            {
                case 1:
                    AttendanceMigrationProgressBar Prograss = new AttendanceMigrationProgressBar(viewModel);
                     this.UploadMDI.Children.Clear();
                     UploadMDI.Children.Add(Prograss);
                    //this.Mdi.Children.Add(suc);
                    Step = 2;
                    break;
                case 2:
                    
                    //EmployeeFinalPayrollProcess pfp = new EmployeeFinalPayrollProcess(viewModel);
                    //this.Mdi.Children.Clear();
                    //this.Mdi.Children.Add(pfp);
                    break;
                default:
                    break;
            }
        }
    }
}
