using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dto.Modelos
{
    public class ResumenDiarioNuevo : DocumentoResumen
    {
        [JsonProperty(Required = Required.Always)]
        public List<GrupoResumenNuevo> Resumenes { get; set; }
    }
}
