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
	/// Interaction logic for ERPBankBranchWindow1.xaml
	/// </summary>
	public partial class ERPBankBranchWindow : Window
	{
		ERPBankBranchViewModel viewModel;
        public ERPBankBranchWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new ERPBankBranchViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void bank_branch_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void bank_branch_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void bank_branch_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}