using ERP.ERPService;
using ERP.Payroll.SlipTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using ERP.Reports;
using CustomBusyBox;

namespace ERP.Payroll.SlipTransfer
{
    class BankSelector
    {
        //string path = @"c:\ERPBankSlips";
        string DetailFile = "Genext Slip Transfer.txt";
        //string HeaderFile = "HBBB001.txt";
        string fullPath = "";
        int BankCode;
        int CompanyBankBranch;
        string CompanyAccountNo;
        string CompanyAccountName = "";
        string payPeriod = "";
        string companyBranch = "";
        string periodid = "";
        string companybranchid = "";
        DateTime Date;
        string BankName;
        string rulename = "";
        List<SlipTransferView> list = new List<SlipTransferView>();
        List<BonusSlipTransferView> bonuslist = new List<BonusSlipTransferView>();
        List<ExternalBankLoanSlipView> externalbanklist = new List<ExternalBankLoanSlipView>();
        List<SlipTransferEmployeeDeductionView> deductionList = new List<SlipTransferEmployeeDeductionView>();
        List<ExcelUploadedSlipTransferView> exceluploadedList = new List<ExcelUploadedSlipTransferView>();
        DateTime FDate;
        DateTime TDate;
        List<EmployeeThirdPartyPaymentsSlipTransferSP_Result> thirdpartypaymentlist = new List<EmployeeThirdPartyPaymentsSlipTransferSP_Result>();
        ERPServiceClient serviceClient;
        string sliptype = "";
        decimal tot = 0;
        int count = 0;
        string DirectoryPath = "";
        //string path1 = "";
        //string path2 ="";
        //string path3 ="";

        public BankSelector(string BankName, List<SlipTransferView> list, DateTime Date, int CompanyBank, int CompanyBankBranch, string CompanyAccountNo, string CompanyAccountName, string payeriod, string companybranch, string companybranchID, string periodID)
        {
            this.Date = Date;
            this.list = list;
            this.BankCode = CompanyBank;
            this.CompanyBankBranch = CompanyBankBranch;
            this.CompanyAccountNo = CompanyAccountNo;
            this.BankName = BankName;
            this.CompanyAccountName = CompanyAccountName;
            this.payPeriod = payeriod;
            this.companyBranch = companybranch;
            this.companybranchid = companybranchID;
            this.periodid = periodID;
            this.sliptype = "Salary";

            serviceClient = new ERPServiceClient();

            DirectoryPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\" + this.BankName + "\\" + this.companyBranch + "\\";
            //path1 = @"" + DirectoryPath + "\\" + "_Slip_Detail.txt";
            //path2 = @"" + DirectoryPath + "\\" + "_Slip_Hedder.txt";
            //path3 = @"" + DirectoryPath + "\\" + "_No_Account_No_List.txt";
            //this.CheckPath();
        }

        public BankSelector(string BankName, List<BonusSlipTransferView> list, DateTime Date, int CompanyBank, int CompanyBankBranch, string CompanyAccountNo, string CompanyAccountName, string payeriod, string companybranch, string companybranchID, string periodID)
        {
            this.Date = Date;
            this.bonuslist = list;
            this.BankCode = CompanyBank;
            this.CompanyBankBranch = CompanyBankBranch;
            this.CompanyAccountNo = CompanyAccountNo;
            this.BankName = BankName;
            this.CompanyAccountName = CompanyAccountName;
            this.payPeriod = payeriod;
            this.companyBranch = companybranch;
            this.companybranchid = companybranchID;
            this.periodid = periodID;
            this.sliptype = "Bonus";

            serviceClient = new ERPServiceClient();

            DirectoryPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\" + this.BankName + "\\";
            //path1 = @"" + DirectoryPath + "\\" + "_Slip_Detail.txt";
            //path2 = @"" + DirectoryPath + "\\" + "_Slip_Hedder.txt";
            //path3 = @"" + DirectoryPath + "\\" + "_No_Account_No_List.txt";
            //this.CheckPath();
        }

        public BankSelector(string BankName, List<ExternalBankLoanSlipView> list, DateTime Date, int CompanyBank, int CompanyBankBranch, string CompanyAccountNo, string CompanyAccountName, string payeriod, string companybranch, string companybranchID, string periodID)
        {
            this.Date = Date;
            this.externalbanklist = list;
            this.BankCode = CompanyBank;
            this.CompanyBankBranch = CompanyBankBranch;
            this.CompanyAccountNo = CompanyAccountNo;
            this.BankName = BankName;
            this.CompanyAccountName = CompanyAccountName;
            this.payPeriod = payeriod;
            this.companyBranch = companybranch;
            this.companybranchid = companybranchID;
            this.periodid = periodID;
            this.sliptype = "EBL";

            serviceClient = new ERPServiceClient();

            DirectoryPath = @"C:\\H2SO4\\SlipTransfer\\External Bank Loan\\" + this.payPeriod + "\\";
            //path1 = @"" + DirectoryPath + "\\" + "_Slip_Detail.txt";
            //path2 = @"" + DirectoryPath + "\\" + "_Slip_Hedder.txt";
            //path3 = @"" + DirectoryPath + "\\" + "_No_Account_No_List.txt";
            //this.CheckPath();
        }

