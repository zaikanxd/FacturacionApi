using FacturaApi.archivos.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacturaApi.archivos.CommonAggregateComponents
{
    [Serializable]
    public class CompanyId
    {
        public string SchemeId { get; set; }

        public string SchemeName { get; set; }

        public string SchemeAgencyName { get; set; }

        public string SchemeUri { get; set; }

        public string Value { get; set; }

        public CompanyId()
        {
            SchemeName = ValoresUbl.CompanySchemeName;
            SchemeAgencyName = ValoresUbl.SchemeAgencyName;
            SchemeUri = ValoresUbl.CompanySchemeUri;
        }
    }
}