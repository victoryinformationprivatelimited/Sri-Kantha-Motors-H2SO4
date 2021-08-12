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
    /// Interaction logic for QualificationCategoryNameWindow.xaml
    /// </summary>
    public partial class QualificationCategoryNameWindow : Window
    {
        QualificationCategoryNameViewModel viewmodel;
        public QualificationCategoryNameWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewmodel = new QualificationCategoryNameViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };

        }

        private void Qualification_Category_Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Qualification_Category_Name_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Qualification_Category_Name_titlebar_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }  

        private void Qualification_Category_Name_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Qualification_Category_New_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Qualification_Category_save_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
