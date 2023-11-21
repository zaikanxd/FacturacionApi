using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class MeasurementDimension
    {
        public string AttributeId { get; set; }
        public decimal Measure { get; set; }

    }
}