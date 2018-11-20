using System;
using System.Collections.Generic;
using System.Text;

namespace Proposing.API.Domain.Model.ProposalAggregate.HR
{
    public class HrProductScopeDto: ProductScopeDto
    {
        public short LevelId { get; set; }
    }

    public class HrProductCountryScopeDto: ProductCountryScopeDto
    {
    }
}
