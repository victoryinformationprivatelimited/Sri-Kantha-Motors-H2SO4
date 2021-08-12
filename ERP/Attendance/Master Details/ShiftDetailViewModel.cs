using ERP.ERPService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

namespace ERP.MastersDetails
{
    class ShiftDetailViewModel : ViewModelBase
    {

        private ERPServiceClient serviceClient = new ERPServiceClient();


        //#region Constructor
        public ShiftDetailViewModel()
        {
            //this.reafreshShiftDetailViewList();
            //this.reafreshShiftCatergories();
            //this.reafreshBasicDays();
            //this.New();
            reafreshShiftDetailViewList();
            reafreshShiftCatergories();
            reafreshBasicDays();
            New();
        }
        private IEnumerable<z_ShiftCategory> _ShiftCatagorys;
        public IEnumerable<z_ShiftCategory> ShiftCatagorys
        {
            get { return _ShiftCatagorys; }
            set { _ShiftCatagorys = value; this.OnPropertyChanged("ShiftCatagorys"); }
        }

        private z_ShiftCategory _CurrentShiftCatagory;
        public z_ShiftCategory CurrentShiftCatagory
        {
            get { return _CurrentShiftCatagory; }
            set
            {
                _CurrentShiftCatagory = value; this.OnPropertyChanged("CurrentShiftCatagory");
                if (CurrentShiftCatagory != null)
                {
                    EmployeeShiftDetails = EmployeeShiftDetails.Where(z => z.shift_category_id == CurrentShiftCatagory.shift_id);
                }
            }
        }

        private IEnumerable<z_BasicDay> _BasicDays;
        public IEnumerable<z_BasicDay> BasicDayas
        {
            get { return _BasicDays; }
            set { _BasicDays = value; this.OnPropertyChanged("BasicDays"); }
        }

        private z_BasicDay _CurrentDay;
        public z_BasicDay CurrentDay
        {
            get { return _CurrentDay; }
            set { _CurrentDay = value; this.OnPropertyChanged("CurrentDay"); }
        }

        private IEnumerable<EmployeeShiftDetailView> _EmployeeShiftDetails;
        public IEnumerable<EmployeeShiftDetailView> EmployeeShiftDetails
        {
            get { return _EmployeeShiftDetails; }
            set { _EmployeeShiftDetails = value; this.OnPropertyChanged("EmployeeShiftDetails"); }
        }

        private EmployeeShiftDetailView _CurrentShiftDetail;
        public EmployeeShiftDetailView CurrentShiftDetail
        {
            get { return _CurrentShiftDetail; }
            set { _CurrentShiftDetail = value; this.OnPropertyChanged("CurrentShiftDetail"); }
        }

        private void reafreshShiftDetailViewList()
        {
            this.serviceClient.GetShiftDetailsViewListCompleted += (s, e) =>
                {
                    this.EmployeeShiftDetails = e.Result;
                };
            this.serviceClient.GetShiftDetailsViewListAsync();
        }

        private void reafreshShiftCatergories()
        {
            this.serviceClient.GetShiftcategoryCompleted += (s, e) =>
                {
                    this.ShiftCatagorys = e.Result.Where(z => z.is_active == true && z.isdelete == false);
                };
            this.serviceClient.GetShiftcategoryAsync();
        }
        private void reafreshBasicDays()
        {
            BasicDayas = this.serviceClient.GetBasicDays();
            //this.serviceClient.GetBasicDaysCompleted += (s, e) =>
            //    {
            //        this.BasicDayas = e.Result;
            //    };
            //this.serviceClient.GetBasicDaysAsync();
        }

        public ICommand NewButton
        {
            get
            {
                return new RelayCommand(New, newCanExecute);
            }
        }

        private bool newCanExecute()
        {
            return true;
        }

        void New()
        {

            CurrentShiftDetail = null;
            CurrentShiftCatagory = null;
            CurrentDay = null;
            CurrentShiftDetail = new EmployeeShiftDetailView();
            CurrentShiftDetail.shift_detail_id = Guid.NewGuid();
            //CurrentEmployeeShiftDetailView = new ERPService.EmployeeShiftDetailView();
            //CurrentEmployeeShiftDetailView.shift_detail_id = Guid.NewGuid();
            //CurrentEmployeeShiftDetailView.shift_category_id = new Guid();
            //CurrentEmployeeShiftDetailView.day_id = new Guid();
            //CurrentShift = new z_ShiftCategory();
            //CurrentShift.shift_id = new Guid();
            //CurrentBasicDay = new z_BasicDay();
            //CurrentBasicDay.day_id = new Guid();
            //reafreshShiftDetailViewList();
            reafreshShiftDetailViewList();

        }

        public ICommand SaveButton
        {
            get
            {
                return new RelayCommand(Save, saveCanExecute);
            }
        }

