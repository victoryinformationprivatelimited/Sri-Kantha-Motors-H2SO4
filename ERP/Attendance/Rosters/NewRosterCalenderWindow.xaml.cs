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
    /// Interaction logic for NewRosterCalenderWindow.xaml
    /// </summary>
    public partial class NewRosterCalenderWindow : Window
    {
        NewRosterCalenderViewModel viewModel;
        public NewRosterCalenderWindow()
        {
            InitializeComponent();
            viewModel = new NewRosterCalenderViewModel();
            Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}