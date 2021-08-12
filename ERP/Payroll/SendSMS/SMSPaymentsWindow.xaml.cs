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

namespace ERP.Payroll.SendSMS
{
    /// <summary>
    /// Interaction logic for SMSPaymentsWindow.xaml
    /// </summary>
    public partial class SMSPaymentsWindow : Window
    {
        SendSMSWindow SendSMSW;
        BonuOtherSendSMSWindow BonuOtherSendSMSW;
        ThirdPartySendSMSWindow ThirdPartySendSMSW;
        public SMSPaymentsWindow()
        {
            InitializeComponent();
        }

        private void FormClose()
        {
            if (SendSMSW != null)
                SendSMSW.Close();
            if (BonuOtherSendSMSW != null)
                BonuOtherSendSMSW.Close();
            if (ThirdPartySendSMSW != null)
                ThirdPartySendSMSW.Close();
        }

        private void Salary_Button_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsSecurity.GetViewPermission(109))
                {
                    FormClose();
                    SendSMSW = new SendSMSWindow();
                    SendSMSW.Show(); 
                }
                else
                    clsMessages.setMessage("You don't have permission to view this form");
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message.ToString());
            }
        }

        private void BonusOther_Button_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsSecurity.GetViewPermission(110))
                {
                    FormClose();
                    BonuOtherSendSMSW = new BonuOtherSendSMSWindow();
                    BonuOtherSendSMSW.Show(); 
                }
                else
                    clsMessages.setMessage("You don't have permission to view this form");
            }
            catch (Exception ex)
            {
                clsMessages.setMessage(ex.Message.ToString());
            }
        }

        private void ThirdParty_Button_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clsSecurity.GetViewPermission(111))
                {
                    FormClose();
                    ThirdPartySendSMSW = new ThirdPartySendSMSWindow();
                    ThirdPartySendSMSW.Show();
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
