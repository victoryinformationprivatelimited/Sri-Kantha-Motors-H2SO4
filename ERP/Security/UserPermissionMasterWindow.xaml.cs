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
	/// Interaction logic for UserPermissionMasterWindow.xaml
	/// </summary>
	public partial class UserPermissionMasterWindow : Window
	{
		UserPermissionMasterViewModel viewModel = new UserPermissionMasterViewModel();
        public UserPermissionMasterWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
        private void searchbox_TextChanged_4(object sender, TextChangedEventArgs e)
        {
        }
	}
}