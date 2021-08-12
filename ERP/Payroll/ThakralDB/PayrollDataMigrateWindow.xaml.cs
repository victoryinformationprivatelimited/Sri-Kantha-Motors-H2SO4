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
    /// Interaction logic for PayrollDataMigrateWindow.xaml
    /// </summary>
    public partial class PayrollDataMigrateWindow : Window
    {
        MigrateSalaryDetailsWindow MigrateSalaryDetailsW;
        MigrateOtherPaymentsDetailsWindow MigrateOtherPaymentsDetailsW;
        public PayrollDataMigrateWindow()
        {
            InitializeComponent();
        }

        private void CloseForm()
        {
            if (MigrateSalaryDetailsW != null)
                MigrateSalaryDetailsW.Close();
            if (MigrateOtherPaymentsDetailsW != null)
                MigrateOtherPaymentsDetailsW.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseForm();
            MigrateSalaryDetailsW = new MigrateSalaryDetailsWindow();
            MigrateSalaryDetailsW.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CloseForm();
            MigrateOtherPaymentsDetailsW = new MigrateOtherPaymentsDetailsWindow();
            MigrateOtherPaymentsDetailsW.Show();
        }
    }
}
