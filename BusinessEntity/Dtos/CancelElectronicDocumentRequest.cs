using Dto.Modelos;

namespace BusinessEntity.Dtos
{
    public class CancelElectronicDocumentRequest
    {
        public string project { get; set; }
        public string idDocumento { get; set; }
        // Fecha de la comunicación de baja
        public string fechaEmision { get; set; }
        // Fecha emisión del comprobante
        public string fechaReferencia { get; set; }
        public Contribuyente emisor { get; set; }
        public DocumentoBaja documentoBaja { get; set; }
    }
}
