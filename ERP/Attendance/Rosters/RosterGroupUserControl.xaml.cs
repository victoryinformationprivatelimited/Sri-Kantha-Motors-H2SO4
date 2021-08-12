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

namespace ERP.Attendance.Rosters
{
    /// <summary>
    /// Interaction logic for RosterGroupUserControl.xaml
    /// </summary>
    public partial class RosterGroupUserControl : UserControl
    {
        RosterGroupViewModel viewMode;
        public RosterGroupUserControl()
        {
            InitializeComponent();
            viewMode = new RosterGroupViewModel();
            Loaded += (s, e) => { DataContext = viewMode; };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}