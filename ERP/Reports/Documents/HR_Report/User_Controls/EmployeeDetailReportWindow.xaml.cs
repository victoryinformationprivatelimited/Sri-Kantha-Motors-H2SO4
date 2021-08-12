using ERP.Reports.Documents.HR_Report.User_Controls;
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
	/// Interaction logic for EmployeeDetailReportWindow.xaml
	/// </summary>
	public partial class EmployeeDetailReportWindow : Window
	{
		EmployeeDetailViewModel viewmodel = new EmployeeDetailViewModel();
        public EmployeeDetailReportWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
	}
}