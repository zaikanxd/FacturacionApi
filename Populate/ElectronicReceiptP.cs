using BusinessEntity;
using System.Data;

namespace Populate
{
    public class ElectronicReceiptP
    {
        public static ElectronicReceiptBE getElectronicReceipt(IDataReader dr)
        {
            ElectronicReceiptBE oElectronicReceiptBE = new ElectronicReceiptBE();

            if (!dr.IsDBNull(dr.GetOrdinal("id")))
                oElectronicReceiptBE.id = dr.GetInt32(dr.GetOrdinal("id"));
            if (!dr.IsDBNull(dr.GetOrdinal("project")))
                oElectronicReceiptBE.project = dr.GetString(dr.GetOrdinal("project"));
            if (!dr.IsDBNull(dr.GetOrdinal("format")))
                oElectronicReceiptBE.format = dr.GetString(dr.GetOrdinal("format"));
            if (!dr.IsDBNull(dr.GetOrdinal("senderDocumentTypeId")))
                oElectronicReceiptBE.senderDocumentTypeId = dr.GetInt32(dr.GetOrdinal("senderDocumentTypeId"));
            if (!dr.IsDBNull(dr.GetOrdinal("senderDocument")))
                oElectronicReceiptBE.senderDocument = dr.GetString(dr.GetOrdinal("senderDocument"));
            if (!dr.IsDBNull(dr.GetOrdinal("senderName")))
                oElectronicReceiptBE.senderName = dr.GetString(dr.GetOrdinal("senderName"));
            if (!dr.IsDBNull(dr.GetOrdinal("series")))
                oElectronicReceiptBE.series = dr.GetString(dr.GetOrdinal("series"));
            if (!dr.IsDBNull(dr.GetOrdinal("correlative")))
                oElectronicReceiptBE.correlative = dr.GetInt32(dr.GetOrdinal("correlative"));
            if (!dr.IsDBNull(dr.GetOrdinal("receiptTypeId")))
                oElectronicReceiptBE.receiptTypeId = dr.GetInt32(dr.GetOrdinal("receiptTypeId"));
            if (!dr.IsDBNull(dr.GetOrdinal("recipientDocumentTypeId")))
                oElectronicReceiptBE.recipientDocumentTypeId = dr.GetInt32(dr.GetOrdinal("recipientDocumentTypeId"));
            if (!dr.IsDBNull(dr.GetOrdinal("recipientDocument")))
                oElectronicReceiptBE.recipientDocument = dr.GetString(dr.GetOrdinal("recipientDocument"));
            if (!dr.IsDBNull(dr.GetOrdinal("recipientName")))
                oElectronicReceiptBE.recipientName = dr.GetString(dr.GetOrdinal("recipientName"));
            if (!dr.IsDBNull(dr.GetOrdinal("discount")))
                oElectronicReceiptBE.discount = dr.GetDecimal(dr.GetOrdinal("discount"));
            if (!dr.IsDBNull(dr.GetOrdinal("subtotal")))
                oElectronicReceiptBE.subtotal = dr.GetDecimal(dr.GetOrdinal("subtotal"));
            if (!dr.IsDBNull(dr.GetOrdinal("totalIGV")))
                oElectronicReceiptBE.totalIGV = dr.GetDecimal(dr.GetOrdinal("totalIGV"));
            if (!dr.IsDBNull(dr.GetOrdinal("total")))
                oElectronicReceiptBE.total = dr.GetDecimal(dr.GetOrdinal("total"));
            if (!dr.IsDBNull(dr.GetOrdinal("acceptedBySunat")))
                oElectronicReceiptBE.acceptedBySunat = dr.GetBoolean(dr.GetOrdinal("acceptedBySunat"));
            if (!dr.IsDBNull(dr.GetOrdinal("sunatDescription")))
                oElectronicReceiptBE.sunatDescription = dr.GetString(dr.GetOrdinal("sunatDescription"));
            if (!dr.IsDBNull(dr.GetOrdinal("qrCode")))
                oElectronicReceiptBE.qrCode = dr.GetString(dr.GetOrdinal("qrCode"));
            if (!dr.IsDBNull(dr.GetOrdinal("pdfLink")))
                oElectronicReceiptBE.pdfLink = dr.GetString(dr.GetOrdinal("pdfLink"));
            if (!dr.IsDBNull(dr.GetOrdinal("xmlLink")))
                oElectronicReceiptBE.xmlLink = dr.GetString(dr.GetOrdinal("xmlLink"));
            if (!dr.IsDBNull(dr.GetOrdinal("issueDate")))
                oElectronicReceiptBE.issueDate = dr.GetDateTime(dr.GetOrdinal("issueDate"));
            if (!dr.IsDBNull(dr.GetOrdinal("issueTime")))
                oElectronicReceiptBE.issueTime = dr.GetString(dr.GetOrdinal("issueTime"));
            if (!dr.IsDBNull(dr.GetOrdinal("currency")))
                oElectronicReceiptBE.currency = dr.GetString(dr.GetOrdinal("currency"));
            if (!dr.IsDBNull(dr.GetOrdinal("errorMessage")))
                oElectronicReceiptBE.errorMessage = dr.GetString(dr.GetOrdinal("errorMessage"));
            if (!dr.IsDBNull(dr.GetOrdinal("cdrTicketNumber")))
                oElectronicReceiptBE.cdrTicketNumber = dr.GetString(dr.GetOrdinal("cdrTicketNumber"));
            if (!dr.IsDBNull(dr.GetOrdinal("userCreated")))
                oElectronicReceiptBE.userCreated = dr.GetString(dr.GetOrdinal("userCreated"));
            if (!dr.IsDBNull(dr.GetOrdinal("creationDate")))
                oElectronicReceiptBE.creationDate = dr.GetDateTime(dr.GetOrdinal("creationDate"));
            if (!dr.IsDBNull(dr.GetOrdinal("updateDate")))
                oElectronicReceiptBE.updateDate = dr.GetDateTime(dr.GetOrdinal("updateDate"));
            if (!dr.IsDBNull(dr.GetOrdinal("cdrLink")))
                oElectronicReceiptBE.cdrLink = dr.GetString(dr.GetOrdinal("cdrLink"));
            if (!dr.IsDBNull(dr.GetOrdinal("canceled")))
                oElectronicReceiptBE.canceled = dr.GetBoolean(dr.GetOrdinal("canceled"));
            if (!dr.IsDBNull(dr.GetOrdinal("cancellationReason")))
                oElectronicReceiptBE.cancellationReason = dr.GetString(dr.GetOrdinal("cancellationReason"));
            if (!dr.IsDBNull(dr.GetOrdinal("cancellationName")))
                oElectronicReceiptBE.cancellationName = dr.GetString(dr.GetOrdinal("cancellationName"));

            if (!dr.IsDBNull(dr.GetOrdinal("senderDocumentType")))
                oElectronicReceiptBE.senderDocumentType = dr.GetString(dr.GetOrdinal("senderDocumentType"));
            if (!dr.IsDBNull(dr.GetOrdinal("receiptType")))
                oElectronicReceiptBE.receiptType = dr.GetString(dr.GetOrdinal("receiptType"));
            if (!dr.IsDBNull(dr.GetOrdinal("recipientDocumentType")))
                oElectronicReceiptBE.recipientDocumentType = dr.GetString(dr.GetOrdinal("recipientDocumentType"));

            return oElectronicReceiptBE;
        }
    }
}