using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using Microsoft.Office.Interop.Excel;
using System.Windows;
using System.IO;
using System.Runtime.InteropServices;
using CustomBusyBox;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    class EPFETFGenerateExcelViewModel : ViewModelBase
    {
        #region Fields
        ERPServiceClient serviceClient;
        string DirectoryPath = "";
        #endregion

        #region Constructor
        public EPFETFGenerateExcelViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshCompanyBranch();
            RefreshPeriods();
            EPF = true;
            Date = System.DateTime.Now;
        }
        #endregion

        #region Properties
        private IEnumerable<z_Period> _Periods;

        public IEnumerable<z_Period> Periods
        {
            get { return _Periods; }
            set { _Periods = value; OnPropertyChanged("Periods"); }
        }

        private z_Period _CurrentPeriod;

        public z_Period CurrentPeriod
        {
            get { return _CurrentPeriod; }
            set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); }
        }

        private IEnumerable<z_CompanyBranches> _companyBranches;
        public IEnumerable<z_CompanyBranches> companyBranches
        {
            get { return _companyBranches; }
            set { _companyBranches = value; OnPropertyChanged("companyBranches"); }
        }

        private z_CompanyBranches _CurrentBranch;
        public z_CompanyBranches CurrentBranch
        {
            get { return _CurrentBranch; }
            set { _CurrentBranch = value; OnPropertyChanged("CurrentBranch"); if (CurrentPeriod != null && CurrentBranch != null) RefreshEmployeeFunds(); }
        }

        private IEnumerable<Rpt_sp_EmployeeFundView> _EmployeeFunds;

        public IEnumerable<Rpt_sp_EmployeeFundView> EmployeeFunds
        {
            get { return _EmployeeFunds; }
            set { _EmployeeFunds = value; OnPropertyChanged("EmployeeFunds"); }
        }

        private bool _EPF;

        public bool EPF
        {
            get { return _EPF; }
            set { _EPF = value; OnPropertyChanged("EPF"); }
        }

        private bool _ETF;

        public bool ETF
        {
            get { return _ETF; }
            set { _ETF = value; OnPropertyChanged("ETF"); }
        }

        private bool _Excel;

        public bool Excel
        {
            get { return _Excel; }
            set { _Excel = value; OnPropertyChanged("Excel"); }
        }

        private bool _Text;
        public bool Text
        {
            get { return _Text; }
            set { _Text = value; OnPropertyChanged("Text"); }
        }

        private DateTime _Date;

        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; OnPropertyChanged("Date"); }
        }


        #endregion

        #region Refresh Methods
        private void RefreshPeriods()
        {
            serviceClient.GetPeriodsCompleted += (s, e) =>
            {
                Periods = e.Result.OrderBy(c => c.start_date);
            };
            serviceClient.GetPeriodsAsync();
        }

        private void RefreshEmployeeFunds()
        {
            //serviceClient.GetEmployeeFundDetailsCompleted += (s, e) =>
            //{
            EmployeeFunds = serviceClient.GetEmployeeFundDetails(CurrentPeriod.period_id).Where(d => d.companyBranch_id == CurrentBranch.companyBranch_id).OrderBy(c => c.emp_id);
            //};
            //serviceClient.GetEmployeeFundDetailsAsync(CurrentPeriod.period_id);
        }

        private void RefreshCompanyBranch()
        {
            companyBranches = serviceClient.GetCompanyBranches().OrderBy(c => c.companyBranch_id);
        }
        #endregion

        #region Commands And Methods
        public ICommand GenerateExcel
        {
            get { return new RelayCommand(Generate); }
        }

        private void Generate()
        {
            try
            {
                BusyBox.ShowBusy("Please Wait Untill Generate Completed...");
                if (CurrentPeriod != null && EmployeeFunds.Count() > 0)
                {
                    if (Excel == true)
                    {

                        if (EPF == true)
                        {
                            DirectoryPath = @"C:\\H2SO4\\ExcelFiles\\" + CurrentPeriod.period_name + "\\" + CurrentBranch.companyBranch_Name + "\\EPF\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            ws.Range["A1"].Value = "NIC/Passport number";
                            ws.Range["B1"].Value = "Last Name";
                            ws.Range["C1"].Value = "Initials";
                            ws.Range["D1"].Value = "Member AC number";
                            ws.Range["E1"].Value = "Total Contribution amount (Rs.)";
                            ws.Range["F1"].Value = "Employer's Contribution Amount (Rs)";
                            ws.Range["G1"].Value = "Member’s Contribution Amount (Rs.)";
                            ws.Range["H1"].Value = "Total Earnings (Rs.)";
                            ws.Range["I1"].Value = "Member Status";
                            ws.Range["J1"].Value = "Zone code";
                            ws.Range["K1"].Value = "Employer Number";
                            ws.Range["L1"].Value = "Contribution Year Month";
                            ws.Range["M1"].Value = "Data Submission Number";
                            ws.Range["N1"].Value = "No.of days worked";
                            ws.Range["O1"].Value = "Occupation Classification Grade(As per the classification of censes  & statistic Dept) ";


                            int j = 2;
                            foreach (var item in EmployeeFunds.Where(c => c.company_variableID == new Guid("00000000-0000-0000-0000-000000000001")))
                            {
                                ws.Range["A:A"].NumberFormat = "@";
                                ws.Range["A" + j].Value = item.nic;
                                if (item.epf_name != null)
                                {
                                    //string[] name = item.epf_name.Split(new string[] { ". " }, StringSplitOptions.RemoveEmptyEntries);
                                    ws.Range["B" + j].Value = item.surname.ToUpper();//name[1].ToLower();
                                }
                                if (item.epf_name != null)
                                {
                                    //string[] name = item.epf_name.Split(new string[] { ". " }, StringSplitOptions.RemoveEmptyEntries);
                                    ws.Range["C" + j].Value = item.initials.ToUpper();//name[0].ToLower();
                                }
                                ws.Range["D:D"].NumberFormat = "@";
                                ws.Range["D" + j].Value = item.epf_no;//.TrimStart('0');
                                ws.Range["G" + j].Value = item.value;
                                ws.Range["H" + j].Value = item.calculate_salary;
                                ws.Range["I" + j].Value = "E";
                                ws.Range["J" + j].Value = "A";
                                //if (item.companyBranch_id == new Guid("00000000-0000-0000-0000-000000000000"))
                                //{
                                //    ws.Range["J" + j].Value = "A";
                                //}
                                //else if (item.companyBranch_id == new Guid("00000000-0000-0000-0000-000000000002"))
                                //{
                                //    ws.Range["J" + j].Value = "B";
                                //}
                                //else if (item.companyBranch_id == new Guid("00000000-0000-0000-0000-000000000003"))
                                //{
                                //    ws.Range["J" + j].Value = "A";
                                //}
                                //else
                                //{
                                //    ws.Range["J" + j].Value = "Y";
                                //}
                                ws.Range["K" + j].Value = item.epf_registstion_no.Trim(new Char[] { '-', 'A', 'B' });
                                ws.Range["L" + j].Value = Date.ToString("yyyyMM");
                                ws.Range["M" + j].Value = "1";
                                ws.Range["N" + j].Value = "22";
                                ws.Range["O" + j].Value = "51";
                                j++;
                            }
                            j = 2;
                            foreach (var item in EmployeeFunds.Where(c => c.company_variableID == new Guid("00000000-0000-0000-0000-000000000002")))
                            {
                                ws.Range["F" + j].Value = item.value;
                                j++;
                            }
                            for (j = 2; j <= (EmployeeFunds.Where(c => c.company_variableID == new Guid("00000000-0000-0000-0000-000000000001")).Count()) + 1; j++)
                            {
                                ws.Range["E" + j].FormulaLocal = "=F" + j + "+" + "G" + j;
                            }
                            wb.SaveAs("C:\\H2SO4\\ExcelFiles\\" + CurrentPeriod.period_name + "\\" + CurrentBranch.companyBranch_Name + "\\EPF\\EPF.xlsx");
                            Marshal.ReleaseComObject(app);
                            New();
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("EPF Excel Generated Successfully");
                        }
                        else
                        {
                            DirectoryPath = @"C:\\H2SO4\\ExcelFiles\\" + CurrentPeriod.period_name + "\\" + CurrentBranch.companyBranch_Name + "\\ETF\\";
                            CreatSubFolder();
                            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                            Worksheet ws = wb.Worksheets[1];
                            ws.Range["A1"].Value = "Member Number";
                            ws.Range["B1"].Value = "Initials";
                            ws.Range["C1"].Value = "Surname";
                            ws.Range["D1"].Value = "NIC Number";
                            ws.Range["E1"].Value = "Contribution";
                            //ws.Range["F1"].Value = "Contribution period To";
                            //ws.Range["G1"].Value = "Total 6 Months Contributions in Cents";

                            int j = 2;
                            foreach (var item in EmployeeFunds.Where(c => c.company_variableID == new Guid("00000000-0000-0000-0000-000000000003")))
                            {
                                ws.Range["A:A"].NumberFormat = "@";
                                ws.Range["A" + j].Value = item.epf_no;//.TrimStart('0');
                                if (item.epf_name != null)
                                {
                                    //string[] name = item.initials.Split(new string[] { ". " }, StringSplitOptions.RemoveEmptyEntries);
                                    ws.Range["B" + j].Value = item.initials;//name[0].ToLower();
                                }
                                if (item.epf_name != null)
                                {
                                    //string[] name = item.surname.Split(new string[] { ". " }, StringSplitOptions.RemoveEmptyEntries);
                                    ws.Range["C" + j].Value = item.surname;//name[1].ToLower();
                                }
                                ws.Range["D:D"].NumberFormat = "@";
                                ws.Range["D" + j].Value = item.nic;
                                //ws.Range["E" + j].Value = Date.ToString("yyyyMM");
                                //ws.Range["F" + j].Value = Date.ToString("yyyyMM");
                                int value = Convert.ToInt32(decimal.Truncate((decimal)((item.value))));// * 100m)) / 100m));
                                ws.Range["E" + j].Value = value;
                                j++;
                            }
                            wb.SaveAs("C:\\H2SO4\\ExcelFiles\\" + CurrentPeriod.period_name + "\\" + CurrentBranch.companyBranch_Name + "\\ETF\\ETF.xlsx");
                            Marshal.ReleaseComObject(app);
                            New();
                            BusyBox.CloseBusy();
                            clsMessages.setMessage("ETF Excel Generated Successfully");
                        }
                    }

                    else
                    {
                        if (EPF == true)
                        {
                            try
                            {
                                DirectoryPath = @"C:\\H2SO4\\TextFiles\\" + CurrentPeriod.period_name + "\\" + CurrentBranch.companyBranch_Name + "\\EPF\\";
                                CreatSubFolder();
                                var registration_no = EmployeeFunds.Where(d => d.epf_registstion_no != null).Select(c => c.epf_registstion_no).Distinct().ToList();

                                foreach (var no in registration_no)
                                {
                                    IEnumerable<Rpt_sp_EmployeeFundView> FilteredEmployeeFundsTemp = EmployeeFunds.Where(c => (c.company_variableID == new Guid("00000000-0000-0000-0000-000000000001") || c.company_variableID == new Guid("00000000-0000-0000-0000-000000000002")) && c.epf_registstion_no == no);
                                    string employer = no.ToString();
                                    //string companyBranch = FilteredEmployeeFundsTemp.Where(d => d.companyBranch_Name != null).Select(c => c.companyBranch_Name).FirstOrDefault();
                                    //string bank_code = FilteredEmployeeFundsTemp.Select(C => C.bank).FirstOrDefault();
                                    //string branch_code = FilteredEmployeeFundsTemp.Select(C => C.bank_branch_code).FirstOrDefault();
                                    //string account_no = FilteredEmployeeFundsTemp.Select(C => C.account_no).FirstOrDefault();
                                    //string payment_reference = bank_code + account_no;
                                    var Month = CurrentPeriod.end_date.Value.Month;
                                    string a = Month.ToString("00");

                                    string endtDate = (CurrentPeriod.end_date.Value.Year) + a;

                                    //string zone_code = FilteredEmployeeFundsTemp.Select(c => c.zone_code).FirstOrDefault();
                                    //string district_office_code = FilteredEmployeeFundsTemp.Select(c => c.district_office_code).FirstOrDefault();

                                    employer = employer.Replace(" ", string.Empty);

                                    if (employer.Contains('/'))
                                    {
                                        employer = employer.Split('/')[0];
                                    }

                                    int TotalMembers = 0;
                                    List<string> selectedList = new List<string>();
                                    string selectedStringHeaderRecord = "";

                                    decimal totalContributionFor_EVEMP = 0;

                                    foreach (var item in FilteredEmployeeFundsTemp.Where(c => c.company_variableID == new Guid("00000000-0000-0000-0000-000000000001")))
                                    {
                                        string NIC = item.nic;
                                        string epfNo = item.epf_no;
                                        string secondName = item.surname;
                                        string initialsFirst = item.initials == null ? String.Empty : item.initials;
                                        string initials = initialsFirst.Replace(".", string.Empty);
                                        if (epfNo == null || secondName == null || epfNo == "" || NIC == null)
                                        {
                                            continue;
                                        }
                                        //string initials = item.initials.Substring(0, item.initials.IndexOf(' '));
                                        //initials = initials == null ? " " : initials.Replace(".", " ");
                                        //string lastName = epfName.Substring(epfName.IndexOf(' ') + 1);
                                        //string member_status = item.member_status;

                                        decimal employeesContribution = Math.Round(Convert.ToDecimal(FilteredEmployeeFundsTemp.Where(c => c.company_variableID == new Guid("00000000-0000-0000-0000-000000000001") && c.epf_no == item.epf_no).Select(d => d.value).FirstOrDefault()), 2);
                                        decimal employersContribution = Math.Round(Convert.ToDecimal(FilteredEmployeeFundsTemp.Where(c => c.company_variableID == new Guid("00000000-0000-0000-0000-000000000002") && c.epf_no == item.epf_no).Select(d => d.value).FirstOrDefault()), 2);
                                        decimal totalContribution = employeesContribution + employersContribution;
                                        totalContributionFor_EVEMP += Math.Round(totalContribution, 2);

                                        decimal totalEarning = Math.Round(Convert.ToDecimal(item.calculate_salary), 2);
                                        string zonecode = "A";

                                        //if (item.companyBranch_id == new Guid("00000000-0000-0000-0000-000000000000"))
                                        //{
                                        //    zonecode = "A";
                                        //}
                                        //else if (item.companyBranch_id == new Guid("00000000-0000-0000-0000-000000000002"))
                                        //{
                                        //    zonecode = "B";
                                        //}
                                        //else if (item.companyBranch_id == new Guid("00000000-0000-0000-0000-000000000003"))
                                        //{
                                        //    zonecode = "A";
                                        //}
                                        //else
                                        //{
                                        //    zonecode = "Y";
                                        //}

                                        selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}",
                                            NIC.PadRight(20),
                                            secondName.ToUpper().PadRight(40),
                                            initials.ToUpper().PadRight(20),
                                            epfNo.PadLeft(6),
                                            totalContribution.ToString().PadLeft(10),
                                            employersContribution.ToString().PadLeft(10),
                                            employeesContribution.ToString().PadLeft(10),
                                            totalEarning.ToString().PadLeft(12),
                                            "E",
                                            zonecode.PadRight(2),
                                            //member_status,
                                            //zone_code,
                                            employer.Trim(new Char[] { 'A', '-', 'B', 'Y' }),    //too large with /GA
                                            endtDate,
                                            "1".PadLeft(2),  //data submission no
                                            "30.00",  //working days
                                            "1".PadLeft(3)) + Environment.NewLine);  //Occupation Classification Grade 014-Hospitality, shop and related services managers

                                        TotalMembers += 1;

                                    }

                                    if (selectedList != null && selectedList.Count > 0)
                                    {
                                        selectedStringHeaderRecord = string.Format("{0}{1}{2}{3}{4}{5}",
                                            //zone_code,
                                            employer.PadLeft(6),    //too large with /GA
                                            endtDate,
                                            "1", //data submission no
                                            totalContributionFor_EVEMP.ToString().PadLeft(12),
                                            TotalMembers.ToString().PadLeft(5),
                                            "1");  //mode of payment  1-Cheque
                                                   //payment_reference.PadLeft(20, '0'),//too large
                                                   //date.PadLeft(10, '0'),
                                                   //district_office_code);

                                        File.Delete(this.DirectoryPath + @"\\" + CurrentBranch.companyBranch_Name + ".txt");
                                        //File.Delete(this.DirectoryPath + @"\" + CurrentBranch.companyBranch_Name + "_Header.txt");
                                        foreach (string item in selectedList)
                                        {
                                            File.AppendAllText(this.DirectoryPath + @"\\" + CurrentBranch.companyBranch_Name + ".txt", item);
                                        }

                                        //if (selectedStringHeaderRecord != null)
                                        //{
                                        //    File.AppendAllText(this.DirectoryPath + @"\" + CurrentBranch.companyBranch_Name + "_Header.txt", selectedStringHeaderRecord);
                                        //}
                                    }

                                }
                                BusyBox.CloseBusy();
                                MessageBox.Show("EPF File Generated Successfully");
                            }
                            catch (Exception e)
                            {
                                BusyBox.CloseBusy();
                                MessageBox.Show("File Generate Error! \n" + e.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                DirectoryPath = @"C:\\H2SO4\\TextFiles\\" + CurrentPeriod.period_name + "\\" + CurrentBranch.companyBranch_Name + "\\ETF\\";
                                CreatSubFolder();

                                var registration_no = EmployeeFunds.Where(d => d.epf_registstion_no != null).Select(c => c.epf_registstion_no).Distinct().ToList();
                                //var companyBranch = EmployeeFunds.Where(d => d.companyBranchForETF_EPF != null && d.epf_registstion_no != null).Select(c => c.companyBranchForETF_EPF).Distinct().ToList();

                                foreach (var no in registration_no)
                                {
                                    IEnumerable<Rpt_sp_EmployeeFundView> FilteredEmployeeFundsTemp = EmployeeFunds.Where(c => c.company_variableID == new Guid("00000000-0000-0000-0000-000000000003") && c.epf_registstion_no == no);
                                    string employer = no.ToString();
                                    employer = employer.Replace(" ", string.Empty);
                                    decimal totalContribution = 0;
                                    string companyBranch = FilteredEmployeeFundsTemp.Where(d => d.companyBranch_Name != null).Select(c => c.companyBranch_Name).FirstOrDefault();

                                    string startDate = (CurrentPeriod.start_date.Value.Year) + "0" + (CurrentPeriod.start_date.Value.Month);

                                    var Month = CurrentPeriod.end_date.Value.Month;
                                    string a = Month.ToString("00");
                                    
                                    string endDtae = (CurrentPeriod.end_date.Value.Year) + a;
                                    //string zone_code = FilteredEmployeeFundsTemp.Select(c => c.zone_code).FirstOrDefault(); ;

                                    if (employer.Contains('/'))
                                    {
                                        employer = employer.Split('/')[0];
                                    }

                                    int TotalMembers = 0;
                                    List<string> selectedList = new List<string>();

                                    foreach (var item in FilteredEmployeeFundsTemp.OrderBy(c => c.emp_id))
                                    {
                                        string etfNo = item.etf_no;
                                        string epfName = item.surname;
                                        if (etfNo == null || epfName == null || etfNo == "")
                                        {
                                            continue;
                                        }
                                        string initials = item.initials = item.initials == null ? String.Empty : item.initials.Replace(".",""); // = epfName.Substring(0, epfName.IndexOf(' '));                                        
                                        //initials = initials == null ? " " : initials.Replace(".", " ");
                                        string lastName = epfName;//.Substring(epfName.IndexOf(' ') + 1);
                                        string NIC = item.nic;
                                        //decimal totalEarning = Math.Round(Convert.ToDecimal(item.calculate_salary), 2);
                                        decimal contribution = Math.Round(Convert.ToDecimal(item.value), 2);
                                        decimal amount = Convert.ToDecimal(contribution.ToString().Split('.')[0] + contribution.ToString().Split('.')[1]);
                                        //employer = employer.Replace("-", "0");

                                        selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                                            "D",
                                            //zone_code.PadRight(2),
                                            employer.PadLeft(8).TrimStart(new Char[] {'A'}),
                                            etfNo.PadLeft(6, '0'),
                                            initials.ToUpper().PadRight(20),
                                            lastName.ToUpper().PadRight(30),
                                             NIC.PadLeft(12),
                                            endDtae.PadRight(3),
                                            endDtae.PadRight(3),
                                            amount.ToString("000000000") + Environment.NewLine));

                                        totalContribution += Convert.ToDecimal(item.value);
                                        TotalMembers += 1;
                                    }
                                    totalContribution = Math.Round(totalContribution, 2);
                                    string selectedStringHeaderRecord = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                        "H",
                                        //zone_code.PadRight(2),
                                        //employer,//.Trim(new Char[] {'-'}),
                                        employer.PadLeft(8, '0'),
                                        endDtae,
                                        endDtae,
                                        TotalMembers.ToString("000000"),
                                        (totalContribution.ToString().Split('.')[0] + totalContribution.ToString().Split('.')[1]).PadLeft(14, '0'),
                                        "22");

                                    if (selectedList != null && selectedList.Count > 0)
                                    {
                                        File.Delete(this.DirectoryPath + @"\\" + companyBranch + ".txt");
                                        //File.Delete(this.DirectoryPath + @"\\" + companyBranch + "_Header.txt");
                                        foreach (string item in selectedList)
                                        {
                                            File.AppendAllText(this.DirectoryPath + @"\\" + companyBranch + ".txt", item);
                                        }

                                        if (selectedStringHeaderRecord != null)
                                        {
                                            File.AppendAllText(this.DirectoryPath + @"\\" + companyBranch + ".txt", selectedStringHeaderRecord);
                                        }
                                    }
                                }
                                BusyBox.CloseBusy();
                                MessageBox.Show("ETF File Generated Successfully");

                            }
                            catch (Exception e)
                            {
                                BusyBox.CloseBusy();
                                MessageBox.Show("File Generate Error! \n" + e.Message);
                            }
                        }
                    }
                }
                else
                {
                    BusyBox.CloseBusy();
                    clsMessages.setMessage("No Data Found For The Selected Period");
                }
            }
            catch (Exception ex)
            {
                BusyBox.CloseBusy();
                MessageBox.Show(ex.Message);
            }
        }

        void CreatSubFolder()
        {
            if (Directory.Exists(DirectoryPath) == false)
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }

        private void New()
        {
            Periods = null;
            RefreshPeriods();
            CurrentPeriod = new z_Period();
            EPF = true;
            Date = System.DateTime.Now;
        }
        #endregion
    }
}
