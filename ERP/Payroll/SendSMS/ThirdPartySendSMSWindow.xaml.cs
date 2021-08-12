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
using CustomBusyBox;

namespace ERP.Payroll.SendSMS
{
    /// <summary>
    /// Interaction logic for ThirdPartySendSMSWindow.xaml
    /// </summary>
    public partial class ThirdPartySendSMSWindow : Window
    {
        ThirdPartySendSMSViewModel viewModel;
        public ThirdPartySendSMSWindow()
        {
            BusyBox.InitializeThread(this);
            viewModel = new ThirdPartySendSMSViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}
