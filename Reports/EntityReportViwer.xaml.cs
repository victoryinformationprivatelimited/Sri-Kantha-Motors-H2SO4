using CrystalDecisions.CrystalReports.Engine;
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
using System.Windows.Shapes;

namespace ERP.Reports
{
    /// <summary>
    /// Interaction logic for EntityReportViwer.xaml
    /// </summary>
    public partial class EntityReportViwer : Window
    {
        public EntityReportViwer()
        {
            InitializeComponent();
        }

        public void setReport(ReportDocument objReportDocument)
        {
            ReportDocument objrep = new ReportDocument();
            objrep = objReportDocument;
            EntityViwer.ViewerCore.ReportSource = objrep;
        }
    }
}
