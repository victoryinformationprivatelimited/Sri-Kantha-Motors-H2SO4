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

namespace ERP.AttendanceModule.Calendar
{
    /// <summary>
    /// Interaction logic for CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window
    {
        CalendarViewModel ViewModel;
        public CalendarWindow()
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            ViewModel = new CalendarViewModel(this);
            Loaded += (s, e) => { DataContext = ViewModel; };
        }

        public CalendarWindow(Guid? employeeID)
        {
            InitializeComponent();
            this.Owner = clsWindow.Mainform;
            ViewModel = new CalendarViewModel(this, employeeID, 1);
            Loaded += (s, e) => { DataContext = ViewModel; };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
