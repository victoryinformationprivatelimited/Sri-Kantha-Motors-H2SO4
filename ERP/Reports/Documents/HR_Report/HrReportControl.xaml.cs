using ERP.Reports.Documents.HR_Report.NewHrReports;
using ERP.Reports.Documents.HR_Report.NewHrReports.ViewModels;
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

namespace ERP.Reports.Documents.HR_Report
{
    
    public partial class HrReportControl : UserControl
    {
        #region Memeber Windows

        NewHrReportsWindow ReportWindow;

        #endregion

        #region Constructor

        public HrReportControl()
        {
            InitializeComponent();
        } 

        #endregion

        #region Window Closing Methods

        void closeWindows()
        {
            if (ReportWindow != null)
                ReportWindow.Close();
        }

        #endregion

        private void DrivingLicenceAndPassport_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            DrivingLicenceAndPassportViewModel ViewModel = new DrivingLicenceAndPassportViewModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();
        }

        private void RaceAndNAtionality_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            RaceAndNationalityViweModel ViewModel = new RaceAndNationalityViweModel();
            ReportWindow  = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }

        private void Awords_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            AwordsViweModel ViewModel = new AwordsViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }

        private void EmployeeFullDetails_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            EmployeeFullDetailsViweModel ViewModel = new EmployeeFullDetailsViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();
        }

        private void EPF_And_ETF_Names_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            EpfEtfNamesViweModel ViewModel = new EpfEtfNamesViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();
        }

        private void DisplayName_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            DisplayNameViweModel ViewModel = new DisplayNameViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();
        }

        private void BloodType_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            BloodTypeViweModel ViewModel = new BloodTypeViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();
        }

        private void Employee_ID_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            EmployeeIDViweModel ViewModel = new EmployeeIDViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();
        }

        private void Activity_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            ExtracurryculerActivityViweModel ViewModel = new ExtracurryculerActivityViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }

        private void FamilyDetails_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            FamilyDetailsViweModel ViewModel = new FamilyDetailsViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }

        private void AcadamicQualification_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            AcadamicQualificationViweModel ViewModel = new AcadamicQualificationViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }

        private void InterestedFeild_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            InterestedDetailsViweModel ViewModel = new InterestedDetailsViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }

        private void PreviousCompany_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            PreviousCompanyViweModel ViewModel = new PreviousCompanyViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();


        }

        private void ProffetionalQulification_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            ProffetionalQulificationViweModel ViewModel = new ProffetionalQulificationViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }

        private void Skills_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            SkillsViweModel ViewModel = new SkillsViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }

        private void SocialMedia_Click_1(object sender, RoutedEventArgs e)
        {
            closeWindows();
            SocialMediaLinksViweModel ViewModel = new SocialMediaLinksViweModel();
            ReportWindow = new NewHrReportsWindow(ViewModel);
            ReportWindow.Show();

        }
    }
}
