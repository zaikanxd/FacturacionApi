using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class Consignment
    {
        public PlannedPickupTransportEvent PlannedPickupTransportEvent { get; set; }
        public string CarrierServiceInstructions { get; set; }
        public string Id { get; set; }
        public DeliveryTerms DeliveryTerms { get; set; }
        public TransportHandlingUnit TransportHandlingUnit { get; set; }
        public decimal DeclaredForCarriageValueAmount { get; set; }

        public Consignment()
        {
            DeliveryTerms = new DeliveryTerms();
            PlannedPickupTransportEvent = new PlannedPickupTransportEvent();
            TransportHandlingUnit = new TransportHandlingUnit();
        }
    }
}