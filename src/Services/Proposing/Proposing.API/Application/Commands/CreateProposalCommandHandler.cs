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
    public class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, int>
    {
        private readonly ProposingContext _context;

        public CreateProposalCommandHandler(ProposingContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
        {
            var proposal = new Proposal(request.Countries);
            proposal.SetGeneralProperties(request.Name, request.ClientName, request.Comments);
            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync(cancellationToken);
            return proposal.Id;
       }

    }
}
