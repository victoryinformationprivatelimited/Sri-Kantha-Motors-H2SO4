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
	/// Interaction logic for EmployeeDetailsWindow.xaml
	/// </summary>
	public partial class EmployeeDetailsWindow : Window
	{
		EmployeeDetailsViewmodel viewModel=new EmployeeDetailsViewmodel();

        public EmployeeDetailsWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
                {
                    DataContext = viewModel;
                };
        }


        private void closeClick(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
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