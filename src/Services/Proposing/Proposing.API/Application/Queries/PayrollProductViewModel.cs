using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class PayrollProductViewModel
    {
        public int ProposalId { get; set; }
        public IEnumerable<PayrollProductCountryViewModel> Countries { get; set; }
    }
}
