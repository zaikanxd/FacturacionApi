using BusinessEntity;
using System;
using System.Collections.Generic;

namespace InterfaceData
{
    public interface IReportDA
    {
        List<ElectronicReceiptBE> getListBy(DateTime date, string project, string senderDocument);
    }
}
