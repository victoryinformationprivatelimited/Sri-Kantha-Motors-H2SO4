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

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    /// <summary>
    /// Interaction logic for EmployeeUniformOfferingWindow.xaml
    /// </summary>
    public partial class EmployeeUniformOfferingWindow : Window
    {
        EmployeeUniformOfferingViewModel viewModel;

        public EmployeeUniformOfferingWindow()
        {
            this.Owner = clsWindow.Mainform;
            InitializeComponent();

            searchOptionCMB.Items.Add("Employee ID");
            searchOptionCMB.Items.Add("First Name");
            searchOptionCMB.Items.Add("Uniform Name");
            searchOptionCMB.Items.Add("Designation");

            viewModel = new EmployeeUniformOfferingViewModel();
            Loaded += (s, e) =>
            {
                this.DataContext = viewModel;
            };
        }

        private void button2_Copy_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
