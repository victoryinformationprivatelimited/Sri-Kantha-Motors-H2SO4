using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.ERPService;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace ERP.Reports.Documents.PayrollReport.PayrollReportUserControl
{
    class SalaryCoinAnalizeViewModel : ViewModelBase
    {

        #region Service Client

        ERPServiceClient serviceClient;

        #endregion

        #region Constructor

        public SalaryCoinAnalizeViewModel()
        {
            serviceClient = new ERPServiceClient();

            RefreshPayMethod();
            RefresPeriod();
            RefreshCompanyBranch();
            RefreshDepartment();
            RefreshSection();
            RefreshDesignation();
            B2 = false;

        }

        #endregion

        #region Propeties

            #region IsEnable Property

            private Boolean _B2;
            public Boolean B2
            {
                get { return _B2; }
                set { _B2 = value; OnPropertyChanged("B2"); }
            }
       
            #endregion

            #region Payment Method

                private IEnumerable<PaymentMethodView_Cash_> _PayMethod;
                public IEnumerable<PaymentMethodView_Cash_> PayMethod
                {
                    get { return _PayMethod; }
                    set { _PayMethod = value; OnPropertyChanged("PayMethod"); }
                }

                private PaymentMethodView_Cash_ _CurrentPayMethod;
                public PaymentMethodView_Cash_ CurrentPayMethod
                {
                    get { return _CurrentPayMethod; }
                    set { _CurrentPayMethod = value; OnPropertyChanged("CurrentpayMethod"); }
                }
                

            #endregion

            #region Pay Period
            private IEnumerable<z_Period> _Period;
            public IEnumerable<z_Period> Period
            {
                get { return _Period; }
                set { _Period = value; OnPropertyChanged("Period"); }
            }

            private z_Period _CurrentPeriod;
            public z_Period CurrentPeriod
            {
                get { return _CurrentPeriod; }
                set { _CurrentPeriod = value; OnPropertyChanged("CurrentPeriod"); B2 = false; }
            }
            #endregion

            #region Department

                private IEnumerable<z_Department> _Department;
                public IEnumerable<z_Department> Department
                {
                    get { return _Department; }
                    set { _Department = value; OnPropertyChanged("Department"); }
                }

                private z_Department _CurrentDepartment;
                public z_Department CurrentDepartment
                {
                    get { return _CurrentDepartment; }
                    set { _CurrentDepartment = value; OnPropertyChanged("CurrentDepartment"); }
                } 

            #endregion

            #region Section

                private IEnumerable<z_Section> _Section;
                public IEnumerable<z_Section> Section
                {
                    get { return _Section; }
                    set { _Section = value; OnPropertyChanged("Section"); }
                }

                private z_Section _CurrentSection;

                public z_Section CurrentSection
                {
                    get { return _CurrentSection; }
                    set { _CurrentSection = value; OnPropertyChanged("CurrentSection"); }
                }
                        
           #endregion

            #region Branch

                private IEnumerable<z_CompanyBranches> _Branch;
                public IEnumerable<z_CompanyBranches> Branch
                {
                    get { return _Branch; }
                    set { _Branch = value; OnPropertyChanged("Branch"); }
                }

                private z_CompanyBranches _CurrentBranch;

                public z_CompanyBranches CurrentBranch
                {
                    get { return _CurrentBranch; }
                    set { _CurrentBranch = value; OnPropertyChanged("CurrentBranch"); }
                }
                     
           #endregion

            #region Designation

                private IEnumerable<z_Designation> _Designation;

                public IEnumerable<z_Designation> Designation
                {
                    get { return _Designation; }
                    set { _Designation = value; OnPropertyChanged("Designation"); }
                    
                }

                private z_Designation _CurrentDesignation;

                public z_Designation CurrentDesignation
                {
                    get { return _CurrentDesignation; }
                    set { _CurrentDesignation = value; OnPropertyChanged("Currentdesignation"); }
                }
                
                
           #endregion


         
        #endregion

        #region Refresh Methods

                void RefreshPayMethod() 
                {
                    try
                    {
                        serviceClient.GetPaymentMethodView_Cash_Completed += (s, e) =>
                        {
                            this.PayMethod = e.Result;
                        };

                        this.serviceClient.GetPaymentMethodView_Cash_Async();
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }
                }     

            void RefresPeriod()
            {
                try
                {
                    serviceClient.GetPeriodsCompleted += (s, e) =>
                    {
                        this.Period = e.Result.Where(z => z.isdelete == false);
                    };

                    this.serviceClient.GetPeriodsAsync();
                }
                catch (Exception)
                {

                }
            }

            void RefreshDepartment() 
            {
                try
                {
                    serviceClient.GetDepartmentsCompleted += (s, e) =>
                    {
                        this.Department = e.Result;
                    };

                    this.serviceClient.GetDepartmentsAsync();

                }
                catch (Exception)
                {
                    
                    
                }
            }

            void RefreshCompanyBranch() 
            {
                try
                {
                    serviceClient.GetCompanyBranchesCompleted += (s, e) => 
                    {
                        this.Branch = e.Result;
                    };

                    this.serviceClient.GetCompanyBranchesAsync();
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }

            void RefreshSection() 
            {
                try
                {
                    serviceClient.GetSectionsCompleted += (s,e) =>
                    {
                        this.Section = e.Result;
                    };
                    this.serviceClient.GetSectionsAsync();

                }
                catch (Exception)
                {
                    
                    throw;
                }
            }

            void RefreshDesignation() 
            {
                try
                {
                    serviceClient.GetDesignationsCompleted += (s, e) => 
                    {
                        this.Designation = e.Result;
                    };

                    this.serviceClient.GetDesignationsAsync();
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }

        #endregion

        #region Analyze Button


            public ICommand AnalyzeButton
            {
                get { return new RelayCommand(Analyze); }
            }



            void Analyze()
            {
                try
                {

                    int r = serviceClient.GetCoinAnalyzeData(CurrentPeriod.period_id.ToString(), CurrentPayMethod.paymet_method_id.ToString());
                   if (r == 1) 
                   {
                       MessageBox.Show("Analyst Complete");
                     
                       RefreshCompanyBranch();
                       RefreshDepartment();
                       RefreshSection();
                       RefreshDesignation();
                       B2 = true;  

                   }

                   if (r == 0) 
                   {
                       MessageBox.Show("Analyst Error");
                   }

                }
                catch (Exception)
                {

                    throw;
                }
            }

        #endregion

        #region Summary Button

            public ICommand SummaryButton
            {
                get { return new RelayCommand(Summary); }
            }

            void Summary() 
            { 
                
                try
                {

                    if (PayMethod != null && Period != null)
                    {

                        ReportPrint print = new ReportPrint("\\Reports\\Documents\\PayrollReport\\CoinAnalyzeSummaryReport");
                        print.setParameterValue("@Branch",CurrentBranch ==null|| CurrentBranch.companyBranch_id == null ?"" :CurrentBranch.companyBranch_id.ToString());                     
                        print.setParameterValue("@Department", CurrentDepartment ==null|| CurrentDepartment.department_id == null ? "" : CurrentDepartment.department_id.ToString());
                        print.setParameterValue("@Section",CurrentSection == null|| CurrentSection.section_id == null ?"" : CurrentSection.section_id.ToString());
                        print.setParameterValue("@Designation",CurrentDesignation == null || CurrentDesignation.designation_id == null ?"" : CurrentDesignation.designation_id.ToString());
                        print.PrintReportWithReportViewer();
                    }

                }
                catch (Exception)
                {

                }
        
            }
        
        #endregion

        #region Detail Button
                   
              public ICommand  DetailButton
            {
                get { return new RelayCommand(Detail); }
            }

              void Detail() 
              {
                  try
                  {
                        if (PayMethod != null && Period != null)
                    {

                        ReportPrint print = new ReportPrint("\\Reports\\Documents\\PayrollReport\\CoinAnalyzeDetailReport");
                        print.setParameterValue("@Branch",CurrentBranch ==null|| CurrentBranch.companyBranch_id == null ?"" :CurrentBranch.companyBranch_id.ToString());                     
                        print.setParameterValue("@Department", CurrentDepartment ==null|| CurrentDepartment.department_id == null ? "" : CurrentDepartment.department_id.ToString());
                        print.setParameterValue("@Section",CurrentSection == null|| CurrentSection.section_id == null ?"" : CurrentSection.section_id.ToString());
                        print.setParameterValue("@Designation",CurrentDesignation == null || CurrentDesignation.designation_id == null ?"" : CurrentDesignation.designation_id.ToString());
                        print.PrintReportWithReportViewer();
                    }
                  }
                  catch (Exception)
                  {
                      
                 
                  }
              }
        
        #endregion
    }
}
