using ERP.Masters;
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
    /// Interaction logic for ShiftCatergoryMasterWindow.xaml
    /// </summary>
    public partial class ShiftCatergoryMasterWindow : Window
    {
        ShiftCatergoryViewModel viewModel;
        public ShiftCatergoryMasterWindow()
        {
            InitializeComponent();
            viewModel = new ShiftCatergoryViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }

        private void Shift_Categories_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
