using ERP.HelperClass;
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
	/// Interaction logic for DeductionsWindow1.xaml
	/// </summary>
	public partial class DeductionsWindow : Window
	{
		private DeductionViewModel viewModel = new DeductionViewModel();
        public DeductionsWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void SearchBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            searchbox.Focus();
        }

        private void deduction_master_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void deduction_master_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void deduction_master_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}