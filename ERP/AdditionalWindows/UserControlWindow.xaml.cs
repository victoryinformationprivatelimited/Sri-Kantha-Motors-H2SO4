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

namespace ERP.AdditionalWindows
{
    /// <summary>
    /// Interaction logic for UserControlWindow.xaml
    /// </summary>
    public partial class UserControlWindow : Window
    {
        public UserControlWindow(dynamic UControl)
        {
            this.Owner = clsWindow.Mainform;
            try
            {
                UControl.Windows(this);
            }
            catch (Exception)
            {
            }
            InitializeComponent();
            Wrp.Children.Add(UControl);

        }

        private void Window_MouseLeave_1(object sender, MouseEventArgs e)
        {
            if (Wrp.Children.Count == 0 || Wrp.Children == null)
            {
                Close();
            }
        }



        private void userGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Wrp.Children.Count == 0 || Wrp.Children == null)
            {
                Close();
            }
        }

        private void Grid_MouseLeave_2(object sender, MouseEventArgs e)
        {
            if (Wrp.Children.Count == 0 || Wrp.Children == null)
            {
                Close();
            }
        }

    }
}
