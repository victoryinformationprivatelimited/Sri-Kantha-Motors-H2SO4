using ERP.HelperClass;
using ERP.Reports.Documents.HR_Report.User_Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP
{
	/// <summary>
	/// Interaction logic for EmployeeBaasicSalaryWindow.xaml
	/// </summary>
	public partial class EmployeeBaasicSalaryWindow : Window
	{
		EmployeeBasicSalaryViewModel viewmodel = new EmployeeBasicSalaryViewModel();
        public EmployeeBaasicSalaryWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}