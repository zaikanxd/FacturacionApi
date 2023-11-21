using Estructuras.CommonBasicComponents;
using System;

namespace Estructuras.SunatAggregateComponents
{
    [Serializable]
    public class BillingPayment
    {
        public PartyIdentificationId Id { get; set; }

        public PayableAmount PaidAmount { get; set; }

        public string InstructionId { get; set; }

        public BillingPayment()
        {
            PaidAmount = new PayableAmount();
            Id = new PartyIdentificationId();
        }
    }
}