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

namespace ERP.AttendanceModule.SMS
{
    /// <summary>
    /// Interaction logic for SMSAttendanceWindow.xaml
    /// </summary>
    public partial class SMSAttendanceWindow : Window
    {
        SMSAttendanceViewModel ViewModel;
        public SMSAttendanceWindow()
        {
            InitializeComponent();
            ViewModel = new SMSAttendanceViewModel();

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }
    }
}
