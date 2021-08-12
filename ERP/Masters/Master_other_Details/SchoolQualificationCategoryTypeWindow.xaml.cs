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
    /// Interaction logic for SchoolQualificationCategoryTypeWindow.xaml
    /// </summary>
    public partial class SchoolQualificationCategoryTypeWindow : Window
    {
        SchoolQualificationCategoryTypeViewModel viewmodel;
        public SchoolQualificationCategoryTypeWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewmodel = new SchoolQualificationCategoryTypeViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };
        }

        private void School_qualification_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void School_qualification_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void School_qualification_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void School_Qualification_CategoryNew_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void School_Qualification_Categor_Delete_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
