using System;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class UsabilityPeriod
    {
        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int DurationMeasure { get; set; }
    }
}