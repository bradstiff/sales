using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
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
