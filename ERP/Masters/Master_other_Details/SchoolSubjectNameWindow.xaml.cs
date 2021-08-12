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
    /// Interaction logic for SchoolSubjectNameWindow.xaml
    /// </summary>
    public partial class SchoolSubjectNameWindow : Window
    {
        SchoolSubjectNameViewModel viewmodel;
        public SchoolSubjectNameWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
           viewmodel= new SchoolSubjectNameViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };
        }

        private void School_Subject_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void School_Subject_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void School_Subject_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void School_Subject_New_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void School_Subject_Save_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
