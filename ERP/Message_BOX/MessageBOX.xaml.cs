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

namespace ERP.Message_BOX
{
    /// <summary>
    /// Interaction logic for MessageBOX.xaml
    /// </summary>
    public partial class MessageBOX : Window
    {
       public  MessageBOXviewModel viewModel;

        public MessageBOX()
        {
            viewModel = new MessageBOXviewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        public MessageBOX(String Title,String Content)
        {
            clsWindow.Mainform = null;
            this.Owner = clsWindow.Mainform;
            viewModel = new MessageBOXviewModel();
            viewModel.Content = Content;
            viewModel.Title = Title;
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        public MessageBOX(String Title, String Content,Visibility Cancel)
        {
            this.Owner = clsWindow.Mainform;
            viewModel = new MessageBOXviewModel( Cancel);
            viewModel.Content = Content;
            viewModel.Title = Title;
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void Rectangle_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            viewModel.Result = null;
            this.Close();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
