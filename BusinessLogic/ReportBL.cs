using BusinessEntity;
using DataAccess;
using InterfaceData;
using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class ReportBL : IReportDA
    {
        private ReportDA _ReportDA;
        private ReportDA oReportDA
        {
            get { return (_ReportDA == null ? _ReportDA = new ReportDA() : _ReportDA); }
        }

        public List<ElectronicReceiptBE> getListBy(DateTime date, string project, string senderDocument)
        {
            return oReportDA.getListBy(date, project, senderDocument);
        }
    }
}
