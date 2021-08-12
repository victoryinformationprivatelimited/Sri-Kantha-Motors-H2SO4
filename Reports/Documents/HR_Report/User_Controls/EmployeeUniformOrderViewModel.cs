using ERP.ERPService;
using ERP.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace ERP.Reports.Documents.HR_Report.User_Controls
{
    class EmployeeUniformOrderViewModel : ViewModelBase
    {
        #region Service Object
        ERPServiceClient serviceClient;
        #endregion

        #region Lists
        List<Appointment_Letter_View> listsEmp = new List<Appointment_Letter_View>();
        List<dtl_Uniform> listItem = new List<dtl_Uniform>();
        List<dtl_Uniform> listSelectedItem = new List<dtl_Uniform>();
        List<EmployeeUniformOrderQtyView> DelList = new List<EmployeeUniformOrderQtyView>();
        List<EmployeeUniformOrderQtyView> listempOrder = new List<EmployeeUniformOrderQtyView>();
        //List<EmployeeUniformOrderQty> SelectedItemGridList = new List<EmployeeUniformOrderQty>();
        List<Employee_Uniform_Order_View> OrderList = new List<Employee_Uniform_Order_View>();
        #endregion

        #region Constructor

        public EmployeeUniformOrderViewModel()
        {
            try
            {
                serviceClient = new ERPServiceClient();
                RefreshEmployees();
                RefreshUniforms();
                RefreshItems();
                RefreshOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Properites
        string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; OnPropertyChanged("SearchText"); Search(); }
        }

        private int? searchIndex;
        public int? SearchIndex
        {
            get { return searchIndex; }
            set { searchIndex = value; OnPropertyChanged("SearchIndex"); }
        }

        private decimal? percentage;
        public decimal? Percentage
        {
            get { return percentage; }
            set { percentage = value; OnPropertyChanged("Percentage"); }
        }


        private DateTime? issueDate;
        public DateTime? IssueDate
        {
            get { return issueDate; }
            set { issueDate = value; OnPropertyChanged("IssueDate"); }
        }

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
            set { currentUniform = value; OnPropertyChanged("CurrentUniform"); ItemFilter(); }
        }

        private IEnumerable<dtl_Uniform> item;
        public IEnumerable<dtl_Uniform> Item
        {
            get { return item; }
            set { item = value; OnPropertyChanged("Item"); }
        }

        private dtl_Uniform currentItem;
        public dtl_Uniform CurrentItem
        {
            get { return currentItem; }
            set { currentItem = value; OnPropertyChanged("CurrentItem"); }
        }

        private IEnumerable<Appointment_Letter_View> employee;
        public IEnumerable<Appointment_Letter_View> Employee
        {
            get { return employee; }
            set { employee = value; OnPropertyChanged("Employee"); }
        }

        private Appointment_Letter_View currentemployee;
        public Appointment_Letter_View CurrentEmployee
        {
            get { return currentemployee; }
            set { currentemployee = value; OnPropertyChanged("CurrentEmployee"); }
        }

        private IEnumerable<EmployeeUniformOrderQtyView> selectedItemGrid;
        public IEnumerable<EmployeeUniformOrderQtyView> SelectedItemGrid
        {
            get { return selectedItemGrid; }
            set { selectedItemGrid = value; OnPropertyChanged("selectedItemGrid"); }
        }

        private EmployeeUniformOrderQtyView currentSelectedItemGrid;
        public EmployeeUniformOrderQtyView CurrentSelectedItemGrid
        {
            get { return currentSelectedItemGrid; }
            set { currentSelectedItemGrid = value; OnPropertyChanged("CurrentSelectedItemGrid"); }
        }

        private IEnumerable<Employee_Uniform_Order_View> order;
        public IEnumerable<Employee_Uniform_Order_View> Order
        {
            get { return order; }
            set { order = value; OnPropertyChanged("Order"); }
        }

        private Employee_Uniform_Order_View currentOrder;
        public Employee_Uniform_Order_View CurrentOrder
        {
            get { return currentOrder; }
            set { currentOrder = value; OnPropertyChanged("CurrentOrder"); }
        }

        private IEnumerable<EmployeeUniformOrderQtyView> deleteItems;
        public IEnumerable<EmployeeUniformOrderQtyView> DeleteItems
        {
            get { return deleteItems; }
            set { deleteItems = value; OnPropertyChanged("DeleteItems"); }
        }





        #endregion

        #region Refresh Methods

        private void RefreshEmployees()
        {
            try
            {
                listsEmp.Clear();
                IEnumerable<Appointment_Letter_View> ie;

                serviceClient.GetAppointmentViewCompleted += (s, e) =>
                {
                    ie = e.Result;
                    Employee = ie;
                    if (ie != null)
                    {
                        listsEmp = ie.ToList();
                    }
                };
                serviceClient.GetAppointmentViewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RefreshUniforms()
        {
            try
            {
                serviceClient.GetUniformsCompleted += (s, e) =>
                {
                    Uniform = e.Result;
                };
                serviceClient.GetUniformsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RefreshItems()
        {
            try
            {
                serviceClient.GetDtlUniformCompleted += (s, e) =>
                {
                    Item = e.Result;
                    listItem = Item.ToList();
                    Item = null;
                };
                serviceClient.GetDtlUniformAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RefreshOrders()
        {
            try
            {
                serviceClient.GetUniformOrdersViewCompleted += (s, e) =>
                {
                    Order = e.Result;
                    OrderList = Order.ToList();
                };
                serviceClient.GetUniformOrdersViewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RefreshDelOrders()
        {
            try
            {
                DelList.Clear();
                serviceClient.GetEmployeeUniformOrderQtyViewCompleted += (s, e) =>
                {
                    DeleteItems = e.Result;
                    if (DeleteItems != null)
                        DelList = DeleteItems.ToList();
                };
                serviceClient.GetEmployeeUniformOrderQtyViewAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Search
        private void Search()
        {
            Employee = null;
            Employee = listsEmp;
            try
            {
                if (SearchIndex == 0)
                    Employee = Employee.Where(c => c.first_name != null && c.first_name.ToUpper().Contains(SearchText.ToUpper()));
                else if (SearchIndex == 1)
                    Employee = Employee.Where(c => c.second_name != null && c.second_name.ToUpper().Contains(SearchText.ToUpper()));
                else if (SearchIndex == 2)
                    Employee = Employee.Where(c => c.emp_id != null && c.emp_id.ToUpper().Contains(SearchText.ToUpper()));
                else if (SearchIndex == 3)
                    Employee = Employee.Where(c => c.designation != null && c.designation.ToUpper().Contains(SearchText.ToUpper()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region FilterMethods

        private void ItemFilter()
        {
            try
            {
                Item = null;
                Item = listItem;
                if (Item.Count(c => c.Uniform_ID == CurrentUniform.Uniform_ID) == 0)
                {
                    Item = Item.Where(c => c.Uniform_ID == CurrentUniform.Uniform_ID);
                    MessageBox.Show("This Uniform has no Items");
                }
                else
                    Item = Item.Where(c => c.Uniform_ID == CurrentUniform.Uniform_ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        #endregion

        #region Button Commands

        public ICommand SingleSelct
        {
            get { return new RelayCommand(SinSelect); }
        }
        public ICommand SingleDeselect
        {
            get { return new RelayCommand(SinDeselect); }
        }
        public ICommand MultipleSelect
        {
            get { return new RelayCommand(MulSelect); }
        }
        public ICommand MultipleDeselect
        {
            get { return new RelayCommand(MulDeselect); }
        }
        public ICommand Savebtn
        {
            get { return new RelayCommand(Save); }
        }
        public ICommand Deletebtn
        {
            get { return new RelayCommand(Delete); }
        }

        #endregion

        #region Button Methods

        private void SinSelect()
        {
            try
            {
                if (CurrentUniform == null)
                    MessageBox.Show("Please Select a Uniform", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (CurrentItem == null)
                    MessageBox.Show("Please Select an Item", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    try
                    {
                        if (listempOrder.Count(c => c.Item_No == CurrentItem.Item_No) == 0)
                        {
                            EmployeeUniformOrderQtyView Titem = new EmployeeUniformOrderQtyView();
                            Titem.Item_No = CurrentItem.Item_No;
                            Titem.Item_Name = CurrentItem.Item_Name;
                            Titem.Quantity = 0;
                            Titem.Price = CurrentItem.Price;
                            listempOrder.Add(Titem);
                            SelectedItemGrid = null;
                            SelectedItemGrid = listempOrder;
                            CurrentItem = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SinDeselect()
        {
            if (CurrentUniform == null)
                MessageBox.Show("Please Select a Uniform", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else if (listempOrder.Count() == 0)
                MessageBox.Show("No Items to Remove", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else if (CurrentSelectedItemGrid == null)
                MessageBox.Show("Please Select an Item", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                if (listempOrder.Count(c => c.Item_No == CurrentSelectedItemGrid.Item_No) != 0)
                {
                    listempOrder.Remove(listempOrder.FirstOrDefault(c => c.Item_No == CurrentSelectedItemGrid.Item_No));
                }
                SelectedItemGrid = null;
                SelectedItemGrid = listempOrder;
            }
        }

        private void MulSelect()
        {
            try
            {
                if (CurrentUniform == null)
                    MessageBox.Show("Please Select a Uniform", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (Item == null)
                    MessageBox.Show("No Items in the List", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (Item.Count() == 0)
                    MessageBox.Show("No Items in the List", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else
                {
                    try
                    {
                        SelectedItemGrid = null;
                        listempOrder.Clear();
                        foreach (var x in Item)
                        {
                            EmployeeUniformOrderQtyView Titem = new EmployeeUniformOrderQtyView();
                            Titem.Item_No = x.Item_No;
                            Titem.Item_Name = x.Item_Name;
                            Titem.Price = x.Price;
                            Titem.Quantity = 0;
                            listempOrder.Add(Titem);
                        }

                        SelectedItemGrid = listempOrder;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MulDeselect()
        {
            if (SelectedItemGrid == null)
                MessageBox.Show("Please Select a Uniform", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else if (SelectedItemGrid.Count() == 0)
                MessageBox.Show("No Items to Remove", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            else
            {
                listempOrder.Clear();
                SelectedItemGrid = null;
                SelectedItemGrid = listempOrder;
            }
        }

        private void Save()
        {

            try
            {

                if (IssueDate == null)
                    MessageBox.Show("Please Select a Uniform Issue Date", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (SelectedItemGrid == null)
                    MessageBox.Show("Please Select a Uniform and Items to Offer", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (SelectedItemGrid.Count(c => c.Quantity <= 0) > 0)
                    MessageBox.Show("There are Zero Or Invalid Values in 'Quantity' Column" + "\n" + "Please Change its Value through 'Item Quantity'" + "\n" + "Text Box by Selecting the Item", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (Percentage == null || Percentage > 100 || Percentage < 0)
                    MessageBox.Show("Percentage Should be Zero or a Greater Value which is Less Than 100", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (CurrentEmployee == null)
                    MessageBox.Show("Please Select an Employee", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                else if (Order.Count(c => c.emp_id == CurrentEmployee.emp_id) > 0)
                    MessageBox.Show("You Have Already Offered a Uniform for this Employee", "", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    StringBuilder ItemsString = new StringBuilder();
                    foreach (var y in SelectedItemGrid)
                    {
                        ItemsString.AppendFormat("\n" + "\t" + "{0} => " + "{1}", y.Item_Name.ToString(), y.Quantity.ToString());
                    }

                    StringBuilder messageString = new StringBuilder();
                    messageString.AppendFormat("You are Going to Issue Employee no {0} , The {1} " + "Uniform " + "Containing Following Items" + "\n", CurrentEmployee.emp_id.ToString(), CurrentUniform.Uni_Name.ToString());
                    messageString.Append(ItemsString);
                    MessageBoxResult res = MessageBox.Show(messageString.ToString() + "\n" + "\n" + "Do you want to Proceed?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        try
                        {
                            z_EmployeeUniformOrders serOrder = new z_EmployeeUniformOrders();
                            serOrder.order_id = serviceClient.GetLastUniformOrderNo();
                            serOrder.employee_id = CurrentEmployee.employee_id;
                            serOrder.Uniform_ID = CurrentUniform.Uniform_ID;
                            serOrder.issue_date = IssueDate;
                            serOrder.Percentage = Percentage;
                            serOrder.Is_Delete = false;

                            List<dtl_EmployeeUniformOrder> serOrderItem = new List<dtl_EmployeeUniformOrder>();
                            foreach (var x in SelectedItemGrid)
                            {
                                dtl_EmployeeUniformOrder orderItem = new dtl_EmployeeUniformOrder();
                                orderItem.order_id = serOrder.order_id;
                                orderItem.Item_No = x.Item_No;
                                orderItem.Quantity = x.Quantity;
                                orderItem.Is_Delete = false;
                                serOrderItem.Add(orderItem);
                            }

                            if (serviceClient.SaveUniformOrders(serOrder, serOrderItem.ToArray()))
                            {
                                MessageBox.Show("Order Was Successfull", "", MessageBoxButton.OK, MessageBoxImage.Information);
                                RefreshOrders();
                                CurrentEmployee = null;
                                SelectedItemGrid = null;
                                Percentage = null;

                            }
                            else
                                MessageBox.Show("Order Was Unsuccessfull", "", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Delete()
        {
            if (CurrentOrder == null)
            {
                MessageBox.Show("Please Select an Order", "", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            }
            else
            {
                MessageBoxResult res = MessageBox.Show("Are you sure You Want to Delete This Record?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        RefreshDelOrders();
                        z_EmployeeUniformOrders serOrder = new z_EmployeeUniformOrders();
                        serOrder.order_id = CurrentOrder.order_id;

                        List<dtl_Uniform> DelItemlist = new List<dtl_Uniform>();
                        DelItemlist.Clear();
                        foreach (var x in DelList.Where(c => c.order_id == CurrentOrder.order_id))
                        {
                            dtl_Uniform Ditem = new dtl_Uniform();
                            Ditem.Item_No = x.Item_No;
                            DelItemlist.Add(Ditem);
                        }

                        List<dtl_EmployeeUniformOrder> serOrderItem = new List<dtl_EmployeeUniformOrder>();
                        foreach (var x in DelItemlist)
                        {
                            dtl_EmployeeUniformOrder orderItem = new dtl_EmployeeUniformOrder();
                            orderItem.Item_No = x.Item_No;
                            orderItem.order_id = CurrentOrder.order_id;
                            serOrderItem.Add(orderItem);
                        }

                        if (serviceClient.DeleteUniformOrders(serOrder, serOrderItem.ToArray()))
                        {
                            MessageBox.Show("Order Deleted", "", MessageBoxButton.OK, MessageBoxImage.Information);
                            RefreshOrders();
                            RefreshDelOrders();
                            CurrentOrder = null;
                            CurrentEmployee = null;
                            SelectedItemGrid = null;
                            Percentage = null;
                        }
                        else
                            MessageBox.Show("Delete Failed", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Delete Failed", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }

        }

        #endregion
    }
}