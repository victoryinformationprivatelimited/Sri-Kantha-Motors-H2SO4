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
    /// Interaction logic for BonuOtherSendSMSWindow.xaml
    /// </summary>
    public partial class BonuOtherSendSMSWindow : Window
    {
        BonusOtherSendSMSViewModel viewModel;
        public BonuOtherSendSMSWindow()
        {
            BusyBox.InitializeThread(this);
            viewModel = new BonusOtherSendSMSViewModel();
            InitializeComponent();
            Loaded += (s, e) => { DataContext = viewModel; };
        }
    }
}
