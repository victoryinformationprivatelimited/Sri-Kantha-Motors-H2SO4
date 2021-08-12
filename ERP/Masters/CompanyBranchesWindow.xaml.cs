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
	/// Interaction logic for CompanyBranchesWindow1.xaml
	/// </summary>
	public partial class CompanyBranchesWindow : Window
	{
        private CompanyBranchesViewModel viewModel;
        public CompanyBranchesWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new CompanyBranchesViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void company_branch_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void company_branch_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void company_branch_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}