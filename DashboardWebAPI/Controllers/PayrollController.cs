using DashboardService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DashboardWebAPI.Controllers
{

    [RoutePrefix("Api/Payroll")]
    public class PayrollController : ApiController
    {

        #region Dashboard Service
        private IDashboardServicePayroll dashboardService = new DashboardService.Services.DashboardService();
        #endregion

        #region Properties
        #endregion

        #region Root Route

        [Route("")]
        public IHttpActionResult Get()
        {
            var result = "Welcome to H2SO4 Dashboard API : Payroll Section";
            if (result.Count() == 0)
                return NotFound();
            return Ok(result);
        }

        #endregion

        #region Payroll Section
        #endregion

    }

}
