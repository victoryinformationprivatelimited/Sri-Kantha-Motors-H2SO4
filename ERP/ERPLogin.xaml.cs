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

namespace ERP
{
    /// <summary>
    /// Interaction logic for ERPLogin.xaml
    /// </summary>
    public partial class ERPLogin : Window
    {
        private ERPLoginViewModel viewModel = new ERPLoginViewModel();

        public ERPLogin()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
           
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void pwd_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }


    }
}
