using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class PlannedPickupTransportEvent
    {
        public string LocationId { get; set; }
    }
}