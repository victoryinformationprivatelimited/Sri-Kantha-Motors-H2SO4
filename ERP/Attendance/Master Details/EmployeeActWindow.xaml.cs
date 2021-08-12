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
    /// Interaction logic for EmployeeActWindow.xaml
    /// </summary>
    public partial class EmployeeActWindow : Window
    {
        public EmployeeActViewModel viewModel;

        public EmployeeActWindow(Guid EmployeeID,decimal basicSalaray)
        {
            InitializeComponent();
            viewModel = new EmployeeActViewModel(EmployeeID, basicSalaray);
            Loaded += (s, e) => { DataContext = viewModel; };
        }
        
        private void employee_act_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
