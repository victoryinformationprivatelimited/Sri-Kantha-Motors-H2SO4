using CustomBusyBox;
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

namespace ERP.AttendanceModule.AttendanceProcessMaster
{
    /// <summary>
    /// Interaction logic for AttendanceSaveFromAPI.xaml
    /// </summary>
    public partial class AttendanceSaveFromAPI : Window
    {
        AttendanceSaveFromAPIViewModel viewModel;
        public AttendanceSaveFromAPI()
        {
            InitializeComponent();
            BusyBox.InitializeThread(this);
            viewModel = new AttendanceSaveFromAPIViewModel();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void Data_Download_Master_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
