using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class ProductCountryScopeQueryBase<TProductCountryScopeQueryResult> : IRequest<List<TProductCountryScopeQueryResult>>
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
