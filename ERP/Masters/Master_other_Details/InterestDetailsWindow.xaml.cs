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
    /// Interaction logic for InterestDetailsWindow.xaml
    /// </summary>
    public partial class InterestDetailsWindow : Window
    {

        InterestDetailsViewModel viewmodel;
        public InterestDetailsWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewmodel = new InterestDetailsViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };


        }

        private void InterestDetails_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void InterestDetails_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void InterestDetails_title_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Interest_Details_New_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Interest_Details_Save_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
