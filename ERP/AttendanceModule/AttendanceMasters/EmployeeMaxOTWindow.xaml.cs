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
    /// Interaction logic for EmployeeMaxOTWindow.xaml
    /// </summary>
    public partial class EmployeeMaxOTWindow : Window
    {
        EmployeeMaxOTViewModel viewModel;
        public EmployeeMaxOTWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new EmployeeMaxOTViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
            
        }

        private void maxOtTitle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	this.DragMove();
        }

        private void maxOtTitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	this.DragMove();
        }

        private void maxOtCloseBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	this.Close();
        }

        private void removeGrpBtn_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.RemoveSelectedGroup();
        }

        private void removeDateBtn_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.RemoveSelectedDate();
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Delete)
            {
                this.viewModel.RemoveSelectedDate();
            }
        }
    }
}
