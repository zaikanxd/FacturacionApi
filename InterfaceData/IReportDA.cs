using BusinessEntity;
using System.Collections.Generic;

namespace InterfaceData
{
    public interface IReportDA
    {
        List<ElectronicReceiptBE> getListBy(string project, string senderDocument);
    }
}
