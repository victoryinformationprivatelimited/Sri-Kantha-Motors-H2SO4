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
//using System.Windows.Controls.ComboBox;

namespace ERP.Payroll
{
    /// <summary>
    /// Interaction logic for sinhala_payslip.xaml
    /// </summary>
    public partial class sinhala_payslip : Window
    {
        sinhalapayrule db = new sinhalapayrule();
        public sinhala_payslip()
        {
            InitializeComponent();
            CallCmb();
        }
        public void CallCmb()
        {
            cmb1.ItemsSource = db.GetRuleName().DefaultView;
        }

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {

            if (clsSecurity.GetSavePermission(514))
            {
                if (string.IsNullOrEmpty(text1.Text) == true)
                {
                    clsMessages.setMessage("Enter Sinhala Rule Name");
                }
                else if (cmb1.SelectedIndex == -1)
                {
                    clsMessages.setMessage("Select Rule Name");
                }
                else
                {
                    int i = db.SaveSinalaName(cmb1.SelectedValue.ToString(), text1.Text);
                    if (i > 0)
                    {
                        clsMessages.setMessage("Save is Success");
                    }

                    else
                    {
                        clsMessages.setMessage("Save Failed");
                    }
                } 
            }
            else
                clsMessages.setMessage("You don't have permission to Save this record(s)");
        }//get data for combo box
    }


}
