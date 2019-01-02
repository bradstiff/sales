using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class ProposalViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string Comments { get; set; }
        public IEnumerable<ProposalCountryViewModel> Countries { get; set; }
        public int ProductModelId { get; set; }
        public long ProductIds { get; set; }
    }
}