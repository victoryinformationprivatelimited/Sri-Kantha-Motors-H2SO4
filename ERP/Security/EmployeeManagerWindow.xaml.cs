using ERP.HelperClass;
using ERP.Security;
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
	/// Interaction logic for EmployeeManagerWindow.xaml
	/// </summary>
	public partial class EmployeeManagerWindow : Window
	{
		EmployeeManagerViewModel viewModel;
        public EmployeeManagerWindow()
        {
            
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new EmployeeManagerViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			this.DragMove();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	// TODO: Add event handler implementation here.
			this.DragMove();
        }
	}
}