using System;
using System.Collections.Generic;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class Item
    {
        public string Description { get; set; }

        public SellersItemIdentification SellersItemIdentification { get; set; }

        public CommodityClassification CommodityClassification { get; set; }

        public StandardItemIdentification StandardItemIdentification { get; set; }

        public List<AdditionalItemProperty> AdditionalItemProperties { get; set; }

        public Item()
        {
            SellersItemIdentification = new SellersItemIdentification();
            CommodityClassification = new CommodityClassification();
            StandardItemIdentification = new StandardItemIdentification();
            AdditionalItemProperties = new List<AdditionalItemProperty>();
        }
    }
}