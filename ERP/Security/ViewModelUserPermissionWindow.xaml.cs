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
	/// Interaction logic for ViewModelUserPermissionWindow1.xaml
	/// </summary>
	public partial class ViewModelUserPermissionWindow : Window
	{
		ViewModelUserPermissionsViewModel viewModel = new ViewModelUserPermissionsViewModel();

        public ViewModelUserPermissionWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}