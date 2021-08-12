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
    /// Interaction logic for ExtraCurriculamActivityWindow.xaml
    /// </summary>
    public partial class ExtraCurriculamActivityWindow : Window
    {
        ExtraCurriculamActivityViewModel viewmodel;
        public ExtraCurriculamActivityWindow()
        {
            InitializeComponent();
            viewmodel = new ExtraCurriculamActivityViewModel();
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewmodel; };
        }

        private void Extra_curriculam_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }

        private void Extra_curriculam_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }

        private void Extra_curriculam_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Ecactivity_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Curr_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
