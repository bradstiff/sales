using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate
{
    public interface IProductCountryScope
    {
        int CountryId { get; }
    }
}
