
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
	/// Interaction logic for LeaveRegisterReport.xaml
	/// </summary>
	public partial class LeaveRegisterReport : Window
	{

        LeaveRegisterViewModel viewModel;
		public LeaveRegisterReport()
		{
            viewModel = new LeaveRegisterViewModel();
			this.InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
			
			// Insert code required on object creation below this point.
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}