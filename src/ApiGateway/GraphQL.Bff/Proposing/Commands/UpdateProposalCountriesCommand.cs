using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands
{
    public class UpdateProposalCountriesCommand : IRequest<bool>
    {
        public UpdateProposalCountriesCommand(int proposalId, IEnumerable<int> countryIds)
        {
            ProposalId = proposalId;
            CountryIds = countryIds;
        }

        public int ProposalId { get; set; }
        public IEnumerable<int> CountryIds { get; set; }
    }
}
