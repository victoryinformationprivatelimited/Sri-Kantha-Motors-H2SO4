using ERP.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for MainAttendanceProcess.xaml
    /// </summary>
    public partial class MainAttendanceProcess : UserControl
    {
        AttendanceProcessViewModel viewmodel = new AttendanceProcessViewModel();
     

        public MainAttendanceProcess()
        {
            InitializeComponent();
            this.Loaded +=(s,e)=> {this.DataContext =this.viewmodel; };
            addForm();
        }
        public void addForm()
        {
            AttendanceProcessUserControl process = new AttendanceProcessUserControl(this.viewmodel);
            AttendanceMDI.Children.Clear();
            AttendanceMDI.Children.Add(process);
            
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    switch (step)
        //    {
        //        case 0:
        //            ProsessStartMDI processStart = new ProsessStartMDI(this.viewmodel);
        //    AttendanceMDI.Children.Clear();
        //    AttendanceMDI.Children.Add(processStart);
        //    Enable = false;
        //    step = 1;
        //            break;

        //        case 1:
        //            ((WrapPanel)this.Parent).Children.Remove(this);
        //            break;

        //        default:
        //            break;

        //    }
            
        //}

        //private bool _Enable;
        //public bool Enable
        //{
        //    get { return _Enable; }
        //    set { _Enable = value; }
        //}

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            progressBar process = new progressBar(viewmodel);
            AttendanceMDI.Children.Clear();
            AttendanceMDI.Children.Add(process);
        }
        
        
       
    }
}
