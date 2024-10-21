namespace BusinessEntity.Dtos
{
    public class UpdateCanceledCdrLinkRequest
    {
        public string project { get; set; }
        public string nroRUC { get; set; }
        public string series { get; set; }
        public int correlative { get; set; }
        public string cancellationName { get; set; }
        public string canceledCdrLink { get; set; }
        public string canceledTicketNumber { get; set; }
    }
}
