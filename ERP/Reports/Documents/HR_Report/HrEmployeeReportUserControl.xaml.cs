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
using ERP.Reports.Documents.HR_Report.User_Controls;
using ERP.HR.HR_Report_Window;

namespace ERP.Reports.Documents.HR_Report
{
    /// <summary>
    /// Interaction logic for HrEmployeeReportUserControl.xaml
    /// </summary>
    public partial class HrEmployeeReportUserControl : UserControl
    {
        NewHrReportsWindow Window;
        LPEmployeeDetailsWindow LPEmployeeDetailsW;

        public HrEmployeeReportUserControl()
        {
            InitializeComponent();
        }

        private void WindowClose()
        {
            if (Window != null)
                Window.Close();
            if (LPEmployeeDetailsW != null)
                LPEmployeeDetailsW.Close();
        }

        private void Address_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewAddressReportViewModel ViewModel = new NewAddressReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void Age_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewAgeReportViewModel ViewModel = new NewAgeReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void Basic_Info_Click_1(object sender, RoutedEventArgs e)
        {
            //WindowClose();
            //NewBasicInfoReportViewModel ViewModel = new NewBasicInfoReportViewModel();
            //Window = new NewHrReportsWindow(ViewModel);
            //Window.Show();
            WindowClose();
            LPEmployeeDetailsW = new LPEmployeeDetailsWindow();
            LPEmployeeDetailsW.Show();
        }

        private void Civil_Status_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewCivilStatusReportViewModel ViewModel = new NewCivilStatusReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void Contract_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewContractsReportViewModel ViewModel = new NewContractsReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();

        }

        private void Contacts_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewContactsReportViewModel ViewModel = new NewContactsReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void Emails_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewEmailsReportViewModel ViewModel = new NewEmailsReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void Detailed_Info_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewDetailedInfoReportViewModel ViewModel = new NewDetailedInfoReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void EPF_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewEPFReportViewModel ViewModel = new NewEPFReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void Gender_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewGenderReportViewModel ViewModel = new NewGenderReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void NIC_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewNICReportViewModel ViewModel = new NewNICReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void PayMethod_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewPaymethodReportViewModel ViewModel = new NewPaymethodReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void Religion_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewReligionReportViewModel ViewModel = new NewReligionReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void EmploymentStatus_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewEmploymentStatusReportViewModel ViewModel = new NewEmploymentStatusReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void Resignation_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewResignationReportViewModel ViewModel = new NewResignationReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void ServicePeriod_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewServicePeriodReportViewModel ViewModel = new NewServicePeriodReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }

        private void History_Click_1(object sender, RoutedEventArgs e)
        {
            WindowClose();
            NewHistoryReportViewModel ViewModel = new NewHistoryReportViewModel();
            Window = new NewHrReportsWindow(ViewModel);
            Window.Show();
        }
    }
}
