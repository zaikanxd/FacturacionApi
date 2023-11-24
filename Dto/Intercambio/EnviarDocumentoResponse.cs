namespace Dto.Intercambio
{
    public class EnviarDocumentoResponse : RespuestaComunConArchivo
    {
        public string CodigoRespuesta { get; set; }

        public string MensajeRespuesta { get; set; }

        public string TramaZipCdr { get; set; }

        public string NroTicketCdr { get; set; }

        public string qrCode { get; set; }

        public string pdfPath { get; set; }

        public string xmlPath { get; set; }
    }
}