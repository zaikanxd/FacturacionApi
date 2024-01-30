using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Dto.Intercambio;
using Dto.Modelos;
using InterfaceData;
using System.Data.Common;
using System.Data;

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

                var serieCorrelativo = documento.IdDocumento.Split('-');

                db.AddInParameter(cmd, "series", DbType.String, serieCorrelativo[0]);
                db.AddInParameter(cmd, "correlative", DbType.Int32, int.Parse(serieCorrelativo[1]));
                db.AddInParameter(cmd, "receiptTypeId", DbType.Int32, int.Parse(documento.TipoDocumento));
                db.AddInParameter(cmd, "documentTypeReceiptId", DbType.Int32, int.Parse(documento.Receptor.TipoDocumento));
                db.AddInParameter(cmd, "document", DbType.String, documento.Receptor.NroDocumento);
                db.AddInParameter(cmd, "name", DbType.String, documento.Receptor.NombreLegal);
                db.AddInParameter(cmd, "subtotal", DbType.Decimal, documento.Gravadas);
                db.AddInParameter(cmd, "totalIGV", DbType.Decimal, documento.TotalIgv);
                db.AddInParameter(cmd, "total", DbType.Decimal, documento.TotalVenta);
                db.AddInParameter(cmd, "acceptedBySunat", DbType.Boolean, pEnviarDocumentoResponse.Exito);
                db.AddInParameter(cmd, "sunatDescription", DbType.String, pEnviarDocumentoResponse.MensajeRespuesta);
                db.AddInParameter(cmd, "canceled", DbType.Boolean, false);
                db.AddInParameter(cmd, "qrCode", DbType.String, pEnviarDocumentoResponse.qrCode);
                db.AddInParameter(cmd, "pdfLink", DbType.String, pEnviarDocumentoResponse.pdfPath);
                db.AddInParameter(cmd, "xmlLink", DbType.String, pEnviarDocumentoResponse.xmlPath);
                db.AddInParameter(cmd, "issueDate", DbType.DateTime, documento.FechaEmision);
                db.AddInParameter(cmd, "currency", DbType.String, documento.Moneda);
                db.AddInParameter(cmd, "userCreated", DbType.String, documento.UserCreated);

                /*
                //DATOS DE EMPRESA EMISORA Y PROYECTO
                db.AddInParameter(cmd, "document", DbType.String, documento.Emisor.NombreLegal);
                db.AddInParameter(cmd, "document", DbType.String, documento.Emisor.TipoDocumento);
                db.AddInParameter(cmd, "document", DbType.String, documento.Emisor.NroDocumento);

                // CODIGO ERROR Y MENSAJE ERROR
                db.AddInParameter(cmd, "errorMessage", DbType.String, pEnviarDocumentoResponse.MensajeError);
                */

                db.ExecuteNonQuery(cmd);
            }
        }
    }
}
