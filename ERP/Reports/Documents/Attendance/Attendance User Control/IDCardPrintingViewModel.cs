using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.ERPService;
using System.Windows.Input;
using ERP.BasicSearch;
using System.Data;
using System.Windows.Forms;
using ERP.Reports.Documents.Attendance;

namespace ERP.Reports.Documents.Attendance.Attendance_User_Control
{
    class IDCardPrintingViewModel : ViewModelBase
    {

        ERPServiceClient serviceClient;
        List<GetIDCardDetailsView> empList = new List<GetIDCardDetailsView>();
        List<GetIDCardDetailsView> idList = new List<GetIDCardDetailsView>();
        EmployeeIDData empDataSet = new EmployeeIDData();

        public IDCardPrintingViewModel()
        {
            serviceClient = new ERPServiceClient();
            RefreshIDList();
        }

        #region Filter
        private IEnumerable<GetIDCardDetailsView> _Filter;
        public IEnumerable<GetIDCardDetailsView> Filter
        {
            get { return _Filter; }
            set { _Filter = value; OnPropertyChanged("Filter"); }
        }

        #endregion

        #region RefreshList

        void RefreshIDList()
        {
            serviceClient.GetIDCardInformationViewCompleted += (s, e) =>
            {
                Filter = e.Result;
                if (Filter != null)
                {
                    idList = Filter.ToList();
                }
            };

            serviceClient.GetIDCardInformationViewAsync();
        }

        #endregion

        #region Select Button
        public ICommand SelectEmployeesButton
        {
            get { return new RelayCommand(SelectEmp); }
        }

        void SelectEmp()
        {
            empDataSet.GetIDCardDetailsView.Clear();
            empList.Clear();
            EmployeeMultipleSearchWindow window = new EmployeeMultipleSearchWindow();
            window.ShowDialog();
            if (window.viewModel.selectEmployeeList != null && window.viewModel.selectEmployeeList.Count > 0)
                try
                {
                    foreach (var item in window.viewModel.selectEmployeeList)
                    {
                        if (idList.Where(c => c.employee_id == item.employee_id) != null)
                            empList.Add(idList.Where(c => c.employee_id == item.employee_id).FirstOrDefault());
                    }
                    foreach (GetIDCardDetailsView currentID in empList)
                    {
                        EmployeeIDData.GetIDCardDetailsViewRow addRow = empDataSet.GetIDCardDetailsView.NewGetIDCardDetailsViewRow();
                        addRow.employee_id = currentID.employee_id;
                        addRow.emp_id = currentID.emp_id;
                        addRow.initials = currentID.initials;
                        addRow.first_name = currentID.first_name;
                        addRow.second_name = currentID.second_name;
                        addRow.surname = currentID.surname;
                        addRow.section_name = currentID.section_name;
                        addRow.department_name = currentID.department_name;
                        addRow.designation = currentID.designation;
                        addRow.nic = currentID.nic;
                        addRow.empImage = currentID.empImage;
                        if (currentID.join_date != null)
                            addRow.join_date = (DateTime)currentID.join_date;
                        empDataSet.GetIDCardDetailsView.AddGetIDCardDetailsViewRow(addRow);
                    }

                }
                catch (Exception)
                {
                    clsMessages.setMessage("Data Set Error");
                }

            window.Close();
        }
        #endregion


        #region PrintButton

        public ICommand PrintButton
        {
            get { return new RelayCommand(Print); }
        }

        void Print()
        {

            try
            {

                IDCardPrintingReport rpt = new IDCardPrintingReport();
                rpt.SetDataSource(empDataSet);
                ReportViewer r = new ReportViewer(rpt, false);
                r.Show();

            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }

        #endregion
    }
}
