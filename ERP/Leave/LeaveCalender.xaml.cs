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
    /// Interaction logic for LeaveCalender.xaml
    /// </summary>
    public partial class LeaveCalender : UserControl
    {
        CalenderViewModel viewModel = new CalenderViewModel(DateTime.Today.Date);
        public LeaveCalender()
        {
            this.Loaded += (s, e) => { this.DataContext = viewModel;};
            InitializeComponent();
        }

        private void DatePicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            refreshCalender();
        }

        private void refreshCalender()
        {
            this.CalenderMDI.Children.Clear();
            CalenderUserControl cuc = new CalenderUserControl(viewModel);
            this.CalenderMDI.Children.Add(cuc);
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            refreshCalender();
        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            refreshCalender();
        }

        private void ComboBox_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {
            refreshCalender();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ((WrapPanel)this.Parent).Children.Remove(this);
        }


    }
}
