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

namespace ERP.AttendanceModule.Calendar.Calendar_Items
{
    /// <summary>
    /// Interaction logic for DateUserControl.xaml
    /// </summary>
    public partial class DateUserControl : UserControl
    {
        CalendarViewModel ViewModel;
       public int Dateselected = 0;
       public DateUserControl(CalendarViewModel ViewModel)
        {
           
            InitializeComponent();
            this.ViewModel = ViewModel;
            Loaded += (s, e) => { DataContext = this.ViewModel; };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedDate(Dateselected.ToString());
        }
    }
}
