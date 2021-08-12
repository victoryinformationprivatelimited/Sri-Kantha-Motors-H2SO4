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

namespace ERP.AttendanceModule.AttendanceMasters
{
    /// <summary>
    /// Interaction logic for AssignedShiftPreviewWindow.xaml
    /// </summary>
    public partial class AssignedShiftPreviewWindow : Window
    {
        IEnumerable<WeekDayShift> shiftDays;
        public IEnumerable<WeekDayShift> ShiftDays
        {
            get { if (shiftDays != null) shiftDays = shiftDays.OrderBy(c => c.DateOfDay.Date); return shiftDays; }
            set { shiftDays = value;}
        }

        //int NumRows = 5;
        //int NumCols = 5;
        public AssignedShiftPreviewWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { DataContext = this; };
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }
    }
}
