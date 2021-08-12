using ERP.HelperClass;
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
	/// Interaction logic for EmployeeResignWindow.xaml
	/// </summary>
	public partial class EmployeeResignWindow : Window
	{
		EmployeeResignDetailsViewModel viewModel = new EmployeeResignDetailsViewModel();

        public EmployeeResignWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            resign.Items.Add("3");
            resign.Items.Add("6");
            resign.Items.Add("9");
            resign.Items.Add("12");



            this.Loaded += (s, e) =>
                {
                    DataContext = viewModel;
                }; 



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