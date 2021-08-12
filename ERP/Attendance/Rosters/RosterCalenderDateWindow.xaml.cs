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
    /// Interaction logic for RosterCalenderDateWindow.xaml
    /// </summary>
    public partial class RosterCalenderDateWindow : Window
    {
        RosterCalenderDateViewModel viewModel;
        public RosterCalenderDateWindow(DateTime? calenderDate)
        {
            InitializeComponent();
            viewModel = new RosterCalenderDateViewModel(calenderDate);
            Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}