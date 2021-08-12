using ERP.HelperClass;
using ERP.Reports.Documents.Leave.LeaveUserControl;
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
	/// Interaction logic for LeaveRegisterWindow.xaml
	/// </summary>
	public partial class LeaveRegisterWindow : Window
	{
		 LeaveRegisterViewModel viewModel;
		public LeaveRegisterWindow()
		{
			this.InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new LeaveRegisterViewModel();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
			
			// Insert code required on object creation below this point.
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void leave_register_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void leave_register_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
	}
}