using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class Delivery
    {
        public Despatch Despatch { get; set; }
        public Despatch DeliveryLocation { get; set; }
        public List<DeliveryTerms> DeliveryTerms { get; set; }
        public Shipment Shipment { get; set; }

        public Delivery()
        {
            Despatch = new Despatch();
            DeliveryLocation = new Despatch();
            DeliveryTerms = new List<DeliveryTerms>();
            Shipment = new Shipment();
        }
    }
}