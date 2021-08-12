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
    /// Interaction logic for InstituteGradeWindow.xaml
    /// </summary>
    public partial class InstituteGradeWindow : Window
    {
        InstituteGradeViewModel viewmodel;
        public InstituteGradeWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewmodel = new InstituteGradeViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };
        }

        private void InstituteGrade_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void InstituteGrade_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void InstituteGrade_title_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Institute_GradeNew_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Institute_Grade_Save_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
