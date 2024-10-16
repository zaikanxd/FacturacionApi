using Newtonsoft.Json;

namespace Dto.Modelos
{
    public class GrupoResumenNuevo : GrupoResumen
    {
        [JsonProperty(Required = Required.Always)]
        public string IdDocumento { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string TipoDocumentoReceptor { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string NroDocumentoReceptor { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int CodigoEstadoItem { get; set; }

        public string DocumentoRelacionado { get; set; }

        public string TipoDocumentoRelacionado { get; set; }

        public string Correlativo { get; set; }

        public string MotivoBaja { get; set; }
    }
}
