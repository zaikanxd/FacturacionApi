﻿using Microsoft.Practices.EnterpriseLibrary.Data;
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
    }
}
