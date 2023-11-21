using Estructuras.CommonBasicComponents;
using System;

namespace Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class DeliveryTerms
    {
        public string Id { get; set; }
        public PayableAmount Amount { get; set; }
    }
}