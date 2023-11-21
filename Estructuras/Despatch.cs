using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturaApi.archivos.CommonAggregateComponents
{

    [Serializable]
    public class Despatch
    {
        public DespatchAddress DespatchAddress { get; set; }
        public string Instructions { get; set; }

    }
}