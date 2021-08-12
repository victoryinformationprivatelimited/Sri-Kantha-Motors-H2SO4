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

namespace ERP.Attendance.Rosters
{
    /// <summary>
    /// Interaction logic for RosterGroupWindow.xaml
    /// </summary>
    public partial class RosterGroupWindow : Window
    {
        RosterGroupViewModel viewModel;
        public RosterGroupWindow()
        {
            InitializeComponent();
            viewModel = new RosterGroupViewModel();
            Loaded += (s, e) =>
            {
                DataContext = viewModel;
            };
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
