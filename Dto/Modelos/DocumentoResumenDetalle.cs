using Newtonsoft.Json;

namespace Dto.Modelos
{
    public abstract class DocumentoResumenDetalle
    {
        [JsonProperty(Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string TipoDocumento { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string Serie { get; set; }
    }
}
