using ERP.HelperClass;
using ERP.Medical;
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
	/// Interaction logic for EmployeeMedicalWindow1.xaml
	/// </summary>
	public partial class EmployeeMedicalWindow : Window
	{
        EmployeeMedicalViewModel viewModel;

        public EmployeeMedicalWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new EmployeeMedicalViewModel();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }

        private void emp_medic_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_medic_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void emp_medic_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}