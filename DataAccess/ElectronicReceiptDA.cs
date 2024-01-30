using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Dto.Intercambio;
using Dto.Modelos;
using InterfaceData;
using System.Data.Common;

namespace DataAccess
{
    public class ElectronicReceiptDA : IElectronicReceiptDA
    {
        private Database _db;
        public Database db { get { return (_db == null ? _db = new SqlDatabase(Util.AppSettings.cnxBillingBD) : _db); } }

        public void insertElectronicReceipt(EnviarDocumentoResponse pEnviarDocumentoResponse, DocumentoElectronico documento)
        {
            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceipt_Insert))
            {
                cmd.CommandTimeout = 0;

                // TODO agregar los parametros

                db.ExecuteNonQuery(cmd);
            }
        }
    }
}
