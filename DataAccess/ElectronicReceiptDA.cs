using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Dto.Intercambio;
using Dto.Modelos;
using InterfaceData;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using BusinessEntity;
using Populate;
using System;
using BusinessEntity.Dtos;

namespace DataAccess
{
    public class ElectronicReceiptDA : IElectronicReceiptDA
    {
        private Database _db;
        public Database db { get { return (_db == null ? _db = new SqlDatabase(Util.AppSettings.cnxBillingBD) : _db); } }

        public void insertElectronicReceipt(EnviarDocumentoResponse pEnviarDocumentoResponse, DocumentoElectronico documento, string jsonPath)
        {
            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceipt_Insert))
            {
                cmd.CommandTimeout = 0;

                DataTable dt = new DataTable();
                dt.Columns.Add("description", typeof(string));
                dt.Columns.Add("additionalDescription", typeof(string));
                dt.Columns.Add("productCode", typeof(string));
                dt.Columns.Add("quantity", typeof(int));
                dt.Columns.Add("unitSunatCode", typeof(string));
                dt.Columns.Add("unitPrice", typeof(decimal));
                dt.Columns.Add("discount", typeof(decimal));
                dt.Columns.Add("igv", typeof(decimal));
                dt.Columns.Add("total", typeof(decimal));

                foreach (var det in documento.Items)
                {
                    decimal unitPrice = Math.Round(det.PrecioUnitario * 1.18m, 2);
                    decimal discount = Math.Round((det.Descuento / det.Cantidad) * 1.18m, 2);
                    dt.Rows.Add(det.Descripcion, det.DescripcionAdicional, det.CodigoItem, det.Cantidad, det.UnidadMedida, unitPrice, discount, det.Impuesto, det.PrecioReferencial * det.Cantidad);
                }

                var parameter = cmd.CreateParameter();
                parameter.ParameterName = "@electronicReceiptDet";
                parameter.Value = dt;

                cmd.Parameters.Add(parameter);

                DataTable dt2 = new DataTable();
                dt2.Columns.Add("accountType", typeof(string));
                dt2.Columns.Add("payment", typeof(decimal));
                dt2.Columns.Add("change", typeof(decimal));
                dt2.Columns.Add("date", typeof(DateTime));
                dt2.Columns.Add("operationNum", typeof(string));

                foreach (var det in documento.Cuentas)
                {
                    dt2.Rows.Add(det.Nombre, det.Pago, det.Vuelto, det.Fecha, det.NroOperacion);
                }

                var parameter2 = cmd.CreateParameter();
                parameter2.ParameterName = "@receiptPaymentDet";
                parameter2.Value = dt2;

                cmd.Parameters.Add(parameter2);

                var serieCorrelativo = documento.IdDocumento.Split('-');

                db.AddInParameter(cmd, "project", DbType.String, documento.Project);
                db.AddInParameter(cmd, "format", DbType.String, documento.formato);
                db.AddInParameter(cmd, "senderDocumentTypeId", DbType.Int32, int.Parse(documento.Emisor.TipoDocumento));
                db.AddInParameter(cmd, "senderDocument", DbType.String, documento.Emisor.NroDocumento);
                db.AddInParameter(cmd, "senderName", DbType.String, documento.Emisor.NombreLegal);
                db.AddInParameter(cmd, "series", DbType.String, serieCorrelativo[0]);
                db.AddInParameter(cmd, "correlative", DbType.Int32, int.Parse(serieCorrelativo[1]));
                db.AddInParameter(cmd, "receiptTypeId", DbType.Int32, int.Parse(documento.TipoDocumento));
                db.AddInParameter(cmd, "recipientDocumentTypeId", DbType.Int32, int.Parse(documento.Receptor.TipoDocumento));
                db.AddInParameter(cmd, "recipientDocument", DbType.String, documento.Receptor.NroDocumento);
                db.AddInParameter(cmd, "recipientName", DbType.String, documento.Receptor.NombreLegal);
                db.AddInParameter(cmd, "discount", DbType.Decimal, documento.DescuentoGlobal);
                db.AddInParameter(cmd, "subtotal", DbType.Decimal, documento.Gravadas);
                db.AddInParameter(cmd, "totalIGV", DbType.Decimal, documento.TotalIgv);
                db.AddInParameter(cmd, "total", DbType.Decimal, documento.TotalVenta);
                db.AddInParameter(cmd, "acceptedBySunat", DbType.Boolean, pEnviarDocumentoResponse.Exito);
                db.AddInParameter(cmd, "sunatDescription", DbType.String, pEnviarDocumentoResponse.MensajeRespuesta);
                db.AddInParameter(cmd, "qrCode", DbType.String, pEnviarDocumentoResponse.qrCode);
                db.AddInParameter(cmd, "pdfLink", DbType.String, pEnviarDocumentoResponse.pdfPath);
                db.AddInParameter(cmd, "xmlLink", DbType.String, pEnviarDocumentoResponse.xmlPath);
                db.AddInParameter(cmd, "issueDate", DbType.DateTime, documento.FechaEmision);
                db.AddInParameter(cmd, "issueTime", DbType.String, documento.HoraEmision);
                db.AddInParameter(cmd, "currency", DbType.String, documento.Moneda);
                db.AddInParameter(cmd, "errorMessage", DbType.String, pEnviarDocumentoResponse.MensajeError);
                db.AddInParameter(cmd, "cdrTicketNumber", DbType.String, pEnviarDocumentoResponse.NroTicketCdr);
                db.AddInParameter(cmd, "userCreated", DbType.String, documento.UserCreated);
                db.AddInParameter(cmd, "cdrLink", DbType.String, pEnviarDocumentoResponse.cdrPath);
                db.AddInParameter(cmd, "jsonLink", DbType.String, jsonPath);
                db.AddInParameter(cmd, "observation", DbType.String, documento.Notas);
                if (documento.Discrepancias != null && documento.Discrepancias.Count > 0)
                {
                    db.AddInParameter(cmd, "discrepancyRefNumber", DbType.String, documento.Discrepancias[0].NroReferencia);
                    db.AddInParameter(cmd, "discrepancyType", DbType.Int32, int.Parse(documento.Discrepancias[0].Tipo));
                    db.AddInParameter(cmd, "discrepancyDescription", DbType.String, documento.Discrepancias[0].Descripcion);
                }
                db.ExecuteNonQuery(cmd);
            }
        }

        public void updateElectronicReceipt(int id, EnviarDocumentoResponse pEnviarDocumentoResponse)
        {
            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceipt_Update))
            {
                cmd.CommandTimeout = 0;

                db.AddInParameter(cmd, "id", DbType.Int32, id);
                db.AddInParameter(cmd, "acceptedBySunat", DbType.Boolean, pEnviarDocumentoResponse.Exito);
                db.AddInParameter(cmd, "sunatDescription", DbType.String, pEnviarDocumentoResponse.MensajeRespuesta);
                db.AddInParameter(cmd, "errorMessage", DbType.String, pEnviarDocumentoResponse.MensajeError);
                db.AddInParameter(cmd, "cdrTicketNumber", DbType.String, pEnviarDocumentoResponse.NroTicketCdr);
                db.AddInParameter(cmd, "cdrLink", DbType.String, pEnviarDocumentoResponse.cdrPath);

                db.ExecuteNonQuery(cmd);
            }
        }

        public List<ElectronicReceiptBE> getListPending()
        {
            List<ElectronicReceiptBE> list = new List<ElectronicReceiptBE>();
            
            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceipt_GetAllPending))
            {
                cmd.CommandTimeout = 0;

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

        public List<ElectronicReceiptBE> getListBy(DateTime date, string filter = null)
        {
            List<ElectronicReceiptBE> list = new List<ElectronicReceiptBE>();

            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceipt_GetAllBy))
            {
                cmd.CommandTimeout = 0;
                db.AddInParameter(cmd, "date", DbType.Date, date);
                db.AddInParameter(cmd, "filter", DbType.String, filter);
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

        public ElectronicReceiptBE get(int id)
        {
            ElectronicReceiptBE oElectronicReceiptBE = new ElectronicReceiptBE();

            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceipt_GetOne))
            {
                cmd.CommandTimeout = 0;
                db.AddInParameter(cmd, "id", DbType.Int32, id);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    while (dr.Read())
                    {
                        oElectronicReceiptBE = ElectronicReceiptP.getElectronicReceipt(dr);
                    }
                }
            }

            return oElectronicReceiptBE;
        }

        public void cancelElectronicReceipt(CancelElectronicReceiptRequest cancelElectronicReceiptRequest)
        {
            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceipt_Cancel))
            {
                cmd.CommandTimeout = 0;

                db.AddInParameter(cmd, "project", DbType.String, cancelElectronicReceiptRequest.project);
                db.AddInParameter(cmd, "nroRUC", DbType.String, cancelElectronicReceiptRequest.nroRUC);
                db.AddInParameter(cmd, "series", DbType.String, cancelElectronicReceiptRequest.series);
                db.AddInParameter(cmd, "correlative", DbType.Int32, cancelElectronicReceiptRequest.correlative);
                db.AddInParameter(cmd, "cancellationReason", DbType.String, cancelElectronicReceiptRequest.cancellationReason);
                db.AddInParameter(cmd, "cancellationName", DbType.String, cancelElectronicReceiptRequest.cancellationName);
                db.AddInParameter(cmd, "canceledPdfLink", DbType.String, cancelElectronicReceiptRequest.canceledPdfLink);
                db.AddInParameter(cmd, "canceledXmlLink", DbType.String, cancelElectronicReceiptRequest.canceledXmlLink);
                db.AddInParameter(cmd, "canceledCdrLink", DbType.String, cancelElectronicReceiptRequest.canceledCdrLink);
                db.AddInParameter(cmd, "canceledTicketNumber", DbType.String, cancelElectronicReceiptRequest.canceledTicketNumber);

                db.ExecuteNonQuery(cmd);
            }
        }

        public string getJsonLink(JsonLinkRequest jsonLinkRequest)
        {
            string jsonLink = null;

            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_GetJsonLink))
            {
                cmd.CommandTimeout = 0;
                db.AddInParameter(cmd, "project", DbType.String, jsonLinkRequest.project);
                db.AddInParameter(cmd, "senderDocumentTypeId", DbType.Int32, jsonLinkRequest.senderDocumentTypeId);
                db.AddInParameter(cmd, "senderDocument", DbType.String, jsonLinkRequest.senderDocument);
                db.AddInParameter(cmd, "series", DbType.String, jsonLinkRequest.series);
                db.AddInParameter(cmd, "correlative", DbType.Int32, jsonLinkRequest.correlative);
                db.AddInParameter(cmd, "issueDate", DbType.DateTime, jsonLinkRequest.issueDate);
                db.AddOutParameter(cmd, "jsonLink", DbType.String, 100);
                db.ExecuteNonQuery(cmd);

                if (db.GetParameterValue(cmd, "jsonLink") != DBNull.Value)
                    jsonLink = Convert.ToString(db.GetParameterValue(cmd, "jsonLink"));
            }

            return jsonLink;
        }

        public void updateCanceledCdrLink(UpdateCanceledCdrLinkRequest updateCanceledCdrLinkRequest)
        {
            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_ElectronicReceipt_UpdateCanceledCdrLink))
            {
                cmd.CommandTimeout = 0;

                db.AddInParameter(cmd, "project", DbType.String, updateCanceledCdrLinkRequest.project);
                db.AddInParameter(cmd, "nroRUC", DbType.String, updateCanceledCdrLinkRequest.nroRUC);
                db.AddInParameter(cmd, "series", DbType.String, updateCanceledCdrLinkRequest.series);
                db.AddInParameter(cmd, "correlative", DbType.Int32, updateCanceledCdrLinkRequest.correlative);
                db.AddInParameter(cmd, "cancellationName", DbType.String, updateCanceledCdrLinkRequest.cancellationName);
                db.AddInParameter(cmd, "canceledCdrLink", DbType.String, updateCanceledCdrLinkRequest.canceledCdrLink);
                db.AddInParameter(cmd, "canceledTicketNumber", DbType.String, updateCanceledCdrLinkRequest.canceledTicketNumber);

                db.ExecuteNonQuery(cmd);
            }
        }
    }
}
