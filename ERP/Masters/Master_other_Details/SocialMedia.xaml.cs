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
    /// Interaction logic for SocialMedia.xaml
    /// </summary>
    public partial class SocialMedia : Window
    {
        SocialMediaviewModel viewModel;
        public SocialMedia()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new SocialMediaviewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void social_media_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void social_media_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void social_media_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SocialMediaNew_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SocialMediaSave_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
