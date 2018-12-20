using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class ProductCountryScopeQueryBase
    {
        public void Init(int proposalId, int[] countryIds)
        {
            this.ProposalId = proposalId;
            this.CountryIds = countryIds.ToList();
        }

        public int ProposalId { get; private set; }
        public List<int> CountryIds { get; private set; }
    }
}
