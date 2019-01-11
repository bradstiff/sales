using Proposing.API.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public abstract class ProductScopeDto<TCountryScopeDto> where TCountryScopeDto: ProductCountryScopeDto
    {
        public IEnumerable<TCountryScopeDto> CountryScopes { get; set; }
    }

    public abstract class ProductCountryScopeDto
    {
        public int CountryId { get; set; }
    }
}
