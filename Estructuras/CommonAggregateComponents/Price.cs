using Estructuras.CommonBasicComponents;
using System;

namespace Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class Price
    {
        public PayableAmount PriceAmount { get; set; }

        public Price()
        {
            PriceAmount = new PayableAmount();
        }
    }
}