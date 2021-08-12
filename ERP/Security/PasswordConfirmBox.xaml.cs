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
    /// Interaction logic for PasswordConfirmBox.xaml
    /// </summary>
    public partial class PasswordConfirmBox : Window
    {

       public PasswordConfirmBox(UserMasterViewModel viewModel)
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewModel; };
        }

       private void Button_Click_1(object sender, RoutedEventArgs e)
       {
           this.Visibility = Visibility.Hidden;
       }
    }
}
