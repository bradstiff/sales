﻿using Microsoft.EntityFrameworkCore;
using Proposing.API.Application.Exceptions;
using Proposing.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public static class ProposalQueries
    {
        public static async Task<Proposal> FindByIdAsync(this IQueryable<Proposal> proposals, int id, CancellationToken cancellationToken)
        {
            var proposal = await proposals
                .Include(p => p.ProposalCountries)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (proposal == null)
            {
                throw new ResourceNotFoundException($"Proposal {id} not found.");
            }
            return proposal;
        }
    }
}
