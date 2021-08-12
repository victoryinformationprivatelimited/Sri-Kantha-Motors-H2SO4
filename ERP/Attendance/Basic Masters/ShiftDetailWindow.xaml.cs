using ERP.MastersDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERP.Attendance.Basic_Masters
{
    /// <summary>
    /// Interaction logic for ShiftDetailWindow.xaml
    /// </summary>
    public partial class ShiftDetailWindow : Window
    {
        ShiftDetailViewModel viewModel;

        public ShiftDetailWindow()
        {
            InitializeComponent();
            viewModel = new ShiftDetailViewModel();
            Loaded += (s, e) => 
            {
                DataContext = viewModel;
            };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBox.Focus();
        }

        private void Shift_Detail_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
