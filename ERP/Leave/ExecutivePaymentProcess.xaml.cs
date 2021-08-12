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
    /// Interaction logic for ExecutivePaymentProcess.xaml
    /// </summary>
    public partial class ExecutivePaymentProcess : Window
    {
        ExecutivePaymentProcessViewModel ViewModel;
        public ExecutivePaymentProcess()
        {
            InitializeComponent();
            ViewModel = new ExecutivePaymentProcessViewModel();

            Loaded += (s, e) =>
            {
                DataContext = ViewModel;
            };
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


    }
}
