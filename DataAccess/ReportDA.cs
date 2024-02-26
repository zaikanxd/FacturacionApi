using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using InterfaceData;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using BusinessEntity;
using Populate;
using System;

namespace DataAccess
{
    public class ReportDA : IReportDA
    {
        private Database _db;
        public Database db { get { return (_db == null ? _db = new SqlDatabase(Util.AppSettings.cnxBillingBD) : _db); } }

        public List<ElectronicReceiptBE> getListBy(DateTime date, string project, string senderDocument)
        {
            List<ElectronicReceiptBE> list = new List<ElectronicReceiptBE>();

            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceiptReport_GetAllBy))
            {
                cmd.CommandTimeout = 0;
                db.AddInParameter(cmd, "date", DbType.Date, date);
                db.AddInParameter(cmd, "project", DbType.String, project);
                db.AddInParameter(cmd, "senderDocument", DbType.String, senderDocument);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    while (dr.Read())
                    {
                        list.Add(ElectronicReceiptP.getElectronicReceipt(dr));
                    }
                }
            }

            return list;
        }
    }
}
