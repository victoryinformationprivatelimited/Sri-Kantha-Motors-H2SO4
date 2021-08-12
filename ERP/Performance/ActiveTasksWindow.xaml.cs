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

namespace ERP.Performance
{
    /// <summary>
    /// Interaction logic for ActiveTasksWindow.xaml
    /// </summary>
    public partial class ActiveTasksWindow : Window
    {
        EmployeeTasksViewerWindow MainWindow;

        public ActiveTasksWindow(EmployeeTasksViewerViewmodel ViewModel)
        {
            MainWindow = ViewModel.MainWindow;

            InitializeComponent();

            Loaded += (s, e) => 
            {
                DataContext = ViewModel;
                this.Owner = MainWindow;
            };
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
