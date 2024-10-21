namespace BusinessEntity.Dtos
{
    public class CancellationTicketRequest
    {
        public string project { get; set; }
        public string nroRUC { get; set; }
        public string nroTicket { get; set; }
        public string nombreArchivo { get; set; }
        public int receiptTypeId { get; set; }
        public string series { get; set; }
        public int correlative { get; set; }
    }
}
