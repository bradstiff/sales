using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class HrProduct
    {
        public int ProposalId { get; set; }
        public int LevelId { get; set; }
        public IEnumerable<HrProductCountry> Countries { get; set; }
    }
}
