using ERP.MastersDetails;
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
    /// Interaction logic for MaximumLeavesWindow.xaml
    /// </summary>
    public partial class MaximumLeavesWindow : Window
    {
        public MaximumLeavesWindow(EmployeeLeaveDetailViewModel ViewModel)
        {
            InitializeComponent();
            Loaded+=(s,e)=>
            {
                DataContext = ViewModel;
            };
        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
