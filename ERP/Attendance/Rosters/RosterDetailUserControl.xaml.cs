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
    /// Interaction logic for RosterDetailUserControl.xaml
    /// </summary>
    public partial class RosterDetailUserControl : UserControl
    {
        RosterDetailViewModel viewModel;
        public RosterDetailUserControl()
        {
            InitializeComponent();
            viewModel = new RosterDetailViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }
    }
}