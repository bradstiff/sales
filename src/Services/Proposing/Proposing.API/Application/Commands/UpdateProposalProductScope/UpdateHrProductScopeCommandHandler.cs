using MediatR;
using Microsoft.EntityFrameworkCore;
using Proposing.API.Infrastructure.Context;
using Proposing.Domain.Core;
using Proposing.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proposing.API.Application.Commands.UpdateProposalProductScope
{
    public class UpdateHrProductScopeCommandHandler : IRequestHandler<CommandWithResourceId<int, UpdateHrProductScopeCommand, bool>, bool>
    {
        private readonly ProposingContext _context;
        private readonly IMediator _mediator;

        public UpdateHrProductScopeCommandHandler(ProposingContext context, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CommandWithResourceId<int, UpdateHrProductScopeCommand, bool> request, CancellationToken cancellationToken)
        {
            var command = request.InnerCommand;
            var hasLevel = command.LevelId > 0;
            var proposal = await _context.Proposals.FindByIdAsync(request.ResourceId, cancellationToken);

            foreach (var proposalCountry in proposal.ProposalCountries)
            {
                var country = command.CountryScopes.FirstOrDefault(c => c.CountryId == proposalCountry.CountryId);
                if (country != null)
                {
                    proposalCountry.SetHrProductScope(new HrProductCountryScope(command.LevelId));
                    if (hasLevel)
                    {
                        proposalCountry.AddProductType(ProductType.HR);
                    }
                }
                else
                {
                    proposalCountry.SetHrProductScope(new HrProductCountryScope());
                    proposalCountry.RemoveProductType(ProductType.HR);
                }
            }

            proposal.SetHrProductScope(new HrProductGlobalScope(command.LevelId));
            if (hasLevel && proposal.ProposalCountries.Any(c => c.HasProductType(ProductType.HR)))
            {
                proposal.AddProductType(ProductType.HR);
            }
            else
            {
                proposal.RemoveProductType(ProductType.HR);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
