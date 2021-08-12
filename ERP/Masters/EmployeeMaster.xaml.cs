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
using ERP.Masters;

namespace ERP
{
    /// <summary>
    /// Interaction logic for EmployeeMaster.xaml
    /// </summary>
    public partial class EmployeeMaster : UserControl
    {
        EmployeeMasterViewModel viewModel = new EmployeeMasterViewModel();
        //BasicEmployeeDetailUserControl beduc = new BasicEmployeeDetailUserControl();
        //EmployeeDetailUserControl educ = new EmployeeDetailUserControl();

        public EmployeeMaster()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            EmployeeDetailUserControl EmployeeDetails = new EmployeeDetailUserControl();
            Mdi.Children.Clear();
            Mdi.Children.Add(EmployeeDetails);
            this.Mdi.Children.Clear();
            this.Mdi.Children.Add(EmployeeDetails);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //BasicEmployeeDetailUserControl BasicEmployeedetails = new BasicEmployeeDetailUserControl();
            //Mdi.Children.Clear();
            //Mdi.Children.Add(BasicEmployeedetails);
            //this.Mdi.Children.Clear();
            //this.Mdi.Children.Add(BasicEmployeedetails);
        }
    }
}
