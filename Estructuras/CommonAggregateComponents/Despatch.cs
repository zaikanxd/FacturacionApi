﻿using System;

namespace Estructuras.CommonAggregateComponents
{

    [Serializable]
    public class Despatch
    {
        public DespatchAddress DespatchAddress { get; set; }
        public string Instructions { get; set; }

    }
}