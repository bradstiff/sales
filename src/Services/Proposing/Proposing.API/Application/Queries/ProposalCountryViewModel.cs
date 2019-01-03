using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Queries
{
    public class ProposalCountryViewModel
    {
        public int Id { get; set; }
        public int ProposalId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public int ProductModelId { get; set; }
        public long ProductIds { get; set; }
        public int? Headcount { get; set; }
    }
}
