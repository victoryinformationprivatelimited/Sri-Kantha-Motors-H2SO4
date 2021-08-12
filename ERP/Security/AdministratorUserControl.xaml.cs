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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP.Security;
using ERP.UI_UserControlers;

namespace ERP
{
    /// <summary>
    /// Interaction logic for AdministratorUserControl.xaml
    /// </summary>
    public partial class AdministratorUserControl : UserControl
    {
        public AdministratorUserControl()
        {
            InitializeComponent();
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AdminButtons1 adminbtn = new AdminButtons1(this);
            Mdiwrappanel.Children.Add(adminbtn);
        }
    }
}
