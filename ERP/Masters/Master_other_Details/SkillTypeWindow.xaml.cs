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
    /// Interaction logic for SkillTypeWindow.xaml
    /// </summary>
    public partial class SkillTypeWindow : Window
    {
        SkillTypeViewModel viewmodel;
        public SkillTypeWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewmodel = new SkillTypeViewModel();
            this.Loaded += (s, e) => { DataContext = viewmodel; };



        }

        private void SkillType_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SkillType_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SkillType_title_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Skill_Type_New_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Skill_Type_Save_btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
