using System;

namespace BusinessEntity
{
    public class ElectronicReceiptBE
    {
        public int id { get; set; }
        public string project { get; set; }
        public string format { get; set; }
        public int senderDocumentTypeId { get; set; }
        public string senderDocument { get; set; }
        public string senderName { get; set; }
        public string series { get; set; }
        public int correlative { get; set; }
        public int receiptTypeId { get; set; }
        public int recipientDocumentTypeId { get; set; }
        public string recipientDocument { get; set; }
        public string recipientName { get; set; }
        public decimal discount { get; set; }
        public decimal subtotal { get; set; }
        public decimal totalIGV { get; set; }
        public decimal total { get; set; }
        public bool acceptedBySunat { get; set; }
        public string sunatDescription { get; set; }
        public string qrCode { get; set; }
        public string pdfLink { get; set; }
        public string xmlLink { get; set; }
        public DateTime issueDate { get; set; }
        public string issueTime { get; set; }
        public string currency { get; set; }
        public string errorMessage { get; set; }
        public string cdrTicketNumber { get; set; }
        public string userCreated { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime updateDate { get; set; }
        //
        public string senderDocumentType { get; set; }
        public string receiptType { get; set; }
        public string recipientDocumentType { get; set; }
    }
}
