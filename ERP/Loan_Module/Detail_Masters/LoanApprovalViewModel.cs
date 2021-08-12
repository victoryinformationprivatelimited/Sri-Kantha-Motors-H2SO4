using CrystalDecisions.CrystalReports.Engine;
using ERP.BasicSearch;
using ERP.ERPService;
using ERP.HelperClass;
using ERP.Loan_Module.Reports;
using ERP.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ERP.Loan_Module.Detail_Masters
{
    class LoanApprovalViewModel : ViewModelBase
    {


    }

    //#region Service Object
    //private ERPServiceClient serviceClient;
    //#endregion

    //#region Constructor
    //public LoanApprovalViewModel()
    //{
    //    serviceClient = new ERPServiceClient();
    //    New();
    //}
    //#endregion

    //#region List
    //List<LoanApprovedView> LoanList = new List<LoanApprovedView>();
    //#endregion

    //#region Properties
    //private IEnumerable<dtl_ApprovedLoan> approvedLoan;
    //public IEnumerable<dtl_ApprovedLoan> ApprovedLoan
    //{
    //    get { return approvedLoan; }
    //    set { approvedLoan = value; OnPropertyChanged("ApprovedLoan"); }
    //}

    //private dtl_ApprovedLoan currentApprovedLoan;
    //public dtl_ApprovedLoan CurrentApprovedLoan
    //{
    //    get { return currentApprovedLoan; }
    //    set { currentApprovedLoan = value; OnPropertyChanged("CurrentApprovedLoan"); }
    //}

    //private IEnumerable<LoanApprovedView> loanApprovedView;
    //public IEnumerable<LoanApprovedView> LoanApprovedView
    //{
    //    get { return loanApprovedView; }
    //    set
    //    {
    //        loanApprovedView = value; OnPropertyChanged("LoanApprovedView");
    //        //if (LoanApprovedView != null)
    //        //LoanApprovedView.GroupBy(g => g.approved);
    //    }
    //}

    //private LoanApprovedView currentLoanApprovedView;
    //public LoanApprovedView CurrentLoanApprovedView
    //{
    //    get { return currentLoanApprovedView; }
    //    set { currentLoanApprovedView = value; OnPropertyChanged("CurrentLoanApprovedView"); }
    //}

    //private int searchIndex;
    //public int SearchIndex
    //{
    //    get { return searchIndex; }
    //    set { searchIndex = value; OnPropertyChanged("SearchIndex"); }
    //}

    //#endregion

    //#region Refresh Method
    //void refreshLoanApprovedView()
    //{
    //    try
    //    {
    //        serviceClient.GetLoanApprovedViewCompleted += (s, e) =>
    //           {
    //               LoanApprovedView = e.Result;
    //               if (LoanApprovedView != null)
    //                   LoanList = LoanApprovedView.ToList();
    //           };
    //        serviceClient.GetLoanApprovedViewAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show("refreshLoanApprovedView()" + ex.ToString());
    //    }
    //}

    //void refreshApprovedLoan()
    //{
    //    try
    //    {
    //        serviceClient.GetApprovedLoanCompleted += (s, e) =>
    //        { ApprovedLoan = e.Result; };
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show("refreshApprovedLoan()" + ex.ToString());
    //    }
    //}


    //#endregion

    //#region New
    //void New()
    //{
    //    refreshLoanApprovedView();
    //    CurrentLoanApprovedView = null;
    //    CurrentLoanApprovedView = new LoanApprovedView();
    //    CurrentLoanApprovedView.approved_loan_id = Guid.NewGuid();
    //}
    //#endregion

    //#region Save
    //void Save(bool approved)
    //{
    //    if (clsSecurity.GetSavePermission(604))
    //    {

    //        bool Update = true;
    //        dtl_ApprovedLoan newAppApprovedLoan = new dtl_ApprovedLoan();
    //        dtl_EmployeeLoans newEmpLoans = new dtl_EmployeeLoans();
    //        if (CurrentLoanApprovedView.approved_loan_id == null)
    //        {
    //            CurrentLoanApprovedView.approved_loan_id = Guid.NewGuid();
    //            newAppApprovedLoan.approved_loan_id = (Guid)CurrentLoanApprovedView.approved_loan_id;
    //        }
    //        else
    //        {
    //            newAppApprovedLoan.approved_loan_id = (Guid)CurrentLoanApprovedView.approved_loan_id;
    //        }
    //        newAppApprovedLoan.comment = CurrentLoanApprovedView.comment;
    //        newAppApprovedLoan.employee_loan_id = CurrentLoanApprovedView.employee_loan_id;
    //        newAppApprovedLoan.approved = approved;

    //        newEmpLoans.employee_loan_id = (Guid)CurrentLoanApprovedView.employee_loan_id;
    //        newEmpLoans.modified_datetime = System.DateTime.Now;
    //        newEmpLoans.modified_user_id = clsSecurity.loggedUser.user_id;

    //        if (newAppApprovedLoan != null && newEmpLoans != null)
    //            Update = serviceClient.GetSavedApprovedLoan(newAppApprovedLoan.approved_loan_id);

    //        if (newAppApprovedLoan != null && newAppApprovedLoan.approved_loan_id != null && newEmpLoans != null && newEmpLoans.employee_loan_id != null)
    //        {
    //            if (Update)
    //            {
    //                newAppApprovedLoan.modified_datetime = System.DateTime.Now;
    //                newAppApprovedLoan.modified_user_id = clsSecurity.loggedUser.user_id;

    //                if (serviceClient.UpdateApprovedLoan(newAppApprovedLoan) && serviceClient.UpdatePendingEmloyeeLoans(newEmpLoans))
    //                {
    //                    MessageBox.Show("Record Update Successfully");
    //                    clsMessages.setMessage(Properties.Resources.UpdateSucess);
    //                    refreshLoanApprovedView();
    //                    refreshApprovedLoan();
    //                }
    //                else
    //                {
    //                    MessageBox.Show("Record Update Failed");
    //                    clsMessages.setMessage(Properties.Resources.UpdateFail);
    //                }
    //            }
    //            else
    //            {
    //                newAppApprovedLoan.save_datetime = System.DateTime.Now;
    //                newAppApprovedLoan.save_user_id = clsSecurity.loggedUser.user_id;

    //                if (serviceClient.SaveApprovedLoan(newAppApprovedLoan) && serviceClient.UpdatePendingEmloyeeLoans(newEmpLoans))
    //                {

    //                    MessageBox.Show("Record Save Successfully");
    //                    clsMessages.setMessage(Properties.Resources.SaveSucess);
    //                    refreshLoanApprovedView();
    //                    New();
    //                }
    //                else
    //                {
    //                    MessageBox.Show("Record SaveFailed");
    //                    clsMessages.setMessage(Properties.Resources.SaveFail);
    //                }
    //            }
    //        }
    //    }
    //    else
    //        clsMessages.setMessage("You don't have Permission to Approve Loans or Reject Loans in this Form...");

    //}
    //#endregion

    //#region Approved
    //void Approve()
    //{ Save(true); }
    //#endregion

    //#region Rejected
    //void Reject()
    //{ Save(false); }
    //#endregion

    //#region Report
    //void ViewReport()
    //{
    //    if (CurrentLoanApprovedView != null && CurrentLoanApprovedView.employee_loan_id != Guid.Empty)
    //    {
    //        string path = "\\Reports\\Documents\\Loan_Report\\LoanApprovalReport";
    //        ReportPrint print = new ReportPrint(path);
    //        print.setParameterValue("@EmpLoanID", CurrentLoanApprovedView.employee_loan_id.ToString());
    //        print.LoadToReportViewer();
    //    }
    //}
    //#endregion

    //#region Button Properties & Button Function
    //#region Buttons
    //public ICommand DetailView
    //{
    //    get { return new RelayCommand(ViewReport, DetailReportCanExecute); }
    //}

    //public ICommand ApproveButton
    //{
    //    get { return new RelayCommand(Approve, ApproveCanExeCute); }
    //}

    //public ICommand RejectButton
    //{
    //    get { return new RelayCommand(Reject, RejectCanExecute); }
    //}

    //public ICommand Refresh
    //{
    //    get { return new RelayCommand(New); }
    //}
    //#endregion

    //#region Button Can Execute Methods
    //public bool ApproveCanExeCute()
    //{
    //    if (CurrentLoanApprovedView == null || CurrentLoanApprovedView.comment == null || CurrentLoanApprovedView.comment.ToString() == String.Empty)
    //        return false;
    //    else
    //        return true;
    //}

    //public bool RejectCanExecute()
    //{
    //    if (CurrentLoanApprovedView == null || CurrentLoanApprovedView.comment == null || CurrentLoanApprovedView.comment.ToString() == String.Empty)
    //        return false;
    //    else
    //        return true;
    //}

    //public bool DetailReportCanExecute()
    //{
    //    if (CurrentLoanApprovedView == null || CurrentLoanApprovedView.employee_loan_id == Guid.Empty)
    //        return false;
    //    else
    //        return true;
    //}
    //#endregion

    //#endregion

    //#region Search
    //void Search()
    //{
    //    EmployeeSearchWindow Window = new EmployeeSearchWindow();
    //    Window.ShowDialog();
    //    if (Window.viewModel.CurrentEmployeeSearchView != null && Window.viewModel.CurrentEmployeeSearchView.employee_id != Guid.Empty)
    //        SearchFilter((Guid)Window.viewModel.CurrentEmployeeSearchView.employee_id);
    //    Window.Close();
    //}

    //void SearchFilter(Guid ID)
    //{

    //    LoanApprovedView = null;
    //    LoanApprovedView = LoanList;
    //    LoanApprovedView = LoanApprovedView.Where(r => r.employee_id == ID);
    //}

    //public ICommand SearchButton
    //{
    //    get { return new RelayCommand(Search); }
    //}
    //#endregion
}
