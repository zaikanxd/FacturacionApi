using FacturaApi.archivos.CommonBasicComponents;
using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class AllowanceCharge
    {
        public bool ChargeIndicator { get; set; }

        public PayableAmount Amount { get; set; }

        public string ReasonCode { get; set; }

        public decimal MultiplierFactorNumeric { get; set; }

        public PayableAmount BaseAmount { get; set; }

        public AllowanceCharge()
        {
            Amount = new PayableAmount();
            BaseAmount = new PayableAmount();
            ReasonCode = "00";
        }
    }
}