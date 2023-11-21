using FacturaApi.archivos.CommonBasicComponents;
using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class PartyIdentification
    {
        public PartyIdentificationId Id { get; set; }

        public PartyIdentification()
        {
            Id = new PartyIdentificationId();
        }
    }
}