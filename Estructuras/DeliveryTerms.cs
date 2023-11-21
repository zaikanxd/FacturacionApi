using FacturaApi.archivos.CommonBasicComponents;
using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class DeliveryTerms
    {
        public string Id { get; set; }
        public PayableAmount Amount { get; set; }
    }
}