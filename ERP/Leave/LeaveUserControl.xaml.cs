using ERP.MastersDetails;
using ERP.UI_UserControlers;
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


namespace ERP.Leave
{
    /// <summary>
    /// Interaction logic for LeaveUserControl.xaml
    /// </summary>
    public partial class LeaveUserControl : UserControl
    {
        public LeaveUserControl()
        {
            InitializeComponent();
        }

       
        private void Leave_Loaded(object sender, RoutedEventArgs e)
        {
            LeaveButton leavebtn = new LeaveButton(this);
            Mdiwrappanel.Children.Add(leavebtn);
        }
    }
}
