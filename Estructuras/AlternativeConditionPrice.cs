using FacturaApi.archivos.CommonBasicComponents;
using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{

    [Serializable]
    public class AlternativeConditionPrice
    {
        public PayableAmount PriceAmount { get; set; }

        public string PriceTypeCode { get; set; }

        public AlternativeConditionPrice()
        {
            PriceAmount = new PayableAmount();
        }
    }
}