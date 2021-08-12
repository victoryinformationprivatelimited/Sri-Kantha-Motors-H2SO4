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

namespace ERP.AttendanceModule.AttendanceMasters
{
	/// <summary>
	/// Interaction logic for Window2.xaml
	/// </summary>
	public partial class ShiftCategory : Window
	{
        ShiftCategoryViewModel viewModel;
		public ShiftCategory()
		{
            this.Owner = clsWindow.Mainform;
			this.InitializeComponent();
            viewModel = new ShiftCategoryViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
		}

        private void addShiftCategoryTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void addShiftCategoryTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void shiftCategoryCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}