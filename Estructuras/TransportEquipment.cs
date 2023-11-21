using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class TransportEquipment
    {
        public string Id { get; set; }
        public string SizeTypeCode { get; set; }

        public Delivery Delivery { get; set; }
        public bool ReturnabilityIndicator { get; set; }
    }
}