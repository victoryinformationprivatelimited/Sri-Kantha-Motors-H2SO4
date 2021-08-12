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

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
	/// <summary>
	/// Interaction logic for PaySheetFilterWindow.xaml
	/// </summary>
	public partial class PaySheetFilterWindow : Window
	{
        public PaySheetFilterWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            EmployeePaySheetControlViewModel viewmodel = new EmployeePaySheetControlViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };

        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}