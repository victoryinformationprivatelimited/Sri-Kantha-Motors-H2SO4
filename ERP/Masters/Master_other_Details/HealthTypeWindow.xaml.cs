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
    /// Interaction logic for HealthTypeWindow.xaml
    /// </summary>
    public partial class HealthTypeWindow : Window
    {
        HealthTypeViewModel viewmodel;
        public HealthTypeWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewmodel = new HealthTypeViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };
        }

        private void Health_Type_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Health_Type_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Health_Type_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Health_Type_New_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Health_Type_save_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
