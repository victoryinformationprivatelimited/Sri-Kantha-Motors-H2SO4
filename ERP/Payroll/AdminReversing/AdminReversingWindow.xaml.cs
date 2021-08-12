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

namespace ERP.Payroll.AdminReversing
{
    /// <summary>
    /// Interaction logic for AdminReversingWindow.xaml
    /// </summary>
    public partial class AdminReversingWindow : Window
    {
        ReversBonusAndOtherPaymentsWindow ReversBonusAndOtherPaymentsW;
        ReverseExternalBankLoansWindow ReverseExternalBankLoansW;
        ReverseInternalLoanWindow ReverseInternalLoanW;
        public AdminReversingWindow()
        {
            InitializeComponent();
        }

        private void FormClose()
        {
            if (ReversBonusAndOtherPaymentsW != null)
                ReversBonusAndOtherPaymentsW.Close();
            if (ReverseExternalBankLoansW != null)
                ReverseExternalBankLoansW.Close();
            if (ReverseInternalLoanW != null)
                ReverseInternalLoanW.Close();
        }

        private void Reverse_Bonus_Other_Payments(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsSecurity.GetViewPermission(113))
                {
                    FormClose();
                    ReversBonusAndOtherPaymentsW = new ReversBonusAndOtherPaymentsWindow();
                    ReversBonusAndOtherPaymentsW.Show();
                }
                else
                    clsMessages.setMessage("You don't have permission to view this form");
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message.ToString());
            }
        }

        private void Reverse_External_Bank_Loans(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsSecurity.GetViewPermission(114))
                {
                    FormClose();
                    ReverseExternalBankLoansW = new ReverseExternalBankLoansWindow();
                    ReverseExternalBankLoansW.Show();
                }
                else
                    clsMessages.setMessage("You don't have permission to view this form");
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message.ToString());
            }
        }

        private void Reverse_Internal_Loans(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsSecurity.GetViewPermission(115))
                {
                    FormClose();
                    ReverseInternalLoanW = new ReverseInternalLoanWindow();
                    ReverseInternalLoanW.Show();
                }
                else
                    clsMessages.setMessage("You don't have permission to view this form");
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message.ToString());
            }
        }
    }
}
