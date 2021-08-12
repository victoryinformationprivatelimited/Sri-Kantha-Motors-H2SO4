﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERP.Reports.Documents.Leave.LeaveUserControl
{
    /// <summary>
    /// Interaction logic for LeaveSumarryReportUserControl.xaml
    /// </summary>
    public partial class LeaveSumarryReportUserControl : UserControl
    {
        LeaveSumarryViewModel viewmodel = new LeaveSumarryViewModel();
        public LeaveSumarryReportUserControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.DataContext = viewmodel; };
        }
    }
}
