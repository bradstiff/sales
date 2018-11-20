using MediatR;
using Microsoft.EntityFrameworkCore;
using Proposing.API.Infrastructure.Context;
using Proposing.API.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands
{
    public class UpdateProposalCommandHandler : IRequestHandler<UpdateProposalCommand, bool>
    {
        private readonly ProposingContext _context;

        public UpdateProposalCommandHandler(ProposingContext context)
        {
            _context = context;
        }
               
        public async Task<bool> Handle(UpdateProposalCommand request, CancellationToken cancellationToken)
        {
            var proposal = await _context.Proposals.FindByIdAsync(request.ProposalId, cancellationToken);
            proposal.SetGeneralProperties(request.Name, request.ClientName, request.Comments);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