        public BankSelector(string BankName, List<SlipTransferEmployeeDeductionView> list, DateTime Date, int CompanyBank, int CompanyBankBranch, string CompanyAccountNo, string CompanyAccountName, string payeriod, string companybranch, string companybranchID, string periodID, string ruleName)
        {
            this.Date = Date;
            this.deductionList = list;
            this.BankCode = CompanyBank;
            this.CompanyBankBranch = CompanyBankBranch;
            this.CompanyAccountNo = CompanyAccountNo;
            this.BankName = BankName;
            this.CompanyAccountName = CompanyAccountName;
            this.payPeriod = payeriod;
            this.companyBranch = companybranch;
            this.companybranchid = companybranchID;
            this.periodid = periodID;
            this.rulename = ruleName;
            this.sliptype = "DR";

            serviceClient = new ERPServiceClient();

            DirectoryPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\" + this.rulename + "\\";
            //path1 = @"" + DirectoryPath + "\\" + "_Slip_Detail.txt";
            //path2 = @"" + DirectoryPath + "\\" + "_Slip_Hedder.txt";
            //path3 = @"" + DirectoryPath + "\\" + "_No_Account_No_List.txt";
            //this.CheckPath();
        }

        public BankSelector(string BankName, List<ExcelUploadedSlipTransferView> list, DateTime Date, int CompanyBank, int CompanyBankBranch, string CompanyAccountNo, string CompanyAccountName, string payeriod, string companybranch, string companybranchID, string periodID)
        {
            this.Date = Date;
            this.exceluploadedList = list;
            this.BankCode = CompanyBank;
            this.CompanyBankBranch = CompanyBankBranch;
            this.CompanyAccountNo = CompanyAccountNo;
            this.BankName = BankName;
            this.CompanyAccountName = CompanyAccountName;
            this.payPeriod = payeriod;
            this.companyBranch = companybranch;
            this.companybranchid = companybranchID;
            this.periodid = periodID;
            this.sliptype = "EUFR";

            serviceClient = new ERPServiceClient();

            DirectoryPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\";
            //path1 = @"" + DirectoryPath + "\\" + "_Slip_Detail.txt";
            //path2 = @"" + DirectoryPath + "\\" + "_Slip_Hedder.txt";
            //path3 = @"" + DirectoryPath + "\\" + "_No_Account_No_List.txt";
            //this.CheckPath();
        }

        public BankSelector(string BankName, List<EmployeeThirdPartyPaymentsSlipTransferSP_Result> list, DateTime FromDate, DateTime ToDate, int CompanyBank, int CompanyBankBranch, string CompanyAccountNo, string CompanyAccountName, string companybranch, string companybranchID)
        {
            this.FDate = FromDate;
            this.TDate = ToDate;
            this.thirdpartypaymentlist = list;
            this.BankCode = CompanyBank;
            this.CompanyBankBranch = CompanyBankBranch;
            this.CompanyAccountNo = CompanyAccountNo;
            this.BankName = BankName;
            this.CompanyAccountName = CompanyAccountName;
            this.companyBranch = companybranch;
            this.companybranchid = companybranchID;
            this.sliptype = "ETPP";

            serviceClient = new ERPServiceClient();

            DirectoryPath = @"C:\\H2SO4\\SlipTransfer\\Third Party Payments\\" + this.FDate.ToString("yyyyMM") + "\\";
            //path1 = @"" + DirectoryPath + "\\" + "_Slip_Detail.txt";
            //path2 = @"" + DirectoryPath + "\\" + "_Slip_Hedder.txt";
            //path3 = @"" + DirectoryPath + "\\" + "_No_Account_No_List.txt";
            //this.CheckPath();
        }

        public BankSelector(string BankName, DateTime Date, int CompanyBank, int CompanyBankBranch, string CompanyAccountNo, string CompanyAccountName, string payeriod, string companybranch, string companybranchID, string periodID)
        {
            this.Date = Date;
            this.BankCode = CompanyBank;
            this.CompanyBankBranch = CompanyBankBranch;
            this.CompanyAccountNo = CompanyAccountNo;
            this.BankName = BankName;
            this.CompanyAccountName = CompanyAccountName;
            this.payPeriod = payeriod;
            this.companyBranch = companybranch;
            this.companybranchid = companybranchID;
            this.periodid = periodID;

            serviceClient = new ERPServiceClient();
            DirectoryPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\" + this.BankName + "\\";
            //path1 = @"" + DirectoryPath + "\\" + "_Slip_Detail.txt";
            //path2 = @"" + DirectoryPath + "\\" + "_Slip_Hedder.txt";
            //path3 = @"" + DirectoryPath + "\\" + "_No_Account_No_List.txt";

            //this.CheckPath();
        }

