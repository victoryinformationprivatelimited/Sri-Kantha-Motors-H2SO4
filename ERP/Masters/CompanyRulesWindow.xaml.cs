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
	/// Interaction logic for CompanyRulesWindow1.xaml
	/// </summary>
	public partial class CompanyRulesWindow : Window
	{
		private CompanyRulesViewModel viewModel = new CompanyRulesViewModel();
        public CompanyRulesWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void SearchBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            
        }
        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
	}
}