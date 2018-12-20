using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class ProductScopeQueryBase
    {
        public int ProposalId { get; private set; }
        public void Init(int proposalId)
        {
            this.ProposalId = proposalId;
        }
    }
}
