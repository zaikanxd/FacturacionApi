using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class DespatchAddress
    {
        public string Id { get; set; }
        public string AddressLine { get; set; }
    }
}