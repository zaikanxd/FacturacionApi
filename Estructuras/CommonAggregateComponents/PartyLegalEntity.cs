using System;

namespace Estructuras.CommonAggregateComponents
{
    [Serializable]
    public class PartyLegalEntity
    {
        public string RegistrationName { get; set; }
        public RegistrationAddress RegistrationAddress { get; set; }

        public PartyLegalEntity()
        {
            RegistrationAddress = new RegistrationAddress();
        }
    }
}