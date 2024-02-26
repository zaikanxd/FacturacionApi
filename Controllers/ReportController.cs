﻿using BusinessEntity;
using BusinessLogic;
using System.Collections.Generic;
using System.Web.Http;

namespace FacturacionApi.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("report")]
    public class ReportController : ApiController
    {
        private ReportBL _ReportBL;
        private ReportBL oReportBL
        {
            get { return (_ReportBL == null ? _ReportBL = new ReportBL() : _ReportBL); }
        }

        [HttpGet, Route("listBy")]
        public IEnumerable<ElectronicReceiptBE> getListBy(string project, string senderDocument)
        {
            return oReportBL.getListBy(project, senderDocument);
        }
    }
}