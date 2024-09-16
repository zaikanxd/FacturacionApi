using Newtonsoft.Json;

namespace Dto.Modelos
{
    public class Compania : Contribuyente
    {
        [JsonProperty(Order = 5)]
        [JsonRequired]
        public string CodigoAnexo { get; set; }

        public Compania()
        {
            CodigoAnexo = "0000";
        }
    }
}