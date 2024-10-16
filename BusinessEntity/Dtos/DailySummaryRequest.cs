using Dto.Modelos;

namespace BusinessEntity.Dtos
{
    public class DailySummaryRequest
    {
        public string project { get; set; }
        public string idDocumento { get; set; }
        // Fecha de emision del documento a dar de baja
        public string fechaEmision { get; set; }
        // Fecha que se da de baja
        public string fechaReferencia { get; set; }
        public Compania emisor { get; set; }
        public GrupoResumenNuevo resumen { get; set; }
    }
}
