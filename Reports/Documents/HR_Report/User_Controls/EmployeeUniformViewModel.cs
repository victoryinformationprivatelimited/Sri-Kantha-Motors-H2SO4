using ERP.ERPService;
using ERP.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    class EmployeeUniformViewModel : ViewModelBase
    {
        #region Serviceobject
        ERPServiceClient serviceClient;
        #endregion

        #region Constructor
        public EmployeeUniformViewModel()
        {
            try
            {
                serviceClient = new ERPServiceClient();
                refreshUniform();
                New();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region  Properties
        private IEnumerable<z_Uniform> uniform;
        public IEnumerable<z_Uniform> Uniform
        {
            get { return uniform; }
            set { uniform = value; OnPropertyChanged("Uniform"); }
        }

        private z_Uniform currentUniform;
        public z_Uniform CurrentUniform
        {
            get { return currentUniform; }
            set { currentUniform = value; OnPropertyChanged("CurrentUniform"); if (CurrentUniform != null) TbUnlock(); if (CurrentUniform != null) refreshUniformDetail(); if (CurrentUniform != null) { if (StartList.Count(c => c.Uniform_ID == CurrentUniform.Uniform_ID) > 0) INew(); }; }
        }

        private IEnumerable<dtl_Uniform> items;
        public IEnumerable<dtl_Uniform> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged("Items"); }
        }

        private dtl_Uniform currentItem;
        public dtl_Uniform CurrentItem
        {
            get { return currentItem; }
            set { currentItem = value; OnPropertyChanged("CurrentItem"); if (CurrentItem != null) TbUnlock2(); }
        }

        private bool tb1;
        public bool Tb1
        {
            get { return tb1; }
            set { tb1 = value; OnPropertyChanged("Tb1"); }
        }

        private bool tb2;
        public bool Tb2
        {
            get { return tb2; }
            set { tb2 = value; OnPropertyChanged("Tb2"); }
        }


        #endregion

        #region Lists

        List<z_Uniform> StartList;
        List<z_Uniform> Ulist;
        List<dtl_Uniform> ItemList;

        #endregion

        #region Refresh Methods
        private void refreshUniform()
        {
            try
            {
                StartList = new List<z_Uniform>();

                serviceClient.GetUniformsCompleted += (s, e) =>
                {
                    StartList = e.Result.ToList();
                    Uniform = e.Result;
                };

                serviceClient.GetUniformsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void refreshUniformDetail()
        {
            try
            {
                ItemList = new List<dtl_Uniform>();
                ItemList.Clear();
                serviceClient.GetDtlUniformCompleted += (s, e) =>
                {
                    Items = null;
                    Items = e.Result;
                    if (Items != null)
                        ItemList = Items.ToList();
                    Items = null;
                    FilterManager();
                };
                serviceClient.GetDtlUniformAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        #endregion

        #region Methods

        #region Button Commands
        public ICommand Newbtn
        {
            get { return new RelayCommand(New); }
        }

        public ICommand Savebtn
        {
            get { return new RelayCommand(Save, SaveCanExecute); }
        }

        public ICommand Deletebtn
        {
            get { return new RelayCommand(Delete, DeleteCanExecute); }
        }

        public ICommand INewbtn
        {
            get { return new RelayCommand(INew); }
        }

        public ICommand ISavebtn
        {
            get { return new RelayCommand(ISave, ISaveCanExcute); }
        }

        public ICommand IDeletebtn
        {
            get { return new RelayCommand(IDelete, IDeleteCanExecute); }
        }

        #endregion

        #region CanExcutes

        bool SaveCanExecute()
        {
            if (CurrentUniform == null)
            {
                return false;
            }

            else
                return true;
        }

        bool DeleteCanExecute()
        {
            if (CurrentUniform == null)
            {
                return false;
            }

            else
                return true;
        }

        bool ISaveCanExcute()
        {
            if (CurrentItem == null)
                return false;
            else
                return true;
        }

        bool IDeleteCanExecute()
        {
            if (CurrentItem == null)
                return false;
            else
                return true;
        }



        #endregion

        private void ISave()
        {
            //if (CurrentUniform == null)
            //{
            //    MessageBox.Show("Please select a Uniform Name", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            //else if (CurrentUniform.Uni_Name == null)
            //{
            //    MessageBox.Show("Please Enter a Uniform Name", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            if (CurrentItem.Item_Name == null)
            {
                MessageBox.Show("Please enter an Item name", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (CurrentItem.Price < 0)
            {
                MessageBox.Show("Please enter a Valid Price", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (CurrentItem.Price == null)
            {
                MessageBox.Show("Please enter a Valid Price", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {

                    bool IsUpdate = ItemList.Count(c => c.Item_No == CurrentItem.Item_No) == 1;

                    if (!IsUpdate)
                    {
                        dtl_Uniform SerItem = new dtl_Uniform();
                        SerItem.Item_No = serviceClient.GetLastItemNo();
                        SerItem.Item_Name = CurrentItem.Item_Name;
                        SerItem.Price = CurrentItem.Price;
                        SerItem.Uniform_ID = CurrentUniform.Uniform_ID;
                        SerItem.Is_Delete = false;

                        if (serviceClient.SaveUniformItem(SerItem))
                        {
                            MessageBox.Show("Item successfully saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreshUniformDetail();
                            INew();
                        }
                        else
                        {
                            MessageBox.Show("Save was not successfull", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreshUniformDetail();
                            INew();
                        }

                    }
                    else
                    {
                        if (CurrentItem.Item_Name.Length == 0)
                        {
                            MessageBox.Show("Please enter an Item name", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else if (CurrentItem.Price < 0)
                        {
                            MessageBox.Show("Please enter a Valid Price", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else if (CurrentItem.Price == null)
                        {
                            MessageBox.Show("Please enter a Valid Price", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        else
                        {
                            MessageBoxResult var = MessageBox.Show("Are You Sure You want to Update This Item?", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                            if (var == MessageBoxResult.OK)
                            {
                                dtl_Uniform SerItem = new dtl_Uniform();
                                SerItem.Item_No = CurrentItem.Item_No;
                                SerItem.Item_Name = CurrentItem.Item_Name;
                                SerItem.Price = CurrentItem.Price;
                                SerItem.Uniform_ID = CurrentUniform.Uniform_ID;

                                if (serviceClient.UpdateUniformItem(SerItem))
                                {
                                    MessageBox.Show("Item successfully updated", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                    refreshUniformDetail();
                                    INew();

                                }
                                else
                                {
                                    MessageBox.Show("Update was not successfull", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                    refreshUniformDetail();
                                    INew();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

        private void IDelete()
        {
            if (CurrentItem.Item_Name == null)
            {
                MessageBox.Show("Please Select an Item", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (CurrentItem.Price == null)
            {
                MessageBox.Show("Please Select an Item", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBoxResult var = MessageBox.Show("Are You Sure You want to Delete This Item?", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                if (var == MessageBoxResult.OK)
                {
                    try
                    {
                        dtl_Uniform serItem = new dtl_Uniform();
                        serItem.Item_No = CurrentItem.Item_No;

                        if (serviceClient.DeleteUniformItem(serItem))
                        {
                            MessageBox.Show("Item successfully deleted", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreshUniformDetail();
                            INew();

                        }
                        else
                        {
                            MessageBox.Show("Delete was not successfull", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreshUniformDetail();
                            INew();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

            }
        }

        private void INew()
        {
            if (CurrentUniform == null)
            {
                MessageBox.Show("Please Select a Uniform Name", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (CurrentUniform.Uni_Name == null)
            {
                MessageBox.Show("Please Select a Uniform Name", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    CurrentItem = null;
                    CurrentItem = new dtl_Uniform();
                    try
                    {
                        CurrentItem.Item_No = serviceClient.GetLastItemNo();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                refreshUniformDetail();
                TbUnlock2();
            }
        }

        private void Delete()
        {
            if (CurrentUniform.Uni_Name == null)
            {
                MessageBox.Show("Plesae Select a Uniform name", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else
            {
                MessageBoxResult var = MessageBox.Show("Are You Sure You want to Delete This Uniform?", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                if (var == MessageBoxResult.OK)
                {
                    try
                    {
                        z_Uniform serUniform = new z_Uniform();
                        serUniform.Uniform_ID = CurrentUniform.Uniform_ID;
                        if (serviceClient.DeleteUniform(serUniform))
                        {
                            MessageBox.Show("Uniform successfully deleted", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            CurrentUniform = null;
                            refreshUniform();
                            New();
                        }

                        else
                        {
                            MessageBox.Show("Delete was not successfull", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            CurrentUniform = null;
                            refreshUniform();
                            New();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

                refreshUniform();
                Items = null;

            }
        }

        private void Save()
        {
            if (CurrentUniform.Uni_Name == null)
            {
                MessageBox.Show("Plesae enter or select a Uniform name", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else
            {
                Ulist = new List<z_Uniform>();

                if (Uniform != null)
                    Ulist = Uniform.ToList();

                try
                {
                    bool isUpdate = Ulist.Count(c => c.Uniform_ID == CurrentUniform.Uniform_ID) == 1;
                    if (!isUpdate)
                    {

                        z_Uniform SerUniform = new z_Uniform();
                        SerUniform.Uniform_ID = CurrentUniform.Uniform_ID;
                        SerUniform.Uni_ID = CurrentUniform.Uni_ID;
                        SerUniform.Uni_Name = CurrentUniform.Uni_Name;
                        SerUniform.Is_Delete = false;

                        if (serviceClient.SaveUniform(SerUniform))
                        {
                            MessageBox.Show("Uniform successfully saved", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            refreshUniform();
                            New();
                        }

                        else
                        {
                            MessageBox.Show("Save was not successfull", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            refreshUniform();
                            New();
                        }
                    }

                    else
                    {
                        if (CurrentUniform.Uni_Name.Length == 0)
                        {
                            MessageBox.Show("Plesae enter a Uniform name", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBoxResult var = MessageBox.Show("Are You Sure You want to Update This Uniform?", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                            if (var == MessageBoxResult.OK)
                            {
                                z_Uniform SerUniform = new z_Uniform();
                                SerUniform.Uniform_ID = CurrentUniform.Uniform_ID;
                                SerUniform.Uni_ID = CurrentUniform.Uni_ID;
                                SerUniform.Uni_Name = CurrentUniform.Uni_Name;
                                SerUniform.Is_Delete = CurrentUniform.Is_Delete;

                                if (serviceClient.UpdateUniform(SerUniform))
                                {
                                    MessageBox.Show("Uniform successfully Updated", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                    refreshUniform();
                                    New();

                                }
                                else
                                {
                                    MessageBox.Show("Update was not successfull", "", MessageBoxButton.OK, MessageBoxImage.Error);
                                    refreshUniform();
                                    New();
                                }
                            }

                        }
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
        }

        private void New()
        {
            try
            {
                CurrentUniform = null;
                CurrentUniform = new z_Uniform();
                CurrentUniform.Uniform_ID = Guid.NewGuid();

                try
                {
                    CurrentUniform.Uni_ID = serviceClient.GetLastUniID();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.ToString());
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }

            refreshUniform();
            refreshUniformDetail();
            TbUnlock();
        }

        void TbUnlock()
        {
            Tb1 = true;
        }

        void TbUnlock2()
        {
            Tb2 = true;
        }

        private void FilterManager()
        {
            Items = ItemList;
            Items = Items.Where(c => c.Uniform_ID == CurrentUniform.Uniform_ID);
        }

        #endregion
    }
}