        bool Selector(int BankCode)
        {

            try
            {
                bool iscomplete = false;

                switch (BankCode)
                {
                    case 7010://BankOfCeylon

                        BankOfCeylon();
                        break;

                    case 7038://StandardCharteredBank
                        StandardCharteredBank();
                        break;

                    case 7047: //CitiBank
                        CitiBank();
                        break;

                    case 7056://CommercialBankPLC
                        //iscomplete=
                        CommercialBankPLC();
                        break;

                    case 7074:// HabibBankLtd
                        HabibBankLtd();
                        break;

                    case 7083://HattonNationalBankPLC
                        HattonNationalBankPLC();
                        break;

                    case 7092:// HongkongShanghaiBank
                        HongkongShanghaiBank();
                        break;

                    case 7108://IndianBank
                        IndianBank();
                        break;

                    case 7117://IndianOverseasBank
                        IndianOverseasBank();
                        break;

                    case 7135://PeoplesBank 
                        PeoplesBank();
                        break;

                    case 7144://StateBankofIndia
                        StateBankofIndia();
                        break;

                    case 7162://NationsTrustBankPLC 
                        NationsTrustBankPLC();
                        break;

                    case 7205://DeutscheBank()
                        DeutscheBank();
                        break;

                    case 7214://NationalDevelopmentBankPLC
                        NationalDevelopmentBankPLC();
                        break;

                    case 7269://MCBBankLtd
                        MCBBankLtd();
                        break;

                    case 7278://SampathBankPLC
                        SampathBankPLC();
                        break;

                    case 7287://SeylanBankPLC
                        SeylanBankPLC();
                        break;

                    case 7296://PublicBank
                        PublicBank();
                        break;

                    case 7302://UnionBankofColomboPLC
                        UnionBankofColomboPLC();
                        break;

                    case 7311://PanAsiaBankingCorporationPLC
                        PanAsiaBankingCorporationPLC();
                        break;

                    case 7384://ICICIBankLtd
                        ICICIBankLtd();
                        break;

                    case 7454://DFCCVardhanaBankLtd
                        DFCCVardhanaBankLtd();
                        break;

                    case 7463://AmanaBank
                        AmanaBank();
                        break;

                    case 7472://AxisBank
                        AxisBank();
                        break;

                    case 7481://CargillsBankLimited
                        CargillsBankLimited();
                        break;

                    case 7719://NationalSavingsBank
                        NationalSavingsBank();
                        break;

                    case 7728://SanasaDevelopmentBank
                        SanasaDevelopmentBank();
                        break;

                    case 7737://HDFCBank
                        HDFCBank();
                        break;

                    case 7746://CitizenDevelopmentBusinessFinancePLC
                        CitizenDevelopmentBusinessFinancePLC();
                        break;

                    case 7755://RegionalDevelopmentBank
                        RegionalDevelopmentBank();
                        break;

                    case 7764://StateMortgageANDInvestmeBank
                        StateMortgageANDInvestmeBank();
                        break;

                    case 7773://LBFinancePLC
                        LBFinancePLC();
                        break;

                    case 7782://SenkadagalaFinancePLC
                        SenkadagalaFinancePLC();
                        break;

                    case 7807://CommercialLeasingandFinance
                        CommercialLeasingandFinance();
                        break;

                    case 8004://CentralBankofSriLanka
                        CentralBankofSriLanka();
                        break;
                }

                return iscomplete;
            }
            catch (Exception ex)
            {

                return false;
            }

        }

        #region Banks

        public void BankOfCeylon()
        {
            //List<string> selectedList = new List<string>();
            //CreatSubFolder();
            //foreach (var item in list)
            //{
            //    selectedList.Add(string.Format("0000{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}00000000", Convert.ToInt32(item.bank_code).ToString("0000"), item.branch_id.ToString("000"), Convert.ToInt32(item.account_no).ToString("000000000000"), Convert.ToInt32(item.second_name).ToString("00000000000000000000"), "23", "00", "0", "000000", Convert.ToInt32(item.total_salary).ToString("000000000000"), "SLR", BankCode.ToString("0000"), CompanyBankBranch.ToString("000"), CompanyAccountNo.ToString("000000000000"), Convert.ToInt32(this.CompanyAccountName).ToString("00000000000000000000"), "000000000000000", "000000000000000", Date.ToString("000000")));

            //}
            //if (selectedList != null)
            //    foreach (string item in selectedList)
            //    {
            //        File.WriteAllText(this.fullPath + @"\" + "Writer.txt", "puka" + item);
            //    }
        }

