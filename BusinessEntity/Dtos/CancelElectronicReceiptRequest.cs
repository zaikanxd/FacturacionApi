namespace BusinessEntity.Dtos
{
    public class CancelElectronicReceiptRequest
    {
        public string project { get; set; }
        public string nroRUC { get; set; }
        public string series { get; set; }
        public string correlative { get; set; }
        public string cancellationReason { get; set; }
        public string cancellationName { get; set; }
        public string canceledPdfLink { get; set; }
        public string canceledXmlLink { get; set; }
        public string canceledCdrLink { get; set; }
        public string canceledTicketNumber { get; set; }
    }
}
