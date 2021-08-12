using CustomBusyBox;
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

namespace ERP.Payroll.ThakralDB
{
    /// <summary>
    /// Interaction logic for MigrateOtherPaymentsDetailsWindow.xaml
    /// </summary>
    public partial class MigrateOtherPaymentsDetailsWindow : Window
    {
        MigrateOtherPaymentsDetailsViewModel viewModel;
        public MigrateOtherPaymentsDetailsWindow()
        {
            InitializeComponent();
            BusyBox.InitializeThread(this);
            viewModel = new MigrateOtherPaymentsDetailsViewModel();
            this.Loaded += (s, e) => { this.DataContext = this.viewModel; };
        }
    }
}
