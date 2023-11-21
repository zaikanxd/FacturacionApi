using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estructuras.CommonExtensionComponents
{
    [Serializable]
    public class UblExtensions
    {
        public UblExtension Extension1 { get; set; }

        public UblExtension Extension2 { get; set; }

        public UblExtensions()
        {
            Extension1 = new UblExtension();
            Extension2 = new UblExtension();
        }
    }
}