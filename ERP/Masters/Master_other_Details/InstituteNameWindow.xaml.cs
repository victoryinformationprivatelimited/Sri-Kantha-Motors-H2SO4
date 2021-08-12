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
    /// Interaction logic for InstituteNameWindow.xaml
    /// </summary>
    public partial class InstituteNameWindow : Window
    {
        InstituteNameViewModel viewmodel;
        public InstituteNameWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewmodel = new InstituteNameViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };
        }

        private void Institute_Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Institute_Name_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Institute_Name_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Institute_Name_New_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Institute_Name_save_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
