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

namespace ERP.Attendance.Master_Details
{
    /// <summary>
    /// Interaction logic for EmployeeLeaveWindow.xaml
    /// </summary>
    public partial class EmployeeLeaveWindow : Window
    {
        public EmployeeAutoLeaveViewModel viewModel;
        public EmployeeLeaveWindow(Guid EmployeeID)
        {
            InitializeComponent();
            viewModel = new EmployeeAutoLeaveViewModel(EmployeeID);
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void employee_act_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