        private bool saveCanExecute()
        {
            if (CurrentShiftDetail == null)
                return false;
            if (CurrentShiftDetail.shift_detail_id == null || CurrentShiftDetail.shift_detail_id == Guid.Empty)
                return false;
            if (CurrentShiftDetail.shift_category_id == null || CurrentShiftDetail.shift_category_id == Guid.Empty)
                return false;
            if (CurrentShiftDetail.in_day_id == null || CurrentShiftDetail.in_day_id == Guid.Empty)
                return false;
            if (CurrentShiftDetail.out_day_id == null || CurrentShiftDetail.out_day_id == Guid.Empty)
                return false;
            if (CurrentShiftDetail.shift_out_day_id == null || CurrentShiftDetail.shift_out_day_id == Guid.Empty)
                return false;
            if (CurrentShiftDetail.shift_in_day_id == null || CurrentShiftDetail.shift_in_day_id == Guid.Empty)
                return false;
            if (CurrentShiftDetail.shift_in_time == null)
                return false;
            if (CurrentShiftDetail.shift_out_time == null)
                return false;
            if (CurrentShiftDetail.in_time == null)
                return false;
            if (CurrentShiftDetail.out_time == null)
                return false;
            return true;
        }

        void Save()
        {
            try
            {
                bool IsUpdate = false;

                dtl_Shift newDetailShift = new dtl_Shift();
                newDetailShift.shift_detail_id = CurrentShiftDetail.shift_detail_id;
                newDetailShift.shift_category_id = CurrentShiftDetail.shift_category_id;
                newDetailShift.in_day_id = CurrentShiftDetail.in_day_id;
                newDetailShift.out_day_id = CurrentShiftDetail.out_day_id;
                newDetailShift.in_time = CurrentShiftDetail.in_time;
                newDetailShift.out_time = CurrentShiftDetail.out_time;
                newDetailShift.grace_in = CurrentShiftDetail.grace_in;
                newDetailShift.grace_out = CurrentShiftDetail.grace_out;
                newDetailShift.shift_in_time = CurrentShiftDetail.shift_in_time;
                newDetailShift.shift_out_time = CurrentShiftDetail.shift_out_time;
                newDetailShift.shift_in_day_id = CurrentShiftDetail.shift_in_day_id;
                newDetailShift.shift_out_day_id = CurrentShiftDetail.shift_out_day_id;
                newDetailShift.is_off = CurrentShiftDetail.is_off;
                newDetailShift.can_continue = CurrentShiftDetail.can_continue;

                IEnumerable<dtl_Shift> allShiftDetails = this.serviceClient.GetShiftDetails();

                if (allShiftDetails != null)
                {
                    foreach (var Shift in allShiftDetails)
                    {
                        if (Shift.shift_detail_id == CurrentShiftDetail.shift_detail_id && Shift.shift_category_id == CurrentShiftDetail.shift_category_id)
                        {
                            IsUpdate = true;
                        }
                    }
                }
                if (IsUpdate)
                {
                    if (clsSecurity.GetUpdatePermission(302))
                    {
                        if (this.serviceClient.UpdateShiftDetail(newDetailShift))
                        {
                            System.Windows.MessageBox.Show("Record Update Successfully");
                            clsMessages.setMessage(Properties.Resources.UpdateSucess);
                            this.New();
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Record Update Failed");
                            clsMessages.setMessage(Properties.Resources.UpdateFail);
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Update this record(s)");
                }
                else
                {
                    if (clsSecurity.GetSavePermission(302))
                    {
                        if (this.serviceClient.SaveShiftDetails(newDetailShift))
                        {
                            System.Windows.MessageBox.Show("Record Save Successfully");
                            clsMessages.setMessage(Properties.Resources.SaveSucess);
                            this.New();
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Record SaveFailed");
                            clsMessages.setMessage(Properties.Resources.SaveFail);
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Save this record(s)");
                }
                reafreshShiftDetailViewList();
                this.New();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        bool deleteCanExecte()
        {
            if (CurrentShiftDetail == null)
            {
                return false;
            }
            return true;
        }
        public ICommand DeleteButton
        {
            get
            {
                return new RelayCommand(Delete, deleteCanExecte);
            }
        }
        void Delete()
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Do You Want To Delete This Record...?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    break;
                case MessageBoxResult.Yes:
                    if (clsSecurity.GetDeletePermission(302))
                    {
                        if (this.serviceClient.DeleteShiftDetail(CurrentShiftDetail))
                        {
                            System.Windows.MessageBox.Show("Record Deleted");
                            clsMessages.setMessage(Properties.Resources.DeleteSucess);
                            reafreshShiftDetailViewList();
                            this.New();
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Record Delete Failed");
                            clsMessages.setMessage(Properties.Resources.DeleteFail);
                        }
                    }
                    else
                        clsMessages.setMessage("You don't have permission to Delete this record(s)");
                    break;
                default:
                    break;
            }
        }


    }
}
