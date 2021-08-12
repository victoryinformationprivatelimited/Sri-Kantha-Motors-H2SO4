using ERP.HelperClass;
using ERP.Payroll;
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
	/// Interaction logic for CompanyVariableWindow1.xaml
	/// </summary>
	public partial class CompanyVariableWindow : Window
	{
		CompanyVariableViewModel viewModel = new CompanyVariableViewModel();
        public CompanyVariableWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }

        private void company_variable_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void company_variable_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void company_variable_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}