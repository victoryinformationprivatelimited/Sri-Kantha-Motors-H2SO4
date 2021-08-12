using ERP.HelperClass;
using ERP.Masters;
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
    /// Interaction logic for CompanyVariablsWindow1.xaml
    /// </summary>
    public partial class CompanyVariablsWindow : Window
    {
        private CompanyVariablesViewModel viewModel = new CompanyVariablesViewModel();
        public CompanyVariablsWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }


        private void searchbox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
        }

        private void company_variable_master_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void company_variable_master_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void company_variable_master_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}