        public void StandardCharteredBank()
        {

        }
        public void CitiBank()
        {

        }
        public void CommercialBankPLC()
        {
            //try
            //{
            //    List<string> selectedList = new List<string>();
            //    CreatSubFolder();
            //    foreach (var item in list)
            //    {                
            //        string particulars = "Particulars".PadRight(15);                  
            //        string reference = "Reference".PadRight(15);
            //        int bank_code = Convert.ToInt32(item.bank_code);
            //        int bank_branch_code = Convert.ToInt32(item.bank_branch_code);
            //        Int64 emp_acc_no =Convert.ToInt64( item.account_no);
            //        string emp_name = item.second_name.ToString().PadRight(20); 
            //        int salary = Convert.ToInt32 (item.total_salary);
            //        int bankCode = BankCode;
            //        int company_bank_branch = CompanyBankBranch;
            //        Int64 company_acc_no = Convert.ToInt64( CompanyAccountNo);
            //        string company_acc_name = CompanyAccountName;




            //        selectedList.Add(string.Format("0000{0}{1}{2}{3}{4}{5}0000000{6}SLR{7}{8}{9}{10}{11}{12}00000000", bank_code.ToString("0000"), bank_branch_code.ToString("000"), emp_acc_no.ToString("000000000000"), emp_name.ToString(), "23", "00", salary.ToString("000000000000"), bankCode.ToString("0000"), company_acc_no.ToString("000000000000"), company_acc_name.ToString(), particulars.ToString(), reference.ToString(), Date.ToString("yyMMdd"))+Environment.NewLine);

            //    }
            //    if (selectedList != null)

            //        File.Delete(this.fullPath + @"\" + this.DetailFile);

            //        foreach (string item in selectedList)
            //        {
            //            File.AppendAllText(this.fullPath + @"\" + this.DetailFile,item);
            //        }

            //        MessageBox.Show("Slip Generated Successfully");
            //       return true; 

            //}
            //catch (Exception e)
            //{

            //    MessageBox.Show(e.ToString());
            //    return false;
            //}
        }
        public void HabibBankLtd()
        {

        }
        public void HattonNationalBankPLC()
        {

        }
        public void HongkongShanghaiBank()
        {

        }
        public void IndianBank()
        {

        }
        public void IndianOverseasBank()
        {

        }
        public void PeoplesBank()
        {
            //try
            //{
            //    fullPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\" + this.BankName + "\\";
            //    List<string> selectedList = new List<string>();
            //    CreatSubFolder();
            //    string account_number = null;
            //    string account_name = null;
            //    string f_name = null;
            //    if (sliptype == "Salary")
            //    {
            //        foreach (var item in list.OrderBy(c => c.emp_id))
            //        {
            //            int bank_code = Convert.ToInt32(item.bank_code);
            //            int bank_branch_code = Convert.ToInt32(item.bank_branch_code);
            //            Int64 emp_acc_no = Convert.ToInt64(item.account_no);
            //            if (emp_acc_no.ToString().Length > 12)
            //            {
            //                if (emp_acc_no.ToString().Length > 17)
            //                    account_number = emp_acc_no.ToString().Substring(6).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 16)
            //                    account_number = emp_acc_no.ToString().Substring(5).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 15)
            //                    account_number = emp_acc_no.ToString().Substring(4).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 14)
            //                    account_number = emp_acc_no.ToString().Substring(3).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 13)
            //                    account_number = emp_acc_no.ToString().Substring(2).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length == 13)
            //                    account_number = emp_acc_no.ToString().Substring(1).PadLeft(12, '0');
            //            }
            //            else
            //                account_number = emp_acc_no.ToString().PadLeft(12, '0');
            //            string acc_name = item.account_name.ToString();
            //            if (acc_name.ToString().Length > 20)
            //                account_name = acc_name.ToString().Substring(0, 20).PadRight(20);
            //            else
            //                account_name = acc_name.ToString().PadRight(20);
            //            int salary = Convert.ToInt32(decimal.Truncate(((decimal)((item.total_salary * 100) * 100m)) / 100m));
            //            tot = tot + salary;
            //            string particulars = ("SALARY" + Date.ToString("yyyyMM")).PadLeft(15, ' ');
            //            if (item.first_name.ToString().Length >= 12)
            //                f_name = item.first_name.ToString().Substring(0, 10);
            //            else
            //                f_name = item.first_name.ToString();
            //            string reference = (f_name + item.emp_id).PadLeft(15, ' ');

            //            selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", bank_code.ToString("0000"), bank_branch_code.ToString("000"), "000", account_number.ToString(), account_name.ToString(), "23", "0", salary.ToString().PadLeft(12, '0'), particulars.ToString(), reference.ToString(), Date.ToString("yyMMdd"), "000000") + Environment.NewLine);
            //            count++;
            //        }
            //    }
            //    else if (sliptype == "Bonus")
            //    {
            //        foreach (var item in bonuslist.OrderBy(c => c.emp_id))
            //        {
            //            int bank_code = Convert.ToInt32(item.bank_code);
            //            int bank_branch_code = Convert.ToInt32(item.bank_branch_code);
            //            Int64 emp_acc_no = Convert.ToInt64(item.account_no);
            //            if (emp_acc_no.ToString().Length > 12)
            //            {
            //                if (emp_acc_no.ToString().Length > 14)
            //                    account_number = emp_acc_no.ToString().Substring(3).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 13)
            //                    account_number = emp_acc_no.ToString().Substring(2).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length == 13)
            //                    account_number = emp_acc_no.ToString().Substring(1).PadLeft(12, '0');
            //            }
            //            else
            //                account_number = emp_acc_no.ToString().PadLeft(12, '0');
            //            string acc_name = item.account_name.ToString();
            //            if (acc_name.ToString().Length > 20)
            //                account_name = acc_name.ToString().Substring(0, 20).PadRight(20);
            //            else
            //                account_name = acc_name.ToString().PadRight(20);
            //            int salary = Convert.ToInt32(decimal.Truncate(((decimal)((item.PayedBonusAmount * 100) * 100m)) / 100m));
            //            tot = tot + salary;
            //            string particulars = ("BONUS" + Date.ToString("yyyyMM")).PadLeft(15, ' ');
            //            if (item.first_name.ToString().Length >= 12)
            //                f_name = item.first_name.ToString().Substring(0, 10);
            //            else
            //                f_name = item.first_name.ToString();
            //            string reference = (f_name + item.emp_id).PadLeft(15, ' ');

            //            selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", bank_code.ToString("0000"), bank_branch_code.ToString("000"), "000", account_number.ToString(), account_name.ToString(), "23", "0", salary.ToString().PadLeft(12, '0'), particulars.ToString(), reference.ToString(), Date.ToString("yyMMdd"), "000000") + Environment.NewLine);
            //            count++;
            //        }
            //    }
            //    else if (sliptype == "EBL")
            //    {
            //        fullPath = @"C:\\H2SO4\\SlipTransfer\\External Bank Loan\\" + this.payPeriod + "\\";
            //        foreach (var item in externalbanklist.OrderBy(c => c.emp_id))
            //        {
            //            int bank_code = Convert.ToInt32(item.bank_code);
            //            int bank_branch_code = Convert.ToInt32(item.bank_branch_code);
            //            Int64 emp_acc_no = Convert.ToInt64(item.bankAccountNo);
            //            if (emp_acc_no.ToString().Length >= 12)
            //            {
            //                if (emp_acc_no.ToString().Length > 14)
            //                    account_number = emp_acc_no.ToString().Substring(3).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 13)
            //                    account_number = emp_acc_no.ToString().Substring(2).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 12)
            //                    account_number = emp_acc_no.ToString().Substring(1).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length == 12)
            //                    account_number = emp_acc_no.ToString();
            //            }
            //            else
            //                if (emp_acc_no.ToString().Length == 12)
            //                    account_number = emp_acc_no.ToString();
            //                else
            //                    account_number = emp_acc_no.ToString().PadLeft(12, '0');
            //            string acc_name = item.bankAccountName.ToString();
            //            if (acc_name.ToString().Length > 20)
            //                account_name = acc_name.ToString().Substring(0, 20).PadRight(20);
            //            else
            //                account_name = acc_name.ToString().PadRight(20);
            //            int salary = Convert.ToInt32(decimal.Truncate(((decimal)((item.InstallmentAmount * 100) * 100m)) / 100m));
            //            tot = tot + salary;
            //            string particulars = ("ExtBnkLn" + Date.ToString("yyyyMM")).PadLeft(15, ' ');
            //            if (item.first_name.ToString().Length >= 12)
            //                f_name = item.first_name.ToString().Substring(0, 10);
            //            else
            //                f_name = item.first_name.ToString();
            //            string reference = (f_name + item.emp_id).PadLeft(15, ' ');

            //            selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", bank_code.ToString("0000"), bank_branch_code.ToString("000"), "000", account_number.ToString(), account_name.ToString(), "23", "0", salary.ToString().PadLeft(12, '0'), particulars.ToString(), reference.ToString(), Date.ToString("yyMMdd"), "000000") + Environment.NewLine);
            //            count++;
            //        }
            //    }
            //    else if (sliptype == "DR")
            //    {
            //        fullPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\" + this.rulename + "\\";
            //        foreach (var item in deductionList.OrderBy(c => c.deduction_payment_id))
            //        {
            //            int bank_code = Convert.ToInt32(item.bank_code);
            //            int bank_branch_code = Convert.ToInt32(item.bank_branch_code);
            //            Int64 emp_acc_no = Convert.ToInt64(item.acc_no);
            //            if (emp_acc_no.ToString().Length >= 12)
            //            {
            //                if (emp_acc_no.ToString().Length > 14)
            //                    account_number = emp_acc_no.ToString().Substring(3).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 13)
            //                    account_number = emp_acc_no.ToString().Substring(2).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 12)
            //                    account_number = emp_acc_no.ToString().Substring(1).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length == 12)
            //                    account_number = emp_acc_no.ToString();
            //            }
            //            else
            //                if (emp_acc_no.ToString().Length == 12)
            //                    account_number = emp_acc_no.ToString();
            //                else
            //                    account_number = emp_acc_no.ToString().PadLeft(12, '0');
            //            string acc_name = item.acc_name.ToString();
            //            if (acc_name.ToString().Length > 20)
            //                account_name = acc_name.ToString().Substring(0, 20).PadRight(20);
            //            else
            //                account_name = acc_name.ToString().PadRight(20);
            //            int salary = Convert.ToInt32(decimal.Truncate(((decimal)((item.total_amount * 100) * 100m)) / 100m));
            //            tot = tot + salary;
            //            string rn = rulename.Replace(" ",string.Empty);
            //            if (rn.Length > 9)
            //                rn = rulename.Substring(0, 8);
            //            string particulars = (rn+""+ Date.ToString("yyyyMM")).PadLeft(15, ' ');
            //            string reference = (rulename.Replace(" ", string.Empty)).PadLeft(15, ' ');

            //            selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", bank_code.ToString("0000"), bank_branch_code.ToString("000"), "000", account_number.ToString(), account_name.ToString(), "23", "0", salary.ToString().PadLeft(12, '0'), particulars.ToString(), reference.ToString(), Date.ToString("yyMMdd"), "000000") + Environment.NewLine);
            //            count++;
            //        }
            //    }
            //    else if (sliptype == "EUFR")
            //    {
            //        fullPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\";
            //        foreach (var item in exceluploadedList.OrderBy(c => c.emp_id))
            //        {
            //            int bank_code = Convert.ToInt32(item.bank_code);
            //            int bank_branch_code = Convert.ToInt32(item.bank_branch_code);
            //            Int64 emp_acc_no = Convert.ToInt64(item.account_no);
            //            if (emp_acc_no.ToString().Length >= 12)
            //            {
            //                if (emp_acc_no.ToString().Length > 14)
            //                    account_number = emp_acc_no.ToString().Substring(3).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 13)
            //                    account_number = emp_acc_no.ToString().Substring(2).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 12)
            //                    account_number = emp_acc_no.ToString().Substring(1).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length == 12)
            //                    account_number = emp_acc_no.ToString();
            //            }
            //            else
            //                if (emp_acc_no.ToString().Length == 12)
            //                    account_number = emp_acc_no.ToString();
            //                else
            //                    account_number = emp_acc_no.ToString().PadLeft(12, '0');
            //            string acc_name = item.account_name.ToString();
            //            if (acc_name.ToString().Length > 20)
            //                account_name = acc_name.ToString().Substring(0, 20).PadRight(20);
            //            else
            //                account_name = acc_name.ToString().PadRight(20);
            //            int salary = Convert.ToInt32(decimal.Truncate(((decimal)((item.netAmount * 100) * 100m)) / 100m));
            //            tot = tot + salary;
            //            string particulars = ("OPayments" + Date.ToString("yyyyMM")).PadLeft(15, ' ');
            //            if (item.first_name.ToString().Length >= 12)
            //                f_name = item.first_name.ToString().Substring(0, 10);
            //            else
            //                f_name = item.first_name.ToString();
            //            string reference = (f_name + item.emp_id).PadLeft(15, ' ');

            //            selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", bank_code.ToString("0000"), bank_branch_code.ToString("000"), "000", account_number.ToString(), account_name.ToString(), "23", "0", salary.ToString().PadLeft(12, '0'), particulars.ToString(), reference.ToString(), Date.ToString("yyMMdd"), "000000") + Environment.NewLine);
            //            count++;
            //        }
            //    }
            //    else if (sliptype == "ETPP")
            //    {
            //        fullPath = @"C:\\H2SO4\\SlipTransfer\\Third Party Payments\\" + this.FDate.ToString("yyyyMM") + "\\";
            //        foreach (var item in thirdpartypaymentlist.OrderBy(c => c.emp_id))
            //        {
            //            int bank_code = Convert.ToInt32(item.bank_code);
            //            int bank_branch_code = Convert.ToInt32(item.bank_branch_code);
            //            Int64 emp_acc_no = Convert.ToInt64(item.account_no);
            //            if (emp_acc_no.ToString().Length >= 12)
            //            {
            //                if (emp_acc_no.ToString().Length > 14)
            //                    account_number = emp_acc_no.ToString().Substring(3).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 13)
            //                    account_number = emp_acc_no.ToString().Substring(2).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length > 12)
            //                    account_number = emp_acc_no.ToString().Substring(1).PadLeft(12, '0');
            //                else if (emp_acc_no.ToString().Length == 12)
            //                    account_number = emp_acc_no.ToString();
            //            }
            //            else
            //                if (emp_acc_no.ToString().Length == 12)
            //                    account_number = emp_acc_no.ToString();
            //                else
            //                    account_number = emp_acc_no.ToString().PadLeft(12, '0');
            //            string acc_name = item.account_name.ToString();
            //            if (acc_name.ToString().Length > 20)
            //                account_name = acc_name.ToString().Substring(0, 20).PadRight(20);
            //            else
            //                account_name = acc_name.ToString().PadRight(20);
            //            int salary = Convert.ToInt32(decimal.Truncate(((decimal)((item.amount * 100) * 100m)) / 100m));
            //            tot = tot + salary;
            //            string particulars = ("TPayments" + FDate.ToString("yyyyMM")).PadLeft(15, ' ');
            //            if (item.first_name.ToString().Length >= 12)
            //                f_name = item.first_name.ToString().Substring(0, 10);
            //            else
            //                f_name = item.first_name.ToString();
            //            string reference = (f_name + item.emp_id).PadLeft(15, ' ');

            //            selectedList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}", bank_code.ToString("0000"), bank_branch_code.ToString("000"), "000", account_number.ToString(), account_name.ToString(), "23", "0", salary.ToString().PadLeft(12, '0'), particulars.ToString(), reference.ToString(), Date.ToString("yyMMdd"), "000000") + Environment.NewLine);
            //            count++;
            //        }
            //    }
            //    tot = tot / 100;
            //    if (selectedList != null)

            //        File.Delete(this.fullPath + @"\" + this.DetailFile);

            //    foreach (string item in selectedList)
            //    {
            //        File.AppendAllText(this.fullPath + @"\" + this.DetailFile, item);
            //    }

            //    BusyBox.CloseBusy();
            //    MessageBox.Show("Slip Generated Successfully Total "+tot.ToString("n2")+" Count "+count);
            //    return true;

            //}
            //catch (Exception e)
            //{
            //    BusyBox.CloseBusy();
            //    MessageBox.Show(e.ToString());
            //    return false;
            //}
        }
        public void StateBankofIndia()
        {

        }
        public void NationsTrustBankPLC()
        {

        }
        public void DeutscheBank()
        {

        }
        public void NationalDevelopmentBankPLC()
        {

        }
        public void MCBBankLtd()
        {

        }
        public bool SampathBankPLC()
        {
            try
            {
                fullPath = @"C:\\H2SO4\\SlipTransfer\\" + this.payPeriod + "\\" + this.BankName + "\\" + this.companyBranch + "\\";
                string account_name = "";
                string emp_name = "";
                string account_number = null;
                List<string> selectedList = new List<string>();
                CreatSubFolder();
                foreach (var item in list)
                {
                    string trns_mode = "";

                    if (item.bank_code == "7278")
                    {
                        trns_mode = "SBA".PadLeft(9);
                    }
                    else
                    {
                        trns_mode = "SLI".PadLeft(9);
                    }
                    string sal = "Salary";
                    //string particulars = "Particulars".PadRight(15);
                    //string reference = "Reference".PadRight(15);
                    int bank_code = Convert.ToInt32(item.bank_code);
                    int bank_branch_code = Convert.ToInt32(item.bank_branch_code);
                    Int64 emp_acc_no = Convert.ToInt64(item.account_no);
                    if (emp_acc_no.ToString().Length > 12)
                    {
                        if (emp_acc_no.ToString().Length > 17)
                            account_number = emp_acc_no.ToString().Substring(6).PadLeft(12, '0');
                        else if (emp_acc_no.ToString().Length > 16)
                            account_number = emp_acc_no.ToString().Substring(5).PadLeft(12, '0');
                        else if (emp_acc_no.ToString().Length > 15)
                            account_number = emp_acc_no.ToString().Substring(4).PadLeft(12, '0');
                        else if (emp_acc_no.ToString().Length > 14)
                            account_number = emp_acc_no.ToString().Substring(3).PadLeft(12, '0');
                        else if (emp_acc_no.ToString().Length > 13)
                            account_number = emp_acc_no.ToString().Substring(2).PadLeft(12, '0');
                        else if (emp_acc_no.ToString().Length == 13)
                            account_number = emp_acc_no.ToString().Substring(1).PadLeft(12, '0');
                    }
                    else
                        account_number = emp_acc_no.ToString().PadLeft(12, '0');

                    account_name = item.account_name.ToString();
                    if (account_name.Length > 25)
                    {
                        emp_name = item.account_name.ToString().Substring(0, 25).PadRight(25);
                    }
                    else
                        emp_name = item.account_name.ToString().PadRight(25);

                    //int salary = Convert.ToInt32(decimal.Truncate(((decimal)((item.total_salary * 100) * 100m)) / 100m));//Convert.ToInt32(item.total_salary);
                    decimal salarytot = Math.Round(Convert.ToDecimal(item.total_salary), 2);

                    int bankCode = BankCode;
                    int company_bank_branch = CompanyBankBranch;
                    Int64 company_acc_no = Convert.ToInt64(CompanyAccountNo);
                    string company_acc_name = CompanyAccountName;
                    string date = Date.ToString("MMM");
                    string year = Date.ToString("yyyy");
                    string officemobile = item.office_mobile == null ? string.Empty.PadLeft(43) : item.office_mobile.PadLeft(43);
                    string officemail = item.office_email == null ? string.Empty : item.office_email;
                    //string date = DateTime.Now.ToString("MMMM");
                    //string cont_num  = item.

                    //2020-09-25
                    sal = (sal + " " + date + " " + year).PadLeft(20);

                    selectedList.Add(string.Format("{0} {1}{2} {3} {4} {5}{6}{7}", sal.ToString(), emp_name.ToString(), account_number, bank_code.ToString("0000"), bank_branch_code.ToString("000"), salarytot.ToString("0000000.00"), sal.ToString(), trns_mode.ToString()) + Environment.NewLine);//bank_code.ToString("0000"), bank_branch_code.ToString("000"), emp_acc_no.ToString("000000000000"), emp_name.ToString(), "23", "00", salary.ToString("000000000000"), bankCode.ToString("0000"), company_acc_no.ToString("000000000000"), company_acc_name.ToString(), particulars.ToString(), reference.ToString(), Date.ToString("yyMMdd")) + Environment.NewLine);
                }
                if (selectedList != null)
                {
                    if (Directory.Exists(this.fullPath + @"\" + this.DetailFile) == true)
                        File.Delete(this.fullPath + @"\" + this.DetailFile);
                }

                foreach (string item in selectedList)
                {
                    File.AppendAllText(this.fullPath + @"\" + this.DetailFile, item);
                }

                MessageBox.Show("Slip Generated Successfully");
                return true;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }
        public void SeylanBankPLC()
        {

            //bool isGenarateDetail = false;
            //bool isGenarateHedder = false;

            //List<string> SlipList = new List<string>();
            //int count = 0;
            //int noaccountEmployee = 0;
            //CreatSubFolder();

            //decimal total_amount_forHedder = 0m;
            //int employee_count_forHedder = 0;

            //IEnumerable<EmployeeSumarryView> allEmployee = serviceClient.GetAllEmployeeDetail();

            //allEmployee = allEmployee.Where(z => z.payment_methord_id == Guid.Parse("00000000-0000-0000-0000-000000000003"));

            //foreach (var item_employee in allEmployee.ToList())
            //{
            //    SlipTransferView currentView = list.FirstOrDefault(z => z.employee_id == item_employee.employee_id);
            //    if (currentView != null)
            //    {
            //        total_amount_forHedder = total_amount_forHedder + currentView.total_salary.Value;
            //        employee_count_forHedder++;
            //        try
            //        {
            //            string particulars = "Particulars".PadRight(15);
            //            var employee_name = item_employee.initials + " " + item_employee.first_name + " " + item_employee.second_name;
            //            var emp_name = string.Join("", (employee_name.TrimStart(' ').Take(20).ToArray()));
            //            string reference = "Reference".PadRight(15);
            //            int employee_no = int.Parse(item_employee.emp_id);
            //            int bank_code = currentView.bank_code == null ? 0 : int.Parse(currentView.bank_code.ToString());
            //            int bank_branch_code = currentView.bank_branch_code == null ? 0 : int.Parse(currentView.bank_branch_code.ToString());
            //            decimal account_no = currentView.account_no == null ? 0m : decimal.Parse(currentView.account_no.ToString());
            //            //string emp_name= (item_selected.initials==null ?"0000000000000000000000000000":item_selected.initials.ToString() +" "+item_selected.first_name==null ?"":item_selected.first_name.ToString()+" " +item_selected.second_name==null?"":item_selected.second_name.ToString()).ToString();
            //            decimal amount = Math.Round(currentView.total_salary == null ? 0m : decimal.Parse(currentView.total_salary.ToString()), 2);
            //            decimal decimal_point = amount - (int)amount;
            //            int sence = 0;
            //            if (decimal_point == 0m)
            //            {
            //                // decimal_point = decimal_point % 1;
            //            }
            //            else
            //            {
            //                sence = int.Parse(string.Join("", (decimal_point.ToString().Trim('0').Trim('.').Take(2).ToArray())));

            //            }

            //            int company_bankcode = currentView.bank_code == null ? 0 : int.Parse(currentView.bank_code.ToString());
            //            double company_account_no = Math.Round(currentView.account_no == null ? 0 : double.Parse(currentView.account_no.ToString()), 0);
            //            //string company_account_name = item_selected.account_name == null ? "0000000000" : currentView.account_name.ToString();
            //            //DateTime date = S;

            //            //SlipList.Add(string.Format("0000{0}{1}{2}{3}{4}{5}0000000{6}SLR{7}{8}{9}{10}{11}{12}00000000", bank_code.ToString("0000"), bank_branch_code.ToString("000"), account_no.ToString("000000000000"), emp_name.ToString(), "23", "00", amount.ToString("000000000000"), company_bankcode.ToString("0000"), company_account_no.ToString("000000000000"), company_account_name.ToString(), particulars.ToString(), reference.ToString(), System.DateTime.Now.Date.ToString("yyMMdd")) + Environment.NewLine);
            //            SlipList.Add(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", employee_no.ToString("00000000"), emp_name.PadRight(20, ' '), bank_code.ToString("0000"), bank_branch_code.ToString("000"), account_no.ToString("00000000000000"), amount.ToString("0000000000000"), sence.ToString("00"), Date.Date.ToString("yyMMdd")));

            //        }
            //        catch (Exception exx)
            //        {

            //            clsMessages.setMessage("Slip Transfer File Genarate Fail " + exx.InnerException.ToString());
            //        }
            //    }
            //    else
            //    {

            //        if (noaccountEmployee == 0)
            //        {
            //            using (StreamWriter sw = new StreamWriter(path3))
            //            {
            //                sw.WriteLine(item_employee.emp_id + "\t" + item_employee.initials + " " + item_employee.first_name + " " + item_employee.second_name);
            //                sw.Close();
            //                noaccountEmployee++;
            //            }
            //        }
            //        else
            //        {
            //            using (StreamWriter sw = File.AppendText(path3))
            //            {
            //                sw.WriteLine(item_employee.emp_id + "\t" + item_employee.initials + " " + item_employee.first_name + " " + item_employee.second_name);
            //                sw.Close();
            //                noaccountEmployee++;
            //            }
            //        }
            //    }


            //    count++;
            //}
            //int icount = 0;
            //try
            //{
            //    foreach (var item_string in SlipList)
            //    {
            //        if (icount == 0)
            //        {
            //            using (StreamWriter sw = new StreamWriter(path1))
            //            {
            //                sw.WriteLine(item_string);
            //                sw.Close();
            //                icount++;

            //            }

            //        }
            //        else
            //        {
            //            using (StreamWriter sw = File.AppendText(path1))
            //            {
            //                sw.WriteLine(item_string);
            //                sw.Close();
            //                icount++;

            //            }
            //        }
            //        isGenarateDetail = true;

            //    }
            //}
            //catch (Exception)
            //{


            //}


            //try
            //{
            //    string account_name = CompanyAccountName;
            //    decimal account_no = Math.Round(CompanyAccountNo == null ? 1 : decimal.Parse(CompanyAccountNo), 0);
            //    int bank_code = int.Parse(BankCode.ToString());
            //    int bank_branch_code = int.Parse(CompanyBankBranch.ToString());
            //    decimal total_amount = Math.Round(total_amount_forHedder);
            //    decimal slip_amount = (int)total_amount;
            //    decimal amount = total_amount - (int)total_amount;
            //    int sence = 0;
            //    if (amount == 0m)
            //    {
            //        // decimal_point = decimal_point % 1;
            //    }
            //    else
            //    {
            //        sence = int.Parse(string.Join("", (amount.ToString().Trim('0').Trim('.').Take(2).ToArray())));

            //    }
            //    int employee_count = employee_count_forHedder;
            //    string total_amount_slip = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", account_name.PadLeft(4, '0'), Date.Date.ToString("MMyyyy"), bank_code.ToString("0000"), bank_branch_code.ToString("000"), account_no.ToString("0000000000000"), slip_amount.ToString("0000000000000000000"), sence.ToString("00"), employee_count.ToString("00000"));
            //    using (StreamWriter sw = new StreamWriter(path2))
            //    {
            //        sw.WriteLine(total_amount_slip);
            //        sw.Close();
            //    }
            //    isGenarateHedder = true;
            //}
            //catch (Exception)
            //{


            //}

            //if (isGenarateDetail == true && isGenarateHedder == true)
            //{
            //    clsMessages.setMessage("Slip Transfer File Genarate Successfully");
            //}
            //else
            //{
            //    clsMessages.setMessage("Slip Transfer File Genarate Fail");
            //}
            //icount = 0;
            //return true;
        }




        public void PublicBank()
        {

        }
        public void UnionBankofColomboPLC()
        {

        }
        public void PanAsiaBankingCorporationPLC()
        {
            //try
            //{
            //    string path = "\\Reports\\Documents\\PayrollReport\\PanAsiaPaymentDetails";
            //    ReportPrint print = new ReportPrint(path);
            //    print.setParameterValue("@Branch_id", companybranchid);
            //    print.setParameterValue("@PayPeriod_id", periodid);
            //    print.setParameterValue("Paydate", Date);
            //    print.LoadToReportViewer();
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }
        public void ICICIBankLtd()
        {

        }
        public void DFCCVardhanaBankLtd()
        {

        }
        public void AmanaBank()
        {

        }
        public void AxisBank()
        {

        }
        public void CargillsBankLimited()
        {

        }
        public void NationalSavingsBank()
        {

        }
        public void SanasaDevelopmentBank()
        {

        }
        public void HDFCBank()
        {

        }
        public void CitizenDevelopmentBusinessFinancePLC()
        {

        }
        public void RegionalDevelopmentBank()
        {

        }
        public void StateMortgageANDInvestmeBank()
        {

        }
        public void LBFinancePLC()
        {

        }
        public void SenkadagalaFinancePLC()
        {

        }
        public void CommercialLeasingandFinance()
        {

        }
        public void CentralBankofSriLanka()
        {

        }
        #endregion

        #region Create Save Path

        public bool CheckPath()
        {
            try
            {


                CreatSubFolder();

                return Selector(BankCode);




            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
                return false;
            }
        }

        void CreatSubFolder()
        {

            if (Directory.Exists(DirectoryPath) == false)
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            //if (!File.Exists(path1))
            //{
            //var file1 = File.Create(path1);
            //file1.Close();
            //}

            //if (Directory.Exists(DirectoryPath) == false)
            //{
            //Directory.CreateDirectory(DirectoryPath);
            //}
            //if (!File.Exists(path2))
            //{
            //var file2 = File.Create(path2);
            //file2.Close();

            //}

            //if (Directory.Exists(DirectoryPath) == false)
            //{
            //Directory.CreateDirectory(DirectoryPath);
            //}
            //if (!File.Exists(path3))
            //{
            //var file3 = File.Create(path3);
            //file3.Close();
            //}
        }

        #endregion
    }
}