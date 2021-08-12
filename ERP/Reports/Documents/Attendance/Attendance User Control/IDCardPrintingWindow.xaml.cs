using ERP.HelperClass;
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

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    /// <summary>
    /// Interaction logic for IDCardPrintingWindow.xaml
    /// </summary>
    public partial class IDCardPrintingWindow : Window
    {
        IDCardPrintingViewModel viewModel;

        public IDCardPrintingWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new IDCardPrintingViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };

        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
