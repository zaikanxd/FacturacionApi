namespace BusinessEntity.Dtos
{
    public class SendXMLRequest
    {
        public int id { get; set; }
        public string project { get; set; }
        public string senderDocument { get; set; }
        public string series { get; set; }
        public int correlative { get; set; }
        public string xmlPath { get; set; }
        public string pdfPath { get; set; }
        public string qrCode { get; set; }
        public int receiptTypeId { get; set; }
    }
}
