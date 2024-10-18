using Dto.Modelos;

namespace BusinessEntity.Dtos
{
    public class DailySummaryRequest
    {
        public string project { get; set; }
        public string idDocumento { get; set; }
        // Fecha de la comunicación de baja
        public string fechaEmision { get; set; }
        // Fecha emisión del comprobante
        public string fechaReferencia { get; set; }
        public Compania emisor { get; set; }
        public GrupoResumenNuevo resumen { get; set; }
    }
}
