using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries.ProductScope
{
    public class PayrollScopeViewModel
    {
        public int ProposalId { get; set; }
        public IEnumerable<PayrollCountryScopeViewModel> Countries { get; set; }
    }
}
