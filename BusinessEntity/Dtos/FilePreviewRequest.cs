using Dto.Modelos;

namespace BusinessEntity.Dtos
{
    public class FilePreviewRequest
    {
        public bool sinValorFiscal { get; set; }
        public DocumentoElectronico documento { get; set; }
    }
}
