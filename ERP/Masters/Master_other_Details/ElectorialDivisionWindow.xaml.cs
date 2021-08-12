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

namespace ERP.Masters.Master_other_Details
{
    /// <summary>
    /// Interaction logic for ElectorialDivisionWindow.xaml
    /// </summary>
    public partial class ElectorialDivisionWindow : Window
    {
        ElectorialDivisionViewModel ViewModel;
        public ElectorialDivisionWindow()
        {
            InitializeComponent();
            Owner = clsWindow.Mainform;
            ViewModel = new ElectorialDivisionViewModel();
            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
            };
        }

        private void Health_Type_close_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Health_Type_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
