using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dto.Modelos
{
    public class ComunicacionBaja : DocumentoResumen
    {
        [JsonProperty(Required = Required.Always)]
        public List<DocumentoBaja> Bajas { get; set; }

        public string Project { get; set; }
    }
}
