using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class AccountingSupplierParty
    {
        public Party Party { get; set; }

        public PartyTaxScheme PartyTaxScheme { get; set; }

        public string CustomerAssignedAccountId { get; set; }

        public string AdditionalAccountId { get; set; }

        public AccountingSupplierParty()
        {
            Party = new Party();
        }
    }
}