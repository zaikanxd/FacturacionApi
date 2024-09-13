namespace BusinessEntity.Dtos
{
    public class JsonLinkRequest
    {
        public string project { get; set; }

        public int senderDocumentTypeId { get; set; }

        public string senderDocument { get; set; }

        public string series { get; set; }

        public int correlative { get; set; }

        public string issueDate { get; set; }
    }
}