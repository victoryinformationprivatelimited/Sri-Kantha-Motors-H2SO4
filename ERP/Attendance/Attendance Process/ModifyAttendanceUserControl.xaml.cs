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
    /// Interaction logic for ModifyAttendanceUserControl.xaml
    /// </summary>
    public partial class ModifyAttendanceUserControl : UserControl
    {
        ModifyAttendanceViewmodel viewModel;

        public ModifyAttendanceUserControl()
        {
            InitializeComponent();
            viewModel = new ModifyAttendanceViewmodel();
            Loaded+=(s,e)=>{DataContext = viewModel;};
        }


        #region DragMove
        Window LoadedWindow = new Window();
        public void Windows(Window LoadedWindow)
        {
            this.LoadedWindow = LoadedWindow;
        }
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LoadedWindow.DragMove();
        }
        #endregion

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }

        private void DataGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            viewModel.ShowApprovalWindow();
        }
    }
}
