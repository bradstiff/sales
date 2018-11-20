using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands
{
    public class UpdateProposalCountriesCommand : IRequest<bool>
    {
        public UpdateProposalCountriesCommand(List<ProposalCountryDto> countries)
        {
            Countries = countries;
        }

        public List<ProposalCountryDto> Countries { get; set; }
    }
}
