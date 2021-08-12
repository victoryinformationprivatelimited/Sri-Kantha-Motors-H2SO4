using System;
using ERP.Dashboard.API;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace ERP.Dashboard
{
    class EmployeeSectionViewModel : ViewModelBase
    {
        #region Properties

        #region Model

                public static JArray upComingBirthdayList;
                public static JArray newJoineeList;
                public static JArray permanentList;
                public static JArray resignList;
                public static JArray companyGrowth;
                public static JArray employeesDepartmentWiseList;
                public static JArray employeesDesignationWiseList;

                public static List<APIData.EmployeesByDepartment> employeesDepartmentWise { get; set; }
                public static List<APIData.EmployeesByDesignation> employeesDesignationWise { get; set; }
                public static List<APIData.YearWiseEmploees> yearWiseEmployeeListByJoinDate { get; set; }
                public static List<APIData.YearWiseEmploees> yearWiseEmployeeListByResignDate { get; set; }
                public static List<APIData.MonthWiseEmploees> monthWiseEmployeeList { get; set; }


                private string _UpComingBirthdayCount;

                public string UpComingBirthdayCount
                {
                    get { return _UpComingBirthdayCount; }
                    set { _UpComingBirthdayCount = value; }
                }

                private int _permanentEmployeeCount;

                public int PermanentEmployeeCount
                {
                    get { return _permanentEmployeeCount; }
                    set { _permanentEmployeeCount = value; }
                }

                private int _resignEmployeeCount;

                public int ResignEmployeeCount
                {
                    get { return _resignEmployeeCount; }
                    set { _resignEmployeeCount = value; }
                }

                private int _newJoineeCount;

                public int NewJoineeCount
                {
                    get { return _newJoineeCount; }
                    set { _newJoineeCount = value; }
                }


        #endregion

        #region Charts

            #region Pie Chart


                    #endregion

            #region Line Chart

                    public static SeriesCollection SeriesCollection { get; set; }
                    public static SeriesCollection SeriesCollectionResign { get; set; }
                    public static string[] Labels { get; set; }
                    public static string[] LabelsResign { get; set; }
                    public static Func<double, string> Formatter { get; set; }



            #endregion

        #endregion

        #endregion

        #region Constructor

        public EmployeeSectionViewModel() {
            employeesDepartmentWise = new List<APIData.EmployeesByDepartment>();
            employeesDesignationWise = new List<APIData.EmployeesByDesignation>();
            yearWiseEmployeeListByJoinDate = new List<APIData.YearWiseEmploees>();
            yearWiseEmployeeListByResignDate = new List<APIData.YearWiseEmploees>();
            monthWiseEmployeeList = new List<APIData.MonthWiseEmploees>();
            backgroundLoading();
        }

        #endregion

        #region private Methods

        private async System.Threading.Tasks.Task loadupComingBirthdayListAsync()
        {
            upComingBirthdayList = await ApiMethods.getupComingBirthdayListAsync();
        }

        private async System.Threading.Tasks.Task loadupPermanentEmployeeListAsync()
        {
            permanentList = await ApiMethods.getPermanentListAsync();
        }

        private async System.Threading.Tasks.Task loadupResignEmployeeListAsync()
        {
            resignList = await ApiMethods.getResignListAsync();
        }

        private async System.Threading.Tasks.Task loadupNewJoineeListAsync()
        {
            newJoineeList = await ApiMethods.getResignListAsync();
        }

        private async System.Threading.Tasks.Task loadupEmployeesDepartmentListAsync()
        {
            employeesDepartmentWiseList = await ApiMethods.getEmployeesDepartmentWiseAsync();
        }

        private async System.Threading.Tasks.Task loadupEmployeesDesignationListAsync()
        {
            employeesDesignationWiseList = await ApiMethods.getEmployeesDesignationWiseAsync();
        }

        private async System.Threading.Tasks.Task loadupCompanyGrowthListAsync()
        {
            companyGrowth = await ApiMethods.getCompanyGrowthDetailsAsync();
        }

        private void backgroundLoading()
        {
            try
            {
                Task.Run(() => this.loadupComingBirthdayListAsync()).Wait();
                if (upComingBirthdayList.Count > 0)
                {
                    UpComingBirthdayCount = upComingBirthdayList.Count.ToString();
                }
                else
                {
                    UpComingBirthdayCount = "0";
                }

                Task.Run(() => this.loadupPermanentEmployeeListAsync()).Wait();
                if (permanentList.Count > 0)
                {
                    foreach (JObject item in permanentList)
                    {
                        PermanentEmployeeCount += Convert.ToInt32(item.GetValue("count").ToString());
                    }
                }
                else
                {
                    PermanentEmployeeCount = 0;
                }

                Task.Run(() => this.loadupResignEmployeeListAsync()).Wait();
                if (resignList.Count > 0)
                {
                    foreach (JObject item in resignList)
                    {
                        ResignEmployeeCount += Convert.ToInt32(item.GetValue("count").ToString());
                    }
                }
                else
                {
                    ResignEmployeeCount = 0;
                }

                Task.Run(() => this.loadupEmployeesDepartmentListAsync()).Wait();
                if (employeesDepartmentWiseList.Count > 0)
                {
                    //set department counts with departments
                    foreach (JObject x in employeesDepartmentWiseList)
                    {
                        APIData.EmployeesByDepartment obj = new APIData.EmployeesByDepartment(x.GetValue("department_id").ToString(), x.GetValue("department_Name").ToString(), x.GetValue("count").ToString());
                        employeesDepartmentWise.Add(obj);
                    }
                }
                else
                {

                }

                Task.Run(() => this.loadupEmployeesDesignationListAsync()).Wait();
                if (employeesDesignationWiseList.Count > 0)
                {
                    foreach (JObject x in employeesDesignationWiseList)
                    {
                        APIData.EmployeesByDesignation obj = new APIData.EmployeesByDesignation(x.GetValue("designation_id").ToString(), x.GetValue("designation_Name").ToString(), x.GetValue("count").ToString());
                        employeesDesignationWise.Add(obj);
                    }
                }

                Task.Run(() => this.loadupCompanyGrowthListAsync()).Wait();
                if (companyGrowth.Count > 0)
                {
                    //set department counts with departments
                    foreach (JObject x in companyGrowth)
                    {
                        APIData.YearWiseEmploees obj = new APIData.YearWiseEmploees(x.GetValue("year").ToString(), x.GetValue("count").ToString());
                        JArray jarr = JArray.Parse(x.GetValue("monthWiseEmployees").ToString());
                        foreach (JObject y in jarr)
                        {
                            APIData.MonthWiseEmploees monthWise = new APIData.MonthWiseEmploees(y.GetValue("monthNo").ToString(), y.GetValue("count").ToString());
                            monthWiseEmployeeList.Add(monthWise);
                        }
                        obj.setMonthWiseEmployeeList(monthWiseEmployeeList);
                        monthWiseEmployeeList = new List<APIData.MonthWiseEmploees>();
                        yearWiseEmployeeListByJoinDate.Add(obj);
                    }
                }
                else
                {

                }

                Task.Run(() => this.loadupResignEmployeeListAsync()).Wait();
                if (resignList.Count > 0)
                {
                    foreach (JObject x in resignList)
                    {
                        APIData.YearWiseEmploees obj = new APIData.YearWiseEmploees(x.GetValue("year").ToString(), x.GetValue("count").ToString());
                        JArray jarr = JArray.Parse(x.GetValue("monthWiseEmployees").ToString());
                        foreach (JObject y in jarr)
                        {
                            APIData.MonthWiseEmploees monthWise = new APIData.MonthWiseEmploees(y.GetValue("monthNo").ToString(), y.GetValue("count").ToString());
                            monthWiseEmployeeList.Add(monthWise);
                        }
                        obj.setMonthWiseEmployeeList(monthWiseEmployeeList);
                        monthWiseEmployeeList = new List<APIData.MonthWiseEmploees>();
                        yearWiseEmployeeListByResignDate.Add(obj);
                    }
                }
                else
                {
                }

                //Load New Joinees Count for current Month
                foreach (var x in yearWiseEmployeeListByJoinDate)
                {
                    if (x.year == DateTime.Today.Year.ToString())
                    {
                        foreach (var y in x.getMonthWiseEmployeeList())
                        {
                            if (y.monthNo == DateTime.Today.Month.ToString())
                            {
                                NewJoineeCount = Convert.ToInt32(y.count);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                clsMessages.setMessage("Something Went Wrong.");
            }
        }

        #endregion

        #region public Methods

        #region Chart Methods

        #region Pie Chart

        public static void loadPieChart(PieChart departmentWiseEmployeesPieChart)
        {
            //PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            ChartValues<double> cht_y_values = new ChartValues<double>();

            LiveCharts.SeriesCollection series = new LiveCharts.SeriesCollection();

            foreach (var n in employeesDepartmentWise)
            {
                if (Convert.ToInt32(n.count) > 0)
                {
                    PieSeries ps = new PieSeries
                    {
                        Title = n.department_Name,
                        Values = new ChartValues<double> { Convert.ToDouble(n.count) },
                        DataLabels = true,
                        //LabelPoint = labelPoint

                    };
                series.Add(ps);
                }
            }
            departmentWiseEmployeesPieChart.Series = series;
        }

        public static void loadDesignationWisePieChart(PieChart designationWiseEmployeesPieChart)
        {
            ChartValues<double> cht_y_values = new ChartValues<double>();

            LiveCharts.SeriesCollection series = new LiveCharts.SeriesCollection();

            foreach (var n in employeesDesignationWise)
            {
                if (Convert.ToInt32(n.count) > 0)
                {
                    PieSeries ps = new PieSeries
                    {
                        Title = n.designation_Name,
                        Values = new ChartValues<double> { Convert.ToDouble(n.count) },
                        DataLabels = true,
                        //LabelPoint = labelPoint

                    };
                    series.Add(ps);
                }
            }
            designationWiseEmployeesPieChart.Series = series;
        }

        public static void PieChart_DataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart) chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries) chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        #endregion

        #region Bar Chart

        public static void loadBarChart(CartesianChart yearWiseEmployeeCountLineChart)
        {
            SeriesCollection = new SeriesCollection();

            foreach (var x in yearWiseEmployeeListByJoinDate)
            {
                SeriesCollection.Add(new ColumnSeries
                {
                    Title = x.year,
                    Values = new ChartValues<int> {
                        Convert.ToInt32(x.monthWiseEmployees[0].count),
                        Convert.ToInt32(x.monthWiseEmployees[1].count),
                        Convert.ToInt32(x.monthWiseEmployees[2].count),
                        Convert.ToInt32(x.monthWiseEmployees[3].count),
                        Convert.ToInt32(x.monthWiseEmployees[4].count),
                        Convert.ToInt32(x.monthWiseEmployees[5].count),
                        Convert.ToInt32(x.monthWiseEmployees[6].count),
                        Convert.ToInt32(x.monthWiseEmployees[7].count),
                        Convert.ToInt32(x.monthWiseEmployees[8].count),
                        Convert.ToInt32(x.monthWiseEmployees[9].count),
                        Convert.ToInt32(x.monthWiseEmployees[10].count),
                        Convert.ToInt32(x.monthWiseEmployees[11].count)
                    }
                });
            }


            Labels = new[] {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
            //Formatter = value => value.ToString("N");
        }

        public static void loadBarChartResignCount(CartesianChart resignCountBarChart)
        {
            SeriesCollectionResign = new SeriesCollection();

            foreach (var x in yearWiseEmployeeListByResignDate)
            {
                SeriesCollectionResign.Add(new ColumnSeries
                {
                    Title = x.year,
                    Values = new ChartValues<int> {
                        Convert.ToInt32(x.monthWiseEmployees[0].count),
                        Convert.ToInt32(x.monthWiseEmployees[1].count),
                        Convert.ToInt32(x.monthWiseEmployees[2].count),
                        Convert.ToInt32(x.monthWiseEmployees[3].count),
                        Convert.ToInt32(x.monthWiseEmployees[4].count),
                        Convert.ToInt32(x.monthWiseEmployees[5].count),
                        Convert.ToInt32(x.monthWiseEmployees[6].count),
                        Convert.ToInt32(x.monthWiseEmployees[7].count),
                        Convert.ToInt32(x.monthWiseEmployees[8].count),
                        Convert.ToInt32(x.monthWiseEmployees[9].count),
                        Convert.ToInt32(x.monthWiseEmployees[10].count),
                        Convert.ToInt32(x.monthWiseEmployees[11].count)
                    }
                });
            }


            LabelsResign = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        }

        #endregion

        #endregion

        #endregion
    }
}
