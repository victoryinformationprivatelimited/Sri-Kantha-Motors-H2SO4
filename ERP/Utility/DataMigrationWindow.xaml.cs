using CustomBusyBox;
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

namespace ERP.Utility
{
    /// <summary>
    /// Interaction logic for DataMigrationWindow.xaml
    /// </summary>
    public partial class DataMigrationWindow : Window
    {
        DataMigrationViewModel viewModel = new DataMigrationViewModel();
        public DataMigrationWindow()
        {
            InitializeComponent();
            BusyBox.InitializeThread(this);
            this.Owner = clsWindow.Mainform;
            this.Loaded += (s, e) => { DataContext = viewModel; };
        }

        private void data_migrate_title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void data_migrate_titlebar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void data_migrate_close_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
