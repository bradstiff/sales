using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proposing.Domain.Model.ProposalAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proposing.API.Infrastructure.Context
{
    public class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            builder.ToTable("Proposal");
            builder.HasKey(p => p.Id);
            builder.Ignore(p => p.DomainEvents);

            builder.Metadata
                .FindNavigation(nameof(Proposal.ProposalCountries))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
