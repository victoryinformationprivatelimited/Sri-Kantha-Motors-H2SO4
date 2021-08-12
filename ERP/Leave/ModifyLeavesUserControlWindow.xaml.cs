using ERP.Performance;
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

namespace ERP.Leave
{
    /// <summary>
    /// Interaction logic for ModifyLeavesUserControlWindow.xaml
    /// </summary>
    public partial class ModifyLeavesUserControlWindow : Window
    {
        ModifyLeavesViewModel ViewModel;
        public ModifyLeavesUserControlWindow()
        {
            InitializeComponent();
            ViewModel = new ModifyLeavesViewModel();

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
