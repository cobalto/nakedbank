using NakedBank.Shared;
using System.Collections.Generic;
using System.Linq;

namespace NakedBank.Domain
{
    public class BaseDomain
    {
        public bool HasErrors => Errors.Any();

        public List<BusinessError> Errors { get; private set; }

        public BaseDomain()
        {
            Errors = new List<BusinessError>();
        }
    }
}
