using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dto.Intercambio
{
    public class EnviarDocumentoResponse : RespuestaComunConArchivo
    {
        public string CodigoRespuesta { get; set; }

        public string MensajeRespuesta { get; set; }

        public string TramaZipCdr { get; set; }

        public string NroTicketCdr { get; set; }

    }
}