using Proposing.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.Domain.Model.ProposalAggregate
{
    public abstract class ProductScopeDto
    {
        public IEnumerable<ProductCountryScopeDto> CountryScopes { get; set; }
    }

    public abstract class ProductCountryScopeDto
    {
        public int CountryId { get; set; }
    }
}
