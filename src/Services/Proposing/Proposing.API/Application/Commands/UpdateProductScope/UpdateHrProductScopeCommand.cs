﻿using MediatR;
using Proposing.API.Infrastructure.Context;
using Proposing.API.Domain.Core;
using Proposing.API.Domain.Model.ProposalAggregate;
using Proposing.API.Domain.Model.ProposalAggregate.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Proposing.API.Application.Commands.UpdateProductScope
{
    public class UpdateHrProductScopeCommand : IRequest<bool>
    {
        public UpdateHrProductScopeCommand()
        {
        }

        public UpdateHrProductScopeCommand(List<int> countryIds, short levelId)
        {
            CountryIds = countryIds;
            LevelId = levelId;
        }

        public short LevelId { get; set; }

        public List<int> CountryIds { get; set; }
    }

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

            var productScope = new HrProductScopeDto
            {
                LevelId = command.LevelId,
                CountryScopes = command.CountryIds.Select(id => new HrProductCountryScopeDto
                {
                    CountryId = id
                })
            };

            proposal.SetProductScope(ProductType.HR, productScope);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
