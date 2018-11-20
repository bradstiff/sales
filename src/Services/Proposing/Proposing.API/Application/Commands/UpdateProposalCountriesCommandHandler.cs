using MediatR;
using Microsoft.EntityFrameworkCore;
using Proposing.API.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands
{
    public class UpdateProposalCountriesCommandHandler : IRequestHandler<CommandWithResourceId<int, UpdateProposalCountriesCommand, bool>, bool>
    {
        private readonly ProposingContext _context;

        public UpdateProposalCountriesCommandHandler(ProposingContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CommandWithResourceId<int, UpdateProposalCountriesCommand, bool> request, CancellationToken cancellationToken)
        {
            var proposal = await _context.Proposals.FindByIdAsync(request.ResourceId, cancellationToken);
            proposal.UpdateCountries(request.InnerCommand.Countries);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
