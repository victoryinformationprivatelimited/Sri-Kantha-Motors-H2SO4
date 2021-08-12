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

namespace ERP.Security
{
    /// <summary>
    /// Interaction logic for OnlineLeaveUserWindow.xaml
    /// </summary>
    public partial class OnlineLeaveUserWindow : Window
    {
        OnlineLeaveUserViewModel viewModel;
        public OnlineLeaveUserWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            viewModel = new OnlineLeaveUserViewModel();
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void leaveUserCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void leaveUserTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void leaveUserTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void shwCharChkBox_Checked(object sender, RoutedEventArgs e)
        {
            pwBox.PasswordChar = '\0';
        }

        private void shwCharChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            pwBox.PasswordChar = '.';
        }
    }
}
