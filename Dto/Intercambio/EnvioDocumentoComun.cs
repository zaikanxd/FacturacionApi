﻿using Newtonsoft.Json;

namespace Dto.Intercambio
{
    public abstract class EnvioDocumentoComun
    {
        [JsonProperty(Required = Required.Always)]
        public string Ruc { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string UsuarioSol { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string ClaveSol { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string IdDocumento { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string TipoDocumento { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string EndPointUrl { get; set; }
    }
}