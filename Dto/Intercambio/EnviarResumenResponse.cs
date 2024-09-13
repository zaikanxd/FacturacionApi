namespace Dto.Intercambio
{
    public class EnviarResumenResponse : RespuestaComunConArchivo
    {
        public string NroTicket { get; set; }

        public string pdfPath { get; set; }

        public string xmlPath { get; set; }

        public string cdrPath { get; set; }
    }
}
