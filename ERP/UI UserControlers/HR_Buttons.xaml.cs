using ERP.Attendance;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP.Leave;
using ERP.Medical;
using ERP.Loan_Module;
using ERP.Performance;
using ERP.Training;
using ERP.WebPortal;
using ERP.Performance.Evaluation;

namespace ERP.UI_UserControlers
{
    /// <summary>
    /// Interaction logic for HR_Buttons.xaml
    /// </summary>
    public partial class HR_Buttons : UserControl
    {
        WrapPanel SubWrap;

        public HR_Buttons(HRUserControl HrUsrContrl)
        {
            SubWrap = HrUsrContrl.hr_sub_buttons;
            InitializeComponent();
        }

        private void Attendence_check_box_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AttendenceUserControl auc = new AttendenceUserControl();
                SubWrap.Children.Clear();
                SubWrap.Children.Add(auc);
            }
            catch (Exception)
            {

            }
        }

        private void Leave_check_box_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LeaveUserControl luc = new LeaveUserControl();
                SubWrap.Children.Clear();
                SubWrap.Children.Add(luc);
            }
            catch (Exception)
            {

            }
        }

        private void Payroll_check_box_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PayrollUserControl puc = new PayrollUserControl();
                SubWrap.Children.Clear();
                SubWrap.Children.Add(puc);
            }
            catch (Exception)
            {

            }
        }

        private void Loan_check_box_Click(object sender, RoutedEventArgs e)
        {
            LoanUserControl usermasUcon = new LoanUserControl();
            SubWrap.Children.Clear();
            SubWrap.Children.Add(usermasUcon);
        }

        private void Medical_check_box_Click(object sender, RoutedEventArgs e)
        {
            MedicalUserControl userConMed = new MedicalUserControl();
            SubWrap.Children.Clear();
            SubWrap.Children.Add(userConMed);
        }

        private void Performance_check_box_Click(object sender, RoutedEventArgs e)
        {
            EvaluationMDIUserControl userPerformance = new EvaluationMDIUserControl();
            SubWrap.Children.Clear();
            SubWrap.Children.Add(userPerformance);
        }

        private void Tasks_check_box_Click(object sender, RoutedEventArgs e)
        {
            PerformanceMDIUserControl userPerformance = new PerformanceMDIUserControl();
            SubWrap.Children.Clear();
            SubWrap.Children.Add(userPerformance);
        }

        private void Training_check_box_Click(object sender, RoutedEventArgs e)
        {
            TrainingUserControl userTraining = new TrainingUserControl();
            SubWrap.Children.Clear();
            SubWrap.Children.Add(userTraining);
        }

        private void Portal_check_box_Click(object sender, RoutedEventArgs e)
        {
            WebPortalUserControl Portal = new WebPortalUserControl();
            SubWrap.Children.Clear();
            SubWrap.Children.Add(Portal);
        }
    }

}
